using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItem : MonoBehaviour
{
    private enum MenuItemType
    {
        Move = 0,
        Attack,
        Skill,
        Equip,
        Wait,
        Look,
        InventoryWeapon,
        InventoryArmor,
        InventoryAcc,

    }
 
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
        switch (item)
        {
            case MenuItemType.Move:
                myManager.currentState = State.PlayerMove;
                break;
            case MenuItemType.Attack:
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

                    }
                }
                else
                {
                    myManager.attackableTiles.Clear();
                }
                myManager.tempObject.transform.position = myManager.currentObject.transform.position;
                myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentObject.currentTile;
                myManager.currentState = State.PlayerAttacking;
                break;
            case MenuItemType.Equip:
                {
                    myManager.currentState = State.PlayerEquippingMenu;
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {
                        myMenuManager.ShowInventoryCanvas();
                        myManager.currentMenuitem = 6;
                        myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                    }
                    break;
                }
            case MenuItemType.Wait:
                myManager.currentState = State.PlayerWait;
                break;
            case MenuItemType.Look:

                myManager.tempObject.transform.position = myManager.currentObject.transform.position;
                myManager.tempObject.GetComponent<GridObject>().currentTile = myManager.currentObject.currentTile;

                myManager.currentState = State.FreeCamera;
                break;
            case MenuItemType.Skill:
                break;
            case MenuItemType.InventoryWeapon:
                {
                    myManager.currentState = State.PlayerEquipping;
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {
                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(0, liveInvokingObject);
                            myManager.updateCurrentMenuPosition(myManager.currentMenuitem);

                        }
                    }
                }
                    break;
            case MenuItemType.InventoryArmor:
                {
                    myManager.currentState = State.PlayerEquipping;
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {
                        if (invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                            myMenuManager.ShowItemCanvas(1, liveInvokingObject);
                            myManager.updateCurrentMenuPosition(myManager.currentMenuitem);

                        }
                    }
                }
                    break;
            case MenuItemType.InventoryAcc:
                {
                    myManager.currentState = State.PlayerEquipping;
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {
                        if(invokingObject.GetComponent<LivingObject>())
                        {
                            LivingObject liveInvokingObject = invokingObject.GetComponent<LivingObject>();
                        myMenuManager.ShowItemCanvas(2, liveInvokingObject);
                        myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                            
                        }
                    }
                }
                    break;

            default:
                break;
        }
    }
    public void ComfirmAction(GridObject invokingObject)
    {
        MenuItemType item = (MenuItemType)itemType;
        switch (item)
        {
            case MenuItemType.Move:
                invokingObject.HASMOVED = true;
                myManager.ComfirmMoveGridObject(invokingObject, myManager.GetTileIndex(invokingObject));
                myManager.currentState = State.PlayerInput;
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
        myManager.currentState = State.PlayerInput;
                break;

        }
    }
    public void CancelAction(GridObject invokingObject)
    {
        switch (myManager.currentState)
        {
            case State.PlayerEquippingMenu:
                {
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {
                        myMenuManager.ShowCommandCanvas();
                    }
                    myManager.currentMenuitem = 2;
                    myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                    myManager.currentState = State.PlayerInput;
                }
                break;
            case State.PlayerEquipping:
 
                {
                    MenuManager myMenuManager = myManager.gameObject.GetComponent<MenuManager>();
                    if (myMenuManager)
                    {
                        myMenuManager.ShowInventoryCanvas();
                    }
                    myManager.currentMenuitem = 6;
                    myManager.updateCurrentMenuPosition(myManager.currentMenuitem);
                    myManager.currentState = State.PlayerEquippingMenu;
                }
                break;
            default:
                myManager.currentState = State.PlayerInput;
                break;
        }
    }
}
