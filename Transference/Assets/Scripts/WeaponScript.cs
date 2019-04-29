using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : UsableScript
{
    [SerializeField]
    private int myAttack;
    [SerializeField]
    private int myAccurracy;
    [SerializeField]
    private int myCritChance;
    [SerializeField]
    private int myStartkDist;
    [SerializeField]
    private int myAttackRange;
    [SerializeField]
    private RangeType range;
    [SerializeField]
    private EType eType;
    [SerializeField]
    private int useCount;
    [SerializeField]
    private ModifiedStat boost;
    [SerializeField]
    private int boostVal;


    private Element myAfinity = Element.Slash;

    private LivingObject owner;

    public LivingObject USER
    {
        get { return owner; }
        set { owner = value; }
    }
    public Element AFINITY
    {
        get { return myAfinity; }
        set { myAfinity = value; }
    }
    public EType ATTACK_TYPE
    {

        get { return eType; }
        set { eType = value; }
    }
    public int ATTACK
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
    public int DIST
    {
        get { return myStartkDist ; }
        set { myStartkDist = value; }
    }
    public int Range
    {
        get { return myAttackRange; }
        set { myAttackRange = value; }
    }

    public RangeType ATKRANGE
    {
        get { return range; }
        set { range = value; }
    }

    public int USECOUNT
    {
        get { return useCount; }
        set { useCount = value; }
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
      
            case Augment.rangeAument:
                Range++;
                DIST++;
                break;
   
            case Augment.elementAugment:
                AFINITY = Common.ChangeElement(AFINITY);
                break;
       
        }
    }


    public void Use()
    {
        useCount++;
        if (USECOUNT % 2 == 0)
        {
            LevelUP();
           
        }
    }

    public override void LevelUP()
    {
        base.LevelUP();
        LEVEL++;
        ATTACK++;
        ACCURACY++;
        if (LEVEL > Common.MaxSkillLevel)
        {
            LEVEL = Common.MaxSkillLevel;
        }
        if (ATTACK > (int)DMG.collassal)
        {
            ATTACK = (int)DMG.collassal;
        }
        if (ACCURACY > 100)
        {
            ACCURACY = 100;
        }
    }

    public override void UpdateDesc()
    {
        base.UpdateDesc();

        DESC = "" + BOOST.ToString() + " +" + BOOSTVAL + ".";

        DESC += "Deals " + ATTACK_TYPE + " " + AFINITY + " based dmg.";

        if (Range == 1)
        {
            DESC += " Hits a tile " + DIST + " space away";
        }
        else
        {
            DESC += " Hits " + Range + " tiles  " + DIST + " spaces away";
        }
    }
}
