using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalDisplay : MonoBehaviour
{

    public State[] displayStates;
    public bool requiresSelected = false;
    public bool isAffectDisplay = false;
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
