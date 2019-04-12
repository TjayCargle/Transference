using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalDisplay : MonoBehaviour
{

    public State[] displayStates;
    public bool requiresSelected = false;
    public bool isAffectDisplay = false;
    public bool requiresDoorTile = false;
    public bool requiresShopTile = false;
    ManagerScript manager;
    // Use this for initialization
    void Start()
    {
        manager = GameObject.FindObjectOfType<ManagerScript>();
    }

    //public void UpdateDisplay()
    //{
    //    gameObject.SetActive(false);
    //    if (manager)
    //    {
    //        for (int i = 0; i < displayStates.Length; i++)
    //        {
    //            if (manager.currentState == displayStates[i])
    //            {
    //                gameObject.SetActive(true);

    //                break;
    //            }
    //        }

    //    }

    //}

    public void UpdateDisplay(ManagerScript newmanager)
    {
        manager = newmanager;
        gameObject.SetActive(false);
        bool found = false;
        if (requiresSelected == true)
        {
            if (manager.myCamera.infoObject == null)
            {
                gameObject.SetActive(false);
                return;
            }
            if (isAffectDisplay)
            {
                if (manager)
                {
                    LivingObject livvy = manager.myCamera.infoObject.GetComponent<LivingObject>();
                    if (livvy)
                    {
                        if (livvy.INVENTORY.DEBUFFS.Count > 0 || livvy.INVENTORY.BUFFS.Count > 0 || livvy.GetComponent<EffectScript>() || livvy.GetComponent<SecondStatusScript>())
                        {
                            gameObject.SetActive(true);
                        }

                    }
                }
            }



        }

        if (manager)
        {

            if (requiresDoorTile == true)
            {
                if (manager.myCamera.currentTile.TTYPE != TileType.door)
                {
                    gameObject.SetActive(false);
                    return;
                }
            }

            if (requiresShopTile == true)
            {
                if (manager.myCamera)
                {
                    if (manager.myCamera.currentTile)
                    {
                        if (manager.myCamera.currentTile.TTYPE != TileType.shop)
                        {
                            gameObject.SetActive(false);
                            return;
                        }
                    }
                }
            }
            gameObject.SetActive(true);

            for (int i = 0; i < displayStates.Length; i++)
            {
                if (manager.currentState == displayStates[i])
                {
                    gameObject.SetActive(true);
                    found = true;
                    break;
                }
            }

            if (found == false)
            {
                gameObject.SetActive(false);
            }





        }

    }
}
