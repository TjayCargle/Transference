using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryMangager : MonoBehaviour {
    public GameObject menuItem;
    public LivingObject testLiveObject;
    public GameObject testContent;
    public ScrollRect testRect;
    public int currentIndex = 0;
    public MenuItem selectedMenuItem;
    public void Start()
    {
        if(testLiveObject)
        {
            if(testContent)
            {
                if(!testLiveObject.isSetup)
                {
                    testLiveObject.Setup();
                }
                loadContents(testContent, 0, testLiveObject);
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (testRect)
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
        if(selectedMenuItem)
        {
            if (selectedMenuItem.refItem)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (selectedMenuItem.refItem.GetType() == testLiveObject.WEAPON.GetType())
                    {
                        testLiveObject.WEAPON = (WeaponScript)selectedMenuItem.refItem;
                        Debug.Log(testLiveObject.WEAPON);
                        Debug.Log(testLiveObject.WEAPON.NAME);
                    }
                }
            }
        }
    }
    public void IncreaseScroll()
    {
      
        Vector2 newPos = testRect.normalizedPosition;
        newPos.y += 0.01f;
    
        if (testContent.transform.childCount > 0)
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
            selectedMenuItem = testContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();
            if (selectedMenuItem)
            {
                selectedMenuItem.GetComponentInChildren<Text>().color = Color.yellow;
            }

            float index = currentIndex / (float)testContent.transform.childCount;
            newPos.y = 1.0f - index;
      
        }
        if (newPos.y > 1)
            newPos.y = 1;
        testRect.normalizedPosition = newPos;
    }
    public void DecreaseScroll()
    {

        if(selectedMenuItem)
        {
            selectedMenuItem.GetComponentInChildren<Text>().color = Color.white;
        }
        Vector2 newPos = testRect.normalizedPosition;
        newPos.y -= 0.01f;
    
        if (testContent.transform.childCount > 0)
        {
            if (selectedMenuItem)
            {
                selectedMenuItem.GetComponentInChildren<Text>().color = Color.white;
            }
            currentIndex++;
            if (currentIndex >= testContent.transform.childCount)
            {
                currentIndex = testContent.transform.childCount - 1;
            }
            selectedMenuItem = testContent.transform.GetChild(currentIndex).GetComponent<MenuItem>();

            if (selectedMenuItem)
            {
                selectedMenuItem.GetComponentInChildren<Text>().color = Color.yellow;
            }

            float index = currentIndex / (float)(testContent.transform.childCount - 1);
            newPos.y = 1.0f - index; 
    
        }
        if (newPos.y < 0)
            newPos.y = 0;
        testRect.normalizedPosition = newPos;
    }
    // Use this for initialization
    public void loadContents(GameObject content, int index, LivingObject liveObject)
    {
        switch (index)
        {
            case 0:

                for (int i = 0; i < 4; i++)
                {

                    foreach (WeaponScript weapon in liveObject.GetComponents<WeaponScript>())
                    {
                        GameObject selectableItem = Instantiate(menuItem, content.transform);
                        selectableItem.name = weapon.NAME;
                        if (selectableItem.GetComponentInChildren<Text>())
                        {
                            Text selectedText = selectableItem.GetComponentInChildren<Text>();
                            selectedText.text = weapon.NAME;

                        }
                        if(selectableItem.GetComponent<MenuItem>())
                        {
                        MenuItem selectedItem = selectableItem.GetComponent<MenuItem>();
                        selectedItem.refItem = weapon;
                        }
                    }
                }
                
        
                if (content.GetComponent<RectTransform>())
                {
               
                    RectTransform rt = content.GetComponent<RectTransform>();
                    float newValue = menuItem.GetComponent<RectTransform>().rect.height * content.transform.childCount ;
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, newValue);
                }
                break;
        }
       
    }
}
