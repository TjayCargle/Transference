using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItem : MonoBehaviour
{


    ManagerScript myManager;
    public int itemType = -1;
    public bool inMenuAction = false;
    public RectTransform myRect;
    public UsableScript refItem;
    void Start()
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
    }
    public void ApplyAction(GridObject invokingObject)
    {
        MenuItemType item = (MenuItemType)itemType;
        Debug.Log("Menu item :" + item);
        switch (item)
        {
            case MenuItemType.Move:
                {

                    //myManager.prevState = myManager.currentState;
                    //myManager.currentState = State.PlayerMove
                    menuStackEntry entry = new menuStackEntry();
                    entry.state = State.PlayerMove;
                    entry.index = myManager.invManager.currentIndex;
                    entry.menu = currentMenu.command;
                    myManager.enterState(entry);
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
                                    myManager.attackableTiles[i][j].myColor = Color.red;
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

                                    }

                                }
                                myManager.currentAttackList[i].myColor = Color.green;
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
                            menuStackEntry entry = new menuStackEntry();
                            entry.state = State.PlayerAttacking;
                            entry.index = myManager.invManager.currentIndex;
                            entry.menu = currentMenu.command;
                            myManager.enterState(entry);
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
                    menuStackEntry entry = new menuStackEntry();
                    entry.state = State.PlayerEquippingMenu;
                    entry.index = myManager.invManager.currentIndex;
                    entry.menu = currentMenu.command;
                    myManager.enterState(entry);
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
                myManager.NextTurn();
                myManager.GetComponent<InventoryMangager>().Validate();
                break;
            case MenuItemType.Look:
                {

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
                            myMenuManager.ShowItemCanvas(0, liveInvokingObject);
                            //myManager.updateCurrentMenuPosition(myManager.currentMenuitem);

                        }
                    }
                }
                break;
            case MenuItemType.chooseSkill:
                {
                    Debug.Log("going into select skill");
                    //  myManager.prevState = myManager.currentState;
                    //  myManager.currentState = State.PlayerEquipping;
                    menuStackEntry entry = new menuStackEntry();
                    entry.state = State.PlayerEquippingMenu;
                    entry.index = myManager.invManager.currentIndex;
                    entry.menu = currentMenu.command;
                   myManager.enterState(entry);
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
                    menuStackEntry entry = new menuStackEntry();
                    entry.state = State.PlayerEquipping;
                    entry.index = myManager.invManager.currentIndex;
                    Debug.Log("curr index = " + myManager.invManager.currentIndex);
                    entry.menu = currentMenu.invMain;
                    myManager.enterState(entry);
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {
                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(1, liveInvokingObject);
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
            case MenuItemType.equipSkill:
                {
                    Debug.Log("equip skill");
                    menuStackEntry entry = new menuStackEntry();
                    entry.state = State.PlayerEquippingSkills;
                    entry.index = myManager.invManager.currentIndex;
                    entry.menu = currentMenu.invMain;
                    myManager.enterState(entry);
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {

                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(4, liveInvokingObject);
                            myMenuManager.ShowExtraCanvas(0, invokingObject.GetComponent<LivingObject>());
                            //myManager.updateCurrentMenuPosition(myManager.currentMenuitem);

                        }
                    }
                }
                break;
            case MenuItemType.selectBS:
                {
                    Debug.Log("select battle skill");
                    menuStackEntry entry = new menuStackEntry();
                    entry.state = State.PlayerEquipping;
                    entry.index = myManager.invManager.currentIndex;
                    entry.menu = currentMenu.skillsMain;
                    myManager.enterState(entry);
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
            case MenuItemType.selectAS:
                {
                    Debug.Log("select auto skill");
                    menuStackEntry entry = new menuStackEntry();
                    entry.state = State.PlayerEquipping;
                    entry.index = myManager.invManager.currentIndex;
                    entry.menu = currentMenu.skillsMain;
                    myManager.enterState(entry);
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {

                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(9, liveInvokingObject);

                        }
                    }
                }
                break;
            case MenuItemType.selectPS:
                {
                    Debug.Log("select passive skill");
                    menuStackEntry entry = new menuStackEntry();
                    entry.state = State.PlayerEquipping;
                    entry.index = myManager.invManager.currentIndex;
                    entry.menu = currentMenu.skillsMain;
                    myManager.enterState(entry);
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {

                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(8, liveInvokingObject);

                        }
                    }
                }
                break;
            case MenuItemType.selectOS:
                {
                    Debug.Log("select opp skill");
                    menuStackEntry entry = new menuStackEntry();
                    entry.state = State.PlayerEquipping;
                    entry.index = myManager.invManager.currentIndex;
                    entry.menu = currentMenu.skillsMain;
                    myManager.enterState(entry);
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {

                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(10, liveInvokingObject);

                        }
                    }
                }
                break;
            case MenuItemType.equipOS:
                break;
            default:
                break;
        }
    }
    public void ComfirmAction(GridObject invokingObject)
    {
        MenuItemType item = (MenuItemType)itemType;
        MenuManager myMenuManager = GameObject.FindObjectOfType<MenuManager>();
        switch (item)
        {
            case MenuItemType.Move:
                invokingObject.HASMOVED = true;
                myManager.ComfirmMoveGridObject(invokingObject, myManager.GetTileIndex(invokingObject));
                // myManager.currentState = State.PlayerInput;
                myManager.returnState();
                if (myMenuManager)
                {
                    myMenuManager.ShowCommandCanvas();
                }
                break;
            case MenuItemType.Attack:

                break;
            case MenuItemType.Equip:

                break;
            case MenuItemType.Wait:

                break;
            case MenuItemType.Look:

                break;

            default:
                myManager.returnState();// myManager.currentState = State.PlayerInput;
                break;

        }
    }
    public void CancelAction(GridObject invokingObject)
    {
        switch (myManager.currentState)
        {
            default:
                {

                    Debug.Log("Default");
                    if (myManager.attackableTiles != null)
                    {
                        myManager.attackableTiles.Clear();
                        myManager.ShowWhite();
                    }
                    // myMenuManager.ShowCommandCanvas();
                    myManager.returnState();
                    // myManager.currentState = State.PlayerInput;
                }
                break;
        }
    }
}
