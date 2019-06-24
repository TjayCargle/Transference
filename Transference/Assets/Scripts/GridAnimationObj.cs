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
    public int magnitute = 1;
    public GridObject target;
    public string path;
    public bool prepared = false;
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

    public void LoadGridAnimation()
    {
        if (type == -1)
        {
            path = "Animations/Break/";

        }
        else if (type == -2)
        {
            path = "Animations/Support/";

        }
        else if (type == (int)Element.Buff)
        {
            path = "Animations/Buffs/";

        }
        else
        {
            path = "Animations/" + ((Element)type).ToString().ToLower() + "/";
        }
        script.LoadList(path, false);
    }
    public void StartCountDown()
    {
        if (isShowing == false)
        {
            if(target)
            {
                manager.MoveCameraAndShow(target);
            }
  
         
            if (manager.sfx)
            {
                AudioClip[] audios = Resources.LoadAll<AudioClip>(path);
                if (audios != null)
                {
                    if (audios.Length > 0)
                    {
                        manager.sfx.loadAudio(audios[0]);
                        manager.sfx.playSound();
                    }
                }
            }
          
            gameObject.SetActive(true);
          //  Debug.Log("anim");
            script.anim.speed = 1;
            isShowing = true;
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
        gameObject.SetActive(false);
        prepared = false;
        isShowing = false;
        manager.AnimationRequests--;
        manager.myCamera.UpdateCamera();
    }


    private void Update()
    {
        if (isShowing)
        {
            if (script.index == (int)(script.currentList.Length * 0.3f))
            {
                if (shake)
                {
                    float val = script.currentList.Length * Random.Range(1.4f, 1.8f);
                    StartCoroutine(shake.Shake(val * Time.deltaTime * magnitute, (0.2f * magnitute), (val * 0.5f) * Time.deltaTime));
                }
            }
            // time -= Time.deltaTime;
            if (script.index >= script.currentList.Length - 1)
            {
                script.anim.speed = 0;
                StopCountDown();
            }
        }
    }

}
