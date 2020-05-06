using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextObj : MonoBehaviour {

    public Text text;
    public TextMeshProUGUI textmeshpro;
    public bool isShowing = false;
    private float time = 2;
    public bool isSetup = false;
    public ManagerScript manager;
    // Use this for initialization
    void Start()
    {

        Setup();
    }

    public void Setup()
    {
        if (!isSetup)
        {
            text = GetComponent<Text>();
            textmeshpro = GetComponent<TextMeshProUGUI>();
            isSetup = true;
        }
    }
    public void StartCountDown()
    {
        if (isShowing == false)
        {
            gameObject.SetActive(true);
            isShowing = true;
            time = 1.00f;
        }
    }

    public void StopCountDown()
    {
        time = 0;
        isShowing = false;
      transform.parent.gameObject.SetActive(false);


    }
    public void SetText(string sometext)
    {
        if(text)
        {
            text.text = sometext;
        }

        if(textmeshpro)
        {
            textmeshpro.text = sometext;
        }
    }
    private void Update()
    {
        if (isShowing)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                StopCountDown();
            }
        }
    }
}
