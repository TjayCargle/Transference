using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ManagerScript : EventRunner
{

    public GameObject Tile;
    public GameObject[] tileMap;
    public List<GridObject> gridObjects;
    public List<LivingObject> turnOrder;
    public List<TileScript> currentAttackList;
    public List<List<TileScript>> attackableTiles;
    public PlayerController player;
    Vector2 selectedAttackingTile = Vector2.up;
    public int MapWidth = 0;
    public int MapHeight = 0;
    public State currentState = State.PlayerInput;
    public State prevState = State.PlayerInput;
    public int currentMenuitem = 0;
    public int itemMenuitem = 0;
    public int turnIndex;
    public CameraScript myCamera;
    public bool isSetup = false;
    public GridObject currentObject;
    public GameObject tempObject;// = new GridObject();
    MenuItem[] commandItems;
    public bool freeCamera = false;
    public int testShow = 0;
    public MenuManager menuManager;
    public InventoryMangager invManager;
    float xDist = 0.0f;
    float yDist = 0.0f;
    public GameObject dmgPrefab;
    public GameObject animPrefab;
    public List<DmgTextObj> dmgText;
    public int dmgRequest = 0;
    public List<GridAnimationObj> animObjs;
    public int AnimationRequests = 0;
    public EventManager eventManager;
    public List<menuStackEntry> menuStack;
    public int menuStackCount = 0;
    public descState descriptionState = descState.stats;
    private int skipCount = 0;
    public SFXManager sfx;
    public ImgObj oppImage;
    public int TwoToOneD(int y, int width, int x)
    {
        return y * width + x;
    }
    public List<LivingObject> currOppList;
    public List<TileScript> doubleAdjOppTiles;
    LivingObject oppObj;
    public int targetIndex = 0;
    public GridEvent oppEvent;
    public List<LivingObject> targets;
    menuStackEntry defaultEntry;
    // Use this for initialization
    public void Setup()
    {
        if (!isSetup)
        {

            menuManager = GetComponent<MenuManager>();
            invManager = GetComponent<InventoryMangager>();
            eventManager = GetComponent<EventManager>();
            sfx = GameObject.FindObjectOfType<SFXManager>();
            eventManager.Setup();
            //dmgText = GameObject.FindObjectOfType<DmgTextObj>();
            //if (dmgText)
            //{
            //    if (!dmgText.isSetup)
            //    {
            //        dmgText.Setup();
            //    }
            //    dmgText.gameObject.SetActive(false);
            //}
            menuStack = new List<menuStackEntry>();
            defaultEntry = new menuStackEntry();
            defaultEntry.state = State.PlayerInput;
            defaultEntry.index = 0;
            oppEvent = new GridEvent();
            menuStack.Add(defaultEntry);
            dmgText = new List<DmgTextObj>();
            targets = new List<LivingObject>();
            tileMap = new GameObject[MapWidth * MapHeight];
            for (int i = 0; i < MapHeight; i++)
            {
                for (int j = 0; j < MapWidth; j++)
                {
                    GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)] = Instantiate(Tile, new Vector3(i, 0, j), Quaternion.identity);
                    temp.AddComponent<TileScript>();
                    temp.GetComponent<TileScript>().Setup();
                }
            }
            if (GameObject.FindObjectOfType<CameraScript>())
            {
                myCamera = GameObject.FindObjectOfType<CameraScript>();
            }
            commandItems = GameObject.FindObjectsOfType<MenuItem>();
            player = GameObject.FindObjectOfType<PlayerController>();
            LivingObject[] livingObjects = GameObject.FindObjectsOfType<LivingObject>();
            for (int i = livingObjects.Length - 1; i >= 0; i--)
            {
                if (livingObjects[i].IsEnenmy)
                {
                    continue;
                }
                turnOrder.Add(livingObjects[i]);
            }
            //turnOrder.Sort();
            currentObject = turnOrder[0];
            // currentObject.transform.position = tileMap[TwoToOneD(MapHeight / 2, MapWidth, MapWidth / 2)].transform.position + new Vector3(0, 0.5f, 0);
            tempObject = new GameObject();
            tempObject.AddComponent<GridObject>();
            tempObject.GetComponent<GridObject>().MOVE_DIST = 10000;
            GridObject[] objs = GameObject.FindObjectsOfType<GridObject>();
            attackableTiles = new List<List<TileScript>>();
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].gameObject == tempObject)
                {
                    continue;
                }
                gridObjects.Add(objs[i]);
                objs[i].currentTile = GetTile(objs[i]);
                objs[i].currentTile.isOccupied = true;
            }
            currentState = State.EnemyTurn;
            NextRound();
            //if (myCamera)
            //{
            //    myCamera.currentTile = currentObject.currentTile;
            //    myCamera.infoObject = currentObject;
            //}
            //GridEvent CamEvent = new GridEvent();
            //CamEvent.RUNABLE = CameraEvent;
            //CamEvent.data = currentObject;
            //CamEvent.caller = this;
            //CamEvent.name = ;
            //eventManager.gridEvents.Add(CamEvent);
            CreateEvent(this, currentObject, "Initial Camera Event", CameraEvent);
            isSetup = true;
        }
    }
    void Start()
    {
        if (!isSetup)
        {
            Setup();

        }
        //MaxAttackDist = MoveDist + 2;
    }

    //DMG * (100/100+DEF)


    public void enterState(menuStackEntry entry)
    {
        prevState = currentState;
        currentState = entry.state;
        menuStack.Add(entry);
        invManager.currentIndex = 0;
        invManager.Validate("manager, enter state");
        if (currentState == State.PlayerMove)
        {
            ShowGridObjectMoveArea(currentObject);
        }
    }
    public void returnState()
    {
        if (menuStack.Count > 1)
        {
            menuStackEntry currEntry = menuStack[menuStack.Count - 1];
            menuStackEntry prevEntry = menuStack[menuStack.Count - 2];
            currentState = prevEntry.state;
            MenuManager menuManager = GetComponent<MenuManager>();

            switch (currEntry.menu)
            {
                case currentMenu.command:
                    {
                        ShowGridObjectAffectArea(currentObject);
                        menuManager.ShowCommandCanvas();
                    }
                    break;
                case currentMenu.invMain:
                    {

                        menuManager.ShowInventoryCanvas();
                    }
                    break;
                case currentMenu.skillsMain:
                    {
                        menuManager.ShowSkillCanvas();
                    }
                    break;
                case currentMenu.CmdSkills:
                    {
                        menuManager.ShowItemCanvas(7, currentObject.GetComponent<LivingObject>());
                    }
                    break;
                case currentMenu.OppSelection:
                    {
                        showOppAdjTiles();
                    }
                    break;
                case currentMenu.OppOptions:
                    {
                        menuManager.ShowItemCanvas(3, currentObject.GetComponent<LivingObject>());
                    }
                    break;
                default:
                    ShowGridObjectAffectArea(currentObject);
                    menuManager.ShowCommandCanvas();
                    currentState = State.PlayerInput;
                    break;
            }
            GetComponent<InventoryMangager>().currentIndex = currEntry.index;
            GetComponent<InventoryMangager>().ForceSelect();
            menuStack.Remove(menuStack[menuStack.Count - 1]);
        }
        else if (menuStack.Count == 1)
        {

        }
        else
        {
            Debug.Log("error tjay, do the stuff");
        }

    }
    public void CleanMenuStack()
    {
        State prevst = currentState;
        while (menuStack.Count > 1)
        {
            //   menuStack.Remove(menuStack[menuStack.Count - 1]);
            menuStackEntry currEntry = menuStack[menuStack.Count - 1];
            menuStackEntry prevEntry = menuStack[menuStack.Count - 2];

            returnState();
        }
        if (currentState != State.PlayerInput)
        {
            menuStack.Remove(menuStack[0]);
            menuStack.Add(defaultEntry);
            ShowGridObjectAffectArea(currentObject);
            menuManager.ShowCommandCanvas();
        }
        if (prevst == State.EnemyTurn)
        {
            currentState = State.EnemyTurn;
        }
        //    currentState = State.PlayerInput;

        attackableTiles.Clear();
        //  menuManager.ShowCommandCanvas();
        currentObject = turnOrder[0];
        ShowGridObjectAffectArea(currentObject);
    }
    public GridEvent CreateEvent(Object caller, Object data, string name, RunableEvent run, StartupEvent start = null, int index = -1, StartupWResourcesEvent startupW = null)
    {
        GridEvent newEvent = new GridEvent();
        newEvent.RUNABLE = run;
        newEvent.data = data;
        newEvent.caller = caller;
        newEvent.name = name;
        newEvent.START = start;
        newEvent.STARTW = startupW;
        if (index >= 0)
        {
            eventManager.gridEvents.Insert(index, newEvent);

        }
        else
        {
            eventManager.gridEvents.Add(newEvent);

        }
        return newEvent;
    }


    void Update()
    {

        menuStackCount = menuStack.Count;
        if (Input.GetKey(KeyCode.N))
        {
            NextRound();
        }
        if (currentObject)
        {
            switch (currentState)
            {
                case State.PlayerInput:

                    break;
                case State.PlayerMove:

                    break;
                case State.PlayerAttacking:
                    {

                        bool hitkey = false;
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            ShowWhite();
                            if (selectedAttackingTile == Vector2.up)
                                skipCount++;
                            else
                            {
                                selectedAttackingTile = Vector2.up;
                                skipCount = 0;
                            }
                            hitkey = true;
                        }
                        if (Input.GetKeyDown(KeyCode.D))
                        {
                            ShowWhite();
                            if (selectedAttackingTile == Vector2.right)
                                skipCount++;
                            else
                            {
                                selectedAttackingTile = Vector2.right;
                                skipCount = 0;
                            }
                            hitkey = true;
                        }
                        if (Input.GetKeyDown(KeyCode.S))
                        {
                            ShowWhite();
                            if (selectedAttackingTile == Vector2.down)
                                skipCount++;
                            else
                            {
                                selectedAttackingTile = Vector2.down;
                                skipCount = 0;
                            }
                            hitkey = true;
                        }
                        if (Input.GetKeyDown(KeyCode.A))
                        {
                            ShowWhite();
                            if (selectedAttackingTile == Vector2.left)
                                skipCount++;
                            else
                            {
                                selectedAttackingTile = Vector2.left;
                                skipCount = 0;
                            }
                            hitkey = true;
                        }
                        if (hitkey == true)
                        {

                            if (attackableTiles.Count > 0)
                            {
                                int realAmt = attackableTiles.Count / 4;
                                if (skipCount >= realAmt)
                                {
                                    skipCount = realAmt - 1;
                                    selectedAttackingTile = selectedAttackingTile * new Vector2(-1, -1);
                                }
                                int loopSkipCount = skipCount;
                                int index = 0;

                                for (int i = 0; i < attackableTiles.Count; i++)
                                {
                                    Vector3 dir = attackableTiles[i][0].transform.position - player.current.transform.position;
                                    Vector2 trueDir = new Vector2(dir.x, dir.z);
                                    trueDir.Normalize();
                                    if (selectedAttackingTile == trueDir)
                                    {
                                        if (loopSkipCount > 0)
                                        {
                                            loopSkipCount--;
                                        }
                                        else
                                        {
                                            index = i;
                                            break;
                                        }
                                    }
                                }
                                showAttackableTiles();

                                currentAttackList = attackableTiles[index];
                                bool foundSomething = false;
                                for (int i = 0; i < currentAttackList.Count; i++)
                                {
                                    if (GetObjectAtTile(currentAttackList[i]) != null)
                                    {
                                        foundSomething = true;
                                        if (SetGridObjectPosition(tempObject.GetComponent<GridObject>(), currentAttackList[i].transform.position) == true)
                                        {
                                            ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));

                                        }

                                    }
                                    currentAttackList[i].myColor = Color.green;
                                }
                                if (foundSomething == false)
                                {
                                    if (SetGridObjectPosition(tempObject.GetComponent<GridObject>(), currentAttackList[0].transform.position) == true)
                                    {
                                        ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));

                                    }

                                }
                                //  ShowSelectedTile(tempObject.GetComponent<GridObject>());
                            }
                        }
                    }
                    break;
                case State.PlayerOppSelecting:
                    {
                        bool hitkey = false;
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            ShowWhite();
                            if (selectedAttackingTile == Vector2.up)
                                skipCount++;
                            else
                            {
                                selectedAttackingTile = Vector2.up;
                                skipCount = 0;
                            }
                            hitkey = true;
                        }
                        if (Input.GetKeyDown(KeyCode.D))
                        {
                            ShowWhite();
                            if (selectedAttackingTile == Vector2.right)
                                skipCount++;
                            else
                            {
                                selectedAttackingTile = Vector2.right;
                                skipCount = 0;
                            }
                            hitkey = true;
                        }
                        if (Input.GetKeyDown(KeyCode.S))
                        {
                            ShowWhite();
                            if (selectedAttackingTile == Vector2.down)
                                skipCount++;
                            else
                            {
                                selectedAttackingTile = Vector2.down;
                                skipCount = 0;
                            }
                            hitkey = true;
                        }
                        if (Input.GetKeyDown(KeyCode.A))
                        {
                            ShowWhite();
                            if (selectedAttackingTile == Vector2.left)
                                skipCount++;
                            else
                            {
                                selectedAttackingTile = Vector2.left;
                                skipCount = 0;
                            }
                            hitkey = true;
                        }
                        if (hitkey == true)
                        {

                            if (doubleAdjOppTiles.Count > 0)
                            {
                                int realAmt = doubleAdjOppTiles.Count;
                                if (skipCount >= realAmt)
                                {
                                    skipCount = realAmt - 1;
                                    selectedAttackingTile = selectedAttackingTile * new Vector2(-1, -1);
                                }
                                int loopSkipCount = skipCount;
                                int index = 0;

                                for (int i = 0; i < doubleAdjOppTiles.Count; i++)
                                {
                                    Vector3 dir = doubleAdjOppTiles[i].transform.position - player.current.transform.position;
                                    Vector2 trueDir = new Vector2(dir.x, dir.z);
                                    trueDir.Normalize();
                                    if (selectedAttackingTile == trueDir)
                                    {
                                        if (loopSkipCount > 0)
                                        {
                                            loopSkipCount--;
                                        }
                                        else
                                        {
                                            index = i;
                                            break;
                                        }
                                    }
                                }
                                targetIndex = index;
                                showOppAdjTiles();
                                TileScript targetTile = doubleAdjOppTiles[targetIndex];
                                if (GetObjectAtTile(targetTile) != null)
                                {
                                    targetTile.myColor = Color.green;

                                }
                                if (SetGridObjectPosition(tempObject.GetComponent<GridObject>(), targetTile.transform.position) == true)
                                {
                                    ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));
                                }



                            }
                        }

                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            TileScript targetTile = doubleAdjOppTiles[targetIndex];
                            if (GetObjectAtTile(targetTile) != null)
                            {
                                oppObj = GetObjectAtTile(targetTile) as LivingObject;
                                targetTile.myColor = Color.green;
                                menuStackEntry entry = new menuStackEntry();
                                entry.state = State.PlayerOppOptions;
                                entry.index = invManager.currentIndex;
                                entry.menu = currentMenu.OppSelection;

                                enterState(entry);
                                menuManager.showOpportunityOptions(oppObj);
                            }
                        }
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            oppEvent.caller = null;
                            CleanMenuStack();
                        }
                    }
                    break;
                case State.PlayerOppOptions:
                    {
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            player.useOppAction(oppObj);
                        }


                    }
                    break;
                case State.PlayerEquipping:

                    break;
                case State.PlayerWait:
                    break;
                case State.FreeCamera:

                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        ShowWhite();
                        MoveGridObject(tempObject.GetComponent<GridObject>(), new Vector3(0, 0, 1));
                        ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));
                        if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                            ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                        ShowSelectedTile(tempObject.GetComponent<GridObject>());
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        ShowWhite();
                        MoveGridObject(tempObject.GetComponent<GridObject>(), new Vector3(-1, 0, 0));
                        ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));
                        if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                            ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                        ShowSelectedTile(tempObject.GetComponent<GridObject>());
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        ShowWhite();
                        MoveGridObject(tempObject.GetComponent<GridObject>(), new Vector3(0, 0, -1));
                        ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));
                        if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                            ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                        ShowSelectedTile(tempObject.GetComponent<GridObject>());
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        ShowWhite();
                        MoveGridObject(tempObject.GetComponent<GridObject>(), new Vector3(1, 0, 0));
                        ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));
                        if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                            ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                        ShowSelectedTile(tempObject.GetComponent<GridObject>());
                    }
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if ((int)descriptionState + 1 <= 5)
                        {
                            descriptionState = (descState)((int)descriptionState + 1);
                        }
                        else
                        {
                            descriptionState = descState.stats;
                        }
                    }
                    break;
                case State.EnemyTurn:
                    break;
                case State.PlayerOppMove:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        MoveGridObject(oppObj, new Vector3(0, 0, 1));
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        MoveGridObject(oppObj, new Vector3(-1, 0, 0));
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        MoveGridObject(oppObj, new Vector3(0, 0, -1));
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        MoveGridObject(oppObj, new Vector3(1, 0, 0));
                    }
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        ComfirmMenuAction(oppObj);
                        oppEvent.caller = null;
                        CleanMenuStack();
                     
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        CancelMenuAction(oppObj);
                        oppEvent.caller = null;
                        CleanMenuStack();
                    }
                    break;
                default:
                    break;
            }

        }

    }
    public void NextRound()
    {

        LivingObject[] livingObjects = GameObject.FindObjectsOfType<LivingObject>();
        if (currentState == State.EnemyTurn)
        {
            currentState = State.PlayerInput;

        }
        else
        {
            currentState = State.EnemyTurn;
            menuManager.ShowNone();
        }
        for (int i = livingObjects.Length - 1; i >= 0; i--)
        {
            if (!turnOrder.Contains(livingObjects[i]))
            {
                if (!livingObjects[i].DEAD)
                {

                    if (currentState == State.EnemyTurn)
                    {
                        if (livingObjects[i].IsEnenmy)
                        {
                            livingObjects[i].ACTIONS = 2;
                            turnOrder.Add(livingObjects[i]);
                        }
                    }
                    else
                    {
                        if (!livingObjects[i].IsEnenmy)
                        {
                            livingObjects[i].ACTIONS = 2;
                            turnOrder.Add(livingObjects[i]);

                        }

                    }
                }
            }

        }

        if (currentState == State.EnemyTurn)
        {
            for (int i = 0; i < turnOrder.Count; i++)
            {
                if (turnOrder[i].GetComponent<EnemyScript>())
                {
                    EnemyScript anEnemy = turnOrder[i].GetComponent<EnemyScript>();
                    anEnemy.DetermineActions();
                }
            }
        }
    }
    public bool CameraEvent(Object data)
    {
        bool result = false;
        GridObject obj = (GridObject)data;

        if (obj.isSetup)
        {
            ShowGridObjectAffectArea(obj);
            if (myCamera)
            {
                currentObject = obj;
                myCamera.currentTile = obj.currentTile;
                myCamera.infoObject = obj;
            }
            result = true;
            if (currentState != State.EnemyTurn)
            {
                menuManager.ShowCommandCanvas();
            }
        }
        return result;
    }
    public void NextTurn(string invokingObj)
    {
        currOppList.Clear();
        doubleAdjOppTiles.Clear();
        Debug.Log(invokingObj + " is done with their turn, moving on ");
        if (currentObject.GetComponent<BuffScript>())
        {
            BuffScript[] buffs = currentObject.GetComponents<BuffScript>();

            for (int i = 0; i < buffs.Length; i++)
            {
                buffs[i].UpdateCount(currentObject.GetComponent<LivingObject>());
            }
        }
        if (turnOrder.Count > 0)
            turnOrder.RemoveAt(0);
        if (turnOrder.Count <= 0)
        {
            NextRound();
        }
        // currentObject = turnOrder[0];

        player.current = turnOrder[0];

        if (currentObject.GetComponent<EffectScript>())
        {
            currentObject.GetComponent<EffectScript>().ApplyReaction(this, currentObject.GetComponent<LivingObject>());
        }
        if (currentObject.GetComponent<SecondStatusScript>())
        {
            currentObject.GetComponent<SecondStatusScript>().ReduceCount(this, currentObject.GetComponent<LivingObject>());
        }


        CreateEvent(this, turnOrder[0], "Initial Camera Event", CameraEvent);

    }
    public void showAttackableTiles()
    {
        for (int i = 0; i < attackableTiles.Count; i++)
        {
            for (int j = 0; j < attackableTiles[i].Count; j++)
            {
                attackableTiles[i][j].myColor = Color.red;
            }
        }
    }
    public void loadTargets()
    {
        targets.Clear();
        for (int i = 0; i < attackableTiles.Count; i++)
        {
            for (int j = 0; j < attackableTiles[i].Count; j++)
            {
                GridObject obj = GetObjectAtTile(attackableTiles[i][j]);
                if (obj)
                {
                    if (obj.GetComponent<LivingObject>())
                    {
                        targets.Add(obj.GetComponent<LivingObject>());
                    }
                }
            }
        }
    }
    public void showOppAdjTiles()
    {
        Color orange = new Color(1.0f, 0.369f, 0.0f);
        for (int i = 0; i < doubleAdjOppTiles.Count; i++)
        {

            doubleAdjOppTiles[i].myColor = orange;

        }
    }
    public void updateCurrentMenuPosition(int currentAnchor)
    {
        for (int i = 0; i < commandItems.Length; i++)
        {
            if (commandItems[i].myRect)
            {


                if (commandItems[i].itemType == currentAnchor)
                {
                    commandItems[i].myRect.anchoredPosition = new Vector2(commandItems[i].myRect.anchoredPosition.x + 10, commandItems[i].myRect.anchoredPosition.y);
                }
                else
                {
                    commandItems[i].myRect.anchoredPosition = new Vector2(100, commandItems[i].myRect.anchoredPosition.y);
                }
            }
        }
    }

    public void ShowGridObjectAffectArea(GridObject obj)
    {

        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)];
                float tempX = temp.transform.position.x;
                float tempY = temp.transform.position.z;

                float objX = obj.currentTile.transform.position.x;
                float objY = obj.currentTile.transform.position.z;

                int MoveDist = 0;
                int attackDist = 0;
                int actionsRemaining = 1;
                if (obj.GetComponent<LivingObject>())
                {
                    LivingObject liveObj = obj.GetComponent<LivingObject>();
                    if (!liveObj.isSetup)
                    {
                        liveObj.Setup();
                    }
                    MoveDist = liveObj.MOVE_DIST;
                    attackDist = liveObj.WEAPON.DIST;
                    actionsRemaining = liveObj.ACTIONS;
                }
                xDist = Mathf.Abs(tempX - objX);
                yDist = Mathf.Abs(tempY - objY);
                if (xDist == 0 && yDist == 0)
                {
                    myCamera.currentTile = temp.GetComponent<TileScript>();
                    myCamera.infoObject = GetObjectAtTile(temp.GetComponent<TileScript>());
                    temp.GetComponent<TileScript>().myColor = Color.white;
                }
                else if (xDist + yDist <= MoveDist)
                {
                    temp.GetComponent<TileScript>().myColor = Color.blue;
                }
                else if (xDist + yDist <= MoveDist + attackDist)
                {
                    temp.GetComponent<TileScript>().myColor = Color.red;
                }
                else
                {
                    temp.GetComponent<TileScript>().myColor = Color.white;

                }
            }
        }
    }
    public void ShowWhite()
    {
        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)];
                temp.GetComponent<TileScript>().myColor = Color.white;
            }
        }

    }
    public void ShowSelectedTile(GridObject obj)
    {
        TileScript theTile = GetTile(obj);
        theTile.myColor = Color.grey;

    }
    public List<TileScript> GetMoveAbleTiles(LivingObject target)
    {
        List<TileScript> returnedTiles = new List<TileScript>();
        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)];
                float tempX = temp.transform.position.x;
                float tempY = temp.transform.position.z;

                float objX = target.currentTile.transform.position.x;
                float objY = target.currentTile.transform.position.z;

                int MoveDist = target.MOVE_DIST;


                xDist = Mathf.Abs(tempX - objX);
                yDist = Mathf.Abs(tempY - objY);
                if (temp.GetComponent<TileScript>())
                {

                    TileScript tempTile = temp.GetComponent<TileScript>();
                    if (GetObjectAtTile(tempTile) == null)
                    {

                        if (tempTile != target.currentTile)
                        {

                            if (xDist + yDist <= MoveDist)
                            {
                                returnedTiles.Add(tempTile);
                            }
                        }
                    }

                }
            }
        }

        return returnedTiles;
    }

    public List<TileScript> GetMoveAbleTiles(Vector3 target, int MOVE_DIST)
    {
        List<TileScript> returnedTiles = new List<TileScript>();
        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)];
                float tempX = temp.transform.position.x;
                float tempY = temp.transform.position.z;

                float objX = target.x;
                float objY = target.z;

                int MoveDist = MOVE_DIST;


                xDist = Mathf.Abs(tempX - objX);
                yDist = Mathf.Abs(tempY - objY);
                if (temp.GetComponent<TileScript>())
                {

                    TileScript tempTile = temp.GetComponent<TileScript>();
                    if (GetObjectAtTile(tempTile) == null)
                    {

                        if (tempTile != GetTileAtIndex(GetTileIndex(target)))
                        {

                            if (xDist + yDist <= MoveDist)
                            {
                                returnedTiles.Add(tempTile);
                            }
                        }
                    }

                }
            }
        }

        return returnedTiles;
    }
    public void ShowGridObjectMoveArea(GridObject obj)
    {
        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)];
                float tempX = temp.transform.position.x;
                float tempY = temp.transform.position.z;

                float objX = obj.currentTile.transform.position.x;
                float objY = obj.currentTile.transform.position.z;

                int MoveDist = obj.MOVE_DIST;


                xDist = Mathf.Abs(tempX - objX);
                yDist = Mathf.Abs(tempY - objY);
                if (xDist == 0 && yDist == 0)
                {
                    myCamera.currentTile = temp.GetComponent<TileScript>();
                    myCamera.infoObject = GetObjectAtTile(temp.GetComponent<TileScript>());
                    temp.GetComponent<TileScript>().myColor = Color.white;
                }
                else if (xDist + yDist <= MoveDist)
                {
                    temp.GetComponent<TileScript>().myColor = Color.blue;
                }
                else
                {
                    temp.GetComponent<TileScript>().myColor = Color.white;
                }
            }
        }
    }
    public void ShowGridObjectAttackArea(GridObject obj)
    {
        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)];
                float tempX = temp.transform.position.x;
                float tempY = temp.transform.position.z;

                float objX = obj.currentTile.transform.position.x;
                float objY = obj.currentTile.transform.position.z;

                int attackDist = 0;

                if (obj.GetComponent<LivingObject>())
                {
                    LivingObject liveObj = obj.GetComponent<LivingObject>();
                    attackDist = liveObj.WEAPON.DIST;
                }

                xDist = Mathf.Abs(tempX - objX);
                yDist = Mathf.Abs(tempY - objY);
                if (xDist == 0 && yDist == 0)
                {
                    myCamera.currentTile = temp.GetComponent<TileScript>();
                    myCamera.infoObject = GetObjectAtTile(temp.GetComponent<TileScript>());
                    temp.GetComponent<TileScript>().myColor = Color.white;
                }
                else if (xDist + yDist <= attackDist)
                {
                    temp.GetComponent<TileScript>().myColor = Color.red;
                }
                else
                {
                    temp.GetComponent<TileScript>().myColor = Color.white;
                }
            }
        }
    }
    public bool SetGridObjectPosition(GridObject obj, Vector3 newLocation)
    {

        Vector3 curPos = GetTile(obj).transform.position;
        int TileIndex = TwoToOneD((int)newLocation.z, MapWidth, (int)newLocation.x);
        if (TileIndex >= MapHeight * MapWidth)
            return false;
        if (TileIndex < 0)
            return false;
        if (obj.GetComponent<LivingObject>())
        {
            if (IsTileOccupied(tileMap[TileIndex].GetComponent<TileScript>()) == true)
            {
                return false;
            }
        }

        obj.transform.position = tileMap[TileIndex].transform.position + new Vector3(0, 0.5f, 0);
        myCamera.currentTile = tileMap[TileIndex].GetComponent<TileScript>();
        myCamera.infoObject = GetObjectAtTile(tileMap[TileIndex].GetComponent<TileScript>());
        myCamera.currentTile = tileMap[TileIndex].GetComponent<TileScript>();
        return true;
    }
    public void MoveGridObject(GridObject obj, Vector3 direction)
    {
        direction.Normalize();
        Vector3 curPos = GetTile(obj).transform.position;
        Vector3 newPos = curPos + direction;
        int TileIndex = TwoToOneD((int)newPos.z, MapWidth, (int)newPos.x);
        if (TileIndex >= MapHeight * MapWidth)
            return;
        if (TileIndex < 0)
            return;
        if (obj.GetComponent<LivingObject>())
        {
            if (IsTileOccupied(tileMap[TileIndex].GetComponent<TileScript>()) == true)
            {
                return;
            }
        }
        GameObject temp = tileMap[TileIndex];
        float tempX = temp.transform.position.x;
        float tempY = temp.transform.position.z;

        float objX = obj.currentTile.transform.position.x;
        float objY = obj.currentTile.transform.position.z;


        xDist = Mathf.Abs(tempX - objX);
        yDist = Mathf.Abs(tempY - objY);
        if (xDist + yDist <= obj.MOVE_DIST)
        {
            obj.transform.position = tileMap[TileIndex].transform.position + new Vector3(0, 0.5f, 0);
            myCamera.currentTile = tileMap[TileIndex].GetComponent<TileScript>();
            myCamera.infoObject = GetObjectAtTile(tileMap[TileIndex].GetComponent<TileScript>());
            myCamera.currentTile = tileMap[TileIndex].GetComponent<TileScript>();
        }
    }
    public void ComfirmMoveGridObject(GridObject obj, int tileIndex)
    {
        if (obj.gameObject != tempObject)
        {
            obj.currentTile.isOccupied = false;
            obj.currentTile = tileMap[tileIndex].GetComponent<TileScript>();
            obj.currentTile.isOccupied = true;
        }
        else
        {
            obj.currentTile = tileMap[tileIndex].GetComponent<TileScript>();
        }
        myCamera.currentTile = tileMap[tileIndex].GetComponent<TileScript>();
        myCamera.infoObject = GetObjectAtTile(tileMap[tileIndex].GetComponent<TileScript>());
    }
    public int GetTileIndex(GridObject checkTile)
    {
        int TileIndex = TwoToOneD((int)checkTile.transform.position.z, MapWidth, (int)checkTile.transform.position.x);
        if (TileIndex >= MapHeight * MapWidth)
            return -1;
        if (TileIndex < 0)
            return -1;
        return TileIndex;
    }
    public int GetTileIndex(Vector3 checkPosition)
    {
        int TileIndex = TwoToOneD((int)checkPosition.z, MapWidth, (int)checkPosition.x);
        if (TileIndex >= MapHeight * MapWidth)
            return -1;
        if (TileIndex < 0)
            return -1;
        return TileIndex;
    }
    public TileScript GetTile(GridObject checkTile)
    {
        int index = GetTileIndex(checkTile);
        if (index < 0)
            return null;
        return tileMap[index].GetComponent<TileScript>();
    }
    public TileScript GetTileAtIndex(int checkIndex)
    {
        if (checkIndex >= MapHeight * MapWidth)
            return null;
        if (checkIndex < 0)
            return null;
        return tileMap[checkIndex].GetComponent<TileScript>();
    }
    public GridObject GetObjectAtTile(TileScript checkTile)
    {
        GridObject returnedObject = null;
        if (checkTile.isOccupied == false)
        {
            return returnedObject;
        }
        else
        {
            for (int i = 0; i < gridObjects.Count; i++)
            {
                if (gridObjects[i])
                {

                    if (gridObjects[i].gameObject == tempObject)
                    {
                        continue;
                    }
                    if (gridObjects[i].currentTile == checkTile)
                    {
                        returnedObject = gridObjects[i];
                        break;
                    }
                }
            }
        }
        return returnedObject;
    }
    public List<TileScript> GetAdjecentTiles(LivingObject origin)
    {
        List<TileScript> tiles = new List<TileScript>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 1;

        v2.x += 1;

        v3.z -= 1;

        v4.x -= 1;
        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = GetTileIndex(possiblePossitions[i]);
            if (index > 0)
            {
                TileScript newTile = GetTileAtIndex(index);
                tiles.Add(newTile);
            }
        }

        return tiles;
    }

    public List<TileScript> GetAdjecentTiles(TileScript origin)
    {
        List<TileScript> tiles = new List<TileScript>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 1;

        v2.x += 1;

        v3.z -= 1;

        v4.x -= 1;
        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = GetTileIndex(possiblePossitions[i]);
            if (index > 0)
            {
                TileScript newTile = GetTileAtIndex(index);
                tiles.Add(newTile);
            }
        }

        return tiles;
    }
    public List<TileScript> GetDoubleAdjecentTiles(LivingObject origin)
    {
        List<TileScript> tiles = new List<TileScript>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;

        v1.z += 1;
        v2.x += 1;
        v3.z -= 1;
        v4.x -= 1;

        v5.x += 2;
        v6.z += 2;
        v7.z -= 2;
        v8.x -= 2;

        v9.x -= 1;
        v9.z += 1;

        v10.x += 1;
        v10.z += 1;

        v11.x -= 1;
        v11.z -= 1;

        v12.x += 1;
        v12.z -= 1;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = GetTileIndex(possiblePossitions[i]);
            if (index > 0)
            {
                TileScript newTile = GetTileAtIndex(index);
                tiles.Add(newTile);
            }
        }
        return tiles;
    }

    public List<TileScript> GetOppViaDoubleAdjecentTiles(LivingObject origin, Element trigger)
    {
        Debug.Log("checking for adj");
        Debug.Log("Trigger Element");
        List<TileScript> tiles = new List<TileScript>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;

        v1.z += 1;
        v2.x += 1;
        v3.z -= 1;
        v4.x -= 1;

        v5.x += 2;
        v6.z += 2;
        v7.z -= 2;
        v8.x -= 2;

        v9.x -= 1;
        v9.z += 1;

        v10.x += 1;
        v10.z += 1;

        v11.x -= 1;
        v11.z -= 1;

        v12.x += 1;
        v12.z -= 1;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = GetTileIndex(possiblePossitions[i]);
            if (index > 0)
            {
                TileScript newTile = GetTileAtIndex(index);
                if (newTile.isOccupied)
                {
                    GridObject obj = GetObjectAtTile(newTile);
                    if (obj.GetComponent<LivingObject>())
                    {
                        LivingObject liveObj = obj.GetComponent<LivingObject>();
                        if (!currOppList.Contains(liveObj))
                        {

                            if (liveObj.OPP_SLOTS.SKILLS.Count > 0)
                            {
                                for (int k = 0; k < liveObj.OPP_SLOTS.SKILLS.Count; k++)
                                {
                                    if ((liveObj.OPP_SLOTS.SKILLS[k] as OppSkill).TRIGGER == trigger)
                                    {
                                        tiles.Add(newTile);

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return tiles;
    }

    public List<List<TileScript>> GetSkillsAttackableTiles(GridObject obj, CommandSkill skill)
    {
        int checkIndex = GetTileIndex(obj);
        if (checkIndex == -1)
            return null;

        List<List<TileScript>> returnList = new List<List<TileScript>>();
        List<Vector2> affectedTiles = skill.TILES;
        Vector2 checkDist = Vector2.zero;
        float dist = 0;
        for (int j = 0; j < affectedTiles.Count; j++)
        {
            if (dist < affectedTiles[j].x)
            {
                dist = affectedTiles[j].x;
            }
            if (dist < affectedTiles[j].y)
            {
                dist = affectedTiles[j].y;
            }
        }
        switch (skill.RTYPE)
        {
            case RanngeType.single:
                for (int i = 0; i < 4; i++)
                {
                    List<TileScript> tiles = new List<TileScript>();
                    if (affectedTiles != null)
                    {
                        for (int j = 0; j < affectedTiles.Count; j++)
                        {
                            Vector2 Dist = affectedTiles[j];
                            switch (i)
                            {
                                case 0:
                                    checkDist.x = Dist.x;
                                    checkDist.y = Dist.y;
                                    break;

                                case 1:
                                    checkDist.x = Dist.y;
                                    checkDist.y = Dist.x * -1;
                                    break;

                                case 2:

                                    checkDist.x = Dist.x * -1;
                                    checkDist.y = Dist.y * -1;

                                    break;

                                case 3:
                                    checkDist.x = Dist.y * -1; //Yes x = y
                                    checkDist.y = Dist.x;
                                    break;
                            }


                            Vector3 checkPos = obj.transform.position;
                            checkPos.x += checkDist.x;
                            checkPos.z += checkDist.y;
                            int testIndex = GetTileIndex(checkPos);
                            TileScript t = GetTileAtIndex(testIndex);
                            float checkX = Mathf.Abs(t.transform.position.x - checkPos.x);
                            float checkY = Mathf.Abs(t.transform.position.z - checkPos.z);
                            if (checkX + checkY <= dist)
                            {
                                if (testIndex >= 0)
                                {
                                    TileScript realTile = GetTileAtIndex(testIndex);
                                    if (!tiles.Contains(realTile))
                                        tiles.Add(realTile);
                                }
                            }
                        }
                    }
                    if (tiles.Count > 0)
                        returnList.Add(tiles);

                }
                break;
            case RanngeType.multi:
                for (int i = 0; i < 4; i++)
                {
                    if (affectedTiles != null)
                    {
                        for (int j = 0; j < affectedTiles.Count; j++)
                        {
                            List<TileScript> tiles = new List<TileScript>();
                            Vector2 Dist = affectedTiles[j];
                            switch (i)
                            {
                                case 0:
                                    checkDist.x = Dist.x;
                                    checkDist.y = Dist.y;
                                    break;

                                case 1:
                                    checkDist.x = Dist.y;
                                    checkDist.y = Dist.x * -1;
                                    break;

                                case 2:

                                    checkDist.x = Dist.x * -1;
                                    checkDist.y = Dist.y * -1;

                                    break;

                                case 3:
                                    checkDist.x = Dist.y * -1; //Yes x = y
                                    checkDist.y = Dist.x;
                                    break;
                            }


                            Vector3 checkPos = obj.transform.position;
                            checkPos.x += checkDist.x;
                            checkPos.z += checkDist.y;
                            int testIndex = GetTileIndex(checkPos);
                            TileScript t = GetTileAtIndex(testIndex);
                            float checkX = Mathf.Abs(t.transform.position.x - checkPos.x);
                            float checkY = Mathf.Abs(t.transform.position.z - checkPos.z);
                            if (checkX + checkY <= dist)
                            {
                                if (testIndex >= 0)
                                {
                                    TileScript realTile = GetTileAtIndex(testIndex);
                                    if (!tiles.Contains(realTile))
                                        tiles.Add(realTile);
                                }
                            }
                            if (tiles.Count > 0)
                                returnList.Add(tiles);
                        }
                    }


                }
                break;
            case RanngeType.area:
                {
                    List<TileScript> tiles = new List<TileScript>();
                    for (int i = 0; i < 4; i++)
                    {
                        if (affectedTiles != null)
                        {
                            for (int j = 0; j < affectedTiles.Count; j++)
                            {
                                Vector2 Dist = affectedTiles[j];
                                switch (i)
                                {
                                    case 0:
                                        checkDist.x = Dist.x;
                                        checkDist.y = Dist.y;
                                        break;

                                    case 1:
                                        checkDist.x = Dist.y;
                                        checkDist.y = Dist.x * -1;
                                        break;

                                    case 2:

                                        checkDist.x = Dist.x * -1;
                                        checkDist.y = Dist.y * -1;

                                        break;

                                    case 3:
                                        checkDist.x = Dist.y * -1; //Yes x = y
                                        checkDist.y = Dist.x;
                                        break;
                                }


                                Vector3 checkPos = obj.transform.position;
                                checkPos.x += checkDist.x;
                                checkPos.z += checkDist.y;
                                int testIndex = GetTileIndex(checkPos);
                                TileScript t = GetTileAtIndex(testIndex);
                                float checkX = Mathf.Abs(t.transform.position.x - checkPos.x);
                                float checkY = Mathf.Abs(t.transform.position.z - checkPos.z);
                                if (checkX + checkY <= dist)
                                {
                                    if (testIndex >= 0)
                                    {
                                        TileScript realTile = GetTileAtIndex(testIndex);
                                        if (!tiles.Contains(realTile))
                                            tiles.Add(realTile);
                                    }
                                }
                            }
                        }

                    }
                    if (tiles.Count > 0)
                        returnList.Add(tiles);
                }
                break;
            case RanngeType.any:
                for (int i = 0; i < 4; i++)
                {
                    List<TileScript> tiles = new List<TileScript>();
                    if (affectedTiles != null)
                    {
                        for (int j = 0; j < affectedTiles.Count; j++)
                        {
                            Vector2 Dist = affectedTiles[j];
                            switch (i)
                            {
                                case 0:
                                    checkDist.x = Dist.x;
                                    checkDist.y = Dist.y;
                                    break;

                                case 1:
                                    checkDist.x = Dist.y;
                                    checkDist.y = Dist.x * -1;
                                    break;

                                case 2:

                                    checkDist.x = Dist.x * -1;
                                    checkDist.y = Dist.y * -1;

                                    break;

                                case 3:
                                    checkDist.x = Dist.y * -1; //Yes x = y
                                    checkDist.y = Dist.x;
                                    break;
                            }


                            Vector3 checkPos = obj.transform.position;
                            checkPos.x += checkDist.x;
                            checkPos.z += checkDist.y;
                            int testIndex = GetTileIndex(checkPos);
                            TileScript t = GetTileAtIndex(testIndex);
                            float checkX = Mathf.Abs(t.transform.position.x - checkPos.x);
                            float checkY = Mathf.Abs(t.transform.position.z - checkPos.z);
                            if (checkX + checkY <= dist)
                            {
                                if (testIndex >= 0)
                                {
                                    TileScript realTile = GetTileAtIndex(testIndex);
                                    if (!tiles.Contains(realTile))
                                        tiles.Add(realTile);
                                }
                            }
                        }
                    }
                    if (tiles.Count > 0)
                        returnList.Add(tiles);
                }
                List<TileScript> mytile = new List<TileScript>();
                mytile.Add(GetTileAtIndex(checkIndex));
                returnList.Add(mytile);
                break;
            default:
                break;
        }

        return returnList;
    }

    public List<int> GetTargetList()
    {
        if (currentAttackList.Count > 0)
        {

            List<int> targetIndicies = new List<int>();

            for (int i = 0; i < currentAttackList.Count; i++)
            {
                if (GetObjectAtTile(currentAttackList[i]) != null)
                {

                    targetIndicies.Add(i);

                }
            }
            return targetIndicies;
        }
        return null;
    }

    public void SetTargetList(List<TileScript> newTargets)
    {

        currentAttackList.Clear();
        for (int i = 0; i < newTargets.Count; i++)
        {
            currentAttackList.Add(newTargets[i]);
        }


    }
    public List<List<TileScript>> GetWeaponAttackableTiles(LivingObject liveObj)
    {
        int checkIndex = GetTileIndex(liveObj);
        if (checkIndex == -1)
            return null;

        List<List<TileScript>> returnList = new List<List<TileScript>>();
        int Dist = liveObj.WEAPON.DIST;
        int Range = liveObj.WEAPON.Range;

        Vector2 checkDist = Vector2.zero;

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    checkDist.x = 0;
                    checkDist.y = 1;
                    break;

                case 1:
                    checkDist.x = 1;
                    checkDist.y = 0;
                    break;

                case 2:
                    checkDist.x = 0;
                    checkDist.y = -1;
                    break;

                case 3:
                    checkDist.x = -1;
                    checkDist.y = 0;
                    break;
            }

            List<TileScript> tiles = new List<TileScript>();
            for (int j = 0; j < Range; j++)
            {
                Vector3 checkPos = liveObj.transform.position;

                checkPos.x += (checkDist.x * Dist);
                checkPos.z += (checkDist.y * Dist);

                checkPos.x -= (checkDist.x * j);
                checkPos.z -= (checkDist.y * j);


                int testIndex = GetTileIndex(checkPos);

                TileScript t = GetTileAtIndex(testIndex);

                float checkX = Mathf.Abs(checkPos.x - liveObj.transform.position.x);
                float checkY = Mathf.Abs(checkPos.z - liveObj.transform.position.z);


                if (checkX + checkY <= Dist)
                {

                    if (testIndex >= 0)
                    {

                        TileScript realTile = GetTileAtIndex(testIndex);
                        if (!tiles.Contains(realTile))
                            tiles.Add(realTile);
                    }
                }
            }
            if (tiles.Count > 0)
            {
                returnList.Add(tiles);
            }
        }
        return returnList;
    }
    public void SelectMenuItem(GridObject invokingObject)
    {
        if (invManager)
        {
            invManager.selectedMenuItem.ApplyAction(invokingObject);
        }

    }
    public void SelectMenuItem(MenuItem selectedItem)
    {
        if (invManager)
        {

            selectedItem.ApplyAction(player.current);
        }

    }
    public void HoverMenuItem(MenuItem selectedItem)
    {
        if (invManager)
        {
            invManager.currentIndex = selectedItem.transform.GetSiblingIndex();
            invManager.ForceSelect();
            invManager.Validate("Manager hover");
        }

    }
    public void ComfirmMenuAction(GridObject invokingObject)
    {
        for (int i = 0; i < commandItems.Length; i++)
        {

            if (commandItems[i].itemType == currentMenuitem)
            {
                commandItems[i].ComfirmAction(invokingObject);
                break;
            }


        }
    }
    public void CancelMenuAction(GridObject invokingObject)
    {
        for (int i = 0; i < commandItems.Length; i++)
        {

            if (commandItems[i].itemType == currentMenuitem)
            {
                commandItems[i].CancelAction(invokingObject);
                break;
            }
        }
    }
    public bool IsTileOccupied(TileScript checkTile)
    {
        bool result = false;
        int tileX = (int)checkTile.transform.position.x;
        int tileY = (int)checkTile.transform.position.y;
        for (int k = 0; k < gridObjects.Count; k++)
        {
            if (gridObjects[k].gameObject == tempObject)
            {
                continue;
            }
            TileScript gridTile = GetTile(gridObjects[k]);
            if (gridTile == checkTile)
            {
                result = true;
                break;
            }
        }

        return result;
    }
    public void DamageLivingObject(LivingObject dmgObject, int dmg)
    {
        dmgObject.STATS.HEALTH -= dmg;

    }
    public DmgReaction CalcDamage(LivingObject attackingObject, LivingObject dmgObject, Element attackingElement, EType attackType, int dmg, Reaction alteration = Reaction.none)
    {
        DmgReaction react = new DmgReaction();

        float mod = 0.0f;
        switch (dmgObject.ARMOR.HITLIST[(int)attackingElement])
        {
            case EHitType.normal:
                mod = 1.0f;
                react.reaction = Reaction.none;

                break;
            case EHitType.resists:
                mod = 0.5f;
                react.reaction = Reaction.none;

                break;
            case EHitType.nulls:
                react.damage = 0;
                react.reaction = Reaction.nulled;

                return react;

            case EHitType.reflects:
                if (dmgObject.ARMOR.HITLIST[(int)attackingElement] != EHitType.reflects)
                {

                    react = CalcDamage(dmgObject, attackingObject, attackingElement, attackType, dmg);
                    react.reaction = Reaction.reflected;
                }
                else
                {
                    Debug.Log("Reflects would cause infinate");
                    react.damage = 0;
                    react.reaction = Reaction.nulled;
                }

                return react;
            case EHitType.absorbs:
                mod = -1.0f;
                react.reaction = Reaction.none;
                break;

            case EHitType.weak:
                mod = 2.0f;
                react.reaction = Reaction.none;
                break;

            case EHitType.knocked:

                react.reaction = Reaction.knockback;
                //todo add lose a action
                break;

            case EHitType.cripples:
                mod = 4.0f;
                react.reaction = Reaction.cripple;
                break;

            case EHitType.leathal:
                mod = 6.0f;
                react.reaction = Reaction.cripple;
                dmgObject.GENERATED = 0;
                //todo add lose a action
                break;
            default:
                break;
        }

        int returnInt = 0;
        float calc = 0.0f;
        float reduction = 1.0f;
        float increasedDmg = 1.0f;
        float resist = 1.0f;
        if (attackType == EType.physical)
        {
            resist = dmgObject.DEFENSE + dmgObject.ARMOR.DEFENSE;
            if (alteration == Reaction.reduceDef)
            {
                resist = resist * 0.5f;
            }
        }
        else
        {
            resist = dmgObject.RESIESTANCE + dmgObject.ARMOR.RESISTANCE;
            if (alteration == Reaction.reduceRes)
            {
                resist = resist * 0.5f;
            }
        }
        if (attackingObject.PSTATUS == PrimaryStatus.tired)
        {
            reduction = 0.8f;

        }
        if (attackingObject.PSTATUS == PrimaryStatus.crippled)
        {
            reduction = 0.5f;
        }

        if (dmgObject.PSTATUS == PrimaryStatus.tired)
        {
            increasedDmg = 1.25f;

        }
        if (dmgObject.PSTATUS == PrimaryStatus.crippled)
        {
            increasedDmg = 2.0f;
        }
        // Debug.Log("Str: " + attackingObject.STRENGTH);
        // Debug.Log("E Def: " + (dmgObject.DEFENSE + dmgObject.ARMOR.DEFENSE));
        // Debug.Log("red: " + reduction);
        // Debug.Log("IncDmg: " + increasedDmg);
        // Debug.Log("Resist: " + resist);

        if (attackType == EType.physical)
        {
            calc = ((float)attackingObject.STRENGTH * reduction * increasedDmg) * (attackingObject.STRENGTH / resist);
        }
        else
        {
            calc = ((float)attackingObject.MAGIC * reduction * increasedDmg) * (attackingObject.MAGIC / resist);
        }
        mod = ApplyDmgMods(attackingObject, mod, attackingElement);
        mod = ApplyDmgMods(dmgObject, mod, attackingElement);
        // Debug.Log("Mod: " + mod);
        calc = calc * mod;
        // Debug.Log("Calc: " + calc);
        // Debug.Log("DMG: " + dmg);
        calc = dmg * calc;
        Debug.Log("Combined: " + calc);
        //   Debug.Log("Calc2:" + calc);
        calc = calc * (attackingObject.LEVEL / dmgObject.LEVEL);
        // Debug.Log("Calc with levels:" + calc);
        calc = Mathf.Sqrt(calc * 2);
        // Debug.Log("Calc4:" + calc);

        // Debug.Log("Calc final: " + calc);
        if (calc < 0)
        {
            calc = 0;
        }
        returnInt = (Mathf.RoundToInt(calc));
        // Debug.Log("FInal:" + returnInt);
        react.damage = returnInt;


        // Debug.Log("returnin count state:" + currentState + " invoker: " + attackingObject.FullName);
        return react;
    }
    public DmgReaction CalcDamage(LivingObject attackingObject, LivingObject dmgObject, CommandSkill skill, Reaction alteration = Reaction.none)
    {
        if (skill.ELEMENT == Element.Buff)
        {
            List<CommandSkill> passives = dmgObject.GetComponent<InventoryScript>().BUFFS;
            if (!passives.Contains(skill))
            {
                bool sameType = false;
                for (int i = 0; i < passives.Count; i++)
                {
                    if (passives[i].BUFFEDSTAT == skill.BUFFEDSTAT && passives[i].BUFFVAL == skill.BUFFVAL)
                    {
                        sameType = true;
                        break;
                    }
                }
                if (sameType == false)
                {
                    dmgObject.GetComponent<InventoryScript>().BUFFS.Add(skill);
                    BuffScript buff = dmgObject.gameObject.AddComponent<BuffScript>();
                    buff.SKILL = skill;
                    buff.BUFF = skill.BUFF;
                    buff.COUNT = 3;
                    dmgObject.ApplyPassives();
                }
            }
            return new DmgReaction() { damage = 0, reaction = Reaction.none };
        }
        else
        {
            DmgReaction react = CalcDamage(attackingObject, dmgObject, skill.ELEMENT, skill.ETYPE, (int)skill.DAMAGE, alteration);
            if (skill.EFFECT != SideEffect.none)
            {
                Debug.Log("Applying effect");
                ApplyEffect(dmgObject, skill.EFFECT);
            }
            return react;
        }
    }

    public DmgReaction CalcDamage(AtkConatiner conatiner)
    {
        if (conatiner.command)
            return CalcDamage(conatiner.attackingObject, conatiner.dmgObject, conatiner.attackingElement, conatiner.attackType, conatiner.dmg, conatiner.alteration);
        else
            return CalcDamage(conatiner.attackingObject, conatiner.dmgObject, conatiner.attackingObject.WEAPON, conatiner.alteration);

    }

    public DmgReaction CalcDamage(LivingObject attackingObject, LivingObject dmgObject, WeaponEquip weapon, Reaction alteration = Reaction.none)
    {
        return CalcDamage(attackingObject, dmgObject, weapon.AFINITY, weapon.ATTACK_TYPE, weapon.ATTACK, alteration);
    }

    public float ApplyDmgMods(LivingObject living, float dmg, Element atkAffinity)
    {
        List<PassiveSkill> passives = living.GetComponent<InventoryScript>().PASSIVES;
        for (int i = 0; i < passives.Count; i++)
        {
            if (passives[i].ModStat == ModifiedStat.ElementDmg)
            {
                for (int k = 0; k < passives[i].ModElements.Count; k++)
                {
                    if (passives[i].ModElements[k] == atkAffinity)
                    {
                        dmg += (float)passives[i].ModValues[k];
                    }
                }
            }
        }
        return dmg;
    }
    public void ApplyReaction(LivingObject attackingObject, LivingObject target, DmgReaction react)
    {
        Debug.Log("Applying dmg: " + react.damage);
        switch (react.reaction)
        {
            case Reaction.none:
                DamageLivingObject(target, react.damage);

                break;
            case Reaction.cripple:
                DamageLivingObject(target, react.damage);
                target.PSTATUS = PrimaryStatus.crippled;
                break;
            case Reaction.nulled:
                DamageLivingObject(target, react.damage);

                break;
            case Reaction.reflected:
                DamageLivingObject(attackingObject, react.damage);
                break;
            case Reaction.knockback:
                DamageLivingObject(target, react.damage);
                Vector3 direction = attackingObject.transform.position - target.transform.position;
                MoveGridObject(target, (-1 * direction));
                ComfirmMoveGridObject(target, GetTileIndex(target));

                break;
            case Reaction.snatched:
                DamageLivingObject(target, react.damage);
                //TODO lose random item
                break;
            default:
                break;
        }
        DmgTextObj dto = null;
        dmgRequest++;
        if (dmgRequest <= dmgText.Count)
        {

            for (int i = 0; i < dmgText.Count; i++)
            {
                if (!dmgText[i].isShowing)
                {
                    dto = dmgText[i];
                    //       dto.gameObject.SetActive(true);
                    dto.text.text = react.damage.ToString();
                    dto.border.text = react.damage.ToString();
                    Vector3 v3 = new Vector3(target.transform.position.x, 1, target.transform.position.z);
                    v3.z -= 0.1f;
                    dto.transform.position = v3;
                    dto.text.color = new Color(Mathf.Min(255, react.damage), 0, 0);
                    //        dto.StartCountDown();
                    break;
                }
            }
        }

        else
        {
            //  Debug.Log("new dmg text");
            GameObject tjObject = Instantiate(dmgPrefab);
            tjObject.SetActive(false);
            if (tjObject != null)
            {
                //  Debug.Log("found  text");
            }
            else
            {
                //  Debug.Log("no text");
            }
            dto = tjObject.GetComponent<DmgTextObj>();
            dmgText.Add(dto);
            dto.manager = this;
            dto.Setup();
            // dto.gameObject.SetActive(true);
            dto.text.text = react.damage.ToString();
            dto.border.text = react.damage.ToString();
            Vector3 v3 = new Vector3(target.transform.position.x, 1, target.transform.position.z);
            v3.z -= 0.1f;
            dto.transform.position = v3;
            dto.text.color = new Color(Mathf.Min(255, react.damage), 0, 0);
            //  dto.StartCountDown();
        }

        GridAnimationObj gao = null;
        AnimationRequests++;
        if (AnimationRequests <= animObjs.Count)
        {

            for (int i = 0; i < animObjs.Count; i++)
            {
                if (!animObjs[i].isShowing)
                {
                    gao = animObjs[i];
                    //gao.gameObject.SetActive(true);
                    Vector3 v3 = new Vector3(target.transform.position.x, 1, target.transform.position.z);
                    v3.z -= 0.1f;
                    gao.transform.position = v3;
                    //  gao.StartCountDown();
                    break;
                }
            }
        }

        else
        {
            GameObject tjObject = Instantiate(animPrefab);
            tjObject.SetActive(false);
            gao = tjObject.GetComponent<GridAnimationObj>();
            animObjs.Add(gao);
            gao.manager = this;
            gao.Setup();
            // gao.gameObject.SetActive(true);
            Vector3 v3 = new Vector3(target.transform.position.x, 1, target.transform.position.z);
            v3.z -= 0.1f;
            gao.transform.position = v3;
            //gao.StartCountDown();
        }

        if (eventManager)
        {

            CreateEvent(this, dto, "DmgText request: " + dmgRequest + "", CheckDmgText, dto.StartCountDown, 0);
            CreateEvent(this, gao, "Animation request: " + AnimationRequests + "", CheckAnimation, gao.StartCountDown, 0);
        }
        if (target.HEALTH <= 0)
        {
            if (turnOrder.Contains(target))
            {
                turnOrder.Remove(target);
            }
            target.DEAD = true;//gameObject.SetActive(false);
            if (target.IsEnenmy)
            {
                target.gameObject.SetActive(false);
                Destroy(target);
            }
            LivingObject[] livingObjects = GameObject.FindObjectsOfType<LivingObject>();
            bool winner = true;
            bool survivor = false;
            for (int i = 0; i < livingObjects.Length; i++)
            {
                LivingObject liver = livingObjects[i];
                if (liver.IsEnenmy)
                {
                    winner = false;
                }
                if (!liver.IsEnenmy)
                {
                    if (!liver.DEAD)
                    {
                        survivor = true;

                    }
                }
            }
            if (winner == true)
            {
                SceneManager.LoadScene("start");
            }
            if (survivor == false)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }
    public bool AttackTargets(LivingObject invokingObject, CommandSkill skill)
    {
        bool hitSomething = false;
        if (currentAttackList.Count > 0)
        {

            List<int> targetIndicies = GetTargetList();

            if (targetIndicies != null)
            {
                if (targetIndicies.Count > 0)
                {
                    hitSomething = true;

                    if (skill != null)
                    {
                        for (int i = 0; i < targetIndicies.Count; i++)
                        {
                            GridObject potentialTarget = GetObjectAtTile(currentAttackList[targetIndicies[i]]);
                            if (potentialTarget.GetComponent<LivingObject>())
                            {
                                LivingObject target = potentialTarget.GetComponent<LivingObject>();

                                //DmgReaction react;

                                for (int k = 0; k < skill.HITS; k++)
                                {
                                    // react = CalcDamage(invokingObject, target, skill);
                                    // ApplyReaction(invokingObject, target, react);
                                    AtkConatiner conatiner = new AtkConatiner();
                                    conatiner.alteration = skill.REACTION;
                                    conatiner.attackingElement = skill.ELEMENT;
                                    conatiner.attackType = skill.ETYPE;
                                    conatiner.attackingObject = invokingObject;
                                    conatiner.command = skill;
                                    conatiner.dmg = (int)skill.DAMAGE;
                                    conatiner.dmgObject = target;
                                    CreateEvent(this, conatiner, "Skill use event", AttackEvent);
                                }

                            }


                        }
                        float modification = 1.0f;
                        if (skill.ETYPE == EType.magical)
                            modification = invokingObject.STATS.SPCHANGE;
                        if (skill.ETYPE == EType.physical)
                            modification = invokingObject.STATS.FTCHANGE;
                        skill.UseSkill(invokingObject, modification);
                    }
                }

            }
        }
        return hitSomething;
    }

    public bool AttackTargets(LivingObject invokingObject, WeaponEquip weapon)
    {
        bool hitSomething = false;
        if (currentAttackList.Count > 0)
        {

            List<int> targetIndicies = GetTargetList();

            if (targetIndicies != null)
            {
                if (targetIndicies.Count > 0)
                {

                    hitSomething = true;
                    Debug.Log("Targets: " + targetIndicies.Count);

                    for (int i = 0; i < targetIndicies.Count; i++)
                    {
                        GridObject potentialTarget = GetObjectAtTile(currentAttackList[targetIndicies[i]]);
                        if (potentialTarget.GetComponent<LivingObject>())
                        {
                            LivingObject target = potentialTarget.GetComponent<LivingObject>();

                            DmgReaction react;
                            Reaction atkReaction = Reaction.none;
                            if (weapon != null)
                            {
                                for (int k = 0; k < invokingObject.AUTO_SLOTS.SKILLS.Count; k++)
                                {
                                    if ((invokingObject.AUTO_SLOTS.SKILLS[k] as AutoSkill).ACT == AutoAct.beforeDmg)
                                    {
                                        AutoSkill auto = (invokingObject.AUTO_SLOTS.SKILLS[k] as AutoSkill);
                                        float chance = auto.CHANCE;
                                        float result = Random.value * 100;
                                        if (chance < result)
                                        {
                                            Debug.Log("Auto skill : " + auto.NAME + "has gone off");
                                            atkReaction = auto.Activate(0);
                                            break;
                                        }
                                    }
                                }
                                AtkConatiner conatiner = new AtkConatiner();
                                conatiner.alteration = atkReaction;
                                conatiner.attackingElement = weapon.AFINITY;
                                conatiner.attackType = weapon.ATTACK_TYPE;
                                conatiner.attackingObject = invokingObject;
                                conatiner.command = null;
                                conatiner.dmg = weapon.ATTACK;
                                conatiner.dmgObject = target;
                                CreateEvent(this, conatiner, "Skill use event", WeaponAttackEvent);

                            }


                        }
                    }
                }


            }
        }
        return hitSomething;
    }

    public void ApplyEffect(LivingObject target, SideEffect effect)
    {
        switch (effect)
        {
            case SideEffect.slow:
                {
                    if (target.SSTATUS == SecondaryStatus.normal)
                    {

                        target.SSTATUS = SecondaryStatus.slow;
                        SecondStatusScript sf = target.gameObject.AddComponent<SecondStatusScript>();
                        sf.COUNTDOWN = 3;
                        sf.STATUS = SecondaryStatus.slow;
                        sf.Activate();
                    }
                }
                break;
            case SideEffect.rage:
                {
                    if (target.SSTATUS == SecondaryStatus.normal)
                    {

                        target.SSTATUS = SecondaryStatus.rage;
                        SecondStatusScript sf = target.gameObject.AddComponent<SecondStatusScript>();
                        sf.COUNTDOWN = 3;
                        sf.STATUS = SecondaryStatus.rage;
                    }
                }
                break;
            case SideEffect.charm:
                {
                    if (target.SSTATUS == SecondaryStatus.normal)
                    {

                        target.SSTATUS = SecondaryStatus.charm;
                        SecondStatusScript sf = target.gameObject.AddComponent<SecondStatusScript>();
                        sf.COUNTDOWN = 3;
                        sf.STATUS = SecondaryStatus.charm;
                    }
                }
                break;
            case SideEffect.seal:
                {
                    if (target.SSTATUS == SecondaryStatus.normal)
                    {

                        target.SSTATUS = SecondaryStatus.seal;
                        SecondStatusScript sf = target.gameObject.AddComponent<SecondStatusScript>();
                        sf.COUNTDOWN = 3;
                        sf.STATUS = SecondaryStatus.seal;
                    }
                }
                break;
            case SideEffect.poison:
                {
                    if (target.SSTATUS == SecondaryStatus.normal)
                    {

                        target.SSTATUS = SecondaryStatus.poisoned;
                        SecondStatusScript sf = target.gameObject.AddComponent<SecondStatusScript>();
                        sf.COUNTDOWN = 3;
                        sf.STATUS = SecondaryStatus.poisoned;
                    }
                }
                break;
            case SideEffect.confusion:
                {
                    if (target.SSTATUS == SecondaryStatus.normal)
                    {

                        target.SSTATUS = SecondaryStatus.confusion;
                        SecondStatusScript sf = target.gameObject.AddComponent<SecondStatusScript>();
                        sf.COUNTDOWN = 3;
                        sf.STATUS = SecondaryStatus.confusion;
                    }
                }
                break;
            case SideEffect.paralyze:
                {
                    if (!target.GetComponent<EffectScript>())
                    {

                        EffectScript ef = target.gameObject.AddComponent<EffectScript>();
                        ef.EFFECT = StatusEffect.paralyzed;
                    }
                }
                break;
            case SideEffect.sleep:
                {
                    if (!target.GetComponent<EffectScript>())
                    {

                        EffectScript ef = target.gameObject.AddComponent<EffectScript>();
                        ef.EFFECT = StatusEffect.sleep;
                    }
                }
                break;
            case SideEffect.freeze:
                {
                    if (!target.GetComponent<EffectScript>())
                    {

                        EffectScript ef = target.gameObject.AddComponent<EffectScript>();
                        ef.EFFECT = StatusEffect.frozen;
                    }
                }
                break;
            case SideEffect.burn:
                {
                    if (!target.GetComponent<EffectScript>())
                    {

                        EffectScript ef = target.gameObject.AddComponent<EffectScript>();
                        ef.EFFECT = StatusEffect.burned;
                    }
                }
                break;
        }
    }
    public bool AttackEvent(Object data)
    {
        loadTargets();
        AtkConatiner container = data as AtkConatiner;
        DmgReaction react = CalcDamage(container);
        ApplyReaction(container.attackingObject, container.dmgObject, react);
        if (react.reaction < Reaction.nulled)
        {
            if (!currOppList.Contains(container.attackingObject))
            {
                currOppList.Add(container.attackingObject);
                doubleAdjOppTiles = GetOppViaDoubleAdjecentTiles(container.attackingObject, container.attackingElement);
                if (doubleAdjOppTiles.Count > 0)
                {
                    for (int i = 0; i < doubleAdjOppTiles.Count; i++)
                    {

                        CreateEvent(this, GetObjectAtTile(doubleAdjOppTiles[i]) as LivingObject, "Opp Announcement", OppAnnounceEvent, null, -1, OppAnnounceStart);
                    }
                    oppEvent = CreateEvent(this, null, "Opp Event", CheckOppEvent, OppStart);
                }
            }
        }
        return true;
    }
    public bool WeaponAttackEvent(Object data)
    {
        loadTargets();
        AtkConatiner container = data as AtkConatiner;
        DmgReaction react = CalcDamage(container);
        LivingObject invokingObject = container.attackingObject;
        for (int k = 0; k < invokingObject.AUTO_SLOTS.SKILLS.Count; k++)
        {
            if ((invokingObject.AUTO_SLOTS.SKILLS[k] as AutoSkill).ACT == AutoAct.afterDmg)
            {
                AutoSkill auto = (invokingObject.AUTO_SLOTS.SKILLS[k] as AutoSkill);
                float chance = auto.CHANCE;
                float result = Random.value * 100;
                if (chance > result)
                {
                    Debug.Log("Auto skill : " + auto.NAME + "has gone off");
                    Debug.Log("chance= " + auto.CHANCE);
                    Debug.Log("result= " + result);
                    auto.Activate(react.damage);
                }
            }
        }
        ApplyReaction(container.attackingObject, container.dmgObject, react);
        if (react.reaction < Reaction.nulled)
        {
            if (!currOppList.Contains(container.attackingObject))
            {
                currOppList.Add(container.attackingObject);
                doubleAdjOppTiles = GetOppViaDoubleAdjecentTiles(container.attackingObject, container.attackingElement);
                if (doubleAdjOppTiles.Count > 0)
                {
                    for (int i = 0; i < doubleAdjOppTiles.Count; i++)
                    {

                        CreateEvent(this, GetObjectAtTile(doubleAdjOppTiles[i]) as LivingObject, "Opp Announcement", OppAnnounceEvent, null, -1, OppAnnounceStart);
                    }
                    Debug.Log("found opp skill user");
                    oppEvent = CreateEvent(this, null, "Opp Event", CheckOppEvent, OppStart);
                }
            }
        }
        return true;
    }

    public bool CheckAnimation(Object data)
    {
        GridAnimationObj gao = data as GridAnimationObj;


        if (gao.isShowing)
        {
            return false;
        }
        else
        {

            return true;
        }
    }

    public void OppStart()
    {
        menuStackEntry entry = new menuStackEntry();
        entry.state = State.PlayerOppSelecting;
        entry.menu = currentMenu.command;
        enterState(entry);
        menuManager.ShowNone();
        showOppAdjTiles();
    }
    public bool CheckDmgText(Object data)
    {
        DmgTextObj dto = data as DmgTextObj;
        return !dto.isShowing;
    }
    public void OppAnnounceStart(Object data)
    {
        LivingObject invokingObj = data as LivingObject;


        AudioClip audio = Resources.LoadAll<AudioClip>(invokingObj.FullName + "/Opp/")[0];
        if (audio)
        {
            Debug.Log("found audio");
        }
        sfx.loadAudio(audio);
        sfx.playSound();

        if (oppImage)
        {
            oppImage.gameObject.SetActive(true);
            oppImage.myImage.sprite = Resources.LoadAll<Sprite>(invokingObj.FullName + "/Opp/")[0];
            oppImage.StartCountDown(audio.length);
        }
    }
    public bool OppAnnounceEvent(Object data)
    {

        return !sfx.SOURCE.isPlaying;
    }

    public bool CheckOppEvent(Object data)
    {

        if (oppEvent.caller)
        {
            return false;

        }
        return true;
    }
    public bool NextTurnEvent(Object data)
    {
        return false;
    }
}
