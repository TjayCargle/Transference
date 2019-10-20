using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgTextObj : MonoBehaviour
{
    public TextMesh text;
    public TextMesh border;
    public TextMesh shadow;
    public bool isShowing = false;
    public bool loaded = false;
    private float time = 2;
    public bool isSetup = false;
    public ManagerScript manager;
    public GridObject target;
    // Use this for initialization
    void Start()
    {

        Setup();
    }

    public void Setup()
    {
        if (!isSetup)
        {
            text = GetComponentsInChildren<TextMesh>()[0];
            border = GetComponentsInChildren<TextMesh>()[1];
            shadow = GetComponentsInChildren<TextMesh>()[2];
            isSetup = true;
        }
    }
    public void StartCountDown()
    {
        if (isShowing == false)
        {
            if (target)
                manager.MoveCameraAndShow(target);
            gameObject.SetActive(true);
            isShowing = true;
            time = 0.75f;
            if(manager)
            {
                manager.PlayHitSound();
            }
        }
    }

    public void StopCountDown()
    {
        time = 0;
        isShowing = false;
        loaded = false;
        gameObject.SetActive(false);
        manager.dmgRequest--;
        manager.myCamera.UpdateCamera();
        manager.SoftReset();
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
