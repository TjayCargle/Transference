using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItem : MonoBehaviour
{
    private enum MenuItemType
    {
        Move = 0,
        Attack,
        Equip,
        Wait,
        Look,
        Skill,
        Inventory,

    }
    ManagerScript myManager;
    public int itemType;
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
                    if (myManager.currentAttackList.Count > 0)
                    {
                        bool foundSomething = false;
                        for (int i = 0; i < myManager.currentAttackList.Count; i++)
                            if (myManager.GetObjectAtTile(myManager.currentAttackList[i]) != null)
                            {
                                foundSomething = true;
                                if (myManager.SetGridObjectPosition(myManager.tempObject.GetComponent<GridObject>(), myManager.currentAttackList[i].transform.position) == true)
                                {
                                    myManager.ComfirmMoveGridObject(myManager.tempObject.GetComponent<GridObject>(), myManager.GetTileIndex(myManager.tempObject.GetComponent<GridObject>()));

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
                myManager.currentState = State.PlayerEquipping;
                break;
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
            case MenuItemType.Inventory:
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
                break;

        }
        myManager.currentState = State.PlayerInput;
    }
    public void CancelAction(GridObject invokingObject)
    {
        myManager.currentState = State.PlayerInput;
    }
}
