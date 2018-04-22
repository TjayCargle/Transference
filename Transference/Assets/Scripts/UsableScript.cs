using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableScript : MonoBehaviour {
    [SerializeField]
    protected string myName;
    protected int refType;
    public string NAME
    {
        get { return myName; }
        set { myName = value; }
    }
    public int TYPE
    {
        get { return refType; }
        set { refType = value; }
    }
}
