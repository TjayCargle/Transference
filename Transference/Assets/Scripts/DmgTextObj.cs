using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgTextObj : MonoBehaviour
{
    public TextMeshPro text;
    public TextMeshPro border;
    public TextMeshPro shadow;
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
            if(GetComponentsInChildren<TextMeshPro>().Length > 2)
            {

            text = GetComponentsInChildren<TextMeshPro>()[0];
            border = GetComponentsInChildren<TextMeshPro>()[1];
            shadow = GetComponentsInChildren<TextMeshPro>()[2];
            }
            isSetup = true;
        }
    }
    public void StartCountDown()
    {
        if (isShowing == false)
        {
            if (target)
                manager.MoveCameraAndShow(target);
            manager.myCamera.SetCameraPosFar();
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
