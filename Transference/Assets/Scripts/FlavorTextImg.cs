using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlavorTextImg : MonoBehaviour
{
    public string theText;
    public TextObj textObj;
    public Text myText;
    public TextMeshProUGUI myOtherText;
    public ManagerScript manager;
    public bool isSetup = false;
    // Use this for initialization
    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        if(!isSetup)
        {

            myText = GetComponentInChildren<Text>();
            myOtherText = GetComponentInChildren<TextMeshProUGUI>();
            if (myText)
            {
                textObj = myText.GetComponent<TextObj>();
            }
            if (myOtherText)
            {
                textObj = myOtherText.GetComponent<TextObj>();
            }
            manager = Common.GetManager();
            if (manager)
            {
                manager.flavor = this;
            }
            gameObject.SetActive(false);
            isSetup = true;
        }
    }

    public void SetText()
    {

        if (myText)
        {
            myText.text = theText;
        }

        if(myOtherText)
        {
            myOtherText.text = theText;
        }
    }
}
