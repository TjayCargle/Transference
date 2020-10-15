using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCanvasManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> myChildren;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            myChildren.Add(transform.GetChild(i).gameObject);
        }
    }
    public void Showcase(int index)
    {
        for (int i = 0; i < myChildren.Count; i++)
        {
            if(i == index)
            {
                myChildren[i].SetActive(true);
            }
            else
            {
                myChildren[i].SetActive(false);
            }
        }
    }
}
