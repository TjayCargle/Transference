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
    // public int testIndex = 0;
    public void Start()
    {

        //if (testLiveObject)
        //{
        //    if (testContent)
        //    {
        //        if (!testLiveObject.isSetup)
        //        {
        //            testLiveObject.Setup();
        //        }
        //    //    loadContents(testContent, testIndex, testLiveObject);
        //    }
        //}

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
            }
        }
    }
    // Use this for initialization
    public void loadContents(GameObject content, ScrollRect rect, int index, LivingObject liveObject)
    {
        lastObject = liveObject;
        currentContent = content;
        currentRect = rect;
        UsableScript itemType = new UsableScript();
        switch (index)
        {
            case 0:
                itemType = new WeaponScript();
                break;
            case 1:
                itemType = new ArmorScript();
                break;
            case 2:
                itemType = new AccessoryScript();
                break;
            case 3:
                itemType = new ItemScript();
                break;
            case 4:
                itemType = new SkillScript();
                break;
        }
        // for (int i = 0; i < 4; i++)
        {

            UsableScript[] invokingObjectsUse = liveObject.GetComponents<UsableScript>();
            for (int useCount = 0; useCount < invokingObjectsUse.Length; useCount++) //UsableScript item in liveObject.GetComponents<UsableScript>())
            {
                UsableScript item = invokingObjectsUse[useCount];
                if (itemType.GetType() != item.GetType())
                {
                    //   Debug.Log("Selected type is not a : " + weapon.GetType().ToString());
                    continue;
                }
                if (!item)
                {
                    continue;
                }

                if (tempObjects.ContainsKey(item.NAME))
                {
                    tempObjects[item.NAME].SetActive(true);
                    tempObjects[item.NAME].transform.parent = content.transform;
                    continue;
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

    }

    public void unloadContents()
    {
        if (lastObject)
        {
            UsableScript[] invokingObjectsUse = lastObject.GetComponents<UsableScript>();
            for (int useCount = 0; useCount < invokingObjectsUse.Length; useCount++) 
            {
                UsableScript item = invokingObjectsUse[useCount];
                if (tempObjects.ContainsKey(item.NAME))
                {
                    GameObject obj = tempObjects[item.NAME];
                    obj.transform.parent = null;
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }

}
