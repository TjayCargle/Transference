using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAnimationObj : GridObject
{

    public AnimationScript script;
    public Animation anim;
    public bool isShowing = false;
    public int type = 0;
    public CameraShake shake;

    //  private float time = 2;

    public override void Setup()
    {
        if (!isSetup)
        {

            script = GetComponent<AnimationScript>();
            anim = GetComponent<Animation>();
            script.Setup();
            base.Setup();
            shake = GameObject.FindObjectOfType<CameraShake>();
            isSetup = true;
        }
    }
    public ManagerScript manager;

    public void StartCountDown()
    {
        if (isShowing == false)
        {
            if (type == 0)
            {
                script.LoadList("Animations/Cuts/");

            }
            else
            {
                script.LoadList("Animations/Buffs/");
            }
            gameObject.SetActive(true);

            isShowing = true;
            script.index = 0;
            //anim.Play();
            //  time = 1.05f;
        }
    }

    public void StopCountDown()
    {
        //time = 0;
        // Debug.Log("Countdown finished.");
        if (shake)
        {
            shake.ToOrigin();
        }
        isShowing = false;
        manager.AnimationRequests--;
        gameObject.SetActive(false);

    }

    
    private void Update()
    {
        if (isShowing)
        {
            if (script.index == (int)(script.currentList.Length * 0.7f))
            {
                if (shake)
                {
                    float val = script.currentList.Length;
                    StartCoroutine(shake.Shake(val * Time.deltaTime, 0.2f * (type % 6), (val * 0.5f) * Time.deltaTime));
                }
            }
            // time -= Time.deltaTime;
            if (script.index >= script.currentList.Length - 1)
            {

                StopCountDown();
            }
        }
    }

}
