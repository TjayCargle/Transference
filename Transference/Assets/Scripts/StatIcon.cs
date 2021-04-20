using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class StatIcon : MonoBehaviour
{
    public Image myImage;
    public Text myText;
    public TextMeshProUGUI myProText;
    public Image childImage;
    public bool isSetup = false;
    public TooltipConnector tooltip = null;
    public void Setup()
    {
        if (!isSetup)
        {
            if (myImage == null)
                myImage = GetComponent<Image>();

            if (myImage == null)
                myImage = this.gameObject.AddComponent<Image>();

            if (myText == null)
                myText = GetComponent<Text>();

            if (myProText == null)
                myProText = GetComponentInChildren<TextMeshProUGUI>();

            if (myProText == null)
                myProText = this.gameObject.AddComponent<TextMeshProUGUI>();

            if (childImage == null)
                childImage = GetComponentsInChildren<Image>()[1];

            if (childImage == null)
                childImage = myImage;

            if (tooltip == null)
                tooltip = GetComponent<TooltipConnector>();
            isSetup = true;
        }
    }

}
