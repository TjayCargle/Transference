using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableScript : ScriptableObject {

    [SerializeField]
    protected string myName;

    [SerializeField]
    private string description;

    protected int refType;

    public string DESC
    {
        get { return description; }
        set { description = value; }
    }
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
