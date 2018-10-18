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
    public NewSkillPrompt prompt;
    public AudioClip[] sfxClips;

    public int TwoToOneD(int y, int width, int x)
    {
        return y * width + x;
    }
    public List<LivingObject> currOppList;
    public List<TileScript> doubleAdjOppTiles;
    public LivingObject oppObj;
    public int targetIndex = 0;
    public GridEvent oppEvent;
    public List<LivingObject> targets;
    menuStackEntry defaultEntry;
    public FlavorTextImg flavor;
    TextEventManager textManager;
    Expbar expbar;
    public OptionsManager options;
    MenuStackManager stackManager;
    public DetailsScreen detailsScreen;
    public GridEvent newSkillEvent;

    // Use this for initialization
    public void Setup()
    {
        if (!isSetup)
        {
            prompt = GameObject.FindObjectOfType<NewSkillPrompt>();
            menuManager = GetComponent<MenuManager>();
            invManager = GetComponent<InventoryMangager>();
            eventManager = GetComponent<EventManager>();
            textManager = GetComponent<TextEventManager>();

            options = GameObject.FindObjectOfType<OptionsManager>();
            if (options)
                options.gameObject.SetActive(false);
            detailsScreen = GameObject.FindObjectOfType<DetailsScreen>();
            if (detailsScreen)
                detailsScreen.gameObject.SetActive(false);
            sfx = GameObject.FindObjectOfType<SFXManager>();
            expbar = GameObject.FindObjectOfType<Expbar>();
            //flavor = FindObjectOfType<FlavorTextImg>();
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
            newSkillEvent = new GridEvent();
            // menuStack.Add(defaultEntry);
            dmgText = new List<DmgTextObj>();
            targets = new List<LivingObject>();
            GameObject tileParent = new GameObject();
            tileMap = new GameObject[MapWidth * MapHeight];

            stackManager = GameObject.FindObjectOfType<MenuStackManager>();
            if (!stackManager)
            {
                stackManager = gameObject.AddComponent<MenuStackManager>();
            }
            for (int i = 0; i < MapHeight; i++)
            {
                for (int j = 0; j < MapWidth; j++)
                {
                    GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)] = Instantiate(Tile, new Vector3(i, 0, j), Quaternion.identity);
                    temp.transform.parent = tileParent.transform;
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

                if (livingObjects[i].GetComponent<EnemyScript>() || livingObjects[i].GetComponent<HazardScript>())
                {
                    continue;
                }
                turnOrder.Add(livingObjects[i]);
            }
            //turnOrder.Sort();
            if (turnOrder.Count > 0)
                currentObject = turnOrder[0];
            // currentObject.transform.position = tileMap[TwoToOneD(MapHeight / 2, MapWidth, MapWidth / 2)].transform.position + new Vector3(0, 0.5f, 0);
            tempObject = new GameObject();
            tempObject.AddComponent<TempObject>();
            tempObject.GetComponent<GridObject>().MOVE_DIST = 10000;
            tempObject.transform.position = Vector3.zero;
            tempObject.GetComponent<GridObject>().currentTile = GetTileAtIndex(GetTileIndex(Vector3.zero));
            GridObject[] objs = GameObject.FindObjectsOfType<GridObject>();
            attackableTiles = new List<List<TileScript>>();
            ShowWhite();
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
            currentState = State.FreeCamera;
            // Invoke("NextRoundBegin", 0.1f);
            //if (myCamera)
            //  NextRound();
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

            // CreateEvent(this, currentObject, "Initial Camera Event", CameraEvent);
            ShowGridObjectAffectArea(tempObject.GetComponent<GridObject>(), true);
            menuManager.ShowNone();
            ShowSelectedTile(tempObject.GetComponent<GridObject>());
            for (int i = 0; i < turnOrder.Count; i++)
            {
                ShowSelectedTile(turnOrder[i], Common.orange);

            }
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
        if (currentState == State.PlayerAttacking || currentState == State.PlayerTransition)
        {
            if (player.current)
            {
                SpriteRenderer sr = player.current.GetComponent<SpriteRenderer>();
                if (sr)
                {
                    sr.color = Color.white;
                }
            }
        }
        if (sfx)
        {
            if (sfxClips.Length > 1)
            {
                sfx.loadAudio(sfxClips[2]);
                sfx.playSound();
            }
        }
        prevState = currentState;
        currentState = entry.state;

        menuStack.Add(entry);
        invManager.currentIndex = 0;
        invManager.Validate("manager, enter state");
        if (currentState == State.PlayerMove)
        {
            ShowGridObjectMoveArea(currentObject);
        }
        if (currentState == State.PlayerEquipping)
        {
            descriptionState = descState.stats;
        }
        if (currentState == State.PlayerAttacking)
        {
            if (player.current)
            {
                SpriteRenderer sr = player.current.GetComponent<SpriteRenderer>();
                if (sr)
                {
                    sr.color = Common.semi;
                }
            }
        }
        myCamera.UpdateCamera();
    }
    public void PlayExitSnd()
    {
        if (sfx)
        {
            if (sfxClips.Length > 0)
            {
                sfx.loadAudio(sfxClips[0]);
                sfx.playSound();
            }
        }
    }

    public void PlaySquishSnd()
    {
        if (sfx)
        {
            if (sfxClips.Length > 0)
            {
                sfx.loadAudio(sfxClips[3]);
                sfx.playSound();
            }
        }
    }
    public void returnState()
    {
        // Debug.Log("returnin");
        if (currentState == State.PlayerAttacking || currentState == State.PlayerTransition || currentState == State.PlayerEquipping)
        {
            if (player.current)
            {
                SpriteRenderer sr = player.current.GetComponent<SpriteRenderer>();
                if (sr)
                {
                    sr.color = Color.white;
                }
            }
        }
        PlayExitSnd();
        if (menuStack.Count > 1)
        {
            menuStackEntry currEntry = menuStack[menuStack.Count - 1];
            menuStackEntry prevEntry = menuStack[menuStack.Count - 2];
            currentState = prevEntry.state;
            MenuManager menuManager = GetComponent<MenuManager>();
            // Debug.Log("curr :" + currEntry.menu);
            // Debug.Log("prev :" + prevEntry.menu);
            switch (currEntry.menu)
            {
                case currentMenu.command:
                    {

                        currentState = State.PlayerInput;
                        menuManager.ShowCommandCanvas();
                        ShowGridObjectAffectArea(currentObject);
                    }
                    break;
                case currentMenu.act:
                    {
                        currentState = State.PlayerInput;
                        ShowGridObjectAffectArea(currentObject);
                        menuManager.ShowActCanvas();
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
                        myCamera.infoObject = player.current;
                        myCamera.showActions = true;
                        myCamera.currentTile = player.current.currentTile;
                        myCamera.UpdateCamera();
                    }
                    break;
                case currentMenu.OppSelection:
                    {
                        menuManager.ShowNone();
                        showOppAdjTiles();
                    }
                    break;
                case currentMenu.OppOptions:
                    {
                        menuManager.ShowItemCanvas(3, currentObject.GetComponent<LivingObject>());
                    }
                    break;
                case currentMenu.PlayerOptions:
                    {
                        menuManager.DontShowOptions();
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
            if (currentObject)
            {
                if(currentObject == myCamera.infoObject)
                {

                tempObject.transform.position = currentObject.transform.position;
                tempObject.GetComponent<GridObject>().currentTile = currentObject.currentTile;
                }
            }
            player.current = null;
            currentState = State.FreeCamera;
            menuManager.ShowNone();
            menuStack.Remove(menuStack[0]);
        }
        else
        {

            currentState = State.ChangeOptions;
            menuManager.ShowOptions();
        }

        if (doubleAdjOppTiles.Count <= 0)
        {
            if (currentState == State.PlayerOppSelecting)
            {
                CleanMenuStack();
            }
        }
        myCamera.UpdateCamera();
        newSkillEvent.caller = null;
    }
    public void CleanMenuStack(bool toCam = false, bool checkForNext = true)
    {

        if (toCam)
        {

            while (menuStack.Count > 0)
            {
                returnState();
            }
        }
        else
        {

            while (menuStack.Count > 2)
            {
                returnState();
            }
        }

        if (checkForNext)
        {

            bool nextturn = true;
            for (int i = 0; i < turnOrder.Count; i++)
            {
                if (turnOrder[i].ACTIONS > 0)
                {
                    nextturn = false;
                    break;
                }
            }
            if (nextturn == true)
            {
                //NextTurn("manager");
            }
        }
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
            if (index < eventManager.gridEvents.Count)
            {
                eventManager.gridEvents.Insert(index, newEvent);

            }
            else
            {
                eventManager.gridEvents.Insert(0, newEvent);

            }

        }
        else
        {
            eventManager.gridEvents.Add(newEvent);

        }
        return newEvent;
    }

    public TextEvent CreateTextEvent(Object caller, string data, string name, RunableEvent run, StartupEvent start = null, int index = -1)
    {
        TextEvent newEvent = new TextEvent();
        if (options)
        {
            if (options.displayMessages == true)
            {


                newEvent.RUNABLE = run;
                newEvent.data = data;
                newEvent.caller = caller;
                newEvent.name = name;
                newEvent.START = start;

                if (index >= 0)
                {
                    textManager.textEvents.Insert(index, newEvent);

                }
                else
                {
                    textManager.textEvents.Add(newEvent);

                }
            }
        }
        return newEvent;

    }

    public void MovetoMousePos(LivingObject movedObj)
    {

        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 w = hit.point;
            w.x = Mathf.Round(w.x);
            w.y = Mathf.Round(w.y);
            w.z = Mathf.Round(w.z);

            if (GetTileIndex(w) >= 0)
            {
                TileScript hitTile = GetTileAtIndex(GetTileIndex(w));
                bool alreadySelected = false;
                if (!hitTile.isOccupied)
                {

                    if (tempObject.GetComponent<GridObject>().currentTile == hitTile)
                    {
                        alreadySelected = true;

                    }

                    if (alreadySelected)
                    {
                        if (ComfirmMenuAction(movedObj))
                        {

                            if (currentState != State.PlayerOppMove && currentState != State.PlayerOppOptions)
                            {
                                player.current.TakeAction();
                                CleanMenuStack();
                            }
                            else
                            {
                                ComfirmMenuAction(oppObj);
                                oppEvent.caller = null;

                                CreateEvent(this, null, "return state event", BufferedReturnEvent);
                                CreateEvent(this, null, "return state event", BufferedReturnEvent);
                                CreateEvent(this, null, "return state event", BufferedReturnEvent);
                            }
                        }
                        else
                        {
                            Debug.Log("ERROR, Could not find menu item");
                            return;
                        }
                    }
                    else
                    {
                        tempObject.transform.position = hitTile.transform.position;
                        tempObject.GetComponent<GridObject>().currentTile = hitTile;
                        float tempX = hitTile.transform.position.x;
                        float tempY = hitTile.transform.position.z;

                        float objX = movedObj.currentTile.transform.position.x;
                        float objY = movedObj.currentTile.transform.position.z;


                        xDist = Mathf.Abs(tempX - objX);
                        yDist = Mathf.Abs(tempY - objY);
                        if (xDist + yDist <= player.current.MOVE_DIST)
                        {
                            movedObj.transform.position = hitTile.transform.position + new Vector3(0, 0.5f);
                            myCamera.currentTile = hitTile;
                        }




                    }
                }
            }
        }
        myCamera.UpdateCamera();
    }
    void LateUpdate()
    {
        if (menuStack != null)
            menuStackCount = menuStack.Count;
        if (Input.GetKeyDown(KeyCode.V))
        {
            ShowWhite();
        }
        // if (currentObject)
        {
            switch (currentState)
            {
                case State.PlayerInput:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        returnState();
                    }
                    if (Input.GetMouseButtonDown(1))
                    {
                        returnState();
                    }
                    break;


                case State.PlayerMove:
                    if (Input.GetMouseButtonDown(1))
                    {
                        CancelMenuAction(player.current);
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        MovetoMousePos(player.current);
                    }
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
                                if (selectedAttackingTile == Vector2.zero)
                                    selectedAttackingTile = Vector2.up;
                                else
                                    selectedAttackingTile = Vector2.zero;
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
                                if (selectedAttackingTile == Vector2.zero)
                                    selectedAttackingTile = Vector2.right;
                                else
                                    selectedAttackingTile = Vector2.zero;
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
                                if (selectedAttackingTile == Vector2.zero)
                                    selectedAttackingTile = Vector2.down;
                                else
                                    selectedAttackingTile = Vector2.zero;
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
                                if (selectedAttackingTile == Vector2.zero)
                                    selectedAttackingTile = Vector2.left;
                                else
                                    selectedAttackingTile = Vector2.zero;

                                skipCount = 0;
                            }
                            hitkey = true;
                        }
                        if (Input.GetMouseButtonDown(0))
                        {
                            RaycastHit hit = new RaycastHit();
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                            if (Physics.Raycast(ray, out hit))
                            {
                                Vector3 w = hit.point;
                                w.x = Mathf.Round(w.x);
                                w.y = Mathf.Round(w.y);
                                w.z = Mathf.Round(w.z);

                                if (GetTileIndex(w) >= 0)
                                {
                                    TileScript hitTile = GetTileAtIndex(GetTileIndex(w));
                                    bool alreadySelected = false;
                                    int outer = -1;
                                    int innner = -1;
                                    for (int i = 0; i < attackableTiles.Count; i++)
                                    {


                                        for (int j = 0; j < attackableTiles[i].Count; j++)
                                        {

                                            if (attackableTiles[i][j] == hitTile)
                                            {
                                                outer = i;
                                                innner = j;
                                                currentAttackList = attackableTiles[i];
                                                if (tempObject.GetComponent<GridObject>().currentTile == hitTile)
                                                {
                                                    alreadySelected = true;
                                                    break;

                                                }

                                            }
                                        }

                                    }
                                    if (alreadySelected)
                                    {

                                        player.UseOrAttack();
                                    }
                                    else
                                    {
                                        if (outer >= 0 && innner >= 0)
                                        {

                                            showAttackableTiles();
                                            tempObject.transform.position = hitTile.transform.position;
                                            ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), hitTile);
                                            if (player.currentSkill)
                                            {
                                                if (player.currentSkill.ELEMENT == Element.Buff)
                                                {
                                                    //  hitTile.myColor = Common.green;
                                                    for (int i = 0; i < currentAttackList.Count; i++)
                                                    {
                                                        currentAttackList[i].myColor = Common.green; ;
                                                    }
                                                }
                                                else
                                                {
                                                    // hitTile.myColor =  Common.red;
                                                    for (int i = 0; i < currentAttackList.Count; i++)
                                                    {
                                                        currentAttackList[i].myColor = Common.red; ;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //  hitTile.myColor = Common.red;
                                                for (int i = 0; i < currentAttackList.Count; i++)
                                                {
                                                    currentAttackList[i].myColor = Common.red; ;
                                                }
                                            }

                                        }
                                        else
                                        {
                                            //   Debug.Log("no outer : " + outer + " or inner: " + innner);
                                        }

                                    }
                                }
                            }
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
                                    if (player.currentSkill)
                                    {
                                        if (player.currentSkill.ELEMENT == Element.Buff)
                                        {
                                            currentAttackList[i].myColor = Common.green;
                                        }
                                        else
                                        {
                                            currentAttackList[i].myColor = Common.red;
                                        }
                                    }
                                    else
                                    {
                                        currentAttackList[i].myColor = Common.red;

                                    }
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
                    if (Input.GetMouseButtonDown(1))
                    {
                        CancelMenuAction(player.current);
                        player.currentSkill = null;
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
                        if (Input.GetMouseButtonDown(0))
                        {
                            RaycastHit hit = new RaycastHit();
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            if (Physics.Raycast(ray, out hit))
                            {
                                Vector3 w = hit.point;
                                w.x = Mathf.Round(w.x);
                                w.y = Mathf.Round(w.y);
                                w.z = Mathf.Round(w.z);

                                if (GetTileIndex(w) >= 0)
                                {
                                    TileScript hitTile = GetTileAtIndex(GetTileIndex(w));
                                    bool alreadySelected = false;
                                    int inex = -1;

                                    for (int i = 0; i < doubleAdjOppTiles.Count; i++)
                                    {

                                        if (doubleAdjOppTiles[i] == hitTile)
                                        {
                                            inex = i;

                                            if (tempObject.GetComponent<GridObject>().currentTile == hitTile)
                                            {
                                                alreadySelected = true;
                                                break;

                                            }

                                        }


                                    }
                                    if (alreadySelected)
                                    {
                                        oppObj = GetObjectAtTile(hitTile) as LivingObject;
                                        hitTile.myColor = Color.red;
                                        menuStackEntry entry = new menuStackEntry();
                                        entry.state = State.PlayerOppOptions;
                                        entry.index = invManager.currentIndex;
                                        entry.menu = currentMenu.OppSelection;

                                        enterState(entry);
                                        menuManager.showOpportunityOptions(oppObj);
                                    }
                                    else
                                    {
                                        if (inex >= 0)
                                        {

                                            showOppAdjTiles();
                                            tempObject.transform.position = hitTile.transform.position;
                                            ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), hitTile);

                                            hitTile.myColor = Color.red;
                                        }


                                    }
                                }
                            }
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
                                    targetTile.myColor = Color.red;

                                }
                                if (SetGridObjectPosition(tempObject.GetComponent<GridObject>(), targetTile.transform.position) == true)
                                {
                                    ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));
                                }



                            }
                        }

                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            if (sfx)
                            {
                                if (sfxClips.Length > 1)
                                {
                                    sfx.loadAudio(sfxClips[2]);
                                    sfx.playSound();
                                }
                            }
                            if (targetIndex < doubleAdjOppTiles.Count)
                            {
                                if (prevState != State.PlayerOppSelecting)
                                {

                                    TileScript targetTile = doubleAdjOppTiles[targetIndex];
                                    if (GetObjectAtTile(targetTile) != null)
                                    {
                                        oppObj = GetObjectAtTile(targetTile) as LivingObject;
                                        targetTile.myColor = Color.red;
                                        menuStackEntry entry = new menuStackEntry();
                                        entry.state = State.PlayerOppOptions;
                                        entry.index = invManager.currentIndex;
                                        entry.menu = currentMenu.OppSelection;

                                        enterState(entry);
                                        menuManager.showOpportunityOptions(oppObj);
                                    }
                                }
                            }
                        }
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            player.current.TakeAction();
                            oppEvent.caller = null;
                            CleanMenuStack();
                        }
                        if (Input.GetMouseButtonDown(1))
                        {
                            player.current.TakeAction();
                            oppEvent.caller = null;
                            CleanMenuStack();
                        }
                    }
                    break;
                case State.PlayerOppOptions:
                    {
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            if (sfx)
                            {
                                if (sfxClips.Length > 1)
                                {
                                    sfx.loadAudio(sfxClips[2]);
                                    sfx.playSound();
                                }
                            }
                            player.useOppAction(oppObj);

                        }

                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            CancelMenuAction(oppObj);
                            eventManager.currentEvent.data = player;
                            showOppAdjTiles();
                        }
                        if (Input.GetMouseButtonDown(1))
                        {
                            CancelMenuAction(oppObj);
                            eventManager.currentEvent.data = player;
                            showOppAdjTiles();
                        }
                    }
                    break;
                case State.PlayerEquipping:
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if ((int)descriptionState + 1 <= 2)
                        {
                            descriptionState = (descState)((int)descriptionState + 1);
                            myCamera.UpdateCamera();

                        }
                        else
                        {
                            descriptionState = descState.stats;
                            myCamera.UpdateCamera();

                        }
                    }
                    break;
                case State.PlayerWait:
                    break;
                case State.FreeCamera:
                    if (Input.GetMouseButtonDown(1))
                    {
                        if (myCamera.infoObject)
                        {
                            if (myCamera.infoObject.GetComponent<LivingObject>())
                            {

                                if (detailsScreen)
                                {
                                    detailsScreen.currentObj = myCamera.infoObject.GetComponent<LivingObject>();
                                    StackDetails();
                                }
                            }
                            else
                            {
                                StackOptions();
                            }
                        }
                        else
                        {
                            StackOptions();
                        }
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        RaycastHit hit = new RaycastHit();
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit))
                        {
                            Vector3 w = hit.point;
                            w.x = Mathf.Round(w.x);
                            w.y = Mathf.Round(w.y);
                            w.z = Mathf.Round(w.z);

                            if (GetTileIndex(w) >= 0)
                            {
                                TileScript hitTile = GetTileAtIndex(GetTileIndex(w));
                                bool alreadySelected = false;

                                if (tempObject.GetComponent<GridObject>().currentTile == hitTile)
                                {
                                    alreadySelected = true;

                                }

                                if (alreadySelected)
                                {
                                    bool hitplayer = false;
                                    for (int i = 0; i < turnOrder.Count; i++)
                                    {
                                        if (turnOrder[i] == myCamera.infoObject)
                                        {
                                            if (turnOrder[i].ACTIONS > 0)
                                            {
                                                currentObject = myCamera.infoObject;
                                                player.current = currentObject.GetComponent<LivingObject>();

                                                enterState(defaultEntry);
                                                menuManager.ShowCommandCanvas();
                                                CreateEvent(this, currentObject, "Select Camera Event", CameraEvent);
                                                hitplayer = true;
                                                break;
                                            }
                                        }

                                    }
                                    if (!hitplayer)
                                    {
                                        PlayExitSnd();
                                    }
                                }
                                else
                                {
                                    ShowWhite();
                                    for (int i = 0; i < turnOrder.Count; i++)
                                    {
                                        ShowSelectedTile(turnOrder[i], Common.orange);

                                    }
                                    tempObject.transform.position = hitTile.transform.position;
                                    ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), hitTile);
                                    if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                                        ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                                    ShowSelectedTile(tempObject.GetComponent<GridObject>());
                                }
                            }
                        }
                        //Vector3 mousepos = Input.mousePosition;
                        //mousepos.z = -myCamera.z;


                    }
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        ShowWhite();
                        for (int i = 0; i < turnOrder.Count; i++)
                        {
                            ShowSelectedTile(turnOrder[i], Common.orange);

                        }
                        MoveGridObject(tempObject.GetComponent<GridObject>(), new Vector3(0, 0, 1));
                        ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));
                        if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                            ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                        ShowSelectedTile(tempObject.GetComponent<GridObject>());
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        ShowWhite();
                        for (int i = 0; i < turnOrder.Count; i++)
                        {
                            ShowSelectedTile(turnOrder[i], Common.orange);

                        }
                        MoveGridObject(tempObject.GetComponent<GridObject>(), new Vector3(-1, 0, 0));
                        ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));
                        if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                            ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                        ShowSelectedTile(tempObject.GetComponent<GridObject>());
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        ShowWhite();
                        for (int i = 0; i < turnOrder.Count; i++)
                        {
                            ShowSelectedTile(turnOrder[i], Common.orange);

                        }
                        MoveGridObject(tempObject.GetComponent<GridObject>(), new Vector3(0, 0, -1));
                        ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));
                        if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                            ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                        ShowSelectedTile(tempObject.GetComponent<GridObject>());
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        ShowWhite();
                        for (int i = 0; i < turnOrder.Count; i++)
                        {
                            ShowSelectedTile(turnOrder[i], Common.orange);

                        }
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
                            myCamera.UpdateCamera();

                        }
                        else
                        {
                            descriptionState = descState.stats;
                            myCamera.UpdateCamera();

                        }
                    }
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        if (sfx)
                        {
                            if (sfxClips.Length > 1)
                            {
                                sfx.loadAudio(sfxClips[2]);
                                sfx.playSound();
                            }
                        }
                        for (int i = 0; i < turnOrder.Count; i++)
                        {
                            if (turnOrder[i].ACTIONS > 0)
                            {

                                if (tempObject.GetComponent<GridObject>().currentTile == turnOrder[i].currentTile)
                                {
                                    player.current = turnOrder[i];
                                    currentObject = turnOrder[i];

                                    // enterState(defaultEntry);
                                    StackCMDSelection();
                                    CreateEvent(this, currentObject, "Select Camera Event", CameraEvent);
                                }
                            }

                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Escape))
                    {

                        if (myCamera.infoObject)
                        {
                            if (myCamera.infoObject.GetComponent<LivingObject>())
                            {

                                if (detailsScreen)
                                {
                                    detailsScreen.currentObj = myCamera.infoObject.GetComponent<LivingObject>();
                                    StackDetails();
                                }
                            }
                            else
                            {
                                StackOptions();
                            }
                        }
                        else
                        {
                            StackOptions();
                        }
                    }
                    break;
                case State.EnemyTurn:
                    break;
                case State.PlayerOppMove:
                    if (Input.GetMouseButtonDown(0))
                    {
                        MovetoMousePos(oppObj);
                    }
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
                        if (sfx)
                        {
                            if (sfxClips.Length > 1)
                            {
                                sfx.loadAudio(sfxClips[2]);
                                sfx.playSound();
                            }
                        }
                        ComfirmMenuAction(oppObj);
                        oppEvent.caller = null;
                        CreateEvent(this, null, "return state event", BufferedReturnEvent);
                        CreateEvent(this, null, "return state event", BufferedReturnEvent);
                        CreateEvent(this, null, "return state event", BufferedReturnEvent);
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        CancelMenuAction(oppObj);
                        eventManager.currentEvent.data = player;
                    }
                    if (Input.GetMouseButtonDown(1))
                    {
                        CancelMenuAction(oppObj);
                        eventManager.currentEvent.data = player;
                    }
                    break;

                case State.ChangeOptions:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        returnState();
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        returnState();
                    }
                    break;


                case State.CheckDetails:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        StackDetails();
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        StackDetails();
                    }
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        if (detailsScreen.selectedContent > 3)
                        {
                            detailsScreen.selectedContent--;
                        }
                        else if (detailsScreen.selectedContent == 3)
                        {
                            detailsScreen.selectedContent = 1;
                        }
                        else
                        {
                            detailsScreen.selectedContent = 7;
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        if (detailsScreen.selectedContent == 7)
                        {
                            detailsScreen.selectedContent = 1;
                        }
                        else if (detailsScreen.selectedContent < 3)
                        {
                            detailsScreen.selectedContent = 3;
                        }
                        else
                        {
                            detailsScreen.selectedContent++;
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        if (detailsScreen.selectedContent == 0)
                        {
                            int truedetail = (int)detailsScreen.detail;
                            truedetail--;
                            if (truedetail < 0)
                            {
                                truedetail = 5;
                            }
                            detailsScreen.detail = (DetailType)truedetail;
                        }
                        else if (detailsScreen.selectedContent < 3)
                        {
                            detailsScreen.selectedContent--;
                        }

                    }

                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        if (detailsScreen.selectedContent == 2)
                        {
                            int truedetail = (int)detailsScreen.detail;
                            truedetail++;
                            if (truedetail > 5)
                            {
                                truedetail = 0;
                            }
                            detailsScreen.detail = (DetailType)truedetail;
                        }
                        else if (detailsScreen.selectedContent < 2)
                        {
                            detailsScreen.selectedContent++;
                        }

                    }
                    break;
                default:
                    break;
            }

        }

    }
    public void NextRound()
    {
        //  Debug.Log("next round");
        currOppList.Clear();
        doubleAdjOppTiles.Clear();
        menuManager.ShowNone();
        LivingObject[] livingObjects = GameObject.FindObjectsOfType<LivingObject>();
        if (currentState == State.EnemyTurn)
        {
            currentState = State.FreeCamera;

        }
        else
        {
            currentState = State.EnemyTurn;
            menuManager.ShowNone();
        }
        turnOrder.Clear();
        for (int i = livingObjects.Length - 1; i >= 0; i--)
        {
            if (!turnOrder.Contains(livingObjects[i]))
            {
                if (!livingObjects[i].DEAD)
                {


                    if (currentState == State.EnemyTurn)
                    {
                        if (livingObjects[i].FACTION == Faction.enemy)
                        {
                            turnOrder.Add(livingObjects[i]);
                        }
                    }
                    else
                    {
                        if (livingObjects[i].FACTION == Faction.ally)
                        {
                            turnOrder.Add(livingObjects[i]);

                        }

                    }






                }
            }

        }

        // if (currentState == State.EnemyTurn)
        {
            for (int i = 0; i < turnOrder.Count; i++)
            {
                if (turnOrder[i].GetComponent<BuffScript>())
                {
                    BuffScript[] buffs = turnOrder[i].GetComponents<BuffScript>();

                    for (int j = 0; j < buffs.Length; j++)
                    {
                        buffs[j].UpdateCount(currentObject.GetComponent<LivingObject>());
                    }
                }
                if (turnOrder[i].GetComponent<InventoryScript>())
                {
                    turnOrder[i].INVENTORY.ChargeShields();//GetComponent<EffectScript>().ApplyReaction(this, turnOrder[i].GetComponent<LivingObject>());
                }
                if (turnOrder[i].GetComponent<EffectScript>())
                {
                    turnOrder[i].GetComponent<EffectScript>().ApplyReaction(this, turnOrder[i].GetComponent<LivingObject>());
                }
                if (turnOrder[i].GetComponent<SecondStatusScript>())
                {
                    turnOrder[i].GetComponent<SecondStatusScript>().ReduceCount(this, turnOrder[i].GetComponent<LivingObject>());
                }
                if (turnOrder[i].refreshState > 0)
                {
                    turnOrder[i].refreshState--;
                    if (turnOrder[i].refreshState <= 0)
                    {
                        turnOrder[i].PSTATUS = PrimaryStatus.normal;
                    }
                }
                if (turnOrder[i].STATS)
                {

                    int acts = (int)(turnOrder[i].SPEED / 10);

                    if (turnOrder[i].GENERATED < 0)
                    {
                        if (-1 * turnOrder[i].GENERATED >= acts)
                        {
                            acts = 2;
                        }
                    }
                    else
                    {

                        acts += turnOrder[i].GENERATED;
                        acts += 2;
                    }



                    turnOrder[i].ACTIONS = acts;


                    turnOrder[i].GENERATED = 0;

                    if (turnOrder[i].GetComponent<EnemyScript>())
                    {
                        EnemyScript anEnemy = turnOrder[i].GetComponent<EnemyScript>();
                        CreateEvent(this, anEnemy, "Enemy Event", EnemyEvent, null, -1, SetEnemyEvent);
                    }
                }
            }
        }

    }
    public bool EnemyEvent(Object data)
    {
        EnemyScript enemy = data as EnemyScript;
        return !enemy.isPerforming;
    }
    public void SetEnemyEvent(Object data)
    {
        EnemyScript enemy = data as EnemyScript;
        enemy.DetermineActions();
        currentObject = enemy;
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
            myCamera.UpdateCamera();
        }
        return result;
    }
    public void NextTurn(string invokingObj)
    {
        //Debug.Log("next " + invokingObj);
        CreateEvent(this, null, "Next turn event from " + invokingObj, NextTurnEvent);

    }
    public void showAttackableTiles()
    {
        for (int i = 0; i < attackableTiles.Count; i++)
        {
            for (int j = 0; j < attackableTiles[i].Count; j++)
            {
                if (player.currentSkill)
                {
                    if (player.currentSkill.ELEMENT == Element.Buff)
                    {
                        attackableTiles[i][j].myColor = Common.lime;

                    }
                    else
                    {
                        attackableTiles[i][j].myColor = Common.pink;

                    }
                }
                else
                {
                    attackableTiles[i][j].myColor = Common.pink;

                }
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
        ShowWhite();
        for (int i = 0; i < doubleAdjOppTiles.Count; i++)
        {

            doubleAdjOppTiles[i].myColor = Common.orange;

        }
        myCamera.UpdateCamera();
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

    public void ShowGridObjectAffectArea(GridObject obj, bool cameraLock = true)
    {

        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)];
                float tempX = temp.transform.position.x;
                float tempY = temp.transform.position.z;
                if (!obj)
                    return;
                float objX = obj.currentTile.transform.position.x;
                float objY = obj.currentTile.transform.position.z;

                int MoveDist = 0;
                int attackDist = 0;
                //  int actionsRemaining = 1;
                if (obj.GetComponent<LivingObject>())
                {
                    LivingObject liveObj = obj.GetComponent<LivingObject>();
                    if (!liveObj.isSetup)
                    {
                        liveObj.Setup();
                    }
                    MoveDist = liveObj.MOVE_DIST;
                    attackDist = liveObj.WEAPON.DIST;
                    // actionsRemaining = liveObj.ACTIONS;
                }
                xDist = Mathf.Abs(tempX - objX);
                yDist = Mathf.Abs(tempY - objY);
                if (xDist == 0 && yDist == 0)
                {
                    if (cameraLock)
                    {

                        myCamera.currentTile = temp.GetComponent<TileScript>();
                        if (!obj.GetComponent<TempObject>())
                            myCamera.infoObject = GetObjectAtTile(temp.GetComponent<TileScript>());
                    }
                    temp.GetComponent<TileScript>().myColor = Color.white;
                }
                else if (xDist + yDist <= MoveDist)
                {
                    temp.GetComponent<TileScript>().myColor = Color.cyan;
                }
                else if (xDist + yDist <= MoveDist + attackDist)
                {
                    temp.GetComponent<TileScript>().myColor = Common.red;
                }
                else
                {
                    temp.GetComponent<TileScript>().myColor = Color.white;

                }
            }
        }
        myCamera.UpdateCamera();
    }
    public void ShowWhite()
    {
        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)];
                TileScript tile = temp.GetComponent<TileScript>();
                if (tile)
                {
                    if (tile.canBeOccupied)
                    {
                        temp.GetComponent<TileScript>().myColor = Color.white;

                    }
                }
            }
        }
        myCamera.UpdateCamera();

    }
    public void ShowSelectedTile(GridObject obj)
    {
        TileScript theTile = GetTile(obj);
        theTile.myColor = Color.grey;
        myCamera.UpdateCamera();
    }

    public void ShowSelectedTile(GridObject obj, Color color)
    {
        TileScript theTile = GetTile(obj);
        theTile.myColor = color;
        myCamera.UpdateCamera();
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
        myCamera.UpdateCamera();
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
                                //    Debug.Log("tile = " + tempTile.transform.position);
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
                    temp.GetComponent<TileScript>().myColor = Color.cyan;
                }
                else
                {
                    temp.GetComponent<TileScript>().myColor = Color.white;
                }
            }
        }
        myCamera.UpdateCamera();
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
                    temp.GetComponent<TileScript>().myColor = Common.red;
                }
                else
                {
                    temp.GetComponent<TileScript>().myColor = Color.white;
                }
            }
        }
        myCamera.UpdateCamera();
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
        myCamera.UpdateCamera();
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
        if (GetTileAtIndex(TileIndex).canBeOccupied == false)
            return;
        if (obj.GetComponent<LivingObject>())
        {
            TileScript atile = tileMap[TileIndex].GetComponent<TileScript>();
            if (IsTileOccupied(atile) == true)
            {
                GridObject gridObject = GetObjectAtTile(atile).GetComponent<LivingObject>();
                if (gridObject.GetComponent<LivingObject>())
                {
                    if (gridObject.GetComponent<LivingObject>().FACTION != obj.GetComponent<LivingObject>().FACTION)
                    {
                        return;

                    }
                }
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
        }
        myCamera.UpdateCamera();
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
    public void ComfirmMoveGridObject(GridObject obj, TileScript tile)
    {
        if (obj.gameObject != tempObject)
        {
            obj.currentTile.isOccupied = false;
            obj.currentTile = tile;
            obj.currentTile.isOccupied = true;
        }
        else
        {
            obj.currentTile = tile;
        }
        myCamera.currentTile = tile;
        myCamera.infoObject = GetObjectAtTile(tile);
        myCamera.UpdateCamera();
    }
    public int GetTileIndex(GridObject checkTile)
    {
        if (checkTile == null)
        {
            return -1;
        }
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
        if (GetTileAtIndex(TileIndex).canBeOccupied == false)
        { return -1; }
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
        //  Debug.Log("checking for adj");
        // Debug.Log("Trigger Element");
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
                    if (obj)
                    {

                        if (obj.GetComponent<LivingObject>())
                        {
                            LivingObject liveObj = obj.GetComponent<LivingObject>();
                            if (!liveObj.DEAD)
                            {
                                if (liveObj.FACTION == origin.FACTION)
                                {

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
                            if (testIndex >= 0)
                            {
                                TileScript t = GetTileAtIndex(testIndex);
                                float checkX = Mathf.Abs(t.transform.position.x - checkPos.x);
                                float checkY = Mathf.Abs(t.transform.position.z - checkPos.z);
                                if (checkX + checkY <= dist)
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
                            if (testIndex >= 0)
                            {
                                TileScript t = GetTileAtIndex(testIndex);
                                float checkX = Mathf.Abs(t.transform.position.x - checkPos.x);
                                float checkY = Mathf.Abs(t.transform.position.z - checkPos.z);
                                if (checkX + checkY <= dist)
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
                                if (testIndex >= 0)
                                {
                                    TileScript t = GetTileAtIndex(testIndex);
                                    float checkX = Mathf.Abs(t.transform.position.x - checkPos.x);
                                    float checkY = Mathf.Abs(t.transform.position.z - checkPos.z);
                                    if (checkX + checkY <= dist)
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
                {


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
                                if (testIndex >= 0)
                                {
                                    TileScript t = GetTileAtIndex(testIndex);
                                    float checkX = Mathf.Abs(t.transform.position.x - checkPos.x);
                                    float checkY = Mathf.Abs(t.transform.position.z - checkPos.z);
                                    if (checkX + checkY <= dist)
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
                }
                break;
            case RanngeType.anyarea:
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
                                if (testIndex >= 0)
                                {
                                    TileScript t = GetTileAtIndex(testIndex);
                                    float checkX = Mathf.Abs(t.transform.position.x - checkPos.x);
                                    float checkY = Mathf.Abs(t.transform.position.z - checkPos.z);
                                    if (checkX + checkY <= dist)
                                    {
                                        TileScript realTile = GetTileAtIndex(testIndex);
                                        if (!tiles.Contains(realTile))
                                            tiles.Add(realTile);
                                    }
                                }
                            }
                        }

                    }
                    // List<TileScript> mytile = new List<TileScript>();

                    tiles.Add(GetTileAtIndex(checkIndex));

                    returnList.Add(tiles);
                }
                break;
            case RanngeType.multiarea:
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
                            if (testIndex >= 0)
                            {
                                TileScript t = GetTileAtIndex(testIndex);
                                float checkX = Mathf.Abs(t.transform.position.x - checkPos.x);
                                float checkY = Mathf.Abs(t.transform.position.z - checkPos.z);
                                if (checkX + checkY <= dist)
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
            default:
                break;
        }

        return returnList;
    }

    public void ShowSkillAttackbleTiles(LivingObject obj, CommandSkill skill)
    {
        ShowWhite();
        List<List<TileScript>> tempTiles = GetSkillsAttackableTiles(obj, skill);
        if (tempTiles == null)
            return;
        if (currentState == State.PlayerOppOptions)
            return;
        if (tempTiles.Count > 0)
        {
            for (int i = 0; i < tempTiles.Count; i++) //list of lists
            {
                for (int j = 0; j < tempTiles[i].Count; j++) //indivisual list
                {

                    if (skill.ELEMENT == Element.Buff)
                    {
                        tempTiles[i][j].myColor = Common.green;

                    }
                    else
                    {
                        tempTiles[i][j].myColor = Common.red;

                    }

                }
            }

        }
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
                if (testIndex >= 0)
                {
                    TileScript t = GetTileAtIndex(testIndex);

                    float checkX = Mathf.Abs(checkPos.x - liveObj.transform.position.x);
                    float checkY = Mathf.Abs(checkPos.z - liveObj.transform.position.z);


                    if (checkX + checkY <= Dist)
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
            Debug.Log("Selecting menu item from obj :" + invokingObject.FullName);
            invManager.selectedMenuItem.ApplyAction(invokingObject);
        }
        myCamera.UpdateCamera();

    }
    public void SelectMenuItem(MenuItem selectedItem)
    {
        if (invManager)
        {
            if (sfx)
            {
                if (sfxClips.Length > 1)
                {
                    sfx.loadAudio(sfxClips[1]);
                    sfx.playSound();
                }
            }

            selectedItem.ApplyAction(player.current);
        }
        myCamera.UpdateCamera();

    }
    public void HoverMenuItem(MenuItem selectedItem)
    {
        if (invManager)
        {
            if (currentState != State.ChangeOptions)
            {
                if (sfx)
                {
                    if (sfxClips.Length > 1)
                    {
                        sfx.loadAudio(sfxClips[1]);
                        sfx.playSound();
                    }
                }

                invManager.HoverSelect(selectedItem, selectedItem.transform.parent.gameObject);
                invManager.currentIndex = selectedItem.transform.GetSiblingIndex();
            }// invManager.ForceSelect();
            // invManager.Validate("Manager hover");

        }
        myCamera.UpdateCamera();
    }
    public bool ComfirmMenuAction(GridObject invokingObject)
    {
        bool res = false;
        for (int i = 0; i < commandItems.Length; i++)
        {

            if (commandItems[i].itemType == currentMenuitem)
            {
                res = commandItems[i].ComfirmAction(invokingObject);
                return res;
            }

        }

        MenuItem savedItem = commandItems[0];
        int savedType = savedItem.itemType;

        savedItem.itemType = currentMenuitem;
        res = savedItem.ComfirmAction(invokingObject);

        return res;
    }
    public void CancelMenuAction(GridObject invokingObject)
    {

        Vector3 resetPos = invokingObject.currentTile.transform.position;
        resetPos.y = 0.5f;
        invokingObject.transform.position = resetPos;

        if (attackableTiles != null)
        {
            attackableTiles.Clear();
            ShowWhite();
        }
        if (newSkillEvent.caller)
        {
            newSkillEvent.caller = null;

        }
        else
        {
            CreateEvent(this, null, "return state event", BufferedReturnEvent);

        }
        //for (int i = 0; i < commandItems.Length; i++)
        //{

        //    if (commandItems[i].itemType == currentMenuitem)
        //    {
        //        commandItems[i].CancelAction(invokingObject);
        //        break;
        //    }
        //}
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
       if( dmgObject.ARMOR.DamageArmor(dmg))
        {
            if (options.battleAnims)
            {
               
                GridAnimationObj gao = null;
                gao = PrepareGridAnimation(null, dmgObject);
                gao.type = -1;
                gao.magnitute = 0;
                CreateEvent(this, gao, "Animation request: " + AnimationRequests + "", CheckAnimation, gao.StartCountDown, 0);

            }
            CreateTextEvent(this, dmgObject.NAME + " ward broke!", "ward break event", CheckText, TextStart);
        }

    }
    public DmgReaction CalcDamage(LivingObject attackingObject, LivingObject dmgObject, Element attackingElement, EType attackType, int dmg, Reaction alteration = Reaction.none)
    {
        if (attackingElement == Element.Buff)
        {
            Debug.Log("U don goofed");
        }
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
                mod = 1.0f;
                react.reaction = Reaction.absorb;
                break;

            case EHitType.weak:
                mod = 2.0f;
                react.reaction = Reaction.weak;
                break;

            case EHitType.savage:

                react.reaction = Reaction.turnloss;
                mod = 2.0f;
                //todo add lose a action
                break;

            case EHitType.cripples:
                mod = 4.0f;
                react.reaction = Reaction.cripple;
                break;

            case EHitType.leathal:
                mod = 6.0f;
                react.reaction = Reaction.turnAndcrip;
                //dmgObject.GENERATED = 0;
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
            resist = dmgObject.DEFENSE;
            if (alteration == Reaction.reduceDef)
            {
                resist = resist * 0.5f;
            }

        }
        else
        {
            resist = dmgObject.RESIESTANCE;
            if (alteration == Reaction.reduceRes)
            {
                resist = resist * 0.5f;
            }
        }

        if (resist <= 0)
        {
            resist = 1;
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
        //  Debug.Log("Combined: " + calc);
        //   Debug.Log("Calc2:" + calc);
        calc = calc * ((float)attackingObject.LEVEL / (float)dmgObject.LEVEL);
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
        if (returnInt > Common.maxDmg)
        {
            returnInt = Common.maxDmg;
        }
        react.damage = returnInt;


        // Debug.Log("returnin count state:" + currentState + " invoker: " + attackingObject.FullName);
        return react;
    }
    public DmgReaction CalcDamage(LivingObject attackingObject, LivingObject dmgObject, CommandSkill skill, Reaction alteration = Reaction.none)
    {
        //  Debug.Log("Calc 1");
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
            return new DmgReaction() { damage = 0, reaction = Reaction.buff };
        }
        else
        {
            int chance = skill.ACCURACY;
            int result = (int)Random.Range(0.0f, 100.0f);
            if (result <= chance)
            {
                return CalcDamage(attackingObject, dmgObject, skill.ELEMENT, skill.ETYPE, (int)skill.DAMAGE, alteration);
            }
            else
            {
                return new DmgReaction() { damage = 0, reaction = Reaction.missed };
            }
            //if (skill.EFFECT != SideEffect.none)
            //{
            //    Debug.Log("Applying effect");
            //    ApplyEffect(dmgObject, skill.EFFECT);
            //}

        }
    }

    public DmgReaction CalcDamage(AtkConatiner conatiner)
    {
        // Debug.Log("Calc 2");
        if (conatiner.command)
        {
            return CalcDamage(conatiner.attackingObject, conatiner.dmgObject, conatiner.command, conatiner.alteration);
        }
        else
        {
            int chance = conatiner.attackingObject.WEAPON.ACCURACY;
            int result = (int)Random.Range(0.0f, 100.0f);
            if (result <= chance)
            {
                return CalcDamage(conatiner.attackingObject, conatiner.dmgObject, conatiner.attackingObject.WEAPON, conatiner.alteration);
            }
            else
            {
                return new DmgReaction() { damage = 0, reaction = Reaction.missed };
            }


        }

    }


    public DmgReaction CalcDamage(LivingObject attackingObject, LivingObject dmgObject, WeaponEquip weapon, Reaction alteration = Reaction.none)
    {
        // Debug.Log("Calc 3");
        int chance = weapon.ACCURACY;
        int result = (int)Random.Range(0.0f, 100.0f);
        if (result <= chance)
        {
            return CalcDamage(attackingObject, dmgObject, weapon.AFINITY, weapon.ATTACK_TYPE, weapon.ATTACK, alteration);
        }
        else
        {
            return new DmgReaction() { damage = 0, reaction = Reaction.missed };
        }
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
    public GridAnimationObj PrepareGridAnimation(GridAnimationObj gao, LivingObject target)
    {
    
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
        myCamera.currentTile = target.currentTile;
        return gao;
    }
    public void ApplyReaction(LivingObject attackingObject, LivingObject target, DmgReaction react, Element dmgElement)
    {
        //  Debug.Log("Applying dmg: " + react.damage);
        int gtype = (int)dmgElement;
        switch (react.reaction)
        {
            case Reaction.none:
                DamageLivingObject(target, react.damage);

                break;
            case Reaction.buff:

                break;
            case Reaction.cripple:
                DamageLivingObject(target, react.damage);
                target.PSTATUS = PrimaryStatus.crippled;
                CreateTextEvent(this, "" + attackingObject.FullName + " did CRIPPLING damage", "enemy atk", CheckText, TextStart);

                break;
            case Reaction.nulled:
                DamageLivingObject(target, react.damage);
                CreateTextEvent(this, "" + attackingObject.FullName + " attack was NULLED", "enemy atk", CheckText, TextStart);

                break;
            case Reaction.reflected:
                DamageLivingObject(attackingObject, react.damage);
                CreateTextEvent(this, "" + attackingObject.FullName + " attack was reflected back at them", "enemy atk", CheckText, TextStart);

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
            case Reaction.reduceStr:
                break;
            case Reaction.reduceDef:
                break;
            case Reaction.reduceSpd:
                break;
            case Reaction.reduceMag:
                break;
            case Reaction.reduceRes:
                break;
            case Reaction.reduceLuck:
                break;
            case Reaction.turnloss:
                DamageLivingObject(target, react.damage);
                target.GENERATED--;
                break;
            case Reaction.turnAndcrip:
                CreateTextEvent(this, "" + attackingObject.FullName + " did LEATHAL damage", "enemy atk", CheckText, TextStart);

                DamageLivingObject(target, react.damage);
                target.PSTATUS = PrimaryStatus.crippled;
                target.GENERATED--;
                break;
            case Reaction.absorb:
                target.STATS.HEALTH += react.damage;
                CreateTextEvent(this, "" + attackingObject.FullName + " attack healed the enemy", "enemy atk", CheckText, TextStart);
                break;
            case Reaction.weak:
                DamageLivingObject(target, react.damage);
                CreateTextEvent(this, "" + attackingObject.FullName + " attack did weakening damage", "enemy atk", CheckText, TextStart);
                break;
            case Reaction.missed:
                CreateTextEvent(this, "" + attackingObject.FullName + " attack missed", "enemy atk", CheckText, TextStart);
                break;
            default:
                break;
        }
        //DmgTextObj dto = null;
        //dmgRequest++;
        //if (dmgRequest <= dmgText.Count)
        //{

        //    for (int i = 0; i < dmgText.Count; i++)
        //    {
        //        if (!dmgText[i].isShowing)
        //        {
        //            dto = dmgText[i];
        //            dto.text.text = react.damage.ToString();
        //            dto.border.text = react.damage.ToString();
        //            Vector3 v3 = new Vector3(target.transform.position.x, 1, target.transform.position.z);
        //            v3.z -= 0.1f;
        //            dto.transform.position = v3;
        //            dto.text.color = new Color(Mathf.Min(255, react.damage), 0, 0);
        //            break;
        //        }
        //    }
        //}

        //else
        //{
        //    //  Debug.Log("new dmg text");
        //    GameObject tjObject = Instantiate(dmgPrefab);
        //    tjObject.SetActive(false);

        //    dto = tjObject.GetComponent<DmgTextObj>();
        //    dmgText.Add(dto);
        //    dto.manager = this;
        //    dto.Setup();
        //    dto.text.text = react.damage.ToString();
        //    dto.border.text = react.damage.ToString();
        //    Vector3 v3 = new Vector3(target.transform.position.x, 1, target.transform.position.z);
        //    v3.z -= 0.1f;
        //    dto.transform.position = v3;
        //    dto.text.color = new Color(Mathf.Min(255, react.damage), 0, 0);
        //}


        if (eventManager)
        {

            if (options)
            {
                if (options.dmgAnims)
                {


                    if (react.reaction != Reaction.missed)
                    {
                        if (react.reaction == Reaction.buff)
                        {
                            CreateDmgTextEvent("+", Common.red, target);

                        }
                        else if (react.reaction > Reaction.weak)
                        {
                            float colorVal = 0.0f;
                            if (react.damage > 0)
                            {
                                colorVal = (float)react.damage * 1.5f / 100.0f;
                            }
                            CreateDmgTextEvent(react.reaction.ToString(), Common.red, target);
                            CreateDmgTextEvent(react.damage.ToString(), new Color(Mathf.Min(1, colorVal), 0, 0), target);

                        }
                        else
                        {
                            float colorVal = 0.0f;
                            if (react.damage > 0)
                            {
                                colorVal = (float)react.damage * 1.5f / 100.0f;
                            }
                            CreateDmgTextEvent(react.damage.ToString(), new Color(Mathf.Min(1, colorVal), 0, 0), target);
                        }
                    }
                    else
                    {
                        CreateDmgTextEvent("MISSED", Common.red, target);

                    }


                }
                // CreateEvent(this, dto, "DmgText request: " + dmgRequest + "", CheckDmgText, dto.StartCountDown, 0);
                if (options.battleAnims)
                {

                    GridAnimationObj gao = null;
                    gao = PrepareGridAnimation(null, target);
                    gao.type = gtype;
                    gao.magnitute = react.damage;
                    CreateEvent(this, gao, "Animation request: " + AnimationRequests + "", CheckAnimation, gao.StartCountDown, 0);



                }
            }
        }
        bool killedEnemy = false;
        if (target.HEALTH <= 0)
        {
            if (turnOrder.Contains(target))
            {
                turnOrder.Remove(target);
            }
            target.DEAD = true;//gameObject.SetActive(false);
            target.Die();
            // target.GetComponent<SpriteRenderer>().color = Common.semi;
            if (target.FACTION != Faction.ally)
            {
                killedEnemy = true;

            }
            LivingObject[] livingObjects = GameObject.FindObjectsOfType<LivingObject>();
            bool winner = true;
            bool survivor = false;
            for (int i = 0; i < livingObjects.Length; i++)
            {
                LivingObject liver = livingObjects[i];
                if (liver.FACTION == Faction.enemy)
                {
                    winner = false;
                }
                if (liver.FACTION == Faction.ally)
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
        if (currentState != State.EnemyTurn)
        {
            if (react.reaction != Reaction.missed)
            {

                DetermineExp(attackingObject, target, killedEnemy);
                if (options)
                {
                    if (options.showExp)
                    {
                        if (currentState != State.EnemyTurn)
                        {
                            CreateEvent(this, null, "Exp event", UpdateExpBar, ShowExpBar);

                        }

                    }
                    else
                    {

                        if (attackingObject.BASE_STATS.EXP >= 100)
                        {
                            attackingObject.LevelUp();

                        }
                    }
                }
                else
                {
                    if (attackingObject.BASE_STATS.EXP >= 100)
                    {
                        attackingObject.LevelUp();
                    }
                }

            }
        }
    }
    public void ShowExpBar()
    {
        expbar.gameObject.SetActive(true);
        expbar.updating = true;
    }
    public bool UpdateExpBar(Object data)
    {
        return !expbar.updating;
    }
    public void DetermineExp(LivingObject atker, LivingObject target, bool killed)
    {
        if (expbar)
        {

            expbar.currentUser = atker;
            expbar.slider.value = atker.BASE_STATS.EXP;
        }
        int diff = target.LEVEL - atker.LEVEL + 1;
        //   Debug.Log("res = " + diff);
        int amount = diff + 4;
        //   Debug.Log("Killed = " + killed);
        if (killed == true)
        {
            amount += (20 * diff) + 20;

        }
        //  Debug.Log("amt = " + amount);
        if (amount < 0)
        {
            amount = 0;
        }
        //  Debug.Log("amt2 = " + amount);

        atker.GainExp(amount);

    }
    public void CreateDmgTextEvent(string dmgValue, Color color, LivingObject target)
    {
        DmgTextObj dto = null;
        dmgRequest++;
        if (dmgRequest <= dmgText.Count)
        {

            for (int i = 0; i < dmgText.Count; i++)
            {
                if (!dmgText[i].isShowing)
                {
                    dto = dmgText[i];
                    dto.text.text = dmgValue;
                    dto.border.text = dmgValue;
                    Vector3 v3 = new Vector3(target.transform.position.x, 1, target.transform.position.z);
                    v3.z -= 0.1f;
                    dto.transform.position = v3;
                    dto.text.color = color;
                    break;
                }
            }
        }

        else
        {
            //  Debug.Log("new dmg text");
            GameObject tjObject = Instantiate(dmgPrefab);
            tjObject.SetActive(false);

            dto = tjObject.GetComponent<DmgTextObj>();
            dmgText.Add(dto);
            dto.manager = this;
            dto.Setup();
            dto.text.text = dmgValue;
            dto.border.text = dmgValue;
            Vector3 v3 = new Vector3(target.transform.position.x, 1, target.transform.position.z);
            v3.z -= 0.1f;
            dto.transform.position = v3;
            dto.text.color = color;
        }
        CreateEvent(this, dto, "DmgText request: " + dmgRequest + "", CheckDmgText, dto.StartCountDown, 0);

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

                    if (skill != null)
                    {
                        MassAtkConatiner conatiners = ScriptableObject.CreateInstance<MassAtkConatiner>();
                        conatiners.atkConatiners = new List<AtkConatiner>();
                        for (int i = 0; i < targetIndicies.Count; i++)
                        {
                            GridObject potentialTarget = GetObjectAtTile(currentAttackList[targetIndicies[i]]);
                            if (potentialTarget.GetComponent<LivingObject>())
                            {
                                LivingObject target = potentialTarget.GetComponent<LivingObject>();
                                bool acceptable = false;
                                if (skill.ELEMENT == Element.Buff)
                                {
                                    if (target.FACTION == invokingObject.FACTION)
                                    {
                                        if (!target.INVENTORY.BUFFS.Contains(skill))
                                        {
                                            acceptable = true;

                                        }
                                        else
                                        {
                                            CreateTextEvent(this, skill.NAME + " is already applied to " + target.FullName, "validation text", CheckText, TextStart);
                                            PlayExitSnd();
                                        }
                                    }
                                    else
                                    {
                                        CreateTextEvent(this, skill.NAME + " Not a valid target " + target.FullName, "validation text", CheckText, TextStart);
                                        PlayExitSnd();
                                    }
                                }
                                else
                                {
                                    if (target.FACTION != invokingObject.FACTION)
                                    {
                                        acceptable = true;
                                    }

                                }
                                if (acceptable == true)
                                {
                                    hitSomething = true;


                                    //     for (int k = 0; k < skill.HITS; k++)
                                    //    {
                                    // react = CalcDamage(invokingObject, target, skill);
                                    // ApplyReaction(invokingObject, target, react);
                                    AtkConatiner conatiner = ScriptableObject.CreateInstance<AtkConatiner>();
                                    conatiner.alteration = skill.REACTION;
                                    conatiner.attackingElement = skill.ELEMENT;
                                    conatiner.attackType = skill.ETYPE;
                                    conatiner.attackingObject = invokingObject;
                                    conatiner.command = skill;
                                    conatiner.dmg = (int)skill.DAMAGE;
                                    conatiner.dmgObject = target;

                                    conatiners.atkConatiners.Add(conatiner);
                                    CreateEvent(this, conatiner, "Skill use event", AttackEvent);
                                    // }
                                }

                            }


                        }
                        CreateEvent(this, conatiners, "Skill use event", CheckForOppChanceEvent);


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


                    for (int i = 0; i < targetIndicies.Count; i++)
                    {
                        GridObject potentialTarget = GetObjectAtTile(currentAttackList[targetIndicies[i]]);
                        if (potentialTarget.GetComponent<LivingObject>())
                        {
                            LivingObject target = potentialTarget.GetComponent<LivingObject>();

                            Reaction atkReaction = Reaction.none;
                            if (target.FACTION != invokingObject.FACTION)
                            {
                                hitSomething = true;

                                if (weapon != null)
                                {
                                    for (int k = 0; k < invokingObject.AUTO_SLOTS.SKILLS.Count; k++)
                                    {
                                        if ((invokingObject.AUTO_SLOTS.SKILLS[k] as AutoSkill).ACT == AutoAct.beforeDmg)
                                        {
                                            AutoSkill auto = (invokingObject.AUTO_SLOTS.SKILLS[k] as AutoSkill);
                                            float chance = auto.CHANCE;
                                            float result = Random.value * 100;
                                            if (result <= chance)
                                            {
                                                //Debug.Log("Auto skill : " + auto.NAME + "has gone off");
                                                CreateTextEvent(this, "Auto skill : " + auto.NAME + " has gone off", "auto atk", CheckText, TextStart);

                                                atkReaction = auto.Activate(0);
                                                break;
                                            }
                                        }
                                    }
                                }
                                AtkConatiner conatiner = ScriptableObject.CreateInstance<AtkConatiner>();
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

    public void ApplyEffect(LivingObject target, SideEffect effect, float chance, CommandSkill skill = null)
    {
        float realChance = (chance / 360.0f) * Random.Range(0.8f, 1.5f) ;
        float valres = Random.value;
        // Debug.Log("Chance: " + realChance + ", Reuslt: " + valres);
        if (valres < realChance)
        {

            switch (effect)
            {
                case SideEffect.slow:
                    {
                        if (target.SSTATUS == SecondaryStatus.normal)
                        {
                            CreateTextEvent(this, target.FullName + " has been inflicted with slow", "auto atk", CheckText, TextStart);

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
                            CreateTextEvent(this, target.FullName + " has been inflicted with rage", "auto atk", CheckText, TextStart);

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
                            CreateTextEvent(this, target.FullName + " has been inflicted with charm", "auto atk", CheckText, TextStart);

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
                            CreateTextEvent(this, target.FullName + " has been inflicted with seal", "auto atk", CheckText, TextStart);

                            target.SSTATUS = SecondaryStatus.seal;
                            SecondStatusScript sf = target.gameObject.AddComponent<SecondStatusScript>();
                            sf.COUNTDOWN = 3;
                            sf.STATUS = SecondaryStatus.seal;
                        }
                    }
                    break;
                case SideEffect.poison:
                    {
                        if (target.ESTATUS == StatusEffect.none)
                        {
                            CreateTextEvent(this, target.FullName + " has been inflicted with poison", "auto atk", CheckText, TextStart);

                            target.ESTATUS = StatusEffect.poisoned;
                            EffectScript ef = target.gameObject.AddComponent<EffectScript>();
                            ef.EFFECT = StatusEffect.poisoned;
                        }
                    }
                    break;
                case SideEffect.confusion:
                    {
                        if (target.SSTATUS == SecondaryStatus.normal)
                        {
                            CreateTextEvent(this, target.FullName + " has been inflicted with confusion", "auto atk", CheckText, TextStart);

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
                            CreateTextEvent(this, target.FullName + " has been paralyzed", "auto atk", CheckText, TextStart);

                            EffectScript ef = target.gameObject.AddComponent<EffectScript>();
                            ef.EFFECT = StatusEffect.paralyzed;
                            target.ESTATUS = StatusEffect.paralyzed;
                        }
                    }
                    break;
                case SideEffect.sleep:
                    {
                        if (!target.GetComponent<EffectScript>())
                        {
                            CreateTextEvent(this, target.FullName + " has been inflicted with sleep", "auto atk", CheckText, TextStart);

                            EffectScript ef = target.gameObject.AddComponent<EffectScript>();
                            ef.EFFECT = StatusEffect.sleep;
                            target.ESTATUS = StatusEffect.sleep;
                        }
                    }
                    break;
                case SideEffect.freeze:
                    {
                        if (!target.GetComponent<EffectScript>())
                        {
                            CreateTextEvent(this, target.FullName + " has been frozen", "auto atk", CheckText, TextStart);

                            EffectScript ef = target.gameObject.AddComponent<EffectScript>();
                            ef.EFFECT = StatusEffect.frozen;
                            target.ESTATUS = StatusEffect.frozen;
                        }
                    }
                    break;
                case SideEffect.burn:
                    {
                        if (!target.GetComponent<EffectScript>())
                        {
                            CreateTextEvent(this, target.FullName + " has been burned", "auto atk", CheckText, TextStart);

                            EffectScript ef = target.gameObject.AddComponent<EffectScript>();
                            ef.EFFECT = StatusEffect.burned;
                            target.ESTATUS = StatusEffect.burned;
                        }
                    }
                    break;

                case SideEffect.bleed:
                    if (!target.GetComponent<EffectScript>())
                    {
                        CreateTextEvent(this, target.FullName + " has been paralyzed", "auto atk", CheckText, TextStart);

                        EffectScript ef = target.gameObject.AddComponent<EffectScript>();
                        ef.EFFECT = StatusEffect.paralyzed;
                        target.ESTATUS = StatusEffect.paralyzed;
                    }
                    break;
                case SideEffect.debuff:
                    {
                        if (skill)
                        {

                            List<CommandSkill> debuffs = target.INVENTORY.DEBUFFS;
                            if (!debuffs.Contains(skill))
                            {
                                bool sameType = false;
                                for (int i = 0; i < debuffs.Count; i++)
                                {
                                    if (debuffs[i].BUFFEDSTAT == skill.BUFFEDSTAT && debuffs[i].BUFFVAL == skill.BUFFVAL)
                                    {
                                        sameType = true;
                                        break;
                                    }
                                }
                                if (sameType == false)
                                {
                                    target.INVENTORY.DEBUFFS.Add(skill);
                                    DebuffScript buff = target.gameObject.AddComponent<DebuffScript>();
                                    buff.SKILL = skill;
                                    buff.BUFF = skill.BUFF;
                                    buff.COUNT = 3;
                                    target.ApplyPassives();
                                }
                            }
                        }
                    }
                    break;

            }
        }
    }
    public bool AttackEvent(Object data)
    {
        menuManager.ShowNone();
        loadTargets();
        AtkConatiner container = data as AtkConatiner;
        CreateTextEvent(this, "" + container.attackingObject.FullName + " used " + container.command.NAME, "skill atk", CheckText, TextStart);
        CommandSkill skill = container.command;
        float modification = 1.0f;
        if (skill.ETYPE == EType.magical)
            modification = container.attackingObject.STATS.SPCHANGE;

        if (skill.ETYPE == EType.physical)
        {
            if (skill.COST > 0)
            {
                modification = container.attackingObject.STATS.FTCHARGECHANGE;
            }
            else
            {
                modification = container.attackingObject.STATS.FTCOSTCHANGE;
            }
        }
        SkillScript newSkill = skill.UseSkill(container.attackingObject, modification);
        if (newSkill != null)
        {
            newSkillEvent.caller = container.attackingObject;
            newSkillEvent.data = newSkill;
            ChangeSkillPrompt(container.attackingObject.FullName, newSkill);
            CreateEvent(this, null, "New Skill Event", CheckNewSKillEvent, NewSkillStart);
            CreateTextEvent(this, "" + container.attackingObject.FullName + " learned a new skill. Equip in inventory ", "new skill event", CheckText, TextStart);
        }
        DmgReaction react = CalcDamage(container);
        container.alteration = react.reaction;
        for (int k = 0; k < skill.HITS; k++)
        {
            ApplyReaction(container.attackingObject, container.dmgObject, react, container.attackingElement);

        }
        if (react.reaction < Reaction.nulled)
        {
            if (container.command.EFFECT != SideEffect.none)
            {
                if (container.command.EFFECT == SideEffect.debuff)
                {
                    ApplyEffect(container.dmgObject, container.command.EFFECT, (float)container.command.DAMAGE * container.attackingObject.SKILL, container.command);

                }
                else
                {
                    ApplyEffect(container.dmgObject, container.command.EFFECT, (float)container.command.DAMAGE * container.attackingObject.SKILL);

                }
            }
        }
        currentState = State.PlayerTransition;


        return true;
    }
    public bool CheckForOppChanceEvent(Object data)
    {
        MassAtkConatiner containers = data as MassAtkConatiner;
        if (containers.atkConatiners.Count == 0)
        {
            return true;
        }
        bool hit = false;
        for (int i = 0; i < containers.atkConatiners.Count; i++)
        {
            AtkConatiner container = containers.atkConatiners[i];

            if (container.alteration < Reaction.nulled)
            {
                hit = true;
                bool oppAction = false;

                if (!container.dmgObject.DEAD)
                {

                    if (!currOppList.Contains(container.attackingObject))
                    {
                        currOppList.Add(container.attackingObject);
                        doubleAdjOppTiles = GetOppViaDoubleAdjecentTiles(container.attackingObject, container.attackingElement);
                        if (doubleAdjOppTiles.Count > 0)
                        {
                            for (int j = 0; j < doubleAdjOppTiles.Count; j++)
                            {
                                CreateEvent(this, GetObjectAtTile(doubleAdjOppTiles[j]) as LivingObject, "Opp Announcement", OppAnnounceEvent, null, -1, OppAnnounceStart);
                            }
                            oppEvent = CreateEvent(this, null, "Opp Event", CheckOppEvent, OppStart);
                            oppAction = true;

                        }


                    }



                }

                if (oppAction == false)
                {
                    //if (currOppList.Count == 0)
                    {
                        oppEvent.caller = null;
                        CreateEvent(this, null, "Show Command", player.ShowCmd);

                    }
                }
                break;
            }
        }
        if (!hit)
        {
            CreateEvent(this, null, "Show Command", player.ShowCmd);
        }
        return true;
    }

    public bool ECheckForOppChanceEvent(Object data)
    {
        AtkConatiner container = data as AtkConatiner;
        if (!container.dmgObject.DEAD)
        {

            if (!currOppList.Contains(container.attackingObject))
            {
                currOppList.Add(container.attackingObject);
                doubleAdjOppTiles = GetOppViaDoubleAdjecentTiles(container.attackingObject, container.attackingElement);
                if (doubleAdjOppTiles.Count > 0)
                {
                    EnemyScript enemy = GetObjectAtTile(doubleAdjOppTiles[0]).GetComponent<EnemyScript>();
                    if (enemy)
                    {
                        enemy.ACTIONS++;
                        oppEvent = CreateEvent(this, container.dmgObject, "Opp Event", enemy.EAtkEvent, null, 1);
                        CreateEvent(this, enemy, "Opp Announcement", OppAnnounceEvent, null, 1, OppAnnounceStart);
                    }
                }


            }



        }

        return true;
    }
    public bool WeaponAttackEvent(Object data)
    {
        menuManager.ShowNone();
        loadTargets();
        AtkConatiner container = data as AtkConatiner;
        CreateTextEvent(this, "" + container.attackingObject.FullName + " used their " + container.attackingObject.WEAPON.NAME + " attack", "weapon atk", CheckText, TextStart);
        DmgReaction react = CalcDamage(container);
        container.alteration = react.reaction;
        LivingObject invokingObject = container.attackingObject;


        container.attackingObject.WEAPON.Use();
        ApplyReaction(container.attackingObject, container.dmgObject, react, container.attackingElement);
        currentState = State.PlayerTransition;
        bool oppAction = false;

        if (react.reaction < Reaction.nulled && react.reaction != Reaction.missed)
        {
            for (int k = 0; k < invokingObject.AUTO_SLOTS.SKILLS.Count; k++)
            {
                if ((invokingObject.AUTO_SLOTS.SKILLS[k] as AutoSkill).ACT == AutoAct.afterDmg)
                {
                    AutoSkill auto = (invokingObject.AUTO_SLOTS.SKILLS[k] as AutoSkill);
                    float chance = auto.CHANCE + invokingObject.SKILL;
                    float result = Random.value * 100;
                    if (chance > result)
                    {
                        CreateTextEvent(this, "" + "Auto skill : " + auto.NAME + " has gone off", "auto skill ", CheckText, TextStart);

                        auto.Activate(react.damage);
                        break;
                    }
                }
            }
            if (!container.dmgObject.DEAD)
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
                        oppAction = true;
                    }

                }

            }

        }
        if (oppAction == false)
        {
            //if (currOppList.Count == 0)
            {
                oppEvent.caller = null;
                CreateEvent(this, null, "Show Command", player.ShowCmd);

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

        //menuStackEntry entry = new menuStackEntry();
        //entry.state = State.PlayerOppSelecting;
        //entry.menu = currentMenu.command;
        //enterState(entry);
        StackNewSelection(State.PlayerOppSelecting, currentMenu.command);
        showOppAdjTiles();
        menuManager.ShowNone();

    }
    public void NewSkillStart()
    {
        menuManager.ShowNewSkillPrompt();
        currentState = State.AquireNewSkill;
    }
    public void TextStart()
    {
        if (flavor)
        {

            flavor.gameObject.SetActive(true);
            flavor.textObj.StartCountDown();
        }
    }
    public bool CheckText(Object data)
    {
        if (flavor)
        {
            return !flavor.textObj.isShowing;

        }
        else
        {
            return true;
        }
    }
    public bool CheckDmgText(Object data)
    {
        DmgTextObj dto = data as DmgTextObj;
        return !dto.isShowing;
    }

    public void OppAnnounceStart(Object data)
    {
        LivingObject invokingObj = data as LivingObject;

        string shrtname = invokingObj.FullName;
        string[] subs = shrtname.Split(' ');
        shrtname = "";
        for (int i = 0; i < subs.Length; i++)
        {
            shrtname += subs[i];
        }
        AudioClip audio = Resources.LoadAll<AudioClip>(shrtname + "/Opp/")[0];

        if (audio)
        {
            // Debug.Log("found audio");
            // Debug.Log("found audio");
            //CreateTextEvent(this, "Opportunity Found!", "opp announce text", CheckText, TextStart);
            if (options)
            {
                if (options.sfx.volume > 0.0f)
                {
                    sfx.loadAudio(audio);
                    sfx.playSound();
                }
            }
        }

        if (oppImage)
        {
            oppImage.gameObject.SetActive(true);
            oppImage.myImage.sprite = Resources.LoadAll<Sprite>(shrtname + "/Opp/")[0];
            oppImage.StartCountDown(audio.length);
        }
    }
    public bool OppAnnounceEvent(Object data)
    {

        return !sfx.SOURCE.isPlaying;
    }

    public bool CheckOppEvent(Object data)
    {
        if (data)
        {
            return true;
        }
        if (oppEvent.caller)
        {
            return false;

        }
        return true;
    }

    public bool CheckNewSKillEvent(Object data)
    {


        if (newSkillEvent.caller)
        {
            return false;

        }
        return true;
    }

    public bool NextRoundBegin()
    {
        doubleAdjOppTiles.Clear();
        Debug.Log("At next round begin");
        NextRound();
        //player.current = turnOrder[0];
        //CreateEvent(this, turnOrder[0], "Initial Camera Event", CameraEvent);
        return true;
    }
    public bool NextTurnEvent(Object data)
    {

        //          Debug.Log(" is done with their turn, moving on ");
        //currOppList.Clear();

        if (currentState != State.PlayerTransition)
            doubleAdjOppTiles.Clear();

        if (currentState != State.EnemyTurn)
        {
            //    Debug.Log("Next turn...");
            CleanMenuStack(true, false);
            currentState = State.FreeCamera;
        }
        else
        {
            while (turnOrder.Count > 0)
            {
                turnOrder.Remove(turnOrder[0]);
            }
        }
        bool nextround = true;
        for (int i = 0; i < turnOrder.Count; i++)
        {
            if (turnOrder[i].ACTIONS > 0)
            {
                nextround = false;
                break;
            }
        }
        if (nextround == true)
        {
            //  Debug.Log("next round from end turn");
            NextRound();
            ShowWhite();

            for (int i = 0; i < turnOrder.Count; i++)
            {
                ShowSelectedTile(turnOrder[i], Common.orange);

            }

        }
        // currentObject = turnOrder[0];

        //    player.current = turnOrder[0];
        //CleanMenuStack();

        // CreateEvent(this, turnOrder[0], "Next turn Camera Event", CameraEvent);
        return true;
    }
    public bool BufferedReturnEvent(Object data)
    {
        returnState();
        return true;
    }
    public bool BufferedCleanEvent(Object data)
    {
        Debug.Log("buffered clean event");
        CleanMenuStack(true);
        return true;
    }
    public void StackSkills()
    {


        if (stackManager)
        {
            menuStackEntry skillsMain = stackManager.GetSkillStack();
            skillsMain.index = invManager.currentIndex;
            enterState(skillsMain);
            menuManager.ShowSkillCanvas();
        }
        else
        {
            Debug.Log("No stack manager");
        }

    }

    public void StackInventory()
    {

        if (stackManager)
        {
            menuStackEntry inventoryMain = stackManager.GetInventoryStack();
            inventoryMain.index = invManager.currentIndex;
            enterState(inventoryMain);
            menuManager.ShowInventoryCanvas();
        }
        else
        {
            Debug.Log("No stack manager");
        }

    }

    public void StackOptions()
    {

        if (stackManager)
        {
            if (currentState != State.ChangeOptions)
            {

                menuStackEntry playerOptions = stackManager.GetOptionsStack();

                playerOptions.index = invManager.currentIndex;
                enterState(playerOptions);
                menuManager.ShowOptions();
            }
            else
            {
                if (menuStack.Count == 0)
                {
                    menuManager.ShowNone();
                    currentState = State.FreeCamera;
                }
                else
                {
                    returnState();

                }
            }
        }
        else
        {
            Debug.Log("No stack manager");
        }

    }
    public void StackDetails()
    {

        if (stackManager)
        {
            if (currentState != State.CheckDetails)
            {

                menuStackEntry playerDetails = stackManager.GetDetailStack();

                playerDetails.index = invManager.currentIndex;
                enterState(playerDetails);
                menuManager.ShowDetails();
                if (detailsScreen)
                    detailsScreen.updateDetails();

            }
            else
            {
                if (menuStack.Count == 0)
                {
                    menuManager.dontShowDetails();
                    currentState = State.FreeCamera;
                }
                else
                {
                    returnState();
                    menuManager.dontShowDetails();


                }
            }
        }
        else
        {
            Debug.Log("No stack manager");
        }

    }
    public void StackOppSelection()
    {

        if (stackManager)
        {
            menuStackEntry oppSelection = stackManager.GetOppSelectionStack();

            oppSelection.index = invManager.currentIndex;
            enterState(oppSelection);
            menuManager.ShowNone();
        }
        else
        {
            Debug.Log("No stack manager");
        }

    }

    public void StackActSelection()
    {

        if (stackManager)
        {
            menuStackEntry actSelection = stackManager.GetActStack();

            actSelection.index = invManager.currentIndex;
            enterState(actSelection);
            menuManager.ShowActCanvas();
        }
        else
        {
            Debug.Log("No stack manager");
        }

    }

    public void StackCMDSelection()
    {

        if (stackManager)
        {
            menuStackEntry cmdSelection = stackManager.GetCmdStack();

            cmdSelection.index = invManager.currentIndex;
            enterState(cmdSelection);
            menuManager.ShowCommandCanvas();
        }
        else
        {
            Debug.Log("No stack manager");
        }

    }

    public void StackNewSelection(State newState, currentMenu current)
    {

        if (stackManager)
        {
            menuStackEntry topEntry = stackManager.GetTopStack();

            topEntry.index = invManager.currentIndex;
            topEntry.state = newState;
            topEntry.menu = current;
            enterState(topEntry);
            menuManager.ShowNone();
        }
        else
        {
            Debug.Log("No stack manager");
        }

    }
    public void GotoSpecialEquip()
    {
        int ItemIndex = 0;
        int extraIndex = 0;
        LivingObject living = newSkillEvent.caller as LivingObject;
        SkillScript skill = newSkillEvent.data as SkillScript;
        if (!skill)
            return;
        if (skill.GetType() == typeof(CommandSkill))
        {
            ItemIndex = 5;
            extraIndex = 0;
        }
        else if (skill.GetType() == typeof(PassiveSkill))
        {
            ItemIndex = 8;
            extraIndex = 1;
        }
        else if (skill.GetType() == typeof(AutoSkill))
        {
            ItemIndex = 9;
            extraIndex = 2;
        }
        else if (skill.GetType() == typeof(OppSkill))
        {
            ItemIndex = 10;
            extraIndex = 3;
        }
        StackNewSelection(State.PlayerEquippingSkills, currentMenu.none);
        menuManager.ShowItemCanvas(ItemIndex, living);
        menuManager.ShowExtraCanvas(extraIndex, living);


    }
    private void ChangeSkillPrompt(string objName, SkillScript skill)
    {
        if (prompt)
        {
            if (prompt.text)
            {

                if (!skill)
                    return;
                if (skill.GetType() == typeof(CommandSkill))
                {
                    prompt.text.text = objName + " learned a new Command Skill. Go to equip skill?";
                }
                else if (skill.GetType() == typeof(PassiveSkill))
                {
                    prompt.text.text = objName + " learned a new Passive Skill. Go to equip skill?";
                }
                else if (skill.GetType() == typeof(AutoSkill))
                {
                    prompt.text.text = objName + " learned a new Auto Skill. Go to equip skill?";
                }
                else if (skill.GetType() == typeof(OppSkill))
                {
                    prompt.text.text = objName + " learned a new Opportunity Skill. Go to equip skill?";
                }
            }
        }
    }
}
