using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorScript : UsableScript
{
    [SerializeField]
    private int myDefense;
    [SerializeField]
    private int myRes;
    [SerializeField]
    private int mySpeed;

    [SerializeField]
    private List<EHitType> hitList;

    //[SerializeField]
    //private int myHealth;

    //[SerializeField]
    //private int myMaxHealth;

    [SerializeField]
    private float healthPercent = 100.0f;

    [SerializeField]
    private float maxHealthPercent = 100.0f;
    private LivingObject owner;
    [SerializeField]
    private int breakCount = 0;

    public List<EHitType> HITLIST
    {
        get { return hitList; }
        set { hitList = value; }
    }

    public LivingObject USER
    {
        get { return owner; }
        set { owner = value; }
    }
    public int DEFENSE
    {
        get { return myDefense; }
        set { myDefense = value; }
    }
    public int RESISTANCE
    {
        get { return myRes; }
        set { myRes = value; }
    }
    public int SPEED
    {
        get { return mySpeed; }
        set { mySpeed = value; }
    }
    public int BREAKS
    {
        get { return breakCount; }
        set { breakCount = value; }
    }
    public float MAX_HEALTH
    {
        get { return maxHealthPercent; }
        set
        {
            maxHealthPercent = value;
            if (maxHealthPercent > 100)
            {
                maxHealthPercent = 100;
            }
        }
    }
    public float HEALTH
    {
        get { return healthPercent; }
        set
        {
            healthPercent = value;
            if (healthPercent > maxHealthPercent)
            {
                healthPercent = maxHealthPercent;
            }

            if (healthPercent < 0)
            {
                healthPercent = 0;
            }
        }
    }
    public override void LevelUP()
    {
        base.LevelUP();
        if(LEVEL < Common.MaxSkillLevel)
        {
            LEVEL++;
            DEFENSE++;
            RESISTANCE++;
            SPEED++;
        }
    }
    public override void UpdateDesc()
    {
        base.UpdateDesc();
        DESC = "Def +" + DEFENSE + ", Res+" + RESISTANCE + " Spd +" + SPEED;
        for (int i = 0; i < HITLIST.Count; i++)
        {
            if (HITLIST[i] != EHitType.normal)
            {
                if (HITLIST[i] < EHitType.normal)
                {
                    DESC += ", " + HITLIST[i].ToString() + " " + (Element)i;
                }
                else
                {
                    DESC += ", " + HITLIST[i].ToString() + " to " + (Element)i;
                }
            }
        }
    }
    public override void ApplyAugment(Augment aug)
    {
        base.ApplyAugment(aug);
        switch (aug)
        {
     
            case Augment.levelAugment:
                for (int i = 0; i < 5; i++)
                {
                    LevelUP();
                }
                break;
           
            case Augment.effectAugment1:
                for (int i = 0; i < HITLIST.Count; i++)
                {
                    if(HITLIST[i] < EHitType.normal && HITLIST[i] > EHitType.absorbs)
                    {
                        HITLIST[i] = HITLIST[i] - 1;
                    }
                    else if (HITLIST[i] > EHitType.normal && HITLIST[i] < EHitType.lethal)
                    {
                        HITLIST[i] = HITLIST[i] + 1;
                    }
                }
                break;
           
            case Augment.strAugment:
                break;
            case Augment.magAugment:
                break;
            case Augment.sklAugment:
                break;
            case Augment.defAugment:
                DEFENSE += 5;
                break;
            case Augment.resAugment:
                RESISTANCE += 5;
                break;
            case Augment.spdAugment:
                SPEED += 5;
                break;
         
        }
    }
}
