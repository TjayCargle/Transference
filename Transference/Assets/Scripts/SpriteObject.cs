using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteObject : MonoBehaviour
{
    public bool isSetup = false;
    public SpriteRenderer sr = null;
    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

   public void Setup()
    {
        if(!isSetup)
        {
            sr = GetComponent<SpriteRenderer>();
            if(sr == null)
            {
                sr = gameObject.AddComponent<SpriteRenderer>();
            }
        }
    }
}
