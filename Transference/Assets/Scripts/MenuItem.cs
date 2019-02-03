using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
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
        if (myManager.currentState == State.ChangeOptions)
        {
            return;
        }
        if (invokingObject == null)
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
                    myManager.StackNewSelection(State.PlayerMove, currentMenu.act);

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
                        myManager.attackableTiles = myManager.GetWeaponAttackableTiles(liveInvokingObject);
                        myManager.ShowWhite();
                        if (myManager.attackableTiles.Count > 0)
                        {
                            for (int i = 0; i < myManager.attackableTiles.Count; i++)
                            {
                                for (int j = 0; j < myManager.attackableTiles[i].Count; j++)
                                {
                                    myManager.attackableTiles[i][j].myColor = Common.pink;
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
                                                    if(myManager.player.currentSkill)
                                                        reac= myManager.CalcDamage(myManager.player.current, livvy, myManager.player.currentSkill, Reaction.none, false);
                                                    else
                                                        reac= myManager.CalcDamage(myManager.player.current, livvy, myManager.player.current.WEAPON, Reaction.none, false);
                                                    if (reac.reaction > Reaction.weak)
                                                        reac.damage = 0;
                                                    myManager.myCamera.potentialDamage = reac.damage;
                                                    myManager.myCamera.UpdateCamera();
                                                    if (myManager.potential)
                                                    {
                                                        myManager.potential.pulsing = true;
                                                    }
                                                    if (reac.damage == 0)
                                                        Debug.Log("Zero " + " /" + reac.reaction);
                                                }
                                            }
                                        }
                                    }

                                }
                                myManager.currentAttackList[i].myColor = Common.red;
                            }
                            if (foundSomething == false)
                            {
                                if (myManager.SetGridObjectPosition(myManager.tempObject.GetComponent<GridObject>(), myManager.currentAttackList[0].transform.position) == true)
                                {
                                    myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));

                                }

                            }

                            MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
                            if (myMenuManager)
                            {
                                myMenuManager.ShowNone();
                            }
                            //menuStackEntry entry = new menuStackEntry();
                            //entry.state = State.PlayerAttacking;
                            //entry.index = myManager.invManager.currentIndex;
                            //entry.menu = currentMenu.command;
                            //myManager.enterState(entry);
                       
                            myManager.StackNewSelection(State.PlayerAttacking, currentMenu.act);

                        }
                        else
                        {
                            Debug.Log("Couldnt find tiles");
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
                if (invokingObject.GetComponent<LivingObject>())
                {
                    invokingObject.GetComponent<LivingObject>().Wait();
                }
                myManager.NextTurn(invokingObject.FullName);
                myManager.GetComponent<InventoryMangager>().Validate("menu item, waiting");
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
                    myManager.StackNewSelection(State.PlayerEquipping, currentMenu.invMain);
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {
                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(1, liveInvokingObject);
                            myMenuManager.ShowExtraCanvas(5, liveInvokingObject);

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
            case MenuItemType.selectBS:
                {
                    //   Debug.Log("select battle skill");
                    //menuStackEntry entry = new menuStackEntry();
                    //entry.state = State.PlayerEquipping;
                    //entry.index = myManager.invManager.currentIndex;
                    //entry.menu = currentMenu.skillsMain;
                    //myManager.enterState(entry);
                    myManager.StackNewSelection(State.playerUsingSkills, currentMenu.act);

                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {

                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(7, liveInvokingObject);

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
                            myMenuManager.ShowItemCanvas(2, liveInvokingObject);
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
            case MenuItemType.special:
                break;
            case MenuItemType.prevMenu:

                myManager.CreateEvent(this, null, "returning", myManager.BufferedReturnEvent);
                myManager.currentState = State.PlayerTransition;
                break;
            case MenuItemType.generated:

                if (myManager.currentState == State.PlayerOppOptions)
                {
                    myManager.player.useOppAction(myManager.oppObj);
                }
                else
                {
                    myManager.player.useOrEquip();

                }


                break;

            case MenuItemType.selectItem:
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
                }
                break;

            case MenuItemType.selectAct:
                {
                    //myManager.menuManager.ShowActCanvas();

                    myManager.StackActSelection();
                }
                break;
            default:
                break;
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
