using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalDisplay : MonoBehaviour
{

    public State[] displayStates;
    public bool requiresSelected = false;

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

        if (manager)
        {
            for (int i = 0; i < displayStates.Length; i++)
            {
                if (manager.currentState == displayStates[i])
                {
                    gameObject.SetActive(true);

                    break;
                }
            }


            if (requiresSelected == true)
            {
                if (manager.myCamera.infoObject == null)
                {
                    gameObject.SetActive(false);
                }
            }
        }

    }
}
