using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NewSkillPrompt : MonoBehaviour
{

    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI BodyText;

    [SerializeField]
    public Text choice1;

    [SerializeField]
    public Text choice2;

    public bool isSetup = false;


    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        if (!isSetup)
        {
            if (!TitleText)
            {
                if (GetComponentInChildren<Text>())
                {
                    TitleText = GetComponentsInChildren<TextMeshProUGUI>()[0];
                }
            }

            if (!BodyText)
            {
                if (GetComponentInChildren<TextMeshProUGUI>())
                {
                    BodyText = GetComponentsInChildren<TextMeshProUGUI>()[1];
                }
                isSetup = true;
            }
        }
    }

}
