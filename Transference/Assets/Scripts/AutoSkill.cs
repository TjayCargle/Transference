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
            case AutoReact.reduceStr:
                return Reaction.ApplyEffect;
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
            case AutoReact.reduceDef:
                break;
            case AutoReact.reduceMag:
                break;
            case AutoReact.reduceRes:
                break;
            case AutoReact.reduceSpd:
                break;
            case AutoReact.reduceLuck:
                break;
            default:
                Debug.Log("No reaction error");
                return Reaction.none;
                break;
        }


        return Reaction.none;
    }

    public override void ApplyAugment(Augment augment)
    {
        switch (augment)
        {
          
            case Augment.effectAugment1:
                CHANCE += 20.0f;
                break;
            case Augment.effectAugment2:
                CHANCE += 20.0f;
                break;
            case Augment.effectAugment3:
                CHANCE += 20.0f;
                break;
         
        }
    }
    public override void UpdateDesc()
    {
        base.UpdateDesc();
        DESC = CHANCE + " % chance + skl to ";

        switch (REACT)
        {
            case AutoReact.healByDmg:
                DESC += "heal the damage you dealt";
                break;
   
            case AutoReact.GainManaByDmg:
                DESC += "gain mana by the damage you dealt";
                break;

            case AutoReact.ChargeFTByDmg:
                DESC += "charge FT by the damage you dealt";
                break;
      
            case AutoReact.HealFTByDmg:
                DESC += "reduce FT by the damage you dealt";
                break;
 
            case AutoReact.extraAction:
                DESC += "gain an additional action";
                break;
   
            case AutoReact.reduceDef:
                DESC += "deal damage as if enemy def has halved";
                break;
       
            case AutoReact.reduceRes:
                DESC += "deal damage as if enemy res has halved";
                break;
       
         
            case AutoReact.discoverItem:
                DESC += "discover a random item";
                break;
        }

        switch (ACT)
        {
            case AutoAct.beforeDmg:
                DESC += " before doing ";
                break;
            case AutoAct.afterDmg:
                DESC += " after hitting with ";
                break;
        
        }
        DESC += " a basic attack";
    }

}
