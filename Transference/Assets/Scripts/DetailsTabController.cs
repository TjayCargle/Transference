using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DetailsTabController : MonoBehaviour, IPointerEnterHandler
{

    public int type;
    public DetailsScreen detailsScreen;
    public bool isSetup = false;
    public Image myimage;
    public void Setup()
    {
        if(!isSetup)
        {
            detailsScreen = GameObject.FindObjectOfType<DetailsScreen>();
            myimage = GetComponent<Image>();
            isSetup = true;
        }
    }
    private void Start()
    {
        Setup();
    }
    public void SelectTab()
    {
        if(detailsScreen)
        {
            int currDetail = (int)detailsScreen.detail;

            if (type == 2) //right
            {
                currDetail++;
                if(currDetail > (int)DetailType.Exp)
                {
                    currDetail = 0;
                }
            }
            else if (type == 0)// left
            {
                currDetail--;
                if (currDetail < 0)
                {
                    currDetail = (int)DetailType.Exp;
                }
            }
            detailsScreen.detail = (DetailType)currDetail;

        }
        else
        {
            Debug.Log("No details screen found");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(detailsScreen)
        detailsScreen.selectedContent = type;
    }

    public void ChangeDescriptionType()
    {
       if(detailsScreen)
        {
            if(type == 35)
            {
                detailsScreen.fullDescription = !detailsScreen.fullDescription;
                detailsScreen.updateDetails();
                if(GetComponentInChildren<Text>())
                {
                    if(detailsScreen.fullDescription)
                    {
                        GetComponentInChildren<Text>().text = "Battle Description";
                    }
                    else
                    {
                        GetComponentInChildren<Text>().text = "Level Description";
                    }
                }
            }
        }
    }
}
