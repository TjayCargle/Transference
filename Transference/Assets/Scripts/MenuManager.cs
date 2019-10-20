using System.Collections;
using TMPro;
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
    public Canvas optionsCanvas;
    private TextMeshProUGUI descText;
    public ScrollRect ItemRect;
    public InventoryMangager inManager;
    public bool isSetup = false;
    public CameraScript myCamera;
    public Canvas actCanvas;
    public Canvas detailCanvas;
    public Canvas newSkillCanvas;
    public Canvas shopCanvas;
    public Canvas gameOverCanvas;
    GridObject prevObj;
    public Canvas newSkillAnnounceCanvas;
    public Canvas hiddenCanvas;
    public Canvas eventCanvas;

    public Canvas hackingCanvas;

    public TextMeshProUGUI DESC
    {
        get { return descText; }
    }
    public void Setup()
    {

        if (descCanvas)
        {
            if (descCanvas.GetComponentInChildren<TextMeshProUGUI>(true))
            {
                descText = descCanvas.GetComponentInChildren<TextMeshProUGUI>(true);
            }
            descCanvas.gameObject.SetActive(false);
        }
        //ShowCommandCanvas();
        isSetup = true;
    }
    void Start()
    {
        if (!isSetup)
        {
            Setup();
        }
    }

    public void ShowHiddenCanvas()
    {
        if (hiddenCanvas)
        {
            hiddenCanvas.gameObject.SetActive(true);
            inManager.setContentAndScroll(hiddenCanvas.GetComponentInChildren<VerticalLayoutGroup>().gameObject, hiddenCanvas.GetComponentInChildren<ScrollRect>(), 0, null);
            inManager.ForceSelect();
        }
    }
    public void DontHiddenCanvas()
    {
        if (hiddenCanvas)
        {
            hiddenCanvas.gameObject.SetActive(false);
        }
    }


    public void ShowEventCanvas()
    {
        if (eventCanvas)
        {
            eventCanvas.gameObject.SetActive(true);
            inManager.setContentAndScroll(eventCanvas.GetComponentInChildren<VerticalLayoutGroup>().gameObject, eventCanvas.GetComponentInChildren<ScrollRect>(), 0, null);
            inManager.ForceSelect();
        }
    }
    public void DontEventCanvas()
    {
        if (eventCanvas)
        {
            eventCanvas.gameObject.SetActive(false);
        }
    }

    public void ShowNewSkillPrompt()
    {
        if(newSkillCanvas)
        {
            newSkillCanvas.gameObject.SetActive(true);
        }
    }
    public void DontShowNewSkillPrompt()
    {
        if (newSkillCanvas)
        {
            newSkillCanvas.gameObject.SetActive(false);
        }
    }

    public void ShowNewSkillAnnouncement()
    {
        if (newSkillAnnounceCanvas)
        {
            newSkillAnnounceCanvas.gameObject.SetActive(true);
        }
    }
    public Canvas GetNewSkillCanvas()
    {
        return newSkillAnnounceCanvas;
    }
    public void DontShowNewSkillAnnoucement()
    {
        if (newSkillAnnounceCanvas)
        {
            newSkillAnnounceCanvas.gameObject.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverCanvas)
        {
            gameOverCanvas.gameObject.SetActive(true);
        }
    }
    public void DontShowGameOver()
    {
        if (gameOverCanvas)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }
    }
    public void ShowActCanvas()
    {
        if (commandCanvas)
        {
            commandCanvas.gameObject.SetActive(false);
        }
        if(hackingCanvas)
        {
            hackingCanvas.gameObject.SetActive(false);
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
        if (optionsCanvas)
        {
            optionsCanvas.gameObject.SetActive(false);
        }
        if (shopCanvas)
        {
            shopCanvas.gameObject.SetActive(false);
        }
        if (actCanvas)
        {
            actCanvas.gameObject.SetActive(true);
            inManager.setContentAndScroll(actCanvas.GetComponentInChildren<VerticalLayoutGroup>().gameObject, actCanvas.GetComponentInChildren<ScrollRect>(), 0, null);
            inManager.ForceSelect();
        }
        if (newSkillCanvas)
        {
            newSkillCanvas.gameObject.SetActive(false);
        }
        if(eventCanvas)
        {
            DontEventCanvas();
        }
    }

    public void ShowOptions()
    {
        if (shopCanvas)
        {
            shopCanvas.gameObject.SetActive(false);
        }
        if (optionsCanvas)
        {
            optionsCanvas.gameObject.SetActive(true);
        }
    }
    public void DontShowOptions()
    {
        if (optionsCanvas)
        {
            optionsCanvas.gameObject.SetActive(false);
        }
    }
    public void ShowHack()
    {
        if (hackingCanvas)
        {
            hackingCanvas.gameObject.SetActive(true);
        }
    }
    public void DontShowHack()
    {
        if (hackingCanvas)
        {
            hackingCanvas.gameObject.SetActive(false);
        }
    }
    public void ShowShop()
    {
        if (shopCanvas)
        {
            shopCanvas.gameObject.SetActive(true);
        }
    }
    public void DontShowShop()
    {
        if (shopCanvas)
        {
            shopCanvas.gameObject.SetActive(false);
        }
    }
    public void ShowDetails()
    {
        if (myCamera)
        {
            ShowNone();
            myCamera.showActions = false;
       //     prevObj = myCamera.infoObject;
        //    myCamera.infoObject = null;
            myCamera.UpdateCamera();
        }

        if (detailCanvas)
        {
            detailCanvas.gameObject.SetActive(true);
        }
    }
    public void dontShowDetails()
    {
       
        if (myCamera)
        {
            myCamera.showActions = true;
        //    myCamera.infoObject = prevObj;
            myCamera.UpdateCamera();

        }

        
        if (detailCanvas)
        {
            detailCanvas.gameObject.SetActive(false);
        }
    }
    public void ShowNone()
    {
        if (hackingCanvas)
        {
            hackingCanvas.gameObject.SetActive(false);
        }
        if (hiddenCanvas)
        {
            hiddenCanvas.gameObject.SetActive(false);
        }
        if (shopCanvas)
        {
            shopCanvas.gameObject.SetActive(false);
        }
        if (myCamera)
        {
            myCamera.showActions = false;
        }
        if (actCanvas)
        {
            actCanvas.gameObject.SetActive(false);
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
        if (optionsCanvas)
        {
            optionsCanvas.gameObject.SetActive(false);
        }
        if (shopCanvas)
        {
            shopCanvas.gameObject.SetActive(false);
        }
        if (detailCanvas)
        {
            detailCanvas.gameObject.SetActive(false);
        }
        if (newSkillCanvas)
        {
            newSkillCanvas.gameObject.SetActive(false);
        }
        if (eventCanvas)
        {
            DontEventCanvas();
        }
    }
    public void ShowCommandCanvas()
    {
        if (hackingCanvas)
        {
            hackingCanvas.gameObject.SetActive(false);
        }
        if (actCanvas)
        {
            actCanvas.gameObject.SetActive(false);
        }
        if (myCamera)
        {
            myCamera.showActions = true;
        }
        if (actCanvas)
        {
            actCanvas.gameObject.SetActive(false);
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
        if (optionsCanvas)
        {
            optionsCanvas.gameObject.SetActive(false);
        }
        if (shopCanvas)
        {
            shopCanvas.gameObject.SetActive(false);
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
        if (newSkillCanvas)
        {
            newSkillCanvas.gameObject.SetActive(false);
        }
        if (eventCanvas)
        {
            DontEventCanvas();
        }
    }

    public void ShowInventoryCanvas()
    {
        if (hackingCanvas)
        {
            hackingCanvas.gameObject.SetActive(false);
        }
        if (actCanvas)
        {
            actCanvas.gameObject.SetActive(false);
        }
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
        if (newSkillCanvas)
        {
            newSkillCanvas.gameObject.SetActive(false);
        }
        if (eventCanvas)
        {
            DontEventCanvas();
        }
    }

    public void ShowItemCanvas(int index, LivingObject invokingObject)
    {
        if (actCanvas)
        {
            actCanvas.gameObject.SetActive(false);
        }
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
        if (newSkillCanvas)
        {
            newSkillCanvas.gameObject.SetActive(false);
        }

        if (eventCanvas)
        {
            DontEventCanvas();
        }
    }
    public void showOpportunityOptions(LivingObject invokingObject)
    {
        if (actCanvas)
        {
            actCanvas.gameObject.SetActive(false);
        }
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
        if (newSkillCanvas)
        {
            newSkillCanvas.gameObject.SetActive(false);
        }

        if (eventCanvas)
        {
            DontEventCanvas();
        }
    }
    public void ShowSkillCanvas()
    {
        if (actCanvas)
        {
            actCanvas.gameObject.SetActive(false);
        }
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
        if (newSkillCanvas)
        {
            newSkillCanvas.gameObject.SetActive(false);
        }
        if (eventCanvas)
        {
            DontEventCanvas();
        }
    }

    public void ShowExtraCanvas(int index, LivingObject invokingObject)
    {
        if (actCanvas)
        {
            actCanvas.gameObject.SetActive(false);
        }
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

    public void ShowExtraCanvas(UsableScript useable, LivingObject invokingObject)
    {
        if (actCanvas)
        {
            actCanvas.gameObject.SetActive(false);
        }
        if (extraCanvas)
        {
            extraCanvas.gameObject.SetActive(true);
            if (inManager)
            {
                if (extraCanvas.GetComponentInChildren<VerticalLayoutGroup>())
                {
                    if (extraCanvas.GetComponentInChildren<ScrollRect>())
                    {
                        inManager.loadExtra(useable, invokingObject);
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

    public void ShowDesc()
    {
        if (descCanvas)
        {
            descCanvas.gameObject.SetActive(true);
     
        }
    }
}