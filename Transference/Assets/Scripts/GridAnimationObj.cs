using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAnimationObj : GridObject
{

    public AnimationScript script;
    public Animation anim;
    public bool isShowing = false;
  //  private float time = 2;

    public override void Setup()
    {
        if (!isSetup)
        {
            
            script = GetComponent<AnimationScript>();
            anim = GetComponent<Animation>();
            script.Setup();
            base.Setup();
            isSetup = true;
        }
    }
    public ManagerScript manager;

    public void StartCountDown()
    {
        if (isShowing == false)
        {
            script.LoadList("Animations/Cuts/");
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
        Debug.Log("Countdown finished.");
        isShowing = false;
        manager.AnimationRequests--;
        gameObject.SetActive(false);

    }

    private void Update()
    {
        if (isShowing)
        {
           // time -= Time.deltaTime;
            if (script.index >= script.currentList.Length -1)
            {
                StopCountDown();
            }
        }
    }

}
