using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlavorTextImg : MonoBehaviour
{

    public Text myText;
    public TextObj textObj;
    public ManagerScript manager;
    // Use this for initialization
    void Start()
    {
        myText = GetComponentInChildren<Text>();
        textObj = myText.GetComponent<TextObj>();
        manager = GameObject.FindObjectOfType<ManagerScript>();
        if(manager)
        {
            manager.flavor = this;
        }
        gameObject.SetActive(false);
    }

}
