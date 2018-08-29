using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : StatScript {

    [SerializeField]
    private int exp;
    public int EXP
    {
        get { return exp; }
        set { exp = value; }
    }
}
