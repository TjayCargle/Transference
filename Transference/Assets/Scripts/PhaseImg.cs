using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseImg : ImgObj
{

    public Text myText;
    private int direction = -1;
    Color textColor;
    Color myColor;
    private void Start()
    {
        if (myText)
        {
            textColor = myText.color;
        }
        if (GetComponent<Image>())
        {
            myColor = GetComponent<Image>().color;
        }
    }
    public override void StopCountDown()
    {
       // Debug.Log("stopping");
        if (manager)
        {
            if (manager.GetState() == State.PlayerTransition)
            {
                manager.currentState = State.FreeCamera;
            }
        }
        else
        {

            manager = GameObject.FindObjectOfType<ManagerScript>();
            if (manager)
            {
                if (manager.GetState() == State.EventRunning)
                {
                    manager.currentState = State.FreeCamera;
                }
            }
        }

        base.StopCountDown();
    }
    public void StartCountDown(Color phaseColor, string whooseTurn)
    {
        if (isShowing == false)
        {
            gameObject.SetActive(true);
            isShowing = true;
            direction = 1;
            if (myText)
            {
                textColor = Color.black;
                textColor.a = 0.1f;
                myText.color = textColor;
                myText.text = whooseTurn;
            }
            if (GetComponent<Image>())
            {
                myColor = phaseColor;
                myColor.a = 0.0f;
                GetComponent<Image>().color = myColor;
            }
        }
    }

    private void Update()
    {
        if (isShowing)
        {
            if (direction == 1)
            {
                if (myText)
                {
                    if (textColor.a + Time.deltaTime < 1.0f)
                    {
                        textColor.a +=  Time.deltaTime;
                        myText.color = textColor;
                    }
                    else
                    {
                        direction = -1;
                    }
                }
                if (GetComponent<Image>())
                {
                    if (myColor.a +  Time.deltaTime  < 1.0f)
                    {
                        myColor.a += Time.deltaTime;
                        GetComponent<Image>().color = myColor;
                    }
                    else
                    {
                        direction = -1;
                    }
                 
                }
            }
            else
            {
                if (myText)
                {
                    if (textColor.a - Time.deltaTime > 0.0f)
                    {
                        textColor.a -=Time.deltaTime;
                        myText.color = textColor;
                    }
                    else
                    {
                        StopCountDown();
                    }
                }
                if (GetComponent<Image>())
                {
                    if (myColor.a -  Time.deltaTime > 0.0f)
                    {
                        myColor.a -=  Time.deltaTime;
                        GetComponent<Image>().color = myColor;
                    }
                    else
                    {
                        StopCountDown();
                    }

                }
          
            }
        }
    }
}
