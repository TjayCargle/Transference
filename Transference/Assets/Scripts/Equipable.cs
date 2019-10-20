using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipable : MonoBehaviour {
    [SerializeField]
    protected string myName;

    [SerializeField]
    protected string description;

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

    public virtual void Equip(UsableScript usable)
    {
        if (usable == null)
            return;
        this.DESC = usable.DESC;
        this.NAME = usable.NAME;
        this.TYPE = usable.TYPE;
    }
}
