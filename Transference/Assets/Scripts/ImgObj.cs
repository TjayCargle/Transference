using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImgObj : MonoBehaviour {

    public Image myImage;
    public bool isShowing = false;
    private float time = 2;
    public bool isSetup = false;
    public ManagerScript manager;
    private void Start()
    {
        myImage = GetComponent<Image>();
    }
    public void StartCountDown(float t = 1.00f)
    {
        if (isShowing == false)
        {
            gameObject.SetActive(true);
            isShowing = true;
            time = t;
        }
    }

    public void StopCountDown()
    {
        time = 0;
        isShowing = false;
        gameObject.SetActive(false);


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
