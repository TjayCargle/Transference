using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : StatScript {

    [SerializeField]
    private int exp;

    [SerializeField]
    private int physexp;

    [SerializeField]
    private int magexp;

    [SerializeField]
    private int skillexp;

    [SerializeField]
    private int myLevel = 1;
    public int EXP
    {
        get { return exp; }
        set { exp = value; }
    }

    public int PHYSEXP
    {
        get { return physexp; }
        set { physexp = value; }
    }

    public int MAGEXP
    {
        get { return magexp; }
        set { magexp = value; }
    }
    public int SKILLEXP
    {
        get { return skillexp; }
        set { skillexp = value; }
    }
    public int LEVEL
    {
        get { return myLevel; }
        set { myLevel = value; }
    }


}
