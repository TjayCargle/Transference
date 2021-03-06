﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatScript : MonoBehaviour
{

    [SerializeField]
    private int myMaxHealth = 100;
   
    [SerializeField]
    private int myMaxMana = 100;

    [SerializeField]
    private int myMaxFatigue = 100;

    [SerializeField]
    private int myBaseStr;
    [SerializeField]
    private int myBaseMag;
    [SerializeField]
    private int myBaseDefense;
    [SerializeField]
    private int myBaseResistance;
    [SerializeField]
    private int myBaseSpeed;
    [SerializeField]
    private int myBaseSkill;
    [SerializeField]
    private int myMoveDist = 0;
    [SerializeField]
    private int myAtkDist = 0;
    [SerializeField]
    List<string> appliedPassives = new List<string>();
    public int type = 0;

    [SerializeField]
    private float manaCostChange = 1.0f;
    [SerializeField]
    private float fatigueCostChange = 1.0f;
    [SerializeField]
    private float healthCostChange = 1.0f;
    [SerializeField]
    private float fatigueChargeChange = 1.0f;
    private LivingObject owner;

    public List<string> MODS
    {
        get { return appliedPassives; }
        set { appliedPassives = value; }
    }

    public LivingObject USER
    {
        get { return owner; }
        set { owner = value; }
    }
    public int MOVE_DIST
    {
        get { return myMoveDist; }
        set { myMoveDist = value; }
    }
    public int Atk_DIST
    {
        get { return myAtkDist; }
        set { myAtkDist = value; }
    }

    public int STRENGTH
    {
        get { return myBaseStr; }
        set { myBaseStr = value;
            if (myBaseStr > Common.MaxLevel)
            {
                myBaseStr = Common.MaxLevel;
            }
        }
    }
    public int MAGIC
    {
        get { return myBaseMag; }
        set { myBaseMag = value;
            if (myBaseMag > Common.MaxLevel)
            {
                myBaseMag = Common.MaxLevel;
            }
        }
    }
    public int DEFENSE
    {
        get { return myBaseDefense; }
        set { myBaseDefense = value;
            if (myBaseDefense > Common.MaxLevel)
            {
                myBaseDefense = Common.MaxLevel;
            }
        }
    }
    public int RESIESTANCE
    {
        get { return myBaseResistance; }
        set { myBaseResistance = value;
            if (myBaseResistance > Common.MaxLevel)
            {
                myBaseResistance = Common.MaxLevel;
            }
        }
    }
    public int SPEED
    {
        get { return myBaseSpeed; }
        set
        {
            myBaseSpeed = value;
            if (myBaseSpeed > Common.MaxLevel)
            {
                myBaseSpeed = Common.MaxLevel;
            }
        }
    }
    public int DEX
    {
        get { return myBaseSkill; }
        set { myBaseSkill = value;
            if (myBaseSkill > Common.MaxLevel)
            {
                myBaseSkill = Common.MaxLevel;
            }
        }
    }
    public int MAX_HEALTH
    {
        get { return myMaxHealth; }
        set { myMaxHealth = value; }
    }

    public int MAX_MANA
    {
        get { return myMaxMana; }
        set { myMaxMana = value; }
    }

    public int MAX_FATIGUE
    {
        get { return myMaxFatigue; }
        set { myMaxFatigue = value; }
    }


    public float MANACHANGE
    {
        get { return manaCostChange; }
        set { manaCostChange = value; }
    }
    public float FTCOSTCHANGE
    {
        get { return fatigueCostChange; }
        set { fatigueCostChange = value; }
    }
    public float HPCOSTCHANGE
    {
        get { return healthCostChange; }
        set { healthCostChange = value; }
    }
    public float FTCHARGECHANGE
    {
        get { return fatigueChargeChange; }
        set { fatigueChargeChange = value; }
    }

    public override string ToString()
    {
        return "" + MAX_HEALTH + "," + MAX_MANA + "," + MAX_FATIGUE + "," + MOVE_DIST + "," + STRENGTH + "," + MAGIC + "," + DEX + "," + DEFENSE + "," + RESIESTANCE + "," + SPEED;
    }

    public virtual void IncreaseStat(ModifiedStat mod, int val, LivingObject baseObj)
    {

        float modification = val;
        switch (mod)
        {
            case ModifiedStat.Health:
                MAX_HEALTH += val;
   
                break;
            case ModifiedStat.ElementDmg:
                break;
            case ModifiedStat.Movement:

                MOVE_DIST += val;
                break;
            case ModifiedStat.Str:
                modification = ((float)val / 100) * baseObj.STRENGTH;
                STRENGTH += (int)modification;
                break;
            case ModifiedStat.Mag:
                modification = ((float)val / 100) * baseObj.MAGIC;
                MAGIC += (int)modification;
                break;
            case ModifiedStat.Atk:
                modification = ((float)val / 100) * baseObj.STRENGTH;
                STRENGTH += (int)modification;
                modification = ((float)val / 100) * baseObj.MAGIC;
                MAGIC += (int)modification;
                modification = ((float)val / 100) * baseObj.DEX;
                DEX += (int)modification;
                break;
            case ModifiedStat.Def:
                modification = ((float)val / 100) * baseObj.DEFENSE;
                DEFENSE += (int)modification;
                break;
            case ModifiedStat.Res:
                modification = ((float)val / 100) * baseObj.RESIESTANCE;
                RESIESTANCE += (int)modification;
                break;
            case ModifiedStat.Guard:
                modification = ((float)val / 100) * baseObj.DEFENSE;
                DEFENSE += (int)modification;
                modification = ((float)val / 100) * baseObj.RESIESTANCE;
                RESIESTANCE += (int)modification;
                modification = ((float)val / 100) * baseObj.SPEED;
                SPEED += (int)modification;
                break;
            case ModifiedStat.Spd:
                modification = ((float)val / 100) * baseObj.SPEED;
                SPEED += (int)modification;
                break;
            case ModifiedStat.Dex:
                modification = ((float)val / 100) * baseObj.DEX;
                DEX += (int)modification;
                break;

            case ModifiedStat.FT:
                break;
            case ModifiedStat.FTCost:
                //  Debug.Log("v = " + val);
                //  Debug.Log("m = " + modification);
                //   Debug.Log("FT = " + FTCOSTCHANGE);
                modification = ((float)val / 100);
                FTCOSTCHANGE = 1.0f - modification;
                if (FTCOSTCHANGE < 0.1)
                    FTCOSTCHANGE = 0.1f;
                break;
            case ModifiedStat.SPCost:
                modification = ((float)val / 100.0f);
                MANACHANGE= 1.0f -modification;
                if (MANACHANGE < 0.1)
                    MANACHANGE = 0.1f;
                break;
            case ModifiedStat.dmg:
                break;
            case ModifiedStat.FTCharge:
                modification = ((float)val / 100);
                FTCHARGECHANGE = 1.0f - modification;
                if (FTCHARGECHANGE < 0.1)
                    FTCHARGECHANGE = 0.1f;
                break;
            case ModifiedStat.none:
                break;
            case ModifiedStat.all:
                break;
            default:
                break;
        }
    }
    public virtual void SetZero(bool hard = false)
    {
        Atk_DIST = 0;
       

        STRENGTH = 0;
        MAGIC = 0;
        DEFENSE = 0;
        RESIESTANCE = 0;
        SPEED = 0;
        DEX = 0;

        FTCOSTCHANGE = 1.0f;
        FTCHARGECHANGE = 1.0f;
        MANACHANGE = 1.0f;

        if (hard == true)
        {

            MAX_HEALTH = 0;
            MAX_MANA = 0;
            MAX_FATIGUE = 0;

        }
    }
    public virtual void Reset(bool hard = false)
    {

        STRENGTH = 0;
        MAGIC = 0;
        DEFENSE = 0;
        RESIESTANCE = 0;
        SPEED = 0;
        DEX = 0;
        FTCOSTCHANGE = 1.0f;
        MANACHANGE = 1.0f;
        FTCHARGECHANGE = 1.0f;
        if (hard == true)
        {
            MAX_HEALTH = 0;
            MAX_MANA = 0;
            MAX_FATIGUE = 0;
        
       
          
        }

    }
}
