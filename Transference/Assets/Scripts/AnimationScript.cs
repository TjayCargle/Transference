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
    GridObject me;
    CameraScript camera;
    public void Setup()
    {
        if (!isSetup)
        {
            obj = GetComponent<GridObject>();
            render = GetComponent<SpriteRenderer>();
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
            me = GetComponent<LivingObject>();
            camera = GameObject.FindObjectOfType<CameraScript>();
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

            index++;
            if (index >= currentList.Length)
                index = 0;
            render.sprite = currentList[index];
        }
    }
}
