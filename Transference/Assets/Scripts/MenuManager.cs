﻿using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Canvas commandCanvas;
    public Canvas inventoryCanvas;
    public Canvas itemCanvas;
    public Canvas descCanvas;
    public Canvas skillCanvas;
    public Canvas extraCanvas;
    private Text descText;
    public ScrollRect ItemRect;
    public InventoryMangager inManager;
    public bool isSetup = false;
    public CameraScript myCamera;

    public Text DESC
    {
        get { return descText; }
    }
    public void Setup()
    {

        if (descCanvas)
        {
            if (descCanvas.GetComponentInChildren<Text>())
            {
                descText = descCanvas.GetComponentInChildren<Text>();
            }
            descCanvas.gameObject.SetActive(false);
        }
        ShowCommandCanvas();
        isSetup = true;
    }
    void Start()
    {
        if (!isSetup)
        {
            Setup();
        }
    }
    
    public void ShowNone()
    {
     if(myCamera)
        {
            myCamera.showActions = false;
        }
        if (commandCanvas)
        {
            commandCanvas.gameObject.SetActive(false);
        }
        if (inventoryCanvas)
        {
            inventoryCanvas.gameObject.SetActive(false);
        }
        if (itemCanvas)
        {
            if (inManager)
            {
                inManager.unloadContents();
            }
            itemCanvas.gameObject.SetActive(false);
        }
        if (descCanvas)
        {
            descCanvas.gameObject.SetActive(false);
        }

        if (inManager)
        {
            inManager.setContentAndScroll(null, null, 0, null);
        }
        if (skillCanvas)
        {
            skillCanvas.gameObject.SetActive(false);
        }
        if (extraCanvas)
        {
            extraCanvas.gameObject.SetActive(false);
        }
    }
    public void ShowCommandCanvas()
    {
        //ManagerScript manager = GameObject.FindObjectOfType<ManagerScript>();
        //if(manager)
        //{
        //    manager.ShowGridObjectAffectArea(manager.currentObject);
        //}
        if (myCamera)
        {
            myCamera.showActions = true;
        }
        if (commandCanvas)
        {
            commandCanvas.gameObject.SetActive(true);

            if (inManager)
            {
                if (commandCanvas.GetComponentInChildren<VerticalLayoutGroup>())
                {
                    if (commandCanvas.GetComponentInChildren<ScrollRect>())
                    {
                        inManager.setContentAndScroll(commandCanvas.GetComponentInChildren<VerticalLayoutGroup>().gameObject, commandCanvas.GetComponentInChildren<ScrollRect>(), 0, null);
                        inManager.ForceSelect();
                    }
                }
            }
        }
        if (inventoryCanvas)
        {
            inventoryCanvas.gameObject.SetActive(false);
        }
        if (itemCanvas)
        {
            if (inManager)
            {
                inManager.unloadContents();
            }
            itemCanvas.gameObject.SetActive(false);
        }
        if (descCanvas)
        {
            descCanvas.gameObject.SetActive(false);
        }
        if (skillCanvas)
        {
            skillCanvas.gameObject.SetActive(false);
        }
        if (extraCanvas)
        {
            extraCanvas.gameObject.SetActive(false);
        }
    }

    public void ShowInventoryCanvas()
    {
        if (commandCanvas)
        {
            commandCanvas.gameObject.SetActive(false);
        }
        if (itemCanvas)
        {
            if (inManager)
            {
                inManager.unloadContents();
            }
            itemCanvas.gameObject.SetActive(false);
        }
        if (descCanvas)
        {
            descCanvas.gameObject.SetActive(false);
        }
        if (skillCanvas)
        {
            skillCanvas.gameObject.SetActive(false);
        }
        if (extraCanvas)
        {
            extraCanvas.gameObject.SetActive(false);
        }
        if (inventoryCanvas)
        {
            inventoryCanvas.gameObject.SetActive(true);
            if (inManager)
            {
                if (inventoryCanvas.GetComponentInChildren<VerticalLayoutGroup>())
                {
                    if (inventoryCanvas.GetComponentInChildren<ScrollRect>())
                    {
                        inManager.setContentAndScroll(inventoryCanvas.GetComponentInChildren<VerticalLayoutGroup>().gameObject, inventoryCanvas.GetComponentInChildren<ScrollRect>(), 0, null);
                        inManager.ForceSelect();
                    }
                }
            }
        }
    }

    public void ShowItemCanvas(int index, LivingObject invokingObject)
    {
        if (commandCanvas)
        {
            commandCanvas.gameObject.SetActive(false);
        }
        if (inventoryCanvas)
        {
            inventoryCanvas.gameObject.SetActive(false);
        }
        if (descCanvas)
        {
            descCanvas.gameObject.SetActive(true);
        }
        if (skillCanvas)
        {
            skillCanvas.gameObject.SetActive(false);
        }
        if (extraCanvas)
        {
            extraCanvas.gameObject.SetActive(false);
        }
        if (itemCanvas)
        {
            itemCanvas.gameObject.SetActive(true);
            if (inManager)
            {
                inManager.unloadContents();
                if (itemCanvas.GetComponentInChildren<VerticalLayoutGroup>())
                {
                    if (itemCanvas.GetComponentInChildren<ScrollRect>())
                    {
                        inManager.loadContents(itemCanvas.GetComponentInChildren<VerticalLayoutGroup>().gameObject, itemCanvas.GetComponentInChildren<ScrollRect>(), index, invokingObject);
                        inManager.ForceSelect();
                    }
                }
            }
        }
    }
    public void showOpportunityOptions(LivingObject invokingObject)
    {
        if (commandCanvas)
        {
            commandCanvas.gameObject.SetActive(false);
        }
        if (inventoryCanvas)
        {
            inventoryCanvas.gameObject.SetActive(false);
        }
        if (descCanvas)
        {
            descCanvas.gameObject.SetActive(true);
        }
        if (skillCanvas)
        {
            skillCanvas.gameObject.SetActive(false);
        }
        if (extraCanvas)
        {
            extraCanvas.gameObject.SetActive(false);
        }
        if (itemCanvas)
        {
            itemCanvas.gameObject.SetActive(true);
            if (inManager)
            {
                inManager.unloadContents();
                if (itemCanvas.GetComponentInChildren<VerticalLayoutGroup>())
                {
                    if (itemCanvas.GetComponentInChildren<ScrollRect>())
                    {
                        inManager.loadContents(itemCanvas.GetComponentInChildren<VerticalLayoutGroup>().gameObject, itemCanvas.GetComponentInChildren<ScrollRect>(), 3, invokingObject);
                        inManager.ForceSelect();
                    }
                }
            }
        }
    }
    public void ShowSkillCanvas()
    {
        if (commandCanvas)
        {
            commandCanvas.gameObject.SetActive(false);
        }
        if (itemCanvas)
        {
            if (inManager)
            {
                inManager.unloadContents();
            }
            itemCanvas.gameObject.SetActive(false);
        }
        if (descCanvas)
        {
            descCanvas.gameObject.SetActive(false);
        }
        if (inventoryCanvas)
        {
            inventoryCanvas.gameObject.SetActive(false);
        }
        if (extraCanvas)
        {
            extraCanvas.gameObject.SetActive(false);
        }
        if (skillCanvas)
        {
            skillCanvas.gameObject.SetActive(true);
            if (inManager)
            {
                if (skillCanvas.GetComponentInChildren<VerticalLayoutGroup>())
                {
                    if (skillCanvas.GetComponentInChildren<ScrollRect>())
                    {
                        inManager.setContentAndScroll(skillCanvas.GetComponentInChildren<VerticalLayoutGroup>().gameObject, skillCanvas.GetComponentInChildren<ScrollRect>(), 0, null);
                        inManager.ForceSelect();
                    }
                }
            }
        }
    }

    public void ShowExtraCanvas(int index, LivingObject invokingObject)
    {

        if (extraCanvas)
        {
            extraCanvas.gameObject.SetActive(true);
            if (inManager)
            {
                if (extraCanvas.GetComponentInChildren<VerticalLayoutGroup>())
                {
                    if (extraCanvas.GetComponentInChildren<ScrollRect>())
                    {
                        inManager.loadExtra(index, invokingObject);
                        //inManager.setContentAndScroll(extraCanvas.GetComponentInChildren<VerticalLayoutGroup>().gameObject, extraCanvas.GetComponentInChildren<ScrollRect>(), 0, null);
                        //inManager.ForceSelect();
                    }
                }
            }
        }
    }

    public void switchToEquiped()
    {
        if (inManager)
        {
            if (extraCanvas)
            {

                if (itemCanvas.GetComponentInChildren<VerticalLayoutGroup>())
                {
                    if (itemCanvas.GetComponentInChildren<ScrollRect>())
                    {
                        inManager.setContentAndScroll(itemCanvas.GetComponentInChildren<VerticalLayoutGroup>().gameObject, itemCanvas.GetComponentInChildren<ScrollRect>(), 0, null);
                        inManager.ForceSelect();
                        inManager.menuSide = -1;
                    }
                }
            }
        }
    }

    public void switchToExtra()
    {
        if (inManager)
        {
            if (extraCanvas.GetComponentInChildren<VerticalLayoutGroup>())
            {
                if (extraCanvas.GetComponentInChildren<ScrollRect>())
                {
                    inManager.setContentAndScroll(extraCanvas.GetComponentInChildren<VerticalLayoutGroup>().gameObject, extraCanvas.GetComponentInChildren<ScrollRect>(), 0, null);
                    inManager.ForceSelect();
                    inManager.menuSide = 1;

                }
            }
        }
    }

}