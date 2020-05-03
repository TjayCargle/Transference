using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopScreen : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI[] shopSkills;

    [SerializeField]
    TextMeshProUGUI[] buyerSkills;


    [SerializeField]
    TextMeshProUGUI descriptionText;

    [SerializeField]
    TextMeshProUGUI secondDescriptionText;
    //[SerializeField]
    //public ShopWindow previousWindow = ShopWindow.none;

    [SerializeField]
    public ShopWindow currentWindow = ShopWindow.main;

    [SerializeField]
    private List<ShopWindow> windowStack = new List<ShopWindow>();

    public int selectedContent;
    private int currentCost;
    [SerializeField]
    Image[] selectableContent;

    [SerializeField]
    Image buyerImage;

    [SerializeField]
    LivingObject buyer = null;

    [SerializeField]
    GameObject learnButton;

    [SerializeField]
    GameObject alterButton;

    [SerializeField]
    GameObject forgetButton;

    [SerializeField]
    GameObject comfirmationObj;

    [SerializeField]
    GameObject shopList;

    [SerializeField]
    GameObject buyerlist;

    [SerializeField]
    Text topicText;


    [SerializeField]
    Text buyerNameText;

    [SerializeField]
    GameObject itemList;

    [SerializeField]
    TextMeshProUGUI comfirmText;


    [SerializeField]
    GameObject currentDetails;
    private shopBtn buyingItem;
    private shopBtn sacrificingItem;
    private Augment augment;
    List<UsableScript> usables = null;

    [SerializeField]
    GameObject auglist;

    [SerializeField]
    AugBtn[] possibleAugments;

    [SerializeField]
    TextMeshProUGUI[] buyerItems;

    [SerializeField]
    Text ItemNameText;

    [SerializeField]
    Sprite[] icons;

    [SerializeField]
    Text currentTypeText;

    [SerializeField]
    GameObject leftPanel;

    [SerializeField]
    GameObject rightPanel;

    public List<UsableScript> SHOPITEMS
    {
        get { return usables; }
        set { usables = value; }
    }

    public shopBtn SELECTED
    {
        get { return buyingItem; }
        set { buyingItem = value; }
    }

    public shopBtn REMOVING
    {
        get { return sacrificingItem; }
        set { sacrificingItem = value; }
    }

    public Augment AUGMENT
    {
        get { return augment; }
        set { augment = value; }
    }



    public int selectedType = -1;
    public ShopWindow previousWindow
    {
        get { if (windowStack.Count > 1) { return windowStack[windowStack.Count - 2]; } else return ShopWindow.none; }

    }
    private void Update()
    {
        if (buyerImage)
        {
            if (buyer)
            {
                buyerImage.sprite = buyer.GetComponent<SpriteRenderer>().sprite;

            }
        }
    }

    public void ChangeTopicAndWindow(string phrase, ShopWindow window)
    {
        if (currentWindow == window)
            return;
        topicText.text = phrase;
        //previousWindow = currentWindow;
        windowStack.Add(window);
        currentWindow = window;
        switch (currentWindow)
        {
            case ShopWindow.removingItem:
                {
                    if (SELECTED)
                    {
                        if (SELECTED.refItem)
                        {
                            loadItemList();
                            for (int i = 0; i < possibleAugments.Length; i++)
                            {
                                if (possibleAugments[i].GetComponent<shopBtn>())
                                {
                                    shopBtn augbtn = possibleAugments[i].GetComponent<shopBtn>();
                                    if (possibleAugments[i].AUGMENT == AUGMENT)
                                    {
                                        augbtn.UpdateInactivity(false);
                                    }
                                    else
                                    {
                                        augbtn.UpdateInactivity(true);
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case ShopWindow.augmenting:
                {
                    if (SELECTED)
                    {
                        if (SELECTED.refItem)
                        {
                            loadAugments(SELECTED.refItem);

                            for (int i = 0; i < buyerSkills.Length; i++)
                            {
                                if (buyerSkills[i].transform.parent.GetComponent<shopBtn>())
                                {
                                    shopBtn skillBtn = buyerSkills[i].transform.parent.GetComponent<shopBtn>();
                                    if (SELECTED == skillBtn)
                                    {
                                        skillBtn.UpdateInactivity(false);
                                    }
                                    else
                                    {
                                        skillBtn.UpdateInactivity(true);
                                    }
                                }

                            }
                        }
                    }
                }
                break;
            case ShopWindow.buying:
                {
                    if (buyerImage)
                    {
                        buyerImage.transform.localScale = new Vector3(1, 1, 1);
                    }

                    if (buyerlist)
                    {
                        if (!buyerlist.gameObject.activeInHierarchy)
                        {
                            if (SELECTED)
                            {
                                if (SELECTED.refItem)
                                {
                                    if (SELECTED.refItem.GetType() == typeof(WeaponScript))
                                    {
                                        loadBuyerList(2);
                                    }
                                    else if (SELECTED.refItem.GetType() == typeof(ArmorScript))
                                    {
                                        loadBuyerList(1);
                                    }
                                    else
                                    {
                                        switch (((SkillScript)SELECTED.refItem).ELEMENT)
                                        {
                                            case Element.Passive:
                                                loadBuyerList(5);
                                                break;
                                            case Element.Auto:
                                                loadBuyerList(4);
                                                break;
                                            case Element.Opp:
                                                loadBuyerList(6);
                                                break;
                                            default:
                                                loadBuyerList(0);
                                                break;
                                        }
                                    }

                                    for (int i = 0; i < shopSkills.Length; i++)
                                    {
                                        if (shopSkills[i].transform.parent.GetComponent<shopBtn>())
                                        {
                                            shopBtn skillBtn = shopSkills[i].transform.parent.GetComponent<shopBtn>();
                                            if (SELECTED == skillBtn)
                                            {
                                                skillBtn.UpdateInactivity(false);
                                            }
                                            else
                                            {
                                                skillBtn.UpdateInactivity(true);
                                            }
                                        }

                                    }

                                }
                            }
                            buyerlist.gameObject.SetActive(true);
                            if (secondDescriptionText)
                            {
                                if (secondDescriptionText.transform.parent)
                                {
                                    secondDescriptionText.transform.parent.gameObject.SetActive(true);
                                }
                            }
                        }
                    }
                }
                break;
            case ShopWindow.confirm:
                {
                    if (comfirmationObj)
                    {
                        comfirmationObj.SetActive(true);

                        if (SELECTED && REMOVING)
                        {
                            if (comfirmText && SELECTED.refItem && REMOVING.refItem)
                            {
                                comfirmText.text = "Give up " + REMOVING.refItem.NAME + " for " + SELECTED.refItem.NAME + "?";
                            }

                            for (int i = 0; i < buyerSkills.Length; i++)
                            {
                                if (buyerSkills[i].transform.parent.GetComponent<shopBtn>())
                                {
                                    shopBtn skillBtn = buyerSkills[i].transform.parent.GetComponent<shopBtn>();
                                    if (REMOVING == skillBtn)
                                    {
                                        skillBtn.UpdateInactivity(false);
                                    }
                                    else
                                    {
                                        skillBtn.UpdateInactivity(true);
                                    }
                                }

                                if (SELECTED)
                                {
                                    SELECTED.UpdateInactivity(true);
                                }

                            }
                        }
                        if (previousWindow == ShopWindow.forget)
                        {
                            comfirmText.text = "Give up " + REMOVING.refItem.NAME + " for a random item?";

                            for (int i = 0; i < buyerSkills.Length; i++)
                            {
                                if (buyerSkills[i].transform.parent.GetComponent<shopBtn>())
                                {
                                    shopBtn skillBtn = buyerSkills[i].transform.parent.GetComponent<shopBtn>();
                                    if (REMOVING == skillBtn)
                                    {
                                        skillBtn.UpdateInactivity(false);
                                    }
                                    else
                                    {
                                        skillBtn.UpdateInactivity(true);
                                    }
                                }

                            }
                        }
                        if (previousWindow == ShopWindow.removingItem)
                        {
                            comfirmText.text = "Give up " + REMOVING.refItem.NAME + " in order to " + Common.GetAugmentText(AUGMENT) + SELECTED.refItem.NAME;

                            for (int i = 0; i < buyerItems.Length; i++)
                            {
                                if (buyerItems[i].transform.parent.GetComponent<shopBtn>())
                                {
                                    shopBtn skillBtn = buyerItems[i].transform.parent.GetComponent<shopBtn>();
                                    if (REMOVING == skillBtn)
                                    {
                                        skillBtn.UpdateInactivity(false);
                                    }
                                    else
                                    {
                                        skillBtn.UpdateInactivity(true);
                                    }
                                }

                            }
                        }
                        if (descriptionText)
                        {
                            if (descriptionText.transform.parent)
                            {
                                descriptionText.transform.parent.gameObject.SetActive(false);
                            }
                        }

                        if (secondDescriptionText)
                        {
                            if (secondDescriptionText.transform.parent)
                            {
                                secondDescriptionText.transform.parent.gameObject.SetActive(false);
                            }
                        }

                    }
                }
                break;
            default:
                Debug.Log("I got bad news for ya");
                break;
        }


    }
    public void loadShop(LivingObject newBuyer)
    {
        windowStack.Clear();
        windowStack.Add(ShopWindow.main);
        currentWindow = ShopWindow.main;
        buyer = newBuyer;
        if (buyerImage)
        {
            buyerImage.transform.localScale = new Vector3(1, 1, 1);
        }
        if (learnButton)
            learnButton.SetActive(true);
        if (alterButton)
            alterButton.SetActive(true);
        if (forgetButton)
            forgetButton.SetActive(true);
        if (shopList)
            shopList.SetActive(false);
        if (buyerlist)
            buyerlist.SetActive(false);
        if (currentDetails)
            currentDetails.SetActive(false);
        if (leftPanel)
        {
            leftPanel.SetActive(true);
        }
        AUGMENT = Augment.none;
        selectedType = 0;
        selectedContent = 0;
        topicText.text = "What do you want to do?";
        updateDesc("");

        SELECTED = null;
        REMOVING = null;
    }
    public void updateDesc(string desc, bool use2 = false)
    {
        if (use2)
        {
            if (secondDescriptionText)
            {
                secondDescriptionText.text = desc;
            }
        }
        else if (descriptionText)
        {
            descriptionText.text = desc;
        }
    }
    public void loadCreate(bool addStack = true)
    {
        if (learnButton)
            learnButton.SetActive(false);
        if (alterButton)
            alterButton.SetActive(false);
        if (forgetButton)
            forgetButton.SetActive(false);
        if (shopList)
            shopList.SetActive(true);
        if (buyerlist)
            buyerlist.SetActive(false);
        if (addStack)
        {
            windowStack.Add(ShopWindow.learn);
        }
        currentWindow = ShopWindow.learn;
        topicText.text = "Oh? Looking for something new?";
        updateDesc("");

        if (leftPanel)
        {
            leftPanel.SetActive(true);
        }
        if (rightPanel)
        {
            buyerlist.transform.parent = rightPanel.transform;
        }
        if (buyerImage)
        {
            // buyerImage.transform.localScale = new Vector3(0.5f, 1, 1);
        }

        loadShopList();

    }
    public void loadAlter(bool addStack = true)
    {
        if (learnButton)
            learnButton.SetActive(false);
        if (alterButton)
            alterButton.SetActive(false);
        if (forgetButton)
            forgetButton.SetActive(false);
        if (shopList)
            shopList.SetActive(false);
        if (buyerlist)
            buyerlist.SetActive(true);
        if (addStack)
        {
            windowStack.Add(ShopWindow.alter);
        }
        currentWindow = ShopWindow.alter;
        AUGMENT = Augment.none;
        topicText.text = "Need to make a few adjustments?";
        updateDesc("");
        SELECTED = null;

        if (buyerImage)
        {
            // buyerImage.transform.localScale = new Vector3(0.5f, 1, 1);
        }


        if (buyer)
        {
            if (buyerNameText)
            {
                buyerNameText.text = buyer.NAME;
            }
            selectedType = 0;
            selectedContent = 0;
            if (leftPanel)
            {
                leftPanel.SetActive(true);
                buyerlist.transform.parent = leftPanel.transform;
            }
            loadBuyerList(0);
            if (currentDetails)
                currentDetails.SetActive(true);
            if (currentTypeText)
                currentTypeText.text = "Strikes";

        }
    }
    public void loadForget(bool addStack = true)
    {
        if (learnButton)
            learnButton.SetActive(false);
        if (alterButton)
            alterButton.SetActive(false);
        if (forgetButton)
            forgetButton.SetActive(false);
        if (shopList)
            shopList.SetActive(false);
        if (buyerlist)
            buyerlist.SetActive(true);
        if (addStack)
        {
            windowStack.Add(ShopWindow.forget);
        }
        currentWindow = ShopWindow.forget;
        topicText.text = "What will you give up?";
        updateDesc("");

        if (buyerImage)
        {
            // buyerImage.transform.localScale = new Vector3(0.25f, 0.5f, 1);
        }

        if (buyer)
        {
            if (leftPanel)
            {
                leftPanel.SetActive(false);
            }
            if (rightPanel)
            {
                buyerlist.transform.parent = rightPanel.transform;
            }

            selectedType = 0;
            selectedContent = 0;
            loadBuyerList(0);
            if (currentDetails)
                currentDetails.SetActive(true);

        }
    }

    public void loadShopList()
    {
        if (buyer)
        {
            if (buyerNameText)
            {
                buyerNameText.text = buyer.NAME;
            }
            if (shopList && usables != null)
            {


                int skillCount = usables.Count;
                for (int i = 0; i < 5; i++)
                {
                    if (i < skillCount)
                    {
                        shopSkills[i].text = usables[i].NAME;
                        if (shopSkills[i].transform.parent.GetComponent<shopBtn>())
                        {
                            shopSkills[i].transform.parent.GetComponent<shopBtn>().refItem = usables[i];
                            UpdateIcon(shopSkills[i]);

                        }

                    }
                    else
                    {
                        shopSkills[i].text = "-";
                        if (shopSkills[i].transform.parent.GetComponent<shopBtn>())
                        {
                            shopSkills[i].transform.parent.GetComponent<shopBtn>().refItem = null;
                            UpdateIcon(shopSkills[i], true);

                        }


                    }
                }
            }
        }
    }

    private void UpdateIcon(TextMeshProUGUI mesh, bool turnOff = false)
    {
        if (icons != null)
        {
            if (mesh)
            {

                if (mesh.transform.parent)
                {

                    if (mesh.transform.parent.GetComponentInChildren<IconImage>())
                    {
                        if (turnOff == true)
                        {
                            mesh.transform.parent.GetComponentInChildren<IconImage>().LoadImage();
                            mesh.transform.parent.GetComponentInChildren<IconImage>().ICON.color = Common.trans;
                            mesh.transform.parent.GetComponentInChildren<IconImage>().ICON.sprite = null;

                        }
                        else
                        {
                            mesh.transform.parent.GetComponentInChildren<IconImage>().LoadImage();
                            mesh.transform.parent.GetComponentInChildren<IconImage>().ICON.color = Color.white;
                            int num = Common.GetIconType(mesh.transform.parent.GetComponent<shopBtn>().refItem);
                            if (num == -1)
                            {
                                Debug.Log("Lost an icon");
                            }
                            else
                            {
                                mesh.transform.parent.GetComponentInChildren<IconImage>().ICON.sprite = icons[num];
                            }
                        }
                    }
                }
            }
        }
    }
    public void loadBuyerList(int listType)
    {
        int skillCount = 0;
        switch (listType)
        {
            case 0:
                skillCount = buyer.INVENTORY.WEAPONS.Count;
                break;
            case 1:
                skillCount = buyer.INVENTORY.ARMOR.Count;
                break;
            case 2:
                skillCount = buyer.PHYSICAL_SLOTS.SKILLS.Count;
                break;
            case 3:
                skillCount = buyer.MAGICAL_SLOTS.SKILLS.Count;
                break;
            case 4:
                skillCount = buyer.PASSIVE_SLOTS.SKILLS.Count;
                break;
            case 5:
                skillCount = buyer.AUTO_SLOTS.SKILLS.Count;
                break;
            case 6:
                skillCount = buyer.INVENTORY.OPPS.Count;
                break;
        }
        if (buyerNameText)
        {
            buyerNameText.text = buyer.NAME;
        }
        if (buyerlist)
        {


            for (int i = 0; i < 5; i++)
            {
                if (i < skillCount)
                {
                    UsableScript refUseable = null;
                    switch (listType)
                    {
                        case 0:
                            refUseable = buyer.INVENTORY.WEAPONS[i];
                            selectedType = 0;
                            if (currentTypeText)
                                currentTypeText.text = "Strikes";
                            break;
                        case 1:
                            refUseable = buyer.INVENTORY.ARMOR[i];
                            selectedType = 1;
                            if (currentTypeText)
                                currentTypeText.text = "Barriers";
                            break;
                        case 2:
                            refUseable = buyer.PHYSICAL_SLOTS.SKILLS[i];
                            selectedType = 2;
                            if (currentTypeText)
                                currentTypeText.text = "Physical Skills";
                            break;
                        case 3:
                            refUseable = buyer.MAGICAL_SLOTS.SKILLS[i];
                            selectedType = 3;
                            if (currentTypeText)
                                currentTypeText.text = "Magical Spells";
                            break;
                        case 4:
                            refUseable = buyer.PASSIVE_SLOTS.SKILLS[i];
                            selectedType = 4;
                            if (currentTypeText)
                                currentTypeText.text = "Passive Skills";
                            break;
                        case 5:
                            refUseable = buyer.AUTO_SLOTS.SKILLS[i];
                            selectedType = 5;
                            if (currentTypeText)
                                currentTypeText.text = "Auto SKills";
                            break;
                        case 6:
                            refUseable = buyer.INVENTORY.OPPS[i];
                            selectedType = 5;
                            if (currentTypeText)
                                currentTypeText.text = "Opportunitty Skills";
                            break;
                    }
                    if (buyerSkills[i].transform.parent.GetComponent<shopBtn>())
                    {
                        buyerSkills[i].transform.parent.GetComponent<shopBtn>().refItem = refUseable;
                        buyerSkills[i].text = refUseable.NAME;
                        UpdateIcon(buyerSkills[i]);
                    }

                }
                else
                {
                    buyerSkills[i].text = "-";
                    if (buyerSkills[i].transform.parent.GetComponent<shopBtn>())
                    {
                        buyerSkills[i].transform.parent.GetComponent<shopBtn>().refItem = null;
                        UpdateIcon(buyerSkills[i], true);
                    }
                }
            }
        }
    }

    public void loadItemList()
    {
        int skillCount = buyer.INVENTORY.ITEMS.Count;
        if (buyerImage)
        {
            if (buyerImage.transform.parent)
            {
                buyerImage.gameObject.transform.parent.gameObject.SetActive(false);
            }
        }

        if (ItemNameText)
        {
            ItemNameText.text = buyer.NAME + " Items";
        }

        if (itemList)
        {
            itemList.SetActive(true);

            for (int i = 0; i < 5; i++)
            {
                if (i < skillCount)
                {
                    UsableScript refUseable = buyer.INVENTORY.ITEMS[i];

                    if (buyerItems[i].transform.parent.GetComponent<shopBtn>())
                    {
                        buyerItems[i].transform.parent.GetComponent<shopBtn>().refItem = refUseable;
                        buyerItems[i].text = refUseable.NAME;
                        UpdateIcon(buyerItems[i]);
                    }

                }
                else
                {
                    buyerItems[i].text = "-";
                    if (buyerItems[i].transform.parent.GetComponent<shopBtn>())
                    {
                        buyerItems[i].transform.parent.GetComponent<shopBtn>().refItem = null;
                        UpdateIcon(buyerItems[i], true);
                    }
                }
            }
        }
    }

    public void loadAugments(UsableScript usingSkill, bool addStack = false)
    {
        if (auglist)
        {
            auglist.SetActive(true);
            if (addStack)
            {
                windowStack.Add(ShopWindow.augmenting);
            }
            currentWindow = ShopWindow.augmenting;
        }
        if (possibleAugments != null)
        {
            List<Augment> useableAugments = null;
            if (usingSkill.GetType().IsSubclassOf(typeof(SkillScript)))
            {
                SkillScript askill = (SkillScript)usingSkill;
                switch (askill.ELEMENT)
                {


                    case Element.Buff:
                        useableAugments = Common.GetSkillAugments();
                        break;
                    case Element.Support:
                        useableAugments = Common.GetSkillAugments();
                        break;
                    case Element.Ailment:
                        useableAugments = Common.GetSkillAugments();
                        break;
                    case Element.Passive:
                        useableAugments = Common.GetSkillAugments();
                        break;
                    case Element.Opp:
                        break;
                    case Element.Auto:
                        useableAugments = Common.GetSkillAugments();
                        break;
                    case Element.none:
                        break;
                    default:
                        {
                            if (usingSkill.GetType() == typeof(CommandSkill))
                            {
                                CommandSkill command = (CommandSkill)usingSkill;
                                switch (command.ETYPE)
                                {
                                    case EType.physical:
                                        if (command.COST < 0)
                                        {
                                            useableAugments = Common.GetPhysCostAugments();
                                        }
                                        else
                                        {
                                            useableAugments = Common.GetPhysChargeAugments();
                                        }
                                        break;
                                    case EType.magical:
                                        useableAugments = Common.GetMagicAugments();
                                        break;
                                }
                            }
                        }
                        break;
                }
            }
            else if (usingSkill.GetType() == typeof(WeaponScript))
            {
                useableAugments = Common.GetAttackAugments();
            }
            else if (usingSkill.GetType() == typeof(ArmorScript))
            {
                useableAugments = Common.GetArmorAugments();
            }

            if (useableAugments != null)
            {
                int useCount = useableAugments.Count;
                for (int i = 0; i < 6; i++)
                {
                    AugBtn augBtn = possibleAugments[i];
                    if (i < useCount)
                    {

                        if (augBtn)
                        {
                            if (usingSkill.AUGMENTS == null)
                            {
                                usingSkill.AUGMENTS = new List<Augment>();
                            }
                            if (!usingSkill.AUGMENTS.Contains(useableAugments[i]))
                            {
                                augBtn.GetComponent<shopBtn>().refItem = null;
                                augBtn.AUGMENT = useableAugments[i];
                                if (augBtn.GetComponentInChildren<TextMeshProUGUI>())
                                    augBtn.GetComponentInChildren<TextMeshProUGUI>().text = Common.GetAugmentNameText(augBtn.AUGMENT);
                                augBtn.GetComponent<shopBtn>().desc = Common.GetAugmentEffectText(augBtn.AUGMENT, usingSkill);
                            }
                            else
                            {
                                augBtn.GetComponent<shopBtn>().refItem = null;
                                augBtn.AUGMENT = Augment.none;
                                if (augBtn.GetComponentInChildren<TextMeshProUGUI>())
                                {
                                    augBtn.GetComponentInChildren<TextMeshProUGUI>().text = "purchased";
                                    augBtn.GetComponent<shopBtn>().UpdateInactivity(true);
                                }
                                augBtn.GetComponent<shopBtn>().desc = "You already purchased this.";
                            }
                        }

                    }
                    else
                    {
                        if (augBtn)
                        {
                            augBtn.GetComponent<shopBtn>().refItem = null;
                            augBtn.AUGMENT = Augment.none;
                            if (augBtn.GetComponentInChildren<TextMeshProUGUI>())
                                augBtn.GetComponentInChildren<TextMeshProUGUI>().text = "-";
                            augBtn.GetComponent<shopBtn>().desc = "";
                        }

                    }
                }
            }
        }
    }

    public void PreviousMenu()
    {

        switch (currentWindow)
        {
            case ShopWindow.none:
                break;
            case ShopWindow.main:
                break;
            case ShopWindow.learn:
                loadShop(buyer);
                break;
            case ShopWindow.alter:
                loadShop(buyer);
                break;
            case ShopWindow.forget:
                loadShop(buyer);
                break;
            case ShopWindow.augmenting:
                {
                    if (previousWindow == ShopWindow.alter)
                    {
                        if (auglist)
                        {
                            auglist.SetActive(false);
                            currentWindow = ShopWindow.alter;
                            topicText.text = "I don't have all day.";
                            AUGMENT = Augment.none;

                            for (int i = 0; i < buyerSkills.Length; i++)
                            {
                                if (buyerSkills[i].transform.parent.GetComponent<shopBtn>())
                                {
                                    shopBtn skillBtn = buyerSkills[i].transform.parent.GetComponent<shopBtn>();

                                    skillBtn.UpdateInactivity(false);
                                }
                            }
                        }
                    }
                }
                break;
            case ShopWindow.buying:
                {
                    if (previousWindow == ShopWindow.learn)
                    {
                        if (buyerlist)
                        {
                            buyerlist.SetActive(false);
                            currentWindow = ShopWindow.learn;
                            topicText.text = "Are you buying something or just browsing?";
                            if (buyerImage)
                            {
                                // buyerImage.transform.localScale = new Vector3(0.5f, 1, 1);
                            }
                            if (secondDescriptionText)
                            {
                                if (secondDescriptionText.transform.parent)
                                {
                                    secondDescriptionText.transform.parent.gameObject.SetActive(false);
                                }
                            }
                            SELECTED = null;

                            for (int i = 0; i < shopSkills.Length; i++)
                            {
                                if (shopSkills[i].transform.parent.GetComponent<shopBtn>())
                                {
                                    shopBtn skillBtn = shopSkills[i].transform.parent.GetComponent<shopBtn>();

                                    skillBtn.UpdateInactivity(false);
                                }
                            }
                        }
                    }
                }
                break;
            case ShopWindow.confirm:
                if (previousWindow == ShopWindow.buying)
                {
                    if (comfirmationObj)
                    {
                        comfirmationObj.SetActive(false);
                        currentWindow = ShopWindow.buying;
                        topicText.text = "Will you make up your mind?";
                        REMOVING = null;
                        SELECTED.UpdateInactivity(false);
                        if (descriptionText)
                        {
                            if (descriptionText.transform.parent)
                            {
                                descriptionText.transform.parent.gameObject.SetActive(true);
                            }
                        }

                        if (secondDescriptionText)
                        {
                            if (secondDescriptionText.transform.parent)
                            {
                                secondDescriptionText.transform.parent.gameObject.SetActive(true);
                            }
                        }
                        for (int i = 0; i < buyerSkills.Length; i++)
                        {
                            if (buyerSkills[i].transform.parent.GetComponent<shopBtn>())
                            {
                                shopBtn skillBtn = buyerSkills[i].transform.parent.GetComponent<shopBtn>();

                                skillBtn.UpdateInactivity(false);
                            }
                        }
                    }
                }
                else if (previousWindow == ShopWindow.forget)
                {
                    if (comfirmationObj)
                    {
                        comfirmationObj.SetActive(false);
                        currentWindow = ShopWindow.forget;
                        topicText.text = "Already forgotten what you wanted?";
                        REMOVING = null;

                        if (descriptionText)
                        {
                            if (descriptionText.transform.parent)
                            {
                                descriptionText.transform.parent.gameObject.SetActive(true);
                            }
                        }
                        for (int i = 0; i < buyerSkills.Length; i++)
                        {
                            if (buyerSkills[i].transform.parent.GetComponent<shopBtn>())
                            {
                                shopBtn skillBtn = buyerSkills[i].transform.parent.GetComponent<shopBtn>();

                                skillBtn.UpdateInactivity(false);
                            }
                        }
                    }
                }
                else if (previousWindow == ShopWindow.removingItem)
                {
                    if (comfirmationObj)
                    {
                        comfirmationObj.SetActive(false);
                        currentWindow = ShopWindow.removingItem;
                        topicText.text = "Second guessing your choice?";
                        REMOVING = null;

                        if (descriptionText)
                        {
                            if (descriptionText.transform.parent)
                            {
                                descriptionText.transform.parent.gameObject.SetActive(true);
                            }
                        }

                        for (int i = 0; i < buyerItems.Length; i++)
                        {
                            if (buyerItems[i].transform.parent.GetComponent<shopBtn>())
                            {
                                shopBtn skillBtn = buyerItems[i].transform.parent.GetComponent<shopBtn>();

                                skillBtn.UpdateInactivity(false);
                            }
                        }

                    }
                }

                break;
            case ShopWindow.removingItem:
                if (previousWindow == ShopWindow.augmenting)
                {
                    if (itemList)
                    {
                        itemList.SetActive(false);
                        currentWindow = ShopWindow.augmenting;
                        topicText.text = "Don't waste my time.";
                        AUGMENT = Augment.none;
                    }
                    if (buyerImage.transform.parent)
                    {
                        buyerImage.gameObject.transform.parent.gameObject.SetActive(true);
                    }

                    for (int i = 0; i < possibleAugments.Length; i++)
                    {
                        if (possibleAugments[i].GetComponent<shopBtn>())
                        {
                            shopBtn augbtn = possibleAugments[i].GetComponent<shopBtn>();

                            augbtn.UpdateInactivity(false);
                        }
                    }
                }
                break;
        }

        windowStack.Remove(windowStack[windowStack.Count - 1]);
    }

}
