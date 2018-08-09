using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimationScript : MonoBehaviour {

    public Sprite[] currentList;
    public SpriteRenderer render;
    public GridObject obj;
    public int index;
    public bool isSetup = false;
    public void Setup()
    {
        if(!isSetup)
        {
            obj = GetComponent<GridObject>();
            render = GetComponent<SpriteRenderer>();
           // Debug.Log("InitialList");
            currentList = Resources.LoadAll<Sprite>("" + obj.FullName + "/Idle/"); //obj.FullName + "/Idle/");
            index = 0;
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
        if(currentList.Length > 0)
        {
            index++;
            if (index >= currentList.Length)
                index = 0;
            render.sprite = currentList[index];
        }
    }
}
