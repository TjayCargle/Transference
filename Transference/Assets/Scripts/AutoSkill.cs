using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSkill : SkillScript
{


    [SerializeField]
    protected float chance;

    [SerializeField]
    protected int next;

    [SerializeField]
    protected int nextCount;

    [SerializeField]
    AutoAct actType;

    [SerializeField]
    AutoReact actReact;

    [SerializeField]
    protected int PersonalValue;

    public int VAL
    {
        get { return PersonalValue; }
        set { PersonalValue = value; }
    }

    public float CHANCE
    {
        get { return chance; }
        set { chance = value; }
    }

    public int NEXT
    {
        get { return next; }
        set { next = value; }
    }

    public int NEXTCOUNT
    {
        get { return nextCount; }
        set { nextCount = value; }
    }

    public AutoAct ACT
    {
        get { return actType; }
        set { actType = value; }
    }

    public AutoReact REACT
    {
        get { return actReact; }
        set { actReact = value; }
    }

    public Reaction Activate(float amount)
    {
        switch (REACT)
        {
            case AutoReact.healByDmg:
                OWNER.STATS.HEALTH += (int)amount;
                return Reaction.none;
                break;
            case AutoReact.healAmount:
                OWNER.STATS.HEALTH += VAL;
                return Reaction.none;
                break;
            case AutoReact.extraAction:
                break;
            case AutoReact.recoverSP:
                OWNER.STATS.MANA += VAL;
                return Reaction.none;
                break;
            case AutoReact.reduceFT:
                OWNER.STATS.FATIGUE -= VAL;
                return Reaction.none;
                break;
            case AutoReact.reduceDef:
                return Reaction.reduceDef;
                break;
            case AutoReact.reduceAtk:
                return Reaction.reduceAtk;
                break;
            case AutoReact.reduceSpd:
                return Reaction.reduceSpd;
                break;
            case AutoReact.reduceMag:
                return Reaction.reduceMag;
                break;
            case AutoReact.reduceRes:
                return Reaction.reduceRes;
                break;
            case AutoReact.reduceLuck:
                return Reaction.reduceLuck;
                break;
            default:
                Debug.Log("No reaction error");
                return Reaction.none;
                break;
        }

     
        return Reaction.none;
    }

    public List<Type> acceptableTypes = new List<Type>();
    public void loadTypes()
    {
        acceptableTypes.Add(typeof(bool));
        //and repeat
    }
    public System.Object Convert(Type propType, string myString)
    {
        var converter =  TypeDescriptor.GetConverter(propType);
        var result = converter.ConvertFrom(myString);
        return result;
    }
}
