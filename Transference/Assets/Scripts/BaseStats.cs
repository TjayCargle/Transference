using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : StatScript {

    [SerializeField]
    private int exp;

    [SerializeField]
    private int myLevel = 1;
    public int EXP
    {
        get { return exp; }
        set { exp = value; }
    }

    public int LEVEL
    {
        get { return myLevel; }
        set { myLevel = value; }
    }
}
