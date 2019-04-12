using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSkill : SkillScript
{

    [SerializeField]
    protected int cost;

    [SerializeField]
    protected int accuraccy;

    [SerializeField]
    protected SideEffect effect;

    [SerializeField]
    protected DMG damage;

    [SerializeField]
    private List<Vector2> affecttedTiles;

    [SerializeField]
    protected RanngeType rType;

    [SerializeField]
    protected float critRate;

    [SerializeField]
    private BuffType buff;

    [SerializeField]
    private float modVal;

    [SerializeField]
    private ModifiedStat buffedStat;

    [SerializeField]
    protected int hitCount;

    [SerializeField]
    protected EType eType;

    [SerializeField]
    protected Reaction reaction;

    [SerializeField]
    protected int next;

    [SerializeField]
    protected int nextCount;

    [SerializeField]
    protected int minHit = -1;
    [SerializeField]
    protected int maxHit = -1;

    public int COST
    {
        get { return cost; }
        set { cost = value; }
    }

    public int ACCURACY
    {
        get { return accuraccy; }
        set { accuraccy = value; }
    }

    public SideEffect EFFECT
    {
        get { return effect; }
        set { effect = value; }
    }

    public DMG DAMAGE
    {
        get { return damage; }
        set { damage = value; }
    }
    public List<Vector2> TILES
    {
        get { return affecttedTiles; }
        set { affecttedTiles = value; }
    }

    public float CRIT_RATE
    {
        get { return critRate; }
        set { critRate = value; }
    }

    public RanngeType RTYPE
    {
        get { return rType; }
        set { rType = value; }
    }
    public Reaction REACTION
    {
        get { return reaction; }
        set { reaction = value; }
    }
    public int HITS
    {
        get { return hitCount; }
        set { hitCount = value; }
    }

    public BuffType BUFF
    {
        get { return buff; }
        set { buff = value; }
    }

    public float BUFFVAL
    {
        get { return modVal; }
        set { modVal = value; }
    }

    public ModifiedStat BUFFEDSTAT
    {
        get { return buffedStat; }
        set { buffedStat = value; }
    }

    public EType ETYPE
    {
        get { return eType; }
        set { eType = value; }
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

    public int MIN_HIT
    {
        get { return minHit; }
        set { minHit = value; }
    }

    public int MAX_HIT
    {
        get { return maxHit; }
        set { maxHit = value; }
    }
    public int GetCost(LivingObject user, float modification = 1.0f)
    {
        if (ETYPE == EType.physical)
        {
            return (int)(COST * modification); ;
        }
        else
        {
            return (int)(COST * modification);

        }
    }
    public SkillScript UseSkill(LivingObject user, float modification = 1.0f)
    {

        if (ETYPE == EType.magical)
        {
            OWNER.STATS.MANA -= (int)(COST * modification);
        }
        else
        {
            OWNER.STATS.FATIGUE += (int)(COST * modification);
        }

        if (NEXT > 0)
        {
            if (NEXTCOUNT > 0)
            {
                NEXTCOUNT--;



            }
        }
        return null;

    }
    public bool CanUse(float modification = 1.0f)
    {
        bool can = false;
        int amt = 0;
        switch (ETYPE)
        {
            case EType.physical:
                amt = owner.FATIGUE + (int)(COST * modification);
                if (amt <= owner.MAX_FATIGUE)
                {
                    if (amt >= 0)
                    {

                        can = true;
                    }
                }
                break;
            case EType.magical:
                amt = owner.MANA - (int)(COST * modification);

                if (amt >= 0)
                {
                    if (amt <= OWNER.MAX_MANA)
                    {
                        can = true;

                    }
                }
                break;
        }
        return can;
    }

    public override void UpdateDesc()
    {
        this.DESC = PossibleDesc(RTYPE, DAMAGE, HITS, EFFECT);
    }

    public string PossibleDesc(RanngeType pRtype, DMG pDMG, int pHits, SideEffect pEffect)
    {
        string potentialDesc;
        if(SUBTYPE == SubSkillType.Buff)
        {
            potentialDesc = "Increases " + BUFFEDSTAT + " of yourself or ally by " + BUFFVAL + "% for 3 turns";
        }
        else if(SUBTYPE == SubSkillType.Debuff)
        {
            potentialDesc = "Decreases " + BUFFEDSTAT + " of yourself or ally by " + BUFFVAL + "% for 3 turns";
        }
        else if (SUBTYPE == SubSkillType.Ailment)
        {
            potentialDesc = ACCURACY + "% chance to inflict enemy with " + EFFECT;
        }
        else if (ELEMENT == Element.Support)
        {
            switch (EFFECT)
            {
                case SideEffect.heal:
                    {
                        potentialDesc = "Heals " + DAMAGE + " amount of health to target";
                    }
                    break;
                default:
                    potentialDesc = "u messed up ";
                    break;
            }
  
        }
        else
        {
            potentialDesc = ACCURACY + "% chance to deal " + pDMG + " " + this.ELEMENT + " based " + this.ETYPE + " damage to";
        }

        switch (RTYPE)
        {
            case RanngeType.single:
                if(TILES.Count > 1)
                {
                    potentialDesc += " targets in range ";
                }
                else
                {
                    potentialDesc += " target ";
                }
                break;
            case RanngeType.multi:
                potentialDesc += " target ";
                break;
            case RanngeType.area:
                potentialDesc += " everyone in range ";
                break;
            case RanngeType.any:
                potentialDesc += " yourself or target ";
                break;
            case RanngeType.anyarea:
                potentialDesc += " everyone including yourself in range ";
                break;
            case RanngeType.multiarea:
                potentialDesc += " everyone in selected area ";
                break;
            default:
                break;
        }

        if (SUBTYPE == SubSkillType.RngAtk)
        {
            potentialDesc += "" + MIN_HIT + "-" + MAX_HIT + " times";
        }
        else if (HITS > 1)
        {
            potentialDesc += "" + HITS + " times";

        }
   
        if (pEffect != SideEffect.none)
        {
            if (this.EFFECT < SideEffect.reduceStr)
            {
                potentialDesc += " with a chance of " + this.EFFECT.ToString();
            }
            else
            {
                potentialDesc += " with a chance to debuff " + Common.GetSideEffectText(this.EFFECT);
            }
        }
        return potentialDesc;
    }
    public override void ApplyAugment(Augment augment)
    {
        switch (augment)
        {
            case Augment.levelAugment:
                // DAMAGE = Common.GetNextDmg(DAMAGE);
                for (int i = 0; i < 5; i++)
                {
                    LevelUP();
                }
                break;
            case Augment.accurracyAugment:
                ACCURACY = (int)((float)ACCURACY * 1.2f);
                break;
            case Augment.sideEffectAugment:
                EFFECT = effect;
                break;
            case Augment.rangeAument:
                //todo
                break;
            case Augment.attackCountAugment:
                if(SUBTYPE == SubSkillType.RngAtk)
                {
                    MIN_HIT++;
                    MAX_HIT++;
                }
                else
                {
                HITS++;
                }
                break;
            case Augment.costAugment:
                COST = (int)((float)COST * 0.5f);
                break;
            case Augment.chargeIncreaseAugment:
                COST = (int)((float)COST * 1.5f);
                break;
            case Augment.chargeDecreaseAugment:
                COST = (int)((float)COST * 0.5f);
                break;
            case Augment.effectAugment1:
                BUFFVAL *= 2;
                break;
            case Augment.effectAugment2:
                BUFFVAL *= 2;
                break;
            case Augment.effectAugment3:
                BUFFVAL *= 2;
                break;

            case Augment.elementAugment:
                ELEMENT = Common.ChangeElement(ELEMENT);
                break;
            default:
                break;
        }
    }



    public override void LevelUP()
    {
        base.LevelUP();

        if(level % 5 == 0)
        {

        switch (ELEMENT)
        {
            case Element.Water:
                    DAMAGE = Common.GetNextDmg(DAMAGE);
                    break;
            case Element.Fire:
                    DAMAGE = Common.GetNextDmg(DAMAGE);
                    TILES.Add(Common.GetNextTileRange(TILES));
                    break;
            case Element.Ice:
                    DAMAGE = Common.GetNextDmg(DAMAGE);
                    break;
            case Element.Electric:
                    DAMAGE = Common.GetNextDmg(DAMAGE);
                    MAX_HIT++;
                break;
            case Element.Slash:
                    DAMAGE = Common.GetNextDmg(DAMAGE);
                    HITS++;
                break;
            case Element.Pierce:
                    DAMAGE = Common.GetNextDmg(DAMAGE);
                    break;
            case Element.Blunt:
                    DAMAGE = Common.GetNextDmg(DAMAGE);
                    ACCURACY += 5;
                    if (ACCURACY > 100)
                        ACCURACY = 100;
                    CRIT_RATE += 0.1f;
                    break;
            case Element.Buff:
                    BUFFVAL += 0.15f;
                break;
            case Element.Support:
                break;
            case Element.Ailment:
                break;
            case Element.Passive:
                break;
            case Element.Opp:
                break;
            case Element.Auto:
                break;
            case Element.none:
                break;
        }

        }

        UpdateDesc();
    }
}
