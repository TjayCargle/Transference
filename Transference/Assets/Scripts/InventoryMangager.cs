using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryMangager : MonoBehaviour
{

    public GameObject menuItem;
    Dictionary<string, GameObject> tempObjects = new Dictionary<string, GameObject>();
    //  public LivingObject testLiveObject;
    public GameObject currentContent;
    public ScrollRect currentRect;
    public int currentIndex = 0;
    public int prevIndex = 0;
    public MenuItem selectedMenuItem;
    private LivingObject lastObject;
    private MenuManager menuManager;
    private ManagerScript manager;
    public GameObject extraContent;
    public ScrollRect extraRect;
    public int menuSide = -1;
    public MenuItem[] itemSlots;
    public MenuItem[] extraSlots;
    UsableScript genericMove;
    UsableScript genericAtk;

    public List<UsableScript> currentList = null;
    public List<UsableScript> extraList = null;
    public int slotIndex;
    //[SerializeField]
    //  private Sprite[] imgTypes;
    [SerializeField]
    private Sprite[] attributeImages;
    public bool isSetup = false;
    public int attrOffset = 120;
    public int otherOffset = 100;

    [SerializeField]
    public GameObject trueExtra;
    public void Setup()
    {
        if (!isSetup)
        {
            menuManager = GameObject.FindObjectOfType<MenuManager>();
            manager = GetComponent<ManagerScript>();
            // imgTypes = Resources.LoadAll<Sprite>("Buttons2/");
            genericMove = ScriptableObject.CreateInstance<UsableScript>();
            genericAtk = ScriptableObject.CreateInstance<UsableScript>();
            //genericWait = ScriptableObject.CreateInstance<UsableScript>();
            genericMove.NAME = "MOVE";
            genericAtk.NAME = "ATTACK";
            //genericWait.name = "WAIT";
            genericMove.DESC = "Allows unit to move.";
            genericAtk.DESC = "Use equipped weapon to attack enemy";
        }
        isSetup = true;
    }

    private void Start()
    {
        Setup();
    }



    // Update is called once per frame
    void Update()
    {
        if (currentRect)
        {
            switch (manager.currentState)
            {
                case State.PlayerInput:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        IncreaseScroll();
                    }
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        IncreaseScroll();
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        DecreaseScroll();
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        DecreaseScroll();
                    }
                    break;
                case State.PlayerEquippingMenu:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        IncreaseScroll();
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        DecreaseScroll();
                    }
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        IncreaseScroll();
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        DecreaseScroll();
                    }
                    break;
                case State.PlayerEquippingSkills:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        IncreaseScroll();



                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        DecreaseScroll();


                    }
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        IncreaseScroll();

                    }

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        DecreaseScroll();

                    }
                    if (extraContent)
                    {
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            if (menuSide == -1)
                            {
                                EquipSkill();
                            }
                            else
                            {
                                UnequipSkill();
                            }
                        }
                        if (extraRect)
                        {
                            if (Input.GetKeyDown(KeyCode.A))
                            {
                                menuManager.switchToEquiped();
                            }
                            if (Input.GetKeyDown(KeyCode.D))
                            {
                                menuManager.switchToExtra();
                            }
                        }
                    }
                    break;
                case State.PlayerEquipping:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        IncreaseScroll();

                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        DecreaseScroll();

                    }
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        IncreaseScroll();
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        DecreaseScroll();
                    }
                    if (extraContent)
                    {
                        if (extraRect)
                        {
                            if (Input.GetKeyDown(KeyCode.Return))
                            {

                            }
                            if (Input.GetKeyDown(KeyCode.A))
                            {
                                menuManager.switchToEquiped();
                                // UpdateColors(itemSlots);
                                //UpdateColors(extraSlots);
                            }
                            if (Input.GetKeyDown(KeyCode.D))
                            {
                                menuManager.switchToExtra();

                            }
                        }
                    }
                    break;
                case State.PlayerSkillsMenu:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        IncreaseScroll();
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        DecreaseScroll();

                    }
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        IncreaseScroll();
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        DecreaseScroll();
                    }
                    break;

                case State.PlayerOppOptions:
                    {
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            IncreaseScroll();

                        }
                        if (Input.GetKeyDown(KeyCode.S))
                        {
                            DecreaseScroll();
                        }
                        if (Input.GetAxis("Mouse ScrollWheel") > 0)
                        {
                            IncreaseScroll();
                        }

                        if (Input.GetAxis("Mouse ScrollWheel") < 0)
                        {
                            DecreaseScroll();
                        }
                    }
                    break;

                case State.PlayerSelectItem:
                    {
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            IncreaseScroll();


                        }
                        if (Input.GetKeyDown(KeyCode.S))
                        {
                            DecreaseScroll();

                        }
                        if (Input.GetAxis("Mouse ScrollWheel") > 0)
                        {
                            IncreaseScroll();
                        }

                        if (Input.GetAxis("Mouse ScrollWheel") < 0)
                        {
                            DecreaseScroll();
                        }
                    }
                    break;
            }

        }

    }
    public void IncreaseScroll()
    {
        if (currentRect)
        {
            if (currentContent)
            {
                Vector2 newPos = currentRect.normalizedPosition;
                newPos.y += 0.01f;

                if (currentContent.transform.childCount > 0)
                {
                    if (selectedMenuItem)
                    {
                        selectedMenuItem.GetComponentInChildren<Text>().color = Color.white;
                        selectedMenuItem.GetComponent<Image>().color = Color.black;
                        Vector2 pos = selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition;
                        pos.x = 0;
                        selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition = pos;
                    }
                    currentIndex--;

                    if (extraContent.activeInHierarchy)
                    {
                        if (menuSide == -1)
                            if (currentIndex == 0 && slotIndex > 5)
                            {
                                ShiftDown();
                                currentIndex++;
                            }

                        if (currentIndex < 0)
                        {
                            currentIndex = Mathf.Min(5, currentList.Count - 1);
                            ShowEnd();
                            if (menuSide == -1)
                                slotIndex = currentList.Count - 1;
                            else
                                slotIndex = extraList.Count - 1;
                        }
                    }
                    else if (currentIndex < 0)
                    {
                        currentIndex = currentList.Count - 1;
                    }

                    if (currentIndex < 0)
                    {
                        currentIndex = currentContent.transform.childCount - 1;
                    }

                    selectedMenuItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
                    if (selectedMenuItem)
                    {
                        selectedMenuItem.GetComponentInChildren<Text>().color = Color.yellow;
                        selectedMenuItem.GetComponent<Image>().color = Color.yellow;
                        Vector2 pos = selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition;
                        pos.x = 15;
                        selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition = pos;

                    }

                    float index = currentIndex / (float)currentContent.transform.childCount;
                    newPos.y = 1.0f - index;

                }
                if (newPos.y > 1)
                    newPos.y = 1;
                currentRect.normalizedPosition = newPos;

                if (menuManager)
                {
                    if (menuManager.DESC)
                    {


                        if (selectedMenuItem)
                        {
                            if (selectedMenuItem.refItem)
                            {
                                menuManager.DESC.text = selectedMenuItem.refItem.DESC;
                                if (selectedMenuItem.refItem.GetType() == typeof(CommandSkill))
                                {
                                    manager.ShowSkillAttackbleTiles(manager.player.current, (selectedMenuItem.refItem as CommandSkill));
                                }
                            }
                        }
                    }
                }
            }
        }

    }
    public void DecreaseScroll()
    {
        if (currentRect)
        {
            if (currentContent)
            {
                if (selectedMenuItem)
                {
                    selectedMenuItem.GetComponentInChildren<Text>().color = Color.white;
                    selectedMenuItem.GetComponent<Image>().color = Color.black;
                    Vector2 pos = selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition;
                    pos.x = 0;
                    selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition = pos;

                }
                Vector2 newPos = currentRect.normalizedPosition;
                newPos.y -= 0.01f;

                if (currentContent.transform.childCount > 0)
                {
                    if (selectedMenuItem)
                    {
                        selectedMenuItem.GetComponentInChildren<Text>().color = Color.white;
                        selectedMenuItem.GetComponent<Image>().color = Color.black;
                        Vector2 pos = selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition;
                        pos.x = 0;
                        selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition = pos;

                    }
                    currentIndex++;
                    if (extraContent.activeInHierarchy)
                    {
                        if (menuSide == -1)
                            if (currentIndex == 5 && slotIndex < currentList.Count - 1)
                            {
                                ShiftUp();
                                currentIndex--;
                            }

                        if (currentIndex > 5)
                        {
                            currentIndex = 0;
                            ShowBegining();
                            slotIndex = 5;
                        }
                        if (currentIndex >= currentList.Count)
                        {
                            currentIndex = 0;
                            ShowBegining();
                            slotIndex = 5;
                        }
                    }
                    else if (currentList.Count > 0)
                    {
                        if (currentIndex > currentList.Count - 1)
                        {
                            currentIndex = 0;

                        }

                    }
                    if (currentIndex > currentContent.transform.childCount - 1)
                    {
                        currentIndex = 0;
                    }

                    selectedMenuItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();

                    if (selectedMenuItem)
                    {
                        selectedMenuItem.GetComponentInChildren<Text>().color = Color.yellow;
                        selectedMenuItem.GetComponent<Image>().color = Color.yellow;
                        Vector2 pos = selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition;
                        pos.x = 15;
                        selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition = pos;

                    }

                    float index = currentIndex / (float)(currentContent.transform.childCount - 1);
                    newPos.y = 1.0f - index;

                }
                if (newPos.y < 0)
                    newPos.y = 0;
                currentRect.normalizedPosition = newPos;

                if (menuManager)
                {
                    if (menuManager.DESC)
                    {


                        if (selectedMenuItem)
                        {
                            if (selectedMenuItem.refItem)
                            {
                                menuManager.DESC.text = selectedMenuItem.refItem.DESC;
                                if (selectedMenuItem.refItem.GetType() == typeof(CommandSkill))
                                {
                                    manager.ShowSkillAttackbleTiles(manager.player.current, (selectedMenuItem.refItem as CommandSkill));
                                }
                            }
                        }
                    }
                }
            }
        }


    }
    public void HoverSelect(MenuItem hoveritem, GameObject content)
    {
        if (currentRect)
        {
            if (currentContent)
            {
                if (selectedMenuItem)
                {
                    selectedMenuItem.GetComponentInChildren<Text>().color = Color.white;
                    selectedMenuItem.GetComponent<Image>().color = Color.black;
                    Vector2 pos = selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition;
                    pos.x = 0;
                    selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition = pos;

                }
                selectedMenuItem = hoveritem;
                currentContent = content;
                currentIndex = currentContent.transform.GetSiblingIndex();
                if (trueExtra)
                {

                    if (currentContent == trueExtra)
                    {

                        menuSide = 1;
                    }
                    else
                    {
                        menuSide = -1;
                    }
                }
                if (selectedMenuItem)
                {
                    selectedMenuItem.GetComponentInChildren<Text>().color = Color.yellow;
                    selectedMenuItem.GetComponent<Image>().color = Color.yellow;
                    Vector2 pos = selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition;
                    pos.x = 15;
                    selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition = pos;

                }

                if (menuManager)
                {
                    if (menuManager.DESC)
                    {


                        if (selectedMenuItem)
                        {
                            if (selectedMenuItem.refItem)
                            {
                                menuManager.DESC.text = selectedMenuItem.refItem.DESC;
                                if (selectedMenuItem.refItem.GetType() == typeof(CommandSkill))
                                {
                                    manager.ShowSkillAttackbleTiles(manager.player.current, (selectedMenuItem.refItem as CommandSkill));
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log("no content!");
            }
        }
        else
        {
            Debug.Log("no rekt!");
        }
    }
    private void ShiftUp()
    {
        if (currentList.Count > 0)
        {
            Debug.Log("shift up");
            for (int i = 0; i < 5; i++)
            {
                itemSlots[i].refItem = itemSlots[i + 1].refItem;
                itemSlots[i].GetComponentInChildren<Text>().text = itemSlots[i + 1].refItem.NAME;
            }
            slotIndex++;

            itemSlots[5].refItem = currentList[slotIndex];
            itemSlots[5].GetComponentInChildren<Text>().text = currentList[slotIndex].NAME;
            Debug.Log("name = " + itemSlots[5].refItem.NAME);
            Debug.Log("index = " + slotIndex);
            // UpdateColors(itemSlots);
            // UpdateColors(extraSlots);
        }
    }
    private void ShiftDown()
    {
        if (menuSide == -1)
        {

            if (currentList.Count > 0)
            {
                if (slotIndex > 5)
                {
                    Debug.Log("shift down");
                    for (int i = 5; i > 0; i--)
                    {
                        // Debug.Log("i = " + i);
                        itemSlots[i].refItem = itemSlots[i - 1].refItem;
                        itemSlots[i].GetComponentInChildren<Text>().text = itemSlots[i - 1].refItem.NAME;
                    }
                    slotIndex--;
                    Debug.Log("index = " + slotIndex);
                    itemSlots[0].refItem = currentList[slotIndex - 5];
                    itemSlots[0].GetComponentInChildren<Text>().text = itemSlots[0].refItem.NAME;
                    Debug.Log("name = " + itemSlots[0].refItem.NAME);
                }
            }
            // UpdateColors(itemSlots);
            // UpdateColors(extraSlots);
        }
    }
    private void ShowBegining()
    {
        if (menuSide == -1)
        {

            for (int useCount = 0; useCount < 6; useCount++) //UsableScript item in liveObject.GetComponents<UsableScript>())
            {
                MenuItem selectableItem = itemSlots[useCount];
                if (useCount < currentList.Count)
                {
                    UsableScript item = currentList[useCount];
                    selectableItem.refItem = item;
                    selectableItem.GetComponentInChildren<Text>().text = item.NAME;
                }
            }
            slotIndex = 5;
        }
        //UpdateColors(itemSlots);
        //UpdateColors(extraSlots);
    }

    private void ShowEnd()
    {
        if (menuSide == -1)
        {

            int realIndex = currentList.Count - 6;
            if (realIndex < 0)
            {
                realIndex = 0;
            }
            for (int useCount = 0; useCount < 6; useCount++) //UsableScript item in liveObject.GetComponents<UsableScript>())
            {
                MenuItem selectableItem = itemSlots[useCount];
                if (useCount < currentList.Count)
                {

                    UsableScript item = currentList[realIndex];
                    selectableItem.refItem = item;
                    selectableItem.GetComponentInChildren<Text>().text = item.NAME;
                    realIndex++;
                }
            }
            slotIndex = currentList.Count - 1;
        }
        // UpdateColors(itemSlots);
        // UpdateColors(extraSlots);
    }

    public void Validate(string caller)
    {
        return;
        if (currentRect)
        {
            if (currentContent)
            {
                //Debug.Log("validating from " + caller);

                if (currentContent.transform.childCount > 0)
                {
                    for (int i = 0; i < currentContent.transform.childCount; i++)
                    {
                        MenuItem temp = currentContent.transform.GetChild(i).GetComponent<MenuItem>();
                        temp.GetComponentInChildren<Text>().color = Color.white;
                        //temp.GetComponent<Image>().sprite = imgTypes[0];
                        if (selectedMenuItem)
                        {

                            selectedMenuItem.GetComponent<Image>().color = Color.black;
                            Vector2 pos = selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition;
                            pos.x = 0;
                            selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition = pos;


                        }

                        if (temp.refItem)
                        {
                            if (lastObject.WEAPON.NAME == temp.refItem.NAME || lastObject.ARMOR.NAME == temp.refItem.NAME || lastObject.ACCESSORY.NAME == temp.refItem.NAME)
                            {
                                //      temp.GetComponent<Image>().sprite = imgTypes[5];
                            }



                        }
                    }
                    if (selectedMenuItem)
                    {
                        selectedMenuItem.GetComponentInChildren<Text>().color = Color.white;
                        selectedMenuItem.GetComponent<Image>().color = Color.black;
                        Vector2 pos = selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition;
                        pos.x = 0;
                        selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition = pos;

                    }

                    if (currentContent.transform.childCount < currentIndex)
                        selectedMenuItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
                    if (selectedMenuItem)
                    {
                        selectedMenuItem.GetComponentInChildren<Text>().color = Color.yellow;
                        selectedMenuItem.GetComponent<Image>().color = Color.yellow;
                        Vector2 pos = selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition;
                        pos.x = 15;
                        selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition = pos;

                    }

                }
            }
        }
    }

    public void ForceSelect()
    {
        if (currentContent)
        {
        for (int i = 0; i < currentContent.transform.childCount; i++)
        {
            MenuItem menuItem = currentContent.transform.GetChild(i).GetComponent<MenuItem>();
            if (menuItem)
            {
                menuItem.GetComponentInChildren<Text>().color = Color.white;
                menuItem.GetComponent<Image>().color = Color.black;
                Vector2 pos = menuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition;
                pos.x = 0;
            }
        }
            if (selectedMenuItem)
            {
                selectedMenuItem.GetComponentInChildren<Text>().color = Color.white;
                selectedMenuItem.GetComponent<Image>().color = Color.black;
                Vector2 pos = selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition;
                pos.x = 0;
                selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition = pos;
                //if (prevIndex < currentContent.transform.childCount)
                //    selectedMenuItem = currentContent.transform.GetChild(prevIndex).GetComponent<MenuItem>();
                //selectedMenuItem.GetComponentInChildren<Text>().color = Color.white;
                //selectedMenuItem.GetComponent<Image>().color = Color.black;
            }
            if (currentContent.transform.childCount > 0)
            {
                if (currentIndex < currentContent.transform.childCount)
                    if (currentContent.transform.GetChild(currentIndex).gameObject.activeInHierarchy)
                    {

                        selectedMenuItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
                        selectedMenuItem.GetComponentInChildren<Text>().color = Color.yellow;
                        selectedMenuItem.GetComponent<Image>().color = Color.yellow;
                        Vector2 pos = selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition;
                        pos.x = 15;
                        selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition = pos;

                    }
                    else
                    {
                        currentIndex = 0;
                        selectedMenuItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
                        selectedMenuItem.GetComponentInChildren<Text>().color = Color.yellow;
                        selectedMenuItem.GetComponent<Image>().color = Color.yellow;
                        Vector2 pos = selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition;
                        pos.x = 15;
                        selectedMenuItem.GetComponentInChildren<Text>().GetComponent<RectTransform>().localPosition = pos;

                    }
            }


        }
    }
    private void UpdateColors(MenuItem[] items)
    {
        return;
        for (int i = 0; i < items.Length; i++)
        {
            MenuItem selectableItem = items[i];
            if (selectableItem.gameObject.activeInHierarchy)
            {
                if (selectableItem.refItem)
                {
                    System.Type itype = selectableItem.refItem.GetType();
                    if (itype != typeof(WeaponScript) && itype != typeof(ArmorScript) && itype != typeof(AccessoryScript) && itype != typeof(ItemScript))
                    {
                        Image selectedImg = selectableItem.GetComponent<Image>();
                        if (selectableItem.refItem)
                        {
                            if (selectableItem.refItem.GetType() != typeof(UsableScript))
                                switch (((SkillScript)selectableItem.refItem).ELEMENT)
                                {
                                    //case Element.Passive:
                                    //    selectedImg.sprite = imgTypes[2];
                                    //    break;
                                    //case Element.Auto:
                                    //    selectedImg.sprite = imgTypes[3];
                                    //    break;
                                    //case Element.Opp:
                                    //    selectedImg.sprite = imgTypes[4];
                                    //    break;
                                    //default:
                                    //    selectedImg.sprite = imgTypes[1];
                                    //    break;
                                }
                        }
                    }
                }
            }
        }

    }
    public void setContentAndScroll(GameObject content, ScrollRect rect, int index, LivingObject liveObject)
    {
        if (liveObject != null)
        {
            lastObject = liveObject;
        }
        currentContent = content;
        currentRect = rect;
    }
    public void loadContents(GameObject content, ScrollRect rect, int index, LivingObject liveObject)
    {

        lastObject = liveObject;
        currentContent = content;
        currentRect = rect;
        currentList.Clear();
        //UsableScript itemType = new UsableScript();
        int useType = -1;
        int windowType = -1;
        //  Debug.Log("index = " + index);
        switch (index)
        {

            case 0:
                // itemType = ScriptableObject.CreateInstance<WeaponScript>();
                // itemType.TYPE = 0;
                useType = 0;
                for (int i = 0; i < liveObject.GetComponent<InventoryScript>().WEAPONS.Count; i++)
                {
                    currentList.Add(liveObject.GetComponent<InventoryScript>().WEAPONS[i]);
                }
                break;
            case 1:
                // itemType = new ArmorScript();
                // itemType.TYPE = 1;
                useType = 1;
                for (int i = 0; i < liveObject.GetComponent<InventoryScript>().ARMOR.Count; i++)
                {
                    currentList.Add(liveObject.GetComponent<InventoryScript>().ARMOR[i]);
                }
                break;
            case 2:
                // itemType = new AccessoryScript();
                // itemType.TYPE = 2;
                useType = 2;
                for (int i = 0; i < liveObject.GetComponent<InventoryScript>().ACCESSORIES.Count; i++)
                {
                    currentList.Add(liveObject.GetComponent<InventoryScript>().ACCESSORIES[i]);
                }
                break;
            case 3:
                // itemType = new ItemScript();
                // itemType.TYPE = 3;
                useType = 4;
                currentList.Add(genericMove);
                currentList.Add(genericAtk);
                for (int i = 0; i < liveObject.GetComponent<InventoryScript>().CSKILLS.Count; i++)
                {
                    currentList.Add(liveObject.GetComponent<InventoryScript>().CSKILLS[i]);
                }
                break;
            case 4:
                // itemType = new SkillScript(); 
                // itemType.TYPE = 4;
                useType = 4;
                windowType = 1; //all skills
                for (int i = 0; i < liveObject.GetComponent<InventoryScript>().SKILLS.Count; i++)
                {
                    currentList.Add(liveObject.GetComponent<InventoryScript>().SKILLS[i]);
                }
                break;
            case 5:
                // itemType = new SkillScript(); 
                // itemType.TYPE = 4;
                useType = 4;
                windowType = 5; //all command skills
                for (int i = 0; i < liveObject.INVENTORY.CSKILLS.Count; i++)
                {
                    currentList.Add(liveObject.INVENTORY.CSKILLS[i]);
                }
                break;

            case 6:
                useType = 4;
                windowType = 2; //all passive skills
                for (int i = 0; i < liveObject.INVENTORY.PASSIVES.Count; i++)
                {
                    currentList.Add(liveObject.INVENTORY.PASSIVES[i]);
                }
                break;

            case 7:
                useType = 4;
                windowType = 3; // useable skill slots
                for (int i = 0; i < liveObject.BATTLE_SLOTS.SKILLS.Count; i++)
                {
                    currentList.Add(liveObject.BATTLE_SLOTS.SKILLS[i]);
                }
                break;

            case 8:
                useType = 4;
                windowType = 5; // passive skill slots
                for (int i = 0; i < liveObject.PASSIVE_SLOTS.SKILLS.Count; i++)
                {
                    currentList.Add(liveObject.PASSIVE_SLOTS.SKILLS[i]);
                }
                break;
            case 9:
                useType = 4;
                windowType = 5; // auto skill slots
                for (int i = 0; i < liveObject.INVENTORY.AUTOS.Count; i++)
                {
                    currentList.Add(liveObject.INVENTORY.AUTOS[i]);
                }
                break;
            case 10:
                useType = 4;
                windowType = 6; // opp skill slots

                for (int i = 0; i < liveObject.INVENTORY.OPPS.Count; i++)
                {
                    currentList.Add(liveObject.INVENTORY.OPPS[i]);
                }
                break;
            case 11:
                useType = 5;
                // items

                for (int i = 0; i < liveObject.INVENTORY.ITEMS.Count; i++)
                {
                    currentList.Add(liveObject.INVENTORY.ITEMS[i]);
                }
                break;
        }
        // for (int i = 0; i < 4; i++)
        //{

        // List<UsableScript> invokingObjectsUse = liveObject.GetComponent<InventoryScript>().USEABLES;
        for (int useCount = 0; useCount < 6; useCount++) //UsableScript item in liveObject.GetComponents<UsableScript>())
        {
            MenuItem selectableItem = itemSlots[useCount];
            selectableItem.itemType = 15;
            Image attr = selectableItem.GetComponentsInChildren<Image>()[1];
            Text selectedText = selectableItem.GetComponentInChildren<Text>();
            selectableItem.gameObject.SetActive(true);
            if (useCount < currentList.Count)
            {
                UsableScript item = currentList[useCount];

                if (selectableItem.GetComponentInChildren<Text>())
                {

                    // RectTransform rectTransform = selectedText.GetComponent<RectTransform>();
                    attr.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    //   rectTransform.position = new Vector3(attrOffset, rectTransform.position.y, rectTransform.position.z);
                    if (item.TYPE != 4)
                    {
                        selectedText.text = item.NAME;
                    }
                    else
                    {

                        selectedText.text = item.NAME;
                        if (windowType == 3 || windowType == -1)
                        {
                            if (manager.currentState == State.PlayerEquipping)
                            {
                                attr.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                                int indxex = (int)((CommandSkill)item).ELEMENT;
                                //Debug.Log("the index is = " + indxex);
                                attr.sprite = attributeImages[indxex];
                                // rectTransform.position = new Vector3(otherOffset, rectTransform.position.y, rectTransform.position.z);
                            }
                            CommandSkill cmd = ((CommandSkill)item);
                            string extraText = "";
                            if (((CommandSkill)item).ETYPE == EType.physical)
                            {
                                extraText = cmd.COST.ToString();// " " + "FT";
                                if (cmd.COST > 0)
                                {
                                    selectedText.text += " FT +" + extraText;

                                }
                                else
                                {
                                    selectedText.text += " FT " + extraText;

                                }

                            }
                            else
                            {
                                extraText = " SP";
                                selectedText.text += " " + ((CommandSkill)item).COST.ToString() + extraText;

                            }
                        }

                    }
                    selectedText.resizeTextForBestFit = true;
                }
                if (selectableItem.GetComponent<MenuItem>())
                {
                    //  MenuItem selectedItem = selectableItem.GetComponent<MenuItem>();
                    item.TYPE = useType;//itemType.TYPE;
                    if (item.NAME.Equals("MOVE") || item.NAME.Equals("ATTACK"))
                    {
                        item.TYPE = 3;
                    }
                    selectableItem.refItem = item;
                    if (index == 3)
                    {
                        if (item == genericMove)
                        {

                            selectableItem.refItem.DESC = "Move a number of tiles";

                        }


                    }
                }


            }
            else
            {
                selectableItem.refItem = null;
                selectedText.text = "";
                if (windowType < 5)
                    selectableItem.gameObject.SetActive(false);
            }


        }

        slotIndex = 5;

        //UpdateColors(itemSlots);

        if (currentRect)
        {
            if (currentContent)
            {
                if (currentContent.transform.childCount > 0)
                    selectedMenuItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
                Validate("inv manager for loading");
            }
        }
        if (menuManager)
        {
            if (menuManager.DESC)
            {


                if (selectedMenuItem)
                {
                    if (selectedMenuItem.refItem)
                    {
                        menuManager.DESC.text = selectedMenuItem.refItem.DESC;

                    }
                }
            }
        }


    }

    public void loadExtra(int index, LivingObject liveObject)
    {
        extraList.Clear();
        int useType = -1;
        Debug.Log("Loading extra " + index);
        switch (index)
        {

            case 0:
                useType = 4;
                for (int i = 0; i < liveObject.BATTLE_SLOTS.SKILLS.Count; i++)
                {
                    extraList.Add(liveObject.BATTLE_SLOTS.SKILLS[i]);
                }
                break;

            case 1:
                useType = 4;
                for (int i = 0; i < liveObject.PASSIVE_SLOTS.SKILLS.Count; i++)
                {
                    extraList.Add(liveObject.PASSIVE_SLOTS.SKILLS[i]);
                }
                break;


            case 2:
                useType = 4;
                for (int i = 0; i < liveObject.AUTO_SLOTS.SKILLS.Count; i++)
                {
                    extraList.Add(liveObject.AUTO_SLOTS.SKILLS[i]);
                }
                break;


            case 3:
                useType = 4;
                for (int i = 0; i < liveObject.OPP_SLOTS.SKILLS.Count; i++)
                {
                    extraList.Add(liveObject.OPP_SLOTS.SKILLS[i]);
                }
                break;


        }
        //UpdateColors(extraSlots);

        for (int useCount = 0; useCount < 6; useCount++) //UsableScript item in liveObject.GetComponents<UsableScript>())
        {
            MenuItem selectableItem = extraSlots[useCount];
            selectableItem.gameObject.SetActive(true);
            Text selectedText = selectableItem.GetComponentInChildren<Text>();
            if (useCount < extraList.Count)
            {
                UsableScript item = extraList[useCount];

                if (selectableItem.GetComponentInChildren<Text>())
                {

                    selectedText.text = item.NAME;

                    selectedText.resizeTextForBestFit = true;
                }
                if (selectableItem.GetComponent<MenuItem>())
                {
                    item.TYPE = useType;//itemType.TYPE;
                    selectableItem.refItem = item;

                }
            }
            else
            {
                selectableItem.refItem = null;
                selectedText.text = "";
                //   selectableItem.gameObject.SetActive(false);
            }


        }

        if (menuManager)
        {
            if (menuManager.DESC)
            {


                if (selectedMenuItem)
                {
                    if (selectedMenuItem.refItem)
                    {
                        menuManager.DESC.text = selectedMenuItem.refItem.DESC;

                    }
                }
            }
        }

    }
    private void DetermineAndFillExtra()
    {
        if (menuSide == -1)
            if (currentIndex < currentList.Count)
            {
                if (selectedMenuItem.refItem)
                {

                    if (selectedMenuItem.refItem.GetType() == typeof(CommandSkill))
                    {
                        loadExtra(0, lastObject);
                    }

                    if (selectedMenuItem.refItem.GetType() == typeof(PassiveSkill))
                    {
                        loadExtra(1, lastObject);
                    }

                    if (selectedMenuItem.refItem.GetType() == typeof(AutoSkill))
                    {
                        loadExtra(2, lastObject);
                    }

                    if (selectedMenuItem.refItem.GetType() == typeof(OppSkill))
                    {
                        loadExtra(3, lastObject);
                    }
                }
            }
        //UpdateColors(itemSlots);
        // UpdateColors(extraSlots);
    }

    public void EquipSkill()
    {
        if (selectedMenuItem.refItem.GetType() == typeof(CommandSkill))
        {
            if (!lastObject.BATTLE_SLOTS.Contains((SkillScript)selectedMenuItem.refItem))
            {
                if (lastObject.BATTLE_SLOTS.SKILLS.Count < 6)
                {

                    lastObject.BATTLE_SLOTS.SKILLS.Add((SkillScript)selectedMenuItem.refItem);
                    loadExtra(0, lastObject);
                }
            }
        }

        else if (selectedMenuItem.refItem.GetType() == typeof(PassiveSkill))
        {
            if (!lastObject.PASSIVE_SLOTS.Contains((SkillScript)selectedMenuItem.refItem))
            {
                if (lastObject.PASSIVE_SLOTS.SKILLS.Count < 6)
                {

                    lastObject.PASSIVE_SLOTS.SKILLS.Add((SkillScript)selectedMenuItem.refItem);
                    lastObject.ApplyPassives();
                    loadExtra(1, lastObject);
                }
            }
        }

        else if (selectedMenuItem.refItem.GetType() == typeof(AutoSkill))
        {
            if (lastObject.AUTO_SLOTS.SKILLS.Count < 6)
            {

                lastObject.AUTO_SLOTS.SKILLS.Add((SkillScript)selectedMenuItem.refItem);
                loadExtra(2, lastObject);
            }
        }

        else if (selectedMenuItem.refItem.GetType() == typeof(OppSkill))
        {
            if (lastObject.OPP_SLOTS.SKILLS.Count < 6)
            {
                lastObject.OPP_SLOTS.SKILLS.Add((SkillScript)selectedMenuItem.refItem);
                loadExtra(3, lastObject);
            }
        }
        else
        {
            Debug.Log("No match!");
            Debug.Log(selectedMenuItem.refItem.GetType());
        }
        //  UpdateColors(itemSlots);
        //  UpdateColors(extraSlots);

    }

    public void UnequipSkill()
    {
        if (selectedMenuItem)
        {
            if (selectedMenuItem.refItem)
            {

                if (selectedMenuItem.refItem.GetType() == typeof(CommandSkill))
                {

                    if (lastObject.BATTLE_SLOTS.Contains((SkillScript)selectedMenuItem.refItem))
                    {
                        bool check = lastObject.BATTLE_SLOTS.SKILLS.Remove((SkillScript)selectedMenuItem.refItem);

                        loadExtra(0, lastObject);
                    }
                }


                else if (selectedMenuItem.refItem.GetType() == typeof(PassiveSkill))
                {
                    if (lastObject.PASSIVE_SLOTS.Contains((SkillScript)selectedMenuItem.refItem))
                    {

                        lastObject.PASSIVE_SLOTS.SKILLS.Remove((SkillScript)selectedMenuItem.refItem);
                        lastObject.ApplyPassives();
                        loadExtra(1, lastObject);
                    }
                }

                else if (selectedMenuItem.refItem.GetType() == typeof(AutoSkill))
                {
                    if (lastObject.AUTO_SLOTS.Contains((SkillScript)selectedMenuItem.refItem))
                    {

                        lastObject.AUTO_SLOTS.SKILLS.Remove((SkillScript)selectedMenuItem.refItem);
                        loadExtra(2, lastObject);
                    }
                }

                else if (selectedMenuItem.refItem.GetType() == typeof(OppSkill))
                {
                    if (lastObject.OPP_SLOTS.Contains((SkillScript)selectedMenuItem.refItem))
                    {

                        lastObject.OPP_SLOTS.SKILLS.Remove((SkillScript)selectedMenuItem.refItem);
                        loadExtra(3, lastObject);
                    }
                }
                else
                {
                    Debug.Log("No match!");
                    Debug.Log(selectedMenuItem.refItem.GetType());
                }
            }
        }
        // UpdateColors(itemSlots);
        //UpdateColors(extraSlots);
    }
    public void unloadContents()
    {
        currentList.Clear();
        //if (lastObject)
        //{
        //    Debug.Log(lastObject.FullName);
        //    List<UsableScript> invokingObjectsUse = lastObject.GetComponent<InventoryScript>().USEABLES;
        //    for (int useCount = 0; useCount < invokingObjectsUse.Count; useCount++)
        //    {
        //        UsableScript item = invokingObjectsUse[useCount];
        //        if (tempObjects.ContainsKey(item.NAME))
        //        {
        //            GameObject obj = tempObjects[item.NAME];
        //            obj.transform.SetParent(null);
        //            obj.gameObject.SetActive(false);
        //        }
        //    }
        //}
    }

}
