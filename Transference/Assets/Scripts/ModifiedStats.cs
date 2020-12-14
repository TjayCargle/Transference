using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiedStats : StatScript {

    [SerializeField]
    private int myHealth;

    [SerializeField]
    private int myFatigue;

    [SerializeField]
    private int myMana;

    public int HEALTH
    {
        get { return myHealth; }
        set
        {
            myHealth = value;
            if (myHealth < 0)
            {
                myHealth = 0;
            }


        }
    }

    public int FATIGUE
    {
        get { return myFatigue; }
        set
        {
            myFatigue = value;
            if (myFatigue < 0)
            {
                myFatigue = 0;
            }


        }
    }


    public int MANA
    {
        get { return myMana; }
        set
        {
            myMana = value;

        }
    }


    public override void IncreaseStat(ModifiedStat mod, int val, LivingObject baseObj)
    {
        base.IncreaseStat(mod, val, baseObj);
        float modification = val;
        switch (mod)
        {
    
            case ModifiedStat.SP:
                MAX_MANA += val;
                if (MANA == MAX_MANA - val)
                {
                    MANA = MAX_MANA;
                }
                break;
            
            default:
                break;
        }
    }
    public override void SetZero(bool hard = false)
    {
        base.SetZero();
        HEALTH = 0;
        MANA = 0;
        FATIGUE = 0;
    }
    public override void Reset(bool hard = false)
    {
        base.Reset(hard);
        if (hard == true)
        {

            MANA = 0;
            FATIGUE = 0;
            HEALTH = 0;
        }

    }

    public override string ToString()
    {
        return "" + HEALTH + "," + MANA + "," + FATIGUE;
    }


}
