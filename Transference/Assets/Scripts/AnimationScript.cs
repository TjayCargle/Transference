using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimationScript : MonoBehaviour
{

    public Sprite[] currentList;
    public SpriteRenderer render;
    public SpriteRenderer ShadowRender;
    public GridObject obj;
    public int index;
    public bool isSetup = false;
    LivingObject me;
    PlayerController controller;
    CameraScript cam;
   public Animator anim;
    Animator shadowAnimator;
    public Animator SHADOWANIM
    {
        get { return shadowAnimator; }
        set { shadowAnimator = value; }
    }
    private bool repeat = true;
    public void Setup()
    {
        if (!isSetup)
        {
            if (!obj)
            {
                obj = GetComponent<GridObject>();
            }
            if (!obj)
            {
                return;
            }
            if (!render)
            {
                render = obj.gameObject.GetComponent<SpriteRenderer>();

            }
            anim = obj.GetComponent<Animator>();
            if(!anim)
            {
                anim = gameObject.AddComponent<Animator>();
            }
            if (obj.FullName == "")
            {
                return;
            }
            if (obj.FullName == null)
            {
                return;
            }
            // Debug.Log("InitialList");

            string shrtname = obj.FullName;
            string[] subs = shrtname.Split(' ');
            shrtname = "";
            for (int i = 0; i < subs.Length; i++)
            {
                shrtname += subs[i];
            }
            currentList = Resources.LoadAll<Sprite>("" + shrtname + "/Idle/"); //obj.FullName + "/Idle/");
            index = 0;
            if (currentList.Length > 0)
            {
                render.sprite = currentList[index];
            }
            me = GetComponent<LivingObject>();
            cam = GameObject.FindObjectOfType<CameraScript>();
            controller = GameObject.FindObjectOfType<PlayerController>();
            isSetup = true;
        }
    }
    public void Start()
    {
        Setup();

    }
    public void LoadList(string path, bool repeation = true)
    {
        index = 0;
        Resources.UnloadUnusedAssets();
        currentList = Resources.LoadAll<Sprite>(path);
        repeat = repeation;

    }
    public void NextImage()
    {
        if(currentList == null)
        {
         
            return;
        }
        if (currentList.Length > 0)
        {
            if (repeat == false)
            {
                if (index >= currentList.Length)
                {
                    return;
                }
            }
            if (me)
            {
                if (cam)
                {
                    if (cam.infoObject == me)
                    {
                        index++;
                        if (index >= currentList.Length)
                            index = 0;
                        render.sprite = currentList[index];
                    }
                }
            }
            else
            {

                index++;
                if (index >= currentList.Length)
                    index = 0;
                render.sprite = currentList[index];
            }
        }
    }
    private void Update()
    {
        CheckAnimation();
    }
    public void CheckAnimation()
    {
        if (anim)
        {

            if (me)
            {
                if (cam)
                {
                    if (cam.infoObject == me)
                    {
                        anim.speed = 1;
                    }
                    else
                    {
                        anim.speed = 0;
                    }
                }
                if(SHADOWANIM)
                {
                    if (cam.infoObject == me)
                    {
                        SHADOWANIM.speed = 1;
                    }
                    else
                    {
                        SHADOWANIM.speed = 0;
                    }
                }
            }
         
        }
    }
}
