using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Canvas commandCanvas;
    public Canvas inventoryCanvas;
    public Canvas itemCanvas;
    public Canvas descCanvas;
    private Text descText;
    public ScrollRect ItemRect;
    public InventoryMangager inManager;
    public bool isSetup = false;

    public Text DESC
    {
        get { return descText; }
    }
    public void Setup()
    {
        if (inventoryCanvas)
        {
            inventoryCanvas.gameObject.SetActive(false);
        }
        if (itemCanvas)
        {
            itemCanvas.gameObject.SetActive(false);
        }

        if(descCanvas)
        {
            if(descCanvas.GetComponentInChildren<Text>())
            {
                descText = descCanvas.GetComponentInChildren<Text>();
            }
            descCanvas.gameObject.SetActive(false);
        }
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
    }
    public void ShowCommandCanvas()
    {
        if (commandCanvas)
        {
            commandCanvas.gameObject.SetActive(true);
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
    }

    public void ShowInventoryCanvas()
    {
        if (commandCanvas)
        {
            commandCanvas.gameObject.SetActive(false);
        }
        if (inventoryCanvas)
        {
            inventoryCanvas.gameObject.SetActive(true);
        }
        if (itemCanvas)
        {
            if(inManager)
            {
                inManager.unloadContents();
            }
            itemCanvas.gameObject.SetActive(false);
        }
        if (descCanvas)
        {
            descCanvas.gameObject.SetActive(false);
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
        if (itemCanvas)
        {
            itemCanvas.gameObject.SetActive(true);
            if (inManager)
            {
                if (itemCanvas.GetComponentInChildren<VerticalLayoutGroup>())
                {
                    if(itemCanvas.GetComponentInChildren<ScrollRect>())
                    {

                    inManager.loadContents(itemCanvas.GetComponentInChildren<VerticalLayoutGroup>().gameObject, itemCanvas.GetComponentInChildren<ScrollRect>(),  index, invokingObject);
                    }
                }
            }
        }
    }


}