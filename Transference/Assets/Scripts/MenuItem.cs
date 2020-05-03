using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{


    ManagerScript myManager;
    public int itemType = -1;
    public bool inMenuAction = false;

    public RectTransform myRect;
    public UsableScript refItem;
    public bool isSetup = false;



    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            //  if (myManager)
            //   myManager.SelectMenuItem(this);
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (myManager)
                myManager.SelectMenuItem(this);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myManager)
            myManager.HoverMenuItem(this);
    }

    public void Setup()
    {
        if (!isSetup)
        {
            if (GameObject.FindObjectOfType<ManagerScript>())
            {
                myManager = GameObject.FindObjectOfType<ManagerScript>();
                if (!myManager.isSetup)
                {
                    myManager.Setup();
                }
            }
            if (GetComponent<RectTransform>())
            {
                myRect = GetComponent<RectTransform>();
            }
            if (!GetComponent<Button>())
            {
                //  gameObject.AddComponent<Button>();
                //  EventTrigger trigger = gameObject.AddComponent<EventTrigger>();


            }

            isSetup = true;
        }
    }
    void Start()
    {
        Setup();
    }

    public void ApplyAction(GridObject invokingObject)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        if (myManager.GetState() == State.ChangeOptions)
        {
            return;
        }
        if (invokingObject == null && myManager.player.current != null)
        {
            return;
        }
        MenuItemType item = (MenuItemType)itemType;
        // Debug.Log("Menu item :" + item);
        switch (item)
        {
            case MenuItemType.Move:
                {

                    //myManager.prevState = myManager.currentState;
                    //myManager.currentState = State.PlayerMove

                    //menuStackEntry entry = new menuStackEntry();
                    //entry.state = State.PlayerMove;
                    //entry.index = myManager.invManager.currentIndex;
                    //entry.menu = currentMenu.command;
                    //myManager.enterState(entry);

                    myManager.StackNewSelection(State.PlayerMove, currentMenu.command);

                    if (invokingObject.GetComponent<AnimationScript>())
                    {
                        AnimationScript anim = invokingObject.GetComponent<AnimationScript>();
                        if (anim.hasMove)
                        {
                            anim.LoadList(anim.movePath);
                        }
                    }

                    MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
                    if (myMenuManager)
                    {
                        myMenuManager.ShowNone();
                    }
                }
                break;
            case MenuItemType.Attack:
                {
                    if (invokingObject.GetComponent<LivingObject>())
                    {
                        LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                        myManager.attackableTiles = myManager.GetAttackableTiles(liveInvokingObject, myManager.player.current.WEAPON.EQUIPPED);
                        myManager.ShowWhite();
                        if (myManager.attackableTiles.Count > 0)
                        {
                            for (int i = 0; i < myManager.attackableTiles.Count; i++)
                            {
                                for (int j = 0; j < myManager.attackableTiles[i].Count; j++)
                                {
                                    myManager.attackableTiles[i][j].MYCOLOR = Common.pink;
                                }
                            }
                            myManager.currentAttackList = myManager.attackableTiles[0];
                            bool foundSomething = false;
                            for (int i = 0; i < myManager.currentAttackList.Count; i++)
                            {

                                if (myManager.GetObjectAtTile(myManager.currentAttackList[i]) != null)
                                {

                                    foundSomething = true;
                                    if (myManager.SetGridObjectPosition(myManager.tempObject.GetComponent<GridObject>(), myManager.currentAttackList[i].transform.position) == true)
                                    {
                                        myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));

                                        myManager.myCamera.potentialDamage = 0;
                                        myManager.myCamera.UpdateCamera();
                                        myManager.anchorHpBar();

                                        GridObject griddy = myManager.GetObjectAtTile(myManager.tempObject.GetComponent<GridObject>().currentTile);
                                        if (griddy)
                                        {
                                            if (griddy.GetComponent<LivingObject>())
                                            {
                                                LivingObject livvy = griddy.GetComponent<LivingObject>();
                                                if (livvy.FACTION != myManager.player.current.FACTION)
                                                {
                                                    DmgReaction reac;
                                                    if (myManager.player.currentSkill)
                                                        reac = myManager.CalcDamage(myManager.player.current, livvy, myManager.player.currentSkill, Reaction.none, false);
                                                    else
                                                        reac = myManager.CalcDamage(myManager.player.current, livvy, myManager.player.current.WEAPON, Reaction.none, false);
                                                    if (reac.reaction > Reaction.weak)
                                                        reac.damage = 0;
                                                    myManager.myCamera.potentialDamage = reac.damage;
                                                    myManager.myCamera.UpdateCamera();
                                                    if (myManager.potential)
                                                    {
                                                        myManager.potential.pulsing = true;
                                                    }

                                                }
                                            }
                                        }
                                    }

                                }
                                myManager.currentAttackList[i].MYCOLOR = Common.red;
                            }
                            if (foundSomething == false)
                            {
                                if (myManager.SetGridObjectPosition(myManager.tempObject.GetComponent<GridObject>(), myManager.player.current.transform.position) == true)
                                {
                                    myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));

                                }

                            }

                            MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
                            if (myMenuManager)
                            {
                                myMenuManager.ShowNone();
                            }
                            myManager.StackNewSelection(State.PlayerAttacking, currentMenu.act);
                            //menuStackEntry entry = new menuStackEntry();
                            //entry.state = State.PlayerAttacking;
                            //entry.index = myManager.invManager.currentIndex;
                            //entry.menu = currentMenu.command;
                            //myManager.enterState(entry);


                        }
                        else
                        {
                              Debug.Log("Looking for weapons?");
                            myManager.CreateTextEvent(this, "No weapon equipped for " + invokingObject.NAME, "no weapon", myManager.CheckText, myManager.TextStart);
                        }

                    }
                    else
                    {
                        myManager.attackableTiles.Clear();
                    }
                    myManager.tempObject.transform.position = myManager.currentObject.transform.position;
                    myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentObject.currentTile;

                }
                break;
            case MenuItemType.Equip:
                {
                    //menuStackEntry entry = new menuStackEntry();
                    //entry.state = State.PlayerEquippingMenu;
                    //entry.index = myManager.invManager.currentIndex;
                    //entry.menu = currentMenu.command;
                    //myManager.enterState(entry);

                    myManager.StackInventory();
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {
                        myMenuManager.ShowInventoryCanvas();
                        myManager.invManager.currentIndex = 0;
                        //myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                    }
                    break;
                }
            case MenuItemType.Wait:
                if (myManager)
                {
                    myManager.myCamera.SetCameraPosFar();
                }
                if (invokingObject.GetComponent<LivingObject>())
                {
                    invokingObject.GetComponent<LivingObject>().Wait();
                }

                break;
            case MenuItemType.Look:
                {
                    Debug.Log("Where u lookin dumbass");
                    return;
                    myManager.tempObject.transform.position = myManager.currentObject.transform.position;
                    myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentObject.currentTile;
                    myManager.GetComponent<MenuManager>().ShowNone();

                    menuStackEntry entry = new menuStackEntry();
                    entry.state = State.FreeCamera;
                    entry.index = myManager.invManager.currentIndex;
                    entry.menu = currentMenu.command;
                    myManager.enterState(entry);
                }
                break;
            case MenuItemType.InventoryWeapon:
                {

                    //menuStackEntry entry = new menuStackEntry();
                    //entry.state = State.PlayerEquipping;
                    //entry.index = myManager.invManager.currentIndex;
                    //entry.menu = currentMenu.invMain;
                    //myManager.enterState(entry);

                    myManager.StackNewSelection(State.PlayerEquipping, currentMenu.invMain);
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {
                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(0, liveInvokingObject);
                            myMenuManager.ShowExtraCanvas(4, liveInvokingObject);

                            //myManager.updateCurrentMenuPosition(myManager.currentMenuitem);

                        }
                    }
                }
                break;
            case MenuItemType.chooseSkill:
                {
                    // Debug.Log("going into select skill");
                    //  myManager.prevState = myManager.currentState;
                    //  myManager.currentState = State.PlayerEquipping;
                    //menuStackEntry entry = new menuStackEntry();
                    //entry.state = State.PlayerEquippingMenu;
                    //entry.index = myManager.invManager.currentIndex;
                    //entry.menu = currentMenu.command;
                    //myManager.enterState(entry);
                    myManager.StackSkills();
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {

                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            myMenuManager.ShowSkillCanvas();

                        }
                    }
                }
                break;
            case MenuItemType.InventoryArmor:
                {
                    //menuStackEntry entry = new menuStackEntry();
                    //entry.state = State.PlayerEquipping;
                    //entry.index = myManager.invManager.currentIndex;
                    //Debug.Log("curr index = " + myManager.invManager.currentIndex);
                    //entry.menu = currentMenu.invMain;
                    //myManager.enterState(entry);
                    myManager.StackNewSelection(State.PlayerEquipping, currentMenu.act);
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {
                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(1, liveInvokingObject);
                            // myMenuManager.ShowExtraCanvas(5, liveInvokingObject);

                            //myManager.updateCurrentMenuPosition(myManager.currentMenuitem);

                        }
                    }
                }
                break;
            case MenuItemType.InventoryAcc:
                {
                    menuStackEntry entry = new menuStackEntry();
                    entry.state = State.PlayerEquipping;
                    entry.index = myManager.invManager.currentIndex;
                    entry.menu = currentMenu.invMain;
                    myManager.enterState(entry);

                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {
                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(2, liveInvokingObject);
                            //myManager.updateCurrentMenuPosition(myManager.currentMenuitem);

                        }
                    }
                }
                break;
            case MenuItemType.equipBS:
                {
                    //Debug.Log("equip cmd skill");
                    //menuStackEntry entry = new menuStackEntry();
                    //entry.state = State.PlayerEquippingSkills;
                    //entry.index = myManager.invManager.currentIndex;
                    //entry.menu = currentMenu.invMain;
                    //myManager.enterState(entry);

                    myManager.StackNewSelection(State.PlayerEquippingSkills, currentMenu.skillsMain);
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {

                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(5, liveInvokingObject);
                            myMenuManager.ShowExtraCanvas(0, invokingObject.GetComponent<LivingObject>());
                            //myManager.updateCurrentMenuPosition(myManager.currentMenuitem);

                        }
                    }
                }
                break;
            case MenuItemType.Skills:
                {

                    myManager.StackNewSelection(State.playerUsingSkills, currentMenu.act);

                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {

                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(5, liveInvokingObject);

                        }
                    }
                }
                break;
            case MenuItemType.equipAS:
                {
                    // Debug.Log("select auto skill");
                    //menuStackEntry entry = new menuStackEntry();
                    //entry.state = State.PlayerEquippingSkills;
                    //entry.index = myManager.invManager.currentIndex;
                    //entry.menu = currentMenu.invMain;
                    //myManager.enterState(entry);

                    myManager.StackNewSelection(State.PlayerEquippingSkills, currentMenu.skillsMain);

                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {

                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            //myMenuManager.ShowItemCanvas(9, liveInvokingObject);
                            myMenuManager.ShowItemCanvas(9, liveInvokingObject);
                            myMenuManager.ShowExtraCanvas(2, invokingObject.GetComponent<LivingObject>());
                        }
                    }
                }
                break;
            case MenuItemType.equipPS:
                {
                    //Debug.Log("select passive skill");
                    //menuStackEntry entry = new menuStackEntry();
                    //entry.state = State.PlayerEquippingSkills;
                    //entry.index = myManager.invManager.currentIndex;
                    //entry.menu = currentMenu.invMain;
                    //myManager.enterState(entry);

                    myManager.StackNewSelection(State.PlayerEquippingSkills, currentMenu.skillsMain);

                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {

                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(6, liveInvokingObject);
                            myMenuManager.ShowExtraCanvas(1, invokingObject.GetComponent<LivingObject>());
                        }
                    }
                }
                break;
            case MenuItemType.equipOS:
                {
                    //Debug.Log("select opp skill");
                    //menuStackEntry entry = new menuStackEntry();
                    //entry.state = State.PlayerEquippingSkills;
                    //entry.index = myManager.invManager.currentIndex;
                    //entry.menu = currentMenu.invMain;
                    //myManager.enterState(entry);

                    myManager.StackNewSelection(State.PlayerEquippingSkills, currentMenu.skillsMain);

                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {

                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            //myMenuManager.ShowItemCanvas(10, liveInvokingObject);
                            myMenuManager.ShowItemCanvas(10, liveInvokingObject);
                            myMenuManager.ShowExtraCanvas(3, invokingObject.GetComponent<LivingObject>());
                        }
                    }
                }
                break;
            case MenuItemType.chooseOptions:

                myManager.StackOptions();

                break;
            case MenuItemType.prevMenu:

                myManager.CreateEvent(this, null, "returning", myManager.BufferedReturnEvent);
                myManager.currentState = State.PlayerTransition;
                break;
            case MenuItemType.generated:

                //  myManager.CreateEvent(this, null, "player user or atk", myManager.ReturnTrue, );
                PlayerUseOrAtk(invokingObject.GetComponent<LivingObject>());

                break;

            case MenuItemType.Items:
                {
                    //Debug.Log("select item");
                    //menuStackEntry entry = new menuStackEntry();
                    //entry.state = State.PlayerSelectItem;
                    //entry.index = myManager.invManager.currentIndex;
                    //entry.menu = currentMenu.invMain;
                    //myManager.enterState(entry);

                    myManager.StackNewSelection(State.PlayerSelectItem, currentMenu.act);

                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {

                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(11, liveInvokingObject);

                        }
                    }
                    InventoryMangager invManager = myManager.GetComponent<InventoryMangager>();
                    if (invManager)
                    {
                        invManager.TurnOffNewDesc();
                    }

                }
                break;

            case MenuItemType.Battle:
                {
                    //myManager.menuManager.ShowActCanvas();

                    myManager.StackActSelection();
                }
                break;
            case MenuItemType.Details:
                {
                    if (invokingObject.GetComponent<LivingObject>())
                    {
                        myManager.detailsScreen.anotherObj = null;
                        myManager.detailsScreen.currentObj = invokingObject.GetComponent<LivingObject>();
                        myManager.StackDetails();
                    }
                    else
                    {
                        myManager.detailsScreen.currentObj = null;
                        myManager.detailsScreen.anotherObj = invokingObject.GetComponent<GridObject>();
                        myManager.StackDetails();
                    }
                   
                }
                break;
            case MenuItemType.Shop:
                {
                    myManager.StackShop();
                }
                break;
            case MenuItemType.Door:
                {
                    Debug.Log("so....");
                    myManager.GotoNewRoom();
                    // myManager.CheckDoorPrompt();
                }
                break;
            case MenuItemType.anEvent:
                {
                    myManager.CheckEventPrompt();
                }
                break;
            case MenuItemType.Spells:
                {

                    myManager.StackNewSelection(State.playerUsingSkills, currentMenu.act);

                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {

                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(12, liveInvokingObject);

                        }
                    }
                }
                break;
            case MenuItemType.Strikes:
                {

                    myManager.StackNewSelection(State.playerUsingSkills, currentMenu.act);

                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {

                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(13, liveInvokingObject);

                        }
                    }
                }
                break;
            case MenuItemType.forceEnd:
                myManager.forceEnd();
                break;
            case MenuItemType.openBattleLog:
                myManager.stackLog();
                break;
            case MenuItemType.yesPrompt:
                myManager.YesPrompt();
                break;
            case MenuItemType.noPrompt:
                myManager.NoPrompt();
                break;
            case MenuItemType.trade:
                break;
            case MenuItemType.Tip:
                {
                    myManager.CheckHelpPrompt();
                }
                break;
            case MenuItemType.Hack:
                {
                    if (myManager)
                    {
                        myManager.BeginHacking();

                    }
                }
                break;
            case MenuItemType.Guard:
                {
                    if (myManager)
                    {
                        myManager.myCamera.SetCameraPosFar();
                    }
                    if (invokingObject.GetComponent<LivingObject>())
                    {
                        invokingObject.GetComponent<LivingObject>().GuardCharge();
                    }

                }
                break;

            case MenuItemType.Talk:
                {
                    if (invokingObject.GetComponent<LivingObject>())
                    {
                        LivingObject living = invokingObject.GetComponent<LivingObject>();
           
                        if (myManager)
                        {

                            EnemyScript enemy = myManager.GetAdjecentEnemy(living);

                            if (enemy)
                            {
                              string response =  enemy.CheckTalkRequirements(living);
                                DatabaseManager database = Common.GetDatabase();
                                if(database)
                                {
                                    SceneContainer scene = database.GenerateScene(enemy.NAME, response, enemy.FACE);
                                    myManager.SetScene(scene);
                                }
                            }

                        }
                    }
                }
                break;

            default:
                break;
        }
    }

    public void PlayerUseOrAtk(LivingObject invokingObject)
    {
        if (myManager.GetState() == State.PlayerOppOptions)
        {
            myManager.player.useOppAction(myManager.oppObj);
        }
        else if (myManager.GetState() == State.AquireNewSkill)
        {
            myManager.player.forgetSkill();
        }
        else
        {
            myManager.player.useOrEquip();
        }
    }
    public bool ComfirmAction(GridObject invokingObject, MenuItemType itemType)
    {
        MenuItemType item = itemType;
        MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
        switch (item)
        {
            case MenuItemType.Move:
                {
                    TileScript checkTile = myManager.GetTile(invokingObject);
                    if (checkTile.isOccupied == false)
                    {

                        myManager.ComfirmMoveGridObject(invokingObject, myManager.GetTileIndex(invokingObject));
                        // myManager.currentState = State.PlayerInput;
                        // myManager.returnState();
                        myManager.CreateEvent(this, null, "return state event", myManager.BufferedReturnEvent);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                break;


            default:
                // myManager.returnState();// myManager.currentState = State.PlayerInput;
                myManager.CreateEvent(this, null, "return state event", myManager.BufferedReturnEvent);

                return true;
                break;

        }
    }
    public bool ComfirmAction(GridObject invokingObject)
    {
        MenuItemType item = (MenuItemType)itemType;
        MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
        switch (item)
        {
            case MenuItemType.Move:
                {
                    TileScript checkTile = myManager.GetTile(invokingObject);
                    if (checkTile.isOccupied == false)
                    {

                        // myManager.currentState = State.PlayerInput;
                        // myManager.returnState();
                        myManager.ComfirmMoveGridObject(invokingObject, myManager.GetTileIndex(invokingObject));
                        myManager.CreateEvent(this, null, "return state event", myManager.BufferedReturnEvent);
                        return true;
                    }
                    else
                    {

                        GridObject obj = null;
                        if ((obj = myManager.GetObjectAtTile(checkTile)) != null)
                        {
                            if (checkTile == invokingObject.currentTile)
                            {
                                myManager.ComfirmMoveGridObject(invokingObject, myManager.GetTileIndex(invokingObject));
                                myManager.CreateEvent(this, null, "return state event", myManager.BufferedReturnEvent);
                                return true;
                            }
                            return false;


                        }
                        else
                        {
                            Debug.Log("No obj found at occupied tile: " + checkTile.name);
                            myManager.SoftReset();
                        }

                        return false;
                    }
                }
                break;


            default:
                // myManager.returnState();// myManager.currentState = State.PlayerInput;
                myManager.CreateEvent(this, null, "return state event", myManager.BufferedReturnEvent);

                return true;
                break;

        }
    }
    public void CancelAction(GridObject invokingObject)
    {
        switch (myManager.currentState)
        {
            default:
                {

                    // myMenuManager.ShowCommandCanvas();
                    // myManager.currentState = State.PlayerInput;
                }
                break;
        }
    }


}
