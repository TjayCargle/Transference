﻿using System.Collections;
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

    public AudioClip[] sfxClips;

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
    public FlavorTextImg flavor;
    TextEventManager textManager;
    Expbar expbar;
    public OptionsManager options;
    MenuStackManager stackManager;
    Color orange = new Color(1.0f, 0.369f, 0.0f);
    public Color pink = new Color(1, 0.678f, 0.925f);
    Color red = new Color(0.693f, 0.0f, 0.230f);
    // Use this for initialization
    public void Setup()
    {
        if (!isSetup)
        {

            menuManager = GetComponent<MenuManager>();
            invManager = GetComponent<InventoryMangager>();
            eventManager = GetComponent<EventManager>();
            textManager = GetComponent<TextEventManager>();

            options = GameObject.FindObjectOfType<OptionsManager>();
            if (options)
                options.gameObject.SetActive(false);
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
            currentState = State.EnemyTurn;
            // Invoke("NextRoundBegin", 0.1f);
            //if (myCamera)
            NextRound();
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
                ShowSelectedTile(turnOrder[i], orange);

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
        if (prevState != State.PlayerOppSelecting && currentState != State.PlayerOppSelecting)
        {
            currOppList.Clear();
        }

        menuStack.Add(entry);
        invManager.currentIndex = 0;
        invManager.Validate("manager, enter state");
        if (currentState == State.PlayerMove)
        {
            ShowGridObjectMoveArea(currentObject);
        }
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
    public void returnState()
    {
        // Debug.Log("returnin");
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
            tempObject.transform.position = currentObject.transform.position;
            tempObject.GetComponent<GridObject>().currentTile = currentObject.currentTile;
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

    }
    public void CleanMenuStack(bool toCam = false)
    {
      
        while (menuStack.Count > 0)
        {
            returnState();
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


    void LateUpdate()
    {

        menuStackCount = menuStack.Count;

        if (currentObject)
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
                        RaycastHit hit = new RaycastHit();
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit))
                        {
                            Vector3 w = hit.point;
                            w.x = Mathf.Round(w.x);
                            w.y = Mathf.Round(w.y);
                            w.z = Mathf.Round(w.z);

                            if (GetTileIndex(w) > 0)
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
                                        ComfirmMenuAction(player.current);
                                        player.current.TakeAction();
                                    }
                                    else
                                    {
                                        tempObject.transform.position = hitTile.transform.position;
                                        tempObject.GetComponent<GridObject>().currentTile = hitTile;
                                        float tempX = hitTile.transform.position.x;
                                        float tempY = hitTile.transform.position.z;

                                        float objX = player.current.currentTile.transform.position.x;
                                        float objY = player.current.currentTile.transform.position.z;


                                        xDist = Mathf.Abs(tempX - objX);
                                        yDist = Mathf.Abs(tempY - objY);
                                        if (xDist + yDist <= player.current.MOVE_DIST)
                                        {
                                            player.current.transform.position = hitTile.transform.position + new Vector3(0, 0.5f);
                                            myCamera.currentTile = hitTile;
                                        }




                                    }
                                }
                            }
                        }
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

                                if (GetTileIndex(w) > 0)
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
                                            hitTile.myColor = red;

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
                                    currentAttackList[i].myColor = red;
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

                                if (GetTileIndex(w) > 0)
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
                            oppEvent.caller = null;
                            CleanMenuStack();
                        }
                        if (Input.GetMouseButtonDown(1))
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
                            if (sfx)
                            {
                                if (sfxClips.Length > 1)
                                {
                                    sfx.loadAudio(sfxClips[2]);
                                    sfx.playSound();
                                }
                            }
                            player.useOppAction(oppObj);
                            returnState();
                        }


                    }
                    break;
                case State.PlayerEquipping:

                    break;
                case State.PlayerWait:
                    break;
                case State.FreeCamera:
                    if (Input.GetMouseButtonDown(1))
                    {
                        menuStackEntry entry = new menuStackEntry();
                        entry.state = State.ChangeOptions;
                        entry.index = invManager.currentIndex;
                        entry.menu = currentMenu.command;
                        enterState(entry);
                        menuManager.ShowOptions();
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

                            if (GetTileIndex(w) > 0)
                            {
                                bool alreadySelected = false;
                                tempObject.transform.position = w;
                                ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));
                                for (int i = 0; i < turnOrder.Count; i++)
                                {
                                    if (turnOrder[i] == myCamera.infoObject)
                                    {
                                        if (turnOrder[i].ACTIONS > 0)
                                        {

                                            if (tempObject.GetComponent<GridObject>().currentTile == turnOrder[i].currentTile)
                                            {
                                                alreadySelected = true;
                                                break;

                                            }
                                        }
                                    }

                                }
                                if (alreadySelected)
                                {
                                    currentObject = myCamera.infoObject;
                                    player.current = currentObject.GetComponent<LivingObject>();

                                    enterState(defaultEntry);
                                    menuManager.ShowCommandCanvas();
                                    CreateEvent(this, currentObject, "Select Camera Event", CameraEvent);
                                }
                                else
                                {

                                    ShowWhite();
                                    for (int i = 0; i < turnOrder.Count; i++)
                                    {
                                        ShowSelectedTile(turnOrder[i], orange);

                                    }
                                    if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                                        ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                                    ShowSelectedTile(tempObject.GetComponent<GridObject>());
                                }
                            }
                        }
                        //Vector3 mousepos = Input.mousePosition;
                        //mousepos.z = -myCamera.z;


                        //Vector3 w = Camera.main.ScreenToWorldPoint(mousepos);
                        //Debug.Log("Mouse World Pos: " + w);
                        //w.x = Mathf.RoundToInt(w.x);
                        //w.z = Mathf.RoundToInt(w.y);
                        //w.y = 0;
                        //Debug.Log("Mouse ROUND Pos: " + w);
                        //if (GetTileIndex(w) > 0)
                        //{
                        //    Debug.Log("tile found: " + GetTileAtIndex(GetTileIndex(w)));
                        //    ShowWhite();
                        //    tempObject.transform.position = w;
                        //    ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));
                        //    if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                        //        ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                        //    ShowSelectedTile(tempObject.GetComponent<GridObject>());
                        //}

                    }
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        ShowWhite();
                        for (int i = 0; i < turnOrder.Count; i++)
                        {
                            ShowSelectedTile(turnOrder[i], orange);

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
                            ShowSelectedTile(turnOrder[i], orange);

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
                            ShowSelectedTile(turnOrder[i], orange);

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
                            ShowSelectedTile(turnOrder[i], orange);

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
                        }
                        else
                        {
                            descriptionState = descState.stats;
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

                        menuStackEntry entry = new menuStackEntry();
                        entry.state = State.ChangeOptions;
                        entry.index = invManager.currentIndex;
                        entry.menu = currentMenu.command;
                        enterState(entry);
                        menuManager.ShowOptions();
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
                        oppEvent.caller = null;
                        CreateEvent(this, null, "return state event", BufferedReturnEvent);
                        CreateEvent(this, null, "return state event", BufferedReturnEvent);
                        CreateEvent(this, null, "return state event", BufferedReturnEvent);
                    }
                    if (Input.GetMouseButtonDown(1))
                    {
                        CancelMenuAction(oppObj);
                        oppEvent.caller = null;
                        CreateEvent(this, null, "return state event", BufferedReturnEvent);
                        CreateEvent(this, null, "return state event", BufferedReturnEvent);
                        CreateEvent(this, null, "return state event", BufferedReturnEvent);
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
                default:
                    break;
            }

        }

    }
    public void NextRound()
    {
        Debug.Log("next");
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
                            turnOrder.Add(livingObjects[i]);
                        }
                    }
                    else
                    {
                        if (!livingObjects[i].IsEnenmy)
                        {
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
                    CreateEvent(this, anEnemy, "Enemy Event", EnemyEvent, null, -1, SetEnemyEvent);
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
        CreateEvent(this, null, "Next turn event from " + invokingObj, NextTurnEvent);

    }
    public void showAttackableTiles()
    {
        for (int i = 0; i < attackableTiles.Count; i++)
        {
            for (int j = 0; j < attackableTiles[i].Count; j++)
            {
                attackableTiles[i][j].myColor = pink;
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
                    temp.GetComponent<TileScript>().myColor = pink;
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

    public void ShowSelectedTile(GridObject obj, Color color)
    {
        TileScript theTile = GetTile(obj);
        theTile.myColor = color;
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
                    temp.GetComponent<TileScript>().myColor = pink;
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
            TileScript atile = tileMap[TileIndex].GetComponent<TileScript>();
            if (IsTileOccupied(atile) == true)
            {
                GridObject gridObject = GetObjectAtTile(atile).GetComponent<LivingObject>();
                if (gridObject.GetComponent<LivingObject>())
                {
                    if (gridObject.GetComponent<LivingObject>().IsEnenmy != obj.GetComponent<LivingObject>().IsEnenmy)
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
                    // List<TileScript> mytile = new List<TileScript>();

                    tiles.Add(GetTileAtIndex(checkIndex));

                    returnList.Add(tiles);
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

        if (tempTiles.Count > 0)
        {
            for (int i = 0; i < tempTiles.Count; i++) //list of lists
            {
                for (int j = 0; j < tempTiles[i].Count; j++) //indivisual list
                {

                    tempTiles[i][j].myColor = Color.red;
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
            Debug.Log("Selecting menu item from obj :" + invokingObject.FullName);
            invManager.selectedMenuItem.ApplyAction(invokingObject);
        }

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
            Debug.Log("Selecting menu item from sselected item");
            selectedItem.ApplyAction(player.current);
        }

    }
    public void HoverMenuItem(MenuItem selectedItem)
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

            invManager.HoverSelect(selectedItem, selectedItem.transform.parent.gameObject);
            //invManager.currentIndex = selectedItem.transform.GetSiblingIndex();
            // invManager.ForceSelect();
            // invManager.Validate("Manager hover");

        }

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
                break;
            }


        }
        return false;
    }
    public void CancelMenuAction(GridObject invokingObject)
    {

        Vector3 resetPos = invokingObject.currentTile.transform.position;
        resetPos.y = 0.5f;
        invokingObject.transform.position = resetPos;
        Debug.Log("Default");
        if (attackableTiles != null)
        {
            attackableTiles.Clear();
            ShowWhite();
        }
        CreateEvent(this, null, "return state event", BufferedReturnEvent);
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
            DmgReaction react = CalcDamage(attackingObject, dmgObject, skill.ELEMENT, skill.ETYPE, (int)skill.DAMAGE, alteration);
            //if (skill.EFFECT != SideEffect.none)
            //{
            //    Debug.Log("Applying effect");
            //    ApplyEffect(dmgObject, skill.EFFECT);
            //}
            return react;
        }
    }

    public DmgReaction CalcDamage(AtkConatiner conatiner)
    {
        // Debug.Log("Calc 2");
        if (conatiner.command)
        {
            DmgReaction react = CalcDamage(conatiner.attackingObject, conatiner.dmgObject, conatiner.command, conatiner.alteration);
            //if (react.reaction < Reaction.cripple)
            //{
            //    if (conatiner.command.EFFECT != SideEffect.none)
            //    {
            //        ApplyEffect(conatiner.dmgObject, conatiner.command.EFFECT);
            //    }

            //}
            return react;
        }
        else
            return CalcDamage(conatiner.attackingObject, conatiner.dmgObject, conatiner.attackingObject.WEAPON, conatiner.alteration);

    }


    public DmgReaction CalcDamage(LivingObject attackingObject, LivingObject dmgObject, WeaponEquip weapon, Reaction alteration = Reaction.none)
    {
        // Debug.Log("Calc 3");
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
        //  Debug.Log("Applying dmg: " + react.damage);
        int gtype = 0;
        switch (react.reaction)
        {
            case Reaction.none:
                DamageLivingObject(target, react.damage);

                break;
            case Reaction.buff:
                gtype = 1;
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
            case Reaction.reduceAtk:
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
                DamageLivingObject(target, -react.damage);
                CreateTextEvent(this, "" + attackingObject.FullName + " attack did weakening damage", "enemy atk", CheckText, TextStart);
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
                    if (gtype != 1) // not a buff;
                    {

                        float colorVal = 0.0f;
                        if (react.damage > 0)
                        {
                            colorVal = (float)react.damage * 1.5f / 100.0f;
                        }
                        CreateDmgTextEvent(react.damage.ToString(), new Color(Mathf.Min(1, colorVal), 0, 0), target);

                    }
                }
                // CreateEvent(this, dto, "DmgText request: " + dmgRequest + "", CheckDmgText, dto.StartCountDown, 0);
                if (options.battleAnims)
                {


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
                    gao.type = gtype;
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
            killedEnemy = true;
            if (target.IsEnenmy)
            {
                gridObjects.Remove(target);
                target.gameObject.SetActive(false);
                target.currentTile.isOccupied = false;
                Destroy(target.gameObject);
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
        if (currentState != State.EnemyTurn)
        {
            DetermineExp(attackingObject, target, killedEnemy);
            if (options)
            {
                if (options.showExp)
                {
                    CreateEvent(this, null, "Exp event", UpdateExpBar, ShowExpBar, 2);

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
                        for (int i = 0; i < targetIndicies.Count; i++)
                        {
                            GridObject potentialTarget = GetObjectAtTile(currentAttackList[targetIndicies[i]]);
                            if (potentialTarget.GetComponent<LivingObject>())
                            {
                                LivingObject target = potentialTarget.GetComponent<LivingObject>();
                                bool acceptable = false;
                                if (skill.ELEMENT == Element.Buff)
                                {
                                    if (target.IsEnenmy == invokingObject.IsEnenmy)
                                    {
                                        acceptable = true;
                                    }
                                }
                                else
                                {
                                    if (target.IsEnenmy != invokingObject.IsEnenmy)
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
                                    CreateEvent(this, conatiner, "Skill use event", AttackEvent);
                                    // }
                                }

                            }


                        }

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

                            DmgReaction react;
                            Reaction atkReaction = Reaction.none;
                            if (target.IsEnenmy != invokingObject.IsEnenmy)
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
                                            if (chance < result)
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

    public void ApplyEffect(LivingObject target, SideEffect effect)
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
                    if (target.SSTATUS == SecondaryStatus.normal)
                    {
                        CreateTextEvent(this, target.FullName + " has been inflicted with poison", "auto atk", CheckText, TextStart);

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
        }
    }
    public bool AttackEvent(Object data)
    {
        loadTargets();
        AtkConatiner container = data as AtkConatiner;
        CreateTextEvent(this, "" + container.attackingObject.FullName + " used " + container.command.NAME, "skill atk", CheckText, TextStart);
        CommandSkill skill = container.command;
        float modification = 1.0f;
        if (skill.ETYPE == EType.magical)
            modification = container.attackingObject.STATS.SPCHANGE;
        if (skill.ETYPE == EType.physical)
            modification = container.attackingObject.STATS.FTCHANGE;
        bool newSkill = skill.UseSkill(container.attackingObject, modification);
        if (newSkill == true)
        {
            CreateTextEvent(this, "" + container.attackingObject.FullName + " learned a new skill. Equip in inventory ", "new skill event", CheckText, TextStart);
        }
        DmgReaction react = CalcDamage(container);
        for (int k = 0; k < skill.HITS; k++)
        {
            ApplyReaction(container.attackingObject, container.dmgObject, react);

        }
        if (container.command.EFFECT != SideEffect.none)
        {
            ApplyEffect(container.dmgObject, container.command.EFFECT);
        }
        currentState = State.PlayerTransition;
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
                else
                {
                    CreateEvent(this, null, "Show Command", player.ShowCmd);
                }

            }
            else
            {
                CreateEvent(this, null, "Show Command", player.ShowCmd);
            }
        }
        else
        {
            CreateEvent(this, null, "Show Command", player.ShowCmd);
        }
        return true;
    }
    public bool WeaponAttackEvent(Object data)
    {
        loadTargets();
        AtkConatiner container = data as AtkConatiner;
        CreateTextEvent(this, "" + container.attackingObject.FullName + " used their " + container.attackingObject.WEAPON.NAME + " attack", "weapon atk", CheckText, TextStart);
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
                    // Debug.Log();
                    CreateTextEvent(this, "" + "Auto skill : " + auto.NAME + " has gone off", "auto skill ", CheckText, TextStart);

                    //Debug.Log("chance= " + auto.CHANCE);
                    //Debug.Log("result= " + result);
                    auto.Activate(react.damage);
                }
            }
        }
        ApplyReaction(container.attackingObject, container.dmgObject, react);
        currentState = State.PlayerTransition;
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
                else
                {
                    CreateEvent(this, null, "Show Command", player.ShowCmd);
                }
            }
            else
            {
                CreateEvent(this, null, "Show Command", player.ShowCmd);
            }

        }
        else
        {
            CreateEvent(this, null, "Show Command", player.ShowCmd);
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
            Debug.Log("found audio");
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

        if (oppEvent.caller)
        {
            return false;

        }
        return true;
    }
    public bool NextRoundBegin()
    {
        doubleAdjOppTiles.Clear();
        NextRound();
        //player.current = turnOrder[0];
        //CreateEvent(this, turnOrder[0], "Initial Camera Event", CameraEvent);
        return true;
    }
    public bool NextTurnEvent(Object data)
    {

        currOppList.Clear();
        doubleAdjOppTiles.Clear();
        //  Debug.Log(invokingObj + " is done with their turn, moving on ");
        if (currentObject.GetComponent<BuffScript>())
        {
            BuffScript[] buffs = currentObject.GetComponents<BuffScript>();

            for (int i = 0; i < buffs.Length; i++)
            {
                buffs[i].UpdateCount(currentObject.GetComponent<LivingObject>());
            }
        }
        if (currentObject.GetComponent<EffectScript>())
        {
            currentObject.GetComponent<EffectScript>().ApplyReaction(this, currentObject.GetComponent<LivingObject>());
        }
        if (currentObject.GetComponent<SecondStatusScript>())
        {
            currentObject.GetComponent<SecondStatusScript>().ReduceCount(this, currentObject.GetComponent<LivingObject>());
        }
        if (turnOrder.Count > 0)
            turnOrder.Remove(currentObject.GetComponent<LivingObject>());
        if (currentState != State.EnemyTurn)
        {
            Debug.Log("yosef");
            CleanMenuStack();
            currentState = State.FreeCamera;

        }
        if (turnOrder.Count <= 0)
        {
            Debug.Log("nxtrond");
            NextRound();
        }
        // currentObject = turnOrder[0];

        //    player.current = turnOrder[0];
        //CleanMenuStack();

        int acts = Mathf.RoundToInt(turnOrder[0].SPEED / 10);

        if (turnOrder[0].GENERATED < 0)
        {
            if (-1 * turnOrder[0].GENERATED >= acts)
            {
                acts = 2;
            }
        }
        else
        {

            acts += turnOrder[0].GENERATED;
            acts += 1;
        }

        if (turnOrder[0].ACTIONS <= 0)
        {
            turnOrder[0].ACTIONS = acts;

        }

        turnOrder[0].GENERATED = 0;

        // CreateEvent(this, turnOrder[0], "Next turn Camera Event", CameraEvent);
        return true;
    }
    public bool BufferedReturnEvent(Object data)
    {
        returnState();
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
            menuStackEntry playerOptions = stackManager.GetOppOptionsStack();

            playerOptions.index = invManager.currentIndex;
            enterState(playerOptions);
            menuManager.ShowOptions();
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
}
