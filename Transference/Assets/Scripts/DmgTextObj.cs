using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgTextObj : MonoBehaviour {
    public TextMesh text;
    public TextMesh border;
    public bool isShowing = false;
    private float time = 2;
    public bool isSetup = false;
    public ManagerScript manager;
	// Use this for initialization
	void Start () {

        Setup();
    }

    public void Setup()
    {
        if(!isSetup)
        {
        text = GetComponentsInChildren<TextMesh>()[0];
        border = GetComponentsInChildren<TextMesh>()[1];
        isSetup = true;
        }
    }
    public void StartCountDown()
    {
        if(isShowing == false)
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
        gameObject.SetActive(false);
        manager.dmgRequest--;
        manager.myCamera.UpdateCamera();
    }

    private void Update()
    {
        if(isShowing)
        {
            time -= Time.deltaTime;
            if(time <= 0)
            {
                StopCountDown();
            }
        }
    }

}
