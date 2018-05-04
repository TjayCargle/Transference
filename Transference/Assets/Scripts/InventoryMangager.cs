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
    public MenuItem selectedMenuItem;
    private LivingObject lastObject;
    private MenuManager menuManager;
    // public int testIndex = 0;
    public void Start()
    {
        menuManager = GameObject.FindObjectOfType<MenuManager>();

    }
    // Update is called once per frame
    void Update()
    {
        if (currentRect)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                IncreaseScroll();

                // Debug.Log(testRect.normalizedPosition);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                DecreaseScroll();
                //  Debug.Log(testRect.normalizedPosition);
            }
        }
        //if (selectedMenuItem)
        //{
        //    if (selectedMenuItem.refItem)
        //    {
        //        if (Input.GetKeyDown(KeyCode.Space))
        //        {
        //            if (selectedMenuItem.refItem.GetType() == testLiveObject.WEAPON.GetType())
        //            {
        //                testLiveObject.WEAPON = (WeaponScript)selectedMenuItem.refItem;
        //                Debug.Log(testLiveObject.WEAPON);
        //                Debug.Log(testLiveObject.WEAPON.NAME);
        //                Debug.Log(testLiveObject.WEAPON.GetType());
        //            }
        //        }
        //    }
        //}
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
                        currentIndex = 0;
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
                            menuManager.DESC.text = selectedMenuItem.refItem.DESC;
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
                        currentIndex = currentContent.transform.childCount - 1;
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
                            menuManager.DESC.text = selectedMenuItem.refItem.DESC;
                        }
                    }
                }
            }
        }
    }
    // Use this for initialization
    public void loadContents(GameObject content, ScrollRect rect, int index, LivingObject liveObject)
    {
        lastObject = liveObject;
        currentContent = content;
        currentRect = rect;
        //UsableScript itemType = new UsableScript();
        int useType = -1;
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

                if (tempObjects.ContainsKey(item.NAME))
                {
                    tempObjects[item.NAME].SetActive(true);
                    tempObjects[item.NAME].transform.SetParent(content.transform);
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
                    selectedText.text = item.NAME;

                }
                if (selectableItem.GetComponent<MenuItem>())
                {
                    MenuItem selectedItem = selectableItem.GetComponent<MenuItem>();
                    item.TYPE = useType;//itemType.TYPE;
                    selectedItem.refItem = item;

                    // selectedItem.itemType = itemType;


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
                selectedMenuItem = currentContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
            }
        }
        if (menuManager)
        {
            if (menuManager.DESC)
            {


                if (selectedMenuItem)
                {
                    menuManager.DESC.text = selectedMenuItem.refItem.DESC;
                }
            }
        }

    }

    public void unloadContents()
    {
        if (lastObject)
        {
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
