
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class shopBtn : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int type;
    public string desc;
    public Image myImage;
    public TextMeshProUGUI pro;
    public ShopScreen shop;
    public UsableScript refItem;
    private void Start()
    {
        myImage = GetComponent<Image>();
        pro = GetComponentInChildren<TextMeshProUGUI>();
        shop = GameObject.FindObjectOfType<ShopScreen>();
        //  controller = GameObject.FindObjectOfType<NonCombatController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (!shop)
            {
                shop = GameObject.FindObjectOfType<ShopScreen>();
            }

            if (shop)
            {
                switch (type)
                {
                    case 0:
                        shop.loadCreate();
                        break;
                    case 1:
                        shop.loadAlter();
                        break;
                    case 2:
                        shop.loadForget();
                        break;
                    case 3:
                        if (refItem)
                        {
                            shop.SELECTED = this;
                            shop.ChangeTopicAndWindow("I need something of equivalent value.", ShopWindow.buying);
                        }
                        break;
                    case 4:

                        switch (shop.currentWindow)
                        {

                            case ShopWindow.learn:

                                break;
                            case ShopWindow.alter:
                                {
                                    if (refItem)
                                    {

                                        shop.SELECTED = this;
                                        shop.ChangeTopicAndWindow("So how do you want to change it?", ShopWindow.augmenting);
                                    }
                                }
                                break;
                            case ShopWindow.forget:
                                if (refItem)
                                {

                                    shop.REMOVING = this;
                                    shop.ChangeTopicAndWindow("Gambling with fate?", ShopWindow.confirm);
                                }
                                break;
                            case ShopWindow.buying:
                                {
                                    if (refItem)
                                    {

                                        shop.REMOVING = this;
                                        shop.ChangeTopicAndWindow("Last chance to turn back.", ShopWindow.confirm);
                                    }
                                }
                                break;
                            case ShopWindow.confirm:
                                break;
                        }

                        break;
                    case 5:
                        if (shop.currentWindow != ShopWindow.augmenting && shop.currentWindow != ShopWindow.confirm && shop.currentWindow != ShopWindow.removingItem)
                        {
                            shop.selectedContent--;
                            if (shop.selectedContent < 0)
                            {
                                shop.selectedContent = 4;
                            }
                            shop.loadBuyerList(shop.selectedContent);
                        }
                        return;
                        break;

                    case 6:
                        if (shop.currentWindow != ShopWindow.augmenting && shop.currentWindow != ShopWindow.confirm && shop.currentWindow != ShopWindow.removingItem)
                        {
                            shop.selectedContent++;
                            if (shop.selectedContent > 4)
                            {
                                shop.selectedContent = 0;
                            }
                            shop.loadBuyerList(shop.selectedContent);
                        }
                        return;
                        break;

                    case 7:
                        {
                            if (GetComponent<AugBtn>())
                            {
                                AugBtn aug = GetComponent<AugBtn>();
                                if (aug.AUGMENT < Augment.end && aug.AUGMENT != Augment.none)
                                {
                                    shop.AUGMENT = aug.AUGMENT;
                                    shop.ChangeTopicAndWindow("You must give up an item for this.", ShopWindow.removingItem);
                                }
                            }
                        }
                        break;
                    case 8:
                        {
                            if (refItem)
                            {
                                shop.REMOVING = this;
                                shop.ChangeTopicAndWindow("Last chance to turn back.", ShopWindow.confirm);
                            }
                        }
                        break;
                }

            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!shop)
        {
            shop = GameObject.FindObjectOfType<ShopScreen>();
        }
        if (shop)
        {
            bool change = false;
            switch (type)
            {
                case 0:
                    if (shop.currentWindow == ShopWindow.main)
                        change = true;
                    break;
                case 1:
                    if (shop.currentWindow == ShopWindow.main)
                        change = true;
                    break;
                case 2:
                    if (shop.currentWindow == ShopWindow.main)
                        change = true;
                    break;
                case 3:
                    if (shop.currentWindow == ShopWindow.learn)
                        change = true;
                    break;
                case 4:

                    if (shop.currentWindow == ShopWindow.buying && shop.previousWindow == ShopWindow.learn)
                        change = true;
                    if (shop.currentWindow == ShopWindow.alter)
                        change = true;
                    if (shop.currentWindow == ShopWindow.forget)
                        change = true;
                    break;

                case 5:
                    if (myImage)
                    {
                        myImage.color = Color.yellow;
                    }
                    return;
                    break;

                case 6:
                    if (myImage)
                    {
                        myImage.color = Color.yellow;
                    }
                    return;
                    break;
                case 7:
                    {
                        if (shop.currentWindow == ShopWindow.augmenting)
                            change = true;
                    }
                    break;
                case 8:
                    {
                        if (shop.currentWindow == ShopWindow.removingItem)
                            change = true;
                    }
                    break;
            }

            if (change == true)
            {

              

                if (refItem)
                {
                    if (shop.currentWindow == ShopWindow.buying)
                    {
                        shop.updateDesc(refItem.DESC, true);
                    }
                    else
                    {
                        shop.updateDesc(refItem.DESC);
                    }
                }
                else
                {
                    if (shop.currentWindow == ShopWindow.buying)
                    {
                        shop.updateDesc(desc, true);
                    }
                    else
                    {
                        shop.updateDesc(desc);
                    }

                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (!shop)
        {
            shop = GameObject.FindObjectOfType<ShopScreen>();
        }

        if (!shop)
        {

            bool change = false;
            switch (type)
            {
                case 0:
                    if (shop.currentWindow == ShopWindow.main)
                        change = true;
                    break;
                case 1:
                    if (shop.currentWindow == ShopWindow.main)
                        change = true;
                    break;
                case 2:
                    if (shop.currentWindow == ShopWindow.main)
                        change = true;
                    break;
                case 3:
                    if (shop.currentWindow == ShopWindow.learn)
                        change = true;
                    break;
                case 4:
                    if (shop.currentWindow == ShopWindow.buying && shop.previousWindow == ShopWindow.learn)
                        change = true;
                    if (shop.currentWindow == ShopWindow.alter)
                        change = true;
                    if (shop.currentWindow == ShopWindow.forget)
                        change = true;
                    break;

                case 5:
                    change = true;
                    break;

                case 6:
                    change = true;
                    break;
                case 7:
                    {

                        if (shop.currentWindow == ShopWindow.augmenting)
                            change = true;
                    }
                    break;
                case 8:
                    {
                        if (shop.currentWindow == ShopWindow.removingItem)
                            change = true;
                    }
                    break;
            }
            if (change == true)
            {
                if (myImage)
                {
                    myImage.color = Color.white;
                }
            }
        }

    }

    public void UpdateInactivity(bool inactive)
    {
        if (GetComponent<Button>())
        {
            Button myButton = GetComponent<Button>();
            if (inactive == true)
            {
                myButton.interactable = false;
            }
            else
            {
                myButton.interactable = true;
            }
        }
    }
}
