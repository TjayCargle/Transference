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
        if (NEXT > 0)
        {
            if (NEXTCOUNT > 0)
            {
                NEXTCOUNT--;
                if (NEXTCOUNT <= 0)
                {

                    DatabaseManager database = GameObject.FindObjectOfType<DatabaseManager>();
                    if (database != null)
                    {
                        database.LearnSkill(NEXT, OWNER);

                    }
                }
            }
        }
        switch (REACT)
        {
            case AutoReact.healByDmg:
                OWNER.ChangeHealth((int)amount);
                return Reaction.none;
                break;
            case AutoReact.healAmount:
                OWNER.ChangeHealth(VAL);
                return Reaction.none;
                break;
            case AutoReact.extraAction:
                OWNER.ACTIONS++;
                return Reaction.none;
                break;
            case AutoReact.GainManaAmount:
                OWNER.ChangeMana(VAL);
                return Reaction.none;
                break;
            case AutoReact.HealFTByAmount:
                OWNER.ChangeFatigue(VAL);
                return Reaction.none;
                break;
            case AutoReact.reduceDef:
                return Reaction.reduceDef;
                break;
            case AutoReact.reduceStr:
                return Reaction.reduceStr;
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
            case AutoReact.ChargeFTByAmount:
                OWNER.ChangeFatigue(-VAL);
                return Reaction.none;
                break;
            case AutoReact.discoverItem:
                OWNER.GetComponent<LivingSetup>().dm.GetItem(UnityEngine.Random.Range(0, 11), OWNER);
                return Reaction.none;
                break;
            case AutoReact.GainManaByDmg:
                OWNER.ChangeMana((int)amount);
                break;
            case AutoReact.ChargeFTByDmg:
                OWNER.ChangeFatigue((int)-amount);
                break;
            case AutoReact.HealFTByDmg:
                OWNER.ChangeFatigue((int)amount);
                break;
            default:
                Debug.Log("No reaction error");
                return Reaction.none;
                break;
        }


        return Reaction.none;
    }

}
