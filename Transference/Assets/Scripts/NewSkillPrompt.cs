using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NewSkillPrompt : MonoBehaviour {

    public Text TitleText;
    public TextMeshProUGUI BodyText;

    [SerializeField]
    public Text choice1;

    [SerializeField]
    public Text choice2;

    public bool isSetup = false;


    void Start () {
        Setup();
    }
	
    public void Setup()
    {
        if(!isSetup)
        {
            if (GetComponentInChildren<Text>())
            {
                TitleText = GetComponentInChildren<Text>();
            }

            if (GetComponentInChildren<TextMeshProUGUI>())
            {
                BodyText = GetComponentInChildren<TextMeshProUGUI>();
            }
            isSetup = true;
        }
    }

}
