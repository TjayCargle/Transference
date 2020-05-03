using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : UsableScript
{
    [SerializeField]
    private DMG myAttack;
    [SerializeField]
    private int myAccurracy;
    [SerializeField]
    private int myCritChance;
    [SerializeField]
    private int cost;
    //  [SerializeField]
    //    private int myAttackRange;
    [SerializeField]
    private RangeType range;
    [SerializeField]
    private EType eType;

    [SerializeField]
    private ModifiedStat boost;
    [SerializeField]
    private int boostVal;

    [SerializeField]
    private Element myAfinity = Element.Slash;


    public Element ELEMENT
    {
        get { return myAfinity; }
        set { myAfinity = value; }
    }
    public EType ATTACK_TYPE
    {

        get { return eType; }
        set { eType = value; }
    }
    public DMG ATTACK
    {
        get { return myAttack; }
        set { myAttack = value; }
    }
    public int ACCURACY
    {
        get { return myAccurracy; }
        set { myAccurracy = value; }
    }
    public int CRIT
    {
        get { return myCritChance; }
        set { myCritChance = value; }
    }

    public int COST
    {
        get { return cost; }
        set { cost = value; }
    }
    //public int DIST
    //{
    //    get { return myStartkDist; }
    //    set { myStartkDist = value; }
    //}
    //public int Range
    //{
    //    get { return myAttackRange; }
    //    set { myAttackRange = value; }
    //}

    public RangeType ATKRANGE
    {
        get { return range; }
        set { range = value; }
    }



    public ModifiedStat BOOST
    {
        get { return boost; }
        set { boost = value; }
    }
    public int BOOSTVAL
    {

        get { return boostVal; }
        set { boostVal = value; }
    }

    public int GetCost(LivingObject user, float modification = 1.0f)
    {
        if (USER)
            return (int)((((float)(cost) / 100.0f) * USER.MAX_HEALTH) * user.STATS.HPCOSTCHANGE);
        else
            return (int)(cost * modification);
    }

    public bool CanUse(float modification = 1.0f)
    {
        bool can = false;
        int amt = 0;
        amt = GetCost(USER, modification);


        //  if (amt <= owner.FATIGUE)
        {
            if (owner.HEALTH > amt)
            {

                can = true;
            }
        }
        return can;
    }

    public override void ApplyAugment(Augment aug)
    {
        switch (aug)
        {
            case Augment.none:
                break;
            case Augment.levelAugment:
                for (int i = 0; i < 5; i++)
                {
                    LevelUP();
                }
                break;
            case Augment.accurracyAugment:
                {
                    ACCURACY += 5;
                }
                break;

            case Augment.rangeAugment:
                {
                    ATKRANGE = (RangeType)((int)(Random.Range((int)RangeType.adjacent, (int)RangeType.crosshair)));
                }

                break;

            case Augment.elementAugment:
                ELEMENT = Common.ChangeElement(ELEMENT);
                break;

        }
    }


    public void Use()
    {
        USER.ChangeHealth(-1 * GetCost(USER, USER.STATS.HPCOSTCHANGE), false);
        useCount++;
        //if (USECOUNT % 2 == 0)
        //{
        //    LevelUP();
       


        //}
    }

    public override void LevelUP()
    {
        // base.LevelUP();
        if (LEVEL < Common.MaxSkillLevel)
        {
            LEVEL++;

            if (LEVEL > Common.MaxSkillLevel)
            {
                LEVEL = Common.MaxSkillLevel;
            }
            if (level % 2 == 0)
                ATTACK = Common.GetNextDmg(ATTACK);
            if (ATTACK > DMG.collassal)
            {
                ATTACK = DMG.collassal;
            }
            ACCURACY++;
            if (ACCURACY > 100)
            {
                ACCURACY = 100;
            }

        }
        UpdateDesc();
    }
    public string GetCurrentLevelStats()
    {
        string returnedString = "";
        if (level < Common.MaxSkillLevel)
        {
            returnedString = "Level " + LEVEL + "";
        }
        else
        {
            returnedString = "Max Level ";
        }

        returnedString += "\n Damage: " + ATTACK.ToString() + "";
        returnedString += "\n Accuracy: " + (ACCURACY) + "";
        //  returnedString += "\n Boosts: " + (BOOST) + " +" + BOOSTVAL;
        return returnedString;
    }
    public string GetNextLevelStats()
    {
        string returnedString = "";
        if (level + 1 < Common.MaxSkillLevel)
        {
            returnedString = "<color=green>Level " + (LEVEL + 1) + "</color>";
        }
        else if (level + 1 == Common.MaxSkillLevel)
        {
            returnedString = "<color=green>Max Level</color>";
        }
        else
        {
            return GetCurrentLevelStats();
        }
        if (level + 1 % 2 == 0)
        {
            if (ATTACK != DMG.collassal)
            {
                returnedString += "\n <color=green>Damage: " + Common.GetNextDmg(ATTACK).ToString() + "</color>";
            }
            else
            {
                returnedString += "\n Damage: " + Common.GetNextDmg(ATTACK).ToString() + "";
            }
        }
        else
        {
            returnedString += "\n Damage: " + Common.GetNextDmg(ATTACK).ToString() + "";
        }

        if (ACCURACY + 1 <= 100)
        {
            returnedString += "\n <color=green>Accuracy: " + (ACCURACY + 1) + "</color>";
        }
        else
        {
            returnedString += "\n Accuracy: 100";
        }

        return returnedString;
    }

    public override void UpdateDesc()
    {
        base.UpdateDesc();

        //  DESC = "" + BOOST.ToString() + " +" + BOOSTVAL + ".";

        DESC = "Deals " + ATTACK_TYPE + " " + ELEMENT + " based dmg.";
        DESC += " Costs " + (cost) + "% Health.";

        //if (Range == 1)
        //{
        //    DESC += " Hits a tile " + DIST + " space away";
        //}
        //else
        //{
        //    DESC += " Hits " + Range + " tiles  " + DIST + " spaces away";
        //}
    }
}
