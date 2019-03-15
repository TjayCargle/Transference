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

    public void UpdateDesc()
    {
        this.DESC = PossibleDesc(RTYPE, DAMAGE, HITS, EFFECT);
    }

    public string PossibleDesc(RanngeType pRtype, DMG pDMG, int pHits, SideEffect pEffect)
    {
        string potentialDesc;
        if (pRtype == RanngeType.area)
        {
            potentialDesc = "Deals " + pDMG + " " + this.ELEMENT + " based " + this.ETYPE + " damage to " + this.DESC + " in range";
        }
        else if (pHits == 1)
        {
            potentialDesc = "Deals " + pDMG + " " + this.ELEMENT + " based " + this.ETYPE + " damage to " + this.TILES.Count + " " + this.DESC;
        }
        else
        {
            potentialDesc = "Deals " + pDMG + " " + this.ELEMENT + " based " + this.ETYPE + " damage to " + this.DESC + " " + this.HITS + " times";

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
    public override void AugmentSkill(Augment augment)
    {
        switch (augment)
        {
            case Augment.damageAugment:
                DAMAGE = DAMAGE + 1;
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
                HITS++;
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
            case Augment.buffAugment:
                BUFFVAL *= 2;
                break;
            default:
                break;
        }
    }

    public override BoolConatainer CheckAugment()
    {
        BoolConatainer conatainer = Common.container;
        conatainer.name = "none";
        conatainer.result = false;
        if (AUGMENTS.costTrigger == 2)
        {
            // AugmentSkill(Augment.costAugment);
            conatainer.result = true;
            conatainer.name = "Damage Augment";
        }
        return conatainer;
    }

    public override void LevelUP()
    {
        base.LevelUP();
     

    }
}
