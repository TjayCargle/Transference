using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : UsableScript
{
    [SerializeField]
    private int targetRange;
    [SerializeField]
    private ItemType iType;
    [SerializeField]
    private ItemTarget tType;
    [SerializeField]
    private int trueValue;

    public int RANGE
    {
        get { return targetRange; }
        set { targetRange = value; }
    }
    
    public ItemType ITYPE
    {
        get { return iType; }
        set { iType = value; }
    }
    public ItemTarget TTYPE
    {
        get { return tType; }
        set { tType = value; }
    }
    public int VALUE
    {
        get { return trueValue; }
        set { trueValue = value; }
    }
}
