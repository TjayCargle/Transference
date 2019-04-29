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
            string path = "";
            if (type == -1)
            {
                path = "Animations/Break/";

            }
           else if (type == (int)Element.Buff)
            {
                path = "Animations/Buffs/";

            }
            else
            {
                path = "Animations/" + ((Element)type).ToString().ToLower() + "/";
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
            script.LoadList(path, false);
            gameObject.SetActive(true);
          //  Debug.Log("anim");
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
                    StartCoroutine(shake.Shake(val * Time.deltaTime * magnitute, (0.02f * magnitute), (val * 0.5f) * Time.deltaTime));
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
