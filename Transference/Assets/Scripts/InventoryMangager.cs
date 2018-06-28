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
    public void Start()
    {
        menuManager = GameObject.FindObjectOfType<MenuManager>();
        manager = GetComponent<ManagerScript>();
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
                    if (extraContent)
                    {
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
                case State.PlayerSkillsMenu:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        IncreaseScroll();
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        DecreaseScroll();
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
                    }
                    currentIndex--;
                    if (currentIndex < 0)
                    {
                        currentIndex = currentContent.transform.childCount - 1;
                    }
                    selectedMenuItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
                    if (selectedMenuItem)
                    {
                        selectedMenuItem.GetComponentInChildren<Text>().color = Color.yellow;
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
                }
                Vector2 newPos = currentRect.normalizedPosition;
                newPos.y -= 0.01f;

                if (currentContent.transform.childCount > 0)
                {
                    if (selectedMenuItem)
                    {
                        selectedMenuItem.GetComponentInChildren<Text>().color = Color.white;
                    }
                    currentIndex++;
                    if (currentIndex >= currentContent.transform.childCount)
                    {
                        currentIndex = 0;
                    }
                    selectedMenuItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();

                    if (selectedMenuItem)
                    {
                        selectedMenuItem.GetComponentInChildren<Text>().color = Color.yellow;
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

                            }
                        }
                    }
                }
            }
        }
    }
    public void ForceSelect()
    {
        if (selectedMenuItem)
        {
            selectedMenuItem.GetComponentInChildren<Text>().color = Color.white;
            if (prevIndex < currentContent.transform.childCount)
                selectedMenuItem = currentContent.transform.GetChild(prevIndex).GetComponent<MenuItem>();
            selectedMenuItem.GetComponentInChildren<Text>().color = Color.white;

        }
        if (currentContent.transform.childCount > 0)
        {

            selectedMenuItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
            selectedMenuItem.GetComponentInChildren<Text>().color = Color.yellow;
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
        //UsableScript itemType = new UsableScript();
        int useType = -1;
        int windowType = -1;
        Debug.Log("index = " + index);
        switch (index)
        {

            case 0:
                // itemType = ScriptableObject.CreateInstance<WeaponScript>();
                // itemType.TYPE = 0;
                useType = 0;
                break;
            case 1:
                // itemType = new ArmorScript();
                // itemType.TYPE = 1;
                useType = 1;
                break;
            case 2:
                // itemType = new AccessoryScript();
                // itemType.TYPE = 2;
                useType = 2;
                break;
            case 3:
                // itemType = new ItemScript();
                // itemType.TYPE = 3;
                useType = 3;
                break;
            case 4:
                // itemType = new SkillScript(); 
                // itemType.TYPE = 4;
                useType = 4;
                windowType = 1; //all usable skills
                break;

            case 5:
                useType = 4;
                windowType = 2; //all passive skills
                break;

            case 6:
                useType = 4;
                windowType = 3; // useable skill slots
                break;

            case 7:
                useType = 4;
                windowType = 4; // passive skill slots
                break;
            case 8:
                useType = 4;
                windowType = 5; // weapon skill slots
                break;
            case 9:
                useType = 4;
                windowType = 5; // all weapon skills
                break;
        }
        // for (int i = 0; i < 4; i++)
        {

            List<UsableScript> invokingObjectsUse = liveObject.GetComponent<InventoryScript>().USEABLES;
            for (int useCount = 0; useCount < invokingObjectsUse.Count; useCount++) //UsableScript item in liveObject.GetComponents<UsableScript>())
            {
                UsableScript item = invokingObjectsUse[useCount];
                if (useType != item.TYPE)
                {
                    continue;
                }
                if (!item)
                {
                    continue;
                }
                if (useType == 4)
                {
                    switch (windowType)
                    {
                        case 1:
                            break;

                        case 2:
                            Debug.Log("yo");
                            Debug.Log(((SkillScript)item).ELEMENT);
                            if (((SkillScript)item).ELEMENT != Element.Passive)
                            {
                                continue;
                            }
                            break;
                        case 3:
                            if (((SkillScript)item).ELEMENT != Element.Auto)
                            {
                                continue;
                            }
                            break;

                    }
                    Debug.Log("yosef");
                }
                if (tempObjects.ContainsKey(item.NAME))
                {

                    if (item.TYPE == 4)
                    {
                        switch (windowType)
                        {
                            case 1:
                                if (!liveObject.GetComponent<InventoryScript>().USEABLES.Contains(((SkillScript)item)))
                                {
                                    continue;
                                }
                                break;

                            case 2:

                                if (!liveObject.GetComponent<InventoryScript>().SKILLS.Contains(((SkillScript)item)))
                                {
                                    continue;
                                }
                                Debug.Log(1);
                                if (((SkillScript)item).ELEMENT != Element.Passive)
                                {
                                    continue;
                                }
                                Debug.Log(2);
                                if (!liveObject.GetComponent<InventoryScript>().PASSIVES.Contains(((PassiveSkill)item)))
                                {
                                    continue;
                                }
                                Debug.Log(3);
                                Debug.Log(item.NAME);
                                break;

                            case 3:
                                if (!liveObject.GetComponent<InventoryScript>().SKILLS.Contains(((SkillScript)item)))
                                {
                                    continue;
                                }

                                if (((SkillScript)item).ELEMENT != Element.Auto)
                                {
                                    continue;
                                }
                                if (!liveObject.BATTLE_SLOTS.Contains(((SkillScript)item)))
                                {
                                    continue;
                                }
                                break;

                            case 4:

                                if (!liveObject.PASSIVE_SLOTS.Contains(((SkillScript)item)))
                                {
                                    continue;
                                }
                                break;
                        }
                        tempObjects[item.NAME].SetActive(true);
                        tempObjects[item.NAME].transform.SetParent(content.transform);



                    }
                    continue;
                    //no need to have else if it continues;
                }

                //  else
                //{
                //  Debug.Log("Yes we are looking for a : " + weapon.GetType().ToString());

                //}
                GameObject selectableItem = Instantiate(menuItem, content.transform);
                selectableItem.name = item.NAME;
                if (selectableItem.GetComponentInChildren<Text>())
                {
                    Text selectedText = selectableItem.GetComponentInChildren<Text>();
                    if (item.TYPE != 4)
                    {
                        selectedText.text = item.NAME;
                    }
                    else
                    {
                        selectedText.text = item.NAME;
                        if (windowType == 1)
                        {

                            string extraText = "";
                            if (((CommandSkill)item).ETYPE == EType.physical)
                                extraText = " " + "HP";
                            else
                                extraText = " " + "SP";
                            selectedText.text += " " + ((CommandSkill)item).COST.ToString() + extraText;
                        }
                    }
                    selectedText.resizeTextForBestFit = true;
                }
                if (selectableItem.GetComponent<MenuItem>())
                {
                    MenuItem selectedItem = selectableItem.GetComponent<MenuItem>();
                    item.TYPE = useType;//itemType.TYPE;
                    selectedItem.refItem = item;




                    tempObjects.Add(selectedItem.refItem.NAME, selectableItem);
                }


            }
        }


        if (content.GetComponent<RectTransform>())
        {

            RectTransform rt = content.GetComponent<RectTransform>();
            float newValue = menuItem.GetComponent<RectTransform>().rect.height * content.transform.childCount;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, newValue);
        }
        if (currentRect)
        {
            if (currentContent)
            {
                if (currentContent.transform.childCount > 0)
                    selectedMenuItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
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

    public void EquipSkill(GameObject target)
    {
        if (currentContent)
        {
            if (extraContent)
            {

            }
        }
    }
    public void unloadContents()
    {
        if (lastObject)
        {
            Debug.Log(lastObject.FullName);
            List<UsableScript> invokingObjectsUse = lastObject.GetComponent<InventoryScript>().USEABLES;
            for (int useCount = 0; useCount < invokingObjectsUse.Count; useCount++)
            {
                UsableScript item = invokingObjectsUse[useCount];
                if (tempObjects.ContainsKey(item.NAME))
                {
                    GameObject obj = tempObjects[item.NAME];
                    obj.transform.SetParent(null);
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }

}
