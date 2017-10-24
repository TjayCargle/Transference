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
    void Start()
    {
        if (GameObject.FindObjectOfType<ManagerScript>())
        {
            myManager = GameObject.FindObjectOfType<ManagerScript>();
            if(!myManager.isSetup)
            {
                myManager.Setup();
            }
        }
        if(GetComponent<RectTransform>())
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
}
