using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableScript : MonoBehaviour {
    [SerializeField]
    protected string myName;

    public string NAME
    {
        get { return myName; }
        set { myName = value; }
    }
}
