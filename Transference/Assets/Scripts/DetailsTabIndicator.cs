using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailsTabIndicator : MonoBehaviour
{
    public Image myImage;
    public DetailType myDetail;
    private Button mybutton;
    void Start()
    {
        myImage = GetComponent<Image>();
        mybutton = GetComponent<Button>();
        if (mybutton)
        {
            mybutton.onClick.AddListener(PressButton);
        }
    }

    public void PressButton()
    {
        DetailsScreen details = GameObject.FindObjectOfType<DetailsScreen>();
        if(details)
        {
            details.detail = myDetail;
            details.updateDetails();
        
        }
    }
}
