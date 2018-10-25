using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimationScript : MonoBehaviour
{

    public Sprite[] currentList;
    public SpriteRenderer render;
    public GridObject obj;
    public int index;
    public bool isSetup = false;
    LivingObject me;
    PlayerController controller;
    CameraScript camera;
    [SerializeField]
    Animator anim;
    public void Setup()
    {
        if (!isSetup)
        {
            obj = GetComponent<GridObject>();
            render = GetComponent<SpriteRenderer>();
            if (!obj)
            {
                return;
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
            camera = GameObject.FindObjectOfType<CameraScript>();
            controller = GameObject.FindObjectOfType<PlayerController>();
            anim = GetComponent<Animator>();
            isSetup = true;
        }
    }
    public void Start()
    {
        Setup();

    }
    public void LoadList(string path)
    {
        index = 0;
        Resources.UnloadUnusedAssets();
        currentList = Resources.LoadAll<Sprite>(path);

    }
    public void NextImage()
    {
        if (currentList.Length > 0)
        {
            if (me)
            {
                if (camera)
                {
                    if (camera.infoObject == me)
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
                if (camera)
                {
                    if (camera.infoObject == me)
                    {
                        anim.speed = 1;
                    }
                    else
                    {
                        anim.speed = 0;
                    }
                }
            }
        }
    }
}
