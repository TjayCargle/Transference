﻿using Doublsb.Dialog;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScript : EventRunner
{

    public GameObject Tile;
    public List<TileScript> tileMap;
    public List<GridObject> gridObjects;
    public List<LivingObject> turnOrder;
    public List<TileScript> currentAttackList;
    public List<List<TileScript>> attackableTiles;
    public PlayerController player;

    public int MapWidth = 0;
    public int MapHeight = 0;
    public State currentState = State.PlayerInput;
    public State prevState = State.PlayerInput;
    public MenuItem currentMenuitem = null;
    public int itemMenuitem = 0;

    public CameraScript myCamera;
    public bool isSetup = false;
    public GridObject currentObject;
    public GameObject tempObject;// = new GridObject();
    [SerializeField]
    MenuItem[] commandItems;
    //    public MenuItem hoveredMenuItem;
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
    private int attackIndex = 0;
    public SFXManager sfx;
    public VoicesManager voiceManager;
    public ImgObj oppImage;
    public NewSkillPrompt prompt;
    public AudioClip[] sfxClips;
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
    public ShopScreen shopScreen;
    public GridEvent newSkillEvent;
    GameObject tileParent;
    TileManager tileManager;
    EnemyManager enemyManager;
    ObjManager objManager;
    HazardManager hazardManager;
    public Texture doorTexture;
    public Texture shopTexture;
    DatabaseManager database;
    ConditionalDisplay[] displays;
    public PotentialSlider potential;
    public PhaseImg phaseImage;
    [SerializeField]
    public StatusIconManager iconManager;
    public Material ShadowMaterial;
    private List<UsableScript> shopItems = new List<UsableScript>();
    public MapDetail currentMap;
    private List<MapDetail> visitedMaps = new List<MapDetail>();
    private float timer = 0.0f;
    [SerializeField]
    public int defaultSceneEntry = 4;
    public GameObject PlayerObject;
    private SceneContainer currentScene;
    [SerializeField]
    TalkPanel talkPanel;
    bool nextRoundCalled = false;
    public BattleLog log;
    public List<GridObject> opptargets = new List<GridObject>();
    public TurnImageManager turnImgManger;
    public List<LivingObject> liveEnemies = new List<LivingObject>();
    public TextObj currentRoomName;
    HackingCtrl hackingGame;
    public HazardScript adjacentGlyph = null;
    public InteractableObject adjacentInteractable = null;
    private bool movingObj = false;
    private List<LivingObject> markedEnemies = new List<LivingObject>();
    [SerializeField]
    ImgObj eventImage;
    public bool showEnemyRanges = false;
    RaycastHit hit = new RaycastHit();
    public TextObjHolder textHolder;
    GridObject tempGridObj = null;
    public Objective currentObjective = null;
    public Tutorial currentTutorial = new Tutorial();
    public bool inTutorialMenu = false;
    public string currentObjectiveString = "Explore the mansion";
    public TextObjHolder[] objectivesAndNotes;
    public CutsceneManager cutscene;

    public StorySection lastStory = StorySection.JaxSaveSlotPrologue;

    public List<UsableScript> SHOPLIST
    {
        get { return shopItems; }
        set { shopItems = value; }
    }

    public int TwoToOneD(int y, int width, int x)
    {
        return (y * width) + x;
    }
    // Use this for initialization

    public void Setup()
    {
        if (!isSetup)
        {
            if (Application.targetFrameRate != 60)
            {
                Application.targetFrameRate = 60;
            }
            database = GameObject.FindObjectOfType<DatabaseManager>();
            textHolder = GetComponent<TextObjHolder>();
            if (textHolder == null)
            {
                textHolder = gameObject.AddComponent<TextObjHolder>();
            }
            menuManager = GetComponent<MenuManager>();
            invManager = GetComponent<InventoryMangager>();
            eventManager = GetComponent<EventManager>();
            textManager = GetComponent<TextEventManager>();
            tileManager = GetComponent<TileManager>();
            enemyManager = GetComponent<EnemyManager>();
            hazardManager = GetComponent<HazardManager>();
            objManager = GetComponent<ObjManager>();
            cutscene = GetComponent<CutsceneManager>();
            displays = GameObject.FindObjectsOfType<ConditionalDisplay>();
            if (potential == null)
            {
                potential = GameObject.FindObjectOfType<PotentialSlider>();

            }
            iconManager = GameObject.FindObjectOfType<StatusIconManager>();
            stackManager = GameObject.FindObjectOfType<MenuStackManager>();
            options = GameObject.FindObjectOfType<OptionsManager>();
            detailsScreen = GameObject.FindObjectOfType<DetailsScreen>();
            shopScreen = GameObject.FindObjectOfType<ShopScreen>();
            log = GameObject.FindObjectOfType<BattleLog>();
            hackingGame = GameObject.FindObjectOfType<HackingCtrl>();
            NewSkillPrompt aprompt = GameObject.FindObjectOfType<NewSkillPrompt>();
            currentObjective = ScriptableObject.CreateInstance<Objective>();
            currentTutorial.steps = new List<tutorialStep>();
            currentTutorial.clarifications = new List<int>();



            if (eventImage)
            {
                eventImage.Setup();
            }
            if (aprompt)
            {
                prompt = aprompt;
                prompt.Setup();
            }
            if (log)
            {
                log.Setup();
            }
            if (!turnImgManger)
            {
                turnImgManger = gameObject.GetComponent<TurnImageManager>();
            }
            turnImgManger.manager = this;
            turnImgManger.Setup();
            isSetup = true;
            if (log)
            {
                log.transform.gameObject.SetActive(false);
            }
            if (!enemyManager)
            {
                enemyManager = gameObject.AddComponent<EnemyManager>();
            }
            if (!objManager)
            {
                objManager = gameObject.AddComponent<ObjManager>();
            }

            if (!hazardManager)
            {
                hazardManager = gameObject.AddComponent<HazardManager>();
            }

            if (!tileManager)
            {
                tileManager = gameObject.AddComponent<TileManager>();
            }

            if (!stackManager)
            {
                stackManager = gameObject.AddComponent<MenuStackManager>();
            }

            if (!eventManager)
            {
                eventManager = gameObject.AddComponent<EventManager>();
            }

            if (!hackingGame && menuManager)
            {
                if (menuManager.hackingCanvas)
                    hackingGame = menuManager.hackingCanvas.GetComponent<HackingCtrl>();
            }

            invManager.Setup();
            enemyManager.Setup();
            hazardManager.Setup();
            eventManager.Setup();
            tileManager.Setup();
            objManager.Setup();
            hackingGame.Setup();

            if (hackingGame)
                hackingGame.gameObject.SetActive(false);
            if (!options && menuManager)
            {
                if (menuManager.optionsCanvas)
                    options = menuManager.optionsCanvas.GetComponent<OptionsManager>();
            }



            if (detailsScreen)
                detailsScreen.gameObject.SetActive(false);
            if (!detailsScreen && menuManager)
            {
                if (menuManager.detailCanvas)
                    detailsScreen = menuManager.detailCanvas.GetComponent<DetailsScreen>();
            }
            if (options)
            {
                options.Setup();
                options.gameObject.SetActive(false);
                options.ForceUpdate();
            }


            if (shopScreen)
                shopScreen.gameObject.SetActive(false);
            if (!shopScreen && menuManager)
            {
                if (menuManager.shopCanvas)
                    shopScreen = menuManager.shopCanvas.GetComponent<ShopScreen>();
            }
            sfx = GameObject.FindObjectOfType<SFXManager>();
            voiceManager = GameObject.FindObjectOfType<VoicesManager>();
            expbar = GameObject.FindObjectOfType<Expbar>();
            if (expbar)
            {
                expbar.manager = this;
            }
            if (!flavor)
            {

                flavor = FindObjectOfType<FlavorTextImg>();
            }

            if (textManager && flavor)
            {
                textManager.flavor = flavor;
            }

            menuStack = new List<menuStackEntry>();
            defaultEntry = new menuStackEntry();
            defaultEntry.state = State.PlayerInput;
            defaultEntry.index = 0;
            oppEvent = new GridEvent();
            newSkillEvent = new GridEvent();
            // menuStack.Add(defaultEntry);
            dmgText = new List<DmgTextObj>();
            targets = new List<LivingObject>();
            tileParent = new GameObject();
            tileParent.name = "Tile Parent";
            tileMap = null;// new GameObject[MapWidth * MapHeight];



            if (GameObject.FindObjectOfType<CameraScript>())
            {
                myCamera = GameObject.FindObjectOfType<CameraScript>();
                myCamera.Setup();
            }
            commandItems = GameObject.FindObjectsOfType<MenuItem>();
            player = GameObject.FindObjectOfType<PlayerController>();
            tempObject = new GameObject();
            tempObject.AddComponent<TempObject>();
            tempGridObj = tempObject.GetComponent<GridObject>();
            tempObject.name = "TempObj";
            tempGridObj.NAME = "TEMPY";
            tempGridObj.MOVE_DIST = 10000;
            tempObject.transform.position = Vector3.zero;
            menuManager.ShowNone();

            //  currentState = State.FreeCamera;
            enterStateTransition();
            database.Setup();
            if (PlayerPrefs.HasKey("defaultSceneEntry"))
            {
                defaultSceneEntry = PlayerPrefs.GetInt("defaultSceneEntry");
            }
            Common.summonedJax = false;
            Common.summonedZeffron = false;

            if (PlayerPrefs.HasKey("continue"))
            {
                int newOrContune = PlayerPrefs.GetInt("continue");

                if (newOrContune == 1)
                {
                    LoadGameAndScene();
                }
                else
                {
                    LoadDefaultScene();
                }
            }

            updateConditionals();

            StartCoroutine(performChecks());
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
    public void stackLog()
    {
        if (log)
        {
            log.transform.gameObject.SetActive(!log.transform.gameObject.activeInHierarchy);

        }
    }
    float tick = 0;
    public void prevCharacter()
    {

        {
            if (GetState() != State.EnemyTurn && GetState() != State.HazardTurn && GetState() != State.PlayerAttacking && GetState() != State.PlayerUsingItems)
            {
                if (player.current)
                {
                    if (turnOrder.Count > 1)
                    {
                        int liveActioners = 0;
                        int oppActioners = 0;
                        int order = 0;
                        for (int i = 0; i < turnOrder.Count; i++)
                        {
                            if (player.current == turnOrder[i])
                            {
                                order = i;

                            }
                            if (turnOrder[i].ACTIONS > 0)
                            {
                                liveActioners++;
                            }
                            if (turnOrder[i].OPP_SLOTS.SKILLS.Count > 0)
                            {
                                oppActioners++;
                            }
                        }
                        order--;
                        if (order < 0)
                            order = turnOrder.Count - 1;
                        if (turnOrder[order].ACTIONS > 0)
                        {
                            if (GetState() == State.PlayerOppOptions)
                            {
                                if (turnOrder[order].OPP_SLOTS.SKILLS.Count > 0)
                                {
                                    oppObj = player.current;
                                    currentObject = turnOrder[order];
                                    turnImgManger.UpdateSelection(order);
                                }
                                else if (oppActioners > 1)
                                {
                                    prevCharacter();
                                }
                            }
                            else
                            {
                                player.current = turnOrder[order];
                                myCamera.currentTile = player.current.currentTile;
                                turnImgManger.UpdateSelection(order);

                            }

                            showCurrentState();
                        }
                        else if (liveActioners > 1)
                        {
                            prevCharacter();
                        }
                    }
                }
                else
                {
                    if (turnOrder.Count > 0)
                    {
                        turnImgManger.UpdateSelection(0);
                        myCamera.currentTile = turnOrder[0].currentTile;
                        myCamera.infoObject = turnOrder[0];
                        currentObject = myCamera.infoObject;
                        player.current = turnOrder[0];
                        if (GetState() == State.FreeCamera)
                        {
                            enterState(defaultEntry);
                            menuManager.ShowCommandCanvas();
                            CreateEvent(this, currentObject, "Select Camera Event", CameraEvent);
                        }
                    }
                }
            }
        }
    }
    public void nextCharacter()
    {

        {
            if (GetState() != State.EnemyTurn && GetState() != State.HazardTurn && GetState() != State.PlayerAttacking && GetState() != State.PlayerUsingItems)
            {
                if (player.current)
                {
                    if (turnOrder.Count > 1)
                    {
                        int liveActioners = 0;
                        int order = 0;
                        for (int i = 0; i < turnOrder.Count; i++)
                        {
                            if (player.current == turnOrder[i])
                            {
                                order = i;
                            }
                            if (turnOrder[i].ACTIONS > 0)
                            {
                                liveActioners++;

                            }
                        }
                        order++;
                        if (order >= turnOrder.Count)
                            order = 0;
                        if (turnOrder[order].ACTIONS > 0)
                        {
                            if (GetState() == State.PlayerOppOptions)
                            {

                                oppObj = player.current;
                            }
                            turnImgManger.UpdateSelection(order);
                            currentObject = turnOrder[order];
                            player.current = turnOrder[order];
                            myCamera.currentTile = player.current.currentTile;

                            showCurrentState();
                        }
                        else if (liveActioners > 1)
                        {
                            nextCharacter();
                        }
                    }
                }
                else
                {
                    if (turnOrder.Count > 0)
                    {
                        turnImgManger.UpdateSelection(0);
                        myCamera.currentTile = turnOrder[0].currentTile;
                        myCamera.infoObject = turnOrder[0];
                        currentObject = myCamera.infoObject;
                        player.current = turnOrder[0];
                        if (GetState() == State.FreeCamera)
                        {
                            enterState(defaultEntry);
                            menuManager.ShowCommandCanvas();
                            CreateEvent(this, currentObject, "Select Camera Event", CameraEvent);
                        }
                    }
                }
            }
        }
    }

    public void selectCharacter(int anIndx)
    {

        {
            if (GetState() != State.EnemyTurn && GetState() != State.HazardTurn && GetState() != State.PlayerAttacking && GetState() != State.PlayerUsingItems)
            {

                if (turnOrder.Count > anIndx)
                {

                    if (turnOrder[anIndx].ACTIONS > 0)
                    {
                        if (GetState() == State.PlayerOppOptions)
                        {
                            if (turnOrder[anIndx].ACTIONS <= 0)
                                return;
                            oppObj = player.current;
                        }
                        turnImgManger.UpdateSelection(anIndx);
                        currentObject = turnOrder[anIndx];
                        player.current = turnOrder[anIndx];
                        myCamera.currentTile = player.current.currentTile;

                        if (GetState() == State.FreeCamera)
                        {
                            enterState(defaultEntry);
                            menuManager.ShowCommandCanvas();
                            CreateEvent(this, currentObject, "Select Camera Event", CameraEvent);
                        }
                        else
                        {
                            showCurrentState();

                        }
                        myCamera.UpdateCamera();
                        updateConditionals();
                    }

                }


            }
        }
    }
    private bool isperforming = false;
    void LateUpdate()
    {
        if (menuStack != null)
            menuStackCount = menuStack.Count;

        if (eventManager.gridEvents.Count == 0)
        {
            if (eventManager.activeEvents == 0 && eventManager.currentEvent.caller == null)
            {

                if (turnOrder.Count == 0)
                {
                    if (GetState() == State.EnemyTurn || GetState() == State.HazardTurn)
                    {
                        NextTurn("manager safegaurd");
                    }
                }
            }
        }
        if (currentState == State.PlayerTransition)
        {
            tick++;
        }
        else
        {
            tick = 0;
        }
        if (tick >= 800)
        {
            returnState(true, true);
            tick = 0;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            prevCharacter();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            nextCharacter();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //  LoadGameAndScene();
            stackLog();

        }
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    SaveGame();
        //}
        {
            switch (currentState)
            {
                case State.GotoNewRoom:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        returnState();
                    }
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        YesPrompt();
                    }
                    break;
                case State.PlayerAllocate:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        returnState();
                    }
                    if (Input.GetMouseButtonDown(1))
                    {

                        returnState();
                    }



                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        SelectMenuItem(player.current);

                    }



                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        returnState();

                    }
                    break;

                case State.PlayerInput:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        returnState();
                    }
                    if (Input.GetMouseButtonDown(1))
                    {

                        returnState();
                    }



                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        SelectMenuItem(player.current);

                    }



                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        returnState();

                    }
                    break;

                case State.PlayerMove:
                    {
                        if (player)
                        {
                            if (player.current)
                            {
                                if (options != null)
                                {
                                    if (options.hoverSelect == true)
                                    {
                                        {


                                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                            if (Physics.Raycast(ray, out hit))
                                            {
                                                Vector3 w = hit.point;
                                                w.x = Mathf.Round(w.x);
                                                w.y = Mathf.Round(w.y);
                                                w.z = Mathf.Round(w.z);

                                                GameObject hitObj = hit.transform.gameObject;

                                                if (hitObj.GetComponent<TileScript>())
                                                {
                                                    TileScript hitTile = hitObj.GetComponent<TileScript>();

                                                    bool alreadySelected = false;

                                                    if (myCamera.currentTile == hitTile)
                                                    {
                                                        alreadySelected = true;

                                                    }

                                                    if (alreadySelected)
                                                    {

                                                    }
                                                    else
                                                    {

                                                        if (tileManager)
                                                        {
                                                            if (tileManager.currentMoveableTiles.Contains(hitTile))
                                                            {
                                                                tempObject.transform.position = hitTile.transform.position;
                                                                ComfirmMoveGridObject(tempGridObj, hitTile, true);
                                                                ShowGridObjectMoveArea(player.current);
                                                                ShowSelectedTile(tempGridObj);
                                                                myCamera.UpdateCamera();
                                                                updateConditionals();
                                                            }
                                                        }

                                                    }
                                                }

                                                if (myCamera.currentTile != myCamera.selectedTile)
                                                {
                                                    if (Vector3.Distance(myCamera.currentTile.transform.position, myCamera.selectedTile.transform.position) >= 6.0)
                                                    {
                                                        myCamera.currentTile = myCamera.selectedTile;
                                                        myCamera.UpdateCamera();
                                                    }


                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        if (Input.GetMouseButtonDown(1))
                        {
                            CancelMenuAction(player.current);
                        }
                        if (Input.GetMouseButtonDown(0))
                        {
                            MovetoMousePos(player.current);
                        }
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            CancelMenuAction(player.current);
                        }

                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            CancelMenuAction(player.current);
                        }
                        break;
                    }
                case State.PlayerAttacking:
                    {


                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            ShowWhite();
                            attackIndex++;
                            ChangeAtkTargets();
                        }
                        if (Input.GetKeyDown(KeyCode.D))
                        {
                            ShowWhite();
                            attackIndex++;
                            ChangeAtkTargets();
                        }
                        if (Input.GetKeyDown(KeyCode.S))
                        {
                            ShowWhite();
                            attackIndex--;
                            ChangeAtkTargets();
                        }
                        if (Input.GetKeyDown(KeyCode.A))
                        {
                            ShowWhite();
                            attackIndex--;
                            ChangeAtkTargets();
                        }



                        if (Input.GetKeyDown(KeyCode.Escape))
                        {

                            CancelMenuAction(player.current);
                            player.currentSkill = null;
                        }

                        if (options != null)
                        {
                            if (options.hoverSelect == true)
                            {
                                {


                                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                    if (Physics.Raycast(ray, out hit))
                                    {
                                        Vector3 w = hit.point;
                                        w.x = Mathf.Round(w.x);
                                        w.y = Mathf.Round(w.y);
                                        w.z = Mathf.Round(w.z);

                                        GameObject hitObj = hit.transform.gameObject;

                                        if (hitObj.GetComponent<TileScript>())
                                        {
                                            TileScript hitTile = hitObj.GetComponent<TileScript>();

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
                                                        if (tempGridObj.currentTile == hitTile)
                                                        {
                                                            myCamera.infoObject = GetObjectAtTile(tempGridObj.currentTile);
                                                            alreadySelected = true;
                                                            break;

                                                        }

                                                    }
                                                }

                                            }
                                            if (tempGridObj.currentTile == hitTile)
                                            {
                                                alreadySelected = true;

                                            }

                                            if (alreadySelected)
                                            {

                                            }
                                            else
                                            {

                                                if (outer >= 0 && innner >= 0)
                                                {

                                                    showAttackableTiles();
                                                    tempObject.transform.position = hitTile.transform.position;
                                                    ComfirmMoveGridObject(tempGridObj, hitTile);

                                                    myCamera.potentialDamage = 0;
                                                    myCamera.UpdateCamera();
                                                    anchorHpBar();

                                                    if (player.currentSkill)
                                                    {
                                                        if (player.currentSkill.SUBTYPE == SubSkillType.Buff || player.currentSkill.ELEMENT == Element.Support)
                                                        {
                                                            //  hitTile.myColor = Common.green;
                                                            for (int i = 0; i < currentAttackList.Count; i++)
                                                            {
                                                                currentAttackList[i].MYCOLOR = Common.green; ;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            // hitTile.myColor =  Common.red;
                                                            for (int i = 0; i < currentAttackList.Count; i++)
                                                            {
                                                                currentAttackList[i].MYCOLOR = Common.red; ;
                                                            }
                                                            myCamera.infoObject = GetObjectAtTile(tempGridObj.currentTile);
                                                            GridObject griddy = GetObjectAtTile(tempGridObj.currentTile);
                                                            if (griddy)
                                                            {
                                                                if (griddy.GetComponent<LivingObject>())
                                                                {
                                                                    LivingObject livvy = griddy.GetComponent<LivingObject>();
                                                                    LivingObject attacker = player.current;
                                                                    if (prevState == State.PlayerOppOptions)
                                                                    {
                                                                        if (currentObject)
                                                                        {
                                                                            if (currentObject.GetComponent<LivingObject>())
                                                                            {
                                                                                attacker = currentObject as LivingObject;
                                                                            }
                                                                        }
                                                                    }
                                                                    if (livvy.FACTION != attacker.FACTION)
                                                                    {
                                                                        DmgReaction reac = CalcDamage(attacker, livvy, player.currentSkill, Reaction.none, false);

                                                                        myCamera.potentialDamage = reac.damage;
                                                                        myCamera.UpdateCamera();

                                                                        // updateConditionals();
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    LivingObject attacker = player.current;
                                                                    if (prevState == State.PlayerOppOptions)
                                                                    {
                                                                        if (currentObject)
                                                                        {
                                                                            if (currentObject.GetComponent<LivingObject>())
                                                                            {
                                                                                attacker = currentObject as LivingObject;
                                                                            }
                                                                        }
                                                                    }
                                                                    DmgReaction reac = CalcDamage(attacker, griddy, player.currentSkill, Reaction.none, false);

                                                                    myCamera.potentialDamage = reac.damage;
                                                                    myCamera.UpdateCamera();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                myCamera.infoObject = null;
                                                                // updateConditionals();
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //  hitTile.myColor = Common.red;
                                                        for (int i = 0; i < currentAttackList.Count; i++)
                                                        {
                                                            currentAttackList[i].MYCOLOR = Common.red; ;

                                                        }
                                                        myCamera.infoObject = GetObjectAtTile(tempGridObj.currentTile);
                                                        GridObject griddy = GetObjectAtTile(tempGridObj.currentTile);
                                                        if (griddy)
                                                        {
                                                            if (griddy.GetComponent<LivingObject>())
                                                            {
                                                                LivingObject livvy = griddy.GetComponent<LivingObject>();
                                                                if (livvy.FACTION != player.current.FACTION)
                                                                {
                                                                    DmgReaction reac = CalcDamage(player.current, livvy, player.current.WEAPON, Reaction.none, false);

                                                                    myCamera.potentialDamage = reac.damage;
                                                                    myCamera.UpdateCamera();

                                                                    //  updateConditionals();
                                                                }
                                                            }
                                                            else
                                                            {

                                                                DmgReaction reac = CalcDamage(player.current, griddy, player.current.WEAPON, Reaction.none, false);

                                                                myCamera.potentialDamage = reac.damage;
                                                                myCamera.UpdateCamera();


                                                            }
                                                        }
                                                        else
                                                        {
                                                            myCamera.infoObject = null;
                                                            //  updateConditionals();
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            //  myCamera.infoObject = null;

                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                            if (Physics.Raycast(ray, out hit))
                            {
                                Vector3 w = hit.point;
                                w.x = Mathf.Round(w.x);
                                w.y = Mathf.Round(w.y);
                                w.z = Mathf.Round(w.z);

                                GameObject hitObj = hit.transform.gameObject;

                                if (hitObj.GetComponent<TileScript>())
                                {
                                    TileScript hitTile = hitObj.GetComponent<TileScript>();
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
                                                if (tempGridObj.currentTile == hitTile)
                                                {
                                                    myCamera.infoObject = GetObjectAtTile(tempGridObj.currentTile);
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
                                            ComfirmMoveGridObject(tempGridObj, hitTile);

                                            myCamera.potentialDamage = 0;
                                            myCamera.UpdateCamera();
                                            anchorHpBar();

                                            if (player.currentSkill)
                                            {
                                                if (player.currentSkill.SUBTYPE == SubSkillType.Buff || player.currentSkill.ELEMENT == Element.Support)
                                                {
                                                    //  hitTile.myColor = Common.green;
                                                    for (int i = 0; i < currentAttackList.Count; i++)
                                                    {
                                                        currentAttackList[i].MYCOLOR = Common.green; ;
                                                    }
                                                }
                                                else
                                                {
                                                    // hitTile.myColor =  Common.red;
                                                    for (int i = 0; i < currentAttackList.Count; i++)
                                                    {
                                                        currentAttackList[i].MYCOLOR = Common.red; ;
                                                    }
                                                    myCamera.infoObject = GetObjectAtTile(tempGridObj.currentTile);
                                                    GridObject griddy = GetObjectAtTile(tempGridObj.currentTile);
                                                    if (griddy)
                                                    {
                                                        if (griddy.GetComponent<LivingObject>())
                                                        {
                                                            LivingObject livvy = griddy.GetComponent<LivingObject>();
                                                            LivingObject attacker = player.current;
                                                            if (prevState == State.PlayerOppOptions)
                                                            {
                                                                if (currentObject)
                                                                {
                                                                    if (currentObject.GetComponent<LivingObject>())
                                                                    {
                                                                        attacker = currentObject as LivingObject;
                                                                    }
                                                                }
                                                            }
                                                            if (livvy.FACTION != attacker.FACTION)
                                                            {
                                                                DmgReaction reac = CalcDamage(attacker, livvy, player.currentSkill, Reaction.none, false);

                                                                myCamera.potentialDamage = reac.damage;
                                                                myCamera.UpdateCamera();

                                                                // updateConditionals();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            LivingObject attacker = player.current;
                                                            if (prevState == State.PlayerOppOptions)
                                                            {
                                                                if (currentObject)
                                                                {
                                                                    if (currentObject.GetComponent<LivingObject>())
                                                                    {
                                                                        attacker = currentObject as LivingObject;
                                                                    }
                                                                }
                                                            }
                                                            DmgReaction reac = CalcDamage(attacker, griddy, player.currentSkill, Reaction.none, false);

                                                            myCamera.potentialDamage = reac.damage;
                                                            myCamera.UpdateCamera();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        myCamera.infoObject = null;
                                                        // updateConditionals();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //  hitTile.myColor = Common.red;
                                                for (int i = 0; i < currentAttackList.Count; i++)
                                                {
                                                    currentAttackList[i].MYCOLOR = Common.red; ;

                                                }
                                                myCamera.infoObject = GetObjectAtTile(tempGridObj.currentTile);
                                                GridObject griddy = GetObjectAtTile(tempGridObj.currentTile);
                                                if (griddy)
                                                {
                                                    if (griddy.GetComponent<LivingObject>())
                                                    {
                                                        LivingObject livvy = griddy.GetComponent<LivingObject>();
                                                        if (livvy.FACTION != player.current.FACTION)
                                                        {
                                                            DmgReaction reac = CalcDamage(player.current, livvy, player.current.WEAPON, Reaction.none, false);

                                                            myCamera.potentialDamage = reac.damage;
                                                            myCamera.UpdateCamera();

                                                            //  updateConditionals();
                                                        }
                                                    }
                                                    else
                                                    {

                                                        DmgReaction reac = CalcDamage(player.current, griddy, player.current.WEAPON, Reaction.none, false);

                                                        myCamera.potentialDamage = reac.damage;
                                                        myCamera.UpdateCamera();


                                                    }
                                                }
                                                else
                                                {
                                                    myCamera.infoObject = null;
                                                    //  updateConditionals();
                                                }
                                            }

                                        }
                                        else
                                        {
                                            //updateConditionals();
                                            //   Debug.Log("no outer : " + outer + " or inner: " + innner);
                                        }

                                    }
                                }
                            }

                            updateConditionals();
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
                            //timbucktoo
                            hitkey = true;
                        }
                        if (Input.GetKeyDown(KeyCode.D))
                        {
                            ShowWhite();

                            hitkey = true;
                        }
                        if (Input.GetKeyDown(KeyCode.S))
                        {
                            ShowWhite();

                            hitkey = true;
                        }
                        if (Input.GetKeyDown(KeyCode.A))
                        {
                            ShowWhite();

                            hitkey = true;
                        }
                        if (Input.GetMouseButtonDown(0))
                        {

                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            if (Physics.Raycast(ray, out hit))
                            {
                                Vector3 w = hit.point;
                                w.x = Mathf.Round(w.x);
                                w.y = Mathf.Round(w.y);
                                w.z = Mathf.Round(w.z);
                                GameObject hitObj = hit.transform.gameObject;

                                if (hitObj.GetComponent<TileScript>())
                                {
                                    TileScript hitTile = hitObj.GetComponent<TileScript>();
                                    bool alreadySelected = false;
                                    int inex = -1;

                                    for (int i = 0; i < doubleAdjOppTiles.Count; i++)
                                    {

                                        if (doubleAdjOppTiles[i] == hitTile)
                                        {
                                            inex = i;

                                            if (tempGridObj.currentTile == hitTile)
                                            {
                                                alreadySelected = true;
                                                break;

                                            }

                                        }


                                    }
                                    if (alreadySelected)
                                    {
                                        //oppObj = GetObjectAtTile(hitTile) as LivingObject;
                                        //hitTile.myColor = Color.red;
                                        ////menuStackEntry entry = new menuStackEntry();
                                        ////entry.state = State.PlayerOppOptions;
                                        ////entry.index = invManager.currentIndex;
                                        ////entry.menu = currentMenu.OppSelection;

                                        ////enterState(entry);
                                        ////menuManager.showOpportunityOptions(oppObj);
                                        //CommandSkill skill = oppObj.GetMostUsedSkill();
                                        //CommandSkill spell = oppObj.GetMostUsedSpell();
                                        //WeaponScript weapon = oppObj.GetMostUsedStrike();
                                        //currentAttackList.Clear();
                                        //for (int i = 0; i < opptargets.Count; i++)
                                        //{
                                        //    currentAttackList.Add(opptargets[i].currentTile);
                                        //}

                                        //if (skill && spell && weapon)
                                        //{
                                        //    int r = Random.Range(0, 2);
                                        //    if (r == 0)
                                        //        AttackTargets(oppObj, skill);
                                        //    else if (r == 1)
                                        //        AttackTargets(oppObj, spell);
                                        //    else
                                        //        AttackTargets(oppObj, weapon);
                                        //}
                                        //else if (skill && spell)
                                        //{
                                        //    int r = Random.Range(0, 1);
                                        //    if (r == 0)
                                        //        AttackTargets(oppObj, skill);
                                        //    else
                                        //        AttackTargets(oppObj, spell);
                                        //}
                                        //else if (spell && weapon)
                                        //{
                                        //    int r = Random.Range(0, 1);
                                        //    if (r == 0)
                                        //        AttackTargets(oppObj, spell);
                                        //    else
                                        //        AttackTargets(oppObj, weapon);
                                        //}
                                        //else if (weapon)
                                        //{

                                        //    AttackTargets(oppObj, weapon);
                                        //}
                                    }
                                    else
                                    {
                                        if (inex >= 0)
                                        {

                                            showOppAdjTiles();
                                            tempObject.transform.position = hitTile.transform.position;
                                            ComfirmMoveGridObject(tempGridObj, hitTile);

                                            hitTile.MYCOLOR = Color.red;
                                        }


                                    }
                                    oppEvent.caller = null;
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
                                        targetTile.MYCOLOR = Color.red;
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
                    {

                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            CancelMenuAction(player.current);


                        }
                        if (Input.GetMouseButtonDown(1))
                        {
                            CancelMenuAction(player.current);

                        }

                    }
                    break;
                case State.playerUsingSkills:
                    {

                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            CancelMenuAction(player.current);
                            player.currentSkill = null;

                        }
                        if (Input.GetMouseButtonDown(1))
                        {
                            CancelMenuAction(player.current);
                            player.currentSkill = null;
                        }

                        if (Input.GetKeyDown(KeyCode.D))
                        {
                            SelectMenuItem(player.current);

                        }
                        if (Input.GetKeyDown(KeyCode.A))
                        {
                            CancelMenuAction(player.current);
                            player.currentSkill = null;

                        }
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            invManager.SetNumAndSelect(0);

                        }

                        if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            invManager.SetNumAndSelect(1);

                        }

                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            invManager.SetNumAndSelect(2);
                        }

                        if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            invManager.SetNumAndSelect(3);

                        }
                    }
                    break;
                case State.PlayerWait:
                    break;
                case State.PlayerUsingItems:
                    {
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            ShowWhite();
                            attackIndex++;
                            ChangeAtkTargets();
                        }
                        if (Input.GetKeyDown(KeyCode.D))
                        {
                            ShowWhite();
                            attackIndex++;
                            ChangeAtkTargets();
                        }
                        if (Input.GetKeyDown(KeyCode.S))
                        {
                            ShowWhite();
                            attackIndex--;
                            ChangeAtkTargets();
                        }
                        if (Input.GetKeyDown(KeyCode.A))
                        {
                            ShowWhite();
                            attackIndex--;
                            ChangeAtkTargets();
                        }


                        if (Input.GetMouseButtonDown(0))
                        {
                            //  myCamera.infoObject = null;

                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                            if (Physics.Raycast(ray, out hit))
                            {
                                Vector3 w = hit.point;
                                w.x = Mathf.Round(w.x);
                                w.y = Mathf.Round(w.y);
                                w.z = Mathf.Round(w.z);

                                GameObject hitObj = hit.transform.gameObject;

                                if (hitObj.GetComponent<TileScript>())
                                {
                                    TileScript hitTile = hitObj.GetComponent<TileScript>();
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
                                                if (tempGridObj.currentTile == hitTile)
                                                {
                                                    myCamera.infoObject = GetObjectAtTile(tempGridObj.currentTile);
                                                    alreadySelected = true;
                                                    break;

                                                }

                                            }
                                        }

                                    }
                                    if (alreadySelected)
                                    {

                                        if (myCamera.infoObject)
                                        {
                                            if (myCamera.infoObject.GetComponent<LivingObject>())
                                            {
                                                player.UseItem(myCamera.infoObject.GetComponent<LivingObject>(), myCamera.currentTile);
                                            }
                                            else if (player.currentItem.ITYPE == ItemType.dmg)
                                            {
                                                player.UseItem(myCamera.infoObject, myCamera.currentTile);
                                            }
                                            else
                                            {
                                                CreateTextEvent(this, "Invalid target", "validation text", CheckText, TextStart);
                                                PlayExitSnd();
                                            }
                                        }
                                        else
                                        {
                                            player.UseItem(null, myCamera.currentTile);
                                        }
                                    }
                                    else
                                    {
                                        if (outer >= 0 && innner >= 0)
                                        {

                                            showAttackableTiles();
                                            tempObject.transform.position = hitTile.transform.position;
                                            ComfirmMoveGridObject(tempGridObj, hitTile);

                                            myCamera.potentialDamage = 0;
                                            myCamera.UpdateCamera();
                                            anchorHpBar();

                                            if (player.currentItem)
                                            {
                                                if (player.currentItem.ITYPE != ItemType.dmg && player.currentItem.ITYPE != ItemType.dart)
                                                {
                                                    //  hitTile.myColor = Common.green;
                                                    for (int i = 0; i < currentAttackList.Count; i++)
                                                    {
                                                        currentAttackList[i].MYCOLOR = Common.green; ;
                                                    }
                                                }
                                                else
                                                {
                                                    // hitTile.myColor =  Common.red;
                                                    for (int i = 0; i < currentAttackList.Count; i++)
                                                    {
                                                        currentAttackList[i].MYCOLOR = Common.red; ;
                                                    }
                                                    myCamera.infoObject = GetObjectAtTile(tempGridObj.currentTile);
                                                    GridObject griddy = GetObjectAtTile(tempGridObj.currentTile);
                                                    if (griddy)
                                                    {
                                                        if (griddy.GetComponent<LivingObject>())
                                                        {
                                                            LivingObject livvy = griddy.GetComponent<LivingObject>();
                                                            if (livvy.FACTION != player.current.FACTION)
                                                            {
                                                                DmgReaction reac = CalcDamage(player.current, livvy, player.currentSkill, Reaction.none, false);

                                                                myCamera.potentialDamage = reac.damage;
                                                                myCamera.UpdateCamera();

                                                                updateConditionals();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            DmgReaction reac = CalcDamage(player.current, griddy, player.currentSkill, Reaction.none, false);

                                                            myCamera.potentialDamage = reac.damage;
                                                            myCamera.UpdateCamera();

                                                            updateConditionals();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        myCamera.infoObject = null;
                                                        updateConditionals();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //  hitTile.myColor = Common.red;
                                                for (int i = 0; i < currentAttackList.Count; i++)
                                                {
                                                    currentAttackList[i].MYCOLOR = Common.lime;
                                                }
                                                myCamera.infoObject = GetObjectAtTile(tempGridObj.currentTile);
                                                GridObject griddy = GetObjectAtTile(tempGridObj.currentTile);
                                                if (griddy)
                                                {
                                                    if (griddy.GetComponent<LivingObject>())
                                                    {
                                                        LivingObject livvy = griddy.GetComponent<LivingObject>();
                                                        if (livvy.FACTION != player.current.FACTION)
                                                        {
                                                            DmgReaction reac = CalcDamage(player.current, livvy, player.current.WEAPON, Reaction.none, false);

                                                            myCamera.potentialDamage = reac.damage;
                                                            myCamera.UpdateCamera();

                                                            updateConditionals();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (!player.currentSkill)
                                                        {
                                                            player.currentSkill = Common.GenericSkill;
                                                        }
                                                        DmgReaction reac = CalcDamage(player.current, griddy, player.currentSkill, Reaction.none, false);

                                                        myCamera.potentialDamage = reac.damage;
                                                        myCamera.UpdateCamera();

                                                        updateConditionals();
                                                    }
                                                }
                                                else
                                                {
                                                    myCamera.infoObject = null;
                                                    updateConditionals();
                                                }
                                            }

                                        }
                                        else
                                        {
                                            updateConditionals();
                                            //   Debug.Log("no outer : " + outer + " or inner: " + innner);
                                        }

                                    }
                                }
                            }

                        }
                        if (Input.GetMouseButtonDown(1))
                        {
                            returnState();
                            player.currentSkill = null;
                            player.currentItem = null;
                        }
                    }
                    break;
                case State.FreeCamera:

                    if (Input.GetMouseButtonDown(1))
                    {
                        if (myCamera.infoObject)
                        {
                            if (myCamera.infoObject)
                            {


                            }
                            else
                            {
                                // StackOptions();
                            }
                        }
                        else
                        {
                            // StackOptions();
                        }
                    }
                    if (Input.GetMouseButtonDown(0))
                    {

                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit))
                        {
                            Vector3 w = hit.point;
                            w.x = Mathf.Round(w.x);
                            w.y = Mathf.Round(w.y);
                            w.z = Mathf.Round(w.z);
                            GameObject hitObj = hit.transform.gameObject;

                            if (hitObj.GetComponent<TileScript>())
                            {
                                TileScript hitTile = hitObj.GetComponent<TileScript>(); //GetTileAtIndex(GetTileIndex(w));
                                bool alreadySelected = false;

                                if (tempGridObj.currentTile == hitTile)
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
                                            if (turnOrder[i].ACTIONS > 0 || liveEnemies.Count == 0)
                                            {
                                                turnImgManger.UpdateSelection(i);
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
                                        // PlayExitSnd();
                                        if (menuManager)
                                        {

                                            if (GetObjectAtTile(hitTile) != null && currentTutorial.steps.Count == 0)
                                            {
                                                if (detailsScreen)
                                                {
                                                    if (myCamera.infoObject.GetComponent<LivingObject>())
                                                    {
                                                        detailsScreen.anotherObj = null;
                                                        detailsScreen.currentObj = myCamera.infoObject.GetComponent<LivingObject>();
                                                        StackDetails();
                                                    }
                                                    else
                                                    {
                                                        detailsScreen.currentObj = null;
                                                        detailsScreen.anotherObj = myCamera.infoObject.GetComponent<GridObject>();
                                                        StackDetails();
                                                    }
                                                }
                                                //LivingObject livvy = GetObjectAtTile(hitTile).GetComponent<LivingObject>();
                                                //if (livvy != null)
                                                //{
                                                //    if (markedEnemies.Contains(livvy))
                                                //    {
                                                //        markedEnemies.Remove(livvy);
                                                //        livvy.RENDERER.color = Color.white;

                                                //    }
                                                //    else
                                                //    {

                                                //        markedEnemies.Add(livvy);
                                                //        livvy.RENDERER.color = Color.magenta;
                                                //    }
                                                //    UpdateMarkedArea();
                                                //}
                                            }
                                            else
                                            {
                                                if (currentTutorial.steps.Count == 0)
                                                {
                                                    //StackNewSelection(State.PlayerInput, currentMenu.none);
                                                    //menuManager.ShowHiddenCanvas();
                                                }
                                                else if (currentTutorial.currentStep < currentTutorial.steps.Count)
                                                {
                                                    CheckTutorialPrompt("-1; Tutorial - " + Common.GetTutorialText(currentTutorial.steps[currentTutorial.currentStep], true));
                                                }
                                                else
                                                {
                                                    currentTutorial.steps.Clear();
                                                    currentTutorial.currentStep = 0;
                                                    currentTutorial.clarifications.Clear();
                                                    //StackNewSelection(State.PlayerInput, currentMenu.none);
                                                    //menuManager.ShowHiddenCanvas();
                                                }
                                            }

                                        }
                                    }
                                    myCamera.currentTile = myCamera.selectedTile;
                                    myCamera.UpdateCamera();
                                    updateConditionals();
                                }
                                else
                                {
                                    ShowWhite();
                                    for (int i = 0; i < gridObjects.Count; i++)
                                    {
                                        ShowSelectedTile(gridObjects[i], Common.GetFactionColor(gridObjects[i].FACTION));
                                    }

                                    tempObject.transform.position = hitTile.transform.position;
                                    ComfirmMoveGridObject(tempGridObj, hitTile);
                                    if (GetObjectAtTile(tempGridObj.currentTile) != null)
                                    {
                                        ShowGridObjectAffectArea(GetObjectAtTile(tempGridObj.currentTile));
                                        //  if (iconManager)
                                        {
                                            //    iconManager.loadIconPanel(GetObjectAtTile(tempGridObj.currentTile).GetComponent<LivingObject>());
                                        }
                                    }
                                    ShowSelectedTile(tempGridObj);
                                    myCamera.UpdateCamera();
                                    updateConditionals();
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
                        MoveGridObject(tempGridObj, new Vector3(0, 0, 1));
                        ComfirmMoveGridObject(tempGridObj, GetTileIndex(tempGridObj));
                        if (GetObjectAtTile(tempGridObj.currentTile) != null)
                            ShowGridObjectAffectArea(GetObjectAtTile(tempGridObj.currentTile));
                        ShowSelectedTile(tempGridObj);
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        ShowWhite();
                        for (int i = 0; i < turnOrder.Count; i++)
                        {
                            ShowSelectedTile(turnOrder[i], Common.orange);

                        }
                        MoveGridObject(tempGridObj, new Vector3(-1, 0, 0));
                        ComfirmMoveGridObject(tempGridObj, GetTileIndex(tempGridObj));
                        if (GetObjectAtTile(tempGridObj.currentTile) != null)
                            ShowGridObjectAffectArea(GetObjectAtTile(tempGridObj.currentTile));
                        ShowSelectedTile(tempGridObj);
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        ShowWhite();
                        for (int i = 0; i < turnOrder.Count; i++)
                        {
                            ShowSelectedTile(turnOrder[i], Common.orange);

                        }
                        MoveGridObject(tempGridObj, new Vector3(0, 0, -1));
                        ComfirmMoveGridObject(tempGridObj, GetTileIndex(tempGridObj));
                        if (GetObjectAtTile(tempGridObj.currentTile) != null)
                            ShowGridObjectAffectArea(GetObjectAtTile(tempGridObj.currentTile));
                        ShowSelectedTile(tempGridObj);
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {

                        ShowWhite();
                        for (int i = 0; i < turnOrder.Count; i++)
                        {
                            ShowSelectedTile(turnOrder[i], Common.orange);

                        }
                        MoveGridObject(tempGridObj, new Vector3(1, 0, 0));
                        ComfirmMoveGridObject(tempGridObj, GetTileIndex(tempGridObj));
                        if (GetObjectAtTile(tempGridObj.currentTile) != null)
                            ShowGridObjectAffectArea(GetObjectAtTile(tempGridObj.currentTile));
                        ShowSelectedTile(tempGridObj);
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") == 0.1f)
                    {
                        if (myCamera.targetOrthoSize < 11)
                        {
                            myCamera.mainCam.orthographicSize += 0.1f;
                            myCamera.targetOrthoSize += 0.1f;
                            myCamera.SetCameraPosDefault();
                        }
                    }
                    if (Input.GetAxis("Mouse ScrollWheel") == -0.1f)
                    {
                        if (myCamera.targetOrthoSize > 5)
                        {
                            myCamera.mainCam.orthographicSize -= 0.1f;
                            myCamera.targetOrthoSize -= 0.1f;
                            myCamera.SetCameraPosDefault();
                        }
                    }

                    //if (Input.GetKeyDown(KeyCode.Space))
                    //{
                    //    if ((int)descriptionState + 1 <= 5)
                    //    {
                    //        descriptionState = (descState)((int)descriptionState + 1);
                    //        myCamera.UpdateCamera();

                    //    }
                    //    else
                    //    {
                    //        descriptionState = descState.stats;
                    //        myCamera.UpdateCamera();

                    //    }
                    //}
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
                        int playerIndx = -1;
                        for (int i = 0; i < turnOrder.Count; i++)
                        {
                            if (tempGridObj.currentTile == turnOrder[i].currentTile)
                            {
                                playerIndx = i;
                                if (turnOrder[i].ACTIONS > 0)
                                {
                                    turnImgManger.UpdateSelection(i);
                                    player.current = turnOrder[i];
                                    currentObject = turnOrder[i];

                                    // enterState(defaultEntry);
                                    StackCMDSelection();
                                    CreateEvent(this, currentObject, "Select Camera Event", CameraEvent);
                                    break;
                                }

                            }

                        }


                        if (playerIndx == -1)
                        {
                            if (menuManager)
                            {
                                // if (menuManager.hiddenCanvas)
                                {
                                    if (currentTutorial.steps.Count == 0)
                                    {
                                        //StackNewSelection(State.PlayerInput, currentMenu.none);
                                        //menuManager.ShowHiddenCanvas();
                                    }
                                    else if (currentTutorial.currentStep < currentTutorial.steps.Count)
                                    {
                                        CheckTutorialPrompt("-1;Tutorial - " + Common.GetTutorialText(currentTutorial.steps[currentTutorial.currentStep], true));
                                    }
                                    else
                                    {
                                        currentTutorial.steps.Clear();
                                        currentTutorial.currentStep = 0;
                                        currentTutorial.clarifications.Clear();
                                        //StackNewSelection(State.PlayerInput, currentMenu.none);
                                        //menuManager.ShowHiddenCanvas();
                                    }
                                }
                            }
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Escape))
                    {

                        if (myCamera.infoObject)
                        {
                            if (myCamera.infoObject)
                            {

                                if (detailsScreen)
                                {
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
                case State.ShopCanvas:
                    if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                    {
                        if (shopScreen)
                        {
                            if (shopScreen.currentWindow != ShopWindow.main)
                            {
                                shopScreen.PreviousMenu();
                            }
                            else
                            {
                                myCamera.PlayPreviousSoundTrack();
                                returnState();
                            }
                        }

                    }
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
                case State.SceneRunning:
                    {
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            NextScene();
                        }
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            EndScene();
                        }
                        if (Input.GetMouseButtonDown(1))
                        {

                            EndScene();
                        }
                    }
                    break;
                case State.ChangeOptions:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        StackOptions();
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        StackOptions();
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
                        if (detailsScreen.viewContent < 3)
                        {
                            detailsScreen.viewContent = 28;
                        }
                        else if (detailsScreen.viewContent >= 21 && detailsScreen.viewContent < 28)
                        {
                            detailsScreen.viewContent -= 10;
                        }
                        else if (detailsScreen.viewContent > 3 && detailsScreen.viewContent <= 7)
                        {
                            detailsScreen.viewContent--;
                        }
                        else if (detailsScreen.viewContent == 3)
                        {
                            detailsScreen.viewContent = 1;
                        }
                        else if (detailsScreen.viewContent == 8 || detailsScreen.viewContent == 9 || detailsScreen.viewContent == 12)  // else if (detailsScreen.viewContent > 8 && detailsScreen.viewContent < 14)
                        {
                            detailsScreen.viewContent = 7;
                        }
                        else if (detailsScreen.viewContent == 10 || detailsScreen.viewContent == 11)
                        {
                            detailsScreen.viewContent -= 2;
                        }
                        else if (detailsScreen.viewContent == 12)
                        {
                            detailsScreen.viewContent = 11;
                        }
                        else if (detailsScreen.detail != DetailType.Exp)
                        {

                            if (detailsScreen.viewContent == 14)
                            {
                                detailsScreen.viewContent = 7;
                            }
                            else if (detailsScreen.viewContent > 14 && detailsScreen.viewContent < 21)
                            {
                                detailsScreen.viewContent = 7;
                            }

                            else if (detailsScreen.viewContent == 28)
                            {
                                detailsScreen.viewContent = 21;
                            }
                        }
                        else
                        {
                            if (detailsScreen.viewContent > 29 && detailsScreen.viewContent < 33)
                            {
                                detailsScreen.viewContent--;
                            }
                            else if (detailsScreen.viewContent == 14)
                            {
                                detailsScreen.viewContent = 32;
                            }
                            else if (detailsScreen.viewContent > 14 && detailsScreen.viewContent < 21)
                            {
                                detailsScreen.viewContent = 7;
                            }
                            else if (detailsScreen.viewContent == 28)
                            {
                                detailsScreen.viewContent = 21;
                            }
                            else if (detailsScreen.viewContent == 29)
                            {
                                detailsScreen.viewContent = 1;
                            }


                        }
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {

                        if (detailsScreen.viewContent >= 22 && detailsScreen.viewContent < 28)
                        {
                            detailsScreen.viewContent = 28;
                        }
                        else if (detailsScreen.viewContent >= 14 && detailsScreen.viewContent < 22)
                        {
                            detailsScreen.viewContent += 7;
                        }
                        else if (detailsScreen.viewContent == 28)
                        {
                            detailsScreen.viewContent = 1;
                        }
                        else if (detailsScreen.viewContent >= 3 && detailsScreen.viewContent < 7)
                        {
                            detailsScreen.viewContent++;
                        }
                        else if (detailsScreen.viewContent > 7 && detailsScreen.viewContent < 14)
                        {
                            detailsScreen.viewContent++;
                        }
                        else if (detailsScreen.detail != DetailType.Exp)
                        {
                            if (detailsScreen.viewContent == 7)
                            {
                                detailsScreen.viewContent = 14;
                            }

                            else if (detailsScreen.viewContent < 3)
                            {
                                detailsScreen.viewContent = 3;
                            }

                        }
                        else
                        {
                            if (detailsScreen.viewContent == 7)
                            {
                                detailsScreen.viewContent = 14;
                            }
                            else if (detailsScreen.viewContent >= 29 && detailsScreen.viewContent < 32)
                            {
                                detailsScreen.viewContent++;
                            }
                            else if (detailsScreen.viewContent == 32)
                            {
                                detailsScreen.viewContent = 14;
                            }
                            else if (detailsScreen.viewContent < 3)
                            {
                                detailsScreen.viewContent = 29;
                            }
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        if (detailsScreen.viewContent == 0)
                        {
                            int truedetail = (int)detailsScreen.detail;
                            truedetail--;
                            if (truedetail < 0)
                            {
                                truedetail = (int)DetailType.Exp;
                            }
                            detailsScreen.detail = (DetailType)truedetail;
                        }
                        else if (detailsScreen.viewContent > 14 && detailsScreen.viewContent < 21)
                        {
                            detailsScreen.viewContent--;
                        }
                        else if (detailsScreen.viewContent > 21 && detailsScreen.viewContent < 28)
                        {
                            detailsScreen.viewContent--;
                        }
                        else if (detailsScreen.viewContent < 3)
                        {
                            detailsScreen.viewContent--;
                        }
                        else if (detailsScreen.detail != DetailType.Exp)
                        {
                            if (detailsScreen.viewContent >= 8 && detailsScreen.viewContent < 13)
                            {
                                detailsScreen.viewContent -= 5;
                            }
                            else if (detailsScreen.viewContent > 2 && detailsScreen.viewContent < 8)
                            {
                                detailsScreen.viewContent += 5;
                            }
                            else if (detailsScreen.viewContent == 13)
                            {
                                detailsScreen.viewContent -= 6;
                            }
                        }
                        else
                        {
                            if (detailsScreen.viewContent >= 8 && detailsScreen.viewContent < 13)
                            {
                                detailsScreen.viewContent = 29;
                            }
                            else if (detailsScreen.viewContent > 28 && detailsScreen.viewContent < 33)
                            {
                                detailsScreen.viewContent = 8;
                            }
                        }

                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        if (detailsScreen.viewContent == 2)
                        {
                            int truedetail = (int)detailsScreen.detail;
                            truedetail++;
                            if (truedetail > (int)DetailType.Exp)
                            {
                                truedetail = 0;
                            }
                            detailsScreen.detail = (DetailType)truedetail;
                        }
                        else if (detailsScreen.viewContent > 13 && detailsScreen.viewContent < 20)
                        {
                            detailsScreen.viewContent++;
                        }
                        else if (detailsScreen.viewContent > 20 && detailsScreen.viewContent < 27)
                        {
                            detailsScreen.viewContent++;
                        }
                        else if (detailsScreen.viewContent < 2)
                        {
                            detailsScreen.viewContent++;
                        }
                        else if (detailsScreen.detail != DetailType.Exp)
                        {

                            if (detailsScreen.viewContent > 2 && detailsScreen.viewContent < 8)
                            {
                                detailsScreen.viewContent += 5;
                            }
                            else if (detailsScreen.viewContent >= 8 && detailsScreen.viewContent < 13)
                            {
                                detailsScreen.viewContent -= 5;
                            }
                            else if (detailsScreen.viewContent == 13)
                            {
                                detailsScreen.viewContent -= 6;
                            }
                        }
                        else
                        {
                            if (detailsScreen.viewContent >= 8 && detailsScreen.viewContent < 13)
                            {
                                detailsScreen.viewContent = 29;
                            }
                            else if (detailsScreen.viewContent > 28 && detailsScreen.viewContent < 33)
                            {
                                detailsScreen.viewContent = 8;
                            }
                        }
                    }
                    break;

                case State.AquireNewSkill:
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        //      YesPrompt();
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        newSkillEvent.data = null;
                        newSkillEvent.caller = null;
                        returnState();
                    }
                    break;
                case State.PlayerEquippingMenu:
                    break;
                case State.PlayerEquippingSkills:
                    break;
                case State.PlayerSkillsMenu:
                    break;
                case State.PlayerSelectItem:
                    {


                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            CancelMenuAction(player.current);
                            player.currentItem = null;

                        }
                        if (Input.GetMouseButtonDown(1))
                        {
                            CancelMenuAction(player.current);
                            player.currentItem = null;
                        }
                        if (Input.GetKeyDown(KeyCode.D))
                        {
                            SelectMenuItem(player.current);

                        }
                        if (Input.GetKeyDown(KeyCode.A))
                        {
                            CancelMenuAction(player.current);
                            player.currentItem = null;

                        }
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            invManager.SetNumAndSelect(0);

                        }

                        if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            invManager.SetNumAndSelect(1);

                        }

                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            invManager.SetNumAndSelect(2);
                        }

                        if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            invManager.SetNumAndSelect(3);

                        }

                    }
                    break;
                case State.PlayerTransition:
                    break;
                case State.HazardTurn:
                    break;
                case State.EventRunning:
                    break;
                case State.FairyPhase:
                    break;
                case State.PlayerAct:
                    break;
                default:
                    break;
            }

        }

    }

    private IEnumerator performChecks()
    {
        while (true)
        {

            isperforming = true;
            if (currentState == State.FreeCamera)
            {
                if (options != null)
                {
                    if (options.hoverSelect == true)
                    {
                        if (myCamera.moving == false)
                        {


                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            if (Physics.Raycast(ray, out hit))
                            {

                                GameObject hitObj = hit.transform.gameObject;

                                if (hitObj.GetComponent<TileScript>())
                                {
                                    TileScript hitTile = hitObj.GetComponent<TileScript>();
                                    bool alreadySelected = false;

                                    if (tempGridObj.currentTile == hitTile)
                                    {
                                        alreadySelected = true;

                                    }

                                    if (alreadySelected)
                                    {

                                    }
                                    else
                                    {
                                        if (tempObject.transform.position != hitTile.transform.position)
                                        {
                                            ShowWhite();
                                            for (int i = 0; i < gridObjects.Count; i++)
                                            {
                                                ShowSelectedTile(gridObjects[i], Common.GetFactionColor(gridObjects[i].FACTION));
                                            }

                                            tempObject.transform.position = hitTile.transform.position;
                                            ComfirmMoveGridObject(tempGridObj, hitTile, true);
                                            GridObject griddy = GetObjectAtTile(tempGridObj.currentTile);
                                            if (griddy != null)
                                            {
                                                ShowGridObjectAffectArea(griddy, false);
                                                // ShowGridObjectAttackArea(griddy);
                                                //if (iconManager)
                                                //{
                                                //    iconManager.loadIconPanel(GetObjectAtTile(tempGridObj.currentTile).GetComponent<LivingObject>());
                                                //}
                                            }
                                            //  ShowSelectedTile(tempGridObj);
                                            ShowSelectedTile(tempGridObj.currentTile, Common.selcted);

                                            myCamera.UpdateCamera();
                                            updateConditionals();
                                        }
                                    }
                                }
                                if (myCamera.currentTile != myCamera.selectedTile)
                                {
                                    if (Vector3.Distance(myCamera.currentTile.transform.position, myCamera.selectedTile.transform.position) >= 6.0f)
                                    {
                                        myCamera.currentTile = myCamera.selectedTile;
                                        myCamera.UpdateCamera();
                                    }


                                }
                            }
                        }
                        {

                        }
                    }
                }
            }
            isperforming = false;
            yield return null;
        }

    }
    public State GetState()
    {
        if (currentState == State.RevenantPhase)
            return State.EnemyTurn;

        if (currentState == State.VamprettiPhase)
            return State.EnemyTurn;

        if (currentState == State.AntileonPhase)
            return State.EnemyTurn;

        if (currentState == State.ThiefPhase)
            return State.EnemyTurn;

        if (currentState == State.GeniePhase)
            return State.EnemyTurn;

        if (currentState == State.FairyPhase)
            return State.EnemyTurn;

        return currentState;
    }
    public void ChangeAtkTargets()
    {

        if (attackableTiles.Count > 0)
        {



            if (attackableTiles.Count - 1 < attackIndex)
            {
                attackIndex = 0;
            }
            if (attackIndex < 0)
            {
                attackIndex = attackableTiles.Count - 1;
            }
            currentAttackList = attackableTiles[attackIndex];
            showAttackableTiles();
            ChangeAttackList();
            for (int i = 0; i < currentAttackList.Count; i++)
            {
                GridObject griddy = GetObjectAtTile(currentAttackList[i]);
                if (griddy)
                {
                    if (griddy.GetComponent<LivingObject>())
                    {
                        LivingObject livvy = griddy.GetComponent<LivingObject>();
                        if (livvy.FACTION != player.current.FACTION)
                        {
                            DmgReaction reac;
                            if (player.currentSkill)
                            {
                                reac = CalcDamage(player.current, livvy, player.currentSkill, Reaction.none, false);
                            }
                            else
                            {
                                reac = CalcDamage(player.current, livvy, player.current.WEAPON, Reaction.none, false);
                            }


                            myCamera.potentialDamage = reac.damage;
                            myCamera.UpdateCamera();

                        }
                    }
                    else
                    {
                        DmgReaction reac;
                        if (player.currentSkill)
                        {
                            reac = CalcDamage(player.current, griddy, player.currentSkill, Reaction.none, false);
                        }
                        else
                        {
                            reac = CalcDamage(player.current, griddy, player.current.WEAPON, Reaction.none, false);
                        }

                        myCamera.potentialDamage = reac.damage;
                        myCamera.UpdateCamera();
                    }
                }
            }

            updateConditionals();
            //  ChangeAttackList();
            //if (potentialTile1)
            //{
            //    return;

            //}
            //if (potentialTile2)
            //{
            //    potentialTile2 = GetTileAtIndex(GetTileIndex(potentialTile2.transform.position + directionVector));
            //    if (potentialTile2)
            //    {
            //        for (int i = 0; i < attackableTiles.Count; i++)
            //        {
            //            for (int j = 0; j < attackableTiles[i].Count; j++)
            //            {
            //                TileScript compareTile = attackableTiles[i][j];
            //                if (potentialTile2 == compareTile)
            //                {
            //                    currentAttackList = attackableTiles[i];

            //                    return;
            //                }
            //            }
            //        }
            //    }
            //}


            //tempObject.transform.position = originalPosition;


        }
    }

    public void ChangeAttackList()
    {
        showAttackableTiles();
        bool foundSomething = false;
        for (int i = 0; i < currentAttackList.Count; i++)
        {
            if (GetObjectAtTile(currentAttackList[i]) != null)
            {
                foundSomething = true;
                if (SetGridObjectPosition(tempGridObj, currentAttackList[i].transform.position) == true)
                {
                    ComfirmMoveGridObject(tempGridObj, GetTileIndex(tempGridObj));

                }

            }
            if (player.currentSkill)
            {
                if (player.currentSkill.SUBTYPE == SubSkillType.Buff || player.currentSkill.ELEMENT == Element.Support)
                {
                    currentAttackList[i].MYCOLOR = Common.green;
                }
                else
                {
                    currentAttackList[i].MYCOLOR = Common.red;
                }
            }
            else
            {
                if (GetState() == State.PlayerUsingItems)
                {
                    if (player.currentItem)
                    {
                        if (player.currentItem.ITYPE != ItemType.dmg && player.currentItem.ITYPE != ItemType.dart)
                        {
                            currentAttackList[i].MYCOLOR = Common.green;
                        }
                        else
                        {
                            currentAttackList[i].MYCOLOR = Common.red;
                        }
                    }
                }
                else
                {

                    currentAttackList[i].MYCOLOR = Common.red;
                }

            }
        }
        if (foundSomething == false)
        {
            if (SetGridObjectPosition(tempGridObj, currentAttackList[0].transform.position) == true)
            {
                ComfirmMoveGridObject(tempGridObj, GetTileIndex(tempGridObj));

            }

        }
    }
    public void PlayHitSound()
    {
        if (sfx)
        {
            if (sfxClips.Length >= 4)
            {
                sfx.loadAudio(sfxClips[5]);
                sfx.playSound();
            }
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
    public void PlayOppSnd()
    {
        if (sfx)
        {
            if (sfxClips.Length > 0)
            {
                sfx.loadAudio(sfxClips[4]);
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
    private void AttemptToGoIntoShadows(GridObject griddy)
    {
        griddy.RENDERER.color = Common.dark;
        if (griddy.GetComponent<LivingObject>())
        {
            LivingObject livvy = griddy.GetComponent<LivingObject>();
            if (livvy.FACTION == Faction.ally)
            {
                //if (CheckAdjecentTilesEnemy(livvy) == true)
                //{
                //    livvy.RENDERER.color = Color.white;
                //    return;
                //}
                for (int i = 0; i < liveEnemies.Count; i++)
                {
                    if (liveEnemies[i].GetComponent<EnemyScript>())
                    {
                        EnemyScript enemy = liveEnemies[i].GetComponent<EnemyScript>();
                        if (enemy.sightedTargets.Contains(livvy))
                        {
                            livvy.RENDERER.color = Color.white;
                            return;
                        }
                    }
                }

            }
        }


    }
    private void updateConditionals()
    {

        for (int i = 0; i < displays.Length; i++)
        {
            displays[i].UpdateDisplay(this);
        }


    }
    public void ShowCantUseText(string showText)
    {
        CreateTextEvent(this, showText, "error text event", CheckText, TextStart);
    }
    public void ShowCantUseText(WeaponScript skill)
    {
        CreateTextEvent(this, "Not enough health to use " + skill.NAME, "error text event", CheckText, TextStart);
    }
    public void ShowCantUseText(CommandSkill skill)
    {
        if (skill != null)
        {

            if (skill.ETYPE == EType.magical)
            {
                CreateTextEvent(this, "Not enough mana to use " + skill.NAME, "error text event", CheckText, TextStart);
            }
            else
            {
                if (skill.COST > 0)
                {

                    CreateTextEvent(this, skill.NAME + " would exceed max Fatigue ", "error text event", CheckText, TextStart);
                }
                else
                {
                    CreateTextEvent(this, "Not enough Fatigue to use " + skill.NAME, "error text event", CheckText, TextStart);

                }
            }
        }
    }
    public void enterState(menuStackEntry entry)
    {


        if (GetState() == State.PlayerAttacking || GetState() == State.PlayerTransition || GetState() == State.PlayerUsingItems)
        {
            if (player.current)
            {
                SpriteRenderer sr = player.current.GetComponent<SpriteRenderer>();
                if (sr)
                {
                    if (player.current.currentTile.isInShadow)
                    {
                        AttemptToGoIntoShadows(player.current);
                    }
                    else
                    {
                        sr.color = Color.white;
                    }
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


        // TextObjectHandler.UpdateText(textHolder.subphaseTracker, currentState.ToString());
        //TextObjectHandler.UpdateText(textHolder.shadowSubphaseTracker, currentState.ToString());

        menuStack.Add(entry);
        invManager.currentIndex = 0;
        if (GetState() == State.PlayerMove)
        {
            if (currentObject.GetComponent<LivingObject>())
            {
                ShowGridObjectMoveArea(currentObject.GetComponent<LivingObject>(), true);
            }
            if (player)
            {
                if (player.current)
                {
                    if (player.current.ANIM)
                    {
                        player.current.ANIM.runAnyway = true;
                        player.current.SHADOW.SCRIPT.runAnyway = true;
                    }
                }
            }
        }
        else
        {
            if (player)
            {
                if (player.current)
                {
                    if (player.current.ANIM)
                    {
                        player.current.ANIM.runAnyway = false;
                        player.current.SHADOW.SCRIPT.runAnyway = false;
                    }
                }
            }
        }
        if (GetState() == State.PlayerEquipping)
        {
            descriptionState = descState.stats;
        }
        if (GetState() == State.playerUsingSkills)
        {
            descriptionState = descState.stats;
        }
        if (GetState() == State.PlayerAttacking || GetState() == State.PlayerTransition || GetState() == State.PlayerUsingItems)
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
        if (GetState() == State.PlayerAttacking)
        {
            myCamera.attackingCheck = true;
            potential.pulsing = true;
        }
        else if (GetState() == State.PlayerUsingItems)
        {
            if (player.currentItem.ITYPE == ItemType.dmg)
            {
                myCamera.attackingCheck = true;
                potential.pulsing = true;
            }
            else
            {
                myCamera.potentialDamage = 0;
                myCamera.attackingCheck = false;
                potential.pulsing = false;
            }
        }
        else
        {
            myCamera.potentialDamage = 0;
            myCamera.attackingCheck = false;
            potential.pulsing = false;
        }
        myCamera.UpdateCamera();
        updateConditionals();

        UpdateCameraPosition();


    }
    public void UpdateCameraPosition()
    {
        switch (GetState())
        {

            case State.PlayerMove:
                myCamera.SetCameraPosFar();
                break;
            case State.PlayerAttacking:
                myCamera.SetCameraPosSlightZoom();
                break;
            case State.playerUsingSkills:
                myCamera.SetCameraPosSlightZoom();
                break;
            case State.PlayerUsingItems:
                myCamera.SetCameraPosSlightZoom();
                break;
            case State.PlayerInput:
                {
                    if (prevState == State.PlayerInput)
                        myCamera.SetCameraPosOffsetZoom();
                    else
                        myCamera.SetCameraPosZoom();
                }
                break;
            case State.PlayerAllocate:
                myCamera.SetCameraPosOffsetZoom();
                break;
            case State.FreeCamera:
                myCamera.SetCameraPosDefault();
                break;

            case State.PlayerOppSelecting:
                myCamera.SetCameraPosDefault();
                break;
            case State.PlayerOppOptions:
                myCamera.SetCameraPosZoom();
                break;

            case State.EnemyTurn:
                myCamera.SetCameraPosFar();
                break;
            case State.HazardTurn:
                myCamera.SetCameraPosFar();
                break;
            case State.PlayerTransition:
                myCamera.SetCameraPosFar();
                break;
        }
    }
    public void enterStateTransition()
    {

        if (currentState != State.SceneRunning && GetState() != State.EnemyTurn && currentState != State.HazardTurn)
        {
            currentState = State.PlayerTransition;
            updateConditionals();

            //TextObjectHandler.UpdateText(textHolder.subphaseTracker, "Resulting");
            //TextObjectHandler.UpdateText(textHolder.shadowSubphaseTracker, "Resulting");

            myCamera.UpdateCamera();
            myCamera.SetCameraPosFar();
        }
    }
    public void returnState(bool shownew = true, bool ignoreEnemyTurn = false, bool playSnd = true)
    {

        if (GetState() == State.EnemyTurn || GetState() == State.HazardTurn)
        {
            if (ignoreEnemyTurn)
            {
                NextTurn("soft reset");
            }

            return;
        }
        if (player)
        {
            if (player.current)
            {
                if (player.current.ANIM)
                {
                    player.current.ANIM.runAnyway = false;
                    player.current.SHADOW.SCRIPT.runAnyway = false;
                }
            }
        }

        // Debug.Log("returnin");

        if (player.current)
        {
            SpriteRenderer sr = player.current.GetComponent<SpriteRenderer>();
            if (sr)
            {
                if (sr.color != Color.white)
                {
                    if (!player.current.currentTile.isInShadow)
                    {
                        sr.color = Color.white;

                    }
                    else
                    {
                        AttemptToGoIntoShadows(player.current);
                    }
                }
            }
        }


        if (GetState() == State.PlayerOppOptions)
        {
            if (currentObject)
            {
                if (currentObject.GetComponent<LivingObject>())
                {
                    currentObject.GetComponent<LivingObject>().OPP_SLOTS.SKILLS.Clear();
                }
            }

        }
        if (playSnd == true)
        {

            PlayExitSnd();
        }
        if (prevState == State.PlayerOppOptions && currentState == State.PlayerAttacking)
        {
            currentState = prevState;
            showCurrentState();

            prevState = State.PlayerTransition;
            return;
        }
        if (menuStack.Count > 1)
        {
            menuStackEntry currEntry = menuStack[menuStack.Count - 1];
            menuStackEntry prevEntry = menuStack[menuStack.Count - 2];
            currentState = prevEntry.state;
            MenuManager menuManager = GetComponent<MenuManager>();
            // Debug.Log("curr :" + currEntry.menu);
            // Debug.Log("prev :" + prevEntry.menu);
            if (shownew)
            {
                switch (currEntry.menu)
                {
                    case currentMenu.command:
                        {
                            if (player.current)
                            {
                                if (player.current.GetComponent<AnimationScript>())
                                {
                                    AnimationScript anim = player.current.GetComponent<AnimationScript>();

                                    anim.LoadList(anim.idlePath);
                                }
                            }
                            currentState = State.PlayerInput;

                            menuManager.ShowCommandCanvas();
                            ComfirmMoveGridObject(tempGridObj, GetTileIndex(player.current));
                            if (currentObject)
                                ShowGridObjectAffectArea(currentObject);
                        }
                        break;
                    case currentMenu.act:
                        {
                            currentState = State.PlayerInput;
                            if (currentObject)
                                ShowGridObjectAffectArea(currentObject);
                            menuManager.ShowActCanvas();
                            ComfirmMoveGridObject(tempGridObj, GetTileIndex(player.current));
                        }
                        break;
                    case currentMenu.invMain:
                        {
                            currentState = State.PlayerAllocate;
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

                    case currentMenu.CmdItems:
                        menuManager.ShowItemCanvas(11, currentObject.GetComponent<LivingObject>());
                        myCamera.infoObject = player.current;
                        myCamera.showActions = true;
                        myCamera.currentTile = player.current.currentTile;
                        myCamera.UpdateCamera();
                        break;
                    case currentMenu.CmdSpells:
                        menuManager.ShowItemCanvas(12, currentObject.GetComponent<LivingObject>());
                        myCamera.infoObject = player.current;
                        myCamera.showActions = true;
                        myCamera.currentTile = player.current.currentTile;
                        myCamera.UpdateCamera();
                        break;
                    case currentMenu.Strikes:
                        menuManager.ShowItemCanvas(13, currentObject.GetComponent<LivingObject>());
                        myCamera.infoObject = player.current;
                        myCamera.showActions = true;
                        myCamera.currentTile = player.current.currentTile;
                        myCamera.UpdateCamera();
                        break;
                    case currentMenu.none:
                        menuManager.ShowNone();
                        break;
                    default:
                        ShowGridObjectAffectArea(currentObject);
                        menuManager.ShowCommandCanvas();
                        currentState = State.PlayerInput;
                        break;
                }
            }

            GetComponent<InventoryMangager>().currentIndex = currEntry.index;
            GetComponent<InventoryMangager>().ForceSelect();
            menuStack.Remove(menuStack[menuStack.Count - 1]);
        }
        else if (menuStack.Count == 1)
        {
            if (currentObject)
            {
                if (currentObject == myCamera.infoObject)
                {

                    tempObject.transform.position = currentObject.transform.position;
                    tempGridObj.currentTile = currentObject.currentTile;
                }
            }
            player.current = null;
            if (prevState == State.EnemyTurn)
            {
                currentState = State.EnemyTurn;
                return;
            }
            else if (prevState == State.HazardTurn)
            {
                currentState = State.HazardTurn;
            }
            else
            {
                turnImgManger.UpdateSelection(-1);
                currentState = State.FreeCamera;
            }
            menuManager.ShowNone();
            menuStack.Remove(menuStack[0]);
        }
        else
        {
            if (currentState == State.EventRunning)
            {
                menuManager.ShowNone();
                currentState = State.FreeCamera;
            }
            else
            {
                //  currentState = State.ChangeOptions;
                // menuManager.ShowOptions();
            }
        }

        if (doubleAdjOppTiles.Count <= 0)
        {
            if (GetState() == State.PlayerOppSelecting)
            {
                CleanMenuStack();
            }
        }
        if (GetState() == State.FreeCamera)
        {
            myCamera.SetCameraPosDefault();
        }
        if (GetState() == State.PlayerInput)
        {
            if (prevState == State.PlayerInput)
            {
                myCamera.SetCameraPosZoom();
            }
            else
            {
                myCamera.SetCameraPosOffsetZoom();
            }
        }
        if (GetState() == State.PlayerAllocate)
        {
            myCamera.SetCameraPosOffsetZoom();
        }
        if (GetState() != State.PlayerAttacking)
        {
            if (GetState() != State.EnemyTurn)
            {
                for (int k = 0; k < liveEnemies.Count; k++)
                {
                    liveEnemies[k].ShowHideWeakness(EHitType.normal, false);

                }
            }
        }
        if (GetState() == State.PlayerAttacking)
        {
            myCamera.attackingCheck = true;
            potential.pulsing = true;
        }
        else if (GetState() == State.PlayerUsingItems)
        {
            if (player.currentItem.ITYPE == ItemType.dmg)
            {
                myCamera.attackingCheck = true;
                potential.pulsing = true;
            }
            else
            {
                myCamera.potentialDamage = 0;
                myCamera.attackingCheck = false;
                potential.pulsing = false;
            }
        }
        else
        {
            myCamera.potentialDamage = 0;
            myCamera.attackingCheck = false;
            potential.pulsing = false;
        }

        myCamera.UpdateCamera();
        updateConditionals();
        newSkillEvent.caller = null;

        UpdateCameraPosition();


        // TextObjectHandler.UpdateText(textHolder.subphaseTracker, currentState.ToString());
        //TextObjectHandler.UpdateText(textHolder.shadowSubphaseTracker, currentState.ToString());

    }

    public void requestTurnImgUpdate(LivingObject obj)
    {
        for (int i = 0; i < turnOrder.Count; i++)
        {
            if (turnOrder[i] == obj)
            {
                turnImgManger.UpdateSelection(i);
            }
        }
    }

    public void showCurrentState()
    {
        ShowWhite();
        //if (menuStack.Count == 2)
        //{
        //    if ()
        //        currentState = State.PlayerInput;
        //}
        if (prevState == State.PlayerTransition)
        {

            menuStackEntry currEntry; //= menuStack[menuStack.Count - 1];
            if (menuStack.Count > 1)
                currEntry = menuStack[menuStack.Count - 1];
            else if (menuStack.Count == 1)
                currEntry = menuStack[0];
            else
            {
                currEntry = defaultEntry;
                menuStack.Add(defaultEntry);
            }

            switch (currEntry.menu)
            {
                case currentMenu.command:
                    {

                        currentState = State.PlayerInput;
                        menuManager.ShowCommandCanvas();
                        ShowGridObjectAffectArea(currentObject);
                        if (player.current)
                            ComfirmMoveGridObject(tempGridObj, GetTileIndex(player.current));
                    }
                    break;
                case currentMenu.act:
                    {

                        currentState = State.PlayerInput;
                        ShowGridObjectAffectArea(currentObject);
                        menuManager.ShowActCanvas();
                        if (player.current)
                            ComfirmMoveGridObject(tempGridObj, GetTileIndex(player.current));
                    }
                    break;
                case currentMenu.invMain:
                    {
                        currentState = State.PlayerAllocate;
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
                        if (player.current)
                            myCamera.infoObject = player.current;
                        myCamera.showActions = true;
                        if (player.current)
                            myCamera.currentTile = player.current.currentTile;
                        if (GetState() == State.PlayerTransition)
                            returnState();
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

                case currentMenu.CmdItems:

                    if (GetState() == State.PlayerTransition)
                    {
                        returnState();
                    }
                    menuManager.ShowItemCanvas(11, currentObject.GetComponent<LivingObject>());
                    myCamera.infoObject = player.current;
                    myCamera.showActions = true;
                    myCamera.currentTile = player.current.currentTile;
                    if (GetState() == State.PlayerTransition)
                        returnState();
                    myCamera.UpdateCamera();

                    break;
                case currentMenu.CmdSpells:

                    menuManager.ShowItemCanvas(12, currentObject.GetComponent<LivingObject>());
                    myCamera.infoObject = player.current;
                    myCamera.showActions = true;
                    myCamera.currentTile = player.current.currentTile;
                    if (GetState() == State.PlayerTransition)
                        returnState();
                    myCamera.UpdateCamera();
                    break;
                case currentMenu.Strikes:

                    menuManager.ShowItemCanvas(13, currentObject.GetComponent<LivingObject>());
                    myCamera.infoObject = player.current;
                    myCamera.showActions = true;
                    myCamera.currentTile = player.current.currentTile;
                    if (GetState() == State.PlayerTransition)
                        returnState();
                    myCamera.UpdateCamera();
                    break;
                default:
                    ShowGridObjectAffectArea(currentObject);
                    menuManager.ShowCommandCanvas();
                    currentState = State.PlayerInput;
                    break;
            }
            invManager.currentIndex = currEntry.index;
            invManager.ForceSelect();
        }
        else if (GetState() == State.PlayerTransition)
        {
            currentState = prevState;
            showCurrentState();
        }
        else
        {
            switch (GetState())
            {
                case State.PlayerInput:
                    {
                        if (menuStack.Count == 1)
                        {

                            currentState = State.PlayerInput;
                            menuManager.ShowCommandCanvas();
                            if (currentObject)
                                ShowGridObjectAffectArea(currentObject);
                            ComfirmMoveGridObject(tempGridObj, GetTileIndex(player.current));
                        }

                        else if (menuStack.Count > 1)
                        {
                            currentState = State.PlayerInput;
                            if (currentObject)
                                ShowGridObjectAffectArea(currentObject);
                            menuManager.ShowActCanvas();
                            ComfirmMoveGridObject(tempGridObj, GetTileIndex(player.current));
                        }
                        else
                        {
                            //Debug.Log("um.. but why");

                        }
                    }
                    break;
                case State.PlayerAllocate:
                    {
                        currentState = State.PlayerAllocate;
                        menuManager.ShowInventoryCanvas();
                        if (currentObject)
                            ShowGridObjectAffectArea(currentObject);
                        ComfirmMoveGridObject(tempGridObj, GetTileIndex(player.current));
                    }
                    break;
                case State.PlayerMove:
                    {
                        if (currentObject)
                            ShowGridObjectMoveArea(player.current);
                    }
                    break;
                case State.PlayerAttacking:

                    break;
                case State.PlayerEquippingMenu:
                    break;
                case State.PlayerEquipping:
                    {
                        if (invManager)
                            menuManager.ShowItemCanvas(invManager.lastIndex, currentObject.GetComponent<LivingObject>());
                        if (player.current)
                            myCamera.infoObject = player.current;
                        myCamera.showActions = true;
                        if (player.current)
                            myCamera.currentTile = player.current.currentTile;
                        myCamera.UpdateCamera();
                    }
                    break;
                case State.playerUsingSkills:
                    {
                        if (menuStack.Count == 3)
                        {
                            if (invManager)
                                menuManager.ShowItemCanvas(invManager.lastIndex, currentObject.GetComponent<LivingObject>());
                            if (player.current)
                                myCamera.infoObject = player.current;
                            myCamera.showActions = true;
                            if (player.current)
                                myCamera.currentTile = player.current.currentTile;
                            myCamera.UpdateCamera();
                        }
                        else if (menuStack.Count == 2)
                        {
                            currentState = State.PlayerInput;
                            showCurrentState();
                        }
                    }
                    break;
                case State.PlayerEquippingSkills:
                    break;
                case State.PlayerSkillsMenu:
                    break;


                case State.PlayerSelectItem:
                    {
                        if (invManager)
                            menuManager.ShowItemCanvas(invManager.lastIndex, currentObject.GetComponent<LivingObject>());
                        if (player.current)
                            myCamera.infoObject = player.current;
                        myCamera.showActions = true;
                        if (player.current)
                            myCamera.currentTile = player.current.currentTile;
                        myCamera.UpdateCamera();
                    }
                    break;
                case State.PlayerOppSelecting:
                    break;
                case State.PlayerOppOptions:
                    {
                        if (invManager)
                            menuManager.ShowItemCanvas(invManager.lastIndex, currentObject.GetComponent<LivingObject>());
                        if (currentObject)
                            myCamera.infoObject = currentObject;
                        myCamera.showActions = true;
                        if (currentObject)
                            myCamera.currentTile = currentObject.currentTile;
                        myCamera.UpdateCamera();
                        updateConditionals();
                    }
                    break;
                case State.PlayerOppMove:
                    break;

                case State.PlayerUsingItems:
                    break;

            }
        }
        UpdateCameraPosition();
        myCamera.UpdateCamera();
        updateConditionals();

    }
    public void CleanMenuStack(bool toCam = false, bool checkForNext = true, bool playySnd = true)
    {

        if (toCam)
        {

            while (menuStack.Count > 0)
            {
                returnState(false, true, false);
                myCamera.UpdateCamera();
            }
        }
        else
        {

            while (menuStack.Count > 2)
            {
                returnState(true, false, false);
                myCamera.UpdateCamera();
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
                NextTurn("manager");
            }
        }
        showCurrentState();

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

        //Debug.Log("Crevent Event: " + name + "  at " + index + " from " + caller.name);
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
        //   Debug.Log("Event count:" + eventManager.gridEvents.Count);
        return newEvent;
    }

    public TextEvent CreateTextEvent(Object caller, string data, string name, RunableEvent run, StartupEvent start = null, int index = -1)
    {
        TextEvent newEvent = new TextEvent();
        if (options)
        {
            if (options.displayMessages == true)
            {
                if (textManager.currentEvent.data != data)
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
        }
        return newEvent;

    }

    public void MovetoMousePos(LivingObject movedObj)
    {
        if (currentState == State.PlayerMove)
        {

            if (movedObj == null)
            {
                returnState();
            }
            if (movedObj.currentTile == null)
            {
                returnState();
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {

                Vector3 w = hit.point;
                w.x = Mathf.Round(w.x);
                w.y = Mathf.Round(w.y);
                w.z = Mathf.Round(w.z);
                GameObject hitoobject = hit.transform.gameObject;
                //   int hitindex = GetTileIndex(w);
                if (hitoobject.GetComponent<TileScript>())
                {
                    TileScript hitTile = hitoobject.GetComponent<TileScript>();
                    bool alreadySelected = false;
                    if (myCamera.currentTile == hitTile && movedObj.transform.position == hitTile.transform.position + new Vector3(0, 0.5f, 0.12f))
                    {
                        alreadySelected = true;

                    }
                    if (!hitTile.isOccupied)
                    {


                        if (alreadySelected)
                        {
                            if (hitTile != null)
                            {
                                movedObj.transform.position = hitTile.transform.position + new Vector3(0, 0.5f, 0.12f);
                                myCamera.currentTile = hitTile;


                                if (ComfirmMenuAction(movedObj))
                                {
                                    ComfirmMoveGridObject(movedObj, hitTile.listindex);
                                    if (currentState != State.PlayerOppMove && currentState != State.PlayerOppOptions)
                                    {
                                        if (newSkillEvent.caller == null)
                                        {
                                            if (player.current)
                                            {
                                                if (player.current.GetComponent<AnimationScript>())
                                                {
                                                    AnimationScript anim = player.current.GetComponent<AnimationScript>();

                                                    anim.LoadList(anim.idlePath);
                                                }
                                                DidCompleteTutorialStep();
                                                player.current.TakeAction();

                                            }
                                            // CleanMenuStack();
                                        }
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

                                    bool res = commandItems[0].ComfirmAction(movedObj, MenuItemType.Move, currentTutorial);
                                    if (res)
                                    {

                                        if (currentState != State.PlayerOppMove && currentState != State.PlayerOppOptions)
                                        {
                                            if (newSkillEvent.caller == null)
                                            {
                                                // player.current.TakeAction();
                                                // Debug.Log("comp");
                                                // CleanMenuStack();
                                            }
                                        }
                                        else
                                        {
                                            ComfirmMenuAction(oppObj);
                                            oppEvent.caller = null;

                                            CreateEvent(this, null, "return state event", BufferedReturnEvent);
                                            CreateEvent(this, null, "return state event", BufferedReturnEvent);
                                            CreateEvent(this, null, "return state event", BufferedReturnEvent);
                                        }
                                        DidCompleteTutorialStep();
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                            else
                            {
                                myCamera.currentTile = hitTile;
                                if (!movingObj)
                                {
                                    movingObj = true;
                                    Vector3 targetposition = hitTile.transform.position + new Vector3(0, 0.5f, 0.12f);
                                    StartCoroutine(Move(movedObj, targetposition, hitTile));
                                }
                            }
                        }
                        else
                        {

                            float tempX = hitTile.transform.position.x;
                            float tempY = hitTile.transform.position.z;

                            float objX = movedObj.currentTile.transform.position.x;
                            float objY = movedObj.currentTile.transform.position.z;


                            xDist = Mathf.Abs(tempX - objX);
                            yDist = Mathf.Abs(tempY - objY);
                            xDist /= 2;
                            yDist /= 2;

                            if (xDist + yDist <= player.current.MOVE_DIST && StartCanMoveCheck(player.current, player.current.currentTile, hitTile))
                            {

                                tempObject.transform.position = hitTile.transform.position;
                                tempGridObj.currentTile = hitTile;
                                // movedObj.transform.position = hitTile.transform.position + new Vector3(0, 0.5f);
                                //myCamera.currentTile = hitTile;
                                if (!movingObj)
                                {
                                    movingObj = true;
                                    Vector3 targetposition = hitTile.transform.position + new Vector3(0, 0.5f, 0.12f);
                                    StartCoroutine(Move(movedObj, targetposition, hitTile));
                                }
                            }




                        }
                    }
                    else if (hitTile == movedObj.currentTile)
                    {
                        if (alreadySelected)
                        {
                            CancelMenuAction(movedObj);

                        }
                        else
                        {
                            tempObject.transform.position = hitTile.transform.position;
                            tempGridObj.currentTile = hitTile;

                            if (!movingObj)
                            {
                                movingObj = true;
                                Vector3 targetposition = hitTile.transform.position + new Vector3(0, 0.5f, 0.12f);
                                StartCoroutine(Move(movedObj, targetposition, hitTile));
                            }
                        }
                    }
                }
            }
            myCamera.UpdateCamera();

        }
    }


    public IEnumerator Move(GridObject griddy, Vector3 targetposition, TileScript hitTile)
    {
        bool loaded = false;

        //if (griddy.GetComponent<AnimationScript>())
        //{
        //    AnimationScript anim = griddy.GetComponent<AnimationScript>();


        //    if (anim.hasMove)
        //    {
        //        anim.LoadList(anim.movePath);
        //        loaded = true;
        //    }
        //}

        // myCamera.currentTile = null;
        if (targetposition.y > griddy.transform.position.y)
            griddy.transform.position = new Vector3(griddy.transform.position.x, targetposition.y, griddy.transform.position.z);
        while (griddy.transform.position != targetposition)
        {

            griddy.transform.position = Vector3.MoveTowards(griddy.transform.position, targetposition, 0.5f);

            //float dist = Vector3.Distance(griddy.transform.position, targetposition);

            //myCamera.currentTile


            yield return null;

        }

        if (loaded == true)
        {
            //if (griddy.GetComponent<AnimationScript>())
            //{
            //    AnimationScript anim = griddy.GetComponent<AnimationScript>();

            //    anim.LoadList(anim.idlePath);
            //    loaded = true;

            //}
        }

        griddy.transform.position = targetposition;
        myCamera.currentTile = hitTile;
        if (hitTile.isInShadow)
        {
            if (griddy.RENDERER != null)
            {
                AttemptToGoIntoShadows(griddy);
            }
        }
        else
        {
            if (griddy.RENDERER != null)
            {
                griddy.RENDERER.color = Color.white;
            }
        }

        movingObj = false;
    }

    public LivingObject GetActor(int id)
    {
        GameObject jax = Instantiate(PlayerObject, Vector2.zero, Quaternion.identity);
        jax.SetActive(true);
        ActorSetup asetup = jax.GetComponent<ActorSetup>();
        asetup.characterId = id;
        LivingObject liveJax = jax.GetComponent<LivingObject>();
        liveJax.FACTION = Faction.ally;
        liveJax.Setup();
        return liveJax;
    }
    private DialogManager dialog;
    public void SetScene(SceneContainer scene)
    {
        if (eventImage)
        {
            eventImage.gameObject.SetActive(false);
        }
        talkPanel.gameObject.SetActive(true);
        currentScene = scene;
        currentState = State.SceneRunning;
        talkPanel.scene = currentScene;
        currentScene.index = 0;
        //UpdateScene();
        currentScene.isRunning = true;
        menuManager.ShowNone();
        talkPanel.text.text = "";

        CreateEvent(this, null, "scene0 event", CheckSceneRunning, null, 0);


        if (currentScene.eventIndexs.Contains(0))
        {
            SceneEventContainer sceneEvent = currentScene.sceneEvents[0];
            for (int i = 0; i < currentScene.sceneEvents.Count; i++)
            {
                sceneEvent = currentScene.sceneEvents[i];
                if (sceneEvent.intercept == 0)
                {
                    ExecuteIntercept(sceneEvent);
                }
            }
        }

        if (dialog == null)
        {
            dialog = GameObject.FindObjectOfType<DialogManager>();
        }
        if (dialog != null)
        {
            var dialogTexts = new List<DialogData>();
            for (int i = 0; i < currentScene.speakertext.Count; i++)
            {
                //Debug.Log(scene.speakerFace);
                dialogTexts.Add(new DialogData(currentScene.speakertext[i], currentScene.speakerFace[i], currentScene.speakerNames[i]));
            }
            dialog.state = Doublsb.Dialog.State.Active;
            dialog.Show(dialogTexts);
        }

    }

    public void UpdateScene()
    {
        if (currentScene.speakerNames.Count > 0)
        {
            int index = currentScene.index;
            string objName = currentScene.speakerNames[index].Trim();
            if (dialog != null)
            {

                if (dialog.FIRST.name == objName)
                {
                    talkPanel.faceName.text = objName;

                }
                else if (dialog.SECOND.name == objName)
                {
                    talkPanel.otherName.text = objName;

                }
                else
                {

                    talkPanel.otherName.text = objName;

                }

            }
            else
            {

                talkPanel.faceName.text = objName;
            }

            string shrtName = Common.GetShortName(objName);
            talkPanel.faceImage.sprite = currentScene.speakerFace[index];
            if (talkPanel.faceImage.sprite == null)
                talkPanel.faceImage.sprite = Resources.LoadAll<Sprite>("" + shrtName + "/Idle/")[0];
            string input = currentScene.speakertext[index];
            string regex = "(/.*/)";
            //talkPanel.text.text = Regex.Replace(input, regex, ""); //currentScene.speakertext[index];


            if (dialog == null)
            {
                talkPanel.text.text = Regex.Replace(input, regex, "");
            }

            if (currentScene.eventIndexs.Contains(index))
            {
                SceneEventContainer sceneEvent = currentScene.sceneEvents[0];
                for (int i = 0; i < currentScene.sceneEvents.Count; i++)
                {
                    sceneEvent = currentScene.sceneEvents[i];
                    if (sceneEvent.intercept == index)
                    {
                        ExecuteIntercept(sceneEvent);
                    }
                }
            }
        }
    }
    private void ExecuteIntercept(SceneEventContainer sceneEvent)
    {

        switch (sceneEvent.scene)
        {
            case SceneEvent.move:
                break;
            case SceneEvent.showimage:
                {
                    if (eventImage)
                    {
                        eventImage.myImage.sprite = Resources.LoadAll<Sprite>("SceneImg")[sceneEvent.data];
                        eventImage.gameObject.SetActive(true);
                    }
                }
                break;
            case SceneEvent.moveToTarget:
                {
                    if (sceneEvent.data < tileMap.Count)
                    {

                        tempGridObj.transform.position = tileMap[sceneEvent.data].transform.position;
                        ComfirmMoveGridObject(tempGridObj, sceneEvent.data);
                    }
                }
                break;
            case SceneEvent.hideimage:
                {
                    if (eventImage)
                    {
                        eventImage.gameObject.SetActive(false);
                    }
                }
                break;
            case SceneEvent.shake:
                break;
            case SceneEvent.scaleimage:
                if (eventImage)
                {
                    eventImage.transform.localScale = new Vector3(sceneEvent.data, sceneEvent.data, 1);
                }
                break;
            case SceneEvent.blackout:
                {
                    if (eventImage)
                    {
                        UnityEngine.UI.Image img = eventImage.transform.parent.GetComponent<UnityEngine.UI.Image>();
                        if (img)
                        {
                            img.color = new Color(0, 0, 0, 1.0f);
                        }
                    }
                }
                break;
            case SceneEvent.dim:
                {
                    if (eventImage)
                    {
                        UnityEngine.UI.Image img = eventImage.transform.parent.GetComponent<UnityEngine.UI.Image>();
                        if (img)
                        {
                            img.color = new Color(0, 0, 0, 0.65f);
                        }
                    }
                }
                break;
            case SceneEvent.clear:
                {
                    if (eventImage)
                    {
                        UnityEngine.UI.Image img = eventImage.transform.parent.GetComponent<UnityEngine.UI.Image>();
                        if (img)
                        {
                            img.color = new Color(0, 0, 0, 0.0f);
                        }
                    }
                }
                break;

            case SceneEvent.saveData:
                {
                    StorySection saveData = (StorySection)sceneEvent.data;
                    SaveChapterData(saveData.ToString());
                }
                break;

            case SceneEvent.endChapter:
                {
                    if (currentState == State.PlayerWait)
                        return;

                    currentState = State.PlayerWait;
                    if (menuManager)
                    {
                        if (menuManager.cTT)
                        {
                            if (menuManager.loadingCanvas != null)
                            {
                                menuManager.loadingCanvas.SetActive(true);
                            }

                            switch (Common.currentStory)
                            {
                                case StorySection.none:
                                    menuManager.cTT.SetColors(Color.red, Common.pink);
                                    menuManager.ToggleCanvas(menuManager.animationCanvas);
                                    menuManager.cTT.chapterText.text = "You";
                                    menuManager.cTT.beginEndText.text = "Broke it!";
                                    menuManager.ToggleCanvas(menuManager.animationCanvas);
                                    break;
                                case StorySection.JaxSaveSlot1:
                                    {
                                        menuManager.cTT.SetColors(Color.red, Common.pink);
                                        menuManager.ToggleCanvas(menuManager.animationCanvas);
                                        menuManager.cTT.chapterText.text = "Chapter 1";
                                        menuManager.cTT.beginEndText.text = "End!";
                                        menuManager.ToggleCanvas(menuManager.animationCanvas);

                                        LeanTween.moveX(gameObject, 30, 275 * Time.deltaTime).setOnComplete(() =>
                                        {
                                            menuManager.ToggleCanvas(menuManager.animationCanvas);
                                            menuManager.cTT.chapterText.text = "DEMO";
                                            menuManager.cTT.beginEndText.text = "COMPLETE!";
                                            menuManager.ToggleCanvas(menuManager.animationCanvas);

                                            LeanTween.moveX(gameObject, 90, 275 * Time.deltaTime).setOnComplete(() =>
                                            {
                                                menuManager.cTT.SetColors(Color.magenta, Common.orange);

                                                menuManager.ToggleCanvas(menuManager.animationCanvas);
                                                menuManager.cTT.nameText.text = "Thank You";
                                                menuManager.cTT.chapterText.text = "For";
                                                menuManager.cTT.beginEndText.text = "Playing!";
                                                menuManager.ToggleCanvas(menuManager.animationCanvas);

                                                LeanTween.moveX(gameObject, 250, 275 * Time.deltaTime).setOnComplete(() =>
                                                {
                                                    SceneManager.LoadSceneAsync("Start");
                                                });
                                            });


                                        });
                                    }
                                    break;
                                case StorySection.JaxSaveSlot2:
                                    break;
                                case StorySection.JaxSaveSlot3:
                                    break;
                                case StorySection.JaxSaveSlot4:
                                    break;
                                case StorySection.JaxSaveSlot5:
                                    break;
                                case StorySection.JaxSaveSlotPrologue:
                                    menuManager.cTT.SetColors(Color.red, Common.pink);
                                    menuManager.cTT.nameText.text = "Jax Drix";
                                    menuManager.cTT.chapterText.text = "Prologue";
                                    menuManager.cTT.beginEndText.text = "End";
                                    menuManager.ToggleCanvas(menuManager.animationCanvas);
                                    LeanTween.moveX(gameObject, 30, 275 * Time.deltaTime).setOnComplete(() =>
                                    {
                                        Debug.Log("0");
                                        menuManager.ToggleCanvas(menuManager.animationCanvas);
                                        menuManager.cTT.chapterText.text = "Chapter 1";
                                        menuManager.cTT.beginEndText.text = "Unlocked!";
                                        menuManager.ToggleCanvas(menuManager.animationCanvas);

                                        LeanTween.moveX(gameObject, 90, 275 * Time.deltaTime).setOnComplete(() =>
                                        {
                                            Debug.Log("1");
                                            menuManager.cTT.SetColors(Color.green, Common.lime);

                                            menuManager.ToggleCanvas(menuManager.animationCanvas);
                                            menuManager.cTT.nameText.text = "Zeffron Drix";
                                            menuManager.cTT.chapterText.text = "Story";
                                            menuManager.cTT.beginEndText.text = "Unlocked!";
                                            menuManager.ToggleCanvas(menuManager.animationCanvas);

                                            LeanTween.moveX(gameObject, 250, 275 * Time.deltaTime).setOnComplete(() =>
                                            {
                                                Debug.Log("2");
                                                SceneManager.LoadSceneAsync("Start");
                                            });
                                        });


                                    });



                                    break;
                                case StorySection.ZeffSaveSlot1:
                                    break;
                                case StorySection.ZeffSaveSlot2:
                                    break;
                                case StorySection.ZeffSaveSlot3:
                                    break;
                                case StorySection.ZeffSaveSlot4:
                                    break;
                                case StorySection.ZeffSaveSlot5:
                                    break;
                                case StorySection.ZeffSaveSlotPrologue:
                                    SaveGame(StorySection.ZeffSaveSlot1.ToString());

                                    menuManager.cTT.SetColors(Color.red, Common.pink);
                                    menuManager.ToggleCanvas(menuManager.animationCanvas);
                                    menuManager.cTT.chapterText.text = "Chapter 1";
                                    menuManager.cTT.beginEndText.text = "End!";
                                    menuManager.ToggleCanvas(menuManager.animationCanvas);

                                    LeanTween.moveX(gameObject, 30, 275 * Time.deltaTime).setOnComplete(() =>
                                    {
                                        menuManager.ToggleCanvas(menuManager.animationCanvas);
                                        menuManager.cTT.chapterText.text = "DEMO";
                                        menuManager.cTT.beginEndText.text = "COMPLETE!";
                                        menuManager.ToggleCanvas(menuManager.animationCanvas);

                                        LeanTween.moveX(gameObject, 90, 275 * Time.deltaTime).setOnComplete(() =>
                                        {
                                            menuManager.cTT.SetColors(Color.magenta, Common.orange);

                                            menuManager.ToggleCanvas(menuManager.animationCanvas);
                                            menuManager.cTT.nameText.text = "Thank You";
                                            menuManager.cTT.chapterText.text = "For";
                                            menuManager.cTT.beginEndText.text = "Playing!";
                                            menuManager.ToggleCanvas(menuManager.animationCanvas);

                                            LeanTween.moveX(gameObject, 250, 275 * Time.deltaTime).setOnComplete(() =>
                                            {
                                                SceneManager.LoadSceneAsync("Start");
                                            });
                                        });


                                    });
                                    break;
                                case StorySection.PryinaSaveSlot1:
                                    break;
                                case StorySection.PryinaSaveSlot2:
                                    break;
                                case StorySection.PryinaSaveSlot3:
                                    break;
                                case StorySection.PryinaSaveSlot4:
                                    break;
                                case StorySection.PryinaSaveSlot5:
                                    break;
                                case StorySection.PyrinaPrologue:
                                    break;
                                case StorySection.FlaraSaveSlot1:
                                    break;
                                case StorySection.FlaraSaveSlot2:
                                    break;
                                case StorySection.FlaraSaveSlot3:
                                    break;
                                case StorySection.FlaraSaveSlot4:
                                    break;
                                case StorySection.FlaraSaveSlot5:
                                    break;
                                case StorySection.FlaraSaveSlotPrologue:
                                    break;
                                case StorySection.SapphireSaveSlot1:
                                    break;
                                case StorySection.SapphireSaveSlot2:
                                    break;
                                case StorySection.SapphireSaveSlot3:
                                    break;
                                case StorySection.SapphireSaveSlot4:
                                    break;
                                case StorySection.SapphireSaveSlot5:
                                    break;
                                case StorySection.SapphireSaveSlotPrologue:
                                    break;
                                case StorySection.LukonSaveSlot1:
                                    break;
                                case StorySection.LukonSaveSlot2:
                                    break;
                                case StorySection.LukonSaveSlot3:
                                    break;
                                case StorySection.LukonSaveSlot4:
                                    break;
                                case StorySection.LukonSaveSlot5:
                                    break;
                                case StorySection.LukonSaveSlotPrologue:
                                    break;
                                default:
                                    {
                                        menuManager.cTT.SetColors(Color.red, Common.pink);
                                        menuManager.ToggleCanvas(menuManager.animationCanvas);
                                        menuManager.cTT.chapterText.text = "You";
                                        menuManager.cTT.beginEndText.text = "Defaulted!";
                                        menuManager.ToggleCanvas(menuManager.animationCanvas);
                                        SceneManager.LoadSceneAsync("Start");

                                    }
                                    break;
                            }
                        }
                    }
                }
                break;
        }



    }
    public void NextScene()
    {

        if (currentScene.index + 1 < currentScene.speakerNames.Count)
        {
            currentScene.index++;
            UpdateScene();
        }
        else
        {

            EndScene();
        }

    }
    public void EndScene()
    {


        talkPanel.gameObject.SetActive(false);
        bool resetState = true;

        if (currentScene.sceneEvents.Count > 0)
        {

            SceneEventContainer sceneEvent = currentScene.sceneEvents[0];
            for (int i = 0; i < currentScene.sceneEvents.Count; i++)
            {
                // if (i < currentScene.index)
                {

                    sceneEvent = currentScene.sceneEvents[i];
                    if (sceneEvent.scene == SceneEvent.saveData || sceneEvent.scene == SceneEvent.endChapter)
                    {
                        if (sceneEvent.scene == SceneEvent.endChapter)
                        {

                            resetState = false;
                        }
                        ExecuteIntercept(sceneEvent);
                    }
                }
            }
        }
        if (resetState == true)
        {
            myCamera.PlayPreviousSoundTrack();

            if (eventManager.activeEvents > 0)
            {

                if (prevState == State.FreeCamera)
                {
                    enterStateTransition();
                }
                else
                {
                    returnState();
                }
            }
            else
            {
                returnState();
                showCurrentState();
            }

            currentScene.isRunning = false;
        }

    }
    public void CheckForMapChangeEvent(MapDetail checkMap)
    {

        if (cutscene != null)
        {
            (currentState, currentObjectiveString) = cutscene.CheckForMapChangeEvent(checkMap, this, defaultSceneEntry, talkPanel, currentScene, currentObjectiveString);
        }
        else
        {

            switch (defaultSceneEntry)
            {
                case 4:
                    {


                        if (checkMap.mapIndex == 4)
                        {
                            myCamera.PlaySoundTrack(3);
                            myCamera.previousClip = myCamera.musicClips[0];
                            if (talkPanel)
                            {

                                talkPanel.gameObject.SetActive(true);
                                currentScene = database.GetSceneData("Scene2");
                                currentState = State.SceneRunning;
                                talkPanel.scene = currentScene;
                                currentScene.index = 0;
                                UpdateScene();
                                currentScene.isRunning = true;
                                menuManager.ShowNone();
                                CreateEvent(this, null, "scene2 event", CheckSceneRunning, null, 0);
                            }
                        }
                        if (checkMap.mapIndex == 8)
                        {
                            if (Common.summonedJax == false)
                            {


                                if (talkPanel)
                                {

                                    talkPanel.gameObject.SetActive(true);
                                    currentScene = database.GetSceneData("Scene0");
                                    currentState = State.SceneRunning;
                                    talkPanel.scene = currentScene;
                                    currentScene.index = 0;
                                    UpdateScene();
                                    currentScene.isRunning = true;
                                    menuManager.ShowNone();
                                    CreateEvent(this, null, "scene0 event", CheckSceneRunning, null, 0);
                                }
                            }


                        }


                        if (checkMap.mapIndex == 10)
                        {
                            myCamera.PlaySoundTrack(3);
                            myCamera.previousClip = myCamera.musicClips[7];
                            if (talkPanel)
                            {

                                talkPanel.gameObject.SetActive(true);
                                currentScene = database.GetSceneData("Scene3");
                                currentState = State.SceneRunning;
                                talkPanel.scene = currentScene;
                                currentScene.index = 0;
                                UpdateScene();
                                currentScene.isRunning = true;
                                menuManager.ShowNone();
                                CreateEvent(this, null, "scene1 event", CheckSceneRunning, null, 0);
                            }
                        }

                        if (checkMap.mapIndex == 14)
                        {
                            myCamera.PlaySoundTrack(5);
                            myCamera.previousClip = myCamera.musicClips[4];
                            if (talkPanel)
                            {

                                talkPanel.gameObject.SetActive(true);
                                currentScene = database.GetSceneData("Scene4");
                                currentState = State.SceneRunning;
                                talkPanel.scene = currentScene;
                                currentScene.index = 0;
                                UpdateScene();
                                currentScene.isRunning = true;
                                menuManager.ShowNone();
                                CreateEvent(this, null, "scene1 event", CheckSceneRunning, null, 0);
                            }
                        }
                    }
                    break;

                case 15:
                    {


                        if (checkMap.mapIndex == 10)
                        {
                            myCamera.PlaySoundTrack(3);
                            myCamera.previousClip = myCamera.musicClips[6];
                            if (talkPanel)
                            {

                                talkPanel.gameObject.SetActive(true);
                                currentScene = database.GetSceneData("Scene3");
                                currentState = State.SceneRunning;
                                talkPanel.scene = currentScene;
                                currentScene.index = 0;
                                UpdateScene();
                                currentScene.isRunning = true;
                                menuManager.ShowNone();
                                CreateEvent(this, null, "scene1 event", CheckSceneRunning, null, 0);
                            }
                        }
                        if (checkMap.mapIndex == 15)
                        {
                            myCamera.PlaySoundTrack(3);
                            myCamera.previousClip = myCamera.musicClips[13];
                            if (talkPanel)
                            {

                                talkPanel.gameObject.SetActive(true);
                                currentScene = database.GetSceneData("Scene1");
                                currentState = State.SceneRunning;
                                talkPanel.scene = currentScene;
                                currentScene.index = 0;
                                UpdateScene();
                                currentScene.isRunning = true;
                                menuManager.ShowNone();
                                CreateEvent(this, null, "scene1 event", CheckSceneRunning, null, 0);
                            }
                        }
                        if (checkMap.mapIndex == 8)
                        {

                            if (Common.summonedZeffron == false)
                            {

                                if (talkPanel)
                                {

                                    talkPanel.gameObject.SetActive(true);
                                    currentScene = database.GetSceneData("JaxFindZeff");
                                    currentState = State.SceneRunning;
                                    talkPanel.scene = currentScene;
                                    currentScene.index = 0;
                                    UpdateScene();
                                    currentScene.isRunning = true;
                                    menuManager.ShowNone();
                                    CreateEvent(this, null, "scene0 event", CheckSceneRunning, null, 0);
                                }
                            }
                        }

                        if (checkMap.mapIndex == 14)
                        {
                            myCamera.PlaySoundTrack(5);
                            myCamera.previousClip = myCamera.musicClips[4];
                            if (talkPanel)
                            {

                                talkPanel.gameObject.SetActive(true);
                                currentScene = database.GetSceneData("Scene4");
                                currentState = State.SceneRunning;
                                talkPanel.scene = currentScene;
                                currentScene.index = 0;
                                UpdateScene();
                                currentScene.isRunning = true;
                                menuManager.ShowNone();
                                CreateEvent(this, null, "scene1 event", CheckSceneRunning, null, 0);
                            }
                        }
                    }
                    break;

                case 26:
                    {
                        if (checkMap.mapIndex == 26)
                        {
                            myCamera.PlaySoundTrack(10);
                            myCamera.previousClip = myCamera.musicClips[13];
                            if (talkPanel)
                            {
                                List<tutorialStep> tSteps = new List<tutorialStep>();
                                List<int> tClar = new List<int>();



                                tSteps.Add(tutorialStep.showTutorial);
                                tClar.Add(27);
                                tSteps.Add(tutorialStep.moveToPosition);
                                tClar.Add(20);
                                tSteps.Add(tutorialStep.showTutorial);
                                tClar.Add(12);
                                // tClar.Add(23);
                                tSteps.Add(tutorialStep.useStrike);
                                tClar.Add(-1);
                                tSteps.Add(tutorialStep.showTutorial);
                                tClar.Add(17);
                                tSteps.Add(tutorialStep.useSpell);
                                tClar.Add(-1);

                                PrepareTutorial(tSteps, tClar);
                                talkPanel.gameObject.SetActive(true);
                                currentScene = database.GetSceneData("JaxPrologue");
                                currentState = State.SceneRunning;
                                talkPanel.scene = currentScene;
                                currentScene.index = 0;
                                SetScene(currentScene);

                            }




                        }
                        if (checkMap.mapIndex == 15)
                        {
                            myCamera.PlaySoundTrack(3);
                            myCamera.previousClip = myCamera.musicClips[13];
                            if (talkPanel)
                            {

                                talkPanel.gameObject.SetActive(true);
                                currentScene = database.GetSceneData("Scene1");
                                currentObjectiveString = "Get rid of the Glyph";
                                currentState = State.SceneRunning;
                                talkPanel.scene = currentScene;
                                currentScene.index = 0;
                                SetScene(currentScene);
                            }
                        }
                        if (checkMap.mapIndex == 17)
                        {

                            if (Common.summonedZeffron == false)
                            {
                                List<tutorialStep> tSteps = new List<tutorialStep>();
                                List<int> tClar = new List<int>();

                                tSteps.Add(tutorialStep.showTutorial);
                                tClar.Add(29);
                                PrepareTutorial(tSteps, tClar);

                                myCamera.PlaySoundTrack(6);
                                myCamera.previousClip = myCamera.musicClips[16];

                                if (talkPanel)
                                {
                                    //MoveCameraAndShow(liveZeff);
                                    talkPanel.gameObject.SetActive(true);
                                    currentScene = database.GetSceneData("JaxFindZeff");
                                    currentObjectiveString = "Work with Zeffron";
                                    currentState = State.SceneRunning;
                                    talkPanel.scene = currentScene;
                                    currentScene.index = 0;
                                    SetScene(currentScene);
                                    //currentScene.isRunning = true;
                                    //menuManager.ShowNone();
                                    //(this, null, "scene0 event", CheckSceneRunning, null, 0);
                                }
                            }
                        }
                        if (checkMap.mapIndex == 8)
                        {
                            myCamera.PlaySoundTrack(4);
                            myCamera.previousClip = myCamera.musicClips[4];
                            if (talkPanel)
                            {

                                talkPanel.gameObject.SetActive(true);
                                currentScene = database.GetSceneData("JaxEncounterGlyph");
                                currentObjectiveString = "Head to the Library";
                                currentState = State.SceneRunning;
                                talkPanel.scene = currentScene;
                                currentScene.index = 0;
                                SetScene(currentScene);
                            }
                        }
                        if (checkMap.mapIndex == 14)
                        {
                            myCamera.PlaySoundTrack(4);
                            myCamera.previousClip = myCamera.musicClips[10];
                            if (talkPanel)
                            {

                                talkPanel.gameObject.SetActive(true);
                                currentScene = database.GetSceneData("Scene4");
                                currentState = State.SceneRunning;
                                talkPanel.scene = currentScene;
                                currentScene.index = 0;
                                SetScene(currentScene);
                            }
                        }

                        if (Common.hasSeenJaxAndZeffCatchUp == false)
                        {
                            if (checkMap.mapIndex == 16 || checkMap.mapIndex == 0)
                            {
                                myCamera.PlaySoundTrack(2);
                                myCamera.previousClip = myCamera.musicClips[10];
                                if (talkPanel)
                                {

                                    talkPanel.gameObject.SetActive(true);
                                    currentScene = database.GetSceneData("JaxAndZeffronChapter1");
                                    currentObjectiveString = "Head to the Library";
                                    currentState = State.SceneRunning;
                                    talkPanel.scene = currentScene;
                                    currentScene.index = 0;
                                    SetScene(currentScene);
                                }
                                Common.hasSeenJaxAndZeffCatchUp = true;
                            }

                        }

                    }
                    break;
            }
        }

        TextObjectHandler.UpdateText(textHolder.subphaseTracker, currentObjectiveString);
        TextObjectHandler.UpdateText(textHolder.shadowSubphaseTracker, currentObjectiveString);

    }

    public void insertNewCharMapEvent(MapDetail checkMap)
    {
        switch (defaultSceneEntry)
        {
            case 27:
                {


                    if (checkMap.mapIndex == 17)
                    {
                        if (Common.summonedJax == false)
                        {

                            myCamera.PlaySoundTrack(3);
                            myCamera.previousClip = myCamera.musicClips[10];
                            GameObject jax = Instantiate(PlayerObject, Vector2.zero, Quaternion.identity);
                            jax.SetActive(true);
                            jax.transform.position = tileMap[3].transform.position + new Vector3(0.0f, 0.5f, 2.0f);
                            //zeffron.transform.position = tileMap[2].transform.position + new Vector3(0.0f, 0.5f, 2.0f);
                            ActorSetup asetup = jax.GetComponent<ActorSetup>();
                            asetup.characterId = 0;
                            LivingObject liveJax = jax.GetComponent<LivingObject>();
                            liveJax.FACTION = Faction.ally;
                            liveJax.Setup();
                            gridObjects.Add(liveJax);
                            turnOrder.Add(liveJax);
                            ComfirmMoveGridObject(liveJax, 37);
                            Common.summonedJax = true;

                        }


                    }





                }
                break;

            case 17:
                {


                    if (checkMap.mapIndex == 17)
                    {

                        if (Common.summonedZeffron == false)
                        {
                            myCamera.PlaySoundTrack(3);
                            myCamera.previousClip = myCamera.musicClips[10];
                            GameObject zeffron = Instantiate(PlayerObject, Vector2.zero, Quaternion.identity);
                            zeffron.SetActive(true);
                            zeffron.transform.position = tileMap[37].transform.position + new Vector3(0.0f, 0.5f, 2.0f);
                            //zeffron.transform.position = tileMap[2].transform.position + new Vector3(0.0f, 0.5f, 2.0f);
                            ActorSetup asetup = zeffron.GetComponent<ActorSetup>();
                            asetup.characterId = 1;
                            LivingObject liveZeff = zeffron.GetComponent<LivingObject>();
                            liveZeff.FACTION = Faction.ally;
                            liveZeff.Setup();
                            gridObjects.Add(liveZeff);
                            turnOrder.Add(liveZeff);
                            ComfirmMoveGridObject(liveZeff, 37);
                            Common.summonedZeffron = true;

                        }
                    }

                }
                break;

            case 26:
                {


                    if (checkMap.mapIndex == 17)
                    {

                        if (Common.summonedZeffron == false)
                        {
                            myCamera.PlaySoundTrack(3);
                            myCamera.previousClip = myCamera.musicClips[10];
                            GameObject zeffron = Instantiate(PlayerObject, Vector2.zero, Quaternion.identity);
                            zeffron.SetActive(true);
                            zeffron.transform.position = tileMap[37].transform.position + new Vector3(0.0f, 0.5f, 2.0f);
                            //zeffron.transform.position = tileMap[2].transform.position + new Vector3(0.0f, 0.5f, 2.0f);
                            ActorSetup asetup = zeffron.GetComponent<ActorSetup>();
                            asetup.characterId = 1;
                            LivingObject liveZeff = zeffron.GetComponent<LivingObject>();
                            liveZeff.FACTION = Faction.ally;
                            liveZeff.Setup();
                            gridObjects.Add(liveZeff);
                            turnOrder.Add(liveZeff);
                            ComfirmMoveGridObject(liveZeff, 37);
                            Common.summonedZeffron = true;

                        }
                    }
                }
                break;
        }
    }
    public void LoadDefaultScene()
    {

        LoadDScene(defaultSceneEntry);
        //myCamera.PlaySoundTrack1();

        if (PlayerObject)
        {
            if (defaultSceneEntry == 15)
            {
                GameObject jax = Instantiate(PlayerObject, Vector2.zero, Quaternion.identity);
                jax.SetActive(true);
                jax.transform.position = tileMap[1].transform.position + new Vector3(0.0f, 0.5f, 0.0f); //new Vector3(2.0f, 0.5f, 0.0f);
                ActorSetup asetup = jax.GetComponent<ActorSetup>();
                asetup.characterId = 0;
                LivingObject liveJax = jax.GetComponent<LivingObject>();
                liveJax.Setup();
                liveJax.TEXT.text = "";
                gridObjects.Add(liveJax);
                turnOrder.Add(liveJax);
                ComfirmMoveGridObject(liveJax, 1);


                //GameObject zeffron = Instantiate(PlayerObject, Vector2.zero, Quaternion.identity);
                //zeffron.SetActive(true);
                //zeffron.transform.position = new Vector3(4.0f, 0.5f, 0.0f);
                //ActorSetup zsetup = zeffron.GetComponent<ActorSetup>();
                //zsetup.characterId = 1;
                //LivingObject liveZeff = zeffron.GetComponent<LivingObject>();
                //liveZeff.Setup();
                //gridObjects.Add(liveZeff);
                //turnOrder.Add(liveZeff);
                //ComfirmMoveGridObject(liveZeff, 3);

            }
            else if (defaultSceneEntry == 27)
            {
                GameObject zeffron = Instantiate(PlayerObject, Vector2.zero, Quaternion.identity);
                zeffron.SetActive(true);
                zeffron.transform.position = new Vector3(8.0f, 0.5f, 2.0f);
                ActorSetup asetup = zeffron.GetComponent<ActorSetup>();
                asetup.characterId = 1;
                LivingObject liveZeff = zeffron.GetComponent<LivingObject>();
                liveZeff.Setup();
                liveZeff.TEXT.text = "";
                gridObjects.Add(liveZeff);
                turnOrder.Add(liveZeff);



            }
            else if (defaultSceneEntry == 26)
            {
                myCamera.PlaySoundTrack(12);
                GameObject jax = Instantiate(PlayerObject, Vector3.zero, Quaternion.identity);
                jax.SetActive(true);
                jax.transform.position = tileMap[6].transform.position + new Vector3(0.0f, 0.5f, 0.0f); //new Vector3(2.0f, 0.5f, 0.0f);
                ActorSetup asetup = jax.GetComponent<ActorSetup>();
                asetup.characterId = 0;
                LivingObject liveJax = jax.GetComponent<LivingObject>();
                liveJax.Setup();
                //liveJax.TEXT.text = "";
                gridObjects.Add(liveJax);
                turnOrder.Add(liveJax);
                ComfirmMoveGridObject(liveJax, 6);
                MoveCameraAndShow(liveJax);

                //GameObject zeffron = Instantiate(PlayerObject, Vector2.zero, Quaternion.identity);
                //zeffron.SetActive(true);
                //zeffron.transform.position = tileMap[5].transform.position + new Vector3(0.0f, 0.5f, 0.0f); //new Vector3(2.0f, 0.5f, 0.0f);
                //ActorSetup asetup2 = zeffron.GetComponent<ActorSetup>();
                //asetup2.characterId = 1;
                //LivingObject liveZeff = zeffron.GetComponent<LivingObject>();
                //liveZeff.Setup();
                //gridObjects.Add(liveZeff);
                //turnOrder.Add(liveZeff);
                //ComfirmMoveGridObject(liveZeff, 5);
            }
            else
            {


                if (PlayerPrefs.HasKey(Common.currentStory.ToString()))
                {
                    ClearObjects();


                    string loadString = PlayerPrefs.GetString(Common.currentStory.ToString());
                    string[] dataString = loadString.Split(',');

                    //Debug.Log(loadString);
                    int dataIIndex = 1;
                    int livingCount = 0;
                    System.Int32.TryParse(dataString[dataIIndex], out livingCount);
                    dataIIndex++;
                    for (int i = 0; i < livingCount; i++)
                    {
                        LivingObject possibleCharacter = Common.ConstructLivingFromStringArray(dataString, dataIIndex, ref dataIIndex);
                        possibleCharacter.ACTIONS = (int)(possibleCharacter.SPEED / 10.0f);
                        if (possibleCharacter.SPEED == 0)
                            possibleCharacter.ACTIONS = 1;
                        else
                            possibleCharacter.ACTIONS += 3;
                        if (!gridObjects.Contains(possibleCharacter))
                        {
                            gridObjects.Add(possibleCharacter);
                        }
                    }
                    SoftReset();
                }
                else
                {
                    GameObject jax = Instantiate(PlayerObject, Vector2.zero, Quaternion.identity);
                    jax.SetActive(true);
                    jax.transform.position = new Vector3(6.0f, 0.5f, 2.0f);
                    ActorSetup asetup = jax.GetComponent<ActorSetup>();
                    asetup.characterId = 0;
                    LivingObject liveJax = jax.GetComponent<LivingObject>();
                    liveJax.Setup();
                    gridObjects.Add(liveJax);
                    turnOrder.Add(liveJax);
                    myCamera.PlaySoundTrack(3);
                }
            }
        }
        prevState = State.FreeCamera;
        // currentState = State.PlayerTransition;
        if (turnOrder.Count > 0)
        {
            CreateEvent(this, turnOrder[0], "Phase Announce Event", PhaseAnnounce, null, -1, PhaseAnnounceStart);

        }
        CreateEvent(this, null, "return state event", BufferedStateChange);
        turnImgManger.LoadTurnImg(turnOrder);
        turnImgManger.UpdateSelection(-1);

    }

    public void LoadDScene(int amapIndex, int startindex = -1)
    {

        if (log)
        {
            log.Clear();
        }
        liveEnemies.Clear();
        menuManager.ShowNone();
        MapDetail map = database.GetMap(amapIndex);
        MapData data = database.GetMapData(map.mapName);
        bool visited = false;
        for (int i = 0; i < visitedMaps.Count; i++)
        {
            if (visitedMaps[i].mapIndex == amapIndex)
            {
                map = visitedMaps[i];
                data = Common.ConvertMapDetail2Data(map, data);
                visited = true;
                break;
            }
        }
        if (visited == false)
        {
            CheckForMapChangeEvent(map);
        }

        if (data.doorIndexes.Count > 0)
        {

            LoadDScene(data, startindex);
            map = Common.ConvertMapData2Detail(data, map);

        }
        else
        {
            data = Common.ConvertMapDetail2Data(map, data);
            LoadDScene(data, startindex);
        }

        currentMap = map;

        if (currentRoomName)
        {
            if (!currentRoomName.isSetup)
            {
                currentRoomName.Setup();
            }
            if (currentRoomName.textmeshpro)
            {
                currentRoomName.textmeshpro.text = currentMap.mapName;
                if (currentMap.mapIndex == 4)
                    currentRoomName.textmeshpro.text = "Storage Room";
            }
        }
        if (visited == false)
        {
            insertNewCharMapEvent(map);
            visitedMaps.Add(map);
        }
        SoftReset();

        for (int i = 0; i < objectivesAndNotes.Length; i++)
        {
            objectivesAndNotes[i].extraData = -1;
            objectivesAndNotes[i].gameObject.SetActive(false);
        }


        for (int i = 0; i < liveEnemies.Count; i++)
        {
            if (liveEnemies[i].FACTION == Faction.hazard)
            {
                HazardScript hazard = liveEnemies[i] as HazardScript;
                switch (hazard.HTYPE)
                {

                    case HazardType.lockDoor:
                        {
                            bool found = false;
                            for (int j = 0; j < objectivesAndNotes.Length; j++)
                            {
                                if (objectivesAndNotes[j].extraData == (int)hazard.HTYPE)
                                {
                                    found = true;
                                    break;
                                }

                            }
                            if (found == false)
                            {

                                for (int j = 0; j < objectivesAndNotes.Length; j++)
                                {
                                    if (objectivesAndNotes[j].extraData == -1)
                                    {
                                        objectivesAndNotes[j].extraData = (int)hazard.HTYPE;
                                        objectivesAndNotes[j].gameObject.SetActive(true);
                                        TextObjectHandler.UpdateText(objectivesAndNotes[j].subphaseTracker, "Lock Glyph prevents exiting");
                                        TextObjectHandler.UpdateText(objectivesAndNotes[j].shadowSubphaseTracker, "Lock Glyph prevents exiting");
                                        break;
                                    }

                                }
                            }
                        }
                        break;
                    case HazardType.redirect:
                        {
                            bool found = false;
                            for (int j = 0; j < objectivesAndNotes.Length; j++)
                            {
                                if (objectivesAndNotes[j].extraData == (int)hazard.HTYPE)
                                {
                                    found = true;
                                    break;
                                }

                            }
                            if (found == false)
                            {

                                for (int j = 0; j < objectivesAndNotes.Length; j++)
                                {
                                    if (objectivesAndNotes[j].extraData == -1)
                                    {
                                        objectivesAndNotes[j].extraData = (int)hazard.HTYPE;
                                        objectivesAndNotes[j].gameObject.SetActive(true);
                                        TextObjectHandler.UpdateText(objectivesAndNotes[j].subphaseTracker, "Redirect Glyph directs attacks");
                                        TextObjectHandler.UpdateText(objectivesAndNotes[j].shadowSubphaseTracker, "Redirect Glyph directs attacks");
                                        break;
                                    }

                                }
                            }
                        }
                        break;
                    case HazardType.controller:
                        {
                            bool found = false;
                            for (int j = 0; j < objectivesAndNotes.Length; j++)
                            {
                                if (objectivesAndNotes[j].extraData == (int)hazard.HTYPE)
                                {
                                    found = true;
                                    break;
                                }

                            }
                            if (found == false)
                            {

                                for (int j = 0; j < objectivesAndNotes.Length; j++)
                                {
                                    if (objectivesAndNotes[j].extraData == -1)
                                    {
                                        objectivesAndNotes[j].extraData = (int)hazard.HTYPE;
                                        objectivesAndNotes[j].gameObject.SetActive(true);
                                        TextObjectHandler.UpdateText(objectivesAndNotes[j].subphaseTracker, "Control Glyph removing tiles");
                                        TextObjectHandler.UpdateText(objectivesAndNotes[j].shadowSubphaseTracker, "Control Glyph removing tiles");
                                        break;
                                    }

                                }
                            }
                        }
                        break;
                    case HazardType.movement:
                        {
                            bool found = false;
                            for (int j = 0; j < objectivesAndNotes.Length; j++)
                            {
                                if (objectivesAndNotes[j].extraData == (int)hazard.HTYPE)
                                {
                                    found = true;
                                    break;
                                }

                            }
                            if (found == false)
                            {

                                for (int j = 0; j < objectivesAndNotes.Length; j++)
                                {
                                    if (objectivesAndNotes[j].extraData == -1)
                                    {
                                        objectivesAndNotes[j].extraData = (int)hazard.HTYPE;
                                        objectivesAndNotes[j].gameObject.SetActive(true);
                                        TextObjectHandler.UpdateText(objectivesAndNotes[j].subphaseTracker, "Move Glyph limits movement");
                                        TextObjectHandler.UpdateText(objectivesAndNotes[j].shadowSubphaseTracker, "Move Glyph limits movement");
                                        break;
                                    }

                                }
                            }
                        }
                        break;
                    case HazardType.zeroExp:
                        {
                            bool found = false;
                            for (int j = 0; j < objectivesAndNotes.Length; j++)
                            {
                                if (objectivesAndNotes[j].extraData == (int)hazard.HTYPE)
                                {
                                    found = true;
                                    break;
                                }

                            }
                            if (found == false)
                            {

                                for (int j = 0; j < objectivesAndNotes.Length; j++)
                                {
                                    if (objectivesAndNotes[j].extraData == -1)
                                    {
                                        objectivesAndNotes[j].extraData = (int)hazard.HTYPE;
                                        objectivesAndNotes[j].gameObject.SetActive(true);
                                        TextObjectHandler.UpdateText(objectivesAndNotes[j].subphaseTracker, "Exp Glyph prevents leveling");
                                        TextObjectHandler.UpdateText(objectivesAndNotes[j].shadowSubphaseTracker, "Exp Glyph prevents leveling");
                                        break;
                                    }

                                }
                            }
                        }
                        break;
                    case HazardType.protection:
                        {
                            bool found = false;
                            for (int j = 0; j < objectivesAndNotes.Length; j++)
                            {
                                if (objectivesAndNotes[j].extraData == (int)hazard.HTYPE)
                                {
                                    found = true;
                                    break;
                                }

                            }
                            if (found == false)
                            {

                                for (int j = 0; j < objectivesAndNotes.Length; j++)
                                {
                                    if (objectivesAndNotes[j].extraData == -1)
                                    {
                                        objectivesAndNotes[j].extraData = (int)hazard.HTYPE;
                                        objectivesAndNotes[j].gameObject.SetActive(true);
                                        TextObjectHandler.UpdateText(objectivesAndNotes[j].subphaseTracker, "Protect Glyph prevents hacking");
                                        TextObjectHandler.UpdateText(objectivesAndNotes[j].shadowSubphaseTracker, "Protect Glyph prevents hacking");
                                        break;
                                    }

                                }
                            }
                        }
                        break;
                    case HazardType.time:
                        break;
                }
            }
        }
    }

    public void LoadDScene(MapDetail map, int startindex = -1)
    {
        MapWidth = map.width;
        MapHeight = map.height;
        Clear();
        if (null == tileMap || tileMap.Count == 0)
        {
            tileMap = tileManager.getTiles(map.width * map.height); // * MapHeight);
        }
        else
        {
            return;
        }
        LivingObject[] livingobjs = GameObject.FindObjectsOfType<LivingObject>();
        int tileIndex = 0;
        float tileHeight = 0;
        float xoffset = 1 / (float)MapWidth;
        float yoffset = 1 / (float)MapHeight;
        for (int i = 0; i < map.width; i++)
        {

            for (int j = 0; j < map.height; j++)
            {
                int mapIndex = TwoToOneD(j, map.width, i);
                TileScript tile = tileMap[tileIndex];
                tile.listindex = mapIndex;
                tile.transform.position = new Vector3(i, j * tileHeight, j);
                tile.transform.parent = tileParent.transform;
                tile.name = "Tile " + mapIndex;
                tile.transform.rotation = Quaternion.Euler(90, 0, 0);
                tile.setTexture(map.texture);
                tile.TTYPE = TileType.regular;
                tile.setUVs((xoffset * (float)i), (xoffset * (float)(i + 1)), (yoffset * (float)j), (yoffset * (float)(j + 1)));
                tileIndex++;
                tile.canBeOccupied = true;
            }
            // yo += 0.05f;

        }
        tileMap.Sort();

        for (int i = 0; i < map.doorIndexes.Count; i++)
        {
            tileMap[map.doorIndexes[i]].MAT.mainTexture = doorTexture;
            tileMap[map.doorIndexes[i]].MAP = map.roomNames[i];
            tileMap[map.doorIndexes[i]].ROOM = map.roomIndexes[i];
            tileMap[map.doorIndexes[i]].START = map.startIndexes[i];
            tileMap[map.doorIndexes[i]].setUVs(0, 1, 0, 1);
            tileMap[map.doorIndexes[i]].TTYPE = TileType.door;
        }
        if (map.shopIndexes.Count > 0)
        {

            for (int i = 0; i < map.shopIndexes.Count; i++)
            {
                tileMap[map.shopIndexes[i]].MAT.mainTexture = shopTexture;
                tileMap[map.shopIndexes[i]].setUVs(0, 1, 0, 1);
                tileMap[map.shopIndexes[i]].TTYPE = TileType.shop;
            }

        }


        tileIndex = 1;
        int largestLevel = 1;
        turnOrder.Clear();
        for (int i = 0; i < livingobjs.Length; i++)
        {
            livingobjs[i].Refresh();
            if (livingobjs[i].GetComponent<EnemyScript>() || livingobjs[i].GetComponent<HazardScript>())
            {
                continue;
            }
            if (livingobjs[i].GetComponent<ActorScript>())
            {

                if (startindex == -1)
                {
                    livingobjs[i].transform.position = tileMap[tileIndex].transform.position + new Vector3(0, 0.5f, 0);
                }
                else if (startindex > 20)
                {
                    livingobjs[i].transform.position = tileMap[startindex - i].transform.position + new Vector3(0, 0.5f, 0);
                }
                else
                {
                    livingobjs[i].transform.position = tileMap[startindex + i].transform.position + new Vector3(0, 0.5f, 0);
                }
                tileIndex++;
                turnOrder.Add(livingobjs[i]);
                if (!livingobjs[i].isSetup)
                {
                    livingobjs[i].Setup();
                }
                if (largestLevel < livingobjs[i].LEVEL)
                {
                    largestLevel = livingobjs[i].LEVEL;
                }
                if (livingobjs[i].DEAD)
                {
                    livingobjs[i].STATS.HEALTH = 1;
                    livingobjs[i].DEAD = false;
                    LivingObject playable = livingobjs[i];
                    //  playable.updateLastSprites();
                    for (int j = 0; j < playable.INVENTORY.EFFECTS.Count; j++)
                    {
                        EffectScript anEffect = playable.INVENTORY.EFFECTS[j];
                        PoolManager.GetManager().ReleaseEffect(anEffect);
                    }
                    playable.INVENTORY.EFFECTS.Clear();
                    playable.Refresh();
                    playable.updateAilmentIcons();

                }
            }
        }
        int lvtimes = 0;
        List<EnemyScript> enemies = enemyManager.getEnemies(map.enemyIndexes.Count);
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyScript anEnemy = enemies[i];
            liveEnemies.Add(anEnemy);
            anEnemy.transform.position = tileMap[map.enemyIndexes[i]].transform.position + new Vector3(0, 0.5f, 0);
            anEnemy.gameObject.SetActive(true);
            if (largestLevel > 3)
            {
                if (largestLevel + lvtimes < Common.MaxLevel)
                    lvtimes = Random.Range(largestLevel - 2, largestLevel + lvtimes) - anEnemy.LEVEL;
                else
                    lvtimes = Common.MaxLevel;
            }
            anEnemy.BASE_STATS.LEVEL += Random.Range(0, largestLevel);
            int rand = Random.Range(0, 2);
            if (anEnemy.id != 8)
            {

                for (int j = 0; j < lvtimes; j++)
                {
                    if (j % 3 == 0)
                    {
                        // anEnemy.LevelUp();
                        rand = Random.Range(0, 3);
                        // Debug.Log("r is " + rand);
                        switch (rand)
                        {
                            case 0:
                                {
                                    anEnemy.GainPhysExp(20, false);
                                }
                                break;
                            case 1:
                                {
                                    anEnemy.GainMagExp(20, false);
                                }
                                break;
                            case 2:
                                {
                                    anEnemy.GainDexExp(20, false);
                                }
                                break;
                            default:
                                {
                                    Debug.Log("default lv up");
                                    anEnemy.GainDexExp(100);
                                }
                                break;
                        }
                        int attackCount = 0;
                        if (rand == 0)
                        {


                            for (int k = 0; k < anEnemy.INVENTORY.CSKILLS.Count; k++)
                            {
                                if (anEnemy.INVENTORY.CSKILLS[k].ETYPE != EType.magical)
                                {
                                    attackCount++;
                                }
                            }
                            if (attackCount < 6)
                            {
                                //    Debug.Log("command skill" + itemNum);
                                int itemNum = 0;
                                itemNum = Random.Range(0, 4);
                                itemNum += (Random.Range(1, 8) * 10);
                                UsableScript useable = database.LearnSkill(itemNum, anEnemy);
                            }

                        }
                        rand = Random.Range(0, 2);
                        attackCount = 0;
                        if (rand == 0)
                        {


                            for (int k = 0; k < anEnemy.INVENTORY.CSKILLS.Count; k++)
                            {
                                if (anEnemy.INVENTORY.CSKILLS[k].ETYPE != EType.physical)
                                {
                                    attackCount++;
                                }
                            }
                            if (attackCount < 6)
                            {
                                //    Debug.Log("command skill" + itemNum);
                                int itemNum = 0;
                                itemNum = Random.Range(4, 7);
                                itemNum += (Random.Range(1, 10) * 10);
                                UsableScript useable = database.LearnSkill(itemNum, anEnemy);
                            }

                        }
                        rand = Random.Range(0, 2);
                        attackCount = 0;
                        if (rand == 0)
                        {

                            if (anEnemy.INVENTORY.WEAPONS.Count < 6)
                            {

                                UsableScript useable = database.GetWeapon(Random.Range(0, 48), anEnemy);
                            }
                        }
                    }
                    else if (j % 2 == 0)
                    {
                        rand = Random.Range(0, 4);
                        switch (rand)
                        {
                            case 0:
                                {
                                    if (anEnemy.INVENTORY.CSKILLS.Count > 0)
                                    {
                                        rand = Random.Range(0, anEnemy.INVENTORY.CSKILLS.Count);
                                        anEnemy.INVENTORY.CSKILLS[rand].GrantXP(2, false);
                                    }
                                }
                                break;

                            case 1:
                                {
                                    if (anEnemy.INVENTORY.WEAPONS.Count > 0)
                                    {
                                        rand = Random.Range(0, anEnemy.INVENTORY.WEAPONS.Count);
                                        anEnemy.INVENTORY.WEAPONS[rand].GrantXP(2, false);
                                    }
                                }
                                break;

                            case 2:
                                {
                                    if (anEnemy.INVENTORY.USEABLES.Count > 0)
                                    {
                                        rand = Random.Range(0, anEnemy.INVENTORY.USEABLES.Count);
                                        UsableScript useable = anEnemy.INVENTORY.USEABLES[rand];
                                        if (useable.GetType() != typeof(ItemScript))
                                        {
                                            useable.GrantXP(2, false);
                                        }
                                    }
                                }
                                break;


                        }
                    }


                }

                if (lvtimes > 5)
                {
                    if (anEnemy.INVENTORY.ARMOR.Count > 0)
                    {
                        for (int j = 0; j < anEnemy.INVENTORY.ARMOR.Count; j++)
                        {
                            if (anEnemy.INVENTORY.ARMOR[j] != anEnemy.DEFAULT_ARMOR)
                            {
                                anEnemy.ARMOR.Equip(anEnemy.INVENTORY.ARMOR[j]);
                            }
                        }
                    }
                }
            }
            anEnemy.STATS.HEALTH = anEnemy.BASE_STATS.MAX_HEALTH;
            anEnemy.STATS.MANA = anEnemy.BASE_STATS.MAX_MANA;
            anEnemy.STATS.FATIGUE = 0;
            anEnemy.MapIndex = map.enemyIndexes[i];
            anEnemy.UpdateHealthbar();
        }

        List<GridObject> gridobjs = objManager.getObjects(map.objMapIndexes.Count);
        for (int i = 0; i < gridobjs.Count; i++)
        {
            gridobjs[i].transform.position = tileMap[map.objMapIndexes[i]].transform.position + new Vector3(0, 0.5f, 0);
            gridobjs[i].gameObject.SetActive(true);


            gridobjs[i].STATS.HEALTH = gridobjs[i].BASE_STATS.MAX_HEALTH;
            gridobjs[i].STATS.MANA = 0;
            gridobjs[i].STATS.FATIGUE = 0;

            //if (gridobjs[i].BASE_STATS.SPEED > 0)
            //{
            //    gridobjs[i].FACTION = Faction.eventObj;
            //}
            //else
            //{
            //    gridobjs[i].FACTION = Faction.ordinary;
            //}
            gridobjs[i].MapIndex = map.objMapIndexes[i];
        }

        List<HazardScript> hazards = hazardManager.getHazards(map.hazardIndexes.Count);
        for (int i = 0; i < hazards.Count; i++)
        {
            liveEnemies.Add(hazards[i]);
            hazards[i].transform.position = tileMap[map.hazardIndexes[i]].transform.position + new Vector3(0, 0.5f, 0);
            hazards[i].gameObject.SetActive(true);
            hazards[i].BASE_STATS.LEVEL = Random.Range(largestLevel, largestLevel + 2);
            for (int j = 0; j < hazards[i].BASE_STATS.LEVEL; j++)
            {
                if (hazards[i].INVENTORY.CSKILLS[0].SUBTYPE == SubSkillType.Magic)
                {
                    hazards[i].GainMagExp(80, false);
                }
                else
                {
                    hazards[i].GainPhysExp(80, false);
                }
            }
            hazards[i].MapIndex = map.hazardIndexes[i];
        }
        if (turnOrder.Count > 0)
            currentObject = turnOrder[0];

        //LivingObject[] livingObjects = GameObject.FindObjectsOfType<LivingObject>();
        //for (int i = livingObjects.Length - 1; i >= 0; i--)
        //{

        //    if (livingObjects[i].GetComponent<EnemyScript>() || livingObjects[i].GetComponent<HazardScript>())
        //    {
        //        continue;
        //    }

        //}


        tempGridObj.currentTile = GetTileAtIndex(GetTileIndex(Vector3.zero));
        attackableTiles = new List<List<TileScript>>();
        ShowWhite();
        GridObject[] objs = GameObject.FindObjectsOfType<GridObject>();
        gridObjects.Clear();
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].gameObject == tempObject)
            {
                continue;
            }

            if (!objs[i].gameObject.activeInHierarchy)
            {
                continue;
            }
            if (!gridObjects.Contains(objs[i]))
            {

                gridObjects.Add(objs[i]);
                if (objs[i].currentTile)
                {
                    objs[i].currentTile.isOccupied = false;
                }
                objs[i].currentTile = GetTile(objs[i]);
                if (objs[i].currentTile)
                {
                    objs[i].currentTile.isOccupied = true;

                }
                if (objs[i].currentTile.isInShadow)
                {
                    AttemptToGoIntoShadows(objs[i]);
                }
                else
                {
                    objs[i].RENDERER.color = Color.white;

                }
            }
        }
        //CleanMenuStack(true);
        //currentState = State.FreeCamera;
        // tempObject.transform.position = Vector3.zero;
        //tempGridObj.currentTile = tileMap[0];
        // myCamera.currentTile = tileMap[0];
        //ShowGridObjectAffectArea(tempGridObj, true);

        ShowSelectedTile(tempGridObj);

        myCamera.UpdateCamera();

        if (shopItems != null)
        {
            shopItems.Clear();
            for (int i = 0; i < 5; i++)
            {
                UsableScript newItem = null;

                int itemNum = 0;

                switch (i)
                {
                    case 0:
                        itemNum = Random.Range(0, 13);
                        newItem = database.GetWeapon(itemNum, null);
                        break;
                    case 1:
                        itemNum = Random.Range(0, 4);
                        itemNum += (Random.Range(1, 8) * 10);
                        //    Debug.Log("command skill" + itemNum);
                        newItem = database.GetSkill(itemNum);
                        break;
                    case 2:
                        itemNum = Random.Range(4, 7);
                        itemNum += (Random.Range(1, 10) * 10);
                        //    Debug.Log("command spell" + itemNum);
                        newItem = database.GetSkill(itemNum);
                        break;
                    case 3:
                        itemNum = Random.Range(110, 117);
                        //  Debug.Log("auto " + itemNum);
                        newItem = database.GetSkill(itemNum);
                        break;
                    case 4:
                        itemNum = Random.Range(204, 214);
                        //  Debug.Log("passive " + itemNum);
                        newItem = database.GetSkill(itemNum);
                        break;

                    default:
                        break;
                }
                shopItems.Add(newItem);
            }
        }



    }

    public void LoadDScene(MapData data, int startindex = -1)
    {

        //       Debug.Log("data");

        MapWidth = data.width;
        MapHeight = data.height;
        Clear();

        if (null == tileMap || tileMap.Count == 0)
        {
            tileMap = tileManager.getTiles(data.width * data.height); // * MapHeight);
        }
        else
        {
            return;
        }
        LivingObject[] livingobjs = GameObject.FindObjectsOfType<LivingObject>();
        int tileIndex = 0;
        float tileHeight = 0;
        float xoffset = 1 / (float)MapWidth;
        float yoffset = 1 / (float)MapHeight;
        float yElevation = 0;
        float xElevation = 0;

        for (int i = 0; i < data.width; i++)
        {

            for (int j = 0; j < data.height; j++)
            {
                int mapIndex = TwoToOneD(j, data.width, i);
                TileScript tile = tileMap[tileIndex];
                tile.listindex = mapIndex;
                if (j > data.yMinRestriction && j < data.yMaxRestriction && i > data.xMinRestriction && i < data.xMaxRestriction)
                    tile.transform.position = new Vector3(i * 2, (j * 2 * tileHeight) + yElevation, j * 2);
                else
                    tile.transform.position = new Vector3(i * 2, (j * 2 * tileHeight), j * 2);
                tile.transform.parent = tileParent.transform;
                tile.name = "Tile " + mapIndex;
                tile.transform.rotation = Quaternion.Euler(90, 0, 0);
                tile.setTexture(data.texture);
                tile.EXTRA = "";
                tile.isInShadow = false;
                tile.TTYPE = TileType.regular;
                tile.setUVs((xoffset * (float)i), (xoffset * (float)(i + 1)), (yoffset * (float)j), (yoffset * (float)(j + 1)));
                tileIndex++;
                tile.canBeOccupied = true;
                if (mapIndex == data.xElevation)
                {
                    data.yElevation = -1 * data.yElevation;
                }
            }
            yElevation += data.yElevation;

        }
        tileMap.Sort();
        data.yElevation = -1 * data.yElevation;
        for (int i = 0; i < data.doorIndexes.Count; i++)
        {
            tileMap[data.doorIndexes[i]].MAT.mainTexture = doorTexture;
            tileMap[data.doorIndexes[i]].MAP = data.roomNames[i];
            tileMap[data.doorIndexes[i]].ROOM = data.roomIndexes[i];
            tileMap[data.doorIndexes[i]].START = data.startIndexes[i];
            tileMap[data.doorIndexes[i]].setUVs(0, 1, 0, 1);
            tileMap[data.doorIndexes[i]].TTYPE = TileType.door;
        }
        if (data.shopIndexes.Count > 0)
        {

            for (int i = 0; i < data.shopIndexes.Count; i++)
            {
                tileMap[data.shopIndexes[i]].MAT.mainTexture = shopTexture;
                tileMap[data.shopIndexes[i]].setUVs(0, 1, 0, 1);
                tileMap[data.shopIndexes[i]].TTYPE = TileType.shop;
            }

        }
        int extraIndex = 0;
        if (data.tilesInShadow.Count > 0)
        {
            for (int i = 0; i < data.tilesInShadow.Count; i++)
            {
                tileMap[data.tilesInShadow[i]].isInShadow = true;
            }
        }
        if (data.specialTileIndexes.Count > 0)
        {

            for (int i = 0; i < data.specialTileIndexes.Count; i++)
            {

                TileType newtype = data.specialiles[i];
                int specialIndex = data.specialTileIndexes[i];
                tileMap[specialIndex].MAT.mainTexture = Common.GetSpecialTexture(newtype);
                tileMap[specialIndex].setUVs(0, 1, 0, 1);
                tileMap[specialIndex].TTYPE = newtype;
                if (newtype == TileType.help)
                {
                    if (extraIndex < data.specialExtra.Count)
                    {
                        int helpIndex = data.specialExtra[extraIndex];
                        tileMap[data.specialTileIndexes[i]].EXTRA = helpIndex + ";" + Common.GetHelpText(helpIndex);
                        extraIndex++;
                    }
                }
                else
                {
                    switch (newtype)
                    {
                        case TileType.knockback:
                            tileMap[data.specialTileIndexes[i]].EXTRA = "18 ;" + Common.GetHelpText(18);
                            break;
                        case TileType.pullin:
                            tileMap[data.specialTileIndexes[i]].EXTRA = "19 ;" + Common.GetHelpText(19);
                            break;
                        case TileType.swap:
                            tileMap[data.specialTileIndexes[i]].EXTRA = "14 ;" + Common.GetHelpText(14);
                            break;
                        case TileType.reposition:
                            tileMap[data.specialTileIndexes[i]].EXTRA = "20 ;" + Common.GetHelpText(20);
                            break;

                    }
                }
            }
        }

        if (data.unOccupiedIndexes != null)
        {

            for (int i = 0; i < data.unOccupiedIndexes.Count; i++)
            {
                int tindex = data.unOccupiedIndexes[i];
                tileMap[tindex].canBeOccupied = false;
                tileMap[tindex].gameObject.SetActive(false);
            }
        }
        int largestLevel = 1;
        //Determinging the player starting positions
        tileIndex = 1;
        turnOrder.Clear();
        List<int> playerIndexes = new List<int>();
        List<TileScript> possibleTiles = null;
        TileScript aTile = null;
        if (startindex != -1)
        {
            aTile = tileMap[startindex];
        }
        else
        {
            aTile = tileMap[tileIndex];
        }

        possibleTiles = tileManager.GetAdjecentTiles(aTile);
        possibleTiles.Insert(0, aTile);
        bool foundtile = false;
        for (int i = 0; i < livingobjs.Length; i++)
        {
            livingobjs[i].Refresh();
            if (livingobjs[i].GetComponent<ActorScript>())
            {
                foundtile = false;
                for (int j = 0; j < possibleTiles.Count; j++)
                {
                    tileIndex = possibleTiles[j].listindex;
                    if (!data.enemyIndexes.Contains(tileIndex))
                    {
                        if (!data.objMapIndexes.Contains(tileIndex))
                        {
                            if (!data.glyphIndexes.Contains(tileIndex))
                            {
                                if (!playerIndexes.Contains(tileIndex))
                                {
                                    foundtile = true;
                                    livingobjs[i].transform.position = tileMap[tileIndex].transform.position + new Vector3(0, 0.5f, 0);
                                    livingobjs[i].currentTileIndex = tileIndex;
                                    playerIndexes.Add(tileIndex);
                                    break;
                                }
                            }
                        }
                    }
                }
                if (foundtile == false)
                {

                    Debug.Log("Found is false?? ");
                    if (startindex == -1)
                    {
                        livingobjs[i].transform.position = tileMap[tileIndex].transform.position + new Vector3(0, 0.5f, 0);
                    }
                    else if (startindex % data.width > (0.5f * data.width))
                    {
                        livingobjs[i].transform.position = tileMap[startindex - i].transform.position + new Vector3(0, 0.5f, 0);
                    }
                    else
                    {
                        livingobjs[i].transform.position = tileMap[startindex + i].transform.position + new Vector3(0, 0.5f, 0);
                    }
                    tileIndex = 1 + i;
                }

                turnOrder.Add(livingobjs[i]);
                if (!livingobjs[i].isSetup)
                {
                    livingobjs[i].Setup();
                }
                if (largestLevel < livingobjs[i].LEVEL)
                {
                    largestLevel = livingobjs[i].LEVEL;
                }
                if (livingobjs[i].DEAD)
                {
                    livingobjs[i].STATS.HEALTH = (-1 * livingobjs[i].MAX_HEALTH) + 1;
                    livingobjs[i].DEAD = false;
                }
            }
        }

        turnOrder.Clear();
        for (int i = 0; i < livingobjs.Length; i++)
        {
            if (livingobjs[i].GetComponent<EnemyScript>() || livingobjs[i].GetComponent<HazardScript>())
            {
                continue;
            }
            if (livingobjs[i].GetComponent<ActorScript>())
            {

                //if (startindex == -1)
                //{
                //    livingobjs[i].transform.position = tileMap[tileIndex].transform.position + new Vector3(0, 0.5f, 0);
                //}
                //else if (startindex > 20)
                //{
                //    livingobjs[i].transform.position = tileMap[startindex - i].transform.position + new Vector3(0, 0.5f, 0);
                //}
                //else
                //{
                //    livingobjs[i].transform.position = tileMap[startindex + i].transform.position + new Vector3(0, 0.5f, 0);
                //}
                tileIndex++;
                turnOrder.Add(livingobjs[i]);
                if (!livingobjs[i].isSetup)
                {
                    livingobjs[i].Setup();
                }
                if (largestLevel < livingobjs[i].LEVEL)
                {
                    largestLevel = livingobjs[i].LEVEL;
                }
                if (livingobjs[i].DEAD)
                {
                    livingobjs[i].STATS.HEALTH = (-1 * livingobjs[i].MAX_HEALTH) + 1;
                    livingobjs[i].DEAD = false;
                }
            }
        }
        int lvtimes = 0;
        List<EnemyScript> enemies = enemyManager.getEnemies(data);
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyScript anEnemy = enemies[i];
            liveEnemies.Add(anEnemy);



            anEnemy.transform.position = tileMap[data.enemyIndexes[i]].transform.position + new Vector3(0, 0.5f, 0);
            anEnemy.gameObject.SetActive(true);

            anEnemy.currentTileIndex = data.enemyIndexes[i];


            if (largestLevel > 1)
            {
                if (largestLevel + lvtimes < Common.MaxLevel)
                    lvtimes = Random.Range(largestLevel - 2, largestLevel + lvtimes);
                else
                    lvtimes = Common.MaxLevel;
            }
            anEnemy.BASE_STATS.LEVEL += Random.Range(0, largestLevel);
            int rand = Random.Range(0, 2);
            if (anEnemy.id != 8)
            {

                for (int j = 0; j < lvtimes; j++)
                {
                    // anEnemy.LevelUp();
                    rand = Random.Range(0, 3);
                    // Debug.Log("r=" + rand);
                    switch (rand)
                    {
                        case 0:
                            {
                                anEnemy.GainPhysExp(30, false);
                            }
                            break;
                        case 1:
                            {
                                anEnemy.GainMagExp(30, false);
                            }
                            break;
                        case 2:
                            {
                                anEnemy.GainDexExp(30, false);
                            }
                            break;
                    }
                    if (j % 3 == 0)
                    {
                        int attackCount = 0;
                        if (rand == 0)
                        {


                            for (int k = 0; k < anEnemy.INVENTORY.CSKILLS.Count; k++)
                            {
                                if (anEnemy.INVENTORY.CSKILLS[k].ETYPE != EType.magical)
                                {
                                    attackCount++;
                                }
                            }
                            if (attackCount < 6)
                            {
                                //    Debug.Log("command skill" + itemNum);
                                int itemNum = 0;
                                itemNum = Random.Range(0, 4);
                                itemNum += (Random.Range(1, 8) * 10);
                                UsableScript useable = database.LearnSkill(itemNum, anEnemy);
                            }

                        }
                        rand = Random.Range(0, 2);
                        attackCount = 0;
                        if (rand == 0)
                        {


                            for (int k = 0; k < anEnemy.INVENTORY.CSKILLS.Count; k++)
                            {
                                if (anEnemy.INVENTORY.CSKILLS[k].ETYPE != EType.physical)
                                {
                                    attackCount++;
                                }
                            }
                            if (attackCount < 6)
                            {
                                //    Debug.Log("command skill" + itemNum);
                                int itemNum = 0;
                                itemNum = Random.Range(4, 7);
                                itemNum += (Random.Range(1, 10) * 10);
                                UsableScript useable = database.LearnSkill(itemNum, anEnemy);
                            }

                        }
                        rand = Random.Range(0, 2);
                        attackCount = 0;
                        if (rand == 0)
                        {

                            if (anEnemy.INVENTORY.WEAPONS.Count < 6)
                            {

                                UsableScript useable = database.GetWeapon(Random.Range(0, 48), anEnemy);
                            }
                        }
                    }
                    if (j % 2 == 0)
                    {
                        rand = Random.Range(0, 4);
                        switch (rand)
                        {
                            case 0:
                                {
                                    if (anEnemy.INVENTORY.CSKILLS.Count > 0)
                                    {
                                        rand = Random.Range(0, anEnemy.INVENTORY.CSKILLS.Count);
                                        anEnemy.INVENTORY.CSKILLS[rand].GrantXP(lvtimes, false);
                                    }
                                }
                                break;

                            case 1:
                                {
                                    if (anEnemy.INVENTORY.WEAPONS.Count > 0)
                                    {
                                        rand = Random.Range(0, anEnemy.INVENTORY.WEAPONS.Count);
                                        anEnemy.INVENTORY.WEAPONS[rand].GrantXP(lvtimes, false);
                                    }
                                }
                                break;

                            case 2:
                                {
                                    if (anEnemy.INVENTORY.USEABLES.Count > 0)
                                    {
                                        rand = Random.Range(0, anEnemy.INVENTORY.USEABLES.Count);
                                        UsableScript useable = anEnemy.INVENTORY.USEABLES[rand];
                                        if (useable.GetType() != typeof(ItemScript))
                                        {
                                            useable.GrantXP(lvtimes, false);
                                        }
                                    }
                                }
                                break;


                        }
                    }
                    switch (Common.GetEPCluster(anEnemy.personality))
                    {
                        case EPCluster.physical:
                            {
                                if (anEnemy.INVENTORY.CSKILLS.Count > 0)
                                {
                                    for (int k = 0; k < anEnemy.INVENTORY.CSKILLS.Count; k++)
                                    {
                                        if (anEnemy.INVENTORY.CSKILLS[k].ETYPE == EType.physical)
                                        {
                                            rand = Random.Range(0, 2);
                                            if (rand == 0)
                                            {
                                                anEnemy.INVENTORY.CSKILLS[k].GrantXP(1, false);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case EPCluster.magical:
                            {
                                if (anEnemy.INVENTORY.CSKILLS.Count > 0)
                                {
                                    for (int k = 0; k < anEnemy.INVENTORY.CSKILLS.Count; k++)
                                    {
                                        if (anEnemy.INVENTORY.CSKILLS[k].ETYPE == EType.magical)
                                        {

                                            rand = Random.Range(0, 2);
                                            if (rand == 0)
                                            {
                                                anEnemy.INVENTORY.CSKILLS[k].GrantXP(1, false);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case EPCluster.logical:
                            {
                                if (anEnemy.INVENTORY.WEAPONS.Count > 0)
                                {
                                    for (int k = 0; k < anEnemy.INVENTORY.WEAPONS.Count; k++)
                                    {


                                        rand = Random.Range(0, 2);
                                        if (rand == 0)
                                        {
                                            anEnemy.INVENTORY.WEAPONS[k].GrantXP(1, false);
                                        }
                                    }

                                }
                            }
                            break;
                        case EPCluster.natural:
                            {
                                if (anEnemy.INVENTORY.ARMOR.Count > 0)
                                {
                                    for (int k = 0; k < anEnemy.INVENTORY.ARMOR.Count; k++)
                                    {


                                        rand = Random.Range(0, 2);
                                        if (rand == 0)
                                        {
                                            anEnemy.INVENTORY.ARMOR[k].LevelUP();
                                        }
                                    }

                                }
                            }
                            break;
                    }

                }

                if (lvtimes > 5)
                {
                    if (anEnemy.INVENTORY.ARMOR.Count > 0)
                    {
                        for (int j = 0; j < anEnemy.INVENTORY.ARMOR.Count; j++)
                        {
                            if (anEnemy.INVENTORY.ARMOR[j] != anEnemy.DEFAULT_ARMOR)
                            {
                                anEnemy.ARMOR.Equip(anEnemy.INVENTORY.ARMOR[j]);
                            }
                        }
                    }
                }
            }
            anEnemy.STATS.HEALTH = anEnemy.BASE_STATS.MAX_HEALTH;
            anEnemy.STATS.MANA = anEnemy.BASE_STATS.MAX_MANA;
            anEnemy.STATS.FATIGUE = 0;
            anEnemy.MapIndex = data.enemyIndexes[i];
            anEnemy.UpdateHealthbar();
        }

        List<GridObject> gridobjs = objManager.getObjects(data);

        for (int i = 0; i < gridobjs.Count; i++)
        {

            gridobjs[i].transform.position = tileMap[data.objMapIndexes[i]].transform.position + new Vector3(0, 0.5f, 0);
            gridobjs[i].gameObject.SetActive(true);
            gridobjs[i].currentTileIndex = data.objMapIndexes[i];

            gridobjs[i].STATS.HEALTH = gridobjs[i].BASE_STATS.MAX_HEALTH;
            gridobjs[i].STATS.MANA = 0;
            gridobjs[i].STATS.FATIGUE = 0;

            //if (gridobjs[i].BASE_STATS.SPEED > 0)
            //{
            //    gridobjs[i].FACTION = Faction.eventObj;
            //}
            //else
            //{
            //    gridobjs[i].FACTION = Faction.ordinary;
            //}
            gridobjs[i].MapIndex = data.objMapIndexes[i];
        }

        int revealIndex = 0;
        List<HazardScript> hazards = hazardManager.getHazards(data);
        for (int i = 0; i < hazards.Count; i++)
        {
            liveEnemies.Add(hazards[i]);
            hazards[i].transform.position = tileMap[data.glyphIndexes[i]].transform.position + new Vector3(0, 0.5f, 0);
            hazards[i].gameObject.SetActive(true);
            hazards[i].BASE_STATS.LEVEL = Random.Range(largestLevel, largestLevel + 2);
            hazards[i].MapIndex = data.glyphIndexes[i];

            hazards[i].currentTileIndex = data.glyphIndexes[i];

            if (data.revealCount > 0)
            {
                if (revealIndex < data.unOccupiedIndexes.Count)
                {
                    for (int j = revealIndex; j < data.unOccupiedIndexes.Count; j++)
                    {
                        hazards[i].revealTiles.Add(data.unOccupiedIndexes[j]);

                    }
                    revealIndex += data.revealCount;
                }
            }
        }
        if (turnOrder.Count > 0)
            currentObject = turnOrder[0];



        // GetTileAtIndex(GetTileIndex(Vector3.zero));

        attackableTiles = new List<List<TileScript>>();
        ShowWhite();
        GridObject[] objs = GameObject.FindObjectsOfType<GridObject>();
        gridObjects.Clear();
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].gameObject == tempObject)
            {
                continue;
            }

            if (!objs[i].gameObject.activeInHierarchy)
            {
                continue;
            }
            if (!gridObjects.Contains(objs[i]))
            {

                gridObjects.Add(objs[i]);
                objs[i].currentTile = GetTile(objs[i]);
                if (objs[i].currentTile)
                {
                    if (objs[i].currentTile.isOccupied)
                    {
                        if (GetObjectAtTile(objs[i].currentTile) != objs[i])
                        {
                            while (objs[i].currentTile.isOccupied || objs[i].currentTile.canBeOccupied == false)
                            {
                                objs[i].currentTile = tileMap[++objs[i].MapIndex];
                                objs[i].transform.position = objs[i].currentTile.transform.position + new Vector3(0, 0.5f, 0);
                            }
                        }
                        else
                        {
                            objs[i].currentTile.isOccupied = false;
                        }
                    }
                    else
                    {
                        objs[i].currentTile.isOccupied = false;
                    }

                }
                objs[i].currentTile = GetTile(objs[i]);
                if (objs[i].currentTile)
                {
                    if (objs[i].currentTile.canBeOccupied)
                    {
                        objs[i].currentTile.isOccupied = true;

                    }
                    else
                    {
                        while (objs[i].currentTile.isOccupied || objs[i].currentTile.canBeOccupied == false)
                        {
                            objs[i].currentTile = tileMap[++objs[i].MapIndex];
                            objs[i].transform.position = objs[i].currentTile.transform.position + new Vector3(0, 0.5f, 0);
                        }
                    }

                }

                objs[i].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        if (gridobjs.Count > 0)
        {

            tempGridObj.currentTile = gridobjs[0].currentTile;
            ShowSelectedTile(tempGridObj);
        }

        myCamera.UpdateCamera();

        if (shopItems != null)
        {
            shopItems.Clear();
            for (int i = 0; i < 5; i++)
            {
                UsableScript newItem = null;

                int itemNum = 0;

                switch (i)
                {
                    case 0:
                        itemNum = Random.Range(0, 13);
                        newItem = database.GetWeapon(itemNum, null);
                        break;
                    case 1:
                        itemNum = Random.Range(0, 4);
                        itemNum += (Random.Range(1, 8) * 10);
                        //    Debug.Log("command skill" + itemNum);
                        newItem = database.GetSkill(itemNum);
                        break;
                    case 2:
                        itemNum = Random.Range(4, 7);
                        itemNum += (Random.Range(1, 10) * 10);
                        //    Debug.Log("command spell" + itemNum);
                        newItem = database.GetSkill(itemNum);
                        break;
                    case 3:
                        itemNum = Random.Range(110, 120);
                        //  Debug.Log("auto " + itemNum);
                        newItem = database.GetSkill(itemNum);
                        break;
                    case 4:
                        itemNum = Random.Range(204, 214);
                        //  Debug.Log("passive " + itemNum);
                        newItem = database.GetSkill(itemNum);
                        break;

                    default:
                        break;
                }
                shopItems.Add(newItem);
            }
        }


    }

    public void SaveGame(string saveSlot = "")
    {
        if (saveSlot == "")
        {
            saveSlot = Common.currentStory.ToString();
        }
        saveSlot = saveSlot + "_c";
        Debug.Log(saveSlot);
        string saveString = "";
        //int tutCount = currentTutorial.steps.Count;
        //saveString += "" + tutCount + ",";
        //saveString += "" + currentTutorial.currentStep;
        //for (int i = 0; i < tutCount; i++)
        //{
        //    saveString += "," + currentTutorial.steps[i] + "," + currentTutorial.clarifications[i];
        //}
        saveString += myCamera.soundTrack + ",";
        saveString += Common.summonedJax + ",";
        saveString += Common.summonedZeffron + ",";
        saveString += Common.enemiesCompletedPhase + ",";
        saveString += Common.haveBeenCrippled + ",";
        saveString += Common.hasAllocated + ",";
        saveString += Common.hasGainedSkill + ",";
        saveString += Common.hasLearnedFromEnemy + ",";
        saveString += Common.GetMapDetailAsString(currentMap) + ",";
        saveString += visitedMaps.Count;
        for (int i = 0; i < visitedMaps.Count; i++)
        {
            saveString += "," + Common.GetMapDetailAsString(visitedMaps[i]);
        }
        LivingObject[] liveObjects = GameObject.FindObjectsOfType<LivingObject>();
        saveString += "," + liveObjects.Length;
        for (int i = 0; i < liveObjects.Length; i++)
        {
            if (liveObjects[i].DEAD == false)
            {
                saveString += "," + database.GenerateSaveString(liveObjects[i]);
            }
        }


        //Debug.Log(saveString);
        PlayerPrefs.SetString(saveSlot, saveString);
        CheckTutorialPrompt(30);
    }

    public void SaveChapterData(string saveSlot)
    {
        string saveString = "";
        LivingObject[] liveObjects = GameObject.FindObjectsOfType<LivingObject>();
        saveString += "," + liveObjects.Length;
        for (int i = 0; i < liveObjects.Length; i++)
        {
            if (liveObjects[i].DEAD == false)
            {
                saveString += "," + database.GenerateSaveString(liveObjects[i]);
            }
        }


        //Debug.Log(saveString);
        PlayerPrefs.SetString(saveSlot, saveString);
    }

    public void LoadGameAndScene()
    {
        MapDetail detail;

        if (Common.currentStory == StorySection.none)
            Common.currentStory = lastStory;

        if (PlayerPrefs.HasKey(Common.currentStory + "_c"))
        {
            Clear();
            string loadString = PlayerPrefs.GetString(Common.currentStory + "_c");

            string[] dataString = loadString.Split(',');
            int dataIIndex = 0;
            bool aCommon = false;

            int soundTrack = 0;

            System.Int32.TryParse(dataString[dataIIndex], out soundTrack);
            myCamera.PlaySoundTrack(soundTrack);
            dataIIndex++;

            System.Boolean.TryParse(dataString[dataIIndex], out aCommon);
            Common.summonedJax = aCommon;
            dataIIndex++;

            System.Boolean.TryParse(dataString[dataIIndex], out aCommon);
            Common.summonedZeffron = aCommon;
            dataIIndex++;

            System.Boolean.TryParse(dataString[dataIIndex], out aCommon);
            Common.enemiesCompletedPhase = aCommon;
            dataIIndex++;

            System.Boolean.TryParse(dataString[dataIIndex], out aCommon);
            Common.haveBeenCrippled = aCommon;
            dataIIndex++;

            System.Boolean.TryParse(dataString[dataIIndex], out aCommon);
            Common.hasAllocated = aCommon;
            dataIIndex++;

            System.Boolean.TryParse(dataString[dataIIndex], out aCommon);
            Common.hasGainedSkill = aCommon;
            dataIIndex++;

            System.Boolean.TryParse(dataString[dataIIndex], out aCommon);
            Common.hasLearnedFromEnemy = aCommon;
            dataIIndex++;

            detail = Common.ConstructMapFromStringArray(dataString, dataIIndex, ref dataIIndex);
            currentMap = detail;
            //Assaign data to current map

            // MapWidth = data.width;
            // MapHeight = data.height;


            int visitedCount = 0;
            System.Int32.TryParse(dataString[dataIIndex], out visitedCount);
            //  Debug.Log("visited=" + visitedCount);
            dataIIndex++;
            visitedMaps.Clear();


            MapData data = database.GetMapData(detail.mapName);
            data = Common.ConvertMapDetail2Data(detail, data);
            MapWidth = data.width;
            MapHeight = data.height;
            Clear();

            if (null == tileMap || tileMap.Count == 0)
            {
                tileMap = tileManager.getTiles(data.width * data.height); // * MapHeight);
            }
            else
            {
                return;
            }
            LivingObject[] livingobjs = GameObject.FindObjectsOfType<LivingObject>();
            int tileIndex = 0;
            float tileHeight = 0;
            float xoffset = 1 / (float)MapWidth;
            float yoffset = 1 / (float)MapHeight;
            float yElevation = 0;
            float xElevation = 0;

            for (int i = 0; i < data.width; i++)
            {

                for (int j = 0; j < data.height; j++)
                {
                    int mapIndex = TwoToOneD(j, data.width, i);
                    TileScript tile = tileMap[tileIndex];
                    tile.listindex = mapIndex;
                    if (j > data.yMinRestriction && j < data.yMaxRestriction && i > data.xMinRestriction && i < data.xMaxRestriction)
                        tile.transform.position = new Vector3(i * 2, (j * 2 * tileHeight) + yElevation, j * 2);
                    else
                        tile.transform.position = new Vector3(i * 2, (j * 2 * tileHeight), j * 2);
                    tile.transform.parent = tileParent.transform;
                    tile.name = "Tile " + mapIndex;
                    tile.transform.rotation = Quaternion.Euler(90, 0, 0);
                    tile.setTexture(data.texture);
                    tile.EXTRA = "";
                    tile.isInShadow = false;
                    tile.TTYPE = TileType.regular;
                    tile.setUVs((xoffset * (float)i), (xoffset * (float)(i + 1)), (yoffset * (float)j), (yoffset * (float)(j + 1)));
                    tileIndex++;
                    tile.canBeOccupied = true;
                    if (mapIndex == data.xElevation)
                    {
                        data.yElevation = -1 * data.yElevation;
                    }
                }
                yElevation += data.yElevation;

            }
            tileMap.Sort();
            data.yElevation = -1 * data.yElevation;
            for (int i = 0; i < data.doorIndexes.Count; i++)
            {
                tileMap[data.doorIndexes[i]].MAT.mainTexture = doorTexture;
                tileMap[data.doorIndexes[i]].MAP = data.roomNames[i];
                tileMap[data.doorIndexes[i]].ROOM = data.roomIndexes[i];
                tileMap[data.doorIndexes[i]].START = data.startIndexes[i];
                tileMap[data.doorIndexes[i]].setUVs(0, 1, 0, 1);
                tileMap[data.doorIndexes[i]].TTYPE = TileType.door;
            }
            if (data.shopIndexes.Count > 0)
            {

                for (int i = 0; i < data.shopIndexes.Count; i++)
                {
                    tileMap[data.shopIndexes[i]].MAT.mainTexture = shopTexture;
                    tileMap[data.shopIndexes[i]].setUVs(0, 1, 0, 1);
                    tileMap[data.shopIndexes[i]].TTYPE = TileType.shop;
                }

            }
            int extraIndex = 0;
            if (data.tilesInShadow.Count > 0)
            {
                for (int i = 0; i < data.tilesInShadow.Count; i++)
                {
                    tileMap[data.tilesInShadow[i]].isInShadow = true;
                }
            }
            if (data.objMapIndexes.Count > 0)
            {

            }
            if (data.specialTileIndexes.Count > 0)
            {

                for (int i = 0; i < data.specialTileIndexes.Count; i++)
                {

                    TileType newtype = data.specialiles[i];
                    int specialIndex = data.specialTileIndexes[i];
                    tileMap[specialIndex].MAT.mainTexture = Common.GetSpecialTexture(newtype);
                    tileMap[specialIndex].setUVs(0, 1, 0, 1);
                    tileMap[specialIndex].TTYPE = newtype;
                    if (newtype == TileType.help)
                    {
                        if (extraIndex < data.specialExtra.Count)
                        {
                            int helpIndex = data.specialExtra[extraIndex];
                            tileMap[data.specialTileIndexes[i]].EXTRA = helpIndex + ";" + Common.GetHelpText(helpIndex);
                            extraIndex++;
                        }
                    }
                    else
                    {
                        switch (newtype)
                        {
                            case TileType.knockback:
                                tileMap[data.specialTileIndexes[i]].EXTRA = "18 ;" + Common.GetHelpText(18);
                                break;
                            case TileType.pullin:
                                tileMap[data.specialTileIndexes[i]].EXTRA = "19 ;" + Common.GetHelpText(19);
                                break;
                            case TileType.swap:
                                tileMap[data.specialTileIndexes[i]].EXTRA = "14 ;" + Common.GetHelpText(14);
                                break;
                            case TileType.reposition:
                                tileMap[data.specialTileIndexes[i]].EXTRA = "20 ;" + Common.GetHelpText(20);
                                break;

                        }
                    }
                }
            }

            if (data.unOccupiedIndexes != null)
            {

                for (int i = 0; i < data.unOccupiedIndexes.Count; i++)
                {
                    int tindex = data.unOccupiedIndexes[i];
                    tileMap[tindex].canBeOccupied = false;
                    tileMap[tindex].gameObject.SetActive(false);
                }
            }

            turnOrder.Clear();



            List<GridObject> gridobjs = objManager.getObjects(data);

            for (int i = 0; i < gridobjs.Count; i++)
            {

                gridobjs[i].transform.position = tileMap[data.objMapIndexes[i]].transform.position + new Vector3(0, 0.5f, 0);
                gridobjs[i].gameObject.SetActive(true);
                gridobjs[i].currentTileIndex = data.objMapIndexes[i];

                gridobjs[i].STATS.HEALTH = gridobjs[i].BASE_STATS.MAX_HEALTH;
                gridobjs[i].STATS.MANA = 0;
                gridobjs[i].STATS.FATIGUE = 0;


                gridobjs[i].MapIndex = data.objMapIndexes[i];
            }






            attackableTiles = new List<List<TileScript>>();
            ShowWhite();
            for (int i = 0; i < visitedCount; i++)
            {
                MapDetail vistiedDetail = Common.ConstructMapFromStringArray(dataString, dataIIndex, ref dataIIndex);
                visitedMaps.Add(vistiedDetail);

            }

            int livingCount = 0;
            System.Int32.TryParse(dataString[dataIIndex], out livingCount);
            dataIIndex++;

            for (int i = 0; i < livingCount; i++)
            {
                LivingObject possibleCharacter = Common.ConstructLivingFromStringArray(dataString, dataIIndex, ref dataIIndex);
                if (!gridObjects.Contains(possibleCharacter))
                {
                    gridObjects.Add(possibleCharacter);
                }
                if (possibleCharacter.GetComponent<EnemyScript>() || possibleCharacter.GetComponent<HazardScript>())
                {
                    if (!liveEnemies.Contains(possibleCharacter))
                    {

                        liveEnemies.Add(possibleCharacter);
                    }
                }
            }


            SoftReset();
            if (turnOrder.Count > 0)
                currentObject = turnOrder[0];

            if (gridobjs.Count > 0)
            {

                tempGridObj.currentTile = gridobjs[0].currentTile;
                ShowSelectedTile(tempGridObj);
            }

            myCamera.UpdateCamera();

            if (shopItems != null)
            {
                shopItems.Clear();
                for (int i = 0; i < 5; i++)
                {
                    UsableScript newItem = null;

                    int itemNum = 0;

                    switch (i)
                    {
                        case 0:
                            itemNum = Random.Range(0, 13);
                            newItem = database.GetWeapon(itemNum, null);
                            break;
                        case 1:
                            itemNum = Random.Range(0, 4);
                            itemNum += (Random.Range(1, 8) * 10);
                            //    Debug.Log("command skill" + itemNum);
                            newItem = database.GetSkill(itemNum);
                            break;
                        case 2:
                            itemNum = Random.Range(4, 7);
                            itemNum += (Random.Range(1, 10) * 10);
                            //    Debug.Log("command spell" + itemNum);
                            newItem = database.GetSkill(itemNum);
                            break;
                        case 3:
                            itemNum = Random.Range(110, 120);
                            //  Debug.Log("auto " + itemNum);
                            newItem = database.GetSkill(itemNum);
                            break;
                        case 4:
                            itemNum = Random.Range(204, 214);
                            //  Debug.Log("passive " + itemNum);
                            newItem = database.GetSkill(itemNum);
                            break;

                        default:
                            break;
                    }
                    shopItems.Add(newItem);
                }
            }
        }
        CleanMenuStack(true);

        if (currentRoomName)
        {
            if (!currentRoomName.isSetup)
            {
                currentRoomName.Setup();
            }
            if (currentRoomName.textmeshpro)
            {
                currentRoomName.textmeshpro.text = currentMap.mapName;
                if (currentMap.mapIndex == 4)
                    currentRoomName.textmeshpro.text = "Storage Room";
            }
        }
        prevState = State.FreeCamera;
        if (turnOrder.Count > 0)
        {
            CreateEvent(this, turnOrder[0], "Phase Announce Event", PhaseAnnounce, null, -1, PhaseAnnounceStart);

        }
        CreateEvent(this, null, "return state event", BufferedStateChange);
        turnImgManger.LoadTurnImg(turnOrder);
        turnImgManger.UpdateSelection(-1);
    }
    public void Clear()
    {
        if (null == tileMap)
            return;
        myCamera.infoObject = null;
        enterStateTransition();
        for (int i = 0; i < gridObjects.Count; i++)
        {
            gridObjects[i].currentTile = null;
            if (gridObjects[i].FACTION != Faction.ally)
            {
                gridObjects[i].gameObject.SetActive(false);
            }
            //if (gridObjects[i].GetComponent<EnemyScript>())
            //    gridObjects[i].gameObject.SetActive(false);
            //else if (gridObjects[i].GetComponent<HazardScript>())
            //    gridObjects[i].gameObject.SetActive(false);
            //else if (!gridObjects[i].GetComponent<LivingObject>())
            //    gridObjects[i].gameObject.SetActive(false);

            //   if (gridObjects[i].GetComponent<SpriteRenderer>())
            // gridObjects[i].GetComponent<SpriteRenderer>().color = Common.trans;
        }
        if (tileMap != null)
        {

            for (int i = tileMap.Count - 1; i >= 0; i--)
            {
                if (tileMap[i])
                {
                    tileMap[i].isOccupied = false;
                    tileMap[i].transform.parent = null;
                    tileMap[i].gameObject.SetActive(false);
                }
            }
        }
        tileMap.Clear();
        tileMap = null;

        gridObjects.Clear();
    }

    public void ClearObjects()
    {
        if (null == tileMap)
            return;
        myCamera.infoObject = null;
        for (int i = 0; i < gridObjects.Count; i++)
        {
            if (gridObjects[i].GetType().IsSubclassOf(typeof(LivingObject)))
            {
                gridObjects[i].currentTile = null;
                gridObjects[i].gameObject.SetActive(false);
            }


        }


        gridObjects.Clear();
        turnOrder.Clear();

    }
    public void SoftReset()
    {
        if (tileMap == null)
        {
            return;
        }
        for (int i = 0; i < tileMap.Count; i++)
        {
            tileMap[i].isOccupied = false;
        }


        GridObject[] objs = GameObject.FindObjectsOfType<GridObject>();

        for (int i = 0; i < objs.Length; i++)
        {
            GridObject griddy = objs[i];
            if (griddy.GetComponent<TempObject>())
            {
                continue;
            }

            if (!griddy.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (griddy.GetComponent<LivingObject>())
            {
                if (griddy.GetComponent<LivingObject>().DEAD)
                {
                    continue;
                }
            }
            if (!gridObjects.Contains(griddy))
                gridObjects.Add(griddy);
            //    Debug.Log(griddy.NAME);
            griddy.currentTile = GetTile(griddy);
            if (griddy.currentTile != null)
                griddy.currentTile.isOccupied = true;

        }
    }

    public void NextRound()
    {
        if (GetState() == State.PlayerTransition)
        {
            currentState = prevState;
        }
        myCamera.armorSet.selectedArmor = null;
        currOppList.Clear();
        doubleAdjOppTiles.Clear();
        menuManager.ShowNone();
        // eventManager.gridEvents.Clear();
        LivingObject[] livingObjects = GameObject.FindObjectsOfType<LivingObject>();

        for (int i = 0; i < turnOrder.Count; i++)
        {
            int acts = (int)(turnOrder[i].SPEED / 10);

            //            Debug.Log(turnOrder[i].NAME + ", gen: " + turnOrder[i].GENERATED + " , phase:" + currentState);
            if (turnOrder[i].GENERATED < 0)
            {
                if ((-1 * turnOrder[i].GENERATED) >= acts)
                {
                    acts = 3;
                }
            }
            else
            {
                if (turnOrder[i].SPEED > 0)
                {
                    acts += turnOrder[i].GENERATED;
                    acts += 3;
                }
                else
                {
                    acts = 1;
                }

            }


            turnOrder[i].GENERATED = 0;
            turnOrder[i].ACTIONS = acts;
            // turnOrder[i].LAST_USED.Clear();
            turnOrder[i].updateLastSprites();
        }

        switch (currentState)
        {
            case State.HazardTurn:

                currentState = State.FreeCamera;
                prevState = State.FreeCamera;

                if (Common.enemiesCompletedPhase == false)
                {

                    List<tutorialStep> tSteps = new List<tutorialStep>();
                    List<int> tClar = new List<int>();

                    tSteps.Add(tutorialStep.showTutorial);
                    tSteps.Add(tutorialStep.moveToPosition);
                    tSteps.Add(tutorialStep.showTutorial);
                    tSteps.Add(tutorialStep.allocate);
                    tSteps.Add(tutorialStep.showTutorial);
                    tClar.Add(23);
                    tClar.Add(35);
                    tClar.Add(24);
                    tClar.Add(-1);
                    tClar.Add(26);
                    PrepareTutorial(tSteps, tClar);
                    Common.enemiesCompletedPhase = true;
                }
                break;
            case State.RevenantPhase:
                currentState = State.VamprettiPhase;
                prevState = State.VamprettiPhase;
                menuManager.ShowNone();
                break;

            case State.VamprettiPhase:
                currentState = State.AntileonPhase;
                prevState = State.AntileonPhase;
                menuManager.ShowNone();
                break;

            case State.AntileonPhase:
                currentState = State.ThiefPhase;
                prevState = State.ThiefPhase;
                menuManager.ShowNone();
                break;

            case State.ThiefPhase:
                currentState = State.GeniePhase;
                prevState = State.GeniePhase;
                menuManager.ShowNone();
                break;

            case State.GeniePhase:
                currentState = State.FairyPhase;
                prevState = State.FairyPhase;
                menuManager.ShowNone();
                break;

            case State.FairyPhase:
                currentState = State.EnemyTurn;
                prevState = State.EnemyTurn;
                menuManager.ShowNone();
                break;

            case State.EnemyTurn:
                currentState = State.HazardTurn;
                prevState = State.HazardTurn;
                menuManager.ShowNone();
                break;
            default:
                currentState = State.RevenantPhase;
                prevState = State.RevenantPhase;
                menuManager.ShowNone();
                break;
        }

        turnOrder.Clear();
        for (int i = livingObjects.Length - 1; i >= 0; i--)
        {

            if (!turnOrder.Contains(livingObjects[i]))
            {
                if (!livingObjects[i].DEAD)
                {


                    if (GetState() == State.HazardTurn)
                    {
                        if (livingObjects[i].FACTION == Faction.hazard)
                        {
                            turnOrder.Add(livingObjects[i]);
                        }
                    }

                    else if (currentState == State.EnemyTurn)
                    {
                        if (livingObjects[i].FACTION == Faction.enemy)
                        {
                            turnOrder.Add(livingObjects[i]);
                        }

                    }
                    else if (currentState == State.RevenantPhase)
                    {

                        if (livingObjects[i].FACTION == Faction.revenant)
                        {
                            turnOrder.Add(livingObjects[i]);
                        }
                    }

                    else if (currentState == State.VamprettiPhase)
                    {

                        if (livingObjects[i].FACTION == Faction.vamprretti)
                        {
                            turnOrder.Add(livingObjects[i]);
                        }
                    }

                    else if (currentState == State.AntileonPhase)
                    {

                        if (livingObjects[i].FACTION == Faction.antileon)
                        {
                            turnOrder.Add(livingObjects[i]);
                        }
                    }

                    else if (currentState == State.ThiefPhase)
                    {

                        if (livingObjects[i].FACTION == Faction.thieves)
                        {
                            turnOrder.Add(livingObjects[i]);
                        }
                    }

                    else if (currentState == State.GeniePhase)
                    {

                        if (livingObjects[i].FACTION == Faction.genie)
                        {
                            turnOrder.Add(livingObjects[i]);
                        }
                    }

                    else if (currentState == State.FairyPhase)
                    {

                        if (livingObjects[i].FACTION == Faction.fairy)
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

        for (int i = 0; i < turnOrder.Count; i++)
        {
            if (turnOrder[i].ACTIONS < 0)
            {
                int amt = Mathf.Abs(turnOrder[i].ACTIONS) * 10;
                //Debug.Log("damaging:" + amt);
                if (amt >= turnOrder[i].HEALTH)
                {
                    amt = turnOrder[i].HEALTH - 1;
                }
                DamageGridObject(turnOrder[i], amt);
                CreateDmgTextEvent("<sprite=2> - " + amt.ToString(), Common.red, turnOrder[i]);
                CreateDmgTextEvent("Desperation", Common.red, turnOrder[i]);
                CreateEvent(this, turnOrder[i], "Action Update: ", ReturnTrue, null, 0, DesperationUpdate);

                int shieldCount = Mathf.Abs(turnOrder[i].ACTIONS);
                for (int j = 0; j < shieldCount; j++)
                {
                    turnOrder[i].Shield();
                }
                turnOrder[i].PrepareBarrier(database.GetArmor(211, turnOrder[i]));
            }
            else if (turnOrder[i].ACTIONS == 0)
            {

                turnOrder[i].PrepareBarrier(database.GetArmor(211, turnOrder[i]));
            }
        }

        if (turnOrder.Count > 0)
        {
            turnImgManger.LoadTurnImg(turnOrder);
            turnImgManger.UpdateSelection(-1);
            if (currentState == State.FreeCamera)
                currentState = State.PlayerTransition;



            bool canAct = false;
            for (int i = 0; i < turnOrder.Count; i++)
            {
                if (turnOrder[i].GetComponent<BuffScript>())
                {
                    BuffScript[] buffs = turnOrder[i].GetComponents<BuffScript>();

                    for (int j = 0; j < buffs.Length; j++)
                    {
                        buffs[j].UpdateCount(buffs[j].SKILL.OWNER);
                    }
                }
                if (turnOrder[i].GetComponent<DebuffScript>())
                {
                    DebuffScript[] buffs = turnOrder[i].GetComponents<DebuffScript>();

                    for (int j = 0; j < buffs.Length; j++)
                    {
                        buffs[j].UpdateCount(buffs[j].SKILL.OWNER);
                    }
                }
                if (turnOrder[i].ARMOR.SCRIPT != turnOrder[i].DEFAULT_ARMOR)
                {
                    if (turnOrder[i].ARMOR.UpdateTurnCount())
                    {
                        CreateTextEvent(this, "" + turnOrder[i].NAME + " barrier has worn off", " barrier subside", CheckText, TextStart);
                    }
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

                    if (turnOrder[i].RENDERER)
                    {
                        if (turnOrder[i].currentTile.isInShadow)
                        {
                            AttemptToGoIntoShadows(turnOrder[i]);
                        }
                        else
                        {
                            turnOrder[i].RENDERER.color = Color.white;
                        }
                    }

                    if (turnOrder[i].INVENTORY != null)
                    {
                        turnOrder[i].INVENTORY.ChargeShields();//GetComponent<EffectScript>().ApplyReaction(this, turnOrder[i].GetComponent<LivingObject>());
                    }

                    for (int j = turnOrder[i].INVENTORY.EFFECTS.Count - 1; j >= 0; j--)
                    {
                        EffectScript anEffect = turnOrder[i].INVENTORY.EFFECTS[j];
                        anEffect.ApplyReaction(this, turnOrder[i]);
                    }

                    if (turnOrder[i].GetComponent<SecondStatusScript>())
                    {
                        turnOrder[i].GetComponent<SecondStatusScript>().ReduceCount(this, turnOrder[i]);
                    }
                    // Debug.Log(" "+turnOrder[i].NAME + " has " + acts + " actions due to " + ((int)(turnOrder[i].SPEED / 10)) + " + 2 + gene " + turnOrder[i].GENERATED);

                    //turnOrder[i].GENERATED = 0;
                    turnOrder[i].turnUpdate(liveEnemies.Count);
                    if (turnOrder[i].ACTIONS > 0)
                    {
                        canAct = true;

                        if (turnOrder[i].FACTION != Faction.ally && turnOrder[i].FACTION != Faction.hazard)
                        {
                            EnemyScript anEnemy = turnOrder[i].GetComponent<EnemyScript>();
                            CreateEvent(this, anEnemy, "Enemy Event", EnemyEvent, null, -1, SetEnemyEvent);
                        }

                        if (turnOrder[i].FACTION == Faction.hazard)
                        {
                            HazardScript hazard = turnOrder[i].GetComponent<HazardScript>();
                            CreateEvent(this, hazard, "Hazard Event", HazardEvent, null, -1, SetHazardEvent);
                        }
                    }

                    UpdateMarkedArea();
                    nextRoundCalled = false;
                }

            }
            for (int i = livingObjects.Length - 1; i >= 0; i--)
            {
                livingObjects[i].updateAilmentIcons();
            }

            if (canAct == true)
            {
                CreateEvent(this, turnOrder[0], "Phase Announce Event", PhaseAnnounce, null, 0, PhaseAnnounceStart);
            }
            else if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
            {
                myCamera.PlaySoundTrack(9);
                menuManager.ShowNone();
                menuManager.ShowGameOver();

                //eventManager.gridEvents.Clear();
                currentState = State.PlayerDead;
                nextRoundCalled = true;
            }
            else
            {
                nextRoundCalled = false;
                NextTurn("next round moving on");
            }
        }
        else if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
        {
            myCamera.PlaySoundTrack(9);
            menuManager.ShowNone();
            menuManager.ShowGameOver();

            //eventManager.gridEvents.Clear();
            currentState = State.PlayerDead;
            nextRoundCalled = true;
        }
        else
        {
            nextRoundCalled = false;
            NextTurn("next round moving on");
        }
        SoftReset();
        myCamera.UpdateCamera();
        updateConditionals();
    }
    public void ReviveCheat()
    {
        nextRoundCalled = false;
        LivingObject[] livingObjects = GameObject.FindObjectsOfType<LivingObject>();
        for (int i = livingObjects.Length - 1; i >= 0; i--)
        {
            if (livingObjects[i].FACTION == Faction.ally)
            {
                LivingObject playable = livingObjects[i];

                if (playable.SHADOW)
                {
                    if (playable.SHADOW.SPRITE.sr)
                    {

                        playable.SHADOW.SPRITE.sr.color = Common.GetFactionColor(playable.FACTION);
                    }
                }

                playable.DEAD = false;

                playable.STATS.Reset(true);

                playable.STATS.HEALTH = (int)(playable.BASE_STATS.MAX_HEALTH);
                playable.STATS.MANA = (int)(playable.BASE_STATS.MAX_MANA);
                playable.STATS.FATIGUE = (int)(playable.BASE_STATS.MAX_FATIGUE);


                playable.RENDERER.color = Color.white;

                //   playable.updateLastSprites();
                for (int j = 0; j < playable.INVENTORY.EFFECTS.Count; j++)
                {
                    EffectScript anEffect = playable.INVENTORY.EFFECTS[j];
                    PoolManager.GetManager().ReleaseEffect(anEffect);
                }
                playable.INVENTORY.EFFECTS.Clear();
                playable.Refresh();
                playable.updateAilmentIcons();
                gridObjects.Add(playable);
                playable.UpdateHealthbar();
            }

            livingObjects[i].Refresh();
        }
        if (PlayerPrefs.HasKey(Common.currentStory + "_c"))
        {
            PlayerPrefs.DeleteKey(Common.currentStory + "_c");
        }
        SoftReset();

        myCamera.PlayPreviousSoundTrack();
    }
    public void ReviveFull()
    {
        nextRoundCalled = false;
        LivingObject[] livingObjects = GameObject.FindObjectsOfType<LivingObject>();
        for (int i = livingObjects.Length - 1; i >= 0; i--)
        {
            if (livingObjects[i].FACTION == Faction.ally)
            {
                LivingObject playable = livingObjects[i];

                if (playable.SHADOW)
                {
                    if (playable.SHADOW.SPRITE.sr)
                    {

                        playable.SHADOW.SPRITE.sr.color = Common.GetFactionColor(playable.FACTION);
                    }
                }

                playable.DEAD = false;

                playable.STATS.Reset(true);

                playable.STATS.HEALTH = (int)(playable.BASE_STATS.MAX_HEALTH);
                playable.STATS.MANA = (int)(playable.BASE_STATS.MAX_MANA);
                playable.STATS.FATIGUE = (int)(playable.BASE_STATS.MAX_FATIGUE);

                CommandSkill skill = playable.GetMostUsedSkill();
                CommandSkill spell = playable.GetMostUsedSpell();
                WeaponScript weapon = playable.GetMostUsedStrike();
                ArmorScript barrier = playable.GetMostUsedBarrier();
                if (skill)
                {
                    playable.INVENTORY.USEABLES.Remove(skill);
                    playable.INVENTORY.CSKILLS.Remove(skill);
                    playable.PHYSICAL_SLOTS.SKILLS.Remove(skill);
                }

                if (spell)
                {
                    playable.INVENTORY.USEABLES.Remove(spell);
                    playable.INVENTORY.CSKILLS.Remove(spell);
                    playable.MAGICAL_SLOTS.SKILLS.Remove(spell);
                }

                if (weapon)
                {
                    playable.INVENTORY.USEABLES.Remove(weapon);
                    playable.INVENTORY.WEAPONS.Remove(weapon);
                }


                if (barrier)
                {
                    playable.INVENTORY.USEABLES.Remove(barrier);
                    playable.INVENTORY.ARMOR.Remove(barrier);
                }


                playable.RENDERER.color = Color.white;

                //   playable.updateLastSprites();
                for (int j = 0; j < playable.INVENTORY.EFFECTS.Count; j++)
                {
                    EffectScript anEffect = playable.INVENTORY.EFFECTS[j];
                    PoolManager.GetManager().ReleaseEffect(anEffect);
                }
                playable.INVENTORY.EFFECTS.Clear();
                playable.Refresh();
                playable.updateAilmentIcons();
                gridObjects.Add(playable);
                playable.UpdateHealthbar();
            }

            livingObjects[i].Refresh();
        }
        SoftReset();

        myCamera.PlayPreviousSoundTrack();
    }

    public void ReviveMedium()
    {
        nextRoundCalled = false;
        LivingObject[] livingObjects = GameObject.FindObjectsOfType<LivingObject>();
        for (int i = livingObjects.Length - 1; i >= 0; i--)
        {
            if (livingObjects[i].FACTION == Faction.ally)
            {

                LivingObject playable = livingObjects[i];

                if (playable.SHADOW)
                {
                    if (playable.SHADOW.SPRITE.sr)
                    {

                        playable.SHADOW.SPRITE.sr.color = Common.GetFactionColor(playable.FACTION);
                    }
                }

                for (int j = playable.INVENTORY.USEABLES.Count - 1; j >= 0; j--)
                {
                    UsableScript usable = playable.INVENTORY.USEABLES[j];
                    if (usable.GetType() == typeof(ComboSkill) || usable.GetType() == typeof(AutoSkill) || usable.GetType() == typeof(OppSkill))
                    {
                        playable.INVENTORY.USEABLES.Remove(usable);
                    }
                    playable.INVENTORY.COMBOS.Clear();
                    playable.INVENTORY.AUTOS.Clear();
                    playable.INVENTORY.OPPS.Clear();

                    playable.COMBO_SLOTS.SKILLS.Clear();
                    playable.AUTO_SLOTS.SKILLS.Clear();
                    playable.OPP_SLOTS.SKILLS.Clear();
                    playable.INVENTORY.OPPS.Clear();
                }
                playable.DEAD = false;

                playable.STATS.Reset(true);
                playable.STATS.HEALTH = (int)(playable.BASE_STATS.MAX_HEALTH * 0.5f);

                playable.STATS.MANA = (int)(playable.BASE_STATS.MAX_MANA * 0.5f);


                playable.STATS.FATIGUE = (int)(playable.BASE_STATS.MAX_FATIGUE * 0.5f);


                playable.RENDERER.color = Color.white;
                //  playable.updateLastSprites();
                for (int j = 0; j < playable.INVENTORY.EFFECTS.Count; j++)
                {
                    EffectScript anEffect = playable.INVENTORY.EFFECTS[j];
                    PoolManager.GetManager().ReleaseEffect(anEffect);
                }
                playable.INVENTORY.EFFECTS.Clear();
                playable.Refresh();
                playable.updateAilmentIcons();
                gridObjects.Add(playable);
                playable.UpdateHealthbar();
            }
            livingObjects[i].Refresh();
        }
        SoftReset();
        myCamera.PlayPreviousSoundTrack();
    }
    public void ReviveLow()
    {
        nextRoundCalled = false;
        //Revive each character with 1 hp, but halves all stats.
        LivingObject[] livingObjects = GameObject.FindObjectsOfType<LivingObject>();
        for (int i = livingObjects.Length - 1; i >= 0; i--)
        {
            if (livingObjects[i].FACTION == Faction.ally)
            {
                LivingObject playable = livingObjects[i];

                if (playable.SHADOW)
                {
                    if (playable.SHADOW.SPRITE.sr)
                    {

                        playable.SHADOW.SPRITE.sr.color = Common.GetFactionColor(playable.FACTION);
                    }
                }



                playable.BASE_STATS.STRENGTH = Mathf.RoundToInt(playable.BASE_STATS.STRENGTH * 0.5f);
                playable.BASE_STATS.DEFENSE = Mathf.RoundToInt(playable.BASE_STATS.DEFENSE * 0.5f);
                playable.BASE_STATS.SPEED = Mathf.RoundToInt(playable.BASE_STATS.SPEED * 0.5f);

                playable.BASE_STATS.MAGIC = Mathf.RoundToInt(playable.BASE_STATS.MAGIC * 0.5f);
                playable.BASE_STATS.RESIESTANCE = Mathf.RoundToInt(playable.BASE_STATS.RESIESTANCE * 0.5f);
                playable.BASE_STATS.DEX = Mathf.RoundToInt(playable.BASE_STATS.DEX * 0.5f);

                if (playable.BASE_STATS.STRENGTH == 0)
                    playable.BASE_STATS.STRENGTH = 1;

                if (playable.BASE_STATS.MAGIC == 0)
                    playable.BASE_STATS.MAGIC = 1;

                if (playable.BASE_STATS.DEX == 0)
                    playable.BASE_STATS.DEX = 1;

                if (playable.BASE_STATS.SPEED == 0)
                    playable.BASE_STATS.SPEED = 1;

                if (playable.BASE_STATS.DEFENSE == 0)
                    playable.BASE_STATS.DEFENSE = 1;

                if (playable.BASE_STATS.RESIESTANCE == 0)
                    playable.BASE_STATS.RESIESTANCE = 1;
                playable.DEAD = false;

                playable.STATS.Reset(true);
                // playable.STATS.HEALTH = playable.BASE_STATS.MAX_HEALTH;
                //   if (playable.STATS.MANA > playable.BASE_STATS.MAX_MANA)
                playable.STATS.MANA = playable.BASE_STATS.MAX_MANA;
                //  if (playable.STATS.FATIGUE > playable.BASE_STATS.MAX_FATIGUE)
                playable.STATS.FATIGUE = playable.BASE_STATS.MAX_FATIGUE;

                playable.STATS.HEALTH = 1;
                playable.RENDERER.color = Color.white;
                //  playable.updateLastSprites();
                for (int j = 0; j < playable.INVENTORY.EFFECTS.Count; j++)
                {
                    EffectScript anEffect = playable.INVENTORY.EFFECTS[j];
                    PoolManager.GetManager().ReleaseEffect(anEffect);
                }
                playable.INVENTORY.EFFECTS.Clear();
                playable.Refresh();
                playable.updateAilmentIcons();
                gridObjects.Add(playable);
                playable.UpdateHealthbar();
            }
            livingObjects[i].Refresh();
        }
        SoftReset();
        myCamera.PlayPreviousSoundTrack();
    }
    public void forceReloadRound(Faction newfaction)
    {
        switch (newfaction)
        {
            case Faction.ally:
                currentState = State.FreeCamera;
                break;
            case Faction.enemy:
                currentState = State.EnemyTurn;
                break;
            case Faction.hazard:
                currentState = State.HazardTurn;
                break;
            case Faction.ordinary:
                break;
            case Faction.fairy:
                currentState = State.EnemyTurn;
                break;
        }
        turnOrder.Clear();

        LivingObject[] livingObjects = GameObject.FindObjectsOfType<LivingObject>();
        for (int i = livingObjects.Length - 1; i >= 0; i--)
        {
            if (!turnOrder.Contains(livingObjects[i]))
            {
                if (!livingObjects[i].DEAD)
                {
                    if (livingObjects[i].FACTION == newfaction)
                    {
                        turnOrder.Add(livingObjects[i]);
                    }
                    int acts = (int)(livingObjects[i].SPEED / 10);

                    if (livingObjects[i].GENERATED < 0)
                    {
                        if (-1 * livingObjects[i].GENERATED >= acts)
                        {
                            acts = 3;
                        }
                    }
                    else
                    {
                        if (turnOrder[i].SPEED > 0)
                        {
                            acts += turnOrder[i].GENERATED;
                            acts += 3;
                        }
                        else
                        {
                            acts = 1;
                        }
                    }

                    livingObjects[i].GENERATED = 0;


                    livingObjects[i].ACTIONS = acts;

                }
            }

        }

        if (turnOrder.Count > 0)
        {
            turnImgManger.LoadTurnImg(turnOrder);
            CreateEvent(this, turnOrder[0], "Phase Announce Event", PhaseAnnounce, null, 0, PhaseAnnounceStart);


            for (int i = 0; i < turnOrder.Count; i++)
            {
                if (turnOrder[i].GetComponent<BuffScript>())
                {
                    BuffScript[] buffs = turnOrder[i].GetComponents<BuffScript>();

                    for (int j = 0; j < buffs.Length; j++)
                    {
                        buffs[j].UpdateCount(turnOrder[i]);
                    }
                }

                if (turnOrder[i].GetComponent<DebuffScript>())
                {
                    DebuffScript[] buffs = turnOrder[i].GetComponents<DebuffScript>();

                    for (int j = 0; j < buffs.Length; j++)
                    {
                        buffs[j].UpdateCount(turnOrder[i]);
                    }
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



                    if (turnOrder[i].GetComponent<InventoryScript>())
                    {
                        turnOrder[i].INVENTORY.ChargeShields();//GetComponent<EffectScript>().ApplyReaction(this, turnOrder[i].GetComponent<LivingObject>());
                    }

                    // turnOrder[i].GetComponent<EffectScript>().ApplyReaction(this, turnOrder[i]);



                    for (int j = turnOrder[i].INVENTORY.EFFECTS.Count - 1; j >= 0; j--)
                    {
                        EffectScript anEffect = turnOrder[i].INVENTORY.EFFECTS[j];
                        anEffect.ApplyReaction(this, turnOrder[i]);
                    }


                    if (turnOrder[i].GetComponent<SecondStatusScript>())
                    {
                        turnOrder[i].GetComponent<SecondStatusScript>().ReduceCount(this, turnOrder[i].GetComponent<LivingObject>());
                    }
                    // Debug.Log(" "+turnOrder[i].NAME + " has " + acts + " actions due to " + ((int)(turnOrder[i].SPEED / 10)) + " + 2 + gene " + turnOrder[i].GENERATED);

                    //  turnOrder[i].GENERATED = 0;

                    if (turnOrder[i].GetComponent<EnemyScript>())
                    {
                        EnemyScript anEnemy = turnOrder[i].GetComponent<EnemyScript>();
                        CreateEvent(this, anEnemy, "Enemy Event", EnemyEvent, null, -1, SetEnemyEvent);
                    }

                    if (turnOrder[i].GetComponent<HazardScript>())
                    {
                        HazardScript hazard = turnOrder[i].GetComponent<HazardScript>();
                        CreateEvent(this, hazard, "Hazard Event", HazardEvent, null, -1, SetHazardEvent);
                    }
                    UpdateMarkedArea();
                }
            }
        }

    }
    public void UpdateTurns()
    {
        turnImgManger.UpdateTurnImgActionCount(turnOrder);
    }
    public bool WaitForSFXEvent(Object data)
    {
        if (!sfx)
        {
            return true;
        }
        if (sfx.isPlaying())
        {
            return false;
        }
        return true;
    }
    public bool EnemyEvent(Object data)
    {
        EnemyScript enemy = data as EnemyScript;
        return !enemy.isPerforming;
    }
    public bool HazardEvent(Object data)
    {
        HazardScript enemy = data as HazardScript;
        return !enemy.isPerforming;
    }
    public void SetEnemyEvent(Object data)
    {
        EnemyScript enemy = data as EnemyScript;
        enemy.DetermineActions();
        currentObject = enemy;
    }

    public void SetHazardEvent(Object data)
    {
        HazardScript enemy = data as HazardScript;
        enemy.DetermineActions();
        currentObject = enemy;
    }
    public bool CameraEvent(Object data)
    {
        bool result = false;
        GridObject obj = (GridObject)data;
        if (obj != null)
        {

            if (obj.isSetup)
            {
                //   ShowGridObjectAffectArea(obj);
                if (myCamera)
                {
                    currentObject = obj;
                    myCamera.currentTile = obj.currentTile;
                    myCamera.infoObject = obj;
                }
                result = true;
                //if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                //{
                //    menuManager.ShowCommandCanvas();
                //}
                MoveCameraAndShow(obj);
                myCamera.UpdateCamera();
            }
            else
            {
                Debug.Log("obj not setup");
                obj.Setup();
                result = true;
            }
        }
        return result;
    }
    public void NextTurn(string invokingObj)
    {
        // Debug.Log("next " + invokingObj + " form stream");
        CreateEvent(this, null, "Next turn event from " + invokingObj, NextTurnEvent);

    }
    public void NextTurn(string invokingObj, State calledState)
    {
        if (currentState == calledState)
        {

            //  Debug.Log("next " + invokingObj + " " + calledState);
            CreateEvent(this, null, "Next turn event from " + invokingObj + " in " + calledState + " in actually cur: " + currentState, NextTurnEvent);
        }

    }
    public void showAttackableTiles()
    {
        for (int i = 0; i < attackableTiles.Count; i++)
        {
            for (int j = 0; j < attackableTiles[i].Count; j++)
            {
                if (player.currentSkill)
                {
                    if (player.currentSkill.SUBTYPE == SubSkillType.Buff || player.currentSkill.ELEMENT == Element.Support)
                    {
                        attackableTiles[i][j].MYCOLOR = Common.lime;

                    }
                    else
                    {
                        attackableTiles[i][j].MYCOLOR = Common.pink;

                    }
                }
                else
                {
                    if (GetState() == State.PlayerUsingItems)
                    {
                        if (player.currentItem)
                        {
                            if (player.currentItem.ITYPE != ItemType.dmg && player.currentItem.ITYPE != ItemType.dart)
                            {
                                attackableTiles[i][j].MYCOLOR = Common.lime;
                            }
                            else
                            {
                                attackableTiles[i][j].MYCOLOR = Common.pink;
                            }
                        }
                        else
                        {
                            attackableTiles[i][j].MYCOLOR = Common.pink;
                        }
                    }
                    else
                    {
                        attackableTiles[i][j].MYCOLOR = Common.pink;
                    }

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

            doubleAdjOppTiles[i].MYCOLOR = Common.orange;

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

    public void UpdateMarkedArea()
    {
        for (int j = 0; j < tileMap.Count; j++)
        {
            TileScript temp = tileMap[j];
            if (temp.isMarked == true)
            {
                if (temp.MYCOLOR != Common.orange)
                {
                    temp.isMarked = false;
                }
            }
        }
        for (int i = 0; i < markedEnemies.Count; i++)
        {
            LivingObject aliveObj = markedEnemies[i];
            if (aliveObj != null)
            {
                if (aliveObj.DEAD == false)
                {
                    ShowGridObjectAffectArea(aliveObj, false, true);
                }
            }
        }
    }
    [SerializeField]
    List<TileScript> atkbles = new List<TileScript>();

    public void ShowGridObjectAffectArea(GridObject obj, bool cameraLock = true, bool mark = false)
    {
        atkbles.Clear();

        LivingObject aliveObj = null;
        if (obj == null)
        {
            return;
        }
        if (obj.currentTile == null)
        {
            return;
        }

        if (obj.GetComponent<LivingObject>())
        {
            aliveObj = obj.GetComponent<LivingObject>();
        }

        if (obj.GetComponent<HazardScript>())
        {
            HazardScript hazy = obj.GetComponent<HazardScript>();
            if (hazy.INVENTORY.CSKILLS.Count > 0)
            {
                ShowSkillAttackbleTiles(hazy, hazy.INVENTORY.CSKILLS[0]);
                return;
            }
        }
        // LivingObject liveObj = obj.GetComponent<LivingObject>();
        if (aliveObj)
        {
            if (!aliveObj.isSetup)
                aliveObj.Setup();

        }

        for (int i = 0; i < tileMap.Count; i++)
        {
            TileScript temp = tileMap[i];
            if (temp.canBeOccupied == false)
            {
                continue;
            }
            float tempX = temp.transform.position.x;
            float tempY = temp.transform.position.z;
            if (!obj)
                return;
            float objX = obj.currentTile.transform.position.x;
            float objY = obj.currentTile.transform.position.z;

            int MoveDist = 0;
            // int attackDist = 0;
            //  int range = 0;
            //  int actionsRemaining = 1;
            if (obj.GetComponent<LivingObject>())
            {
                MoveDist = aliveObj.MOVE_DIST;

                // if (StartCanMoveCheck(obj, obj.currentTile, temp) || temp == obj.currentTile)
                {
                    //ShowWeaponAttackbleTiles(liveObj, liveObj.WEAPON.EQUIPPED);
                    //  attackDist = Common.GetnAtkDist(liveObj);
                    //   range = Common.GetAtkRange(liveObj);
                    // actionsRemaining = liveObj.ACTIONS;



                }
            }
            xDist = Mathf.Abs(tempX - objX);
            yDist = Mathf.Abs(tempY - objY);
            xDist /= 2;
            yDist /= 2;
            if (xDist == 0 && yDist == 0)
            {
                if (cameraLock)
                {

                    myCamera.currentTile = temp.GetComponent<TileScript>();
                    if (!obj.GetComponent<TempObject>())
                        myCamera.infoObject = GetObjectAtTile(temp.GetComponent<TileScript>());
                }
                temp.MYCOLOR = Color.white;
            }

            else if (xDist + yDist <= MoveDist)
            {

                if (aliveObj)
                {
                    if (liveEnemies.Count > 0)
                    {

                        if (StartCanMoveCheck(aliveObj, aliveObj.currentTile, temp))
                        {
                            for (int jk = 0; jk < aliveObj.INVENTORY.WEAPONS.Count; jk++)
                            {
                                WeaponScript wep = aliveObj.INVENTORY.WEAPONS[jk];
                                if (wep.CanUse(wep.USER.STATS.HPCOSTCHANGE))
                                {

                                    List<TileScript> tlist = GetWeaponAttackableTilesOneList(temp, aliveObj.INVENTORY.WEAPONS[jk]);
                                    for (int t = 0; t < tlist.Count; t++)
                                    {
                                        if (!atkbles.Contains(tlist[t]) && tlist[t] != obj.currentTile)
                                            atkbles.Add(tlist[t]);
                                    }
                                }
                            }
                            for (int jk = 0; jk < aliveObj.INVENTORY.CSKILLS.Count; jk++)
                            {
                                CommandSkill cmd = aliveObj.INVENTORY.CSKILLS[jk];
                                float modification = 1.0f;
                                if (cmd.ETYPE == EType.magical)
                                    modification = aliveObj.STATS.MANACHANGE;
                                if (cmd.ETYPE == EType.physical)
                                {
                                    if (cmd.COST > 0)
                                    {
                                        modification = aliveObj.STATS.FTCHARGECHANGE;
                                    }
                                    else
                                    {
                                        modification = aliveObj.STATS.FTCOSTCHANGE;
                                    }
                                }
                                if (cmd.CanUse(modification))
                                {

                                    List<TileScript> tlist = GetSkillAttackableTilesOneList(temp, cmd);
                                    for (int t = 0; t < tlist.Count; t++)
                                    {
                                        if (!atkbles.Contains(tlist[t]) && tlist[t] != obj.currentTile)
                                            atkbles.Add(tlist[t]);
                                    }
                                }
                            }
                            temp.MYCOLOR = Color.cyan;
                            if (aliveObj != null)
                            {
                                if (aliveObj.FACTION != Faction.ally)
                                {
                                    temp.MYCOLOR = Color.blue;
                                }
                            }
                            if (mark == true)
                            {
                                temp.isMarked = true;
                            }
                        }
                        else
                        {
                            temp.MYCOLOR = Color.white;
                        }
                    }
                    else
                    {
                        if (temp.isOccupied)
                        {
                            temp.MYCOLOR = Color.red;
                            if (aliveObj != null)
                            {
                                if (aliveObj.FACTION != Faction.ally)
                                {
                                    temp.MYCOLOR = Color.magenta;
                                }
                            }
                            if (mark == true)
                            {
                                temp.isMarked = true;
                            }
                        }
                        else
                        {

                            temp.MYCOLOR = Color.cyan;
                            if (aliveObj != null)
                            {
                                if (aliveObj.FACTION != Faction.ally)
                                {
                                    temp.MYCOLOR = Color.blue;
                                }
                            }
                            if (mark == true)
                            {
                                temp.isMarked = true;
                            }
                        }
                    }

                }

            }
            else
            {
                temp.GetComponent<TileScript>().MYCOLOR = Color.white;

            }
        }
        //for (int i = 0; i < MapWidth; i++)
        //{
        //    for (int j = 0; j < MapHeight; j++)
        //    {

        //        TileScript temp = tileMap[TwoToOneD(j, MapWidth, i)];


        //    }
        //}

        for (int i = 0; i < atkbles.Count; i++)
        {
            if (aliveObj != null)
            {
                if (aliveObj.FACTION != Faction.ally)
                {
                    if (atkbles[i].MYCOLOR != Color.blue && atkbles[i].MYCOLOR != new Color(0, 0, 1 * 0.55f, 1))
                    {
                        atkbles[i].MYCOLOR = Color.magenta;
                    }

                }
            }
            else
            {

                if (atkbles[i].MYCOLOR != Color.cyan && atkbles[i].MYCOLOR != new Color(0, 1 * 0.55f, 1 * 0.55f, 1))
                {
                    atkbles[i].MYCOLOR = Color.red;
                }
            }
            if (mark == true)
            {
                atkbles[i].isMarked = true;
            }
        }
        myCamera.UpdateCamera();
    }
    public void ShowWhite()
    {
        for (int i = 0; i < MapWidth; i++)
        {
            for (int j = 0; j < MapHeight; j++)
            {
                TileScript temp = tileMap[TwoToOneD(j, MapWidth, i)];
                TileScript tile = temp.GetComponent<TileScript>();
                if (tile)
                {
                    if (tile.canBeOccupied)
                    {
                        temp.GetComponent<TileScript>().MYCOLOR = Color.white;

                    }
                }
            }
        }


        myCamera.UpdateCamera();

    }
    public void ShowSelectedTile(GridObject obj)
    {

        if (obj)
        {
            TileScript theTile = GetTile(obj);
            if (theTile != null)
            {

                theTile.MYCOLOR = Common.selcted;
                myCamera.UpdateCamera();
            }
        }
    }

    public void ShowSelectedTile(GridObject obj, Color color)
    {
        if (!obj)
            return;
        if (!GetTile(obj))
            return;
        TileScript theTile = GetTile(obj);
        theTile.MYCOLOR = color;
        myCamera.UpdateCamera();
    }

    public void ShowSelectedTile(TileScript theTile, Color color)
    {

        theTile.MYCOLOR = color;
        myCamera.UpdateCamera();
    }
    private List<TileScript> pathTiles;
    private List<TileScript> visitedTiles = new List<TileScript>();
    public bool StartCanMoveCheck(LivingObject target, TileScript startTile, TileScript targetTile)
    {
        if (target == tempGridObj)
        {
            if (targetTile.canBeOccupied == false)
                return false;
            else
                return true;
        }
        if (targetTile.canBeOccupied == false)
            return false;
        visitedTiles.Clear();
        if (pathTiles == null)
            pathTiles = new List<TileScript>();
        else
            pathTiles.Clear();
        BoolConatainer con = CanMoveToTile(target, startTile, targetTile);
        bool returnedBool = con.result;
        if (currentMap.mapIndex == 3 && returnedBool == true)
        {
            // Debug.Log(con.name);

            //  Debug.Log(target.FullName + " can move to " + targetTile.name + " resulted in " + returnedBool);
        }
        return returnedBool;
    }
    public BoolConatainer CanMoveToTile(LivingObject target, TileScript startTile, TileScript targetTile, int depth = 0)
    {
        BoolConatainer conatainer = new BoolConatainer();
        // Debug.Log(target.FullName + " pathing at depth " + depth);

        if (depth > MapWidth * MapHeight)
        {
            Debug.Log("Pathing error, depth:" + depth);
            conatainer.result = false;
            return conatainer;
        }

        if (liveEnemies.Count == 0)
        {
            if (targetTile)
            {
                if (targetTile.canBeOccupied == true)
                {
                    if (targetTile == startTile)
                    {
                        conatainer.result = true;
                        conatainer.name = "bypass cuz same";
                        return conatainer;
                    }

                    if (currentMap.objMapIndexes.Count == 0)
                    {


                        conatainer.result = true;
                        conatainer.name = "bypass cuz no enemies";
                        return conatainer;
                    }
                }
            }
        }

        // Debug.Log(target.FullName + " pathing at depth " + depth + " options[ " + i + "] tile at " + options[i].transform.position  );
        //  bool canMove = false;
        TileScript current = startTile;
        if (pathTiles.Contains(current))
        {
            conatainer.result = false;
            return conatainer;
        }
        if (pathTiles.Count >= target.MOVE_DIST)
        {
            conatainer.result = false;
            return conatainer;
        }
        pathTiles.Add(current);

        List<TileScript> options = GetAdjecentTiles(startTile);


        for (int i = 0; i < options.Count; i++)
        {

            current = options[i];
            if (visitedTiles.Contains(current))
            {
                continue;
            }
            //  if (!visitedTiles.Contains(current))
            //    visitedTiles.Add(current);
            if (pathTiles.Contains(current))
            {
                continue;
            }
            //pathTiles.Add(current);
            if (current.canBeOccupied == false)
            {
                continue;
            }

            if (current.isOccupied)
            {
                GridObject obj = null;
                if (obj = GetObjectAtTile(current))
                {

                    if (obj.FACTION != target.FACTION)
                    {
                        if (!visitedTiles.Contains(current))
                            visitedTiles.Add(current);
                        continue;
                    }

                }

            }

            if (current == targetTile)
            {

                //  Debug.Log(target.FullName + " pathing at depth " + depth + "  TRUES, tile at " + current.name);
                conatainer.result = true;
                conatainer.name = current.name;
                return conatainer;
            }



        }





        if (depth > target.MOVE_DIST)
        {
            conatainer.result = false;
            return conatainer;
        }

        if (pathTiles.Count >= target.MOVE_DIST)
        {
            conatainer.result = false;
            return conatainer;
        }

        depth += 1;
        for (int i = 0; i < options.Count; i++)
        {

            current = options[i];
            if (visitedTiles.Contains(current))
            {
                continue;
            }

            if (current.canBeOccupied == false)
            {
                continue;
            }
            if (current.isOccupied)
            {
                GridObject obj = null;
                if (obj = GetObjectAtTile(current))
                {
                    if (obj.FACTION != target.FACTION)
                    {
                        if (!visitedTiles.Contains(current))
                            visitedTiles.Add(current);
                        continue;
                    }

                }
                else
                {
                    Debug.Log("No obj found at occupied tile: " + current.name);
                    SoftReset();
                }
            }

            conatainer = CanMoveToTile(target, current, targetTile, depth);
            if (conatainer.result == true)
            {
                conatainer.name += " " + current.name;
                return conatainer;
            }
            else
            {
                if (current != target.currentTile)
                    pathTiles.Remove(current);
            }
            if (!visitedTiles.Contains(current))
                visitedTiles.Add(current);
        }

        //   Debug.Log(target.FullName + " pathing at depth " + depth + " returning false");
        return conatainer;
    }
    public BoolConatainerWTileList GetMovingTileList(LivingObject target, TileScript startTile, TileScript targetTile, int depth = 0)
    {
        BoolConatainerWTileList conatainer = new BoolConatainerWTileList();
        // Debug.Log(target.FullName + " pathing at depth " + depth);
        if (depth > MapWidth * MapHeight)
        {
            Debug.Log("Pathing error, depth:" + depth);
            conatainer.result = false;
            return conatainer;
        }

        // Debug.Log(target.FullName + " pathing at depth " + depth + " options[ " + i + "] tile at " + options[i].transform.position  );
        //  bool canMove = false;
        TileScript current = startTile;
        pathTiles.Add(current);

        List<TileScript> options = GetAdjecentTiles(startTile);


        for (int i = 0; i < options.Count; i++)
        {

            current = options[i];

            if (pathTiles.Contains(options[i]))
            {
                continue;
            }
            pathTiles.Add(current);

            if (current.isOccupied)
            {
                if (GetObjectAtTile(current))
                {

                    if (GetObjectAtTile(current).FACTION != target.FACTION)
                    {
                        continue;
                    }

                }

            }

            if (current == targetTile)
            {

                //  Debug.Log(target.FullName + " pathing at depth " + depth + "  TRUES, tile at " + current.name);
                conatainer.result = true;
                conatainer.name = current.name;
                conatainer.tiles.Add(targetTile);
                return conatainer;
            }



        }





        if (depth >= target.MOVE_DIST)
        {
            conatainer.result = false;
            return conatainer;
        }

        depth += 1;
        for (int i = 0; i < options.Count; i++)
        {

            current = options[i];

            if (current.isOccupied)
            {
                if (GetObjectAtTile(current))
                {

                    if (GetObjectAtTile(current).GetComponent<LivingObject>())
                    {
                        if (GetObjectAtTile(current).GetComponent<LivingObject>().FACTION != target.FACTION)
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    Debug.Log("No obj found at occupied tile: " + current.name);
                    SoftReset();
                }
            }

            conatainer = GetMovingTileList(target, current, targetTile, depth);
            if (conatainer.result == true)
            {
                conatainer.name += " " + current.name;
                conatainer.tiles.Add(targetTile);
                return conatainer;
            }
        }

        //   Debug.Log(target.FullName + " pathing at depth " + depth + " returning false");
        return conatainer;
    }


    public List<TileScript> GetMoveAbleTiles(Vector3 target, int MOVE_DIST, LivingObject currObj)
    {
        List<TileScript> returnedTiles = tileManager.EDITABLE_TILES;
        returnedTiles.Clear();
        TileScript startTile = GetTileAtIndex(GetTileIndex(target));
        if (startTile)
        {

            for (int i = 0; i < tileMap.Count; i++)
            {
                TileScript temp = tileMap[i];
                float tempX = temp.transform.position.x;
                float tempY = temp.transform.position.z;

                float objX = target.x;
                float objY = target.z;

                int MoveDist = MOVE_DIST;


                xDist = Mathf.Abs(tempX - objX);
                yDist = Mathf.Abs(tempY - objY);

                xDist /= 2;
                yDist /= 2;

                if (temp.GetComponent<TileScript>())
                {

                    TileScript tempTile = temp.GetComponent<TileScript>();
                    {

                        if (GetObjectAtTile(tempTile) == null && tempTile.isOccupied == false && tempTile.isActiveAndEnabled)
                        {

                            if (tempTile != startTile)
                            {

                                if (xDist + yDist <= MoveDist || (tempTile == currentObject.currentTile))
                                {
                                    //    Debug.Log("tile = " + tempTile.transform.position);
                                    if (StartCanMoveCheck(currObj, startTile, tempTile))
                                        returnedTiles.Add(tempTile);
                                }
                            }
                        }

                    }
                }

            }
        }

        return returnedTiles;
    }
    public void ShowGridObjectMoveArea(LivingObject obj, bool updateTileList = false)
    {
        if (obj == null)
            return;
        if (updateTileList == true)
        {

            if (tileManager)
            {
                tileManager.currentMoveableTiles.Clear();
                tileManager.currentMoveableTiles.Add(obj.currentTile);
            }
        }
        for (int i = 0; i < tileMap.Count; i++)
        {
            TileScript temp = tileMap[i];

            float tempX = temp.transform.position.x;
            float tempY = temp.transform.position.z;

            float objX = obj.currentTile.transform.position.x;
            float objY = obj.currentTile.transform.position.z;

            int MoveDist = obj.MOVE_DIST;


            xDist = Mathf.Abs(tempX - objX);
            yDist = Mathf.Abs(tempY - objY);
            xDist = xDist / 2;
            yDist = yDist / 2;
            if (xDist == 0 && yDist == 0)
            {
                // myCamera.currentTile = temp;
                // myCamera.infoObject = obj;
                temp.MYCOLOR = Color.cyan;
            }
            else if (xDist + yDist <= MoveDist)
            {

                if (StartCanMoveCheck(obj, obj.currentTile, temp))
                {
                    temp.MYCOLOR = Color.cyan;
                    if (updateTileList == true)
                    {
                        if (tileManager)
                        {
                            tileManager.currentMoveableTiles.Add(temp);
                        }
                    }
                }
                else
                {
                    temp.MYCOLOR = Color.white;
                }
            }
            else
            {
                temp.MYCOLOR = Color.white;
            }
        }
        //for (int i = 0; i < MapWidth; i++)
        //{
        //    for (int j = 0; j < MapHeight; j++)
        //    {
        //        TileScript temp = tileMap[TwoToOneD(j, MapWidth, i)];

        //    }
        //}
        myCamera.UpdateCamera();
    }
    public void ShowGridObjectAttackArea(GridObject obj)
    {
        if (obj == null)
            return;
        atkbles.Clear();
        LivingObject aliveObj = null;
        if (obj.GetComponent<LivingObject>())
        {
            aliveObj = obj.GetComponent<LivingObject>();
            // attackDist = liveObj.WEAPON.DIST;
        }
        if (aliveObj != null)
        {

            for (int i = 0; i < tileMap.Count; i++)
            {
                TileScript temp = tileMap[i];
                for (int jk = 0; jk < aliveObj.INVENTORY.WEAPONS.Count; jk++)
                {
                    WeaponScript wep = aliveObj.INVENTORY.WEAPONS[jk];
                    if (wep.CanUse(wep.USER.STATS.HPCOSTCHANGE))
                    {

                        List<TileScript> tlist = GetWeaponAttackableTilesOneList(aliveObj.currentTile, aliveObj.INVENTORY.WEAPONS[jk]);
                        Debug.Log("weapon " + aliveObj.INVENTORY.WEAPONS[jk].NAME + " count = " + tlist.Count);
                        for (int t = 0; t < tlist.Count; t++)
                        {
                            if (!atkbles.Contains(tlist[t]) && tlist[t] != obj.currentTile)
                            {
                                atkbles.Add(tlist[t]);
                            }
                        }
                    }
                }

            }
            for (int jk = 0; jk < aliveObj.INVENTORY.CSKILLS.Count; jk++)
            {
                CommandSkill cmd = aliveObj.INVENTORY.CSKILLS[jk];
                float modification = 1.0f;
                if (cmd.ETYPE == EType.magical)
                    modification = aliveObj.STATS.MANACHANGE;
                if (cmd.ETYPE == EType.physical)
                {
                    if (cmd.COST > 0)
                    {
                        modification = aliveObj.STATS.FTCHARGECHANGE;
                    }
                    else
                    {
                        modification = aliveObj.STATS.FTCOSTCHANGE;
                    }
                }
                if (cmd.CanUse(modification))
                {

                    List<TileScript> tlist = GetSkillAttackableTilesOneList(aliveObj.currentTile, cmd);
                    Debug.Log("skill " + cmd.NAME + " count = " + tlist.Count);
                    for (int t = 0; t < tlist.Count; t++)
                    {
                        if (!atkbles.Contains(tlist[t]) && tlist[t] != obj.currentTile)
                        {

                            atkbles.Add(tlist[t]);
                        }
                    }
                }
            }
            for (int i = 0; i < atkbles.Count; i++)
            {
                if (atkbles[i].MYCOLOR != Color.cyan)
                {
                    atkbles[i].MYCOLOR = Color.red;
                }
            }
            myCamera.UpdateCamera();
        }
    }
    public bool SetGridObjectPosition(GridObject obj, Vector3 newLocation)
    {
        if (!obj)
        {
            return false;
        }
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
    public void MoveGridObject(GridObject obj, Vector3 direction, bool ignoreDist = false, int depth = 0)
    {
        if (depth > currentMap.width * currentMap.height)
            return;
        // direction.Normalize();
        Vector3 curPos = GetTile(obj).transform.position;
        Vector3 newPos = curPos + (direction * 2);

        int TileIndex = TwoToOneD((int)newPos.z, MapWidth, (int)newPos.x);
        TileIndex = TileIndex / 2;
        if (TileIndex >= MapHeight * MapWidth)
            return;
        if (TileIndex < 0)
            return;
        //if (obj.GetComponent<LivingObject>())
        //{
        //    if (!StartCanMoveCheck(obj as LivingObject, obj.currentTile, tileMap[TileIndex]))
        //        return;
        //}
        if (GetTileAtIndex(TileIndex).canBeOccupied == false)
        {

            if (direction.x > 0)
                direction.x++;
            if (direction.x < 0)
                direction.x--;
            if (direction.y > 0)
                direction.y++;
            if (direction.y < 0)
                direction.y--;
            if (direction.z > 0)
                direction.z++;
            if (direction.z < 0)
                direction.z--;
            MoveGridObject(obj, direction, ignoreDist, ++depth);
            return;

        }
        //   return;
        TileScript temp = tileMap[TileIndex];
        if (obj.GetComponent<LivingObject>())
        {

            LivingObject liveObj = obj.GetComponent<LivingObject>();
            if (IsTileOccupied(temp) == true)
            {
                GridObject gridObject = GetObjectAtTile(temp);
                if (gridObject)
                {
                    if (gridObject.GetComponent<LivingObject>())
                    {
                        if (gridObject.GetComponent<LivingObject>().FACTION != obj.GetComponent<LivingObject>().FACTION)
                        {
                            return;

                        }
                    }
                }//.GetComponent<LivingObject>();

                if (StartCanMoveCheck(liveObj, liveObj.currentTile, temp))
                {
                    if (ignoreDist == false)
                    {
                        obj.transform.position = tileMap[TileIndex].transform.position + new Vector3(0, 0.5f, 0);
                        myCamera.currentTile = tileMap[TileIndex].GetComponent<TileScript>();
                        myCamera.infoObject = GetObjectAtTile(tileMap[TileIndex].GetComponent<TileScript>());
                        return;
                    }
                }
                else
                {
                    if (ignoreDist == false)
                    {
                        return;
                    }
                }
            }
        }
        float tempX = temp.transform.position.x;
        float tempY = temp.transform.position.z;

        float objX = obj.currentTile.transform.position.x;
        float objY = obj.currentTile.transform.position.z;


        xDist = Mathf.Abs(tempX - objX);
        yDist = Mathf.Abs(tempY - objY);

        xDist /= 2;
        yDist /= 2;

        if (xDist + yDist <= obj.MOVE_DIST && ignoreDist == false)
        {
            obj.transform.position = tileMap[TileIndex].transform.position + new Vector3(0, 0.5f, 0.12f);
            myCamera.currentTile = tileMap[TileIndex].GetComponent<TileScript>();
            myCamera.infoObject = GetObjectAtTile(tileMap[TileIndex].GetComponent<TileScript>());
        }
        else if (ignoreDist == true)
        {
            obj.transform.position = tileMap[TileIndex].transform.position + new Vector3(0, 0.5f, 0.12f);
            myCamera.currentTile = tileMap[TileIndex].GetComponent<TileScript>();
            myCamera.infoObject = GetObjectAtTile(tileMap[TileIndex].GetComponent<TileScript>());
        }
        myCamera.UpdateCamera();
        updateConditionals();
    }
    public bool PushGridObject(GridObject obj, Vector3 direction)
    {
        //direction.Normalize();
        if (!obj)
            return false;
        //  Debug.Log("pre dir: " + direction);

        Vector3 curPos = obj.currentTile.transform.position;
        Vector3 newPos = curPos + (direction * 2);
        // Debug.Log("ct: " + curPos);
        //Debug.Log("nt: " + newPos);
        // Debug.Log("post dir: " + direction);
        if (newPos.x < 0)
        {
            return false;
        }
        //if (newPos.x >= MapWidth)
        //{
        //    return false;
        //}
        //if (newPos.z >= MapHeight)
        //{
        //    return false;
        //}
        if (newPos.z < 0)
        {
            return false;
        }
        //if (direction.x > 0 && direction.z > 0)
        //{
        //    //return false;
        //}
        int TileIndex = TwoToOneD((int)newPos.z, MapWidth, (int)newPos.x);
        TileIndex = TileIndex / 2;
        //       Debug.Log("pre result: " + TwoToOneD( (int)newPos.z, MapWidth, (int)newPos.x));
        //     Debug.Log("post result: " + TileIndex);
        //for (int i = 0; i < tileMap.Count; i++)
        //{
        //    if (tileMap[i].transform.position == newPos)
        //    {
        //        TileIndex = i;
        //    }
        //}
        if (TileIndex >= MapHeight * MapWidth)
            return false;
        if (TileIndex < 0)
            return false;
        if (GetTileAtIndex(TileIndex).canBeOccupied == false)
            return false;
        //  Debug.Log("obj:" + obj + " possibleTile :" + TileIndex + " new pos:" + newPos);
        TileScript atile = tileMap[TileIndex].GetComponent<TileScript>();

        if (IsTileOccupied(atile) == true)
        {
            GridObject otherObject = GetObjectAtTile(atile);
            if (otherObject && otherObject != obj)
            {
                // DamageGridObject(obj, 2);
                // DamageGridObject(otherObject, 2);
                //  obj.transform.Translate((otherObject.transform.position - obj.transform.position) * 0.5f);
                // otherObject.transform.Translate((obj.transform.position + otherObject.transform.position) * 0.5f);
                //AtkContainer sideContainer = ScriptableObject.CreateInstance<AtkContainer>();

                //DmgReaction sideReaction = new DmgReaction();

                //sideReaction.damage = 2;
                //sideReaction.reaction = Reaction.none;
                //sideContainer.react = sideReaction;
                //sideContainer.dmgObject = obj;
                //sideContainer.attackingElement = Element.Blunt;
                //CreateEvent(this, sideContainer, "apply reaction event", ApplyReactionEvent, null, 0);
                //DamageGridObject(otherObject,2);
                direction.Normalize();
                direction.x = Mathf.RoundToInt(direction.x);
                direction.z = Mathf.RoundToInt(direction.z);
                if (PushGridObject(otherObject, direction) == false)
                {
                    return false;
                }




            }
            else
            {
                return false;
            }
        }





        obj.transform.position = tileMap[TileIndex].transform.position + new Vector3(0, 0.5f, 0.12f);
        myCamera.currentTile = tileMap[TileIndex];
        myCamera.infoObject = GetObjectAtTile(tileMap[TileIndex]);


        obj.currentTile.isOccupied = false;
        obj.currentTile = tileMap[TileIndex];
        obj.currentTile.isOccupied = true;

        myCamera.UpdateCamera();
        updateConditionals();
        return true;
    }
    public void ComfirmMoveGridObject(GridObject obj, int tileIndex)
    {

        if (movingObj == false)
        {

            if (obj.gameObject != tempObject)
            {
                obj.currentTile.isOccupied = false;
                obj.currentTile = tileMap[tileIndex].GetComponent<TileScript>();
                obj.currentTile.isOccupied = true;
                obj.currentTileIndex = tileIndex;
                if (obj.currentTile.isInShadow)
                {
                    AttemptToGoIntoShadows(obj);
                }

            }
            else
            {
                obj.currentTile = tileMap[tileIndex];
            }
            myCamera.currentTile = tileMap[tileIndex];
            myCamera.selectedTile = myCamera.currentTile;
            myCamera.infoObject = GetObjectAtTile(tileMap[tileIndex].GetComponent<TileScript>());
            myCamera.UpdateCamera();
            updateConditionals();
        }
    }

    public void CheckDoorPrompt()
    {
        TileScript tile = player.current.currentTile;
        if (tile.ROOM >= 0)
        {

            if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
            {
                menuManager.ShowNone();
                newSkillEvent.caller = this;
                newSkillEvent.name = tile.ROOM.ToString() + "," + tile.START.ToString();

                CreateEvent(this, tile, "door event", CheckNewSKillEvent, null, -1, DoorStart);

            }
        }

    }

    public void CheckEventPrompt()
    {
        GridObject griddy = GetAdjecentTilesEvent(player.current);
        if (griddy)
        {

            if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
            {
                menuManager.ShowNone();
                newSkillEvent.caller = this;
                newSkillEvent.data = griddy;
                CreateEvent(this, griddy, "door event", CheckNewSKillEvent, null, -1, EventStart);
            }
        }

    }

    public void CheckHelpPrompt()
    {
        if (player.current)
        {
            TileScript helpTile = player.current.currentTile;
            if (helpTile)
            {

                if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                {
                    menuManager.ShowNone();
                    newSkillEvent.caller = this;
                    newSkillEvent.data = player.current;
                    CreateEvent(this, player.current, "help event", CheckNewSKillEvent, null, -1, HelpStart);
                }
            }
        }

    }
    public void CheckTutorialPrompt(int checkText)
    {

        // if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
        {
            menuManager.ShowNone();
            newSkillEvent.caller = this;
            newSkillEvent.data = player.current;
            SkillEventContainer sec = new SkillEventContainer();
            sec.name = "?;" + Common.GetHelpText(checkText);
            inTutorialMenu = true;
            CreateEvent(this, sec, "help event", CheckIfIntutorialMenu, null, 0, TutorialStart);
        }
    }

    public void CheckTutorialPromptNonInterrupt(int checkText)
    {

        // if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
        {
            menuManager.ShowNone();
            newSkillEvent.caller = this;
            newSkillEvent.data = player.current;
            SkillEventContainer sec = new SkillEventContainer();
            sec.name = "?;" + Common.GetHelpText(checkText);
            inTutorialMenu = true;
            CreateEvent(this, sec, "help event", CheckIfIntutorialMenu, null, -1, TutorialStart);
        }
    }
    public void CheckTutorialPrompt(string newText)
    {

        // if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
        {
            menuManager.ShowNone();
            newSkillEvent.caller = this;
            newSkillEvent.data = player.current;
            SkillEventContainer sec = new SkillEventContainer();
            sec.name = newText;
            inTutorialMenu = true;
            CreateEvent(this, sec, "help event", CheckIfIntutorialMenu, null, 0, TutorialStart);
        }
    }
    public void ComfirmMoveGridObject(GridObject obj, TileScript tile, bool hover = false)
    {
        if (obj.gameObject != tempObject)
        {
            obj.currentTile.isOccupied = false;
            obj.currentTile = tile;
            obj.currentTile.isOccupied = true;
            obj.currentTileIndex = tile.listindex;

        }
        else
        {
            obj.currentTile = tile;
            obj.currentTileIndex = tile.listindex;

        }
        if (hover == false)
        {
            myCamera.selectedTile = tile;
            myCamera.currentTile = tile;
        }
        else
        {
            myCamera.selectedTile = tile;
        }

        myCamera.infoObject = GetObjectAtTile(tile);
        myCamera.UpdateCamera();
        if (tile.ROOM >= 0)
        {
            if (obj.GetComponent<LivingObject>())
            {

                if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                {
                    newSkillEvent.caller = this;
                    newSkillEvent.name = tile.ROOM.ToString() + "," + tile.START.ToString();
                    CreateEvent(this, tile, "door event", CheckNewSKillEvent, null, -1, DoorStart);

                }
            }
        }
        updateConditionals();
    }
    public int GetTileIndex(TileScript checkTile)
    {
        for (int i = 0; i < tileMap.Count; i++)
        {
            if (checkTile == tileMap[i])
                return i;
        }
        return -1;
    }
    public int GetTileIndex(GridObject checkTile)
    {
        if (checkTile == null)
        {
            return -1;
        }
        Vector3 interceptIndex = checkTile.transform.position / 2;
        //int TileIndex = checkTile.currentTileIndex; //TwoToOneD(Mathf.RoundToInt(checkTile.transform.position.z), MapWidth, Mathf.RoundToInt(checkTile.transform.position.x));
        int TileIndex = TwoToOneD(Mathf.RoundToInt(interceptIndex.z), MapWidth, Mathf.RoundToInt(interceptIndex.x));
        //        Debug.Log("resulting index:" + TileIndex);
        if (TileIndex >= MapHeight * MapWidth)
            return -1;
        if (TileIndex < 0)
            return -1;
        return TileIndex;
    }
    public int GetTileIndex(Vector3 checkPosition)
    {
        //        Debug.Log("og: " + checkPosition);

        Vector3 interceptIndex = checkPosition / 2;
        //       Debug.Log("intercept:" + interceptIndex);

        if ((int)interceptIndex.z >= MapHeight)
            return -1;
        if ((int)interceptIndex.x >= MapWidth)
            return -1;
        if ((int)interceptIndex.z < 0)
            return -1;
        if ((int)interceptIndex.x < 0)
            return -1;
        int TileIndex = TwoToOneD((int)interceptIndex.z, MapWidth, (int)interceptIndex.x);

        if (TileIndex >= MapHeight * MapWidth)
            return -1;
        if (TileIndex < 0)
            return -1;
        if (GetTileAtIndex(TileIndex) != null && GetTileAtIndex(TileIndex).canBeOccupied == false)
        { return -1; }
        return TileIndex;
    }
    public TileScript GetTile(GridObject checkTile)
    {
        int index = GetTileIndex(checkTile);
        // index = index / 2;
        //   Debug.Log("new index:" + index);
        if (index < 0)
            return null;
        if (index > MapHeight * MapWidth)
            return null;
        return tileMap[index];
    }
    public TileScript GetTile(Vector3 checkPosition)
    {
        int index = GetTileIndex(checkPosition);
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
        if (tileMap.Count < checkIndex)
            return null;
        return tileMap[checkIndex].GetComponent<TileScript>();
    }
    public GridObject GetObjectAtTile(TileScript checkTile)
    {
        GridObject returnedObject = null;
        if (checkTile == null)
            return null;
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
    public List<TileScript> GetAdjecentTiles(GridObject origin)
    {
        List<TileScript> tiles = new List<TileScript>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 2;

        v2.x += 2;

        v3.z -= 2;

        v4.x -= 2;
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
        Vector3 originindex = origin.transform.position;
        Vector3 possIndexLeft = origin.transform.position + new Vector3(0, 0, -2.0f);
        Vector3 possIndexRight = origin.transform.position + new Vector3(0, 0, 2.0f);
        Vector3 possIndexUp = origin.transform.position + new Vector3(2.0f, 0, 0);
        Vector3 possIndexDown = origin.transform.position + new Vector3(-2.0f, 0, 0);


        List<Vector3> possibleTiles = new List<Vector3>();
        possibleTiles.Add(possIndexLeft);
        possibleTiles.Add(possIndexRight);
        possibleTiles.Add(possIndexUp);
        possibleTiles.Add(possIndexDown);
        //List<Vector3> possiblePossitions = new List<Vector3>();
        //Vector3 v1 = origin.transform.position;
        //Vector3 v2 = origin.transform.position;
        //Vector3 v3 = origin.transform.position;
        //Vector3 v4 = origin.transform.position;
        //v1.z += 1;

        //v2.x += 1;

        //v3.z -= 1;

        //v4.x -= 1;
        //possiblePossitions.Add(v1);
        //possiblePossitions.Add(v2);
        //possiblePossitions.Add(v3);
        //possiblePossitions.Add(v4);

        for (int i = 0; i < possibleTiles.Count; i++)
        {
            int index = GetTileIndex(possibleTiles[i]);
            if (index >= 0)
            {

                TileScript newTile = tileMap[index];
                if (newTile.isActiveAndEnabled)
                {
                    tiles.Add(newTile);
                }
            }
        }


        return tiles;
    }

    public bool CheckAdjecentTilesEvent(LivingObject origin)
    {


        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 2;

        v2.x += 2;

        v3.z -= 2;

        v4.x -= 2;
        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = GetTileAtIndex(index);
                if (newTile.isOccupied)
                {
                    if (!GetObjectAtTile(newTile))
                    {
                        newTile.isOccupied = false;
                    }
                    else
                    {

                        if (GetObjectAtTile(newTile).FACTION == Faction.eventObj)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public bool CheckAdjecentTilesInteractable(LivingObject origin)
    {
        if (!isSetup || origin == null)
        {
            return false;
        }
        bool checkBool = false;

        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 2;

        v2.x += 2;

        v3.z -= 2;

        v4.x -= 2;
        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = GetTileAtIndex(index);
                if (newTile.isOccupied)
                {
                    if (!GetObjectAtTile(newTile))
                    {
                        newTile.isOccupied = false;
                    }
                    else
                    {

                        if (GetObjectAtTile(newTile).FACTION == Faction.interactable)
                        {
                            if (checkBool == false)
                            {
                                adjacentInteractable = GetObjectAtTile(newTile) as InteractableObject;
                                return true;
                            }
                            else
                            {
                                adjacentInteractable = null;

                            }
                        }
                    }
                }
            }
        }

        return checkBool;
    }
    public bool CheckAdjecentTilesGlyphs(LivingObject origin)
    {
        if (!isSetup || origin == null)
        {
            return false;
        }
        bool checkBool = false;

        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 2;

        v2.x += 2;

        v3.z -= 2;

        v4.x -= 2;
        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = GetTileAtIndex(index);
                if (newTile.isOccupied)
                {
                    if (!GetObjectAtTile(newTile))
                    {
                        newTile.isOccupied = false;
                    }
                    else
                    {

                        if (GetObjectAtTile(newTile).FACTION == Faction.hazard)
                        {
                            if (checkBool == false)
                            {
                                adjacentGlyph = GetObjectAtTile(newTile) as HazardScript;
                                return true;
                            }
                            else
                            {
                                adjacentGlyph = null;

                            }
                        }
                    }
                }
            }
        }

        return checkBool;
    }

    public bool CheckAdjecentTilesEnemy(LivingObject origin)
    {
        bool checkBool = false;

        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 2;

        v2.x += 2;

        v3.z -= 2;

        v4.x -= 2;
        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = GetTileAtIndex(index);
                if (newTile.isOccupied)
                {
                    if (!GetObjectAtTile(newTile))
                    {
                        newTile.isOccupied = false;
                    }
                    else
                    {

                        if (GetObjectAtTile(newTile).FACTION == Faction.enemy)
                        {
                            if (checkBool == false)
                            {

                                return true;
                            }
                            else
                            {
                                adjacentGlyph = null;

                            }
                        }
                    }
                }
            }
        }

        return checkBool;
    }

    public EnemyScript GetAdjecentEnemy(LivingObject origin)
    {
        bool checkBool = false;

        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 2;

        v2.x += 2;

        v3.z -= 2;

        v4.x -= 2;
        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = GetTileAtIndex(index);
                if (newTile.isOccupied)
                {
                    if (!GetObjectAtTile(newTile))
                    {
                        newTile.isOccupied = false;
                    }
                    else
                    {

                        if (GetObjectAtTile(newTile).FACTION == Faction.enemy)
                        {
                            if (checkBool == false)
                            {
                                EnemyScript adjacent = GetObjectAtTile(newTile) as EnemyScript;
                                return adjacent;
                            }
                            else
                            {


                            }
                        }
                    }
                }
            }
        }

        return null;
    }

    public GridObject GetAdjecentTilesEvent(LivingObject origin)
    {


        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 2;

        v2.x += 2;

        v3.z -= 1;

        v4.x -= 1;
        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = GetTileAtIndex(index);
                if (newTile.isOccupied)
                {
                    if (GetObjectAtTile(newTile).FACTION == Faction.eventObj)
                    {
                        return GetObjectAtTile(newTile);
                    }
                }
            }
        }

        return null;
    }
    public List<TileScript> GetDoubleAdjecentTiles(LivingObject origin)
    {
        List<TileScript> tiles = new List<TileScript>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        int realindex = GetTileIndex(origin.transform.position);
        int index = -1;
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

        v1.z += 2;
        v2.x += 2;
        v3.z -= 2;
        v4.x -= 2;

        v5.x += 4;
        v6.z += 4;
        v7.z -= 4;
        v8.x -= 4;

        v9.x -= 2;
        v9.z += 2;

        v10.x += 2;
        v10.z += 2;

        v11.x -= 2;
        v11.z -= 2;

        v12.x += 2;
        v12.z -= 2;

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
            index = GetTileIndex(possiblePossitions[i]);
            if (index > 0)
            {
                TileScript newTile = GetTileAtIndex(index);
                tiles.Add(newTile);
            }
        }
        return tiles;
    }

    public (List<TileScript>, List<CommandSkill>) GetOppViaDoubleAdjecentTiles(LivingObject origin, Element trigger, GridObject dmgObject)
    {
        //  Debug.Log("checking for adj");
        // Debug.Log("Trigger Element");
        List<TileScript> tiles = new List<TileScript>();
        List<CommandSkill> cmds = new List<CommandSkill>();
        if (origin.ACTIONS == 0)
        {
            return (tiles, cmds);
        }



        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        //Vector3 v5 = origin.transform.position;
        //Vector3 v6 = origin.transform.position;
        //Vector3 v7 = origin.transform.position;
        //Vector3 v8 = origin.transform.position;

        //Vector3 v9 = origin.transform.position;
        //Vector3 v10 = origin.transform.position;
        //Vector3 v11 = origin.transform.position;
        //Vector3 v12 = origin.transform.position;

        v1.z += 2;
        v2.x += 2;
        v3.z -= 2;
        v4.x -= 2;

        //v5.x += 2;
        //v6.z += 2;
        //v7.z -= 2;
        //v8.x -= 2;

        //v9.x -= 1;
        //v9.z += 1;

        //v10.x += 1;
        //v10.z += 1;

        //v11.x -= 1;
        //v11.z -= 1;

        //v12.x += 1;
        //v12.z -= 1;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        //possiblePossitions.Add(v5);
        //possiblePossitions.Add(v6);
        //possiblePossitions.Add(v7);
        //possiblePossitions.Add(v8);
        //possiblePossitions.Add(v9);
        //possiblePossitions.Add(v10);
        //possiblePossitions.Add(v11);
        //possiblePossitions.Add(v12);

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

                                        if (liveObj.INVENTORY.CSKILLS.Count > 0)
                                        {
                                            for (int k = 0; k < liveObj.INVENTORY.CSKILLS.Count; k++)
                                            {
                                                CommandSkill potentSkill = liveObj.INVENTORY.CSKILLS[k];
                                                if (potentSkill.OPPORTUNITY == (trigger))
                                                {

                                                    if (dmgObject)
                                                    {
                                                        if (dmgObject.GetComponent<LivingObject>())
                                                        {
                                                            LivingObject liveDmgObject = dmgObject.GetComponent<LivingObject>();
                                                            if (liveDmgObject.ARMOR != null && liveDmgObject.ARMOR.HITLIST.Count == 8)
                                                            {
                                                                // if (liveDmgObject.ARMOR.HITLIST[(int)(potentSkill.ELEMENT)] >= EHitType.resists)
                                                                {
                                                                    // return tiles;
                                                                }
                                                                // else
                                                                {
                                                                    tiles.Add(newTile);
                                                                    cmds.Add(potentSkill);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Debug.Log("might wanna double check ol " + liveDmgObject.NAME);
                                                            }
                                                        }
                                                    }
                                                    //  liveObj.OPP_SLOTS.SKILLS.Add(liveObj.INVENTORY.OPPS[k]);


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
        return (tiles, cmds);
    }
    public List<TileScript> GetDartAttackableTilesOneList(TileScript origin)
    {
        return tileManager.GetPinWheelTiles(origin);

    }

    public List<TileScript> GetSkillAttackableTilesOneList(TileScript origin, CommandSkill skill)
    {
        if (origin == null)
        {
            Debug.Log("um.. freindino i hate to tell u but....");
            tileManager.EDITABLE_TILES.Clear();
            return tileManager.EDITABLE_TILES;
        }
        //TileScript origin = obj.currentTile;
        switch (skill.RTYPE)
        {
            case RangeType.adjacent:
                return tileManager.GetAdjecentTiles(origin);
                break;
            case RangeType.rect:
                return tileManager.GetRectTiles(origin);
                break;
            case RangeType.pinWheel:
                return tileManager.GetPinWheelTiles(origin);
                break;
            case RangeType.detached:
                return tileManager.GetDetachedTiles(origin);
                break;
            case RangeType.stretched:
                return tileManager.GetStretchedTiles(origin);
                break;
            case RangeType.rotator:
                return tileManager.GetRotatorTiles(origin);
                break;
            case RangeType.fan:
                return tileManager.GetFanTiles(origin);
                break;
            case RangeType.spear:
                return tileManager.GetSpearTiles(origin);
                break;
            case RangeType.lance:
                return tileManager.GetLanceTiles(origin);
                break;
            case RangeType.line:
                return tileManager.GetLineTiles(origin);
                break;
            case RangeType.cone:
                return tileManager.GetConeTiles(origin);
                break;
            case RangeType.tpose:
                return tileManager.GetTPoseTiles(origin);
                break;
            case RangeType.clover:
                return tileManager.GetCloverTiles(origin);
                break;
            case RangeType.cross:
                return tileManager.GetCrossTiles(origin);
                break;
            case RangeType.square:
                return tileManager.GetSquareTiles(origin);
                break;
            case RangeType.box:
                return tileManager.GetBoxTiles(origin);
                break;
            case RangeType.diamond:
                return tileManager.GetDiamondTiles(origin);
                break;
            case RangeType.crosshair:
                return tileManager.GetCrosshairTiles(origin);
                break;
            case RangeType.any:
                {
                    List<TileScript> mytiles = tileManager.GetRotatorTiles(origin);

                    mytiles.Add(origin);
                    return mytiles;

                }
                break;
        }
        return null;
    }
    public List<TileScript> GetWeaponAttackableTilesOneList(TileScript origin, WeaponScript skill)
    {
        // TileScript origin = obj.currentTile;
        if (origin == null)
        {
            Debug.Log("um.. fam i hate to tell u but....");
            tileManager.EDITABLE_TILES.Clear();
            return tileManager.EDITABLE_TILES;
        }
        switch (skill.ATKRANGE)
        {
            case RangeType.adjacent:
                return tileManager.GetAdjecentTiles(origin);
                break;
            case RangeType.rect:
                return tileManager.GetRectTiles(origin);
                break;
            case RangeType.pinWheel:
                return tileManager.GetPinWheelTiles(origin);
                break;
            case RangeType.detached:
                return tileManager.GetDetachedTiles(origin);
                break;
            case RangeType.stretched:
                return tileManager.GetStretchedTiles(origin);
                break;
            case RangeType.rotator:
                return tileManager.GetRotatorTiles(origin);
                break;
            case RangeType.fan:
                return tileManager.GetFanTiles(origin);
                break;
            case RangeType.spear:
                return tileManager.GetSpearTiles(origin);
                break;
            case RangeType.lance:
                return tileManager.GetLanceTiles(origin);
                break;
            case RangeType.line:
                return tileManager.GetLineTiles(origin);
                break;
            case RangeType.cone:
                return tileManager.GetConeTiles(origin);
                break;
            case RangeType.tpose:
                return tileManager.GetTPoseTiles(origin);
                break;
            case RangeType.clover:
                return tileManager.GetCloverTiles(origin);
                break;
            case RangeType.cross:
                return tileManager.GetCrossTiles(origin);
                break;
            case RangeType.square:
                return tileManager.GetSquareTiles(origin);
                break;
            case RangeType.box:
                return tileManager.GetBoxTiles(origin);
                break;
            case RangeType.diamond:
                return tileManager.GetDiamondTiles(origin);
                break;
            case RangeType.crosshair:
                return tileManager.GetCrosshairTiles(origin);
                break;
            case RangeType.any:
                {
                    List<TileScript> mytiles = tileManager.GetAdjecentTiles(origin);

                    mytiles.Add(origin);
                    return mytiles;

                }
                break;
        }
        return null;
    }
    public List<List<TileScript>> GetSkillsAttackableTiles(GridObject obj, CommandSkill skill)
    {
        if (obj == null)
            return null;
        if (skill == null)
            return null;
        int checkIndex = GetTileIndex(obj);
        if (checkIndex == -1)
            return null;

        TileScript origin = obj.currentTile;
        List<List<TileScript>> returnList = new List<List<TileScript>>();
        //List<Vector2> affectedTiles = skill.TILES;
        Vector2 checkDist = Vector2.zero;
        float dist = 0;
        //if (affectedTiles != null)
        //{
        //    for (int j = 0; j < affectedTiles.Count; j++)
        //    {
        //        if (dist < affectedTiles[j].x)
        //        {
        //            dist = affectedTiles[j].x;
        //        }
        //        if (dist < affectedTiles[j].y)
        //        {
        //            dist = affectedTiles[j].y;
        //        }
        //    }
        //}
        switch (skill.RTYPE)
        {

            case RangeType.single:
                //{
                //for (int i = 0; i < 4; i++)
                //{
                //    List<TileScript> tiles = new List<TileScript>();
                //    if (affectedTiles != null)
                //    {
                //        for (int j = 0; j < affectedTiles.Count; j++)
                //        {
                //            Vector2 Dist = affectedTiles[j];
                //            switch (i)
                //            {
                //                case 0:
                //                    checkDist.x = Dist.x;
                //                    checkDist.y = Dist.y;
                //                    break;

                //                case 1:
                //                    checkDist.x = Dist.y;
                //                    checkDist.y = Dist.x * -1;
                //                    break;

                //                case 2:

                //                    checkDist.x = Dist.x * -1;
                //                    checkDist.y = Dist.y * -1;

                //                    break;

                //                case 3:
                //                    checkDist.x = Dist.y * -1; //Yes x = y
                //                    checkDist.y = Dist.x;
                //                    break;
                //            }


                //            Vector3 checkPos = obj.transform.position;
                //            checkPos.x += checkDist.x;
                //            checkPos.z += checkDist.y;
                //            int testIndex = GetTileIndex(checkPos);
                //            if (testIndex >= 0)
                //            {
                //                TileScript t = GetTileAtIndex(testIndex);
                //                float checkX = Mathf.Abs(t.transform.position.x - checkPos.x);
                //                float checkY = Mathf.Abs(t.transform.position.z - checkPos.z);
                //                if (checkX + checkY <= dist)
                //                {
                //                    TileScript realTile = GetTileAtIndex(testIndex);
                //                    if (!tiles.Contains(realTile))
                //                        tiles.Add(realTile);
                //                }
                //            }
                //        }
                //    }
                //    if (tiles.Count > 0)
                //        returnList.Add(tiles);

                //}

                //}
                break;
            case RangeType.multi:
                //{

                //for (int i = 0; i < 4; i++)
                //{
                //    if (affectedTiles != null)
                //    {
                //        for (int j = 0; j < affectedTiles.Count; j++)
                //        {
                //            List<TileScript> tiles = new List<TileScript>();
                //            Vector2 Dist = affectedTiles[j];
                //            switch (i)
                //            {
                //                case 0:
                //                    checkDist.x = Dist.x;
                //                    checkDist.y = Dist.y;
                //                    break;

                //                case 1:
                //                    checkDist.x = Dist.y;
                //                    checkDist.y = Dist.x * -1;
                //                    break;

                //                case 2:

                //                    checkDist.x = Dist.x * -1;
                //                    checkDist.y = Dist.y * -1;

                //                    break;

                //                case 3:
                //                    checkDist.x = Dist.y * -1; //Yes x = y
                //                    checkDist.y = Dist.x;
                //                    break;
                //            }


                //            Vector3 checkPos = obj.transform.position;
                //            checkPos.x += checkDist.x;
                //            checkPos.z += checkDist.y;
                //            int testIndex = GetTileIndex(checkPos);
                //            if (testIndex >= 0)
                //            {
                //                TileScript t = GetTileAtIndex(testIndex);
                //                float checkX = Mathf.Abs(t.transform.position.x - checkPos.x);
                //                float checkY = Mathf.Abs(t.transform.position.z - checkPos.z);
                //                if (checkX + checkY <= dist)
                //                {
                //                    TileScript realTile = GetTileAtIndex(testIndex);
                //                    if (!tiles.Contains(realTile))
                //                        tiles.Add(realTile);
                //                }
                //            }
                //            if (tiles.Count > 0)
                //                returnList.Add(tiles);
                //        }
                //    }


                //}
                //}
                break;
            case RangeType.area:
                {
                    //List<TileScript> tiles = new List<TileScript>();
                    //for (int i = 0; i < 4; i++)
                    //{
                    //    if (affectedTiles != null)
                    //    {
                    //        for (int j = 0; j < affectedTiles.Count; j++)
                    //        {
                    //            Vector2 Dist = affectedTiles[j];
                    //            switch (i)
                    //            {
                    //                case 0:
                    //                    checkDist.x = Dist.x;
                    //                    checkDist.y = Dist.y;
                    //                    break;

                    //                case 1:
                    //                    checkDist.x = Dist.y;
                    //                    checkDist.y = Dist.x * -1;
                    //                    break;

                    //                case 2:

                    //                    checkDist.x = Dist.x * -1;
                    //                    checkDist.y = Dist.y * -1;

                    //                    break;

                    //                case 3:
                    //                    checkDist.x = Dist.y * -1; //Yes x = y
                    //                    checkDist.y = Dist.x;
                    //                    break;
                    //            }


                    //            Vector3 checkPos = obj.transform.position;
                    //            checkPos.x += checkDist.x;
                    //            checkPos.z += checkDist.y;
                    //            int testIndex = GetTileIndex(checkPos);
                    //            if (testIndex >= 0)
                    //            {
                    //                TileScript t = GetTileAtIndex(testIndex);
                    //                float checkX = Mathf.Abs(t.transform.position.x - checkPos.x);
                    //                float checkY = Mathf.Abs(t.transform.position.z - checkPos.z);
                    //                if (checkX + checkY <= dist)
                    //                {
                    //                    TileScript realTile = GetTileAtIndex(testIndex);
                    //                    if (!tiles.Contains(realTile))
                    //                        tiles.Add(realTile);
                    //                }
                    //            }
                    //        }
                    //    }

                    //}
                    //if (tiles.Count > 0)
                    //    returnList.Add(tiles);
                }
                break;
            case RangeType.any:
                {
                    returnList = tileManager.GetAdjecentTilesList(origin);
                    List<TileScript> mytile = new List<TileScript>();
                    mytile.Add(obj.currentTile);
                    returnList.Add(mytile);

                }
                break;
            case RangeType.anyarea:
                {


                    //List<TileScript> tiles = new List<TileScript>();
                    //for (int i = 0; i < 4; i++)
                    //{
                    //    if (affectedTiles != null)
                    //    {
                    //        for (int j = 0; j < affectedTiles.Count; j++)
                    //        {
                    //            Vector2 Dist = affectedTiles[j];
                    //            switch (i)
                    //            {
                    //                case 0:
                    //                    checkDist.x = Dist.x;
                    //                    checkDist.y = Dist.y;
                    //                    break;

                    //                case 1:
                    //                    checkDist.x = Dist.y;
                    //                    checkDist.y = Dist.x * -1;
                    //                    break;

                    //                case 2:

                    //                    checkDist.x = Dist.x * -1;
                    //                    checkDist.y = Dist.y * -1;

                    //                    break;

                    //                case 3:
                    //                    checkDist.x = Dist.y * -1; //Yes x = y
                    //                    checkDist.y = Dist.x;
                    //                    break;
                    //            }


                    //            Vector3 checkPos = obj.transform.position;
                    //            checkPos.x += checkDist.x;
                    //            checkPos.z += checkDist.y;
                    //            int testIndex = GetTileIndex(checkPos);
                    //            if (testIndex >= 0)
                    //            {
                    //                TileScript t = GetTileAtIndex(testIndex);
                    //                float checkX = Mathf.Abs(t.transform.position.x - checkPos.x);
                    //                float checkY = Mathf.Abs(t.transform.position.z - checkPos.z);
                    //                if (checkX + checkY <= dist)
                    //                {
                    //                    TileScript realTile = GetTileAtIndex(testIndex);
                    //                    if (!tiles.Contains(realTile))
                    //                        tiles.Add(realTile);
                    //                }
                    //            }
                    //        }
                    //    }

                    //}
                    //// List<TileScript> mytile = new List<TileScript>();

                    //tiles.Add(GetTileAtIndex(checkIndex));

                    //returnList.Add(tiles);
                }
                break;
            case RangeType.multiarea:
                //for (int i = 0; i < 4; i++)
                //{
                //    if (affectedTiles != null)
                //    {
                //        for (int j = 0; j < affectedTiles.Count; j++)
                //        {
                //            List<TileScript> tiles = new List<TileScript>();
                //            Vector2 Dist = affectedTiles[j];
                //            switch (i)
                //            {
                //                case 0:
                //                    checkDist.x = Dist.x;
                //                    checkDist.y = Dist.y;
                //                    break;

                //                case 1:
                //                    checkDist.x = Dist.y;
                //                    checkDist.y = Dist.x * -1;
                //                    break;

                //                case 2:

                //                    checkDist.x = Dist.x * -1;
                //                    checkDist.y = Dist.y * -1;

                //                    break;

                //                case 3:
                //                    checkDist.x = Dist.y * -1; //Yes x = y
                //                    checkDist.y = Dist.x;
                //                    break;
                //            }


                //            Vector3 checkPos = obj.transform.position;
                //            checkPos.x += checkDist.x;
                //            checkPos.z += checkDist.y;
                //            int testIndex = GetTileIndex(checkPos);
                //            if (testIndex >= 0)
                //            {
                //                TileScript t = GetTileAtIndex(testIndex);
                //                float checkX = Mathf.Abs(t.transform.position.x - checkPos.x);
                //                float checkY = Mathf.Abs(t.transform.position.z - checkPos.z);
                //                if (checkX + checkY <= dist)
                //                {
                //                    TileScript realTile = GetTileAtIndex(testIndex);
                //                    if (!tiles.Contains(realTile))
                //                        tiles.Add(realTile);
                //                }
                //            }
                //            if (tiles.Count > 0)
                //                returnList.Add(tiles);
                //        }
                //    }


                //}
                break;
            case RangeType.adjacent:
                returnList = tileManager.GetAdjecentTilesList(origin);
                break;
            case RangeType.rect:
                returnList = tileManager.GetRectTilesList(origin);
                break;
            case RangeType.pinWheel:
                returnList = tileManager.GetPinWheelTilesList(origin);
                break;
            case RangeType.detached:
                returnList = tileManager.GetDetachedTilesList(origin);
                break;
            case RangeType.stretched:
                returnList = tileManager.GetStretchedTilesList(origin);
                break;
            case RangeType.rotator:
                returnList = tileManager.GetRotatorTilesList(origin);
                break;
            case RangeType.fan:
                returnList = tileManager.GetFanTilesList(origin);
                break;
            case RangeType.spear:
                returnList = tileManager.GetSpearTilesList(origin);
                break;
            case RangeType.lance:
                returnList = tileManager.GetLanceTilesList(origin);
                break;
            case RangeType.line:
                returnList = tileManager.GetLineTilesList(origin);
                break;
            case RangeType.cone:
                returnList = tileManager.GetConeTilesList(origin);
                break;
            case RangeType.tpose:
                returnList = tileManager.GetTPoseTilesList(origin);
                break;
            case RangeType.clover:
                returnList = tileManager.GetCloverTilesList(origin);
                break;
            case RangeType.cross:
                returnList = tileManager.GetCrossTilesList(origin);
                break;
            case RangeType.square:
                returnList = tileManager.GetSquareTilesList(origin);
                break;
            case RangeType.box:
                returnList = tileManager.GetBoxTilesList(origin);
                break;
            case RangeType.diamond:
                returnList = tileManager.GetDiamondTilesList(origin);
                break;
            case RangeType.crosshair:
                returnList = tileManager.GetCrosshairTilesList(origin);
                break;
            default:
                break;
        }

        return returnList;
    }

    public List<List<TileScript>> GetAttackableTiles(GridObject obj, CommandSkill skill)
    {
        if (obj == null)
            return null;
        if (skill == null)
            return null;
        int checkIndex = GetTileIndex(obj);
        if (checkIndex == -1)
            return null;

        TileScript origin = obj.currentTile;
        List<List<TileScript>> returnList = new List<List<TileScript>>();

        switch (skill.RTYPE)
        {
            case RangeType.adjacent:
                returnList = tileManager.GetAdjecentTilesList(origin);
                break;
            case RangeType.rect:
                returnList = tileManager.GetRectTilesList(origin);
                break;
            case RangeType.pinWheel:
                returnList = tileManager.GetPinWheelTilesList(origin);
                break;
            case RangeType.detached:
                returnList = tileManager.GetDetachedTilesList(origin);
                break;
            case RangeType.stretched:
                returnList = tileManager.GetStretchedTilesList(origin);
                break;
            case RangeType.rotator:
                returnList = tileManager.GetRotatorTilesList(origin);
                break;
            case RangeType.fan:
                returnList = tileManager.GetFanTilesList(origin);
                break;
            case RangeType.spear:
                returnList = tileManager.GetSpearTilesList(origin);
                break;
            case RangeType.lance:
                returnList = tileManager.GetLanceTilesList(origin);
                break;
            case RangeType.line:
                returnList = tileManager.GetLineTilesList(origin);
                break;
            case RangeType.cone:
                returnList = tileManager.GetConeTilesList(origin);
                break;
            case RangeType.tpose:
                returnList = tileManager.GetTPoseTilesList(origin);
                break;
            case RangeType.clover:
                returnList = tileManager.GetCloverTilesList(origin);
                break;
            case RangeType.cross:
                returnList = tileManager.GetCrossTilesList(origin);
                break;
            case RangeType.square:
                returnList = tileManager.GetSquareTilesList(origin);
                break;
            case RangeType.box:
                returnList = tileManager.GetBoxTilesList(origin);
                break;
            case RangeType.diamond:
                returnList = tileManager.GetDiamondTilesList(origin);
                break;
            case RangeType.crosshair:
                returnList = tileManager.GetCrosshairTilesList(origin);
                break;
            default:
                break;
        }

        return returnList;
    }
    public List<List<TileScript>> GetAttackableTiles(GridObject obj, WeaponScript skill)
    {
        if (obj == null)
            return null;
        if (skill == null)
            return null;
        int checkIndex = GetTileIndex(obj);
        if (checkIndex == -1)
            return null;

        TileScript origin = obj.currentTile;
        List<List<TileScript>> returnList = new List<List<TileScript>>();

        switch (skill.ATKRANGE)
        {
            case RangeType.adjacent:
                returnList = tileManager.GetAdjecentTilesList(origin);
                break;
            case RangeType.rect:
                returnList = tileManager.GetRectTilesList(origin);
                break;
            case RangeType.pinWheel:
                returnList = tileManager.GetPinWheelTilesList(origin);
                break;
            case RangeType.detached:
                returnList = tileManager.GetDetachedTilesList(origin);
                break;
            case RangeType.stretched:
                returnList = tileManager.GetStretchedTilesList(origin);
                break;
            case RangeType.rotator:
                returnList = tileManager.GetRotatorTilesList(origin);
                break;
            case RangeType.fan:
                returnList = tileManager.GetFanTilesList(origin);
                break;
            case RangeType.spear:
                returnList = tileManager.GetSpearTilesList(origin);
                break;
            case RangeType.lance:
                returnList = tileManager.GetLanceTilesList(origin);
                break;
            case RangeType.line:
                returnList = tileManager.GetLineTilesList(origin);
                break;
            case RangeType.cone:
                returnList = tileManager.GetConeTilesList(origin);
                break;
            case RangeType.tpose:
                returnList = tileManager.GetTPoseTilesList(origin);
                break;
            case RangeType.clover:
                returnList = tileManager.GetCloverTilesList(origin);
                break;
            case RangeType.cross:
                returnList = tileManager.GetCrossTilesList(origin);
                break;
            case RangeType.square:
                returnList = tileManager.GetSquareTilesList(origin);
                break;
            case RangeType.box:
                returnList = tileManager.GetBoxTilesList(origin);
                break;
            case RangeType.diamond:
                returnList = tileManager.GetDiamondTilesList(origin);
                break;
            case RangeType.crosshair:
                returnList = tileManager.GetCrosshairTilesList(origin);
                break;
            default:
                break;
        }

        return returnList;
    }
    public List<List<TileScript>> GetAttackableTiles(GridObject obj, ItemScript skill)
    {
        if (obj == null)
            return null;
        if (skill == null)
            return null;
        int checkIndex = GetTileIndex(obj);
        if (checkIndex == -1)
            return null;

        TileScript origin = obj.currentTile;
        List<List<TileScript>> returnList = new List<List<TileScript>>();

        switch (skill.RTYPE)
        {
            case RangeType.adjacent:
                returnList = tileManager.GetAdjecentTilesList(origin);
                break;
            case RangeType.rect:
                returnList = tileManager.GetRectTilesList(origin);
                break;
            case RangeType.pinWheel:
                returnList = tileManager.GetPinWheelTilesList(origin);
                break;
            case RangeType.detached:
                returnList = tileManager.GetDetachedTilesList(origin);
                break;
            case RangeType.stretched:
                returnList = tileManager.GetStretchedTilesList(origin);
                break;
            case RangeType.rotator:
                returnList = tileManager.GetRotatorTilesList(origin);
                break;
            case RangeType.fan:
                returnList = tileManager.GetFanTilesList(origin);
                break;
            case RangeType.spear:
                returnList = tileManager.GetSpearTilesList(origin);
                break;
            case RangeType.lance:
                returnList = tileManager.GetLanceTilesList(origin);
                break;
            case RangeType.line:
                returnList = tileManager.GetLineTilesList(origin);
                break;
            case RangeType.cone:
                returnList = tileManager.GetConeTilesList(origin);
                break;
            case RangeType.tpose:
                returnList = tileManager.GetTPoseTilesList(origin);
                break;
            case RangeType.clover:
                returnList = tileManager.GetCloverTilesList(origin);
                break;
            case RangeType.cross:
                returnList = tileManager.GetCrossTilesList(origin);
                break;
            case RangeType.square:
                returnList = tileManager.GetSquareTilesList(origin);
                break;
            case RangeType.box:
                returnList = tileManager.GetBoxTilesList(origin);
                break;
            case RangeType.diamond:
                returnList = tileManager.GetDiamondTilesList(origin);
                break;
            case RangeType.crosshair:
                returnList = tileManager.GetCrosshairTilesList(origin);
                break;
            default:
                break;
        }

        return returnList;
    }
    public List<List<TileScript>> GetItemUseableTiles(GridObject obj, ItemScript item)
    {
        if (obj == null)
            return null;
        if (item == null)
            return null;
        int checkIndex = GetTileIndex(obj);
        if (checkIndex == -1)
            return null;

        if (item.ITYPE == ItemType.dmg || item.ITYPE == ItemType.dart)
        {
            return GetAttackableTiles(obj, item);
        }

        List<List<TileScript>> returnList = GetAttackableTiles(obj, item);
        List<TileScript> mytile = new List<TileScript>();
        mytile.Add(obj.currentTile);
        returnList.Add(mytile);
        return returnList;
        List<Vector2> affectedTiles = new List<Vector2>();
        affectedTiles.Add(Vector2.up);//skill.TILES;
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
        switch (item.TTYPE)
        {
            case TargetType.ally:
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

                }
                break;
            case TargetType.enemy:
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
            case TargetType.adjecent:
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
            case TargetType.range:
                {
                    affectedTiles.Add(new Vector2(1, 1));
                    affectedTiles.Add(new Vector2(0, 2));
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


            default:
                break;
        }

        return returnList;
    }


    public void ShowSkillAttackbleTiles(LivingObject obj, CommandSkill skill, GridObject target = null, bool ignoreOtherTiles = false)
    {
        for (int k = 0; k < liveEnemies.Count; k++)
        {
            liveEnemies[k].ShowHideWeakness(EHitType.normal, false);

        }
        ShowWhite();

        List<List<TileScript>> tempTiles = GetSkillsAttackableTiles(obj, skill);
        if (tempTiles == null)
            return;

        if (tempTiles.Count > 0)
        {
            for (int i = 0; i < tempTiles.Count; i++) //list of lists
            {
                for (int j = 0; j < tempTiles[i].Count; j++) //indivisual list
                {

                    if (skill.SUBTYPE == SubSkillType.Buff || skill.ELEMENT == Element.Support)
                    {
                        if (target != null)
                        {
                            if (tempTiles[i][j] == target.currentTile)
                            {
                                tempTiles[i][j].MYCOLOR = Common.green;
                            }
                            else if (tempTiles[i][j].MYCOLOR != Common.green && ignoreOtherTiles == false)
                            {
                                tempTiles[i][j].MYCOLOR = Common.lime;
                            }
                            else if (tempTiles[i][j].MYCOLOR != Common.green && ignoreOtherTiles == true)
                            {
                                tempTiles[i][j].MYCOLOR = Color.white;
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                tempTiles[i][j].MYCOLOR = Common.green;
                            }
                            else if (tempTiles[i][j].MYCOLOR != Common.green)
                            {
                                tempTiles[i][j].MYCOLOR = Common.lime;
                            }
                        }



                    }
                    else
                    {
                        if (target != null)
                        {
                            if (tempTiles[i][j] == target.currentTile)
                            {
                                tempTiles[i][j].MYCOLOR = Common.red;
                            }
                            else if (tempTiles[i][j].MYCOLOR != Common.red && ignoreOtherTiles == true)
                            {
                                tempTiles[i][j].MYCOLOR = Common.pink;
                            }
                            else if (tempTiles[i][j].MYCOLOR != Common.red && ignoreOtherTiles == true)
                            {
                                tempTiles[i][j].MYCOLOR = Color.white;
                            }
                            if (GetState() != State.EnemyTurn)
                            {
                                for (int k = 0; k < liveEnemies.Count; k++)
                                {
                                    if (tempTiles[i][j] == liveEnemies[k].currentTile)
                                    {

                                        EHitType hitType = liveEnemies[k].ARMOR.HITLIST[(int)skill.ELEMENT];
                                        liveEnemies[k].ShowHideWeakness(hitType);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                tempTiles[i][j].MYCOLOR = Common.red;
                            }
                            else if (tempTiles[i][j].MYCOLOR != Common.red)
                            {
                                tempTiles[i][j].MYCOLOR = Common.pink;
                            }

                            if (GetState() != State.EnemyTurn)
                            {
                                for (int k = 0; k < liveEnemies.Count; k++)
                                {
                                    if ((tempTiles[i][j] == liveEnemies[k].currentTile && target == null) || (tempTiles[i][j] == liveEnemies[k].currentTile && target.currentTile == liveEnemies[k].currentTile))
                                    {
                                        if (skill.ELEMENT < Element.Buff)
                                        {
                                            EHitType hitType = liveEnemies[k].ARMOR.HITLIST[(int)skill.ELEMENT];
                                            liveEnemies[k].ShowHideWeakness(hitType);
                                        }
                                    }
                                }
                            }
                        }

                    }

                }
            }

        }

        //  if (GetState() == State.PlayerOppOptions)
        //  ShowWhite();
    }

    public void ShowItemAttackbleTiles(LivingObject obj, ItemScript skill, GridObject target = null)
    {
        ShowWhite();
        List<List<TileScript>> tempTiles = GetAttackableTiles(obj, skill);
        if (tempTiles == null)
            return;
        if (GetState() == State.PlayerOppOptions)
            return;
        if (tempTiles.Count > 0)
        {
            for (int i = 0; i < tempTiles.Count; i++) //list of lists
            {
                for (int j = 0; j < tempTiles[i].Count; j++) //indivisual list
                {

                    if (skill.ITYPE != ItemType.dmg && skill.ITYPE != ItemType.dart)
                    {
                        if (target != null)
                        {
                            if (tempTiles[i][j] == target.currentTile)
                            {
                                tempTiles[i][j].MYCOLOR = Common.green;
                            }
                            else if (tempTiles[i][j].MYCOLOR != Common.green)
                            {
                                tempTiles[i][j].MYCOLOR = Common.lime;
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                tempTiles[i][j].MYCOLOR = Common.green;
                            }
                            else if (tempTiles[i][j].MYCOLOR != Common.green)
                            {
                                tempTiles[i][j].MYCOLOR = Common.lime;
                            }

                        }
                    }
                    else
                    {
                        if (target != null)
                        {
                            if (tempTiles[i][j] == target.currentTile)
                            {
                                tempTiles[i][j].MYCOLOR = Common.red;
                            }
                            else if (tempTiles[i][j].MYCOLOR != Common.red)
                            {
                                tempTiles[i][j].MYCOLOR = Common.pink;
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                tempTiles[i][j].MYCOLOR = Common.red;
                            }
                            else if (tempTiles[i][j].MYCOLOR != Common.red)
                            {
                                tempTiles[i][j].MYCOLOR = Common.pink;
                            }
                        }

                    }

                }
            }

        }
    }

    public void ShowWeaponAttackbleTiles(LivingObject obj, WeaponScript skill, GridObject target = null)
    {
        for (int k = 0; k < liveEnemies.Count; k++)
        {
            liveEnemies[k].ShowHideWeakness(EHitType.normal, false);

        }
        ShowWhite();
        List<List<TileScript>> tempTiles = GetAttackableTiles(obj, skill);
        if (tempTiles == null)
            return;
        if (GetState() == State.PlayerOppOptions)
            return;

        if (tempTiles.Count > 0)
        {
            for (int i = 0; i < tempTiles.Count; i++) //list of lists
            {
                for (int j = 0; j < tempTiles[i].Count; j++) //indivisual list
                {
                    if (target != null)
                    {
                        if (tempTiles[i][j] == target.currentTile)
                        {
                            tempTiles[i][j].MYCOLOR = Common.red;
                        }
                        else if (tempTiles[i][j].MYCOLOR != Common.red)
                        {
                            tempTiles[i][j].MYCOLOR = Common.pink;
                        }
                        if (GetState() != State.EnemyTurn)
                        {
                            for (int k = 0; k < liveEnemies.Count; k++)
                            {
                                if (tempTiles[i][j] == liveEnemies[k].currentTile)
                                {

                                    EHitType hitType = liveEnemies[k].ARMOR.HITLIST[(int)skill.ELEMENT];
                                    liveEnemies[k].ShowHideWeakness(hitType);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            tempTiles[i][j].MYCOLOR = Common.red;
                        }
                        else if (tempTiles[i][j].MYCOLOR != Common.red)
                        {
                            tempTiles[i][j].MYCOLOR = Common.pink;
                        }

                        if (GetState() != State.EnemyTurn)
                        {
                            for (int k = 0; k < liveEnemies.Count; k++)
                            {
                                if (tempTiles[i][j] == liveEnemies[k].currentTile)
                                {

                                    EHitType hitType = liveEnemies[k].ARMOR.HITLIST[(int)skill.ELEMENT];
                                    liveEnemies[k].ShowHideWeakness(hitType);
                                }
                            }
                        }
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
        if (liveObj == null)
        {
            return null;
        }
        if (liveObj.currentTile == null)
        {
            return null;
        }
        int checkIndex = GetTileIndex(liveObj);
        if (checkIndex == -1)
            return null;
        TileScript origin = liveObj.currentTile;
        // List<List<TileScript>> returnList = new List<List<TileScript>>();
        switch (liveObj.WEAPON.EQUIPPED.ATKRANGE)
        {
            case RangeType.adjacent:
                return tileManager.GetAdjecentTilesList(origin);
                break;
            case RangeType.rect:
                return tileManager.GetRectTilesList(origin);
                break;
            case RangeType.pinWheel:
                return tileManager.GetPinWheelTilesList(origin);
                break;
            case RangeType.detached:
                return tileManager.GetDetachedTilesList(origin);
                break;
            case RangeType.stretched:
                return tileManager.GetStretchedTilesList(origin);
                break;
            case RangeType.rotator:
                return tileManager.GetRotatorTilesList(origin);
                break;
            case RangeType.fan:
                return tileManager.GetFanTilesList(origin);
                break;
            case RangeType.spear:
                return tileManager.GetSpearTilesList(origin);
                break;
            case RangeType.lance:
                return tileManager.GetLanceTilesList(origin);
                break;
            case RangeType.line:
                return tileManager.GetLineTilesList(origin);
                break;
            case RangeType.cone:
                return tileManager.GetConeTilesList(origin);
                break;
            case RangeType.tpose:
                return tileManager.GetTPoseTilesList(origin);
                break;
            case RangeType.clover:
                return tileManager.GetCloverTilesList(origin);
                break;
            case RangeType.cross:
                return tileManager.GetCrossTilesList(origin);
                break;
            case RangeType.square:
                return tileManager.GetSquareTilesList(origin);
                break;
            case RangeType.box:
                return tileManager.GetBoxTilesList(origin);
                break;
            case RangeType.diamond:
                return tileManager.GetDiamondTilesList(origin);
                break;
            case RangeType.crosshair:
                return tileManager.GetCrosshairTilesList(origin);
                break;
            case RangeType.any:
                {
                    List<TileScript> mytiles = tileManager.GetAdjecentTiles(origin);

                    mytiles.Add(origin);
                    List<List<TileScript>> returnList = new List<List<TileScript>>();
                    returnList.Add(mytiles);
                    return returnList;

                }
                break;
        }
        //int Dist = liveObj.WEAPON.DIST;
        //int Range = liveObj.WEAPON.Range;

        //Vector2 checkDist = Vector2.zero;

        //for (int i = 0; i < 4; i++)
        //{
        //    switch (i)
        //    {
        //        case 0:
        //            checkDist.x = 0;
        //            checkDist.y = 1;
        //            break;

        //        case 1:
        //            checkDist.x = 1;
        //            checkDist.y = 0;
        //            break;

        //        case 2:
        //            checkDist.x = 0;
        //            checkDist.y = -1;
        //            break;

        //        case 3:
        //            checkDist.x = -1;
        //            checkDist.y = 0;
        //            break;
        //    }

        //    List<TileScript> tiles = new List<TileScript>();
        //    for (int j = 0; j < Range; j++)
        //    {
        //        Vector3 checkPos = liveObj.transform.position;

        //        checkPos.x += (checkDist.x * Dist);
        //        checkPos.z += (checkDist.y * Dist);

        //        checkPos.x -= (checkDist.x * j);
        //        checkPos.z -= (checkDist.y * j);


        //        int testIndex = GetTileIndex(checkPos);
        //        if (testIndex >= 0)
        //        {
        //            TileScript t = GetTileAtIndex(testIndex);

        //            float checkX = Mathf.Abs(checkPos.x - liveObj.transform.position.x);
        //            float checkY = Mathf.Abs(checkPos.z - liveObj.transform.position.z);


        //            if (checkX + checkY <= Dist)
        //            {



        //                TileScript realTile = GetTileAtIndex(testIndex);
        //                if (!tiles.Contains(realTile))
        //                    tiles.Add(realTile);
        //            }
        //        }
        //    }
        //    if (tiles.Count > 0)
        //    {
        //        returnList.Add(tiles);
        //    }
        //}
        return null;
    }
    public void SelectMenuItem(GridObject invokingObject)
    {
        CreateEvent(this, invokingObject, "menuItemEvent", ReturnTrue, null, -1, MenuItemStart);

    }
    public void MenuItemStart(Object data)
    {
        GridObject invokingObject = data as GridObject;
        if (invManager)
        {
            //   Debug.Log("Selecting menu item from obj :" + invokingObject.FullName);
            invManager.selectedMenuItem.ApplyAction(invokingObject);
        }
        myCamera.UpdateCamera();
        updateConditionals();
    }

    public bool ReturnTrue(Object data)
    {
        return true;
    }

    public bool CheckSceneRunning(Object data)
    {
        return !currentScene.isRunning;
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
        if (GetState() == State.PlayerTransition)
            return;
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
                if (textHolder.menuDescription)
                {
                    TextObjectHandler.UpdateText(textHolder.menuDescription, selectedItem.GetDescription());
                }
            }// invManager.ForceSelect();
             // invManager.Validate("Manager hover");

        }
        myCamera.UpdateCamera();
        updateConditionals();
    }
    public bool ComfirmMenuAction(GridObject invokingObject)
    {
        bool res = false;
        //for (int i = 0; i < commandItems.Length; i++)
        //{

        //    if (commandItems[i].itemType == currentMenuitem)
        //    {
        //    }
        //}
        res = currentMenuitem.ComfirmAction(invokingObject, currentTutorial);




        return res;


    }
    public void CancelMenuAction(GridObject invokingObject)
    {
        if (attackableTiles != null)
        {
            attackableTiles.Clear();
            ShowWhite();
        }
        if (!invokingObject)
            return;
        Vector3 resetPos = invokingObject.currentTile.transform.position;
        resetPos.y = invokingObject.currentTile.transform.position.y + 0.5f;
        invokingObject.transform.position = resetPos;


        if (newSkillEvent.caller)
        {
            newSkillEvent.caller = null;

        }
        else
        {
            returnState();
            //if (prevState == State.PlayerOppOptions)
            //{
            //}
            //else
            //{
            //CreateEvent(this, null, "return state event", BufferedReturnEvent);

            //}


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
    public void anchorHpBar()
    {
        if (potential)
        {
            potential.anchor();
        }
    }
    public void DamageGridObject(GridObject dmgObject, int dmg)
    {
        if (!dmgObject)
        {
            return;
        }

        dmgObject.STATS.HEALTH -= dmg;

        if (log)
        {
            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(dmgObject.FACTION)) + ">";

            log.Log(coloroption + dmgObject.NAME + "</color> lost " + dmg.ToString() + " health");
        }
        if (dmgObject.GetComponent<LivingObject>())
        {
            LivingObject livedmgObbj = dmgObject.GetComponent<LivingObject>();
            livedmgObbj.UpdateHealthbar();
            if (livedmgObbj.ARMOR.DamageArmor(dmg))
            {

                if (options.battleAnims)
                {
                    GridAnimationObj gao = null;
                    gao = PrepareGridAnimation(null, livedmgObbj);
                    gao.type = -1;
                    gao.magnitute = 0;
                    gao.LoadGridAnimation();

                    CreateEvent(this, gao, "Animation request: " + AnimationRequests + "", CheckAnimation, gao.StartCountDown, 0);
                }
                if (livedmgObbj.ARMOR != livedmgObbj.DEFAULT_ARMOR)
                {

                    CreateTextEvent(this, dmgObject.NAME + " Barrier broke!", "Barrier break event", CheckText, TextStart);
                    if (log)
                    {
                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(dmgObject.FACTION)) + ">";
                        log.Log(coloroption + dmgObject.NAME + "</color> Barrier broke!");
                    }
                }
            }
            if (livedmgObbj.SHIELDS > 0)
            {
                livedmgObbj.SHIELDS--;
                if (livedmgObbj.SHIELDS <= 0)
                {
                    if (livedmgObbj.PSTATUS == PrimaryStatus.guarding)
                    {
                        livedmgObbj.PSTATUS = PrimaryStatus.normal;
                    }
                }
                livedmgObbj.updateAilmentIcons();
            }
        }
        //if (livedmgObbj.PSTATUS != PrimaryStatus.guarding)
        //{
        //    livedmgObbj.ACTIONS--;
        //}


    }

    public void KillGridObject(LivingObject killer, GridObject killedObj)
    {
        killedObj.STATS.HEALTH = 0;
        CheckForDeath(killer, killedObj, true);
    }

    public DmgReaction CalcDamage(LivingObject attackingObject, GridObject dmgObject, Element attackingElement, EType attackType, int dmg, Reaction alteration = Reaction.none)
    {

        if (attackingElement == Element.Buff)
        {
            Debug.Log("U don goofed");
        }
        DmgReaction react = new DmgReaction();

        if (attackingObject == null || dmgObject == null)
        {
            return react;
        }

        react.dmgElement = attackingElement;
        float mod = 1.0f;

        int returnInt = 0;
        float calc = 0.0f;
        float reduction = 1.0f;
        float increasedDmg = 1.0f;


        if (attackingObject.PSTATUS == PrimaryStatus.tired)
        {
            reduction = 0.8f;

        }
        if (attackingObject.PSTATUS == PrimaryStatus.crippled)
        {
            reduction = 0.5f;
        }

        if (attackType == EType.physical)
        {

            calc = (float)attackingObject.STRENGTH / (float)(1 + dmgObject.BASE_STATS.DEFENSE);
        }
        else if (attackType == EType.magical)
        {

            calc = (float)attackingObject.MAGIC / (float)(1 + dmgObject.BASE_STATS.RESIESTANCE);
        }
        else
        {

            calc = (float)attackingObject.DEX / (float)(1 + dmgObject.BASE_STATS.SPEED);
        }

        calc += 1;
        //  calc *= ((dmg * increasedDmg * reduction) / resist);
        calc *= dmg * increasedDmg * reduction;

        mod = ApplyDmgMods(attackingObject, mod, attackingElement, attackType);


        calc = calc * mod;

        if (calc < 0)
        {
            calc = 0;
        }
        else if (calc < 1)
        {
            calc = 1.0f;
        }
        returnInt = (Mathf.RoundToInt(calc));
        // Debug.Log("FInal:" + returnInt);
        if (returnInt > Common.maxDmg)
        {
            returnInt = Common.maxDmg;
        }
        react.damage = returnInt;

        return react;
    }
    public DmgReaction CalcDamage(LivingObject attackingObject, GridObject dmgObject, CommandSkill skill, Reaction alteration = Reaction.none, bool applyAccuraccy = true)
    {
        if (skill == null)
            return new DmgReaction() { damage = 0, reaction = Reaction.none };
        //  Debug.Log("Calc 1");
        switch (skill.SUBTYPE)
        {
            case SubSkillType.Buff:
                {

                }
                break;
            case SubSkillType.Debuff:
                {

                }

                break;
            case SubSkillType.Heal:
                {
                    return new DmgReaction() { damage = (int)(0.10f + ((float)(attackingObject.MAGIC + attackingObject.MAGLEVEL) / 100.0f) * dmgObject.BASE_STATS.MAX_HEALTH), reaction = Reaction.Heal, usedSkill = skill };
                }
                break;
            case SubSkillType.Ailment:
                //  ApplyEffect(dmgObject, skill.EFFECT, skill.ACCURACY, skill);

                break;
            default:
                {
                    int chance = skill.ACCURACY;
                    //   int result = (int)Random.Range(0.0f, 100.0f);
                    //  if (applyAccuraccy == false)
                    chance = 100;
                    //  if (result <= chance)
                    {
                        return CalcDamage(attackingObject, dmgObject, skill.ELEMENT, skill.ETYPE, (int)skill.DAMAGE, alteration);
                    }
                    //   else
                    {
                        //  return new DmgReaction() { damage = 0, reaction = Reaction.missed };
                    }
                }
                break;
        }
        //    ApplyEffect(dmgObject, skill.EFFECT);
        //}
        return new DmgReaction() { damage = 0, reaction = Reaction.none };

    }
    public DmgReaction CalcDamage(LivingObject attackingObject, GridObject dmgObject, WeaponEquip weapon, Reaction alteration = Reaction.none, bool applyAccuraccy = true)
    {
        // Debug.Log("Calc 3");
        //  int chance = weapon.ACCURACY;
        //  int result = (int)Random.Range(0.0f, 100.0f);
        //  if (applyAccuraccy == false)
        {
            //     chance = 100;
        }
        // if (result <= chance)
        {
            DmgReaction react = CalcDamage(attackingObject, dmgObject, weapon.ELEMENT, weapon.ATTACK_TYPE, weapon.ATTACK, alteration);
            react.atkName = weapon.NAME;
            return react;
        }
        //  else
        {
            //   return new DmgReaction() { damage = 0, reaction = Reaction.missed };
        }
    }

    public DmgReaction CalcDamage(LivingObject attackingObject, LivingObject dmgObject, Element attackingElement, EType attackType, int dmg, Reaction alteration = Reaction.none)
    {

        DmgReaction react = new DmgReaction();
        if (attackingElement == Element.Buff || dmgObject == null)
        {
            Debug.Log("U don goofed");
            return react;
        }
        if (attackingElement == Element.Support)
        {
            return react;
        }
        react.dmgElement = attackingElement;
        float mod = 0.0f;
        switch (dmgObject.ARMOR.HITLIST[(int)attackingElement])
        {
            case EHitType.normal:
                mod = 1.0f;
                react.reaction = Reaction.none;

                break;
            case EHitType.resists:
                mod = 0.5f;
                react.reaction = Reaction.resist;

                break;
            case EHitType.nulls:
                react.damage = 0;
                react.reaction = Reaction.nulled;

                return react;

            case EHitType.reflects:
                if (attackingObject.ARMOR.HITLIST[(int)attackingElement] != EHitType.reflects)
                {
                    mod = 1.0f;
                    //   react = CalcDamage(dmgObject, attackingObject, attackingElement, attackType, dmg);
                    react.reaction = Reaction.reflected;

                }
                else
                {
                    Debug.Log("Reflects would cause infinate");
                    react.damage = 0;
                    react.reaction = Reaction.nulled;
                    return react;
                }
                break;
            case EHitType.absorbs:
                mod = 1.0f;
                react.reaction = Reaction.absorb;
                break;

            case EHitType.weak:
                //  mod = 2.0f;
                mod = 1.5f;
                react.reaction = Reaction.weak;
                break;

            case EHitType.savage:

                react.reaction = Reaction.savage;
                // mod = 4.0f;
                mod = 2.0f;
                break;

            case EHitType.cripples:
                //  mod = 4.0f;
                mod = 2.0f;
                react.reaction = Reaction.cripple;
                break;

            case EHitType.lethal:
                // mod = 6.0f;
                mod = 3.0f;
                react.reaction = Reaction.leathal;
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
        if (dmgObject.PSTATUS == PrimaryStatus.guarding)
        {
            // reduction *= 0.5f;
        }



        // Debug.Log("Str: " + attackingObject.STRENGTH);
        // Debug.Log("E Def: " + (dmgObject.DEFENSE + dmgObject.ARMOR.DEFENSE));
        // Debug.Log("red: " + reduction);
        // Debug.Log("IncDmg: " + increasedDmg);
        // Debug.Log("Resist: " + resist);

        int diff = dmg;

        if (attackType == EType.physical)
        {
            //if (alteration == Reaction.reduceDef)
            //{
            //    resist *= 0.5f;
            //}
            //calc = ((float)attackingObject.STRENGTH * reduction * increasedDmg) * (attackingObject.STRENGTH / resist);
            resist = dmgObject.DEFENSE;
            if (resist <= 0)
            {
                resist = 1;
            }
            calc = attackingObject.STRENGTH;
            diff = attackingObject.STRENGTH - dmgObject.DEFENSE;
        }
        else if (attackType == EType.magical)
        {
            //if (alteration == Reaction.reduceRes)
            //{
            //    resist *= 0.5f;
            //}
            //calc = ((float)attackingObject.MAGIC * reduction * increasedDmg) * (attackingObject.MAGIC / resist);
            resist = dmgObject.RESIESTANCE;
            if (resist <= 0)
            {
                resist = 1;
            }
            calc = attackingObject.MAGIC;
            diff = attackingObject.MAGIC - dmgObject.RESIESTANCE;
        }
        else
        {
            resist = dmgObject.SPEED;
            if (resist <= 0)
            {
                resist = 1;
            }
            calc = attackingObject.DEX;
            diff = attackingObject.DEX - dmgObject.SPEED;
        }

        if (diff > 5)
        {
            // calc = dmg * increasedDmg * reduction * 2.0f;
            calc *= ((float)(dmg * increasedDmg * reduction) / (float)resist) * 1.2f;
        }
        else if (diff < -5)
        {
            //  calc = dmg * increasedDmg * reduction * 0.5f;
            calc *= (((float)(dmg * increasedDmg * reduction) / (float)resist) * 0.8f) + 1;
        }
        else
        {
            //  calc = dmg * increasedDmg * reduction;
            calc *= ((float)(dmg * increasedDmg * reduction) / (float)resist);
        }

        //if (attackingObject.LEVEL > dmgObject.LEVEL + 2)
        //{
        //    calc *= 1.2f;
        //}
        //else if (attackingObject.LEVEL - 2 < dmgObject.LEVEL)
        //{
        //    calc *= 0.8f;
        //}

        mod = ApplyDmgMods(attackingObject, dmgObject, mod, attackingElement, attackType);

        // Debug.Log("Mod: " + mod);
        calc = calc * mod;
        // Debug.Log("Calc: " + calc);
        // Debug.Log("DMG: " + dmg);
        // calc = dmg * calc;
        //  Debug.Log("Combined: " + calc);
        //   Debug.Log("Calc2:" + calc);
        //  calc = calc * ((float)attackingObject.LEVEL / (float)dmgObject.LEVEL);
        // Debug.Log("Calc with levels:" + calc);
        //  calc = Mathf.Sqrt(calc * 2);
        // Debug.Log("Calc4:" + calc);

        // Debug.Log("Calc final: " + calc);

        if (dmgObject.PSTATUS == PrimaryStatus.guarding)
        {

            calc = calc * 0.5f;

        }

        if (calc < 0)
        {
            calc = 0;
        }
        if (calc == 0.5f)
        {
            calc = 1.0f;
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

    public DmgReaction CalcDamage(LivingObject attackingObject, LivingObject dmgObject, CommandSkill skill, Reaction alteration = Reaction.none, bool applyAccuraccy = true)
    {
        //  Debug.Log("Calc 1");
        if (!skill)
        {
            return new DmgReaction() { damage = 0, reaction = Reaction.missed };
        }
        switch (skill.SUBTYPE)
        {
            case SubSkillType.Buff:
                {

                    //List<CommandSkill> passives = dmgObject.GetComponent<InventoryScript>().BUFFS;
                    //if (!passives.Contains(skill))
                    //{
                    //    bool sameType = false;
                    //    for (int i = 0; i < passives.Count; i++)
                    //    {
                    //        if (passives[i].BUFFEDSTAT == skill.BUFFEDSTAT && passives[i].BUFFVAL == skill.BUFFVAL)
                    //        {
                    //            sameType = true;
                    //            break;
                    //        }
                    //    }
                    //    if (sameType == false)
                    //    {
                    //        skill.OWNER = dmgObject;
                    //        dmgObject.INVENTORY.BUFFS.Add(skill);
                    //        BuffScript buff = dmgObject.gameObject.AddComponent<BuffScript>();
                    //        buff.SKILL = skill;
                    //        buff.BUFF = skill.BUFF;
                    //        buff.COUNT = 3;
                    //        dmgObject.ApplyPassives();
                    //    }
                    //}
                    return new DmgReaction() { damage = 0, reaction = Reaction.buff, usedSkill = skill };
                }
                break;
            case SubSkillType.Debuff:
                {

                    return new DmgReaction() { damage = 0, reaction = Reaction.debuff, usedSkill = skill };
                }

                break;
            case SubSkillType.Heal:
                {
                    return new DmgReaction() { damage = (int)(0.10f + ((float)(attackingObject.MAGIC + attackingObject.MAGLEVEL) / 100.0f) * dmgObject.MAX_HEALTH), reaction = Reaction.Heal, usedSkill = skill };
                }
                break;
            case SubSkillType.Enemy:
                {
                    Debug.Log("Hi enemy");
                    ApplyEffect(dmgObject, skill.EFFECT, skill.ACCURACY, skill);
                    return new DmgReaction() { damage = 0, reaction = Reaction.none, usedSkill = skill };
                }
                break;
            case SubSkillType.Ailment:
                if (applyAccuraccy == false)
                {
                    return new DmgReaction() { damage = 0, reaction = Reaction.AilmentOnly };
                }
                bool rest = ApplyEffect(dmgObject, skill.EFFECT, skill.ACCURACY, skill);
                if (rest == true)
                {

                    return new DmgReaction() { damage = 0, reaction = Reaction.AilmentOnly };
                }
                else
                {

                    return new DmgReaction() { damage = 0, reaction = Reaction.missed };
                }
                break;
            case SubSkillType.Item:
                {
                    return CalcDamage(attackingObject, dmgObject, skill.ELEMENT, skill.ETYPE, (int)skill.DAMAGE, alteration);
                }
            case SubSkillType.Strike:
                {
                    return CalcDamage(attackingObject, dmgObject, skill.ELEMENT, skill.ETYPE, (int)skill.DAMAGE, alteration);
                }
            case SubSkillType.Skill:
                {
                    return CalcDamage(attackingObject, dmgObject, skill.ELEMENT, skill.ETYPE, (int)skill.DAMAGE, alteration);
                }
            case SubSkillType.Spell:
                {
                    return CalcDamage(attackingObject, dmgObject, skill.ELEMENT, skill.ETYPE, (int)skill.DAMAGE, alteration);
                }
            default:
                {
                    int chance = skill.ACCURACY;
                    int result = (int)Random.Range(0.0f, 100.0f);
                    if (applyAccuraccy == false || currentTutorial.isActive)
                        chance = 100;
                    if (result <= chance)
                    {
                        return CalcDamage(attackingObject, dmgObject, skill.ELEMENT, skill.ETYPE, (int)skill.DAMAGE, alteration);
                    }
                    else
                    {
                        return new DmgReaction() { damage = 0, reaction = Reaction.missed };
                    }
                }
                break;
        }
        //    ApplyEffect(dmgObject, skill.EFFECT);
        //}


    }

    public DmgReaction CalcDamage(AtkContainer conatiner, bool applyAccuraccy = true)
    {
        // Debug.Log("Calc 2");

        if (conatiner.command)
        {

            if (conatiner.dmgObject.GetComponent<LivingObject>())
            {
                return CalcDamage(conatiner.attackingObject, conatiner.dmgObject as LivingObject, conatiner.command, conatiner.alteration, applyAccuraccy);
            }
            else
            {
                return CalcDamage(conatiner.attackingObject, conatiner.dmgObject, conatiner.command, conatiner.alteration, applyAccuraccy);
            }
        }
        else
        {

            if (conatiner.dmgObject.GetComponent<LivingObject>())
            {
                return CalcDamage(conatiner.attackingObject, conatiner.dmgObject as LivingObject, conatiner.attackingObject.WEAPON, conatiner.alteration, applyAccuraccy);
            }
            else
            {
                return CalcDamage(conatiner.attackingObject, conatiner.dmgObject, conatiner.attackingObject.WEAPON, conatiner.alteration, applyAccuraccy);
            }



        }

    }


    public DmgReaction CalcDamage(LivingObject attackingObject, LivingObject dmgObject, WeaponEquip weapon, Reaction alteration = Reaction.none, bool applyAccuraccy = true)
    {
        // Debug.Log("Calc 3");

        int chance = weapon.ACCURACY;
        int result = (int)Random.Range(0.0f, 100.0f);
        if (applyAccuraccy == false)
        {
            chance = 100;
        }
        if (result <= chance)
        {
            DmgReaction react = CalcDamage(attackingObject, dmgObject, weapon.ELEMENT, weapon.ATTACK_TYPE, weapon.ATTACK, alteration);
            react.atkName = weapon.NAME;
            return react;
        }
        else
        {
            return new DmgReaction() { damage = 0, reaction = Reaction.missed };
        }
    }

    public float ApplyDmgMods(LivingObject attacker, LivingObject defender, float dmg, Element atkAffinity, EType eType)
    {
        List<ComboSkill> passives = attacker.INVENTORY.COMBOS;
        attacker.UpdateBuffsAndDebuffs();
        float modification = 1.0f;


        List<ComboSkill> epassives = defender.INVENTORY.COMBOS;
        defender.UpdateBuffsAndDebuffs();

        List<CommandSkill> buffs = attacker.INVENTORY.BUFFS;

        for (int i = 0; i < buffs.Count; i++)
        {
            if (buffs[i].BUFF == BuffType.Str && eType == EType.physical)
            {
                modification += ((100 + buffs[i].BUFFVAL) / 100.0f);
            }
            else if (buffs[i].BUFF == BuffType.Mag && eType == EType.magical)
            {
                modification += ((100 + buffs[i].BUFFVAL) / 100.0f);
            }
            else if (buffs[i].BUFF == BuffType.Dex && eType == EType.mental)
            {
                modification += ((100 + buffs[i].BUFFVAL) / 100.0f);
            }
        }
        List<CommandSkill> ebuffs = defender.INVENTORY.BUFFS;

        for (int i = 0; i < ebuffs.Count; i++)
        {
            if (ebuffs[i].BUFF == BuffType.Def && eType == EType.physical)
            {
                modification -= ((100 + ebuffs[i].BUFFVAL) / 100.0f);
            }
            else if (ebuffs[i].BUFF == BuffType.Res && eType == EType.magical)
            {
                modification -= ((100 + ebuffs[i].BUFFVAL) / 100.0f);
            }
            else if (ebuffs[i].BUFF == BuffType.Spd && eType == EType.mental)
            {
                modification -= ((100 + ebuffs[i].BUFFVAL) / 100.0f);
            }
        }
        List<CommandSkill> debuffs = attacker.INVENTORY.DEBUFFS;
        for (int i = 0; i < debuffs.Count; i++)
        {
            if (debuffs[i].BUFF == BuffType.Str && eType == EType.physical)
            {
                modification -= ((100 + debuffs[i].BUFFVAL) / 100.0f);
            }
            else if (debuffs[i].BUFF == BuffType.Mag && eType == EType.magical)
            {
                modification -= ((100 + debuffs[i].BUFFVAL) / 100.0f);
            }
        }

        List<CommandSkill> edebuffs = defender.INVENTORY.DEBUFFS;
        for (int i = 0; i < edebuffs.Count; i++)
        {
            if (edebuffs[i].BUFF == BuffType.Def && eType == EType.physical)
            {
                modification += ((100 + edebuffs[i].BUFFVAL) / 100.0f);
            }
            else if (edebuffs[i].BUFF == BuffType.Res && eType == EType.magical)
            {
                modification += ((100 + edebuffs[i].BUFFVAL) / 100.0f);
            }
        }

        dmg = dmg * modification;
        return dmg;
    }

    public float ApplyDmgMods(LivingObject attacker, float dmg, Element atkAffinity, EType eType)
    {
        attacker.UpdateBuffsAndDebuffs();


        List<CommandSkill> buffs = attacker.INVENTORY.BUFFS;

        for (int i = 0; i < buffs.Count; i++)
        {
            if (buffs[i].BUFF == BuffType.Str && eType == EType.physical)
            {
                dmg *= ((100 + buffs[i].BUFFVAL) / 100.0f);
            }
            else if (buffs[i].BUFF == BuffType.Mag && eType == EType.magical)
            {
                dmg *= ((100 + buffs[i].BUFFVAL) / 100.0f);
            }
        }

        List<CommandSkill> debuffs = attacker.INVENTORY.DEBUFFS;
        for (int i = 0; i < debuffs.Count; i++)
        {
            if (debuffs[i].BUFF == BuffType.Str && eType == EType.physical)
            {
                dmg *= ((100 + debuffs[i].BUFFVAL) / 100.0f);
            }
            else if (debuffs[i].BUFF == BuffType.Mag && eType == EType.magical)
            {
                dmg *= ((100 + debuffs[i].BUFFVAL) / 100.0f);
            }
        }

        if (dmg > 2.0f)
            dmg *= 0.8f;
        return dmg;
    }
    public void InteractWithObject()
    {
        if (adjacentInteractable != null)
        {
            menuManager.ShowNone();
            adjacentInteractable.Interact(player.current);
        }
    }
    public void BeginHacking()
    {
        //   menuManager.
        if (adjacentGlyph != null)
        {
            for (int i = 0; i < gridObjects.Count; i++)
            {
                if (gridObjects[i].GetComponent<HazardScript>())
                {
                    if ((gridObjects[i] as HazardScript).HTYPE == HazardType.protection)
                    {
                        CreateTextEvent(this, "A Protection Glyph is preventing you from hacking", "no hack", CheckText, TextStart);
                        PlayExitSnd();
                        return;
                    }
                }
            }

            if (hackingGame)
            {
                hackingGame.strikeCount = adjacentGlyph.hackStrikes;
                hackingGame.skillCount = adjacentGlyph.hackSkills;
                hackingGame.spellCount = adjacentGlyph.hackSpells;
                hackingGame.countDown = adjacentGlyph.hackingTimer;
                hackingGame.LoadSequence(adjacentGlyph.hackingSequence);
                CreateEvent(this, null, "Check Hacking Event", CheckHacking, null, -1, HackingStart);
            }
        }
    }

    public GridAnimationObj PrepareGridAnimation(GridAnimationObj gao, GridObject target)
    {
        if (!target)
        {
            GameObject tjObject = Instantiate(animPrefab);
            tjObject.SetActive(false);
            gao = tjObject.GetComponent<GridAnimationObj>();
            animObjs.Add(gao);
            gao.manager = this;
            gao.Setup();
            // gao.gameObject.SetActive(true);
            //   Vector3 v3 = new Vector3(target.transform.position.x, 1, target.transform.position.z);
            //  v3.z -= 0.1f;
            gao.transform.position = Vector3.zero;
            gao.transform.localRotation = Quaternion.Euler(90, 0, 0);
            return gao;
        }
        AnimationRequests++;
        int busyCount = 0;
        if (AnimationRequests <= animObjs.Count)
        {

            for (int i = 0; i < animObjs.Count; i++)
            {
                if (!animObjs[i].isShowing && !animObjs[i].prepared)
                {
                    gao = animObjs[i];
                    //gao.gameObject.SetActive(true);
                    Vector3 v3 = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z);
                    //  v3.z -= 0.1f;
                    gao.transform.position = v3;
                    gao.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    //  gao.StartCountDown();
                    break;
                }
                else
                {
                    busyCount++;
                }
            }

            if (busyCount == animObjs.Count)
            {
                GameObject tjObject = Instantiate(animPrefab);
                tjObject.SetActive(false);
                gao = tjObject.GetComponent<GridAnimationObj>();
                animObjs.Add(gao);
                gao.manager = this;
                gao.Setup();
                // gao.gameObject.SetActive(true);
                Vector3 v3 = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z);
                //  v3.z -= 0.1f;
                gao.transform.position = v3;
                gao.transform.localRotation = Quaternion.Euler(90, 0, 0);
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
            Vector3 v3 = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z);
            //  v3.z -= 0.1f;
            gao.transform.position = v3;
            gao.transform.localRotation = Quaternion.Euler(90, 0, 0);

            //gao.StartCountDown();
        }

        gao.subtype = -1;
        gao.type = -10;
        gao.target = target;
        gao.prepared = true;
        return gao;
    }
    public GridAnimationObj PrepareGridAnimation(GridAnimationObj gao, Vector3 target)
    {

        AnimationRequests++;
        int busyCount = 0;
        if (AnimationRequests <= animObjs.Count)
        {

            for (int i = 0; i < animObjs.Count; i++)
            {
                if (!animObjs[i].isShowing && !animObjs[i].prepared)
                {
                    gao = animObjs[i];
                    //gao.gameObject.SetActive(true);
                    Vector3 v3 = new Vector3(target.x, target.y + 1, target.z);
                    //  v3.z -= 0.1f;
                    gao.transform.position = v3;
                    gao.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    //  gao.StartCountDown();
                    break;
                }
                else
                {
                    busyCount++;
                }
            }

            if (busyCount == animObjs.Count)
            {
                GameObject tjObject = Instantiate(animPrefab);
                tjObject.SetActive(false);
                gao = tjObject.GetComponent<GridAnimationObj>();
                animObjs.Add(gao);
                gao.manager = this;
                gao.Setup();
                // gao.gameObject.SetActive(true);
                Vector3 v3 = new Vector3(target.x, target.y + 1, target.z);
                //  v3.z -= 0.1f;
                gao.transform.position = v3;
                gao.transform.localRotation = Quaternion.Euler(90, 0, 0);
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
            Vector3 v3 = new Vector3(target.x, target.y + 1, target.z);
            //  v3.z -= 0.1f;
            gao.transform.position = v3;
            gao.transform.localRotation = Quaternion.Euler(90, 0, 0);

            //gao.StartCountDown();
        }

        gao.subtype = -1;
        gao.type = -10;
        gao.target = null;
        gao.prepared = true;
        return gao;
    }
    public void ApplyReaction(LivingObject attackingObject, GridObject target, DmgReaction react, Element dmgElement, CommandSkill cmd = null, bool crit = false)
    {
        //  Debug.Log("Applying dmg: " + react.damage);
        int gtype = (int)dmgElement;

        switch (react.reaction)
        {
            case Reaction.none:
                DamageGridObject(target, react.damage);

                break;
            case Reaction.buff:
                {
                    if (cmd)
                    {

                        if (target.GetComponent<LivingObject>())
                        {
                            LivingObject livetarget = target.GetComponent<LivingObject>();
                            List<CommandSkill> buffs = livetarget.INVENTORY.BUFFS;
                            if (!buffs.Contains(cmd))
                            {
                                bool sameType = false;
                                for (int i = 0; i < buffs.Count; i++)
                                {
                                    if (buffs[i].BUFFEDSTAT == cmd.BUFFEDSTAT && buffs[i].BUFFVAL == cmd.BUFFVAL)
                                    {
                                        sameType = true;
                                        break;
                                    }
                                }
                                if (sameType == false)
                                {
                                    cmd.OWNER = livetarget;
                                    cmd.extra = cmd.NAME;
                                    livetarget.INVENTORY.BUFFS.Add(cmd);
                                    BuffScript buff = target.gameObject.AddComponent<BuffScript>();
                                    buff.SKILL = cmd;
                                    buff.BUFF = cmd.BUFF;
                                    buff.COUNT = 3;
                                    livetarget.UpdateBuffsAndDebuffs();
                                    livetarget.INVENTORY.TBUFFS.Add(buff);
                                    livetarget.updateAilmentIcons();
                                }
                            }
                        }
                    }
                }
                break;
            case Reaction.cripple:
                if (target.GetComponent<LivingObject>())
                {
                    LivingObject liveTarget = target.GetComponent<LivingObject>();
                    DamageGridObject(liveTarget, react.damage);
                    if (liveTarget.PSTATUS != PrimaryStatus.guarding)
                    {
                        liveTarget.PSTATUS = PrimaryStatus.crippled;
                        liveTarget.updateAilmentIcons();
                        liveTarget.ACTIONS--;
                        if (Common.haveBeenCrippled == false)
                        {
                            Common.haveBeenCrippled = true;
                            List<tutorialStep> tSteps = new List<tutorialStep>();
                            List<int> tClar = new List<int>();

                            tSteps.Add(tutorialStep.showTutorial);
                            tClar.Add(2);
                            PrepareTutorial(tSteps, tClar);
                        }
                    }
                    CreateTextEvent(this, "" + attackingObject.FullName + " did CRIPPLING damage", "enemy atk", CheckText, TextStart);
                    if (log)
                    {
                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(attackingObject.FACTION)) + ">";

                        log.Log(coloroption + attackingObject.FullName + "</color> did <color=red>CRIPPLING</color> damage  to " + liveTarget.NAME);
                    }
                }
                break;
            case Reaction.nulled:
                DamageGridObject(target, react.damage);
                CreateTextEvent(this, "" + attackingObject.FullName + " attack was NULLED", "enemy atk", CheckText, TextStart);
                if (log)
                {
                    string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(attackingObject.FACTION)) + ">";

                    log.Log(coloroption + attackingObject.FullName + "</color> attack was <color=#ADD8E6>NULLED</color>");
                }
                break;
            case Reaction.reflected:
                DamageGridObject(attackingObject, react.damage);
                target = attackingObject;
                CreateTextEvent(this, "" + attackingObject.FullName + " attack was reflected back at them", "enemy atk", CheckText, TextStart);
                if (log)
                {
                    string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(attackingObject.FACTION)) + ">";

                    log.Log(coloroption + attackingObject.FullName + "</color> attack was <color=#ADD8E6>REFLECTED</color> back at them");
                }
                break;
            case Reaction.knockback:
                {

                    //   DamageGridObject(target, react.damage);

                    Vector3 direction = attackingObject.currentTile.transform.position - target.currentTile.transform.position;

                    direction.Normalize();
                    direction.x = Mathf.RoundToInt(direction.x);
                    direction.z = Mathf.RoundToInt(direction.z);
                    if (PushGridObject(target, (-1 * direction)))
                    {

                        ComfirmMoveGridObject(target, GetTileIndex(target));
                        CreateTextEvent(this, "" + target.FullName + " was knocked back", "knockback atk", CheckText, TextStart);
                    }
                    else
                    {

                    }

                }
                break;
            case Reaction.pullin:
                {

                    //DamageGridObject(target, react.damage);
                    if (target != null)
                    {
                        Vector3 direction = attackingObject.currentTile.transform.position - target.currentTile.transform.position;
                        direction.Normalize();
                        direction.x = Mathf.RoundToInt(direction.x);
                        direction.z = Mathf.RoundToInt(direction.z);
                        if (PushGridObject(target, (direction)))
                        {
                            ComfirmMoveGridObject(target, GetTileIndex(target));
                            CreateTextEvent(this, "" + target.FullName + " was pulled in", "pullin atk", CheckText, TextStart);
                        }
                    }

                }
                break;
            case Reaction.pushforward:
                {
                    //DamageGridObject(target, react.damage);
                    Vector3 direction = attackingObject.currentTile.transform.position - target.currentTile.transform.position;
                    direction.Normalize();
                    direction.x = Mathf.RoundToInt(direction.x);
                    direction.z = Mathf.RoundToInt(direction.z);
                    if (PushGridObject(target, (-1 * direction)))
                    {
                        ComfirmMoveGridObject(target, GetTileIndex(target));
                        PushGridObject(attackingObject, (-1 * direction));
                        ComfirmMoveGridObject(attackingObject, GetTileIndex(attackingObject));
                        CreateTextEvent(this, "" + target.FullName + " and " + attackingObject.FullName + " were pushed forward", "push forward atk", CheckText, TextStart);
                    }

                }
                break;
            case Reaction.pullback:
                {
                    //DamageGridObject(target, react.damage);
                    Vector3 direction = attackingObject.currentTile.transform.position - target.currentTile.transform.position;
                    direction.Normalize();
                    direction.x = Mathf.RoundToInt(direction.x);
                    direction.z = Mathf.RoundToInt(direction.z);
                    if (PushGridObject(attackingObject, (1 * direction)))
                    {
                        ComfirmMoveGridObject(attackingObject, GetTileIndex(attackingObject));
                        PushGridObject(target, (1 * direction));
                        ComfirmMoveGridObject(target, GetTileIndex(target));
                        CreateTextEvent(this, "" + target.FullName + " and " + attackingObject.FullName + " were pulled back", "pullback atk", CheckText, TextStart);
                    }

                }
                break;
            case Reaction.jumpback:
                {
                    // DamageGridObject(target, react.damage);
                    Vector3 direction = attackingObject.currentTile.transform.position - target.currentTile.transform.position;
                    direction.Normalize();
                    direction.x = Mathf.RoundToInt(direction.x);
                    direction.z = Mathf.RoundToInt(direction.z);
                    if (PushGridObject(attackingObject, (direction.normalized)))
                    {
                        ComfirmMoveGridObject(attackingObject, GetTileIndex(attackingObject));
                        CreateTextEvent(this, "" + attackingObject.FullName + " jumped back", "jumpback atk", CheckText, TextStart);
                    }

                }
                break;
            case Reaction.reposition:
                {
                    // DamageGridObject(target, react.damage);
                    Vector3 direction = attackingObject.currentTile.transform.position - target.currentTile.transform.position;
                    direction.Normalize();
                    direction.x = Mathf.RoundToInt(direction.x);
                    direction.z = Mathf.RoundToInt(direction.z);
                    if (PushGridObject(attackingObject, (-2 * direction)))
                    {

                        ComfirmMoveGridObject(attackingObject, GetTileIndex(attackingObject));
                        CreateTextEvent(this, "" + attackingObject.FullName + " repositioned ", "reposition atk", CheckText, TextStart);
                    }

                }
                break;
            case Reaction.Swap:
                {
                    //  DamageGridObject(target, react.damage);
                    Vector3 direction = attackingObject.transform.position - target.transform.position;
                    Vector3 savedPosition = target.transform.position;
                    TileScript tile = target.currentTile;
                    target.transform.position = attackingObject.transform.position;
                    target.currentTile = attackingObject.currentTile;

                    attackingObject.transform.position = savedPosition;
                    attackingObject.currentTile = tile;

                    ComfirmMoveGridObject(target, GetTileIndex(target));

                    ComfirmMoveGridObject(attackingObject, GetTileIndex(attackingObject));
                    CreateTextEvent(this, "" + target.FullName + " and " + attackingObject.FullName + " were swapped", "swap atk", CheckText, TextStart);

                }
                break;
            case Reaction.ApplyEffect:
                break;
            case Reaction.savage:
                if (target.GetComponent<LivingObject>())
                {
                    CreateTextEvent(this, "" + attackingObject.FullName + " did SAVAGE damage", "enemy atk", CheckText, TextStart);

                    LivingObject liveTarget = target.GetComponent<LivingObject>();
                    DamageGridObject(liveTarget, react.damage);
                    liveTarget.GENERATED--;
                    if (liveTarget.PSTATUS != PrimaryStatus.guarding)
                    {

                        liveTarget.ACTIONS--;
                    }

                    if (log)
                    {
                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(attackingObject.FACTION)) + ">";
                        log.Log(coloroption + attackingObject.FullName + "</color> attack did <color=red>SAVAGE</color> damage");

                        coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(target.FACTION)) + ">";
                        log.Log(coloroption + target.FullName + "</color> has lost an action for next turn");
                    }
                    CreateTextEvent(this, "" + liveTarget.FullName + " lost an action", "enemy atk", CheckText, TextStart);

                }
                break;
            case Reaction.leathal:
                CreateTextEvent(this, "" + attackingObject.FullName + " did LEATHAL damage", "enemy atk", CheckText, TextStart);
                if (log)
                {
                    string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(attackingObject.FACTION)) + ">";
                    log.Log(coloroption + attackingObject.FullName + "</color> attack did <color=red>LEATHAL</color> damage");

                    coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(target.FACTION)) + ">";
                    log.Log(coloroption + target.FullName + "</color> has lost an action for next turn");

                    coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(target.FACTION)) + ">";
                    log.Log(coloroption + target.FullName + "</color> is crippled");
                }
                if (target.GetComponent<LivingObject>())
                {
                    LivingObject liveTarget = target.GetComponent<LivingObject>();
                    DamageGridObject(liveTarget, react.damage);
                    if (liveTarget.PSTATUS != PrimaryStatus.guarding)
                    {
                        liveTarget.PSTATUS = PrimaryStatus.crippled;
                        liveTarget.updateAilmentIcons();
                    }
                    liveTarget.GENERATED--;
                    if (liveTarget.PSTATUS != PrimaryStatus.guarding)
                    {

                        liveTarget.ACTIONS--;
                    }
                    CreateTextEvent(this, "" + liveTarget.FullName + " lost an action", "enemy atk", CheckText, TextStart);

                }
                break;
            case Reaction.absorb:
                {

                    target.STATS.HEALTH += react.damage;
                    CreateTextEvent(this, "" + attackingObject.FullName + " attack healed the enemy", "enemy atk", CheckText, TextStart);
                    if (log)
                    {
                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(attackingObject.FACTION)) + ">";
                        log.Log(coloroption + attackingObject.FullName + "</color> attack was <color=#ADD8E6> ABSORBED </color>");

                        coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(target.FACTION)) + ">";

                        log.Log(coloroption + target.FullName + "</color> healed " + react.damage.ToString() + " health");
                    }
                    // attackingObject.GENERATED++;
                    if (target.GetComponent<LivingObject>())
                    {
                        LivingObject liveTarget = target.GetComponent<LivingObject>();
                        if (liveTarget.PSTATUS != PrimaryStatus.guarding)
                        {
                            liveTarget.PSTATUS = PrimaryStatus.crippled;
                            liveTarget.updateAilmentIcons();
                        }
                        if (liveTarget.PSTATUS != PrimaryStatus.guarding)
                        {

                            //  liveTarget.ACTIONS++;
                        }

                    }
                }
                break;
            case Reaction.resist:
                {
                    if (target.GetComponent<LivingObject>())
                    {
                        LivingObject liveTarget = target.GetComponent<LivingObject>();
                        DamageGridObject(liveTarget, react.damage);



                    }
                    if (log)
                    {
                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(attackingObject.FACTION)) + ">";
                        log.Log(coloroption + attackingObject.FullName + "</color> attack was <color=#00FFFF> RESISTED </color> damage");
                    }
                }
                break;
            case Reaction.weak:
                if (target.GetComponent<LivingObject>())
                {
                    LivingObject liveTarget = target.GetComponent<LivingObject>();
                    DamageGridObject(liveTarget, react.damage);

                    liveTarget.ACTIONS--;

                    CreateTextEvent(this, "" + liveTarget.FullName + " lost an action", "enemy atk", CheckText, TextStart);
                }
                if (log)
                {
                    string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(attackingObject.FACTION)) + ">";
                    log.Log(coloroption + attackingObject.FullName + "</color> attack did <color=red>WEAKENING</color> damage");
                }
                break;
            case Reaction.missed:
                CreateTextEvent(this, "" + attackingObject.FullName + " attack missed", "enemy atk", CheckText, TextStart);
                if (log)
                {
                    string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(attackingObject.FACTION)) + ">";

                    log.Log(coloroption + attackingObject.FullName + "</color> attack MISSED");
                }
                break;
            case Reaction.AilmentOnly:
                break;
            case Reaction.debuff:
                {
                    if (cmd)
                    {

                        if (target.GetComponent<LivingObject>())
                        {
                            LivingObject livetarget = target.GetComponent<LivingObject>();
                            List<CommandSkill> debuffs = livetarget.INVENTORY.DEBUFFS;
                            if (!debuffs.Contains(cmd))
                            {
                                bool sameType = false;
                                for (int i = 0; i < debuffs.Count; i++)
                                {
                                    if (debuffs[i].BUFFEDSTAT == cmd.BUFFEDSTAT && debuffs[i].BUFFVAL == cmd.BUFFVAL)
                                    {
                                        sameType = true;
                                        break;
                                    }
                                }
                                if (sameType == false)
                                {
                                    cmd.OWNER = livetarget;
                                    if (cmd.extra == "")
                                        cmd.extra = cmd.NAME;
                                    livetarget.INVENTORY.DEBUFFS.Add(cmd);
                                    DebuffScript buff = target.gameObject.AddComponent<DebuffScript>();
                                    buff.SKILL = cmd;
                                    buff.BUFF = cmd.BUFF;
                                    buff.COUNT = 3;
                                    livetarget.INVENTORY.TDEBUFFS.Add(buff);

                                    livetarget.UpdateBuffsAndDebuffs();
                                    livetarget.updateAilmentIcons();
                                }
                            }
                        }
                    }
                }
                break;
            case Reaction.bonusAction:
                break;
            case Reaction.Heal:
                target.STATS.HEALTH += react.damage;
                CreateTextEvent(this, "" + attackingObject.FullName + " healed " + target.FullName, "heal atk", CheckText, TextStart);
                break;


            default:
                break;
        }

        if (react.damage == -1)
        {
            return;
        }


        if (react.reaction != Reaction.missed)
        {
            bool killedEnemy = CheckForDeath(attackingObject, target);


            if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
            {
                if (options)
                {
                    //if (options.showExp)
                    //{
                    //    if (GetState() != State.EnemyTurn)
                    //    {
                    //        if (dmgAmount > 0)
                    //            CreateEvent(this, null, "Exp event", UpdateExpBar, ShowExpBar, 0);

                    //    }

                    //}
                    //else
                    //{

                    //    if (attackingObject.BASE_STATS.EXP >= 100)
                    //    {
                    //        attackingObject.LevelUp();

                    //    }
                    //}
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
                            if (react.usedSkill.SUBTYPE == SubSkillType.Buff)
                            {
                                CreateDmgTextEvent("+" + react.usedSkill.BUFF, Common.green, target);
                            }
                            else
                            {
                                CreateDmgTextEvent("-" + react.usedSkill.BUFF, Common.red, target);
                            }

                        }
                        else if (react.reaction == Reaction.debuff)
                        {
                            if (react.usedSkill.SUBTYPE == SubSkillType.Debuff)
                                CreateDmgTextEvent("-" + react.usedSkill.BUFF, Common.red, target);
                        }
                        else if (react.reaction == Reaction.AilmentOnly)
                        {

                            CreateDmgTextEvent(react.usedSkill.NAME, Color.red, target);
                        }
                        else if (react.reaction > Reaction.missed)
                        {
                            CreateDmgTextEvent(react.reaction.ToString(), Common.cyan, target);
                            CreateDmgTextEvent(react.damage.ToString(), Common.green, target);
                        }
                        else if (react.reaction > Reaction.weak)
                        {

                            CreateDmgTextEvent(react.reaction.ToString(), Common.cyan, target);
                            CreateDmgTextEvent(react.damage.ToString(), Common.cyan, target);

                            if (react.reaction == Reaction.reflected)
                                CreateDmgTextEvent(react.damage.ToString(), Color.red, attackingObject);
                            else
                                CreateDmgTextEvent(react.damage.ToString(), Common.cyan, target);
                        }
                        else
                        {

                            if (react.reaction == Reaction.reflected)
                            {
                                CreateDmgTextEvent(react.damage.ToString(), Color.red, attackingObject);
                            }
                            else
                            {
                                if (crit == true)
                                {


                                    CreateDmgTextEvent(react.damage.ToString(), Common.gold, target, 2.0f);

                                }
                                else
                                {
                                    CreateDmgTextEvent(react.damage.ToString(), Color.red, target);
                                }
                            }
                        }
                    }
                    else
                    {
                        CreateDmgTextEvent("MISSED", Common.red, target);

                    }


                }
                // CreateEvent(this, dto, "DmgText request: " + dmgRequest + "", CheckDmgText, dto.StartCountDown, 0);
                if (cmd)
                {
                    //if (cmd.HITS == 1)
                    //{
                    //    if (options.battleAnims)
                    //    {
                    //        int gstype = -1;
                    //        if (cmd.ELEMENT == Element.Pierce)
                    //        {
                    //            gstype = (int)cmd.ETYPE;
                    //        }
                    //        GridAnimationObj gao = null;
                    //        if (react.reaction == Reaction.reflected)
                    //            gao = PrepareGridAnimation(null, attackingObject);
                    //        else
                    //            gao = PrepareGridAnimation(null, target);

                    //        gao.type = gtype;
                    //        gao.subtype = gstype;
                    //        gao.magnitute = react.damage;
                    //        gao.LoadGridAnimation();
                    //        CreateEvent(this, gao, "Animation request: " + AnimationRequests + "", CheckAnimation, gao.StartCountDown, 0);



                    //    }
                    //}
                }

            }
        }
    }

    public bool CheckForDeath(LivingObject attackingObject, GridObject target, bool hacked = false)
    {


        bool killedEnemy = false;
        if (!target)
        {
            return killedEnemy;
        }
        if (target.GetComponent<LivingObject>())
        {
            LivingObject livetarget = target.GetComponent<LivingObject>();
            if (livetarget.CheckIfDead() == true)
            {
                if (!livetarget.DEAD)
                {
                    livetarget.STATS.HEALTH = -1 * livetarget.BASE_STATS.MAX_HEALTH;

                    if (turnOrder.Contains(livetarget))
                    {
                        turnOrder.Remove(livetarget);
                    }
                    target.DEAD = true;//gameObject.SetActive(false);
                    livetarget.Die();
                    if (log)
                    {
                        string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(target.FACTION)) + ">";
                        log.Log(coloroption + target.NAME + "</color> has perished");
                    }

                    if (target.FACTION == Faction.hazard)
                    {
                        HazardScript hazard = target.GetComponent<HazardScript>();
                        if (hazard.CheckIfDead() == true)
                        {

                            UsableScript useable = hazard.GiveReward(attackingObject);
                            if (useable != null && hacked == false)
                            {

                                if (attackingObject.FACTION == Faction.ally)
                                {
                                    if (useable != null)
                                    {

                                        if (Common.hasLearnedFromEnemy == false)
                                        {
                                            List<tutorialStep> tSteps = new List<tutorialStep>();
                                            List<int> tClar = new List<int>();
                                            tSteps.Add(tutorialStep.showTutorial);
                                            tClar.Add(25);
                                            PrepareTutorial(tSteps, tClar);
                                            Common.hasLearnedFromEnemy = true;
                                        }

                                    }

                                    CreateEvent(this, useable, "New Skill Event", CheckCount, null, 0, CountStart);
                                    // CreateTextEvent(this, "" + attackingObject.FullName + " learned " + useable.NAME, "new skill event", CheckText, TextStart);
                                }
                                else
                                {
                                    attackingObject.INVENTORY.USEABLES.Add(useable);

                                    if (useable.GetType().IsSubclassOf(typeof(CommandSkill)))
                                    {

                                        attackingObject.INVENTORY.CSKILLS.Add((CommandSkill)useable);
                                        if (attackingObject.PHYSICAL_SLOTS.SKILLS.Count < 6)
                                            attackingObject.PHYSICAL_SLOTS.SKILLS.Add((CommandSkill)useable);
                                    }

                                    CreateTextEvent(this, "" + attackingObject.FullName + " learned a new skill.", "new skill event", CheckText, TextStart);

                                }

                            }
                            if (hazard.HTYPE == HazardType.controller)
                            {
                                RevealHiddenTileEvent(hazard);
                            }
                            if (hazard.MapIndex >= 0 && currentMap.hazardIndexes.Contains(hazard.MapIndex))
                            {
                                currentMap.hazardIndexes.Remove(hazard.MapIndex);
                            }

                            if (hazard.MapIndex >= 0 && currentMap.hazardIndexes.Contains(hazard.MapIndex))
                            {
                                for (int i = 0; i < currentMap.hazardIndexes.Count; i++)
                                {
                                    if (hazard.MapIndex == currentMap.hazardIndexes[i])
                                    {
                                        currentMap.hazardIndexes.Remove(hazard.MapIndex);
                                        currentMap.hazardIds.RemoveAt(i);
                                        break;
                                    }
                                }

                            }

                            if (liveEnemies.Contains(hazard))
                            {
                                liveEnemies.Remove(hazard);
                                for (int i = 0; i < turnOrder.Count; i++)
                                {
                                    turnOrder[i].turnUpdate(liveEnemies.Count);

                                }
                                bool noMore = true;
                                for (int i = 0; i < liveEnemies.Count; i++)
                                {
                                    if (liveEnemies[i].FACTION == Faction.hazard)
                                    {
                                        HazardScript otherHaz = liveEnemies[i] as HazardScript;
                                        if (otherHaz.HTYPE == hazard.HTYPE)
                                        {
                                            noMore = false;
                                            break;
                                        }
                                    }
                                }
                                if (noMore == true)
                                {
                                    for (int j = 0; j < objectivesAndNotes.Length; j++)
                                    {
                                        if (objectivesAndNotes[j].extraData == (int)hazard.HTYPE)
                                        {
                                            objectivesAndNotes[j].extraData = -1;
                                            objectivesAndNotes[j].gameObject.SetActive(false);
                                            break;
                                        }

                                    }
                                }
                            }
                        }
                    }

                    if (target.GetComponent<EnemyScript>())
                    {
                        EnemyScript enemy = target.GetComponent<EnemyScript>();
                        if (enemy.CheckIfDead() == true)
                        {
                            if (enemy.INVENTORY.USEABLES.Count > 0)
                            {


                                float chance = 25.0f;
                                float result = Random.value * 100;
                                if (result <= chance)
                                {

                                    UsableScript someUse = enemy.TransferSkill(attackingObject);
                                    if (someUse != null)
                                    {

                                        if (Common.hasLearnedFromEnemy == false)
                                        {
                                            List<tutorialStep> tSteps = new List<tutorialStep>();
                                            List<int> tClar = new List<int>();
                                            tSteps.Add(tutorialStep.showTutorial);
                                            tClar.Add(25);
                                            PrepareTutorial(tSteps, tClar);
                                            Common.hasLearnedFromEnemy = true;
                                        }

                                    }
                                    else
                                    {

                                    }

                                }

                            }
                            else
                            {
                                //   Debug.Log("Enemy got no skills");
                            }
                            for (int i = 0; i < currentMap.enemyIndexes.Count; i++)
                            {
                                if (enemy.MapIndex == currentMap.enemyIndexes[i])
                                {
                                    currentMap.enemyIndexes.Remove(enemy.MapIndex);
                                    if (i < currentMap.enemyIds.Count)
                                        currentMap.enemyIds.RemoveAt(i);
                                    break;
                                }
                            }
                            //if (enemy.MapIndex >= 0 && currentMap.enemyIndexes.Contains(enemy.MapIndex))
                            //{
                            //    currentMap.enemyIndexes.Remove(enemy.MapIndex);

                            //}
                            if (liveEnemies.Contains(enemy))
                            {
                                liveEnemies.Remove(enemy);
                                for (int i = 0; i < turnOrder.Count; i++)
                                {
                                    turnOrder[i].turnUpdate(liveEnemies.Count);

                                }
                            }
                        }
                    }

                    if (target.currentTile)
                    {

                        target.currentTile.isOccupied = false;
                        target.currentTile = null;
                    }
                    gridObjects.Remove(target);


                    // target.GetComponent<SpriteRenderer>().color = Common.semi;
                    if (livetarget.FACTION != Faction.ally)
                    {
                        killedEnemy = true;

                    }

                }



                //  Debug.Log("current state: " + currentState);
            }

        }
        else
        {
            if (target.STATS.HEALTH <= 0)
            {
                if (!target.DEAD)
                {
                    target.DEAD = true;
                    target.Die();
                    if (target.MapIndex >= 0 && currentMap.objMapIndexes.Contains(target.MapIndex))
                    {
                        for (int i = 0; i < currentMap.objMapIndexes.Count; i++)
                        {
                            if (target.MapIndex == currentMap.objMapIndexes[i])
                            {
                                currentMap.objMapIndexes.Remove(target.MapIndex);
                                currentMap.objIds.RemoveAt(i);
                                break;
                            }
                        }

                    }
                    if (target.currentTile)
                    {
                        target.currentTile.isOccupied = false;
                        target.currentTile = null;
                    }

                    switch (target.FACTION)
                    {

                        case Faction.ordinary:
                            break;
                        case Faction.eventObj:
                            break;

                        case Faction.dropsSkill:
                            {
                                int physCount = 0;
                                for (int i = 0; i < attackingObject.INVENTORY.CSKILLS.Count; i++)
                                {
                                    if (attackingObject.INVENTORY.CSKILLS[i].ETYPE != EType.magical)
                                    {
                                        physCount++;
                                    }
                                }
                                if (physCount < 6)
                                {
                                    //    Debug.Log("command skill" + itemNum);
                                    int itemNum = 0;
                                    itemNum = Random.Range(0, 4);
                                    itemNum += (Random.Range(1, 9) * 10);
                                    UsableScript useable = database.LearnSkill(itemNum, attackingObject);

                                    if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                                        CreateEvent(this, useable, "New Skill Event", CheckCount, null, 0, CountStart);
                                    //CreateTextEvent(this, "" + attackingObject.FullName + " learned " + useable.NAME, "new skill event", CheckText, TextStart);
                                }
                                else
                                {
                                    int itemNum = 0;
                                    itemNum = Random.Range(0, 4);
                                    itemNum += (Random.Range(1, 9) * 10);
                                    UsableScript useable = database.GetSkill(itemNum);

                                    LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                                    learnContainer.attackingObject = attackingObject;
                                    learnContainer.usable = useable;
                                    CreateEvent(this, learnContainer, "New Skill Event", CheckNewSKillEvent, null, 0, NewSkillStart);

                                }
                            }
                            break;
                        case Faction.dropsSpell:
                            {
                                int magCount = 0;
                                for (int i = 0; i < attackingObject.INVENTORY.CSKILLS.Count; i++)
                                {
                                    if (attackingObject.INVENTORY.CSKILLS[i].ETYPE == EType.magical)
                                    {
                                        magCount++;
                                    }
                                }
                                if (magCount < 6)
                                {
                                    int itemNum = 0;
                                    //itemNum = Random.Range(0, 4);
                                    //itemNum += (Random.Range(1, 8) * 10);
                                    ////    Debug.Log("command skill" + itemNum);
                                    //newItem = database.GetSkill(itemNum);

                                    itemNum = Random.Range(4, 7);
                                    itemNum += (Random.Range(1, 9) * 10);
                                    //    Debug.Log("command spell" + itemNum);

                                    UsableScript useable = database.LearnSkill(itemNum, attackingObject);

                                    if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                                        CreateEvent(this, useable, "New Skill Event", CheckCount, null, 0, CountStart);
                                    // CreateTextEvent(this, "" + attackingObject.FullName + " learned " + useable.NAME, "new skill event", CheckText, TextStart);
                                }
                                else
                                {
                                    int itemNum = 0;
                                    itemNum = Random.Range(4, 7);
                                    itemNum += (Random.Range(1, 9) * 10);
                                    UsableScript useable = database.GetSkill(itemNum);

                                    LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                                    learnContainer.attackingObject = attackingObject;
                                    learnContainer.usable = useable;
                                    CreateEvent(this, learnContainer, "New Skill Event", CheckNewSKillEvent, null, 0, NewSkillStart);
                                }
                            }
                            break;
                        case Faction.dropsStrike:
                            {
                                if (attackingObject.INVENTORY.WEAPONS.Count < 6)
                                {
                                    UsableScript useable = database.GetWeapon(Random.Range(0, 57), attackingObject);

                                    if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                                        CreateEvent(this, useable, "New Skill Event", CheckCount, null, 0, CountStart);
                                    // CreateTextEvent(this, "" + attackingObject.FullName + " learned " + useable.NAME, "new skill event", CheckText, TextStart);
                                }
                                else
                                {

                                    UsableScript useable = database.GetWeapon(Random.Range(0, 57), null);

                                    LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                                    learnContainer.attackingObject = attackingObject;
                                    learnContainer.usable = useable;
                                    CreateEvent(this, learnContainer, "New Skill Event", CheckNewSKillEvent, null, 0, NewSkillStart);
                                }
                            }
                            break;
                        case Faction.dropsBarrier:
                            {
                                if (attackingObject.INVENTORY.ARMOR.Count < 6)
                                {
                                    UsableScript useable = database.GetArmor(Random.Range(0, 11), attackingObject);

                                    if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                                        CreateEvent(this, useable, "New Skill Event", CheckCount, null, 0, CountStart);
                                    // CreateTextEvent(this, "" + attackingObject.FullName + " gained " + useable.NAME, "new skill event", CheckText, TextStart);
                                }
                                else
                                {
                                    UsableScript useable = database.GetArmor(Random.Range(0, 11), null);

                                    LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                                    learnContainer.attackingObject = attackingObject;
                                    learnContainer.usable = useable;
                                    CreateEvent(this, learnContainer, "New Skill Event", CheckNewSKillEvent, null, 0, NewSkillStart);
                                }
                            }
                            break;
                        case Faction.dropsItem:
                            {
                                if (attackingObject.INVENTORY.ITEMS.Count < 6)
                                {
                                    int inum = Random.Range(0, 25);
                                    UsableScript useable = database.GetItem(inum, attackingObject);

                                    if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                                        CreateEvent(this, useable, "New Skill Event", CheckCount, null, 0, CountStart);
                                    if (!attackingObject)
                                    { Debug.Log("no attacker"); }
                                    if (!useable)
                                    { Debug.Log("no usable  for " + inum); }
                                    // CreateTextEvent(this, "" + attackingObject.FullName + " gained " + useable.NAME, "new skill event", CheckText, TextStart);
                                }
                                else
                                {
                                    UsableScript useable = database.GetItem(Random.Range(0, 25), null);

                                    LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                                    learnContainer.attackingObject = attackingObject;
                                    learnContainer.usable = useable;
                                    CreateEvent(this, learnContainer, "New Skill Event", CheckNewSKillEvent, null, 0, NewSkillStart);
                                }
                            }
                            break;
                        case Faction.dropsAuto:
                            {

                                if (attackingObject.INVENTORY.AUTOS.Count < 6)
                                {
                                    int itemNum = 0;
                                    itemNum = Random.Range(110, 120);
                                    UsableScript useable = database.LearnSkill(itemNum, attackingObject);

                                    if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                                        CreateEvent(this, useable, "New Skill Event", CheckCount, null, 0, CountStart);
                                    //  CreateTextEvent(this, "" + attackingObject.FullName + " learned " + useable.NAME, "new skill event", CheckText, TextStart);
                                }
                                else
                                {
                                    int itemNum = 0;
                                    itemNum = Random.Range(110, 120);
                                    UsableScript useable = database.GetSkill(itemNum);

                                    LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                                    learnContainer.attackingObject = attackingObject;
                                    learnContainer.usable = useable;
                                    CreateEvent(this, learnContainer, "New Skill Event", CheckNewSKillEvent, null, 0, NewSkillStart);
                                }
                            }
                            break;
                        case Faction.dropsPassive:
                            {
                                if (attackingObject.INVENTORY.COMBOS.Count < 6)
                                {

                                    int itemNum = 0;
                                    itemNum = Random.Range(201, 224);
                                    UsableScript useable = database.LearnSkill(itemNum, attackingObject);

                                    if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                                        CreateEvent(this, useable, "New Skill Event", CheckCount, null, 0, CountStart);
                                    // CreateTextEvent(this, "" + attackingObject.FullName + " learned " + useable.NAME, "new skill event", CheckText, TextStart);
                                }
                                else
                                {
                                    int itemNum = 0;
                                    itemNum = Random.Range(201, 224);
                                    UsableScript useable = database.GetSkill(itemNum);

                                    LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                                    learnContainer.attackingObject = attackingObject;
                                    learnContainer.usable = useable;
                                    CreateEvent(this, learnContainer, "New Skill Event", CheckNewSKillEvent, null, 0, NewSkillStart);
                                }
                            }
                            break;
                        case Faction.inflictsDmg:
                            {

                            }
                            break;
                    }


                }
            }
        }
        updateConditionals();
        myCamera.UpdateCamera();
        return killedEnemy;
    }

    public void ShowExpBar()
    {
        // Debug.Log("updating exp bar");
        expbar.gameObject.SetActive(true);
        expbar.StartUpdating();

    }
    public bool UpdateExpBar(Object data)
    {
        if (!expbar)
            return true;
        return !expbar.updating;
    }
    public int DetermineExp(LivingObject atker, GridObject target, bool killed)
    {

        for (int i = 0; i < gridObjects.Count; i++)
        {
            if (gridObjects[i].GetComponent<HazardScript>())
            {
                if ((gridObjects[i] as HazardScript).HTYPE == HazardType.zeroExp)
                {
                    return 0;
                }
            }
        }
        if (!target || !atker)
        {
            return 0;
        }
        float subtractVal = Mathf.Min(atker.LEVEL * 15, 100.0f);
        int diff = target.BASE_STATS.LEVEL - atker.LEVEL + (int)((float)(subtractVal - atker.LEVEL) * 0.25f);
        //   Debug.Log("res = " + diff);
        int amount = diff + 4;
        if (diff <= 0)
            diff = 4;
        //   Debug.Log("Killed = " + killed);
        if (killed == true)
        {
            amount += (2 * diff) + 10;

        }
        //  Debug.Log("amt = " + amount);
        if (amount < 0)
        {
            amount = 0;
        }
        //  Debug.Log("amt2 = " + amount);

        atker.GainExp(amount);
        if (expbar)
        {

            expbar.finalValue += amount;
        }
        return amount;
    }

    public void DetermineAtkExp(LivingObject atker, GridObject target, CommandSkill skill, bool basic, int dmg, bool show)
    {


        for (int i = 0; i < gridObjects.Count; i++)
        {
            if (gridObjects[i].GetComponent<HazardScript>())
            {
                if ((gridObjects[i] as HazardScript).HTYPE == HazardType.zeroExp)
                {
                    return;
                }
            }
        }


        //if (expbar)
        //{

        //    expbar.currentUser = atker;
        //    expbar.slider.value = atker.BASE_STATS.EXP;
        //}
        // int realnum = dmg;//(int) (dmg * 0.5f);
        int diff = 0;
        if (target != null & atker != null)
        {
            diff = target.BASE_STATS.LEVEL - atker.LEVEL; //+ realnum;
        }
        int amount = diff + 4;

        if (amount < 0)
        {
            amount = 0;
        }
        float subtractVal = 100;
        if (basic)
        {
            subtractVal = Mathf.Min(atker.DEXLEVEL + atker.LEVEL * 15, 100.0f);
            atker.GainDexExp(amount + (int)((float)(subtractVal - atker.DEXLEVEL) * 0.35f), show);
        }

        if (skill)
        {
            if (skill.ETYPE == EType.physical)
            {
                subtractVal = Mathf.Min(atker.PHYSLEVEL + atker.LEVEL * 15, 100.0f);
                atker.GainPhysExp(amount + (int)((float)(subtractVal - atker.PHYSLEVEL) * 0.35f), show);
            }
            if (skill.ETYPE == EType.magical)
            {
                subtractVal = Mathf.Min(atker.MAGLEVEL + atker.LEVEL * 15, 100.0f);
                atker.GainMagExp(amount + (int)((float)(subtractVal - atker.MAGLEVEL) * 0.35f), show);
            }
        }



    }
    public void CreateDmgTextEvent(string dmgValue, Color color, GridObject target, float atime = 0.5f)
    {
        if (!target)
            return;
        // Debug.Log("requesting " + dmgRequest);
        DmgTextObj dto = null;
        dmgRequest++;
        if (dmgRequest <= dmgText.Count)
        {

            for (int i = 0; i < dmgText.Count; i++)
            {
                if (!dmgText[i].loaded)
                {
                    dto = dmgText[i];
                    dto.text.text = dmgValue;
                    dto.border.text = dmgValue;
                    dto.shadow.text = dmgValue;

                    if (dto.border.text.Contains("<sprite="))
                    {

                        dto.border.text = "";

                        dto.shadow.text = dto.border.text;
                    }
                    Vector3 v3 = new Vector3(target.transform.position.x, 1, target.transform.position.z);
                    //   v3.z -= 0.1f;
                    dto.transform.position = v3;
                    dto.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    dto.transform.Rotate(90, 0, 0);
                    dto.text.color = color;
                    dto.border.color = color + new Color(-0.4f, -0.4f, -0.4f);
                    dmgText[i].loaded = true;
                    dto.gameObject.SetActive(false);
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
            dto.shadow.text = dmgValue;

            if (dto.border.text.Contains("<sprite="))
            {

                dto.border.text = "";

                dto.shadow.text = dto.border.text;
            }
            Vector3 v3 = new Vector3(target.transform.position.x, 1, target.transform.position.z);
            // v3.z -= 0.1f;
            dto.transform.position = v3;
            dto.transform.localRotation = Quaternion.Euler(Vector3.zero);
            dto.transform.Rotate(90, 0, 0);
            dto.text.color = color;
            dto.border.color = color + new Color(-0.4f, -0.4f, -0.4f);
        }
        dto.target = target;
        dto.time = atime;
        CreateEvent(this, dto, "DmgText request: " + dmgRequest + "", CheckDmgText, dto.StartCountDown, 0);

    }
    public bool AttackTarget(LivingObject invokingObject, GridObject target, CommandSkill skill, bool oppAtk = true)
    {
        bool hitSomething = false;


        float modification = 1.0f;
        if (skill.ETYPE == EType.magical)
            modification = invokingObject.STATS.MANACHANGE;

        if (skill.ETYPE == EType.physical)
        {
            if (skill.COST > 0)
            {
                modification = invokingObject.STATS.FTCHARGECHANGE;
            }
            else
            {
                modification = invokingObject.STATS.FTCOSTCHANGE;
            }
        }

        bool acceptable = false;



        if (skill != null)
        {
            MassAtkConatiner conatiners = ScriptableObject.CreateInstance<MassAtkConatiner>();
            conatiners.atkContainers = new List<AtkContainer>();



            acceptable = true;
            CreateTextEvent(this, "" + invokingObject.FullName + " used " + skill.NAME, "skill atk", CheckText, TextStart);

            if (acceptable == true)
            {
                skill.UseSkill(invokingObject, modification);
                hitSomething = true;
                if (log)
                {
                    string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(invokingObject.FACTION)) + ">";
                    log.Log(coloroption + invokingObject.NAME + "</color> used " + skill.NAME);
                }
            }

            if (acceptable == true)
            {



                if (skill.SUBTYPE == SubSkillType.RngAtk)
                {
                    skill.HITS = Random.Range(skill.MIN_HIT, skill.MAX_HIT);
                }
                HazardScript redirect = null;
                for (int i = 0; i < gridObjects.Count; i++)
                {
                    if (gridObjects[i].GetComponent<HazardScript>())
                    {
                        if ((gridObjects[i] as HazardScript).HTYPE == HazardType.redirect)
                        {
                            CreateTextEvent(this, "A Glyph redirected your attack", "redirect glyph", CheckText, TextStart);

                            redirect = gridObjects[i] as HazardScript;
                            break;
                        }
                    }
                }


                int dmgAmount = 0;
                int totalDmg = 0;
                Vector3 calcAnimationLocation = Vector3.zero;
                for (int j = 0; j < skill.HITS; j++)
                {
                    AtkContainer conatiner = ScriptableObject.CreateInstance<AtkContainer>();
                    conatiner.alteration = skill.REACTION;
                    conatiner.attackingElement = skill.ELEMENT;
                    conatiner.attackType = skill.ETYPE;
                    conatiner.attackingObject = invokingObject;
                    conatiner.command = skill;
                    conatiner.dmg = (int)skill.DAMAGE;
                    if (redirect && invokingObject.FACTION != Faction.hazard)
                    {
                        conatiner.dmgObject = redirect;
                    }
                    else
                    {
                        conatiner.dmgObject = target;
                    }


                    DmgReaction react = CalcDamage(conatiner);

                    react.usedSkill = skill;
                    conatiner.react = react;

                    conatiners.atkContainers.Add(conatiner);
                    CreateEvent(this, conatiner, "Skill use event", AttackEvent, null, 0);
                }




                if (redirect && invokingObject.FACTION != Faction.hazard)
                {
                    calcAnimationLocation = invokingObject.currentTile.transform.position;
                }
                else
                {

                    calcAnimationLocation = target.currentTile.transform.position;


                }


                //if (totalDmg > 0)
                //    CreateEvent(this, conatiners, "check for opp event", CheckForOppChanceEvent);
                //if (oppAtk == false)
                //    CreateEvent(this, null, "Show Command", player.ShowCmd);


                if (options)
                {
                    if (options.showExp)
                    {
                        if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                        {
                            if (dmgAmount > 0)
                            {

                            }
                            //CreateEvent(this, null, "Exp event", UpdateExpBar, ShowExpBar);

                        }

                    }
                    else
                    {

                        if (invokingObject.BASE_STATS.EXP >= 100)
                        {
                            //  invokingObject.LevelUp();

                        }
                    }
                    //     if (skill.HITS > 1)
                    CreateGridAnimationEvent(calcAnimationLocation, skill, totalDmg);

                }
                CreateEvent(this, conatiners, "check for opp event", CheckForOppChanceEvent);
            }


        }




        return hitSomething;
    }


    public void CreateGridAnimationEvent(Vector3 calcAnimationLocation, CommandSkill skill, int totalDmg)
    {
        if (options)
        {
            if (options.battleAnims)
            {

                GridAnimationObj gao = null;

                gao = PrepareGridAnimation(null, calcAnimationLocation);

                gao.type = (int)skill.ELEMENT;

                gao.subtype = (int)skill.ETYPE;
                gao.magnitute = totalDmg;
                gao.LoadGridAnimation();
                CreateEvent(this, gao, "Animation request: " + AnimationRequests + "", CheckAnimation, gao.StartCountDown, 0);



            }
        }
    }
    public void CreateGridAnimationEvent(Vector3 calcAnimationLocation, WeaponEquip skill, int totalDmg)
    {
        if (options)
        {
            if (options.battleAnims)
            {

                GridAnimationObj gao = null;

                gao = PrepareGridAnimation(null, calcAnimationLocation);

                gao.type = (int)skill.ELEMENT;

                gao.subtype = (int)EType.mental;
                gao.magnitute = totalDmg;
                gao.LoadGridAnimation();
                CreateEvent(this, gao, "Animation request: " + AnimationRequests + "", CheckAnimation, gao.StartCountDown, 0);



            }
        }
    }
    public void CreateGridAnimationEvent(Vector3 calcAnimationLocation, WeaponScript skill, int totalDmg)
    {
        if (options)
        {
            if (options.battleAnims)
            {

                GridAnimationObj gao = null;

                gao = PrepareGridAnimation(null, calcAnimationLocation);

                gao.type = (int)skill.ELEMENT;

                gao.subtype = (int)EType.mental;
                gao.magnitute = totalDmg;
                gao.LoadGridAnimation();
                CreateEvent(this, gao, "Animation request: " + AnimationRequests + "", CheckAnimation, gao.StartCountDown, 0);



            }
        }
    }

    public bool AttackTargets(LivingObject invokingObject, CommandSkill skill, bool oppAtk = false)
    {

        bool hitSomething = false;
        if (currentAttackList.Count > 0)
        {
            float modification = 1.0f;
            if (skill.ETYPE == EType.magical)
                modification = invokingObject.STATS.MANACHANGE;

            if (skill.ETYPE == EType.physical)
            {
                if (skill.COST > 0)
                {
                    modification = invokingObject.STATS.FTCHARGECHANGE;
                }
                else
                {
                    modification = invokingObject.STATS.FTCOSTCHANGE;
                }
            }
            List<int> targetIndicies = GetTargetList();
            bool acceptable = false;

            if (targetIndicies != null)
            {
                if (targetIndicies.Count > 0)
                {
                    opptargets.Clear();
                    //if (expbar)
                    //{
                    //    if (invokingObject)
                    //    {

                    //        expbar.currentUser = invokingObject;
                    //        expbar.slider.value = invokingObject.BASE_STATS.EXP;
                    //    }
                    //}

                    if (skill != null)
                    {
                        MassAtkConatiner conatiners = ScriptableObject.CreateInstance<MassAtkConatiner>();
                        conatiners.atkContainers = new List<AtkContainer>();
                        for (int i = 0; i < targetIndicies.Count; i++)
                        {
                            GridObject potentialTarget = GetObjectAtTile(currentAttackList[targetIndicies[i]]);
                            if (potentialTarget != null)
                            {
                                if (potentialTarget.GetComponent<LivingObject>())
                                {
                                    LivingObject target = potentialTarget.GetComponent<LivingObject>();
                                    if (skill.SUBTYPE == SubSkillType.Buff || skill.ELEMENT == Element.Support)
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
                                                return hitSomething;
                                            }
                                        }

                                        else
                                        {
                                            CreateTextEvent(this, target.FullName + " is not a valid target for " + skill.NAME, "validation text", CheckText, TextStart);
                                            PlayExitSnd();
                                            return hitSomething;
                                        }
                                    }
                                    else
                                    {
                                        if (target.FACTION != invokingObject.FACTION)
                                        {
                                            if (skill.SUBTYPE == SubSkillType.Debuff)
                                            {
                                                if (target.INVENTORY.DEBUFFS.Contains(skill))
                                                {
                                                    CreateTextEvent(this, skill.NAME + " is already applied to " + target.FullName, "validation text", CheckText, TextStart);
                                                    PlayExitSnd();
                                                    return hitSomething;

                                                }

                                            }

                                            acceptable = true;


                                        }
                                        else if (skill.RTYPE == RangeType.area)
                                        {
                                            acceptable = true;
                                        }
                                        else
                                        {
                                            //CreateTextEvent(this, target.FullName + " is not a valid target for " + skill.NAME, "validation text", CheckText, TextStart);
                                            //PlayExitSnd();
                                            //return hitSomething;
                                            acceptable = true;
                                        }

                                    }



                                }
                                else
                                {
                                    if (skill.SUBTYPE == SubSkillType.Buff || skill.ELEMENT == Element.Support || skill.SUBTYPE == SubSkillType.Ailment)
                                    {
                                        CreateTextEvent(this, potentialTarget.FullName + " is not a valid target for " + skill.NAME, "validation text", CheckText, TextStart);
                                        PlayExitSnd();
                                        return hitSomething;
                                    }
                                    else
                                    {
                                        acceptable = true;
                                    }
                                }

                            }
                            else
                            {

                            }


                        }
                        if (acceptable == true)
                        {
                            //skill.UseSkill(invokingObject, modification);
                            if (skill.SUBTYPE == SubSkillType.Charge)
                            {
                                //if (skill.OWNER.FATIGUE < 50)
                                //{
                                //    //skill.OWNER.ACTIONS++;
                                //    //skill.OWNER.GENERATED++;

                                //    if (skill.OWNER.GetComponent<EnemyScript>())
                                //    {
                                //        CreateEvent(skill.OWNER, null, "" + "determine action event ", skill.OWNER.GetComponent<EnemyScript>().DetermineNextAction, null, 0);

                                //    }

                                //    if (skill.OWNER.GetComponent<HazardScript>())
                                //    {
                                //        CreateEvent(skill.OWNER, null, "" + "determine action event ", skill.OWNER.GetComponent<HazardScript>().DetermineNextAction, null, 0);

                                //    }
                                //}

                            }
                            hitSomething = true;
                            if (flavor)
                            {
                                if (flavor.myOtherText != null)
                                {
                                    CreateTextEvent(this, "" + invokingObject.FullName + " used " + (skill.ETYPE == EType.physical ? " <sprite=0> " : " <sprite=1> ") + skill.NAME + " " + Common.GetElementSpriteIndex(skill.ELEMENT), "skill atk", CheckText, TextStart);
                                }
                                else
                                {
                                    CreateTextEvent(this, "" + invokingObject.FullName + " used " + skill.NAME, "skill atk", CheckText, TextStart);
                                }
                            }


                            if (log)
                            {
                                string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(invokingObject.FACTION)) + ">";
                                log.Log(coloroption + invokingObject.NAME + "</color> used " + skill.NAME);
                            }
                        }

                        HazardScript redirect = null;
                        for (int r = 0; r < gridObjects.Count; r++)
                        {
                            if (gridObjects[r].GetComponent<HazardScript>())
                            {
                                if ((gridObjects[r] as HazardScript).HTYPE == HazardType.redirect)
                                {
                                    CreateTextEvent(this, "A Glyph redirected your attack", "locked door", CheckText, TextStart);

                                    redirect = gridObjects[r] as HazardScript;
                                    break;
                                }
                            }
                        }

                        int dmgAmount = 0;
                        int totalDmg = 0;
                        Vector3 calcAnimationLocation = Vector3.zero;
                        for (int i = 0; i < targetIndicies.Count; i++)
                        {
                            int dmgHealth = 0;
                            if (acceptable == true)
                            {

                                if (redirect && invokingObject.FACTION != Faction.hazard)
                                {
                                    dmgHealth = redirect.HEALTH;
                                    //  totalHealth += redirect.HEALTH;
                                }
                                else
                                {
                                    GridObject gridzly = GetObjectAtTile(currentAttackList[targetIndicies[i]]);
                                    if (gridzly != null)
                                    {
                                        if (gridzly.GetComponent<LivingObject>())
                                        {
                                            LivingObject livzly = gridzly.GetComponent<LivingObject>();
                                            dmgHealth = livzly.HEALTH;
                                            //      totalHealth += livzly.HEALTH;
                                        }
                                        else
                                        {
                                            dmgHealth = gridzly.STATS.HEALTH;
                                            //   totalHealth += gridzly.BASE_STATS.HEALTH;
                                        }
                                    }
                                }

                                //     for (int k = 0; k < skill.HITS; k++)
                                //    {
                                // react = CalcDamage(invokingObject, target, skill);
                                // ApplyReaction(invokingObject, target, react);


                                // conatiners.atkConatiners.Add(conatiner);
                                if (skill.SUBTYPE == SubSkillType.RngAtk)
                                {
                                    skill.HITS = Random.Range(skill.MIN_HIT, skill.MAX_HIT + 1);
                                }

                                for (int j = 0; j < skill.HITS; j++)
                                {



                                    AtkContainer conatiner = ScriptableObject.CreateInstance<AtkContainer>();
                                    conatiner.alteration = skill.REACTION;
                                    conatiner.attackingElement = skill.ELEMENT;
                                    conatiner.attackType = skill.ETYPE;
                                    conatiner.attackingObject = invokingObject;
                                    conatiner.command = skill;
                                    conatiner.dmg = (int)skill.DAMAGE;
                                    if (redirect && invokingObject.FACTION != Faction.hazard)
                                    {
                                        conatiner.dmgObject = redirect;
                                    }
                                    else
                                    {
                                        conatiner.dmgObject = GetObjectAtTile(currentAttackList[targetIndicies[i]]);
                                        if (conatiner.dmgObject.GetComponent<EnemyScript>())
                                        {
                                            EnemyScript someEnemy = conatiner.dmgObject as EnemyScript;
                                            if (!someEnemy.sightedTargets.Contains(invokingObject))
                                            {
                                                someEnemy.sightedTargets.Add(invokingObject);
                                            }
                                        }
                                    }
                                    //if (conatiner.dmgObject)
                                    //{
                                    //    if (conatiner.dmgObject.GetComponent<EnemyScript>())
                                    //    {
                                    //        conatiner.dmgObject.GetComponent<EnemyScript>().CheckAttackRequirements(skill.ELEMENT, invokingObject);
                                    //    }
                                    //}

                                    DmgReaction react = CalcDamage(conatiner);
                                    react.usedSkill = skill;
                                    conatiner.react = react;
                                    conatiner.crit = false;

                                    if (j == 0)
                                    {
                                        if (react.reaction >= Reaction.resist && react.reaction <= Reaction.absorb)
                                        {
                                            skill.OWNER.GENERATED++;
                                        }
                                    }

                                    if (react.reaction < Reaction.nulled)
                                    {

                                        float critchance = invokingObject.STRENGTH * 0.5f; ;
                                        critchance += skill.CRIT_RATE;
                                        int chance = Random.Range(0, 100);
                                        if (critchance > chance)
                                        {
                                            //Debug.Log("Prior :" + react.damage);
                                            react.damage *= 2;
                                            //Debug.Log("After :" + react.damage);
                                            conatiner.react = react;
                                            conatiner.crit = true;
                                            CritAnnounceStart(invokingObject);
                                            // PlayOppSnd();
                                            // CreateEvent(this, conatiner.attackingObject, "Critical Announcement", OppAnnounceEvent, null, -1, OppAnnounceStart);
                                            CreateTextEvent(this, "" + conatiner.attackingObject.NAME + " landed a critical hit!", "crit", CheckText, TextStart, 0);
                                            if (log)
                                            {
                                                log.Log("\n Critial Hit!");
                                            }
                                        }
                                    }


                                    if (dmgHealth > 0)
                                    {

                                        dmgHealth -= react.damage;
                                        totalDmg += react.damage;

                                        dmgAmount = DetermineExp(invokingObject, conatiner.dmgObject, dmgHealth <= 0 ? true : false);
                                        opptargets.Add(conatiner.dmgObject);
                                        conatiners.atkContainers.Add(conatiner);
                                        CreateEvent(this, conatiner, "Skill use event", AttackEvent, null, 0);
                                        if (react.reaction >= Reaction.nulled && react.reaction <= Reaction.absorb)
                                        {
                                            break;
                                        }
                                    }

                                    if (dmgHealth <= 0)
                                    {

                                        if (skill.SPECIAL_EVENTS.Count > 0)
                                        {
                                            for (int k = 0; k < skill.SPECIAL_EVENTS.Count; k++)
                                            {
                                                SkillEventContainer sec = skill.SPECIAL_EVENTS[k];
                                                if (sec.theEvent == SkillEvent.onKill)
                                                {
                                                    CreateEvent(this, sec, "skill activation", ReturnTrue, null, -1, SkillActivation);
                                                    //skill.Activate(sec.theReaction, 0.0f, null);
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }




                                // }
                            }
                        }
                        bool giveExp = false;
                        for (int ac = 0; ac < conatiners.atkContainers.Count; ac++)
                        {
                            if (conatiners.atkContainers[ac].react.reaction != Reaction.missed)
                            {
                                giveExp = true;
                                if (skill.SPECIAL_EVENTS.Count > 0)
                                {
                                    for (int i = 0; i < skill.SPECIAL_EVENTS.Count; i++)
                                    {
                                        SkillEventContainer sec = skill.SPECIAL_EVENTS[i];
                                        if (sec.theEvent == SkillEvent.onHit)
                                        {
                                            CreateEvent(this, sec, "skill activation", ReturnTrue, null, -1, SkillActivation);

                                            // skill.Activate(sec.theReaction, 0.0f, null);
                                        }
                                    }
                                }
                                break;
                            }
                            else
                            {
                                if (skill.SPECIAL_EVENTS.Count > 0)
                                {
                                    for (int i = 0; i < skill.SPECIAL_EVENTS.Count; i++)
                                    {
                                        SkillEventContainer sec = skill.SPECIAL_EVENTS[i];
                                        if (sec.theEvent == SkillEvent.onMiss)
                                        {
                                            CreateEvent(this, sec, "skill activation", ReturnTrue, null, -1, SkillActivation);

                                            //  skill.Activate(sec.theReaction, 0.0f, null);
                                        }
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < gridObjects.Count; i++)
                        {
                            if (gridObjects[i].GetComponent<HazardScript>())
                            {
                                if ((gridObjects[i] as HazardScript).HTYPE == HazardType.zeroExp)
                                {
                                    giveExp = false;
                                }
                            }
                        }
                        if (giveExp == true)
                        {
                            bool leveledup = skill.GrantXP(1);
                        }
                        skill.UseSkill(invokingObject, modification);
                        if (skill.OWNER)
                        {
                            skill.OWNER.UpdateLastUsed(skill.ELEMENT);
                        }
                        if (skill.SPECIAL_EVENTS.Count > 0)
                        {
                            for (int i = 0; i < skill.SPECIAL_EVENTS.Count; i++)
                            {
                                SkillEventContainer sec = skill.SPECIAL_EVENTS[i];
                                if (sec.theEvent == SkillEvent.onUse)
                                {
                                    CreateEvent(this, sec, "skill activation", ReturnTrue, null, -1, SkillActivation);

                                    //skill.Activate(sec.theReaction, 0.0f, null);
                                }
                            }
                        }

                        if (targetIndicies.Count == 1)
                        {
                            if (redirect && invokingObject.FACTION != Faction.hazard)
                            {
                                calcAnimationLocation = invokingObject.currentTile.transform.position;
                            }
                            else
                            {
                                GridObject gridzly = GetObjectAtTile(currentAttackList[targetIndicies[0]]);
                                if (gridzly != null)
                                {
                                    calcAnimationLocation = gridzly.currentTile.transform.position;

                                }
                            }

                        }
                        else
                        {
                            if (redirect && invokingObject.FACTION != Faction.hazard)
                            {
                                calcAnimationLocation = invokingObject.currentTile.transform.position;
                            }
                            else
                            {
                                List<GridObject> targets = new List<GridObject>();
                                for (int i = 0; i < targetIndicies.Count; i++)
                                {
                                    GridObject gridzly = GetObjectAtTile(currentAttackList[targetIndicies[i]]);

                                    if (gridzly != null)
                                    {
                                        targets.Add(gridzly);
                                    }
                                }
                                if (targets.Count > 0)
                                {
                                    Vector3 initialpos = targets[0].transform.position;
                                    float minx = initialpos.x;
                                    float miny = initialpos.y;
                                    float minz = initialpos.z;
                                    float maxx = initialpos.x;
                                    float maxy = initialpos.y;
                                    float maxz = initialpos.z;
                                    for (int i = 0; i < targets.Count; i++)
                                    {
                                        Vector3 pos = targets[i].transform.position;
                                        if (minx > pos.x)
                                            minx = pos.x;
                                        if (miny > pos.y)
                                            miny = pos.y;
                                        if (minz > pos.z)
                                            minz = pos.z;

                                        if (maxx < pos.x)
                                            maxx = pos.x;
                                        if (maxy < pos.y)
                                            maxy = pos.y;
                                        if (maxz < pos.z)
                                            maxz = pos.z;
                                    }
                                    calcAnimationLocation = new Vector3((maxx + minx) * 0.5f, maxy, (maxz + minz) * 0.5f);
                                }
                            }
                        }

                        if (currentState != State.PlayerOppOptions)
                        {
                            if (giveExp == true)
                            {
                                TileScript invokingTile = invokingObject.currentTile;
                                if ((skill.EFFECT >= SideEffect.knockback && skill.EFFECT <= SideEffect.swap) || Common.isOverrideTile(invokingTile))
                                {
                                    AtkContainer sideContainer = ScriptableObject.CreateInstance<AtkContainer>();
                                    sideContainer.alteration = skill.REACTION;
                                    sideContainer.attackingElement = skill.ELEMENT;
                                    sideContainer.attackType = skill.ETYPE;
                                    sideContainer.attackingObject = invokingObject;
                                    sideContainer.command = skill;
                                    sideContainer.dmg = (int)skill.DAMAGE;
                                    if (redirect && invokingObject.FACTION != Faction.hazard)
                                    {
                                        sideContainer.dmgObject = redirect;
                                    }
                                    else
                                    {
                                        sideContainer.dmgObject = GetObjectAtTile(currentAttackList[targetIndicies[0]]);
                                    }

                                    DmgReaction sideReaction = new DmgReaction();
                                    sideReaction.damage = -1;


                                    if (Common.isOverrideTile(invokingTile))
                                    {
                                        sideReaction.reaction = Common.SpecialTileToReaction(invokingTile);
                                    }

                                    else
                                    {
                                        sideReaction.reaction = Common.EffectToReaction(skill.EFFECT);
                                    }

                                    sideContainer.react = sideReaction;

                                    CreateEvent(this, sideContainer, "apply reaction event", ApplyReactionEvent, null, 0);
                                }
                            }
                            if (invokingObject.FACTION == Faction.hazard)
                            {
                                AtkContainer sideContainer = ScriptableObject.CreateInstance<AtkContainer>();
                                sideContainer.alteration = skill.REACTION;
                                sideContainer.attackingElement = skill.ELEMENT;
                                sideContainer.attackType = skill.ETYPE;
                                sideContainer.attackingObject = invokingObject;
                                sideContainer.command = skill;
                                sideContainer.dmg = (int)skill.DAMAGE;
                                if (redirect && invokingObject.FACTION != Faction.hazard)
                                {
                                    sideContainer.dmgObject = redirect;
                                }
                                else
                                {
                                    sideContainer.dmgObject = GetObjectAtTile(currentAttackList[targetIndicies[0]]);
                                }

                                DmgReaction sideReaction = new DmgReaction();
                                sideReaction.damage = -1;
                                sideReaction.reaction = Reaction.knockback;
                            }


                        }

                        if (options)
                        {
                            if (options.showExp)
                            {
                                if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                                {
                                    if (dmgAmount > 0)
                                    {

                                    }
                                    //CreateEvent(this, null, "Exp event", UpdateExpBar, ShowExpBar);

                                }

                            }
                            else
                            {

                                if (invokingObject.BASE_STATS.EXP >= 100)
                                {
                                    // invokingObject.LevelUp();

                                }
                            }
                            //     if (skill.HITS > 1)
                            CreateGridAnimationEvent(calcAnimationLocation, skill, totalDmg);

                        }
                        bool show = ((GetState() == State.EnemyTurn) ? false : true);
                        DetermineAtkExp(invokingObject, GetObjectAtTile(currentAttackList[targetIndicies[0]]), skill, false, (int)skill.DAMAGE, show);
                        if (totalDmg > 0)
                        {
                            if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                                CreateEvent(this, conatiners, "check for opp event", CheckForOppChanceEvent);
                        }
                        if (oppAtk == false)
                            CreateEvent(this, invokingObject, "Show Command", player.ShowCmd);


                    }

                }
            }


            //if (GetState() != State.EnemyTurn || currentState != State.HazardTurn)
            //{
            //    SkillScript newSkill = skill.UseSkill(invokingObject, modification);
            //    if (newSkill != null)
            //    {

            //        LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
            //        learnContainer.attackingObject = invokingObject;
            //        learnContainer.usable = newSkill;
            //        CreateEvent(this, learnContainer, "New Skill Event", CheckNewSKillEvent, null, 0, NewSkillStart);
            //        CreateTextEvent(this, "" + invokingObject.FullName + " learned a new skill. Equip in inventory ", "new skill event", CheckText, TextStart);
            //    }
            //}
        }
        if (currentTutorial.steps.Count > 0)
        {
            if (currentTutorial.currentStep < currentTutorial.steps.Count)
            {
                tutorialStep curStep = currentTutorial.steps[currentTutorial.currentStep];
                if (curStep == tutorialStep.useSkill || curStep == tutorialStep.useSpell)
                {
                    DidCompleteTutorialStep();
                }
            }
        }
        return hitSomething;
    }

    public bool AttackTargets(LivingObject invokingObject, WeaponEquip weapon, bool oppAtk = false)
    {
        bool hitSomething = false;
        if (currentAttackList.Count > 0)
        {

            List<int> targetIndicies = GetTargetList();

            if (targetIndicies != null)
            {
                if (targetIndicies.Count > 0)
                {
                    if (expbar)
                    {
                        if (invokingObject)
                        {

                            expbar.currentUser = invokingObject;
                            expbar.slider.value = invokingObject.BASE_STATS.EXP;
                        }
                    }


                    HazardScript redirect = null;
                    for (int i = 0; i < gridObjects.Count; i++)
                    {
                        if (gridObjects[i].GetComponent<HazardScript>())
                        {
                            if ((gridObjects[i] as HazardScript).HTYPE == HazardType.redirect)
                            {
                                CreateTextEvent(this, "A Glyph redirected your attack", "locked door", CheckText, TextStart);

                                redirect = gridObjects[i] as HazardScript;
                                break;
                            }
                        }
                    }
                    int dmgHealth = 0;


                    if (flavor)
                    {
                        if (flavor.myOtherText != null)
                        {
                            CreateTextEvent(this, "" + invokingObject.FullName + " used " + " <sprite=4> " + weapon.NAME + " " + Common.GetElementSpriteIndex(weapon.ELEMENT), "skill atk", CheckText, TextStart);
                        }
                        else
                        {
                            CreateTextEvent(this, "" + invokingObject.FullName + " used " + weapon.NAME, "skill atk", CheckText, TextStart);
                        }
                    }


                    if (log)
                    {
                        log.Log(invokingObject.NAME + " used " + weapon.NAME);
                    }
                    MassAtkConatiner conatiners = ScriptableObject.CreateInstance<MassAtkConatiner>();
                    conatiners.atkContainers = new List<AtkContainer>();

                    int dmgAmount = 0;
                    int totalDmg = 0;
                    Vector3 calcAnimationLocation = Vector3.zero;

                    for (int i = 0; i < targetIndicies.Count; i++)
                    {
                        GridObject potentialTarget = GetObjectAtTile(currentAttackList[targetIndicies[i]]);
                        if (potentialTarget)
                        {
                            int currentTargetHealth = 0;
                            if (redirect && invokingObject.FACTION != Faction.hazard)
                            {
                                dmgHealth = redirect.HEALTH;
                            }
                            else
                            {
                                GridObject gridzly = potentialTarget;
                                if (gridzly != null)
                                {
                                    if (gridzly.GetComponent<LivingObject>())
                                    {
                                        LivingObject livzly = gridzly.GetComponent<LivingObject>();
                                        dmgHealth = livzly.HEALTH;
                                        currentTargetHealth = livzly.HEALTH;

                                    }
                                    else
                                    {
                                        dmgHealth = gridzly.STATS.HEALTH;
                                        currentTargetHealth = gridzly.STATS.HEALTH;

                                    }
                                }
                            }
                            //LivingObject target = potentialTarget.GetComponent<LivingObject>();

                            Reaction atkReaction = Reaction.none;
                            // if (potentialTarget.FACTION != invokingObject.FACTION)
                            {

                                hitSomething = true;

                                if (weapon != null && potentialTarget.GetType().IsSubclassOf(typeof(LivingObject)))
                                {
                                    // for (int k = 0; k < invokingObject.AUTO_SLOTS.SKILLS.Count; k++)
                                    {
                                        if (weapon.SEVENT == SkillEvent.beforeDmg)
                                        {
                                            // AutoSkill auto = (invokingObject.AUTO_SLOTS.SKILLS[k] as AutoSkill);
                                            float chance = weapon.CHANCE;
                                            float result = Random.value * 100;
                                            if (result <= chance)
                                            {
                                                //Debug.Log("Auto skill : " + auto.NAME + "has gone off");
                                                if (flavor)
                                                {
                                                    if (flavor.myOtherText != null)
                                                    {
                                                        CreateTextEvent(this, "<sprite=7> Auto skill from " + weapon.NAME + " activated!", "auto atk", CheckText, TextStart);
                                                    }
                                                    else
                                                    {
                                                        CreateTextEvent(this, "Auto skill from " + weapon.NAME + " activated!", "auto atk", CheckText, TextStart);
                                                    }
                                                }
                                                if (log)
                                                {
                                                    log.Log("Auto skill : " + weapon.NAME + " activated!");
                                                }
                                                atkReaction = weapon.EQUIPPED.Activate(weapon.SREACTION, 0, potentialTarget);
                                                CreateDmgTextEvent("<sprite=4> " + weapon.NAME, Color.red, weapon.EQUIPPED.USER);


                                                AtkContainer spc = ScriptableObject.CreateInstance<AtkContainer>();
                                                spc.alteration = atkReaction;
                                                spc.attackingElement = weapon.ELEMENT;
                                                spc.attackType = weapon.ATTACK_TYPE;
                                                spc.attackingObject = invokingObject;
                                                spc.command = null;
                                                spc.dmg = weapon.ATTACK;

                                                if (redirect && invokingObject.FACTION != Faction.hazard)
                                                {
                                                    spc.dmgObject = redirect;
                                                }
                                                else
                                                {
                                                    spc.dmgObject = potentialTarget;

                                                }


                                                //break;
                                            }
                                        }
                                    }
                                }
                                AtkContainer container = ScriptableObject.CreateInstance<AtkContainer>();
                                container.alteration = Reaction.none;
                                container.attackingElement = weapon.ELEMENT;
                                container.attackType = weapon.ATTACK_TYPE;
                                container.attackingObject = invokingObject;
                                container.command = null;
                                container.dmg = weapon.ATTACK;
                                container.strike = weapon.EQUIPPED;
                                if (redirect && invokingObject.FACTION != Faction.hazard)
                                {
                                    container.dmgObject = redirect;
                                }
                                else
                                {
                                    container.dmgObject = potentialTarget;

                                    if (potentialTarget.GetComponent<EnemyScript>())
                                    {
                                        EnemyScript someEnemy = potentialTarget as EnemyScript;
                                        if (!someEnemy.sightedTargets.Contains(invokingObject))
                                        {
                                            someEnemy.sightedTargets.Add(invokingObject);
                                        }
                                    }

                                }

                                //if (conatiner.dmgObject)
                                //{
                                //    if (conatiner.dmgObject.GetComponent<EnemyScript>())
                                //    {
                                //        conatiner.dmgObject.GetComponent<EnemyScript>().CheckAttackRequirements(weapon.ELEMENT, invokingObject);
                                //    }
                                //}

                                DmgReaction react = CalcDamage(container);
                                container.react = react;
                                container.crit = false;
                                if (react.reaction < Reaction.nulled)
                                {

                                    float critchance = invokingObject.STRENGTH * 0.5f; ;

                                    int chance = Random.Range(0, 100);
                                    if (critchance > chance)
                                    {
                                        //Debug.Log("Prior :" + react.damage);
                                        react.damage *= 2;
                                        //Debug.Log("After :" + react.damage);
                                        container.react = react;
                                        container.crit = true;
                                        CritAnnounceStart(invokingObject);
                                        // CreateEvent(this, conatiner.attackingObject, "Critical Announcement", OppAnnounceEvent, null, -1, OppAnnounceStart);
                                        CreateTextEvent(this, "" + container.attackingObject.NAME + " landed a critical hit!", "crit", CheckText, TextStart, 0);
                                        if (log)
                                        {
                                            log.Log("\n Critial Hit!");
                                        }
                                    }
                                }




                                dmgHealth -= react.damage;
                                currentTargetHealth -= react.damage;
                                totalDmg += react.damage;
                                dmgAmount += DetermineExp(invokingObject, container.dmgObject, (currentTargetHealth <= 0 ? true : false));


                                CreateEvent(this, container, "weapon use event", WeaponAttackEvent, null, 0);
                                conatiners.atkContainers.Add(container);



                            }


                        }

                    }
                    bool giveExp = false;
                    for (int ac = 0; ac < conatiners.atkContainers.Count; ac++)
                    {
                        if (conatiners.atkContainers[ac].react.reaction != Reaction.missed)
                        {
                            giveExp = true;
                            break;
                        }
                    }

                    for (int i = 0; i < gridObjects.Count; i++)
                    {
                        if (gridObjects[i].GetComponent<HazardScript>())
                        {
                            if ((gridObjects[i] as HazardScript).HTYPE == HazardType.zeroExp)
                            {
                                giveExp = false;
                            }
                        }
                    }
                    if (giveExp == true)
                    {
                        bool leveledup = weapon.EQUIPPED.GrantXP(1);
                    }
                    if (targetIndicies.Count == 1)
                    {
                        if (redirect && invokingObject.FACTION != Faction.hazard)
                        {
                            calcAnimationLocation = invokingObject.currentTile.transform.position;
                        }
                        else
                        {
                            GridObject gridzly = GetObjectAtTile(currentAttackList[targetIndicies[0]]);
                            if (gridzly != null)
                            {
                                calcAnimationLocation = gridzly.currentTile.transform.position;

                            }
                        }

                    }
                    else
                    {
                        if (redirect && invokingObject.FACTION != Faction.hazard)
                        {
                            calcAnimationLocation = invokingObject.currentTile.transform.position;
                        }
                        else
                        {
                            List<GridObject> targets = new List<GridObject>();
                            for (int i = 0; i < targetIndicies.Count; i++)
                            {
                                GridObject gridzly = GetObjectAtTile(currentAttackList[targetIndicies[i]]);

                                if (gridzly != null)
                                {
                                    targets.Add(gridzly);
                                }
                            }
                            if (targets.Count > 0)
                            {
                                Vector3 initialpos = targets[0].transform.position;
                                float minx = initialpos.x;
                                float miny = initialpos.y;
                                float minz = initialpos.z;
                                float maxx = initialpos.x;
                                float maxy = initialpos.y;
                                float maxz = initialpos.z;
                                for (int i = 0; i < targets.Count; i++)
                                {
                                    Vector3 pos = targets[i].transform.position;
                                    if (minx > pos.x)
                                        minx = pos.x;
                                    if (miny > pos.y)
                                        miny = pos.y;
                                    if (minz > pos.z)
                                        minz = pos.z;

                                    if (maxx < pos.x)
                                        maxx = pos.x;
                                    if (maxy < pos.y)
                                        maxy = pos.y;
                                    if (maxz < pos.z)
                                        maxz = pos.z;
                                }
                                calcAnimationLocation = new Vector3((maxx + minx) * 0.5f, maxy, (maxz + minz) * 0.5f);
                            }
                        }
                    }

                    if (currentState != State.PlayerOppOptions)
                    {
                        if (giveExp == true)
                        {
                            TileScript invokingTile = invokingObject.currentTile;
                            if (Common.isOverrideTile(invokingTile))
                            {
                                AtkContainer sideContainer = ScriptableObject.CreateInstance<AtkContainer>();
                                sideContainer.alteration = Reaction.none;
                                sideContainer.attackingElement = weapon.ELEMENT;
                                sideContainer.attackType = weapon.ATTACK_TYPE;
                                sideContainer.attackingObject = invokingObject;
                                sideContainer.strike = weapon.EQUIPPED;
                                sideContainer.dmg = (int)weapon.ATTACK;
                                if (redirect && invokingObject.FACTION != Faction.hazard)
                                {
                                    sideContainer.dmgObject = redirect;
                                }
                                else
                                {
                                    sideContainer.dmgObject = GetObjectAtTile(currentAttackList[targetIndicies[0]]);
                                }


                                DmgReaction sideReaction = new DmgReaction();
                                sideReaction.damage = -1;
                                sideReaction.reaction = Common.SpecialTileToReaction(invokingTile);
                                sideContainer.react = sideReaction;

                                CreateEvent(this, sideContainer, "apply reaction event", ApplyReactionEvent, null, 0);
                            }
                        }


                    }

                    //invokingObject.WEAPON.EQUIPPED.GrantXP(1);
                    invokingObject.WEAPON.Use();

                    invokingObject.UpdateLastUsed(weapon.ELEMENT);

                    if (options)
                    {
                        if (options.showExp)
                        {
                            if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                            {
                                if (dmgAmount > 0)
                                {
                                    //     Debug.Log("exp gain = " + dmgAmount);
                                    //CreateEvent(this, null, "Exp event", UpdateExpBar, ShowExpBar);
                                }

                            }

                        }
                        else
                        {

                            if (invokingObject.BASE_STATS.EXP >= 100)
                            {
                                //  invokingObject.LevelUp();

                            }
                        }

                        CreateGridAnimationEvent(calcAnimationLocation, weapon, totalDmg);
                        //if (options.battleAnims)
                        //{

                        //    GridAnimationObj gao = null;

                        //    gao = PrepareGridAnimation(null, calcAnimationLocation);

                        //    gao.type = (int)weapon.ELEMENT;
                        //    if (weapon.ELEMENT == Element.Pierce)
                        //        gao.subtype = (int)EType.natural;
                        //    gao.magnitute = totalDmg;
                        //    gao.LoadGridAnimation();
                        //    CreateEvent(this, gao, "Animation request: " + AnimationRequests + "", CheckAnimation, gao.StartCountDown, 0);



                        //}


                    }

                    if (dmgHealth > 0)
                    {
                        if (GetState() != State.EnemyTurn)
                            CreateEvent(this, conatiners, "check for opp event", CheckForOppChanceEvent);
                    }
                    if (oppAtk == false)
                        CreateEvent(this, invokingObject, "Show Command", player.ShowCmd);


                }


            }
        }
        if (currentTutorial.steps.Count > 0)
        {
            if (currentTutorial.currentStep < currentTutorial.steps.Count)
            {
                tutorialStep curStep = currentTutorial.steps[currentTutorial.currentStep];
                if (curStep == tutorialStep.useStrike)
                {
                    DidCompleteTutorialStep();
                }
            }
        }
        return hitSomething;
    }
    public bool OppAttackTargets(LivingObject target, LivingObject invokingObject, CommandSkill skill)
    {

        bool hitSomething = false;
        if (currentAttackList.Count > 0)
        {
            float modification = 1.0f;
            if (skill.ETYPE == EType.magical)
                modification = invokingObject.STATS.MANACHANGE;

            if (skill.ETYPE == EType.physical)
            {
                if (skill.COST > 0)
                {
                    modification = invokingObject.STATS.FTCHARGECHANGE;
                }
                else
                {
                    modification = invokingObject.STATS.FTCOSTCHANGE;
                }
            }


            //opptargets.Clear();


            if (skill != null)
            {
                MassAtkConatiner conatiners = ScriptableObject.CreateInstance<MassAtkConatiner>();
                conatiners.atkContainers = new List<AtkContainer>();


                if (skill.SUBTYPE == SubSkillType.Buff || skill.ELEMENT == Element.Support)
                {
                    if (target.FACTION == invokingObject.FACTION)
                    {
                        if (!target.INVENTORY.BUFFS.Contains(skill))
                        {

                        }
                        else
                        {
                            CreateTextEvent(this, skill.NAME + " is already applied to " + target.FullName, "validation text", CheckText, TextStart);
                            PlayExitSnd();
                            return hitSomething;
                        }
                    }

                    else
                    {
                        CreateTextEvent(this, target.FullName + " is not a valid target for " + skill.NAME, "validation text", CheckText, TextStart);
                        PlayExitSnd();
                        return hitSomething;
                    }
                }
                else
                {
                    if (target.FACTION != invokingObject.FACTION)
                    {
                        if (skill.SUBTYPE == SubSkillType.Debuff)
                        {
                            if (target.INVENTORY.DEBUFFS.Contains(skill))
                            {
                                CreateTextEvent(this, skill.NAME + " is already applied to " + target.FullName, "validation text", CheckText, TextStart);
                                PlayExitSnd();
                                return hitSomething;

                            }

                        }



                    }
                }







                hitSomething = true;
                if (flavor)
                {
                    if (flavor.myOtherText != null)
                    {
                        CreateTextEvent(this, "" + invokingObject.FullName + " used " + (skill.ETYPE == EType.physical ? " <sprite=0> " : " <sprite=1> ") + skill.NAME + " " + Common.GetElementSpriteIndex(skill.ELEMENT), "skill atk", CheckText, TextStart);
                    }
                    else
                    {
                        CreateTextEvent(this, "" + invokingObject.FullName + " used " + skill.NAME, "skill atk", CheckText, TextStart);
                    }
                }


                if (log)
                {
                    string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(invokingObject.FACTION)) + ">";
                    log.Log(coloroption + invokingObject.NAME + "</color> used " + skill.NAME);
                }


                HazardScript redirect = null;
                for (int r = 0; r < gridObjects.Count; r++)
                {
                    if (gridObjects[r].GetComponent<HazardScript>())
                    {
                        if ((gridObjects[r] as HazardScript).HTYPE == HazardType.redirect)
                        {
                            CreateTextEvent(this, "A Glyph redirected your attack", "locked door", CheckText, TextStart);

                            redirect = gridObjects[r] as HazardScript;
                            break;
                        }
                    }
                }

                int dmgAmount = 0;
                int totalDmg = 0;
                Vector3 calcAnimationLocation = Vector3.zero;

                int dmgHealth = 0;


                if (redirect && invokingObject.FACTION != Faction.hazard)
                {
                    dmgHealth = redirect.HEALTH;
                    //  totalHealth += redirect.HEALTH;
                }
                else
                {
                    dmgHealth = target.HEALTH;

                }


                if (skill.SUBTYPE == SubSkillType.RngAtk)
                {
                    skill.HITS = Random.Range(skill.MIN_HIT, skill.MAX_HIT + 1);
                }

                for (int j = 0; j < skill.HITS; j++)
                {



                    AtkContainer conatiner = ScriptableObject.CreateInstance<AtkContainer>();
                    conatiner.alteration = skill.REACTION;
                    conatiner.attackingElement = skill.ELEMENT;
                    conatiner.attackType = skill.ETYPE;
                    conatiner.attackingObject = invokingObject;
                    conatiner.command = skill;
                    conatiner.dmg = (int)skill.DAMAGE;
                    if (redirect && invokingObject.FACTION != Faction.hazard)
                    {
                        conatiner.dmgObject = redirect;
                    }
                    else
                    {
                        conatiner.dmgObject = target;
                        if (conatiner.dmgObject.GetComponent<EnemyScript>())
                        {
                            EnemyScript someEnemy = conatiner.dmgObject as EnemyScript;
                            if (!someEnemy.sightedTargets.Contains(invokingObject))
                            {
                                someEnemy.sightedTargets.Add(invokingObject);
                            }
                        }
                    }


                    DmgReaction react = CalcDamage(conatiner, false);
                    react.usedSkill = skill;
                    conatiner.react = react;
                    conatiner.crit = false;
                    if (react.reaction < Reaction.nulled)
                    {

                        float critchance = invokingObject.STRENGTH * 0.5f; ;
                        critchance += skill.CRIT_RATE;
                        int chance = Random.Range(0, 100);
                        if (critchance > chance)
                        {
                            //Debug.Log("Prior :" + react.damage);
                            react.damage *= 2;
                            //Debug.Log("After :" + react.damage);
                            conatiner.react = react;
                            conatiner.crit = true;
                            CritAnnounceStart(invokingObject);
                            // PlayOppSnd();
                            // CreateEvent(this, conatiner.attackingObject, "Critical Announcement", OppAnnounceEvent, null, -1, OppAnnounceStart);
                            CreateTextEvent(this, "" + conatiner.attackingObject.NAME + " landed a critical hit!", "crit", CheckText, TextStart, 0);
                            if (log)
                            {
                                log.Log("\n Critial Hit!");
                            }
                        }
                    }


                    if (dmgHealth > 0)
                    {

                        dmgHealth -= react.damage;
                        totalDmg += react.damage;

                        // dmgAmount = DetermineExp(invokingObject, conatiner.dmgObject, dmgHealth <= 0 ? true : false);
                        opptargets.Add(conatiner.dmgObject);
                        conatiners.atkContainers.Add(conatiner);
                        CreateEvent(this, conatiner, "Skill use event", AttackEvent, null, 0);
                        if (react.reaction >= Reaction.nulled && react.reaction <= Reaction.absorb)
                        {
                            break;
                        }
                    }

                    if (dmgHealth <= 0)
                    {

                        if (skill.SPECIAL_EVENTS.Count > 0)
                        {
                            for (int k = 0; k < skill.SPECIAL_EVENTS.Count; k++)
                            {
                                SkillEventContainer sec = skill.SPECIAL_EVENTS[k];
                                if (sec.theEvent == SkillEvent.onKill)
                                {
                                    CreateEvent(this, sec, "skill activation", ReturnTrue, null, -1, SkillActivation);
                                    //skill.Activate(sec.theReaction, 0.0f, null);
                                }
                            }
                        }
                        break;
                    }
                }




                // }


                bool giveExp = false;
                for (int ac = 0; ac < conatiners.atkContainers.Count; ac++)
                {
                    if (conatiners.atkContainers[ac].react.reaction != Reaction.missed)
                    {
                        giveExp = true;
                        if (skill.SPECIAL_EVENTS.Count > 0)
                        {
                            for (int i = 0; i < skill.SPECIAL_EVENTS.Count; i++)
                            {
                                SkillEventContainer sec = skill.SPECIAL_EVENTS[i];
                                if (sec.theEvent == SkillEvent.onHit)
                                {
                                    CreateEvent(this, sec, "skill activation", ReturnTrue, null, -1, SkillActivation);
                                    if (sec.theReaction == SkillReaction.doubleDmg)
                                    {
                                        conatiners.atkContainers[ac].dmg *= 2;
                                    }
                                    else if (sec.theReaction == SkillReaction.ailmentDmg)
                                    {
                                        if (conatiners.atkContainers[ac].dmgObject.GetComponent<LivingObject>())
                                        {
                                            LivingObject livedmgObj = conatiners.atkContainers[ac].dmgObject as LivingObject;
                                            for (int j = 0; j < livedmgObj.INVENTORY.EFFECTS.Count; j++)
                                            {
                                                conatiners.atkContainers[ac].dmg += (int)(((float)(conatiners.atkContainers[ac].command.DAMAGE)) * 0.5f);
                                            }
                                        }
                                    }
                                    else if (sec.theReaction == SkillReaction.ailmentDmg)
                                    {
                                        if (conatiners.atkContainers[ac].dmgObject.GetComponent<LivingObject>())
                                        {
                                            LivingObject livedmgObj = conatiners.atkContainers[ac].attackingObject;
                                            for (int j = 0; j < livedmgObj.INVENTORY.BUFFS.Count; j++)
                                            {
                                                conatiners.atkContainers[ac].dmg += (int)(((float)(conatiners.atkContainers[ac].command.DAMAGE)) * 0.5f);
                                            }
                                        }
                                    }

                                    // skill.Activate(sec.theReaction, 0.0f, null);
                                }
                            }
                        }
                        break;
                    }
                    else
                    {
                        if (skill.SPECIAL_EVENTS.Count > 0)
                        {
                            for (int i = 0; i < skill.SPECIAL_EVENTS.Count; i++)
                            {
                                SkillEventContainer sec = skill.SPECIAL_EVENTS[i];
                                if (sec.theEvent == SkillEvent.onMiss)
                                {
                                    CreateEvent(this, sec, "skill activation", ReturnTrue, null, -1, SkillActivation);

                                    //  skill.Activate(sec.theReaction, 0.0f, null);
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < gridObjects.Count; i++)
                {
                    if (gridObjects[i].GetComponent<HazardScript>())
                    {
                        if ((gridObjects[i] as HazardScript).HTYPE == HazardType.zeroExp)
                        {
                            giveExp = false;
                        }
                    }
                }
                if (giveExp == true)
                {
                    bool leveledup = skill.GrantXP(1);
                }
                skill.UseSkill(invokingObject, modification);
                if (skill.OWNER)
                {
                    skill.OWNER.UpdateLastUsed(skill.ELEMENT);
                }
                if (skill.SPECIAL_EVENTS.Count > 0)
                {
                    for (int i = 0; i < skill.SPECIAL_EVENTS.Count; i++)
                    {
                        SkillEventContainer sec = skill.SPECIAL_EVENTS[i];
                        if (sec.theEvent == SkillEvent.onUse)
                        {
                            CreateEvent(this, sec, "skill activation", ReturnTrue, null, -1, SkillActivation);

                            //skill.Activate(sec.theReaction, 0.0f, null);
                        }
                    }
                }


                if (redirect && invokingObject.FACTION != Faction.hazard)
                {
                    calcAnimationLocation = invokingObject.currentTile.transform.position;
                }
                else
                {
                    calcAnimationLocation = target.currentTile.transform.position;

                }




                if (currentState != State.PlayerOppOptions)
                {
                    if (giveExp == true)
                    {
                        TileScript invokingTile = invokingObject.currentTile;
                        if ((skill.EFFECT >= SideEffect.knockback && skill.EFFECT <= SideEffect.swap) || Common.isOverrideTile(invokingTile))
                        {
                            AtkContainer sideContainer = ScriptableObject.CreateInstance<AtkContainer>();
                            sideContainer.alteration = skill.REACTION;
                            sideContainer.attackingElement = skill.ELEMENT;
                            sideContainer.attackType = skill.ETYPE;
                            sideContainer.attackingObject = invokingObject;
                            sideContainer.command = skill;
                            sideContainer.dmg = (int)skill.DAMAGE;
                            if (redirect && invokingObject.FACTION != Faction.hazard)
                            {
                                sideContainer.dmgObject = redirect;
                            }
                            else
                            {
                                sideContainer.dmgObject = target;
                            }

                            DmgReaction sideReaction = new DmgReaction();
                            sideReaction.damage = -1;


                            if (Common.isOverrideTile(invokingTile))
                            {
                                sideReaction.reaction = Common.SpecialTileToReaction(invokingTile);
                            }

                            else
                            {
                                sideReaction.reaction = Common.EffectToReaction(skill.EFFECT);
                            }

                            sideContainer.react = sideReaction;

                            CreateEvent(this, sideContainer, "apply reaction event", ApplyReactionEvent, null, 0);
                        }
                    }
                    if (invokingObject.FACTION == Faction.hazard)
                    {
                        AtkContainer sideContainer = ScriptableObject.CreateInstance<AtkContainer>();
                        sideContainer.alteration = skill.REACTION;
                        sideContainer.attackingElement = skill.ELEMENT;
                        sideContainer.attackType = skill.ETYPE;
                        sideContainer.attackingObject = invokingObject;
                        sideContainer.command = skill;
                        sideContainer.dmg = (int)skill.DAMAGE;
                        if (redirect && invokingObject.FACTION != Faction.hazard)
                        {
                            sideContainer.dmgObject = redirect;
                        }
                        else
                        {
                            sideContainer.dmgObject = target;
                        }

                        DmgReaction sideReaction = new DmgReaction();
                        sideReaction.damage = -1;
                        sideReaction.reaction = Reaction.knockback;
                    }


                }

                if (options)
                {
                    if (options.showExp)
                    {
                        if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                        {
                            if (dmgAmount > 0)
                            {

                            }
                            //CreateEvent(this, null, "Exp event", UpdateExpBar, ShowExpBar);

                        }

                    }
                    else
                    {

                        if (invokingObject.BASE_STATS.EXP >= 100)
                        {
                            // invokingObject.LevelUp();

                        }
                    }
                    //     if (skill.HITS > 1)
                    CreateGridAnimationEvent(calcAnimationLocation, skill, totalDmg);

                }
                bool show = ((GetState() == State.EnemyTurn) ? false : true);
                //DetermineAtkExp(invokingObject, GetObjectAtTile(currentAttackList[targetIndicies[0]]), skill, false, (int)skill.DAMAGE, show);
                if (totalDmg > 0)
                {
                    if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                        CreateEvent(this, conatiners, "check for opp event", CheckForOppChanceEvent);
                }



            }



        }
        if (currentTutorial.steps.Count > 0)
        {
            if (currentTutorial.currentStep < currentTutorial.steps.Count)
            {
                tutorialStep curStep = currentTutorial.steps[currentTutorial.currentStep];
                if (curStep == tutorialStep.useSkill || curStep == tutorialStep.useSpell)
                {
                    DidCompleteTutorialStep();
                }
            }
        }
        return hitSomething;
    }

    public bool ApplyEffect(LivingObject target, SideEffect effect, float chance, CommandSkill skill = null)
    {

        bool usedEffect = false;
        float realChance = chance;
        float valres = Random.Range(0.0f, 100.0f);
        // Debug.Log("Chance: " + realChance + ", Reuslt: " + valres);
        if (skill)
        {
            if (skill.SUBTYPE == SubSkillType.Ailment)
                realChance = skill.ACCURACY; ;


        }
        if (currentTutorial.steps.Count > 0)
        {
            realChance = 100;
        }
        if (valres <= realChance)
        {

            switch (effect)
            {
                case SideEffect.hatch:
                    {

                        CreateTextEvent(this, "" + skill.OWNER.NAME + " used <sprite=1> Hatch <sprite=41>", "skill atk", CheckText, TextStart);

                        int storedLocation = skill.OWNER.currentTile.listindex;
                        List<int> possiblies = new List<int>();

                        possiblies.Add(0);
                        possiblies.Add(1);
                        possiblies.Add(3);
                        possiblies.Add(5);



                        EnemyScript anEnemy = skill.OWNER as EnemyScript;
                        gridObjects.Remove(anEnemy);
                        liveEnemies.Remove(anEnemy);
                        enemyManager.ReplaceEnemy(anEnemy, possiblies[Random.Range(0, 4)]);
                        liveEnemies.Add(anEnemy);

                        anEnemy.UpdateHealthbar();
                        anEnemy.ACTIONS = 0;
                        //gridObjects.Add(anEnemy);
                        //liveEnemies.Add(anEnemy);
                        SoftReset();

                        usedEffect = true;
                    }
                    break;
                case SideEffect.egg:
                    {
                        EnemyScript anEnemy = enemyManager.getNewEnemies(1, 8)[0];
                        liveEnemies.Add(anEnemy);
                        int index = 0;
                        System.Int32.TryParse(skill.extra, out index);
                        anEnemy.transform.position = tileMap[index].transform.position + new Vector3(0, 0.5f, 0);
                        anEnemy.gameObject.SetActive(true);
                        anEnemy.STATS.HEALTH = anEnemy.BASE_STATS.MAX_HEALTH;
                        anEnemy.STATS.MANA = anEnemy.BASE_STATS.MAX_MANA;
                        anEnemy.STATS.FATIGUE = 0;
                        anEnemy.MapIndex = index;
                        anEnemy.UpdateHealthbar();

                        SoftReset();
                        usedEffect = true;
                    }
                    break;
                case SideEffect.death:
                    {
                        KillGridObject(skill.OWNER, target);
                        CreateTextEvent(this, "Instant Death", "death atk", CheckText, TextStart);
                        usedEffect = true;
                    }
                    break;
                case SideEffect.knockback:
                    {
                        if (skill != null)
                        {
                            LivingObject attackingObject = skill.OWNER;
                            Vector3 direction = attackingObject.currentTile.transform.position - target.currentTile.transform.position;

                            direction.Normalize();
                            direction.x = Mathf.RoundToInt(direction.x);
                            direction.z = Mathf.RoundToInt(direction.z);
                            if (PushGridObject(target, (-1 * direction)))
                            {

                                ComfirmMoveGridObject(target, GetTileIndex(target));
                                CreateTextEvent(this, "" + target.FullName + " was knocked back", "knockback atk", CheckText, TextStart);
                            }
                        }


                    }
                    break;

                case SideEffect.cripple:
                    {
                        if (target.PSTATUS != PrimaryStatus.crippled && target.PSTATUS != PrimaryStatus.guarding)
                        {
                            target.PSTATUS = PrimaryStatus.crippled;

                            usedEffect = true;
                        }
                    }
                    break;
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
                            usedEffect = true;
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
                            usedEffect = true;
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
                            usedEffect = true;
                        }
                    }
                    break;
                case SideEffect.poison:
                    {
                        if (Common.LogicCheckStatus(StatusEffect.poisoned, target))
                        {
                            bool foundOne = false;
                            for (int i = 0; i < target.INVENTORY.EFFECTS.Count; i++)
                            {
                                EffectScript someEffect = target.INVENTORY.EFFECTS[i];
                                if (someEffect.EFFECT == SideEffect.poison)
                                {
                                    CreateTextEvent(this, target.FullName + " poison got worse", "auto atk", CheckText, TextStart);
                                    someEffect.TURNS += 1;
                                    foundOne = true;
                                    break;
                                }
                            }

                            if (foundOne == false)
                            {

                                EffectScript ef = PoolManager.GetManager().GetEffect();
                                ef.EFFECT = SideEffect.poison;
                                ef.TURNS = 2;
                                if (target.INVENTORY.EFFECTS.Contains(ef))
                                {
                                    //target.INVENTORY.EFFECTS[target.INVENTORY.EFFECTS.IndexOf(ef)].coun
                                }
                                CreateTextEvent(this, target.FullName + " has been inflicted with poison", "auto atk", CheckText, TextStart);
                                if (log)
                                {
                                    log.Log(target.FullName + " has been inflicted with poison");
                                }
                                target.INVENTORY.EFFECTS.Add(ef);

                                if (!target.INVENTORY.DEBUFFS.Contains(Common.CommonDebuffStr))
                                {
                                    CommandSkill strDebuff = Common.CommonDebuffStr;
                                    strDebuff.EFFECT = SideEffect.debuff;
                                    strDebuff.BUFF = BuffType.Str;
                                    strDebuff.BUFFVAL = -10f;
                                    strDebuff.ELEMENT = Element.Buff;
                                    strDebuff.SUBTYPE = SubSkillType.Debuff;
                                    strDebuff.OWNER = target;
                                    strDebuff.NAME = "Str Poison";
                                    target.INVENTORY.DEBUFFS.Add(strDebuff);


                                    DebuffScript buff = target.gameObject.AddComponent<DebuffScript>();
                                    buff.SKILL = strDebuff;
                                    buff.BUFF = strDebuff.BUFF;
                                    buff.COUNT = 2;
                                    target.UpdateBuffsAndDebuffs();
                                    target.INVENTORY.TDEBUFFS.Add(buff);

                                    usedEffect = true;
                                    target.updateAilmentIcons();
                                }
                            }
                        }
                    }
                    break;
                case SideEffect.confusion:
                    {
                        if (Common.LogicCheckStatus(StatusEffect.confused, target))
                        {
                            bool foundOne = false;
                            for (int i = 0; i < target.INVENTORY.EFFECTS.Count; i++)
                            {
                                EffectScript someEffect = target.INVENTORY.EFFECTS[i];
                                if (someEffect.EFFECT == SideEffect.confusion)
                                {
                                    CreateTextEvent(this, target.FullName + " confusion got worse", "auto atk", CheckText, TextStart);
                                    someEffect.TURNS += 1;
                                    foundOne = true;
                                    break;
                                }
                            }

                            if (foundOne == true)
                            {

                                CreateTextEvent(this, target.FullName + " has been inflicted with confusion", "auto atk", CheckText, TextStart);

                                EffectScript ef = PoolManager.GetManager().GetEffect();
                                ef.EFFECT = SideEffect.confusion;
                                target.INVENTORY.EFFECTS.Add(ef);

                                target.SSTATUS = SecondaryStatus.confusion;
                                SecondStatusScript sf = target.gameObject.AddComponent<SecondStatusScript>();
                                sf.COUNTDOWN = 2;
                                sf.STATUS = SecondaryStatus.confusion;
                                usedEffect = true;
                            }
                        }
                    }
                    break;
                case SideEffect.paralyze:
                    {
                        if (Common.LogicCheckStatus(StatusEffect.paralyzed, target))
                        {
                            bool foundOne = false;
                            for (int i = 0; i < target.INVENTORY.EFFECTS.Count; i++)
                            {
                                EffectScript someEffect = target.INVENTORY.EFFECTS[i];
                                if (someEffect.EFFECT == SideEffect.paralyze)
                                {
                                    CreateTextEvent(this, target.FullName + " paralysis got worse", "auto atk", CheckText, TextStart);
                                    someEffect.TURNS += 1;
                                    foundOne = true;
                                    break;
                                }
                            }
                            if (foundOne == false)
                            {

                                CreateTextEvent(this, target.FullName + " has been paralyzed", "auto atk", CheckText, TextStart);

                                EffectScript ef = PoolManager.GetManager().GetEffect();
                                ef.EFFECT = SideEffect.paralyze;
                                ef.TURNS = 2;
                                target.INVENTORY.EFFECTS.Add(ef);
                                usedEffect = true;
                            }
                        }
                    }
                    break;
                case SideEffect.sleep:
                    {
                        if (Common.LogicCheckStatus(StatusEffect.sleep, target))
                        {
                            bool foundOne = false;
                            for (int i = 0; i < target.INVENTORY.EFFECTS.Count; i++)
                            {
                                EffectScript someEffect = target.INVENTORY.EFFECTS[i];
                                if (someEffect.EFFECT == SideEffect.sleep)
                                {
                                    CreateTextEvent(this, target.FullName + " is sleeping longer", "auto atk", CheckText, TextStart);
                                    someEffect.TURNS += 1;
                                    foundOne = true;
                                    break;
                                }
                            }
                            if (foundOne == false)
                            {
                                CreateTextEvent(this, target.FullName + " has been inflicted with sleep", "auto atk", CheckText, TextStart);

                                EffectScript ef = PoolManager.GetManager().GetEffect();
                                ef.EFFECT = SideEffect.sleep;
                                ef.TURNS = 1;
                                target.INVENTORY.EFFECTS.Add(ef);
                                usedEffect = true;
                            }
                        }
                    }
                    break;
                case SideEffect.freeze:
                    {
                        if (Common.LogicCheckStatus(StatusEffect.frozen, target))
                        {
                            bool foundOne = false;
                            for (int i = 0; i < target.INVENTORY.EFFECTS.Count; i++)
                            {
                                EffectScript someEffect = target.INVENTORY.EFFECTS[i];
                                if (someEffect.EFFECT == SideEffect.freeze)
                                {
                                    CreateTextEvent(this, target.FullName + " is freezing longer", "auto atk", CheckText, TextStart);
                                    someEffect.TURNS += 1;
                                    foundOne = true;
                                    break;
                                }
                            }
                            if (foundOne == false)
                            {

                                CreateTextEvent(this, target.FullName + " has been frozen", "auto atk", CheckText, TextStart);

                                EffectScript ef = PoolManager.GetManager().GetEffect();
                                ef.EFFECT = SideEffect.freeze;
                                ef.TURNS = 1;
                                target.INVENTORY.EFFECTS.Add(ef);
                                usedEffect = true;
                            }
                        }
                    }
                    break;
                case SideEffect.burn:
                    {
                        if (Common.LogicCheckStatus(StatusEffect.burned, target))
                        {
                            bool foundOne = false;
                            for (int i = 0; i < target.INVENTORY.EFFECTS.Count; i++)
                            {
                                EffectScript someEffect = target.INVENTORY.EFFECTS[i];
                                if (someEffect.EFFECT == SideEffect.burn)
                                {
                                    CreateTextEvent(this, target.FullName + " burn got worse", "auto atk", CheckText, TextStart);
                                    someEffect.TURNS += 1;
                                    foundOne = true;
                                    break;
                                }
                            }
                            if (foundOne == false)
                            {
                                CreateTextEvent(this, target.FullName + " has been burned", "auto atk", CheckText, TextStart);

                                EffectScript ef = PoolManager.GetManager().GetEffect();
                                ef.TURNS = 2;
                                ef.EFFECT = SideEffect.burn;
                                target.INVENTORY.EFFECTS.Add(ef);
                                usedEffect = true;
                            }
                        }
                    }
                    break;

                case SideEffect.bleed:
                    if (Common.LogicCheckStatus(StatusEffect.bleeding, target))
                    {
                        bool foundOne = false;
                        for (int i = 0; i < target.INVENTORY.EFFECTS.Count; i++)
                        {
                            EffectScript someEffect = target.INVENTORY.EFFECTS[i];
                            if (someEffect.EFFECT == SideEffect.bleed)
                            {
                                CreateTextEvent(this, target.FullName + " bleeding got worse", "auto atk", CheckText, TextStart);
                                someEffect.TURNS += 1;
                                foundOne = true;
                                break;
                            }
                        }
                        if (foundOne == false)
                        {

                            CreateTextEvent(this, target.FullName + " started bleeding", "auto atk", CheckText, TextStart);

                            EffectScript ef = PoolManager.GetManager().GetEffect();
                            ef.EFFECT = SideEffect.bleed;
                            ef.TURNS = 2;
                            target.INVENTORY.EFFECTS.Add(ef);
                            usedEffect = true;
                        }
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
                                    skill.OWNER = target;
                                    target.INVENTORY.DEBUFFS.Add(skill);
                                    DebuffScript buff = target.gameObject.AddComponent<DebuffScript>();
                                    buff.SKILL = skill;
                                    buff.BUFF = skill.BUFF;
                                    buff.COUNT = 3;
                                    target.INVENTORY.TDEBUFFS.Add(buff);

                                    target.UpdateBuffsAndDebuffs();
                                    usedEffect = true;

                                    target.updateAilmentIcons();
                                }
                            }
                        }
                    }
                    break;


            }
        }
        if (target != null)
            target.updateAilmentIcons();
        return usedEffect;
    }
    public bool AttackEvent(Object data)
    {
        menuManager.ShowNone();
        loadTargets();
        AtkContainer container = data as AtkContainer;
        //CreateTextEvent(this, "" + container.attackingObject.FullName + " used " + container.command.NAME, "skill atk", CheckText, TextStart);
        CommandSkill skill = container.command;


        DmgReaction react = container.react;



        // for (int k = 0; k < skill.HITS; k++)
        {

            //  react = CalcDamage(container);


            container.alteration = react.reaction;
            react.usedSkill = container.command;

            if (react.reaction < Reaction.nulled && react.reaction != Reaction.missed)
            {
                //DetermineAtkExp(container.attackingObject, container.dmgObject, container.command, false, (int)container.command.DAMAGE);

                //  if (container.crit == true)
                {
                    //    PlayOppSnd();
                    // CreateEvent(this, container.attackingObject, "Critical Announcement", OppAnnounceEvent, null, 0, OppAnnounceStart);
                    //  CreateTextEvent(this, "" + container.attackingObject.NAME + " landed a critical hit!", "crit", CheckText, TextStart, 0);
                }
            }
            CreateEvent(this, container, "apply reaction event", ApplyReactionEvent, null, 0);
            if (react.reaction < Reaction.nulled)
            {
                if (container.dmgObject.GetComponent<LivingObject>())
                {
                    LivingObject newlive = container.dmgObject.GetComponent<LivingObject>();

                    if (container.command.EFFECT != SideEffect.none)
                    {
                        if (container.command.EFFECT == SideEffect.debuff)
                        {
                            ApplyEffect(newlive, container.command.EFFECT, (float)container.attackingObject.MAGIC * 0.5f + container.command.LEVEL, container.command);

                        }
                        else
                        {
                            if (container.command.SUBTYPE == SubSkillType.Ailment)
                            {
                                //     ApplyEffect(newlive, container.command.EFFECT, container.command.ACCURACY, container.command);
                            }

                            else
                            {
                                ApplyEffect(newlive, container.command.EFFECT, (float)container.command.DAMAGE * container.attackingObject.DEX);

                            }

                        }
                    }
                }
            }
            if (react.reaction == Reaction.debuff)
            {
                if (skill)
                {
                    if (container.dmgObject.GetComponent<LivingObject>())
                    {

                        LivingObject target = container.dmgObject.GetComponent<LivingObject>();
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
                                skill.OWNER = target;
                                target.INVENTORY.DEBUFFS.Add(skill);
                                DebuffScript buff = target.gameObject.AddComponent<DebuffScript>();
                                buff.SKILL = skill;
                                buff.BUFF = skill.BUFF;
                                buff.COUNT = 3;

                                target.INVENTORY.TDEBUFFS.Add(buff);
                                target.UpdateBuffsAndDebuffs();

                                target.updateAilmentIcons();
                            }
                        }
                    }
                }
            }

        }
        // currentState = State.PlayerTransition;
        enterStateTransition();

        return true;
    }
    public bool CheckForOppChanceEvent(Object data)
    {

        MassAtkConatiner containers = data as MassAtkConatiner;
        if (containers.atkContainers.Count == 0)
        {
            return true;
        }
        bool hit = false;
        bool oppAction = false;
        LivingObject firstResponder = null;
        for (int i = 0; i < containers.atkContainers.Count; i++)
        {
            AtkContainer container = containers.atkContainers[i];

            if (container.alteration < Reaction.nulled)
            {
                hit = true;

                if (!container.dmgObject.DEAD)
                {

                    if (!currOppList.Contains(container.attackingObject))
                    {
                        currOppList.Add(container.attackingObject);
                        List<CommandSkill> reactedSkills = null;
                        (doubleAdjOppTiles, reactedSkills) = GetOppViaDoubleAdjecentTiles(container.attackingObject, container.attackingElement, container.dmgObject);
                        if (doubleAdjOppTiles.Count > 0)
                        {
                            PlayOppSnd();
                            if (firstResponder == null)
                                firstResponder = GetObjectAtTile(doubleAdjOppTiles[0]) as LivingObject;
                            // tempObject.transform.position = doubleAdjOppTiles[0].transform.position;
                            //ComfirmMoveGridObject(tempGridObj, doubleAdjOppTiles[0]);
                            if (GetState() != State.EnemyTurn && GetState() != State.HazardTurn)
                            {
                                oppEvent.caller = this;
                                oppEvent = CreateEvent(this, firstResponder, "Opp Event", CheckOppEvent, null, 0, OppStart);
                                MoveCameraAndShow(GetObjectAtTile(doubleAdjOppTiles[0]));
                                container.attackingObject.OPP_SLOTS.SKILLS.Clear();
                                container.attackingObject.OPP_SLOTS.lastTarget = container.dmgObject as LivingObject;
                                CreateEvent(this, reactedSkills[0].OWNER, "Opp Announcement", OppAnnounceEvent, null, 0, OppAnnounceStart);
                                for (int j = 0; j < doubleAdjOppTiles.Count; j++)
                                {
                                    container.attackingObject.OPP_SLOTS.SKILLS.Add(reactedSkills[j]);
                                    //OppAttackTargets(container.dmgObject as LivingObject, reactedSkills[j].OWNER, reactedSkills[j]);

                                }
                            }



                            //   oppEvent = CreateEvent(this, null, "Opp Event", CheckOppEvent, OppStart);
                            // oppAction = true;

                        }


                    }



                }


                break;
            }
        }

        return true;
    }

    public bool ECheckForOppChanceEvent(Object data)
    {
        AtkContainer container = data as AtkContainer;
        if (!container.dmgObject)
        {
            Debug.Log("got a null here X(");
            return true;
        }
        if (!container.dmgObject.DEAD)
        {

            if (!currOppList.Contains(container.attackingObject))
            {
                currOppList.Add(container.attackingObject);

                //doubleAdjOppTiles = GetOppViaDoubleAdjecentTiles(container.attackingObject, container.attackingElement, container.dmgObject);
                if (doubleAdjOppTiles.Count > 0)
                {
                    //  EnemyScript enemy = GetObjectAtTile(doubleAdjOppTiles[0]).GetComponent<EnemyScript>();
                    //   if (enemy)
                    {
                        //enemy.ACTIONS++;
                        //oppEvent = CreateEvent(this, container.dmgObject, "Opp Event", enemy.EAtkEvent, enemy.AttackStart, 1);
                        //CreateEvent(this, enemy, "Opp Announcement", OppAnnounceEvent, null, 1, OppAnnounceStart);
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
        AtkContainer container = data as AtkContainer;
        //CreateTextEvent(this, "" + container.attackingObject.FullName + " used their " + container.attackingObject.WEAPON.NAME + " attack", "weapon atk", CheckText, TextStart);
        // DmgReaction react = CalcDamage(container);
        container.alteration = container.react.reaction;
        LivingObject invokingObject = container.attackingObject;


        //container.attackingObject.WEAPON.Use();

        // container.react = react;
        //  ApplyReaction(container.attackingObject, container.dmgObject, react, container.attackingElement);
        CreateEvent(this, container, "apply reaction event", ApplyReactionEvent, null, 0);
        if (container.react.reaction < Reaction.nulled && container.react.reaction != Reaction.missed)
        {
            bool show = ((GetState() == State.EnemyTurn) ? false : true);

            DetermineAtkExp(container.attackingObject, container.dmgObject, null, true, container.react.damage, show);
        }


        //currentState = State.PlayerTransition;
        // enterStateTransition();
        // bool oppAction = false;

        if (container.react.reaction < Reaction.nulled && container.react.reaction != Reaction.missed)
        {
            if (container.dmgObject.GetType().IsSubclassOf(typeof(LivingObject)))
            {
                //   for (int k = 0; k < invokingObject.AUTO_SLOTS.SKILLS.Count; k++)
                {
                    if (container.strike.SEVENT == SkillEvent.afterDmg)
                    {
                        //AutoSkill auto = (invokingObject.AUTO_SLOTS.SKILLS[k] as AutoSkill);
                        //    if (container.strike.SREACTION == SkillReaction.instaKill || container.strike.SREACTION == SkillReaction.debuff)
                        {
                            if (!container.dmgObject.GetComponent<LivingObject>())
                            {
                                // continue;
                            }
                            else
                            {
                                float chance = container.strike.CHANCE + invokingObject.DEX;
                                float result = Random.value * 100;
                                if (chance > result)
                                {
                                    CreateTextEvent(this, "" + "Auto skill : " + container.strike.NAME + " has gone off", "auto skill ", CheckText, TextStart);
                                    if (log)
                                    {
                                        log.Log("Auto skill : " + container.strike.NAME + " activated!");
                                    }
                                    container.strike.Activate(container.strike.SREACTION, container.react.damage, container.dmgObject);
                                }

                            }
                        }
                    }

                }
            }
            //if (!container.dmgObject.DEAD)
            //{

            //    if (!currOppList.Contains(container.attackingObject))
            //    {
            //        currOppList.Add(container.attackingObject);
            //        doubleAdjOppTiles = GetOppViaDoubleAdjecentTiles(container.attackingObject, container.attackingElement);

            //        if (doubleAdjOppTiles.Count > 0)
            //        {
            //            for (int i = 0; i < doubleAdjOppTiles.Count; i++)
            //            {

            //                CreateEvent(this, GetObjectAtTile(doubleAdjOppTiles[i]) as LivingObject, "Opp Announcement", OppAnnounceEvent, null, -1, OppAnnounceStart);
            //            }
            //            oppEvent = CreateEvent(this, null, "Opp Event", CheckOppEvent, OppStart);
            //            oppAction = true;
            //        }

            //    }

            //}

        }


        //if (oppAction == false)
        //{
        //    //if (currOppList.Count == 0)
        //    {
        //        oppEvent.caller = null;
        // CreateEvent(this, null, "Show Command", player.ShowCmd);

        //    }
        //}


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

    public void OppStart(Object data)
    {
        if (player.current)
        {
            SpriteRenderer sr = player.current.GetComponent<SpriteRenderer>();
            if (sr)
            {
                sr.color = Color.white;
            }
        }
        LivingObject firstResponder = data as LivingObject;
        //  StackNewSelection(State.PlayerOppSelecting, currentMenu.command);
        //   showOppAdjTiles();
        //   menuManager.ShowNone();

        if (firstResponder != null)
        {
            oppObj = firstResponder;
            //for (int i = 0; i < turnOrder.Count; i++)
            //{
            //    if (turnOrder[i] == firstResponder)
            //    {
            //        turnImgManger.UpdateSelection(i);
            //        currentObject = turnOrder[i];
            //        // player.current = 
            //        MoveCameraAndShow(turnOrder[i]);
            //        myCamera.currentTile = turnOrder[i].currentTile;
            //        myCamera.UpdateCamera();
            //        updateConditionals();
            //        //selectCharacter(i);
            //        break;
            //    }
            //}
        }
        StackOppSelection();
    }

    public void GotoNewRoom()
    {
        if (player.current)
        {
            TileScript tile = player.current.currentTile;
            if (tile)
            {
                if (tile.TTYPE == TileType.door)
                {

                    for (int i = 0; i < gridObjects.Count; i++)
                    {
                        if (gridObjects[i].GetComponent<HazardScript>())
                        {
                            if ((gridObjects[i] as HazardScript).HTYPE == HazardType.lockDoor)
                            {
                                CreateTextEvent(this, "A Lock Glyph is preventing you from leaving", "locked door", CheckText, TextStart);
                                PlayExitSnd();
                                return;
                            }
                        }
                    }

                    LoadDScene(tile.ROOM, tile.START);
                    CreateEvent(this, null, "return state event", BufferedCleanEvent);
                    CreateEvent(this, null, "next round event", NewRoomRoundBegin);
                }
            }
        }
    }

    public void DoorStart(Object data)
    {
        TileScript doorTile = data as TileScript;
        ChangeToDoorPrompt(doorTile);
        menuManager.ShowNewSkillPrompt();
        currentState = State.GotoNewRoom;
    }
    public void HelpStart(Object data)
    {
        LivingObject livvy = data as LivingObject;

        string[] helpParsed = livvy.currentTile.EXTRA.Split(';');

        if (helpParsed.Length < 3)
        {
            if (CheckAdjecentTilesGlyphs(player.current))
            {
                helpParsed = ("10;" + Common.GetHelpText(10)).Split(';');
            }
        }

        prompt.TitleText.text = helpParsed[1];
        prompt.BodyText.text = helpParsed[2];
        //prompt.choice1.text = "Cool";
        //prompt.choice2.text = "Watever";

        menuManager.ShowNewSkillPrompt();
        menuManager.ShowEventCanvas(1);

        currentState = State.EventRunning;
        newSkillEvent.data = livvy;
    }


    public void TutorialStart(Object data)
    {
        if (defaultSceneEntry == 20)
            return;
        SkillEventContainer tutorialExtra = data as SkillEventContainer;

        string[] helpParsed = tutorialExtra.name.Split(';');

        if (helpParsed.Length < 3)
        {
            if (CheckAdjecentTilesGlyphs(player.current))
            {
                helpParsed = ("10;" + Common.GetHelpText(10)).Split(';');
            }
        }

        prompt.TitleText.text = helpParsed[1];
        prompt.BodyText.text = helpParsed[2];
        //prompt.choice1.text = "Cool";
        //prompt.choice2.text = "Watever";

        menuManager.ShowNewSkillPrompt();
        menuManager.ShowEventCanvas(1);

        currentState = State.EventRunning;
        newSkillEvent.data = tutorialExtra;
    }
    public void EventStart(Object data)
    {
        GridObject griddy = data as GridObject;
        ChangeToEventPrompt(griddy.BASE_STATS.DEX, player.current);
        menuManager.ShowNewSkillPrompt();
        menuManager.ShowEventCanvas();
        currentState = State.EventRunning;
        newSkillEvent.data = griddy;
    }

    public void CheckingStart()
    {
        timer = 1.5f;
        enterStateTransition();
        menuManager.ShowNone();
        menuManager.ShowNewSkillAnnouncement();
    }
    public void SkillActivation(Object data)
    {
        SkillEventContainer sec = data as SkillEventContainer;

        sec.theSkill.Activate(sec.theReaction, 0.0f, null);
        if (flavor)
        {
            string eindx = Common.GetElementSpriteIndex((sec.theSkill as SkillScript).ELEMENT);
            if (flavor.myOtherText != null)
            {
                CreateTextEvent(this, eindx + " " + sec.theSkill.NAME + " activated!", "skill atk", CheckText, TextStart);
            }
            else
            {
                CreateTextEvent(this, "" + sec.theSkill.NAME + " activated! ", "skill atk", CheckText, TextStart);
            }
            CreateDmgTextEvent(eindx + " " + sec.theSkill.NAME, Color.green, sec.theSkill.USER);
        }

    }
    public void ComboActivation(Object data)
    {
        ComboSkill sec = data as ComboSkill;

        sec.Activate(SkillReaction.extraAction, 0.0f, null);
        if (flavor)
        {
            if (flavor.myOtherText != null)
            {
                CreateTextEvent(this, "<sprite=6> " + sec.NAME + " activated!", "skill atk", CheckText, TextStart);
            }
            else
            {
                CreateTextEvent(this, "" + sec.NAME + " activated! ", "skill atk", CheckText, TextStart);
            }
        }
    }
    public void NewSkillStart(Object data)
    {
        LearnContainer container = data as LearnContainer;
        newSkillEvent.caller = container.attackingObject;
        newSkillEvent.data = container.usable;
        if (newSkillEvent.data)
        {

            if (container.usable.GetType().IsSubclassOf(typeof(SkillScript)))
            {
                SkillScript newSkill = (SkillScript)container.usable;
                //ChangeSkillPrompt(container.attackingObject.FullName, newSkill);
            }
            else
            {
                // ChangeSkillPrompt(container.attackingObject.FullName, container.usable);
            }
            if (menuStack.Count > 1)
                StackNewSelection(State.AquireNewSkill, menuStack[menuStack.Count - 1].menu);
            else
                StackNewSelection(State.AquireNewSkill, currentMenu.none);
            if (container.usable.GetType() == typeof(WeaponScript))
            {
                menuManager.ShowItemCanvas(13, container.attackingObject);
            }
            else if (container.usable.GetType() == typeof(ArmorScript))
            {
                menuManager.ShowItemCanvas(1, container.attackingObject);
            }
            else if (container.usable.GetType() == typeof(ItemScript))
            {
                menuManager.ShowItemCanvas(11, container.attackingObject);
            }
            else if (container.usable.GetType() == typeof(ComboSkill))
            {
                menuManager.ShowItemCanvas(8, container.attackingObject);
            }
            else if (container.usable.GetType() == typeof(AutoSkill))
            {
                menuManager.ShowItemCanvas(9, container.attackingObject);
            }
            else if (container.usable.GetType() == typeof(OppSkill))
            {
                menuManager.ShowItemCanvas(3, container.attackingObject);
            }
            else if (container.usable.GetType() == typeof(CommandSkill))
            {
                if ((container.usable as CommandSkill).ETYPE == EType.magical)
                    menuManager.ShowItemCanvas(12, container.attackingObject);
                else
                    menuManager.ShowItemCanvas(7, container.attackingObject);
            }
            menuManager.ShowExtraCanvas(container.usable, container.attackingObject);
            //   menuManager.ShowNewSkillPrompt();
            //    currentState = State.AquireNewSkill;
        }
        else
        {
            Debug.Log("no data for new skill");
        }
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
    public void HackingStart(Object data)
    {
        myCamera.PlaySoundTrack(4);

        hackingGame.currentObj = player.current;
        StackNewSelection(State.EventRunning, currentMenu.command);
        hackingGame.isPlaying = true;
        hackingGame.gameObject.SetActive(true);
    }
    public bool CheckHacking(Object data)
    {
        if (hackingGame)
        {
            if (hackingGame.isPlaying)
                return false;
            else
            {
                if (hackingGame.won == true)
                {
                    if (adjacentGlyph != null)
                    {
                        // adjacentGlyph.BASE_STATS.HEALTH = 0;
                        adjacentGlyph.STATS.HEALTH = 0;
                        CheckForDeath(player.current, adjacentGlyph, true);



                        if (player.current.INVENTORY.ITEMS.Count < 6)
                        {
                            int inum = Random.Range(0, 25);
                            UsableScript useable = database.GetItem(99, player.current);

                            if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
                                CreateEvent(this, useable, "New Skill Event", CheckCount, null, 0, CountStart);
                            if (!player.current)
                            { Debug.Log("no attacker"); }
                            if (!useable)
                            { Debug.Log("no usable  for " + inum); }
                            // CreateTextEvent(this, "" + player.current.FullName + " gained " + useable.NAME, "new skill event", CheckText, TextStart);
                        }
                        else
                        {
                            UsableScript useable = database.GetItem(99, null);

                            LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                            learnContainer.attackingObject = player.current;
                            learnContainer.usable = useable;
                            CreateEvent(this, learnContainer, "New Skill Event", CheckNewSKillEvent, null, 0, NewSkillStart);
                        }
                    }
                }
                else
                {
                    PlayExitSnd();
                    returnState();
                }
                myCamera.PlayPreviousSoundTrack();
                if (player.current)
                    player.current.TakeAction();
                return true;
            }

        }
        else
        {
            return true;
        }
    }
    public void TimerStart()
    {
        timer = 1.5f;
    }
    public void CountStart(Object data)
    {
        UsableScript usable = data as UsableScript;
        Canvas canvas = menuManager.GetNewSkillCanvas();
        bool other = false;
        if (GetState() != State.EnemyTurn && currentState != State.HazardTurn)
        {
            if (usable.USER != null)
            {

                if (usable.USER.FACTION == Faction.ally)
                {

                    if (canvas)
                    {
                        if (canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>())
                        {
                            TMPro.TextMeshProUGUI tmp = canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                            string spritestr = "<sprite=";
                            if (usable.GetType() == typeof(WeaponScript))
                                spritestr += "4>";
                            else if (usable.GetType() == typeof(ArmorScript))
                                spritestr += "3>";
                            else if (usable.GetType() == typeof(ItemScript))
                                spritestr += "19>";
                            else if (usable.GetType().IsSubclassOf(typeof(SkillScript)) || usable.GetType() == typeof(SkillScript))
                            {
                                SkillScript skill = usable as SkillScript;
                                switch (skill.ELEMENT)
                                {
                                    case Element.Passive:
                                        spritestr += "6>";
                                        break;
                                    case Element.Opp:
                                        spritestr += "8>";
                                        break;
                                    case Element.Auto:
                                        spritestr += "7>";
                                        break;
                                    case Element.none:
                                        break;
                                    default:
                                        spritestr += "5>";
                                        break;
                                }
                            }

                            else
                            {
                                other = true;
                            }
                            PlayOppSnd();
                            if (other == false)
                            {
                                if (usable.LEVEL > 1)
                                {
                                    tmp.text = "";
                                    tmp.text = usable.NAME;
                                    LivingObject living = usable.USER;
                                    if (living != null)
                                    {
                                        string test = living.NAME;
                                        tmp.text = test;
                                        tmp.text = "" + usable.USER.NAME + " 's " + spritestr;
                                        tmp.text = "" + usable.USER.NAME + " 's " + spritestr + usable.NAME + " has leveled up!";
                                    }
                                }
                                else
                                {

                                    tmp.text = "" + usable.USER.NAME + " learned " + spritestr + usable.NAME;
                                    if (usable.GetType() == typeof(ItemScript))
                                        tmp.text = "" + usable.USER.NAME + " gained " + spritestr + usable.NAME;
                                }

                                if (currentState != State.ShopCanvas)
                                {

                                    enterStateTransition();
                                    menuManager.ShowNone();
                                }
                            }
                            else
                            {
                                tmp.text = usable.DESC;
                                enterStateTransition();
                                menuManager.ShowNone();
                            }
                            usable.UpdateDesc();
                            menuManager.ShowNewSkillAnnouncement();
                        }
                    }

                }
            }
        }
        timer = 1.2f;
    }
    public bool CheckCount(Object data)
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return false;

        }
        else
        {
            menuManager.DontShowNewSkillAnnoucement();
            showCurrentState();
            return true;
        }
    }

    public bool CheckDmgText(Object data)
    {
        DmgTextObj dto = data as DmgTextObj;
        return !dto.isShowing;
    }
    public void CritAnnounceStart(Object data)
    {
        LivingObject invokingObj = data as LivingObject;

        if (!invokingObj.GetComponent<ActorScript>())
        {
            return;
        }
        string shrtname = invokingObj.FullName;
        string[] subs = shrtname.Split(' ');
        shrtname = "";
        for (int i = 0; i < subs.Length; i++)
        {
            shrtname += subs[i];
        }
        AudioClip audio = Resources.LoadAll<AudioClip>(shrtname + "/Crit/")[0];

        if (audio)
        {
            // Debug.Log("found audio");
            // Debug.Log("found audio");
            //  CreateTextEvent(this, "Opportunity Found!", "opp announce text", CheckText, TextStart);
            if (options)
            {
                if (options.voices.volume > 0.0f)
                {
                    voiceManager.loadAudio(audio);
                    voiceManager.playSound();
                }
            }
        }



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
            // CreateTextEvent(this, "Opportunity Found!", "opp announce text", CheckText, TextStart);
            if (options)
            {
                if (options.voices.volume > 0.0f)
                {
                    voiceManager.loadAudio(audio);
                    voiceManager.playSound();
                }
            }
        }

        if (oppImage)
        {
            oppImage.gameObject.SetActive(true);
            oppImage.myImage.sprite = Resources.LoadAll<Sprite>(shrtname + "/Opp/")[0];
            oppImage.StartCountDown(audio.length);
            RectTransform rect = oppImage.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = new Vector2(Screen.width, rect.anchoredPosition.y);
                LeanTween.move(rect, new Vector2(0, rect.anchoredPosition.y), 0.2f);


            }

        }

    }

    public void MoveCameraAndShow(GridObject obj)
    {
        //  Debug.Log("moving cam");
        if (obj != null)
        {
            if (obj.currentTile != null)
            {

                tempObject.transform.position = obj.currentTile.transform.position;
                ComfirmMoveGridObject(tempGridObj, obj.currentTile);
                //if (GetObjectAtTile(tempGridObj.currentTile) != null)
                //    ShowGridObjectAffectArea(GetObjectAtTile(tempGridObj.currentTile));
                ShowSelectedTile(tempGridObj);
                //  Debug.Log(tempObject.transform.position);
            }
        }
    }

    public void DesperationUpdate(Object data)
    {
        LivingObject livvy = data as LivingObject;

        int acts = (int)(livvy.SPEED / 10);


        //acts += 3;
        acts = 0;



        livvy.ACTIONS = acts;

        if (Common.hasSeenDesperation == false)
        {
            List<tutorialStep> tSteps = new List<tutorialStep>();
            List<int> tClar = new List<int>();
            tSteps.Add(tutorialStep.showTutorial);
            tClar.Add(31);
            PrepareTutorial(tSteps, tClar, false);
            Common.hasLearnedFromEnemy = true;
            Common.hasSeenDesperation = true;
        }

    }
    public void PhaseAnnounceStart(Object data)
    {
        // Debug.Log("starting");
        LivingObject invokingObj = data as LivingObject;

        //Debug.Log("announcing  phase: " + currentState);
        MoveCameraAndShow(turnOrder[0]);

        if (phaseImage)
        {
            if (phaseImage.isShowing == false)
            {
                phaseImage.gameObject.SetActive(true);
                Color theColor;
                string turnText = "";
                switch (invokingObj.FACTION)
                {
                    case Faction.ally:
                        theColor = Color.cyan;
                        turnText = "Player Phase";
                        TextObjectHandler.UpdateText(textHolder.phaseTracker, " Goal");
                        TextObjectHandler.UpdateText(textHolder.shadowPhaseTracker, " Goal");
                        textHolder.phaseTracker.SetColor(Color.cyan);
                        break;
                    case Faction.enemy:
                        theColor = Color.red;
                        turnText = "Enemy Phase";
                        TextObjectHandler.UpdateText(textHolder.phaseTracker, "");
                        TextObjectHandler.UpdateText(textHolder.shadowPhaseTracker, "");
                        textHolder.phaseTracker.SetColor(Color.red);
                        break;
                    case Faction.hazard:
                        theColor = Color.yellow;
                        turnText = "Glyph Phase";
                        TextObjectHandler.UpdateText(textHolder.phaseTracker, "Glyph Phase");
                        TextObjectHandler.UpdateText(textHolder.shadowPhaseTracker, "Glyph Phase");
                        textHolder.phaseTracker.SetColor(Color.yellow);
                        break;
                    case Faction.ordinary:
                        theColor = Common.orange;
                        turnText = "Unknown Phase";
                        break;
                    case Faction.eventObj:
                        theColor = Color.magenta;
                        turnText = "Event Phase";
                        break;
                    case Faction.fairy:
                        theColor = Color.magenta;
                        turnText = "Fairy Phase";
                        TextObjectHandler.UpdateText(textHolder.phaseTracker, "Enemy Phase");
                        TextObjectHandler.UpdateText(textHolder.shadowPhaseTracker, "Enemy Phase");
                        textHolder.phaseTracker.SetColor(Color.red);
                        break;
                    case Faction.genie:
                        {
                            theColor = Color.green;
                            turnText = "Genie Phase";
                            TextObjectHandler.UpdateText(textHolder.phaseTracker, "Genie Phase");
                            TextObjectHandler.UpdateText(textHolder.shadowPhaseTracker, "Genie Phase");
                            textHolder.phaseTracker.SetColor(Color.yellow);
                        }
                        break;
                    case Faction.vamprretti:
                        {
                            theColor = Color.black;
                            turnText = "Vampretti Phase";
                            TextObjectHandler.UpdateText(textHolder.phaseTracker, "Vampretti Phase");
                            TextObjectHandler.UpdateText(textHolder.shadowPhaseTracker, "Vampretti Phase");
                            textHolder.phaseTracker.SetColor(Color.yellow);
                        }
                        break;
                    case Faction.revenant:
                        {
                            theColor = Color.blue;
                            turnText = "Revenant Phase";
                            TextObjectHandler.UpdateText(textHolder.phaseTracker, "Revenant Phase");
                            TextObjectHandler.UpdateText(textHolder.shadowPhaseTracker, "Revenant Phase");
                            textHolder.phaseTracker.SetColor(Color.yellow);
                        }
                        break;
                    case Faction.antileon:
                        {
                            theColor = Common.orange;
                            turnText = "Antileon Phase";
                            TextObjectHandler.UpdateText(textHolder.phaseTracker, "Antileon Phase");
                            TextObjectHandler.UpdateText(textHolder.shadowPhaseTracker, "Antileon Phase");
                            textHolder.phaseTracker.SetColor(Color.yellow);
                        }
                        break;
                    case Faction.thieves:
                        {
                            theColor = Color.grey;
                            turnText = "Theif Phase";
                            TextObjectHandler.UpdateText(textHolder.phaseTracker, "Theif Phase");
                            TextObjectHandler.UpdateText(textHolder.shadowPhaseTracker, "Theif Phase");
                            textHolder.phaseTracker.SetColor(Color.yellow);
                        }
                        break;
                    default:
                        Debug.Log("Ya done goofed");
                        theColor = Color.white;
                        break;
                }
                phaseImage.StartCountDown(theColor, turnText);
            }

        }

    }

    public bool PhaseAnnounce(Object data)
    {

        if (phaseImage)
        {
            return !phaseImage.isShowing;
        }

        return true;
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

    public bool CheckNewSKillEvent(Object data)
    {


        if (newSkillEvent.caller)
        {
            return false;

        }

        return true;
    }

    public bool CheckIfIntutorialMenu(Object data)
    {
        return !inTutorialMenu;
    }

    public bool NewRoomRoundBegin(Object data)
    {
        //considering this is the only time used
        currentState = State.HazardTurn;
        doubleAdjOppTiles.Clear();
        //  Debug.Log("At next round begin");
        if (nextRoundCalled == false)
        {

            nextRoundCalled = true;
            NextRound();
        }
        //player.current = turnOrder[0];
        //CreateEvent(this, turnOrder[0], "Initial Camera Event", CameraEvent);
        return true;
    }
    public bool ApplyReactionEvent(Object data)
    {

        AtkContainer conatiner = data as AtkContainer;

        ApplyReaction(conatiner.attackingObject, conatiner.dmgObject, conatiner.react, conatiner.attackingElement, conatiner.command, conatiner.crit);

        return true;
    }
    public bool NextTurnEvent(Object data)
    {

        //                Debug.Log(" is done with their turn, moving on ");
        //currOppList.Clear();

        if (currentState != State.PlayerTransition)
            doubleAdjOppTiles.Clear();


        if (GetState() != State.EnemyTurn && currentState != State.HazardTurn && currentState != State.PlayerTransition)
        {
            //    Debug.Log("Next turn...");
            CleanMenuStack(true, false);
            currentState = State.FreeCamera;
        }
        else
        {
            // while (turnOrder.Count > 0)
            // {
            //       turnOrder.Remove(turnOrder[0]);
            // }
        }
        bool nextround = true;
        for (int i = 0; i < turnOrder.Count; i++)
        {
            // Debug.Log("checking: " + turnOrder[i].FullName + " with ap: " + turnOrder[i].ACTIONS);
            if (turnOrder[i].ACTIONS > 0)
            {

                //if (SetGridObjectPosition(tempGridObj, turnOrder[i].currentTile.transform.position) == true)
                {
                    tempGridObj.transform.position = turnOrder[i].currentTile.transform.position;
                    ComfirmMoveGridObject(tempGridObj, turnOrder[i].currentTile.listindex);
                    myCamera.currentTile = turnOrder[i].currentTile;
                    myCamera.UpdatePosition(true);
                    //Debug.Log("going to next in turn: " + turnOrder[i].FullName);
                    // break;
                }
                nextround = false;
                //break;
            }
            else
            {
                turnImgManger.UpdateInteractivity(i);
            }
        }
        //     Debug.Log(" turn count: " + turnOrder.Count);
        if (nextround == true)
        {
            //  Debug.Log("next round from end turn in " + currentState + "  by?: " + currentObject.NAME);
            if (nextRoundCalled == false)
            {

                nextRoundCalled = true;
                NextRound();
                ShowWhite();

                for (int i = 0; i < turnOrder.Count; i++)
                {
                    ShowSelectedTile(turnOrder[i], Common.orange);

                }

            }
        }
        myCamera.UpdateCamera();
        // currentObject = turnOrder[0];

        //    player.current = turnOrder[0];
        //CleanMenuStack();

        // CreateEvent(this, turnOrder[0], "Next turn Camera Event", CameraEvent);
        return true;
    }
    public bool BufferedCamUpdate(Object data)
    {
        myCamera.UpdateCamera();
        showCurrentState();
        return true;
    }
    public bool BufferedReturnEvent(Object data)
    {
        returnState();
        myCamera.UpdateCamera();
        updateConditionals();
        return true;
    }
    public bool BufferedStateChange(Object data)
    {

        currentState = prevState;
        myCamera.UpdateCamera();
        updateConditionals();
        return true;
    }

    public bool BufferedNextRound(Object data)
    {

        NextRound();
        updateConditionals();
        return true;
    }
    public bool BufferedCleanEvent(Object data)
    {
        //  Debug.Log("buffered clean event");
        CleanMenuStack(true);
        myCamera.UpdateCamera();

        return true;
    }
    public bool OptionsScreenEvent(object data)
    {
        return currentState != State.ChangeOptions;
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

    public void StackShop()
    {
        if (GetState() == State.EnemyTurn || GetState() == State.HazardTurn)
        {
            return;
        }

        if (stackManager)
        {
            if (currentState != State.ShopCanvas)
            {
                myCamera.PlaySoundTrack(6);
                menuStackEntry shop = stackManager.GetShopStack();
                if (menuStack.Count > 1)
                    shop.menu = currentMenu.act;
                else
                    shop.menu = currentMenu.command;
                shop.index = invManager.currentIndex;
                enterState(shop);
                menuManager.ShowShop();
                if (shopScreen)
                {
                    shopScreen.SHOPITEMS = shopItems;
                    shopScreen.loadShop(player.current);
                }
            }
            else
            {

                myCamera.PlayPreviousSoundTrack();
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
    public void forceEnd()
    {
        for (int i = 0; i < turnOrder.Count; i++)
        {
            if (turnOrder[i].ACTIONS > 0)
            {
                turnOrder[i].Wait();
            }
        }
        NextTurn("manager");
    }


    public void StackOptions()
    {
        if (currentState == State.SceneRunning)
        {
            return;
        }
        if (stackManager)
        {
            if (currentState != State.ChangeOptions && currentState != State.PlayerTransition)
            {

                menuStackEntry playerOptions = stackManager.GetOptionsStack();

                playerOptions.index = invManager.currentIndex;
                enterState(playerOptions);
                menuManager.ShowOptions();
                CreateEvent(this, null, "options", OptionsScreenEvent, null, 0);
            }
            else
            {
                if (menuStack.Count == 0)
                {
                    menuManager.ShowNone();
                    if (prevState != State.PlayerTransition)
                    {
                        currentState = prevState;
                    }

                    else if (turnOrder.Count > 0)
                    {
                        switch (turnOrder[0].FACTION)
                        {
                            case Faction.ally:
                                currentState = State.FreeCamera;
                                break;
                            case Faction.enemy:
                                currentState = State.EnemyTurn;
                                break;
                            case Faction.hazard:
                                currentState = State.HazardTurn;
                                break;

                            case Faction.fairy:
                                currentState = State.FairyPhase;
                                break;
                            default:
                                currentState = State.FreeCamera;
                                break;
                        }


                    }
                    else
                    {
                        currentState = State.FreeCamera;
                    }
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
        if (GetState() == State.EnemyTurn || GetState() == State.HazardTurn)
        {
            return;
        }

        if (stackManager)
        {
            if (currentState != State.CheckDetails)
            {
                if (myCamera.infoObject.GetComponent<LivingObject>())
                {
                    detailsScreen.anotherObj = null;
                    detailsScreen.currentObj = myCamera.infoObject.GetComponent<LivingObject>();
                }
                else
                {
                    detailsScreen.currentObj = null;
                    detailsScreen.anotherObj = myCamera.infoObject.GetComponent<GridObject>();
                }
                // detailsScreen.currentObj = myCamera.infoObject.GetComponent<LivingObject>();
                menuStackEntry playerDetails = stackManager.GetDetailStack();
                if (GetState() == State.FreeCamera)
                {
                    playerDetails.menu = currentMenu.none;
                }
                else
                {
                    playerDetails.menu = currentMenu.command;
                }
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

        updateConditionals();
    }
    public void StackOppSelection()
    {

        if (stackManager)
        {
            menuStackEntry oppSelection = stackManager.GetOppSelectionStack();

            oppSelection.index = invManager.currentIndex;
            enterState(oppSelection);
            menuManager.showOpportunityOptions(currentObject.GetComponent<LivingObject>());
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
    public void YesPrompt()
    {
        if (GetState() == State.AquireNewSkill)
        {
            GotoSpecialEquip();
        }
        else if (GetState() == State.GotoNewRoom)
        {
            int roomNum = -1;
            int startindex = -1;
            string[] parsed = newSkillEvent.name.Split(',');
            if (int.TryParse(parsed[0], out roomNum))
            {
                if (int.TryParse(parsed[1], out startindex))
                {
                    LoadDScene(roomNum, startindex);
                }
            }
            CreateEvent(this, null, "return state event", BufferedCleanEvent);
            newSkillEvent.caller = null;

            //NextRound();
            CreateEvent(this, null, "next round event", NewRoomRoundBegin);
        }
        else if (GetState() == State.EventRunning)
        {
            if (newSkillEvent.data == null)
                return;
            GridObject griddy = newSkillEvent.data as GridObject;
            if (griddy)
            {
                if (griddy.GetComponent<LivingObject>())
                {
                    newSkillEvent.data = null;
                    newSkillEvent.caller = null;
                    return;
                }
                switch (griddy.BASE_STATS.DEX)
                {
                    case 1:
                        {
                            ApplyEvent1Choice1();
                            CreateEvent(this, null, "return state event", BufferedCleanEvent);
                            newSkillEvent.data = null;
                            newSkillEvent.caller = null;
                        }
                        break;
                    case 2:
                        {
                            ApplyEvent1Choice2();
                            CreateEvent(this, null, "return state event", BufferedCleanEvent);
                            newSkillEvent.data = null;
                            newSkillEvent.caller = null;
                        }
                        break;
                    case 3:
                        {
                            ApplyEvent2();
                            CreateEvent(this, null, "return state event", BufferedCleanEvent);
                            newSkillEvent.data = null;
                            newSkillEvent.caller = null;
                        }
                        break;

                }
            }

        }
    }

    public void CleanUpGridObject(GridObject griddy)
    {
        griddy.STATS.HEALTH = 0;
        griddy.Die();
        if (griddy.MapIndex >= 0 && currentMap.objMapIndexes.Contains(griddy.MapIndex))
        {
            for (int i = 0; i < currentMap.objMapIndexes.Count; i++)
            {
                if (griddy.MapIndex == currentMap.objMapIndexes[i])
                {
                    currentMap.objMapIndexes.Remove(griddy.MapIndex);
                    currentMap.objIds.RemoveAt(i);
                    break;
                }
            }

        }
    }
    public void NoPrompt()
    {
        if (GetState() == State.AquireNewSkill)
        {
            returnState();
            newSkillEvent.caller = null;
        }
        else if (GetState() == State.GotoNewRoom)
        {
            returnState();
            newSkillEvent.caller = null;
        }
        else if (GetState() == State.EventRunning)
        {
            returnState();

        }
    }


    public void ApplyEvent1Choice1()
    {
        player.current.BASE_STATS.MAGIC += 10;
        player.current.BASE_STATS.STRENGTH -= 10;
        if (player.current.BASE_STATS.STRENGTH < 0)
        {
            player.current.BASE_STATS.STRENGTH = 0;
        }
        GridObject griddy = newSkillEvent.data as GridObject;
        if (griddy)
        {
            CleanUpGridObject(griddy);

        }
        newSkillEvent.data = null;
        Canvas canvas = menuManager.GetNewSkillCanvas();
        if (canvas)
        {
            TMPro.TextMeshProUGUI tmp = canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            tmp.text = "" + player.current.FullName + " <br> Magic <sprite=1> + 10 <br> Strength <sprite=0> - 10";
            enterStateTransition();
            menuManager.ShowNone();
            menuManager.ShowNewSkillAnnouncement();
        }

        CreateEvent(this, null, "New Skill Event", CheckCount, TimerStart);
    }
    public void ApplyEvent1Choice2()
    {
        player.current.BASE_STATS.STRENGTH += 10;
        player.current.BASE_STATS.MAGIC -= 10;
        if (player.current.BASE_STATS.MAGIC < 0)
        {
            player.current.BASE_STATS.MAGIC = 0;
        }
        GridObject griddy = newSkillEvent.data as GridObject;
        if (griddy)
        {
            CleanUpGridObject(griddy);
        }
        newSkillEvent.data = null;

        Canvas canvas = menuManager.GetNewSkillCanvas();
        if (canvas)
        {
            TMPro.TextMeshProUGUI tmp = canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            tmp.text = "" + player.current.FullName + "<br> Strength <sprite=0> + 10 <br> Magic <sprite=1> - 10";
            enterStateTransition();
            menuManager.ShowNone();
            menuManager.ShowNewSkillAnnouncement();
        }

        CreateEvent(this, null, "New Skill Event", CheckCount, TimerStart);
    }

    public void ApplyEvent2()
    {
        visitedMaps.Clear();
        for (int i = 0; i < tileMap.Count; i++)
        {
            if (tileMap[i].canBeOccupied == false)
            {
                tileMap[i].canBeOccupied = true;
                tileMap[i].gameObject.SetActive(true);
            }
        }
        newSkillEvent.data = null;

        Canvas canvas = menuManager.GetNewSkillCanvas();
        if (canvas)
        {
            TMPro.TextMeshProUGUI tmp = canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            tmp.text = "Enemies, items, and other things have been reset.";
            enterStateTransition();
            menuManager.ShowNone();
            menuManager.ShowNewSkillAnnouncement();
        }

        CreateEvent(this, null, "New Skill Event", CheckCount, TimerStart);
    }

    public void RevealHiddenTileEvent(HazardScript revealer)
    {
        if (revealer)
        {
            if (revealer.revealTiles != null)
            {
                for (int i = 0; i < revealer.revealTiles.Count; i++)
                {
                    int checknum = revealer.revealTiles[i];
                    if (checknum < tileMap.Count)
                    {
                        if (tileMap[checknum].canBeOccupied == false)
                        {
                            tileMap[checknum].canBeOccupied = true;
                            if (currentMap.unOccupiedIndexes.Contains(checknum))
                            {
                                currentMap.unOccupiedIndexes.Remove(checknum);
                            }
                            tileMap[checknum].gameObject.SetActive(true);
                        }
                    }
                }
                newSkillEvent.data = null;

                Canvas canvas = menuManager.GetNewSkillCanvas();
                if (canvas)
                {
                    TMPro.TextMeshProUGUI tmp = canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                    tmp.text = "Hidden Tiles have been revealed!";

                }

                CreateEvent(this, null, "New Skill Event", CheckCount, CheckingStart);
            }
        }

    }

    public void GotoSpecialEquip()
    {
        int ItemIndex = 0;
        int extraIndex = 0;
        LivingObject living = newSkillEvent.caller as LivingObject;
        SkillScript skill = newSkillEvent.data as SkillScript;
        if (skill)
        {

            if (skill.GetType() == typeof(CommandSkill))
            {
                ItemIndex = 5;
                extraIndex = 0;
            }
            else if (skill.GetType() == typeof(ComboSkill))
            {
                ItemIndex = 6;
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
        }
        else
        {
            if (newSkillEvent.data.GetType() == typeof(WeaponScript))
            {
                ItemIndex = 0;
                extraIndex = 4;
            }
            else if (newSkillEvent.data.GetType() == typeof(ArmorScript))
            {
                ItemIndex = 1;
                extraIndex = 5;
            }
            StackNewSelection(State.PlayerEquipping, currentMenu.none);

        }

        menuManager.ShowItemCanvas(ItemIndex, living);
        menuManager.ShowExtraCanvas(extraIndex, living);


    }

    public void PrepareTutorial(List<tutorialStep> newSteps, List<int> newClarity, bool immediate = true)
    {
        return;
        currentTutorial.isActive = true;
        currentTutorial.steps.Clear();
        currentTutorial.clarifications.Clear();
        currentTutorial.currentStep = 0;
        currentTutorial.steps.AddRange(newSteps);
        currentTutorial.clarifications.AddRange(newClarity);

        // for (int i = 0; i < currentTutorial.steps.Count; i++)
        //{
        switch (currentTutorial.steps[0])
        {
            case tutorialStep.moveToPosition:
                {

                    TileScript selectedtile = tileMap[currentTutorial.clarifications[0]];
                    selectedtile.MYCOLOR = Common.orange;
                    selectedtile.isMarked = true;


                }
                break;
            case tutorialStep.showTutorial:
                {
                    if (immediate == true)
                    {
                        CheckTutorialPrompt(currentTutorial.clarifications[currentTutorial.currentStep]);

                    }
                    else
                    {
                        CheckTutorialPromptNonInterrupt(currentTutorial.clarifications[currentTutorial.currentStep]);

                    }

                }
                break;
        }
        // }


        TextObjectHandler.UpdateText(textHolder.subphaseTracker, Common.GetTutorialText(currentTutorial.steps[currentTutorial.currentStep]));
        TextObjectHandler.UpdateText(textHolder.shadowSubphaseTracker, Common.GetTutorialText(currentTutorial.steps[currentTutorial.currentStep]));
    }

    public bool DidCompleteTutorialStep()
    {

        bool returnedBool = false;
        if (currentTutorial.isActive == false)
        {
            return true;
        }
        if (currentTutorial.steps.Count == currentTutorial.clarifications.Count)
        {
            if (currentTutorial.currentStep < currentTutorial.steps.Count && currentTutorial.currentStep > -1)
            {
                tutorialStep checkStep = currentTutorial.steps[currentTutorial.currentStep];
                switch (checkStep)
                {
                    case tutorialStep.moveToPosition:
                        {
                            if (player.current)
                            {
                                if (player.current.currentTile == tileMap[currentTutorial.clarifications[currentTutorial.currentStep]])
                                {
                                    tileMap[currentTutorial.clarifications[currentTutorial.currentStep]].isMarked = false;
                                    returnedBool = true;

                                }
                            }
                        }
                        break;
                    case tutorialStep.useStrike:
                        {
                            if (player.current)
                            {
                                if (player.current.INVENTORY.WEAPONS.Count > 0)
                                {
                                    for (int i = 0; i < player.current.INVENTORY.WEAPONS.Count; i++)
                                    {
                                        if (player.current.INVENTORY.WEAPONS[i].USECOUNT > 0)
                                        {
                                            returnedBool = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case tutorialStep.useSkill:
                        {
                            if (player.current)
                            {
                                if (player.current.INVENTORY.CSKILLS.Count > 0)
                                {
                                    for (int i = 0; i < player.current.INVENTORY.CSKILLS.Count; i++)
                                    {
                                        if (player.current.INVENTORY.CSKILLS[i].ETYPE == EType.physical)
                                        {

                                            if (player.current.INVENTORY.CSKILLS[i].USECOUNT > 0)
                                            {
                                                returnedBool = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case tutorialStep.useSpell:
                        {
                            if (player.current)
                            {
                                if (player.current.INVENTORY.CSKILLS.Count > 0)
                                {
                                    for (int i = 0; i < player.current.INVENTORY.CSKILLS.Count; i++)
                                    {
                                        if (player.current.INVENTORY.CSKILLS[i].ETYPE == EType.magical)
                                        {

                                            if (player.current.INVENTORY.CSKILLS[i].USECOUNT > 0)
                                            {
                                                returnedBool = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case tutorialStep.useBarrier:
                        break;
                    case tutorialStep.useItem:
                        break;
                    case tutorialStep.defeatEnemy:
                        break;
                    case tutorialStep.hackGlyph:
                        break;
                    case tutorialStep.allocate:
                        {
                            if (Common.hasAllocated == true)
                            {

                                returnedBool = true;
                                break;

                            }
                        }
                        break;
                    case tutorialStep.showTutorial:

                        returnedBool = true;
                        break;
                }
            }
        }

        if (returnedBool == true)
        {
            currentTutorial.currentStep++;
            if (currentTutorial.currentStep >= currentTutorial.steps.Count)
            {
                currentTutorial.isActive = false;
                TextObjectHandler.UpdateText(textHolder.subphaseTracker, currentObjectiveString);
                TextObjectHandler.UpdateText(textHolder.shadowSubphaseTracker, currentObjectiveString);
            }
            else
            {
                TextObjectHandler.UpdateText(textHolder.subphaseTracker, Common.GetTutorialText(currentTutorial.steps[currentTutorial.currentStep]));
                TextObjectHandler.UpdateText(textHolder.shadowSubphaseTracker, Common.GetTutorialText(currentTutorial.steps[currentTutorial.currentStep]));
                if (currentTutorial.steps[currentTutorial.currentStep] == tutorialStep.showTutorial)
                {
                    CheckTutorialPrompt(currentTutorial.clarifications[currentTutorial.currentStep]);
                }
                if (currentTutorial.steps[currentTutorial.currentStep] == tutorialStep.moveToPosition)
                {
                    if (tileMap.Count > currentTutorial.clarifications[currentTutorial.currentStep])
                    {

                        TileScript selectedtile = tileMap[currentTutorial.clarifications[currentTutorial.currentStep]];
                        selectedtile.MYCOLOR = Common.orange;
                        selectedtile.isMarked = true;
                    }
                    else
                    {
                        currentTutorial.currentStep = -1;
                        currentTutorial.clarifications.Clear();
                        currentTutorial.steps.Clear();
                    }
                }
            }
        }


        return returnedBool;
    }

    private bool isObjectiveMet()
    {
        if (currentObjective == null)
        {
            return false;
        }

        switch (currentObjective.TYPE)
        {
            case ObjectiveType.reachLocation:
                {
                    for (int i = 0; i < gridObjects.Count; i++)
                    {
                        if (gridObjects[i].FACTION == Faction.ally)
                        {
                            if (gridObjects[i].currentTileIndex == currentObjective.TILE)
                            {
                                return true;
                            }
                        }
                    }
                }
                break;
            case ObjectiveType.defeatSpecificEnemy:
                {
                    for (int i = 0; i < liveEnemies.Count; i++)
                    {
                        if (liveEnemies[i].FACTION != Faction.hazard)
                        {

                            if (liveEnemies[i].id == currentObjective.ENEMY)
                            {

                                return false;

                            }
                        }
                    }

                    return true;
                }
                break;
            case ObjectiveType.defeatAllEnemies:
                {
                    if (currentMap.enemyIndexes.Count > 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                break;
            case ObjectiveType.storyRelated:
                {
                    return true;
                }
                break;
            case ObjectiveType.none:
                {
                    return true;
                }
                break;
            case ObjectiveType.defeatSpecificGlyph:
                {
                    for (int i = 0; i < liveEnemies.Count; i++)
                    {
                        if (liveEnemies[i].FACTION == Faction.hazard)
                        {

                            if (liveEnemies[i].id == currentObjective.GLYPH)
                            {

                                return false;

                            }
                        }
                    }

                    return true;
                }
                break;
            case ObjectiveType.defeatAllGlyphs:
                {
                    if (currentMap.hazardIndexes.Count > 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                break;
        }

        return false;
    }
    //private void ChangeSkillPrompt(string objName, SkillScript skill)
    //{
    //    if (prompt)
    //    {
    //        if (prompt.TitleText)
    //        {

    //            if (!skill)
    //                return;
    //            if (skill.GetType() == typeof(CommandSkill))
    //            {
    //                prompt.TitleText.text = objName + " learned a new Command Skill. Go to equip skill?";
    //            }
    //            else if (skill.GetType() == typeof(PassiveSkill))
    //            {
    //                prompt.TitleText.text = objName + " learned a new Passive Skill. Go to equip skill?";
    //            }
    //            else if (skill.GetType() == typeof(AutoSkill))
    //            {
    //                prompt.TitleText.text = objName + " learned a new Auto Skill. Go to equip skill?";
    //            }
    //            else if (skill.GetType() == typeof(OppSkill))
    //            {
    //                prompt.TitleText.text = objName + " learned a new Opportunity Skill. Go to equip skill?";
    //            }

    //            prompt.choice1.text = "Yes";
    //            prompt.choice2.text = "No";
    //        }
    //    }
    //}

    //private void ChangeSkillPrompt(string objName, UsableScript skill)
    //{
    //    if (prompt)
    //    {
    //        if (prompt.TitleText)
    //        {

    //            if (!skill)
    //                return;
    //            if (skill.GetType() == typeof(WeaponScript))
    //            {
    //                prompt.TitleText.text = objName + " learned a new Strike. Go to equip attack?";
    //            }
    //            else if (skill.GetType() == typeof(ArmorScript))
    //            {
    //                prompt.TitleText.text = objName + " learned a new Barrier spell. Go to equip Barrier?";
    //            }
    //            else
    //            {
    //                ChangeSkillPrompt(objName, (SkillScript)skill);
    //            }

    //            prompt.choice1.text = "Yes";
    //            prompt.choice2.text = "No";
    //        }
    //    }
    //}

    private void ChangeToDoorPrompt(TileScript doorTile)
    {
        if (prompt)
        {
            if (prompt.TitleText)
            {
                prompt.TitleText.text = "Door Event";
                prompt.BodyText.text = "Exit to " + doorTile.MAP + "?";
                prompt.choice1.text = "Yes";
                prompt.choice2.text = "No";
            }
        }
    }

    private void ChangeToEventPrompt(int eventNum, LivingObject living)
    {
        if (prompt)
        {
            if (prompt.TitleText)
            {

                EventDetails details = Common.GetEventText(eventNum, living);
                if (details.choice1 != "")
                {
                    prompt.TitleText.text = details.eventTitle;
                    prompt.BodyText.text = details.eventText;
                    //prompt.choice1.text = details.choice1;
                    //prompt.choice2.text = details.choice2;
                }
            }
        }
    }
    public void CompletePurchase()
    {
        if (shopScreen)
        {
            int inventoryType = shopScreen.selectedType;

            if (shopScreen.SELECTED)
            {
                if (shopScreen.SELECTED.refItem && shopScreen.REMOVING.refItem && shopScreen.AUGMENT == Augment.none)
                {
                    player.current.INVENTORY.USEABLES.Add(shopScreen.SELECTED.refItem);
                    player.current.INVENTORY.USEABLES.Remove(shopScreen.REMOVING.refItem);

                    switch (inventoryType)
                    {
                        case 0:
                            CommandSkill cmd = shopScreen.SELECTED.refItem as CommandSkill;
                            if (cmd.ETYPE == EType.physical)
                            {
                                if (player.current.PHYSICAL_SLOTS.CanAdd())
                                {

                                    player.current.INVENTORY.CSKILLS.Add((CommandSkill)shopScreen.SELECTED.refItem);
                                    player.current.PHYSICAL_SLOTS.SKILLS.Add((CommandSkill)shopScreen.SELECTED.refItem);

                                    player.current.INVENTORY.WEAPONS.Remove((WeaponScript)shopScreen.REMOVING.refItem);
                                    if (shopScreen.REMOVING.refItem.INDEX == player.current.WEAPON.WEPID && player.current.INVENTORY.WEAPONS.Count > 0)
                                    {
                                        player.current.WEAPON.Equip(player.current.INVENTORY.WEAPONS[0]);
                                    }
                                    else if (shopScreen.REMOVING.refItem.INDEX == player.current.WEAPON.WEPID && player.current.INVENTORY.WEAPONS.Count == 0)
                                    {
                                        player.current.WEAPON.unEquip();
                                    }
                                    SHOPLIST.Remove(shopScreen.SELECTED.refItem);

                                    shopScreen.SHOPITEMS = SHOPLIST;
                                    shopScreen.PreviousMenu();
                                    shopScreen.PreviousMenu();
                                    shopScreen.loadShopList();
                                }
                            }
                            else
                            {
                                if (player.current.MAGICAL_SLOTS.CanAdd())
                                {

                                    player.current.INVENTORY.CSKILLS.Add((CommandSkill)shopScreen.SELECTED.refItem);
                                    player.current.MAGICAL_SLOTS.SKILLS.Add((CommandSkill)shopScreen.SELECTED.refItem);

                                    player.current.INVENTORY.WEAPONS.Remove((WeaponScript)shopScreen.REMOVING.refItem);
                                    if (shopScreen.REMOVING.refItem.INDEX == player.current.WEAPON.WEPID && player.current.INVENTORY.WEAPONS.Count > 0)
                                    {
                                        player.current.WEAPON.Equip(player.current.INVENTORY.WEAPONS[0]);
                                    }
                                    else if (shopScreen.REMOVING.refItem.INDEX == player.current.WEAPON.WEPID && player.current.INVENTORY.WEAPONS.Count == 0)
                                    {
                                        player.current.WEAPON.unEquip();
                                    }
                                    SHOPLIST.Remove(shopScreen.SELECTED.refItem);

                                    shopScreen.SHOPITEMS = SHOPLIST;
                                    shopScreen.PreviousMenu();
                                    shopScreen.PreviousMenu();
                                    shopScreen.loadShopList();
                                }
                            }
                            break;
                        case 1:
                            player.current.INVENTORY.ARMOR.Add((ArmorScript)shopScreen.SELECTED.refItem);
                            player.current.INVENTORY.ARMOR.Remove((ArmorScript)shopScreen.REMOVING.refItem);
                            if (shopScreen.REMOVING.refItem.INDEX == player.current.ARMOR.ARMORID && player.current.INVENTORY.ARMOR.Count > 0)
                            {
                                player.current.ARMOR.Equip(player.current.INVENTORY.ARMOR[0]);
                            }
                            else if (shopScreen.REMOVING.refItem.INDEX == player.current.ARMOR.ARMORID && player.current.INVENTORY.ARMOR.Count == 0)
                            {
                                player.current.ARMOR.unEquip();
                            }
                            SHOPLIST.Remove(shopScreen.SELECTED.refItem);

                            shopScreen.SHOPITEMS = SHOPLIST;
                            shopScreen.PreviousMenu();
                            shopScreen.PreviousMenu();
                            shopScreen.loadShopList();
                            break;
                        case 2:
                            if (player.current.INVENTORY.WEAPONS.Count < 6)
                            {

                                player.current.INVENTORY.WEAPONS.Add((WeaponScript)shopScreen.SELECTED.refItem);

                                player.current.PHYSICAL_SLOTS.SKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.CSKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.SKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);

                                SHOPLIST.Remove(shopScreen.SELECTED.refItem);

                                shopScreen.SHOPITEMS = SHOPLIST;
                                shopScreen.PreviousMenu();
                                shopScreen.PreviousMenu();
                                shopScreen.loadShopList();
                            }
                            break;
                        case 3:
                            if (player.current.INVENTORY.WEAPONS.Count < 6)
                            {

                                player.current.INVENTORY.WEAPONS.Add((WeaponScript)shopScreen.SELECTED.refItem);

                                player.current.PHYSICAL_SLOTS.SKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.CSKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.SKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);

                                SHOPLIST.Remove(shopScreen.SELECTED.refItem);

                                shopScreen.SHOPITEMS = SHOPLIST;
                                shopScreen.PreviousMenu();
                                shopScreen.PreviousMenu();
                                shopScreen.loadShopList();
                            }
                            break;
                        case 4:
                            if (player.current.AUTO_SLOTS.CanAdd())
                            {

                                player.current.INVENTORY.AUTOS.Add((AutoSkill)shopScreen.SELECTED.refItem);
                                player.current.AUTO_SLOTS.SKILLS.Add((AutoSkill)shopScreen.SELECTED.refItem);
                                player.current.COMBO_SLOTS.SKILLS.Remove((ComboSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.COMBOS.Remove((ComboSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.SKILLS.Remove((ComboSkill)shopScreen.REMOVING.refItem);

                                SHOPLIST.Remove(shopScreen.SELECTED.refItem);

                                shopScreen.SHOPITEMS = SHOPLIST;
                                shopScreen.PreviousMenu();
                                shopScreen.PreviousMenu();
                                shopScreen.loadShopList();
                            }
                            break;
                        case 5:
                            if (player.current.COMBO_SLOTS.CanAdd())
                            {

                                player.current.INVENTORY.COMBOS.Add((ComboSkill)shopScreen.SELECTED.refItem);
                                player.current.COMBO_SLOTS.SKILLS.Add((ComboSkill)shopScreen.SELECTED.refItem);
                                player.current.AUTO_SLOTS.SKILLS.Remove((AutoSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.AUTOS.Remove((AutoSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.SKILLS.Remove((AutoSkill)shopScreen.REMOVING.refItem);
                                SHOPLIST.Remove(shopScreen.SELECTED.refItem);

                                shopScreen.SHOPITEMS = SHOPLIST;
                                shopScreen.PreviousMenu();
                                shopScreen.PreviousMenu();
                                shopScreen.loadShopList();
                            }

                            break;
                        case 6:
                            player.current.INVENTORY.OPPS.Add((OppSkill)shopScreen.SELECTED.refItem);
                            // player.current.OPP_SLOTS.SKILLS.Remove((OppSkill)shopScreen.REMOVING.refItem);
                            player.current.INVENTORY.OPPS.Remove((OppSkill)shopScreen.REMOVING.refItem);
                            player.current.INVENTORY.SKILLS.Remove((OppSkill)shopScreen.REMOVING.refItem);
                            break;
                    }


                    // shopScreen.loadBuyerList();
                }
                else if (shopScreen.SELECTED.refItem && shopScreen.REMOVING.refItem && shopScreen.AUGMENT != Augment.end)
                {
                    UsableScript refUse = shopScreen.SELECTED.refItem;
                    refUse.ApplyAugment(shopScreen.AUGMENT);
                    refUse.AUGMENTS.Add(shopScreen.AUGMENT);
                    refUse.UpdateDesc();


                    player.current.INVENTORY.ITEMS.Remove((ItemScript)shopScreen.REMOVING.refItem);
                    shopScreen.PreviousMenu();
                    shopScreen.PreviousMenu();
                    shopScreen.PreviousMenu();

                }
            }
            else if (shopScreen.REMOVING && shopScreen.AUGMENT == Augment.none)
            {
                if (shopScreen.REMOVING.refItem)
                {
                    if (player.current.INVENTORY.ITEMS.Count < 6)
                    {

                        player.current.INVENTORY.USEABLES.Remove(shopScreen.REMOVING.refItem);
                        int atkCount = player.current.INVENTORY.WEAPONS.Count;
                        atkCount += player.current.INVENTORY.CSKILLS.Count;
                        if (atkCount <= 1)
                        {
                            if (inventoryType == 0 || inventoryType == 2 || inventoryType == 3)
                            {
                                ShowCantUseText("Can't get rid of only attack");
                                return;
                            }
                        }
                        switch (inventoryType)
                        {
                            case 0:

                                player.current.INVENTORY.WEAPONS.Remove((WeaponScript)shopScreen.REMOVING.refItem);
                                if (shopScreen.REMOVING.refItem.INDEX == player.current.WEAPON.WEPID && player.current.INVENTORY.WEAPONS.Count > 0)
                                {
                                    player.current.WEAPON.Equip(player.current.INVENTORY.WEAPONS[0]);
                                }
                                else if (shopScreen.REMOVING.refItem.INDEX == player.current.WEAPON.WEPID && player.current.INVENTORY.WEAPONS.Count == 0)
                                {
                                    player.current.WEAPON.unEquip();
                                }
                                break;
                            case 1:

                                player.current.INVENTORY.ARMOR.Remove((ArmorScript)shopScreen.REMOVING.refItem);
                                if (shopScreen.REMOVING.refItem.INDEX == player.current.ARMOR.ARMORID && player.current.INVENTORY.ARMOR.Count > 0)
                                {
                                    player.current.ARMOR.Equip(player.current.INVENTORY.ARMOR[0]);
                                }
                                else if (shopScreen.REMOVING.refItem.INDEX == player.current.ARMOR.ARMORID && player.current.INVENTORY.ARMOR.Count == 0)
                                {
                                    player.current.ARMOR.unEquip();
                                }
                                break;
                            case 2:

                                player.current.PHYSICAL_SLOTS.SKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.CSKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.SKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);
                                break;
                            case 3:

                                player.current.MAGICAL_SLOTS.SKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.CSKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.SKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);
                                break;
                            case 4:

                                player.current.COMBO_SLOTS.SKILLS.Remove((ComboSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.COMBOS.Remove((ComboSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.SKILLS.Remove((ComboSkill)shopScreen.REMOVING.refItem);
                                break;
                            case 5:

                                player.current.AUTO_SLOTS.SKILLS.Remove((AutoSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.AUTOS.Remove((AutoSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.SKILLS.Remove((AutoSkill)shopScreen.REMOVING.refItem);
                                break;
                            case 6:

                                // player.current.OPP_SLOTS.SKILLS.Remove((OppSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.OPPS.Remove((OppSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.SKILLS.Remove((OppSkill)shopScreen.REMOVING.refItem);
                                break;
                        }

                        UsableScript useable = database.GetItem(Random.Range(90, 99), player.current);


                        CreateEvent(this, useable, "New Skill Event", CheckCount, null, 0, CountStart);
                        // CreateTextEvent(this, "" + player.current.FullName + " gained " + useable.NAME, "new skill event", CheckText, TextStart);

                        shopScreen.PreviousMenu();
                        shopScreen.PreviousMenu();
                        shopScreen.loadBuyerList(shopScreen.selectedType);
                    }
                    else
                    {
                        CreateTextEvent(this, player.current.NAME + " cannot hold any more items", "item limit", CheckText, TextStart);
                    }
                }
            }


        }
    }
}
