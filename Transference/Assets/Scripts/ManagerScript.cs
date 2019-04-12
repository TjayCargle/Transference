using System.Collections;
using System.Collections.Generic;
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
    HazardManager hazardManager;
    public Texture doorTexture;
    public Texture shopTexture;
    DatabaseManager database;
    ConditionalDisplay[] displays;
    public PotentialSlider potential;
    public PhaseImg phaseImage;
    [SerializeField]
    StatusIconManager iconManager;
    public Material ShadowMaterial;
    private List<UsableScript> shopItems = new List<UsableScript>();

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
            database = GameObject.FindObjectOfType<DatabaseManager>();
            prompt = GameObject.FindObjectOfType<NewSkillPrompt>();
            menuManager = GetComponent<MenuManager>();
            invManager = GetComponent<InventoryMangager>();
            eventManager = GetComponent<EventManager>();
            textManager = GetComponent<TextEventManager>();
            tileManager = GetComponent<TileManager>();
            enemyManager = GetComponent<EnemyManager>();
            hazardManager = GetComponent<HazardManager>();
            displays = GameObject.FindObjectsOfType<ConditionalDisplay>();
            potential = GameObject.FindObjectOfType<PotentialSlider>();
            iconManager = GameObject.FindObjectOfType<StatusIconManager>();
            stackManager = GameObject.FindObjectOfType<MenuStackManager>();
            options = GameObject.FindObjectOfType<OptionsManager>();
            detailsScreen = GameObject.FindObjectOfType<DetailsScreen>();
            shopScreen = GameObject.FindObjectOfType<ShopScreen>();

            if (!enemyManager)
            {
                enemyManager = gameObject.AddComponent<EnemyManager>();
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

            enemyManager.Setup();
            hazardManager.Setup();
            eventManager.Setup();
            tileManager.Setup();
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
            //flavor = FindObjectOfType<FlavorTextImg>();

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
            tileMap = null;// new GameObject[MapWidth * MapHeight];



            if (GameObject.FindObjectOfType<CameraScript>())
            {
                myCamera = GameObject.FindObjectOfType<CameraScript>();
            }
            commandItems = GameObject.FindObjectsOfType<MenuItem>();
            player = GameObject.FindObjectOfType<PlayerController>();
            tempObject = new GameObject();
            tempObject.AddComponent<TempObject>();
            tempObject.name = "TempObj";
            tempObject.GetComponent<GridObject>().NAME = "TEMPY";
            tempObject.GetComponent<GridObject>().MOVE_DIST = 10000;
            tempObject.transform.position = Vector3.zero;
            menuManager.ShowNone();
            isSetup = true;
            currentState = State.FreeCamera;
            LoadDefaultScene();


            updateConditionals();
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

    void LateUpdate()
    {
        if (menuStack != null)
            menuStackCount = menuStack.Count;
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    LoadDScene(1);
        //}
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    Clear();
        //}
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    LoadDScene(0);
        //}
        // if (currentObject)
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
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            player.UseOrAttack();
                        }
                        if (Input.GetMouseButtonDown(0))
                        {
                            //  myCamera.infoObject = null;
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
                                                    myCamera.infoObject = GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile);
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
                                                    myCamera.infoObject = GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile);
                                                    GridObject griddy = GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile);
                                                    if (griddy)
                                                    {
                                                        if (griddy.GetComponent<LivingObject>())
                                                        {
                                                            LivingObject livvy = griddy.GetComponent<LivingObject>();
                                                            if (livvy.FACTION != player.current.FACTION)
                                                            {
                                                                DmgReaction reac = CalcDamage(player.current, livvy, player.currentSkill, Reaction.none, false);
                                                                if (reac.reaction > Reaction.weak)
                                                                    reac.damage = 0;
                                                                myCamera.potentialDamage = reac.damage;
                                                                myCamera.UpdateCamera();

                                                                updateConditionals();
                                                            }
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
                                                    currentAttackList[i].myColor = Common.red; ;

                                                }
                                                myCamera.infoObject = GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile);
                                                GridObject griddy = GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile);
                                                if (griddy)
                                                {
                                                    if (griddy.GetComponent<LivingObject>())
                                                    {
                                                        LivingObject livvy = griddy.GetComponent<LivingObject>();
                                                        if (livvy.FACTION != player.current.FACTION)
                                                        {
                                                            DmgReaction reac = CalcDamage(player.current, livvy, player.current.WEAPON, Reaction.none, false);
                                                            if (reac.reaction > Reaction.weak)
                                                                reac.damage = 0;
                                                            myCamera.potentialDamage = reac.damage;
                                                            myCamera.UpdateCamera();

                                                            updateConditionals();
                                                        }
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
                        if (hitkey == true)
                        {

                            if (attackableTiles.Count > 0)
                            {
                                int realAmt = attackableTiles.Count / 4;
                                if (skipCount > realAmt)
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
                                        if (player.currentSkill.SUBTYPE == SubSkillType.Buff || player.currentSkill.ELEMENT == Element.Support)
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
                case State.playerUsingSkills:
                    //if (Input.GetKeyDown(KeyCode.Space))
                    //{
                    //    if ((int)descriptionState + 1 <= 2)
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
                                    for (int i = 0; i < gridObjects.Count; i++)
                                    {
                                        ShowSelectedTile(gridObjects[i], Common.GetFactionColor(gridObjects[i].FACTION));
                                    }

                                    tempObject.transform.position = hitTile.transform.position;
                                    ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), hitTile);
                                    if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                                    {
                                        ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                                        if (iconManager)
                                        {
                                            iconManager.loadIconPanel(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile).GetComponent<LivingObject>());
                                        }
                                    }
                                    ShowSelectedTile(tempObject.GetComponent<GridObject>());
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
                        if (detailsScreen.selectedContent < 3)
                        {
                            detailsScreen.selectedContent = 28;
                        }
                        else if (detailsScreen.selectedContent >= 21 && detailsScreen.selectedContent < 28)
                        {
                            detailsScreen.selectedContent -= 10;
                        }
                        else if (detailsScreen.selectedContent > 3 && detailsScreen.selectedContent <= 7)
                        {
                            detailsScreen.selectedContent--;
                        }
                        else if (detailsScreen.selectedContent == 3)
                        {
                            detailsScreen.selectedContent = 1;
                        }
                        else if (detailsScreen.selectedContent == 8 || detailsScreen.selectedContent == 9 || detailsScreen.selectedContent == 12)  // else if (detailsScreen.selectedContent > 8 && detailsScreen.selectedContent < 14)
                        {
                            detailsScreen.selectedContent = 7;
                        }
                        else if (detailsScreen.selectedContent == 10 || detailsScreen.selectedContent == 11)
                        {
                            detailsScreen.selectedContent -= 2;
                        }
                        else if (detailsScreen.selectedContent == 12)
                        {
                            detailsScreen.selectedContent = 11;
                        }
                        else if (detailsScreen.detail != DetailType.Exp)
                        {

                            if (detailsScreen.selectedContent == 14)
                            {
                                detailsScreen.selectedContent = 7;
                            }
                            else if (detailsScreen.selectedContent > 14 && detailsScreen.selectedContent < 21)
                            {
                                detailsScreen.selectedContent = 7;
                            }

                            else if (detailsScreen.selectedContent == 28)
                            {
                                detailsScreen.selectedContent = 21;
                            }
                        }
                        else
                        {
                            if (detailsScreen.selectedContent > 29 && detailsScreen.selectedContent < 33)
                            {
                                detailsScreen.selectedContent--;
                            }
                            else if (detailsScreen.selectedContent == 14)
                            {
                                detailsScreen.selectedContent = 32;
                            }
                            else if (detailsScreen.selectedContent > 14 && detailsScreen.selectedContent < 21)
                            {
                                detailsScreen.selectedContent = 7;
                            }
                            else if (detailsScreen.selectedContent == 28)
                            {
                                detailsScreen.selectedContent = 21;
                            }
                            else if (detailsScreen.selectedContent == 29)
                            {
                                detailsScreen.selectedContent = 1;
                            }


                        }
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {

                        if (detailsScreen.selectedContent >= 22 && detailsScreen.selectedContent < 28)
                        {
                            detailsScreen.selectedContent = 28;
                        }
                        else if (detailsScreen.selectedContent >= 14 && detailsScreen.selectedContent < 22)
                        {
                            detailsScreen.selectedContent += 7;
                        }
                        else if (detailsScreen.selectedContent == 28)
                        {
                            detailsScreen.selectedContent = 1;
                        }
                        else if (detailsScreen.selectedContent >= 3 && detailsScreen.selectedContent < 7)
                        {
                            detailsScreen.selectedContent++;
                        }
                        else if (detailsScreen.selectedContent > 7 && detailsScreen.selectedContent < 14)
                        {
                            detailsScreen.selectedContent++;
                        }
                        else if (detailsScreen.detail != DetailType.Exp)
                        {
                            if (detailsScreen.selectedContent == 7)
                            {
                                detailsScreen.selectedContent = 14;
                            }

                            else if (detailsScreen.selectedContent < 3)
                            {
                                detailsScreen.selectedContent = 3;
                            }

                        }
                        else
                        {
                            if (detailsScreen.selectedContent == 7)
                            {
                                detailsScreen.selectedContent = 14;
                            }
                            else if (detailsScreen.selectedContent >= 29 && detailsScreen.selectedContent < 32)
                            {
                                detailsScreen.selectedContent++;
                            }
                            else if (detailsScreen.selectedContent == 32)
                            {
                                detailsScreen.selectedContent = 14;
                            }
                            else if (detailsScreen.selectedContent < 3)
                            {
                                detailsScreen.selectedContent = 29;
                            }
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
                                truedetail = 6;
                            }
                            detailsScreen.detail = (DetailType)truedetail;
                        }
                        else if (detailsScreen.selectedContent > 14 && detailsScreen.selectedContent < 21)
                        {
                            detailsScreen.selectedContent--;
                        }
                        else if (detailsScreen.selectedContent > 21 && detailsScreen.selectedContent < 28)
                        {
                            detailsScreen.selectedContent--;
                        }
                        else if (detailsScreen.selectedContent < 3)
                        {
                            detailsScreen.selectedContent--;
                        }
                        else if (detailsScreen.detail != DetailType.Exp)
                        {
                            if (detailsScreen.selectedContent >= 8 && detailsScreen.selectedContent < 13)
                            {
                                detailsScreen.selectedContent -= 5;
                            }
                            else if (detailsScreen.selectedContent > 2 && detailsScreen.selectedContent < 8)
                            {
                                detailsScreen.selectedContent += 5;
                            }
                            else if (detailsScreen.selectedContent == 13)
                            {
                                detailsScreen.selectedContent -= 6;
                            }
                        }
                        else
                        {
                            if (detailsScreen.selectedContent >= 8 && detailsScreen.selectedContent < 13)
                            {
                                detailsScreen.selectedContent = 29;
                            }
                            else if (detailsScreen.selectedContent > 28 && detailsScreen.selectedContent < 33)
                            {
                                detailsScreen.selectedContent = 8;
                            }
                        }

                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        if (detailsScreen.selectedContent == 2)
                        {
                            int truedetail = (int)detailsScreen.detail;
                            truedetail++;
                            if (truedetail > 6)
                            {
                                truedetail = 0;
                            }
                            detailsScreen.detail = (DetailType)truedetail;
                        }
                        else if (detailsScreen.selectedContent > 13 && detailsScreen.selectedContent < 20)
                        {
                            detailsScreen.selectedContent++;
                        }
                        else if (detailsScreen.selectedContent > 20 && detailsScreen.selectedContent < 27)
                        {
                            detailsScreen.selectedContent++;
                        }
                        else if (detailsScreen.selectedContent < 2)
                        {
                            detailsScreen.selectedContent++;
                        }
                        else if (detailsScreen.detail != DetailType.Exp)
                        {

                            if (detailsScreen.selectedContent > 2 && detailsScreen.selectedContent < 8)
                            {
                                detailsScreen.selectedContent += 5;
                            }
                            else if (detailsScreen.selectedContent >= 8 && detailsScreen.selectedContent < 13)
                            {
                                detailsScreen.selectedContent -= 5;
                            }
                            else if (detailsScreen.selectedContent == 13)
                            {
                                detailsScreen.selectedContent -= 6;
                            }
                        }
                        else
                        {
                            if (detailsScreen.selectedContent >= 8 && detailsScreen.selectedContent < 13)
                            {
                                detailsScreen.selectedContent = 29;
                            }
                            else if (detailsScreen.selectedContent > 28 && detailsScreen.selectedContent < 33)
                            {
                                detailsScreen.selectedContent = 8;
                            }
                        }
                    }
                    break;

                case State.AquireNewSkill:
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        YesPrompt();
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        returnState();
                    }
                    break;

                default:
                    break;
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
    private void updateConditionals()
    {

        for (int i = 0; i < displays.Length; i++)
        {
            displays[i].UpdateDisplay(this);
        }


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

        if (currentState == State.PlayerAttacking || currentState == State.PlayerTransition)
        {

        }
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
            if (currentObject.GetComponent<LivingObject>())
                ShowGridObjectMoveArea(currentObject.GetComponent<LivingObject>());
        }
        if (currentState == State.PlayerEquipping)
        {
            descriptionState = descState.stats;
        }
        if (currentState == State.playerUsingSkills)
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
        if (currentState == State.PlayerAttacking)
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
        myCamera.UpdateCamera();
        updateConditionals();

        switch (currentState)
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
            case State.PlayerInput:
                myCamera.SetCameraPosZoom();
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
        }
    }
    public void enterStateTransition()
    {
        currentState = State.PlayerTransition;
        updateConditionals();
        myCamera.UpdateCamera();
    }
    public void returnState()
    {

        // Debug.Log("returnin");
        if (currentState == State.PlayerAttacking || currentState == State.PlayerTransition || currentState == State.playerUsingSkills)
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
                if (currentObject == myCamera.infoObject)
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
        if (currentState == State.FreeCamera)
        {
            myCamera.SetCameraPosDefault();
        }
        if (currentState == State.PlayerInput)
        {
            myCamera.SetCameraPosZoom();
        }
        if (currentState == State.PlayerAttacking)
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
        myCamera.UpdateCamera();
        newSkillEvent.caller = null;
        updateConditionals();
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
                                if (newSkillEvent.caller == null)
                                {
                                    player.current.TakeAction();
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
                            Debug.Log("ERROR, Could not find menu item");
                            bool res = commandItems[0].ComfirmAction(movedObj, MenuItemType.Move);
                            if (res)
                            {

                                if (currentState != State.PlayerOppMove && currentState != State.PlayerOppOptions)
                                {
                                    if (newSkillEvent.caller == null)
                                    {
                                        player.current.TakeAction();
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
                                Debug.Log("We got problems");
                            }
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
                        if (xDist + yDist <= player.current.MOVE_DIST && StartCanMoveCheck(player.current, player.current.currentTile, hitTile))
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

    public void LoadDefaultScene()
    {
        LoadDScene(0);
        prevState = currentState;
        currentState = State.PlayerTransition;
        CreateEvent(this, turnOrder[0], "Phase Announce Event", PhaseAnnounce, null, -1, PhaseAnnounceStart);
        CreateEvent(this, null, "return state event", BufferedStateChange);
        //Debug.Log("load");
        //if (null == tileMap || tileMap.Count == 0)
        //{
        //    tileMap = tileManager.getTiles(MapWidth * MapHeight);
        //}
        //else
        //{
        //    return;
        //}
        //int tileIndex = 0;
        //for (int i = 0; i < MapWidth; i++)
        //{
        //    for (int j = 0; j < MapHeight; j++)
        //    {

        //        int mapIndex = TwoToOneD(j, MapWidth, i);
        //        TileScript tile = tileMap[tileIndex];
        //        tile.listindex = mapIndex;
        //        tile.transform.position = new Vector3(i, 0, j);
        //        tile.transform.parent = tileParent.transform;
        //        tile.name = "Tile " + mapIndex;
        //        tileIndex++;
        //    }
        //}
        //tileMap.Sort();
        //LivingObject[] livingObjects = GameObject.FindObjectsOfType<LivingObject>();
        //for (int i = livingObjects.Length - 1; i >= 0; i--)
        //{

        //    if (livingObjects[i].GetComponent<EnemyScript>() || livingObjects[i].GetComponent<HazardScript>())
        //    {
        //        continue;
        //    }
        //    turnOrder.Add(livingObjects[i]);
        //}
        //if (turnOrder.Count > 0)
        //    currentObject = turnOrder[0];

        //tempObject.GetComponent<GridObject>().currentTile = GetTileAtIndex(GetTileIndex(Vector3.zero));
        //GridObject[] objs = GameObject.FindObjectsOfType<GridObject>();
        //attackableTiles = new List<List<TileScript>>();
        //ShowWhite();
        //for (int i = 0; i < objs.Length; i++)
        //{
        //    if (objs[i].gameObject == tempObject)
        //    {
        //        continue;
        //    }
        //    gridObjects.Add(objs[i]);
        //    objs[i].currentTile = GetTile(objs[i]);
        //    objs[i].currentTile.isOccupied = true;

        //}
        //currentState = State.FreeCamera;

        //ShowGridObjectAffectArea(tempObject.GetComponent<GridObject>(), true);

        //ShowSelectedTile(tempObject.GetComponent<GridObject>());
        //for (int i = 0; i < turnOrder.Count; i++)
        //{
        //    ShowSelectedTile(turnOrder[i], Common.orange);

        //}
        //myCamera.currentTile = tileMap[0];
    }

    public void LoadDScene(int amapIndex)
    {


        database.Setup();
        MapDetail map = database.GetMap(amapIndex);
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

        float xoffset = 1 / (float)MapWidth;
        float yoffset = 1 / (float)MapHeight;
        for (int i = 0; i < map.width; i++)
        {

            for (int j = 0; j < map.height; j++)
            {
                int mapIndex = TwoToOneD(j, map.width, i);
                TileScript tile = tileMap[tileIndex];
                tile.listindex = mapIndex;
                tile.transform.position = new Vector3(i, 0, j);
                tile.transform.parent = tileParent.transform;
                tile.name = "Tile " + mapIndex;
                tile.transform.rotation = Quaternion.Euler(90, 0, 0);
                tile.setTexture(map.texture);
                tile.TTYPE = TileType.regular;
                tile.setUVs((xoffset * (float)i), (xoffset * (float)(i + 1)), (yoffset * (float)j), (yoffset * (float)(j + 1)));
                tileIndex++;

            }

        }
        tileMap.Sort();

        for (int i = 0; i < map.doorIndexes.Count; i++)
        {
            tileMap[map.doorIndexes[i]].MAT.mainTexture = doorTexture;
            tileMap[map.doorIndexes[i]].MAP = map.roomNames[i];
            tileMap[map.doorIndexes[i]].ROOM = map.roomIndexes[i];
            tileMap[map.doorIndexes[i]].setUVs(0, 1, 0, 1);
            tileMap[map.doorIndexes[i]].TTYPE = TileType.door;
        }

        for (int i = 0; i < map.shopIndexes.Count; i++)
        {
            tileMap[map.shopIndexes[i]].MAT.mainTexture = shopTexture;
            tileMap[map.shopIndexes[i]].setUVs(0, 1, 0, 1);
            tileMap[map.shopIndexes[i]].TTYPE = TileType.shop;
        }


        tileIndex = 1;
        int largestLevel = 1;
        turnOrder.Clear();
        for (int i = 0; i < livingobjs.Length; i++)
        {
            if (livingobjs[i].GetComponent<EnemyScript>() || livingobjs[i].GetComponent<HazardScript>())
            {
                continue;
            }
            if (livingobjs[i].GetComponent<ActorScript>())
            {
                livingobjs[i].transform.position = tileMap[tileIndex].transform.position + new Vector3(0, 0.5f, 0);
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
        List<EnemyScript> enemies = enemyManager.getEnemies(map.enemyIndexes.Count);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.position = tileMap[map.enemyIndexes[i]].transform.position + new Vector3(0, 0.5f, 0);
            enemies[i].gameObject.SetActive(true);
            lvtimes = Random.Range(largestLevel - 1, largestLevel + 2);
            enemies[i].BASE_STATS.LEVEL = largestLevel;

            for (int j = 0; j < lvtimes; j++)
            {
                enemies[i].LevelUp();
                enemies[i].BASE_STATS.HEALTH = enemies[i].BASE_STATS.MAX_HEALTH;
                enemies[i].BASE_STATS.MANA = enemies[i].BASE_STATS.MAX_MANA;
                enemies[i].BASE_STATS.FATIGUE = 0;
            }


        }

        List<HazardScript> hazards = hazardManager.getHazards(map.hazardIndexes.Count);
        for (int i = 0; i < hazards.Count; i++)
        {
            hazards[i].transform.position = tileMap[map.hazardIndexes[i]].transform.position + new Vector3(0, 0.5f, 0);
            hazards[i].gameObject.SetActive(true);
            hazards[i].BASE_STATS.LEVEL = Random.Range(largestLevel, largestLevel + 2);
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


        tempObject.GetComponent<GridObject>().currentTile = GetTileAtIndex(GetTileIndex(Vector3.zero));
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

                objs[i].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        //CleanMenuStack(true);
        //currentState = State.FreeCamera;
        // tempObject.transform.position = Vector3.zero;
        //tempObject.GetComponent<GridObject>().currentTile = tileMap[0];
        // myCamera.currentTile = tileMap[0];
        //ShowGridObjectAffectArea(tempObject.GetComponent<GridObject>(), true);

        ShowSelectedTile(tempObject.GetComponent<GridObject>());

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
                        itemNum = Random.Range(0, 5);
                        itemNum += (Random.Range(1, 9) * 10);
                        //    Debug.Log("command " + itemNum);
                        newItem = database.GetSkill(itemNum);
                        break;
                    case 2:
                        itemNum = Random.Range(110, 116);
                        //  Debug.Log("auto " + itemNum);
                        newItem = database.GetSkill(itemNum);
                        break;
                    case 3:
                        itemNum = Random.Range(200, 211);
                        //  Debug.Log("passive " + itemNum);
                        newItem = database.GetSkill(itemNum);
                        break;
                    case 4:
                        itemNum = Random.Range(0, 12);
                        newItem = database.GetArmor(itemNum, null);
                        break;
                    default:
                        break;
                }
                shopItems.Add(newItem);
            }
        }
    }

    public void Clear()
    {
        if (null == tileMap)
            return;
        enterStateTransition();
        for (int i = 0; i < gridObjects.Count; i++)
        {
            gridObjects[i].currentTile = null;
            if (gridObjects[i].GetComponent<EnemyScript>())
                gridObjects[i].gameObject.SetActive(false);
            if (gridObjects[i].GetComponent<HazardScript>())
                gridObjects[i].gameObject.SetActive(false);
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
    private void SoftReset()
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
            griddy.currentTile = GetTile(griddy);
            griddy.currentTile.isOccupied = true;

        }
    }
    public void NextRound()
    {
        currOppList.Clear();
        doubleAdjOppTiles.Clear();
        menuManager.ShowNone();

        LivingObject[] livingObjects = GameObject.FindObjectsOfType<LivingObject>();
        switch (currentState)
        {
            case State.HazardTurn:
                currentState = State.FreeCamera;
                break;
            case State.EnemyTurn:
                currentState = State.HazardTurn;
                menuManager.ShowNone();
                break;
            default:
                currentState = State.EnemyTurn;
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


                    if (currentState == State.HazardTurn)
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

        if (turnOrder.Count > 0)
        {
            if (turnOrder.Count > 1)
            {
                CreateEvent(this, turnOrder[0], "Phase Announce Event", PhaseAnnounce, null, 0, PhaseAnnounceStart);

            }
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
                    // Debug.Log(" "+turnOrder[i].NAME + " has " + acts + " actions due to " + ((int)(turnOrder[i].SPEED / 10)) + " + 2 + gene " + turnOrder[i].GENERATED);

                    turnOrder[i].GENERATED = 0;

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
                }
            }
        }
        else
        {
            //  NextRound();
            NextTurn("manager next round event");
        }
        SoftReset();
        myCamera.UpdateCamera();
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
            //if (currentState != State.EnemyTurn && currentState != State.HazardTurn)
            //{
            //    menuManager.ShowCommandCanvas();
            //}
            MoveCameraAndShow(obj);
            myCamera.UpdateCamera();
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

        LivingObject aliveObj = null;
        GridObject tempGridObject = GetObjectAtTile(obj.currentTile);
        if (tempGridObject)
        {
            if (GetObjectAtTile(obj.currentTile).GetComponent<LivingObject>())
            {
                aliveObj = GetObjectAtTile(obj.currentTile).GetComponent<LivingObject>();
            }
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
        for (int i = 0; i < MapWidth; i++)
        {
            for (int j = 0; j < MapHeight; j++)
            {
                TileScript temp = tileMap[TwoToOneD(j, MapWidth, i)];
                float tempX = temp.transform.position.x;
                float tempY = temp.transform.position.z;
                if (!obj)
                    return;
                float objX = obj.currentTile.transform.position.x;
                float objY = obj.currentTile.transform.position.z;

                int MoveDist = 0;
                int attackDist = 0;
                int range = 0;
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
                    range = liveObj.WEAPON.Range;
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

                    if (aliveObj)
                    {

                        if (StartCanMoveCheck(aliveObj, aliveObj.currentTile, temp))
                        {
                            temp.GetComponent<TileScript>().myColor = Color.cyan;

                        }
                        else if (GetObjectAtTile(temp) && attackDist + range > 0)
                        {

                            temp.myColor = Common.red;
                        }
                        else
                        {
                            temp.GetComponent<TileScript>().myColor = Color.white;
                        }

                    }

                }
                else if (xDist + yDist <= MoveDist + attackDist && attackDist + range > 0)
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
                        temp.GetComponent<TileScript>().myColor = Color.white;

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
            if (theTile)
            {

                theTile.myColor = Color.grey;
                myCamera.UpdateCamera();
            }
        }
    }

    public void ShowSelectedTile(GridObject obj, Color color)
    {
        TileScript theTile = GetTile(obj);
        theTile.myColor = color;
        myCamera.UpdateCamera();
    }
    private List<TileScript> pathTiles;
    private List<TileScript> comfirmedTiles;
    public bool StartCanMoveCheck(LivingObject target, TileScript startTile, TileScript targetTile)
    {
        comfirmedTiles = new List<TileScript>();
        if (pathTiles == null)
            pathTiles = new List<TileScript>();
        else
            pathTiles.Clear();
        BoolConatainer con = CanMoveToTile(target, startTile, targetTile);
        bool returnedBool = con.result;
        // Debug.Log(con.name);
        //   Debug.Log(target.FullName + " can move to " + targetTile.name + " resulted in " + returnedBool);
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

                if (GetObjectAtTile(current).GetComponent<LivingObject>())
                {
                    if (GetObjectAtTile(current).GetComponent<LivingObject>().FACTION != target.FACTION)
                    {
                        continue;
                    }
                }
                else
                {
                    Debug.Log("No obj found at occupied tile: " + current.name);
                }
            }

            if (current == targetTile)
            {
                comfirmedTiles.Add(current);
                //  Debug.Log(target.FullName + " pathing at depth " + depth + "  TRUES, tile at " + current.name);
                conatainer.result = true;
                conatainer.name = current.name;
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
                if (GetObjectAtTile(current).GetComponent<LivingObject>())
                {
                    if (GetObjectAtTile(current).GetComponent<LivingObject>().FACTION != target.FACTION)
                    {
                        continue;
                    }
                }
                else
                {
                    Debug.Log("No obj found at occupied tile: " + current.name);
                }
            }

            conatainer = CanMoveToTile(target, current, targetTile, depth);
            if (conatainer.result == true)
            {
                conatainer.name += " " + current.name;
                return conatainer;
            }
        }

        //   Debug.Log(target.FullName + " pathing at depth " + depth + " returning false");
        return conatainer;
    }
    public List<TileScript> GetMoveAbleTiles(LivingObject target)
    {
        List<TileScript> returnedTiles = new List<TileScript>();
        for (int i = 0; i < MapWidth; i++)
        {
            for (int j = 0; j < MapHeight; j++)
            {
                TileScript temp = tileMap[TwoToOneD(j, MapWidth, i)];
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
                                if (StartCanMoveCheck(target, target.currentTile, tempTile))
                                {
                                    returnedTiles.Add(tempTile);
                                }
                            }
                        }
                    }

                }
            }
        }
        myCamera.UpdateCamera();
        return returnedTiles;
    }

    public List<TileScript> GetMoveAbleTiles(Vector3 target, int MOVE_DIST, LivingObject currObj)
    {
        List<TileScript> returnedTiles = new List<TileScript>();
        for (int i = 0; i < MapWidth; i++)
        {
            for (int j = 0; j < MapHeight; j++)
            {
                TileScript temp = tileMap[TwoToOneD(j, MapWidth, i)];
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
                    if (GetObjectAtTile(tempTile) == null && tempTile.isOccupied == false)
                    {

                        if (tempTile != GetTileAtIndex(GetTileIndex(target)))
                        {

                            if (xDist + yDist <= MoveDist)
                            {
                                //    Debug.Log("tile = " + tempTile.transform.position);
                                if (StartCanMoveCheck(currObj, currObj.currentTile, tempTile))
                                    returnedTiles.Add(tempTile);
                            }
                        }
                    }

                }
            }
        }

        return returnedTiles;
    }
    public void ShowGridObjectMoveArea(LivingObject obj)
    {
        for (int i = 0; i < MapWidth; i++)
        {
            for (int j = 0; j < MapHeight; j++)
            {
                TileScript temp = tileMap[TwoToOneD(j, MapWidth, i)];
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
                    if (StartCanMoveCheck(obj, obj.currentTile, temp.GetComponent<TileScript>()))
                    {
                        temp.GetComponent<TileScript>().myColor = Color.cyan;
                    }
                    else
                    {
                        temp.GetComponent<TileScript>().myColor = Color.white;
                    }
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
        for (int i = 0; i < MapWidth; i++)
        {
            for (int j = 0; j < MapHeight; j++)
            {
                TileScript temp = tileMap[TwoToOneD(j, MapWidth, i)];
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
                GridObject gridObject = GetObjectAtTile(atile);
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
            }
        }
        TileScript temp = tileMap[TileIndex];
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
            if (obj.currentTile.name.Contains("23"))
            {
                Debug.Log("got em in comfirm");
            }
        }
        else
        {
            obj.currentTile = tileMap[tileIndex].GetComponent<TileScript>();
        }
        myCamera.currentTile = tileMap[tileIndex].GetComponent<TileScript>();
        myCamera.infoObject = GetObjectAtTile(tileMap[tileIndex].GetComponent<TileScript>());
        myCamera.UpdateCamera();

    }

    public void CheckDoorPrompt()
    {
        TileScript tile = player.current.currentTile;
        if (tile.ROOM >= 0)
        {

            if (currentState != State.EnemyTurn && currentState != State.HazardTurn)
            {
                menuManager.ShowNone();
                newSkillEvent.caller = this;
                newSkillEvent.name = tile.ROOM.ToString();
                CreateEvent(this, tile, "door event", CheckNewSKillEvent, null, -1, DoorStart);

            }
        }

    }
    public void ComfirmMoveGridObject(GridObject obj, TileScript tile)
    {
        if (obj.gameObject != tempObject)
        {
            obj.currentTile.isOccupied = false;
            obj.currentTile = tile;
            obj.currentTile.isOccupied = true;
            if (obj.currentTile.name.Contains("23"))
            {
                Debug.Log("got em in comfirm2");
            }
        }
        else
        {
            obj.currentTile = tile;
        }
        myCamera.currentTile = tile;
        myCamera.infoObject = GetObjectAtTile(tile);
        myCamera.UpdateCamera();
        if (tile.ROOM >= 0)
        {
            if (obj.GetComponent<LivingObject>())
            {

                if (currentState != State.EnemyTurn && currentState != State.HazardTurn)
                {
                    newSkillEvent.caller = this;
                    newSkillEvent.name = tile.ROOM.ToString();
                    CreateEvent(this, tile, "door event", CheckNewSKillEvent, null, -1, DoorStart);

                }
            }
        }
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
        if ((int)checkPosition.z >= MapHeight)
            return -1;
        if ((int)checkPosition.x >= MapWidth)
            return -1;
        if ((int)checkPosition.z < 0)
            return -1;
        if ((int)checkPosition.x < 0)
            return -1;
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
            if (index >= 0)
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
            index = GetTileIndex(possiblePossitions[i]);
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

                    if (skill.SUBTYPE == SubSkillType.Buff || skill.ELEMENT == Element.Support)
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
            //   Debug.Log("Selecting menu item from obj :" + invokingObject.FullName);
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
        if (currentState == State.PlayerTransition)
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
    public void anchorHpBar()
    {
        if (potential)
        {
            potential.anchor();
        }
    }
    public void DamageLivingObject(LivingObject dmgObject, int dmg)
    {


        dmgObject.STATS.HEALTH -= dmg;
        if (dmgObject.ARMOR.DamageArmor(dmg))
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
                react.reaction = Reaction.none;

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
                mod = 2.0f;
                react.reaction = Reaction.weak;
                break;

            case EHitType.savage:

                react.reaction = Reaction.turnloss;
                mod = 2.0f;

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


        }
        else
        {
            resist = dmgObject.RESIESTANCE;

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
            if (alteration == Reaction.reduceDef)
            {
                resist *= 0.5f;
            }
            calc = ((float)attackingObject.STRENGTH * reduction * increasedDmg) * (attackingObject.STRENGTH / resist);
        }
        else
        {
            if (alteration == Reaction.reduceRes)
            {
                resist *= 0.5f;
            }
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
    public DmgReaction CalcDamage(LivingObject attackingObject, LivingObject dmgObject, CommandSkill skill, Reaction alteration = Reaction.none, bool applyAccuraccy = true)
    {
        //  Debug.Log("Calc 1");
        switch (skill.SUBTYPE)
        {
            case SubSkillType.Buff:
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
            case SubSkillType.Ailment:
                //  ApplyEffect(dmgObject, skill.EFFECT, skill.ACCURACY, skill);
                return new DmgReaction() { damage = 0, reaction = Reaction.AilmentOnly };
                break;
            default:
                {
                    int chance = skill.ACCURACY;
                    int result = (int)Random.Range(0.0f, 100.0f);
                    if (applyAccuraccy == false)
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

    public DmgReaction CalcDamage(AtkConatiner conatiner, bool applyAccuraccy = true)
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
            if (applyAccuraccy == false)
            {
                chance = 100;
            }
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
            DmgReaction react = CalcDamage(attackingObject, dmgObject, weapon.AFINITY, weapon.ATTACK_TYPE, weapon.ATTACK, alteration);
            react.atkName = weapon.NAME;
            return react;
        }
        else
        {
            return new DmgReaction() { damage = 0, reaction = Reaction.missed };
        }
    }

    public float ApplyDmgMods(LivingObject living, float dmg, Element atkAffinity)
    {
        List<PassiveSkill> passives = living.GetComponent<InventoryScript>().PASSIVES;
        living.ApplyPassives();
        for (int i = 0; i < passives.Count; i++)
        {
            if (passives[i].ModStat == ModifiedStat.ElementDmg)
            {
                for (int k = 0; k < passives[i].ModElements.Count; k++)
                {
                    if (passives[i].ModElements[k] == atkAffinity)
                    {
                        dmg += (100.0f / (float)passives[i].ModValues[k]);
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
                    //  v3.z -= 0.1f;
                    gao.transform.position = v3;
                    gao.transform.localRotation = Quaternion.Euler(90, 0, 0);
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
            //  v3.z -= 0.1f;
            gao.transform.position = v3;
            gao.transform.localRotation = Quaternion.Euler(90, 0, 0);

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
            case Reaction.ApplyEffect:
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
            case Reaction.AilmentOnly:
                break;
            case Reaction.debuff:
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
                            if (react.usedSkill.SUBTYPE == SubSkillType.Buff)
                            {
                                CreateDmgTextEvent("+", Common.green, target);
                            }
                            else
                            {
                                CreateDmgTextEvent("-", Common.red, target);
                            }

                        }
                        else if (react.reaction == Reaction.AilmentOnly)
                        {

                            CreateDmgTextEvent(react.usedSkill.NAME, Color.red, target);
                        }
                        else if (react.reaction > Reaction.missed)
                        {
                            CreateDmgTextEvent(react.reaction.ToString(), Common.red, target);
                            CreateDmgTextEvent(react.damage.ToString(), Common.green, target);
                        }
                        else if (react.reaction > Reaction.weak)
                        {
                            float colorVal = 0.0f;
                            if (react.damage > 0)
                            {
                                colorVal = (float)react.damage * 2.5f / 100.0f;
                            }
                            CreateDmgTextEvent(react.reaction.ToString(), Common.red, target);
                            CreateDmgTextEvent(react.damage.ToString(), new Color(Mathf.Min(1, colorVal), 0.5f, 0), target);

                            if (react.reaction == Reaction.reflected)
                                CreateDmgTextEvent(react.damage.ToString(), new Color(Mathf.Min(1, colorVal), 0.1f, 0), attackingObject);
                            else
                                CreateDmgTextEvent(react.damage.ToString(), new Color(Mathf.Min(1, colorVal), 0.1f, 0), target);
                        }
                        else
                        {
                            float colorVal = 0.0f;
                            if (react.damage > 0)
                            {
                                colorVal = (float)react.damage * 2.5f / 100.0f;
                            }
                            if (react.reaction == Reaction.reflected)
                                CreateDmgTextEvent(react.damage.ToString(), new Color(Mathf.Min(1, colorVal), 0.1f, 0), attackingObject);
                            else
                                CreateDmgTextEvent(react.damage.ToString(), new Color(Mathf.Min(1, colorVal), 0.1f, 0), target);
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
                    if (react.reaction == Reaction.reflected)
                        gao = PrepareGridAnimation(null, attackingObject);
                    else
                        gao = PrepareGridAnimation(null, target);
                    gao.type = gtype;
                    gao.magnitute = react.damage;
                    CreateEvent(this, gao, "Animation request: " + AnimationRequests + "", CheckAnimation, gao.StartCountDown, 0);



                }
            }
        }

        if (react.reaction != Reaction.missed)
        {
            bool killedEnemy = CheckForDeath(attackingObject, target, react);

            DetermineExp(attackingObject, target, killedEnemy);
            if (react.usedSkill)
            {
                react.usedSkill.GrantXP(1);
            }
            if (currentState != State.EnemyTurn && currentState != State.HazardTurn)
            {
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

    public bool CheckForDeath(LivingObject attackingObject, LivingObject target, DmgReaction react)
    {


        bool killedEnemy = false;
        if (target.HEALTH <= 0)
        {
            if (!target.DEAD)
            {

                if (turnOrder.Contains(target))
                {
                    turnOrder.Remove(target);
                }
                target.DEAD = true;//gameObject.SetActive(false);
                if (react.usedSkill)
                {
                    //if (react.usedSkill.AUGMENTS)
                    //{
                    //    react.usedSkill.AUGMENTS.costTrigger++;
                    //    BoolConatainer conatainer = react.usedSkill.CheckAugment();
                    //    if (conatainer.result == true)
                    //    {
                    //        CreateTextEvent(this, "The " + conatainer.name + " for " + react.usedSkill.NAME + " is ready", "Augment event", CheckText, TextStart);
                    //    }
                    //}
                    //else
                    //{
                    //    Debug.Log("no Augments found");
                    //}

                }
                if (target.GetComponent<HazardScript>())
                {
                    HazardScript hazard = target.GetComponent<HazardScript>();
                    UsableScript useable = hazard.GiveReward(attackingObject);
                    if (useable != null)
                    {
                        //   if (useable.GetType() == typeof(SkillScript))
                        //   {
                        //  SkillScript newSkill = (SkillScript)useable;
                        // newSkillEvent.caller = attackingObject;
                        //newSkillEvent.data = newSkill;
                        if (attackingObject.FACTION == Faction.ally)
                        {

                            LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                            learnContainer.attackingObject = attackingObject;
                            learnContainer.usable = useable;

                            CreateEvent(this, learnContainer, "New Skill Event", CheckNewSKillEvent, null, -1, NewSkillStart);
                            CreateTextEvent(this, "" + attackingObject.FullName + " learned a new skill. Equip in inventory ", "new skill event", CheckText, TextStart);
                        }
                        else
                        {
                            attackingObject.INVENTORY.USEABLES.Add(useable);
                            if (hazard.dropsSkill)
                            {
                                if (useable.GetType().IsSubclassOf(typeof(CommandSkill)))
                                {

                                    attackingObject.INVENTORY.CSKILLS.Add((CommandSkill)useable);
                                    if (attackingObject.BATTLE_SLOTS.SKILLS.Count < 6)
                                        attackingObject.BATTLE_SLOTS.SKILLS.Add((CommandSkill)useable);
                                }
                            }
                            CreateTextEvent(this, "" + attackingObject.FullName + " learned a new skill.", "new skill event", CheckText, TextStart);
                        }
                        //    }
                        //else
                        //{
                        //    // newSkill = (WeaponScript)useable;
                        //    newSkillEvent.caller = attackingObject;
                        //    newSkillEvent.data = useable;
                        //    ChangeSkillPrompt(attackingObject.FullName, useable);
                        //    CreateEvent(this, null, "New Skill Event", CheckNewSKillEvent, null, -1, NewSkillStart);
                        //    CreateTextEvent(this, "" + attackingObject.FullName + " learned a new skill. Equip in inventory ", "new skill event", CheckText, TextStart);
                        //}
                    }
                }

                if (target.GetComponent<EnemyScript>())
                {
                    EnemyScript enemy = target.GetComponent<EnemyScript>();
                    if (enemy.INVENTORY.USEABLES.Count > 0)
                    {

                        float chance = 100.0f;
                        float result = Random.value * 100;
                        if (result <= chance)
                        {
                            int cmdnum = Random.Range(0, enemy.INVENTORY.USEABLES.Count - 1);
                            UsableScript useable = enemy.INVENTORY.USEABLES[cmdnum];
                            if (cmdnum < 0)
                            {
                                cmdnum = 0;
                            }
                            if (useable != null)
                            {

                                if (useable.GetType().IsSubclassOf(typeof(SkillScript)))
                                {
                                    useable = database.LearnSkill(useable.INDEX, attackingObject);
                                }
                                else if (useable.GetType() == typeof(WeaponScript))
                                {

                                    useable = database.GetWeapon(useable.INDEX, attackingObject);
                                }
                                else if (useable.GetType() == typeof(ArmorScript))
                                {
                                    useable = database.GetArmor(useable.INDEX, attackingObject);
                                }
                                if (attackingObject.FACTION == Faction.ally)
                                {

                                    LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                                    learnContainer.attackingObject = attackingObject;
                                    learnContainer.usable = useable;

                                    CreateEvent(this, learnContainer, "New Skill Event", CheckNewSKillEvent, null, -1, NewSkillStart);
                                    CreateTextEvent(this, "" + attackingObject.FullName + " learned a new skill. Equip in inventory ", "new skill event", CheckText, TextStart);
                                }
                            }
                            else
                            {
                                Debug.Log("couldn't drop anything");
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Enemy got no skills");
                    }
                }
                if (target.currentTile)
                {

                    target.currentTile.isOccupied = false;
                    target.currentTile = null;
                }
                gridObjects.Remove(target);
                target.Die();
                // target.GetComponent<SpriteRenderer>().color = Common.semi;
                if (target.FACTION != Faction.ally)
                {
                    killedEnemy = true;

                }
                LivingObject[] livingObjects = GameObject.FindObjectsOfType<LivingObject>();
                bool winner = false;
                bool survivor = false;
                for (int i = 0; i < livingObjects.Length; i++)
                {
                    LivingObject liver = livingObjects[i];
                    if (liver.FACTION == Faction.enemy)
                    {
                        winner = false;
                    }
                    if (liver.FACTION == Faction.hazard)
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
        }
        return killedEnemy;
    }

    public void ShowExpBar()
    {
        expbar.gameObject.SetActive(true);
        expbar.updating = true;
    }
    public bool UpdateExpBar(Object data)
    {
        if (!expbar)
            return true;
        return !expbar.updating;
    }
    public int DetermineExp(LivingObject atker, LivingObject target, bool killed)
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
        return amount;
    }
    public void DetermineAtkExp(LivingObject atker, LivingObject target, CommandSkill skill, bool basic, int dmg)
    {
        //if (expbar)
        //{

        //    expbar.currentUser = atker;
        //    expbar.slider.value = atker.BASE_STATS.EXP;
        //}
        int realnum = dmg;//(int) (dmg * 0.5f);
        int diff = target.LEVEL - atker.LEVEL + realnum;
        int amount = diff + 4;

        if (amount < 0)
        {
            amount = 0;
        }
        if (basic)
        {
            atker.GainSklExp(amount);
        }

        if (skill)
        {
            if (skill.ETYPE == EType.physical)
            {
                atker.GainPhysExp(amount);
            }
            if (skill.ETYPE == EType.magical)
            {
                atker.GainMagExp(amount);
            }
        }



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
                    //   v3.z -= 0.1f;
                    dto.transform.position = v3;
                    dto.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    dto.transform.Rotate(90, 0, 0);
                    dto.text.color = color;
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
            Vector3 v3 = new Vector3(target.transform.position.x, 1, target.transform.position.z);
            // v3.z -= 0.1f;
            dto.transform.position = v3;
            dto.transform.localRotation = Quaternion.Euler(Vector3.zero);
            dto.transform.Rotate(90, 0, 0);
            dto.text.color = color;
        }
        CreateEvent(this, dto, "DmgText request: " + dmgRequest + "", CheckDmgText, dto.StartCountDown, 0);

    }
    public bool AttackTargets(LivingObject invokingObject, CommandSkill skill)
    {
        bool hitSomething = false;
        if (currentAttackList.Count > 0)
        {
            float modification = 1.0f;
            if (skill.ETYPE == EType.magical)
                modification = invokingObject.STATS.SPCHANGE;

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
                                    else if (skill.RTYPE == RanngeType.area)
                                    {
                                        acceptable = true;
                                    }
                                    else
                                    {
                                        CreateTextEvent(this, target.FullName + " is not a valid target for " + skill.NAME, "validation text", CheckText, TextStart);
                                        PlayExitSnd();
                                        return hitSomething;
                                    }

                                }



                            }


                        }
                        if (acceptable == true)
                        {
                            skill.UseSkill(invokingObject, modification);
                        }
                        for (int i = 0; i < targetIndicies.Count; i++)
                        {
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
                                conatiner.dmgObject = GetObjectAtTile(currentAttackList[targetIndicies[i]]).GetComponent<LivingObject>();

                                conatiners.atkConatiners.Add(conatiner);
                                CreateEvent(this, conatiner, "Skill use event", AttackEvent);
                                // }
                            }
                        }
                        CreateEvent(this, conatiners, "Skill use event", CheckForOppChanceEvent);


                    }

                }
            }
            //if (currentState != State.EnemyTurn || currentState != State.HazardTurn)
            //{
            //    SkillScript newSkill = skill.UseSkill(invokingObject, modification);
            //    if (newSkill != null)
            //    {
            //        LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
            //        learnContainer.attackingObject = invokingObject;
            //        learnContainer.usable = newSkill;
            //        CreateEvent(this, learnContainer, "New Skill Event", CheckNewSKillEvent, null, -1, NewSkillStart);
            //        CreateTextEvent(this, "" + invokingObject.FullName + " learned a new skill. Equip in inventory ", "new skill event", CheckText, TextStart);
            //    }
            //}
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
        float realChance = (chance / 360.0f) * Random.Range(0.8f, 1.5f);
        float valres = Random.value;
        // Debug.Log("Chance: " + realChance + ", Reuslt: " + valres);
        if (skill)
            if (skill.SUBTYPE == SubSkillType.Ailment)
                realChance = 100.0f;
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
                            ef.EFFECT = SideEffect.poison;
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
                            ef.EFFECT = SideEffect.paralyze;
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
                            ef.EFFECT = SideEffect.sleep;
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
                            ef.EFFECT = SideEffect.freeze;
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
                            ef.EFFECT = SideEffect.burn;
                            target.ESTATUS = StatusEffect.burned;
                        }
                    }
                    break;

                case SideEffect.bleed:
                    if (!target.GetComponent<EffectScript>())
                    {
                        CreateTextEvent(this, target.FullName + " has been paralyzed", "auto atk", CheckText, TextStart);

                        EffectScript ef = target.gameObject.AddComponent<EffectScript>();
                        ef.EFFECT = SideEffect.paralyze;
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


        DmgReaction react;// =  CalcDamage(container);
                          //react.usedSkill = container.command;
                          //container.alteration = react.reaction;
        if (skill.SUBTYPE == SubSkillType.RngAtk)
        {
            skill.HITS = Random.Range(skill.MIN_HIT, skill.MAX_HIT);
        }
        for (int k = 0; k < skill.HITS; k++)
        {

            react = CalcDamage(container);
            container.alteration = react.reaction;
            react.usedSkill = container.command;
            container.react = react;
            //ApplyReaction(container.attackingObject, container.dmgObject, react, container.attackingElement);
            AtkConatiner newcon = new AtkConatiner(container);

            CreateEvent(this, newcon, "apply reaction event", ApplyReactionEvent, null, 0);
            if (react.reaction < Reaction.nulled && react.reaction != Reaction.missed)
                DetermineAtkExp(container.attackingObject, container.dmgObject, container.command, false, (int)container.command.DAMAGE);
            if (react.reaction < Reaction.nulled)
            {
                if (newcon.command.EFFECT != SideEffect.none)
                {
                    if (newcon.command.EFFECT == SideEffect.debuff)
                    {
                        ApplyEffect(newcon.dmgObject, newcon.command.EFFECT, (float)newcon.command.DAMAGE * newcon.attackingObject.SKILL, newcon.command);

                    }
                    else
                    {
                        if (newcon.command.SUBTYPE == SubSkillType.Ailment)
                        {
                            ApplyEffect(newcon.dmgObject, newcon.command.EFFECT, 100.0f, newcon.command);
                        }
                        else
                        {
                            ApplyEffect(newcon.dmgObject, newcon.command.EFFECT, (float)newcon.command.DAMAGE * newcon.attackingObject.SKILL);

                        }

                    }
                }
            }
            if (react.reaction == Reaction.debuff)
            {
                if (skill)
                {
                    LivingObject target = newcon.dmgObject;
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
        }
        // currentState = State.PlayerTransition;
        enterStateTransition();

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
                            PlayOppSnd();
                            // tempObject.transform.position = doubleAdjOppTiles[0].transform.position;
                            //ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), doubleAdjOppTiles[0]);
                            MoveCameraAndShow(GetObjectAtTile(doubleAdjOppTiles[0]));
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
        container.react = react;
        //  ApplyReaction(container.attackingObject, container.dmgObject, react, container.attackingElement);
        CreateEvent(this, container, "apply reaction event", ApplyReactionEvent);
        if (react.reaction < Reaction.nulled && react.reaction != Reaction.missed)
            DetermineAtkExp(container.attackingObject, container.dmgObject, null, true, react.damage);

        //currentState = State.PlayerTransition;
        enterStateTransition();
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

        StackNewSelection(State.PlayerOppSelecting, currentMenu.command);
        showOppAdjTiles();
        menuManager.ShowNone();

    }
    public void DoorStart(Object data)
    {
        TileScript doorTile = data as TileScript;
        ChangeToDoorPrompt(doorTile);
        menuManager.ShowNewSkillPrompt();
        currentState = State.GotoNewRoom;
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
                ChangeSkillPrompt(container.attackingObject.FullName, newSkill);
            }
            else
            {
                ChangeSkillPrompt(container.attackingObject.FullName, container.usable);
            }

            StackNewSelection(State.AquireNewSkill, menuStack[menuStack.Count - 1].menu);
            menuManager.ShowNewSkillPrompt();
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
        }
    }

    public void MoveCameraAndShow(GridObject obj)
    {
        if (obj != null)
        {
            if (obj.currentTile != null)
            {
                tempObject.transform.position = obj.currentTile.transform.position;
                ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), obj.currentTile);
                if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                    ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                ShowSelectedTile(tempObject.GetComponent<GridObject>());
            }
        }
    }
    public void PhaseAnnounceStart(Object data)
    {
        LivingObject invokingObj = data as LivingObject;
        // Debug.Log("announcing  phase: " + currentState);
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
                        theColor = Color.blue;
                        turnText = "Player Phase";
                        break;
                    case Faction.enemy:
                        theColor = Color.red;
                        turnText = "Enemy Phase";
                        break;
                    case Faction.hazard:
                        theColor = Color.yellow;
                        turnText = "Glyph Phase";
                        break;
                    case Faction.other:
                        theColor = Color.green;
                        turnText = "Unknown Phase";
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

    public bool NewRoomRoundBegin(Object data)
    {
        //considering this is the only time used
        currentState = State.HazardTurn;
        doubleAdjOppTiles.Clear();
        //  Debug.Log("At next round begin");
        NextRound();
        //player.current = turnOrder[0];
        //CreateEvent(this, turnOrder[0], "Initial Camera Event", CameraEvent);
        return true;
    }
    public bool ApplyReactionEvent(Object data)
    {

        AtkConatiner conatiner = data as AtkConatiner;

        ApplyReaction(conatiner.attackingObject, conatiner.dmgObject, conatiner.react, conatiner.attackingElement);

        return true;
    }
    public bool NextTurnEvent(Object data)
    {

        //          Debug.Log(" is done with their turn, moving on ");
        //currOppList.Clear();

        if (currentState != State.PlayerTransition)
            doubleAdjOppTiles.Clear();

        if (currentState != State.EnemyTurn && currentState != State.HazardTurn)
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
            if (turnOrder[i].ACTIONS > 0)
            {
                nextround = false;
                break;
            }
        }
        if (nextround == true)
        {
            //  Debug.Log("next round from end turn in " + currentState + "  by?: " + currentObject.NAME);
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
    public bool BufferedCamUpdate(Object data)
    {
        myCamera.UpdateCamera();
        return true;
    }
    public bool BufferedReturnEvent(Object data)
    {
        returnState();
        myCamera.UpdateCamera();
        return true;
    }
    public bool BufferedStateChange(Object data)
    {

        currentState = prevState;
        myCamera.UpdateCamera();
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

        if (stackManager)
        {
            if (currentState != State.ShopCanvas)
            {

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

    public void StackOptions()
    {

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
                detailsScreen.currentObj = myCamera.infoObject.GetComponent<LivingObject>();
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
    public void YesPrompt()
    {
        if (currentState == State.AquireNewSkill)
        {
            GotoSpecialEquip();
        }
        else if (currentState == State.GotoNewRoom)
        {
            int roomNum = -1;
            if (int.TryParse(newSkillEvent.name, out roomNum))
            {
                LoadDScene(roomNum);
            }
            CreateEvent(this, null, "return state event", BufferedCleanEvent);
            newSkillEvent.caller = null;

            //NextRound();
            CreateEvent(this, null, "next round event", NewRoomRoundBegin);
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

    private void ChangeSkillPrompt(string objName, UsableScript skill)
    {
        if (prompt)
        {
            if (prompt.text)
            {

                if (!skill)
                    return;
                if (skill.GetType() == typeof(WeaponScript))
                {
                    prompt.text.text = objName + " learned a new basic attack. Go to equip attack?";
                }
                else if (skill.GetType() == typeof(ArmorScript))
                {
                    prompt.text.text = objName + " learned a new ward spell. Go to equip ward?";
                }
                else
                {
                    ChangeSkillPrompt(objName, (SkillScript)skill);
                }
            }
        }
    }

    private void ChangeToDoorPrompt(TileScript doorTile)
    {
        if (prompt)
        {
            if (prompt.text)
            {
                prompt.text.text = "Exit to " + doorTile.MAP + "?";
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
                            player.current.INVENTORY.WEAPONS.Add((WeaponScript)shopScreen.SELECTED.refItem);
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
                            break;
                        case 2:
                            player.current.INVENTORY.CSKILLS.Add((CommandSkill)shopScreen.SELECTED.refItem);
                            player.current.BATTLE_SLOTS.SKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);
                            player.current.INVENTORY.CSKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);
                            player.current.INVENTORY.SKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);

                            break;
                        case 3:
                            player.current.INVENTORY.PASSIVES.Add((PassiveSkill)shopScreen.SELECTED.refItem);
                            player.current.PASSIVE_SLOTS.SKILLS.Remove((PassiveSkill)shopScreen.REMOVING.refItem);
                            player.current.INVENTORY.PASSIVES.Remove((PassiveSkill)shopScreen.REMOVING.refItem);
                            player.current.INVENTORY.SKILLS.Remove((PassiveSkill)shopScreen.REMOVING.refItem);
                            break;
                        case 4:
                            player.current.INVENTORY.AUTOS.Add((AutoSkill)shopScreen.SELECTED.refItem);
                            player.current.AUTO_SLOTS.SKILLS.Remove((AutoSkill)shopScreen.REMOVING.refItem);
                            player.current.INVENTORY.AUTOS.Remove((AutoSkill)shopScreen.REMOVING.refItem);
                            player.current.INVENTORY.SKILLS.Remove((AutoSkill)shopScreen.REMOVING.refItem);
                            break;
                        case 5:
                            player.current.INVENTORY.OPPS.Add((OppSkill)shopScreen.SELECTED.refItem);
                            player.current.OPP_SLOTS.SKILLS.Remove((OppSkill)shopScreen.REMOVING.refItem);
                            player.current.INVENTORY.OPPS.Remove((OppSkill)shopScreen.REMOVING.refItem);
                            player.current.INVENTORY.SKILLS.Remove((OppSkill)shopScreen.REMOVING.refItem);
                            break;
                    }

                    SHOPLIST.Remove(shopScreen.SELECTED.refItem);

                    shopScreen.SHOPITEMS = SHOPLIST;
                    shopScreen.PreviousMenu();
                    shopScreen.PreviousMenu();
                    shopScreen.loadShopList();
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

                                player.current.BATTLE_SLOTS.SKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.CSKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.SKILLS.Remove((CommandSkill)shopScreen.REMOVING.refItem);
                                break;
                            case 3:

                                player.current.PASSIVE_SLOTS.SKILLS.Remove((PassiveSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.PASSIVES.Remove((PassiveSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.SKILLS.Remove((PassiveSkill)shopScreen.REMOVING.refItem);
                                break;
                            case 4:

                                player.current.AUTO_SLOTS.SKILLS.Remove((AutoSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.AUTOS.Remove((AutoSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.SKILLS.Remove((AutoSkill)shopScreen.REMOVING.refItem);
                                break;
                            case 5:

                                player.current.OPP_SLOTS.SKILLS.Remove((OppSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.OPPS.Remove((OppSkill)shopScreen.REMOVING.refItem);
                                player.current.INVENTORY.SKILLS.Remove((OppSkill)shopScreen.REMOVING.refItem);
                                break;
                        }

                        database.GetItem(Random.Range(0, 17), player.current);

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
