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
    public LivingObject me;
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

            if(render == null)
            {
            render = obj.gameObject.GetComponent<SpriteRenderer>();
            }


            anim = obj.GetComponent<Animator>();
            if (!anim)
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
            Sprite[] possibleFaces = Resources.LoadAll<Sprite>("" + shrtname + "/Face/");
            index = 0;
            if (currentList.Length > 0)
            {
                render.sprite = currentList[index];
            }
            if (possibleFaces.Length > 0)
            {
                obj.FACE = possibleFaces[0];
            }
            if (me == null)
            {
                me = GetComponent<LivingObject>();

            }
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
        render.sprite = currentList[index];
    }
    public void NextImage()
    {
        if (currentList == null)
        {
            return;
        }
        if (currentList.Length > 0)
        {
            if (repeat != true)
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
            else if (obj)
            {
                if(obj.FACTION == Faction.ordinary)
                {
                    if (cam.infoObject == obj)
                    {
                        index++;
                        if (index >= currentList.Length)
                            index = 0;
                        render.sprite = currentList[index];
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
                if (SHADOWANIM)
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

    public void Unset()
    {
        obj = null;
        currentList = null;
        Resources.UnloadUnusedAssets();
        isSetup = false;
    }
}
