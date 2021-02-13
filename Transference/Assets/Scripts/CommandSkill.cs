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

    //[SerializeField]
    //private List<Vector2> affecttedTiles;

    [SerializeField]
    protected RangeType rType;

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
    protected int minHit = -1;
    [SerializeField]
    protected int maxHit = -1;

    [SerializeField]
    public string extra;

    public int COST
    {
        get { return GetCost(); }
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
    //public List<Vector2> TILES
    //{
    //    get { return affecttedTiles; }
    //    set { affecttedTiles = value; }
    //}

    public float CRIT_RATE
    {
        get { return critRate; }
        set { critRate = value; }
    }

    public RangeType RTYPE
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

    public override Reaction Activate(SkillReaction reaction, float amount, GridObject target)
    {

        switch (reaction)
        {

            case SkillReaction.increaseAccurracy:
                {
                    ACCURACY += LEVEL;
                }
                break;
            case SkillReaction.decreaseAccurracy:
                {
                    ACCURACY -= LEVEL;
                }
                break;
            case SkillReaction.resetAccurracy:
                {
                    ACCURACY = 1;
                }
                break;
            case SkillReaction.increaseStr:
                {
                    if (OWNER)
                    {
                        OWNER.BASE_STATS.STRENGTH++;
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;
            case SkillReaction.healByDmg:
                break;
            case SkillReaction.healAmount:
                break;
            case SkillReaction.GainManaByDmg:
                break;
            case SkillReaction.GainManaAmount:
                break;
            case SkillReaction.ChargeFTByDmg:
                break;
            case SkillReaction.ChargeFTByAmount:
                break;
            case SkillReaction.HealFTByDmg:
                break;
            case SkillReaction.HealFTByAmount:
                break;
            case SkillReaction.extraAction:
                break;
            case SkillReaction.reduceStr:
                break;
            case SkillReaction.reduceDef:
                break;
            case SkillReaction.reduceMag:
                break;
            case SkillReaction.reduceRes:
                break;
            case SkillReaction.reduceSpd:
                break;
            case SkillReaction.reduceLuck:
                break;
            case SkillReaction.discoverItem:
                break;
            case SkillReaction.debuff:
                break;
            case SkillReaction.cripple:
                break;
            case SkillReaction.instaKill:
                break;
            case SkillReaction.poison:
                break;
            case SkillReaction.burn:
                break;
            case SkillReaction.freeze:
                break;
            case SkillReaction.confuse:
                break;
            case SkillReaction.increaseDef:
                {
                    if (OWNER)
                    {
                        OWNER.BASE_STATS.DEFENSE++;
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;
            case SkillReaction.increaseMag:
                {
                    if (OWNER)
                    {
                        OWNER.BASE_STATS.MAGIC++;
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;
            case SkillReaction.increaseRes:
                {
                    if (OWNER)
                    {
                        OWNER.BASE_STATS.RESIESTANCE++;
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;
            case SkillReaction.increaseSpd:
                {
                    if (OWNER)
                    {
                        OWNER.BASE_STATS.SPEED++;
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;
            case SkillReaction.increaseDex:
                {
                    if (OWNER)
                    {
                        OWNER.BASE_STATS.DEX++;
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;
            case SkillReaction.maxMana:
                {
                    if (OWNER)
                    {
                        OWNER.ChangeMana(OWNER.MAX_MANA);
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;
            case SkillReaction.maxHealth:
                {
                    if (OWNER)
                    {
                        OWNER.ChangeHealth(OWNER.MAX_HEALTH);
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;
            case SkillReaction.MaxFatigue:
                {
                    if (OWNER)
                    {
                        OWNER.ChangeFatigue(OWNER.MAX_FATIGUE * -1);
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;
            case SkillReaction.FatigueZero:
                {
                    if (OWNER)
                    {
                        OWNER.ChangeFatigue(OWNER.MAX_FATIGUE);
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;
            case SkillReaction.enterGuardState:
                {
                    if (OWNER)
                    {
                        OWNER.Shield();
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;
            case SkillReaction.increaseCrit:
                {
                    CRIT_RATE += LEVEL;
                }
                break;
            case SkillReaction.decreaseCrit:
                {
                    CRIT_RATE -= LEVEL;
                }
                break;
            case SkillReaction.resetCrit:
                {
                    CRIT_RATE = 0;
                }
                break;
            case SkillReaction.becomeWater:
                {
                    ELEMENT = Element.Water;
                }
                break;
            case SkillReaction.becomePyro:
                {
                    ELEMENT = Element.Pyro;
                }
                break;
            case SkillReaction.becomeIce:
                {
                    ELEMENT = Element.Ice;
                }
                break;
            case SkillReaction.becomeElec:
                {
                    ELEMENT = Element.Electric;
                }
                break;
            case SkillReaction.becomeSlash:
                {
                    ELEMENT = Element.Slash;
                }
                break;
            case SkillReaction.becomePierce:
                {
                    ELEMENT = Element.Pierce;
                }
                break;
            case SkillReaction.becomeBlunt:
                {
                    ELEMENT = Element.Blunt;
                }
                break;
            case SkillReaction.becomeForce:
                {
                    ELEMENT = Element.Force;
                }
                break;
            case SkillReaction.gainBecomeEvent:
                {

                }
                break;
            case SkillReaction.removeEvent:
                {
                    if(SPECIAL_EVENTS.Count > 0)
                    {
                        SPECIAL_EVENTS.Remove(SPECIAL_EVENTS[0]);
                    }
                }
                break;
            default:
                break;
        }
        UpdateDesc();
        return Reaction.none;
    }

    public int GetCost(LivingObject user = null, float modification = 1.0f)
    {
        if (user == null)
            user = USER;
        if (ETYPE == EType.physical)
        {
            switch (SUBTYPE)
            {
                case SubSkillType.Charge:
                    {
                        if (OWNER)
                            return Mathf.Max(0,(cost - OWNER.PHYSLEVEL));
                        else
                            return (int)(cost * modification);
                    }
                    break;
                case SubSkillType.Cost:
                    {
                        if (OWNER)
                            return Mathf.Min(0, (cost + (OWNER.PHYSLEVEL*2)));
                        else
                            return (int)(cost * modification);
                    }
                    break;
                case SubSkillType.RngAtk:
                    {
                        if (cost < 0)
                        {
                            if (OWNER)
                                return Mathf.Min(0, (cost + (OWNER.PHYSLEVEL * 2)));

                            else
                                return (int)(cost * modification);
                        }
                        else
                        {
                            if (OWNER)
                                return Mathf.Max(0, (cost - OWNER.PHYSLEVEL));
                            else
                                return (int)(cost * modification);
                        }
                    }
                    break;
                default:
                    return cost;
            }
        }
        else
        {
            if (OWNER)
                return Mathf.Max(0, (cost - OWNER.MAGLEVEL)); 
            else
                return (int)(cost * modification);
        }
    }
    public SkillScript UseSkill(LivingObject user, float modification = 1.0f)
    {
        if (OWNER)
        {
            if (ETYPE == EType.magical)
            {
                OWNER.STATS.MANA -= GetCost(user, modification);
                useCount++;
            }
            else
            {

                switch (SUBTYPE)
                {
                    case SubSkillType.Charge:
                        {
                            OWNER.STATS.FATIGUE += GetCost(user, modification);
                        }
                        break;
                    case SubSkillType.Cost:
                        {
                            int amt = GetCost();
                            // amt *= -1;
                            OWNER.STATS.FATIGUE += amt;
                        }
                        break;
                    case SubSkillType.RngAtk:
                        {

                            int amt = GetCost(user, modification);
                            if (cost < 0)
                            {
                                //amt *= -1;
                            }
                            OWNER.STATS.FATIGUE += amt;

                        }
                        break;
                }
                useCount++;
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
                switch (SUBTYPE)
                {
                    case SubSkillType.Charge:
                        {
                            amt = GetCost();//owner.FATIGUE + (int)((((float)(COST * 10) / 100.0f) * OWNER.MAX_FATIGUE)  * modification);
                            if (owner.FATIGUE + amt <= owner.MAX_FATIGUE)
                            {
                                if (amt >= 0)
                                {

                                    can = true;
                                }
                            }
                        }
                        break;
                    case SubSkillType.Cost:
                        {
                            amt = GetCost();// (int)((((float)(COST * 10) / 100.0f) * OWNER.MAX_FATIGUE) * OWNER.STATS.FTCOSTCHANGE);
                            amt *= -1;

                            //  if (amt <= owner.FATIGUE)
                            {
                                if (owner.FATIGUE >= amt)
                                {

                                    can = true;
                                }
                            }
                        }
                        break;

                    case SubSkillType.RngAtk:
                        {
                            if (cost < 0)
                            {
                                amt = GetCost();// (int)((((float)(COST * 10) / 100.0f) * OWNER.MAX_FATIGUE) * OWNER.STATS.FTCOSTCHANGE);
                                amt *= -1;
                                if (owner.FATIGUE >= amt)
                                {

                                    can = true;
                                }
                            }
                            else
                            {
                                amt = GetCost();// owner.FATIGUE + (int)(COST * modification);
                                if (owner.FATIGUE + amt <= owner.MAX_FATIGUE)
                                {
                                    if (amt >= 0)
                                    {

                                        can = true;
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        return true;
                }

                break;
            case EType.magical:
                amt = GetCost();//owner.MANA - (int)(COST * modification);

                if (OWNER.MANA >= amt)
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
    public string getEffectDesc()
    {
        string potentialDesc = "";
        if (SUBTYPE == SubSkillType.Buff)
        {
            potentialDesc = "" + BUFF + " is increased by " + BUFFVAL + "% due to the effects of " + extra;
        }
        else if (SUBTYPE == SubSkillType.Debuff)
        {
            potentialDesc = "" + BUFF + " is decreased by " + BUFFVAL + "% due to the effects of " + extra;
        }

        return potentialDesc;
    }
    public string PossibleDesc(RangeType pRtype, DMG pDMG, int pHits, SideEffect pEffect)
    {
        string potentialDesc;
        if (SUBTYPE == SubSkillType.Buff)
        {
            potentialDesc = "Increases " + BUFFEDSTAT + " of yourself or ally by " + BUFFVAL + "% for 3 turns." + "";
        }
        else if (SUBTYPE == SubSkillType.Debuff)
        {
            potentialDesc = "Decreases " + BUFFEDSTAT + " of target enemy by " + BUFFVAL + "% for 3 turns. Costs " + "";
        }
        else if (SUBTYPE == SubSkillType.Ailment)
        {
            potentialDesc = ACCURACY + "% chance to inflict enemy with " + EFFECT + "";
        }
        else if (ELEMENT == Element.Support)
        {
            switch (EFFECT)
            {
                case SideEffect.heal:
                    {
                        potentialDesc = "Heals " + DAMAGE + " amount of health to target. Costs " + ((cost * 10)) + "% Mana";
                    }
                    break;
                default:
                    potentialDesc = "u messed up ";
                    break;
            }

        }
        else
        {
            potentialDesc = ACCURACY + "% chance to deal " + pDMG + " " + this.ETYPE + " " + this.ELEMENT + " damage";
        }

        //switch (RTYPE)
        //{
        //    case RangeType.single:
        //        if(TILES.Count > 1)
        //        {
        //            potentialDesc += " targets in range ";
        //        }
        //        else
        //        {
        //            potentialDesc += " target ";
        //        }
        //        break;
        //    case RangeType.multi:
        //        potentialDesc += " target ";
        //        break;
        //    case RangeType.area:
        //        potentialDesc += " everyone in range ";
        //        break;
        //    case RangeType.any:
        //        potentialDesc += " yourself or target ";
        //        break;
        //    case RangeType.anyarea:
        //        potentialDesc += " everyone including yourself in range ";
        //        break;
        //    case RangeType.multiarea:
        //        potentialDesc += " everyone in selected area ";
        //        break;
        //    default:
        //        break;
        //}

        if (SUBTYPE == SubSkillType.RngAtk)
        {
            potentialDesc += " " + MIN_HIT + "-" + MAX_HIT + " times ";
        }
        else if (HITS > 1)
        {
            potentialDesc += " " + HITS + " times ";

        }

        if (pEffect != SideEffect.none)
        {
            if (this.EFFECT < SideEffect.reduceStr)
            {
                potentialDesc += " with a chance of " + this.EFFECT.ToString();
            }
            else if (EFFECT < SideEffect.debuff)
            {
                potentialDesc += " with a chance to debuff " + Common.GetSideEffectText(this.EFFECT);
            }
            else
            {
                switch (EFFECT)
                {

                    case SideEffect.heal:
                        break;
                    case SideEffect.swap:
                        break;
                    case SideEffect.barrier:
                        break;
                }
            }
        }
        if (CRIT_RATE > 0)
        {
            potentialDesc += " and " + CRIT_RATE + "% chance to land a critical hit.";
        }
        else
        {
            potentialDesc += ". ";
        }
        if (ETYPE == EType.physical)
        {
            if (SUBTYPE == SubSkillType.Charge)
            {
                potentialDesc += " Must Charge Fatigue by " +(cost * -1) + ".";
            }
            else
            {
                potentialDesc += " Costs " + ((cost)) + " Fatigue.";
            }
        }
        else
        {
            potentialDesc += "Costs " + ((cost)) + " Mana.";
        }

        if (SPECIAL_EVENTS.Count > 0)
        {

            for (int i = 0; i < SPECIAL_EVENTS.Count; i++)
            {
                SkillEventContainer sec = SPECIAL_EVENTS[i];
                potentialDesc += Common.GetSkillEventText(sec.theEvent, sec.theReaction) + " ";


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
                for (int i = 0; i < 2; i++)
                {
                    LevelUP();
                }
                break;
            case Augment.accurracyAugment:
                ACCURACY = (int)((float)ACCURACY * 1.2f);
                if (ACCURACY > 100)
                    ACCURACY = 100;
                break;
            case Augment.sideEffectAugment:
                EFFECT = effect;
                break;
            case Augment.rangeAugment:
                //todo randomly change range
                RTYPE = (RangeType)((int)(Random.Range((int)RangeType.adjacent, (int)RangeType.crosshair)));
                break;
            case Augment.attackCountAugment:
                if (SUBTYPE == SubSkillType.RngAtk)
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
                cost = (int)((float)cost * 0.5f);
                break;
            case Augment.chargeIncreaseAugment:
                cost = (int)((float)cost * 1.5f);
                break;
            case Augment.chargeDecreaseAugment:
                cost = (int)((float)cost * 0.5f);
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
        // base.LevelUP();

        if (level > 0 && level < Common.MaxSkillLevel)
        {
            LEVEL++;

            switch (ELEMENT)
            {
                case Element.Water:
                    DAMAGE = Common.GetNextDmg(DAMAGE);
                    if (ACCURACY < 100)
                    {
                        ACCURACY += 5;
                    }
                    else
                    {
                        if (SUBTYPE == SubSkillType.None)
                        {
                            SUBTYPE = SubSkillType.RngAtk;
                        }
                        MIN_HIT = HITS;
                        MAX_HIT = HITS + 1;
                    }
                    if (ACCURACY > 100)
                    {
                        ACCURACY = 100;
                    }
                    break;
                case Element.Pyro:
                    DAMAGE = Common.GetNextDmg(DAMAGE);
                    //TILES.Add(Common.GetNextTileRange(TILES));
                    if (ACCURACY < 100)
                    {
                        ACCURACY += 5;
                    }
                    else
                    {
                        if (EFFECT == SideEffect.none)
                        {
                            EFFECT = SideEffect.burn;
                        }
                        else
                        {
                            HITS++;
                        }

                    }
                    if (ACCURACY > 100)
                    {
                        ACCURACY = 100;
                    }
                    break;
                case Element.Ice:
                    DAMAGE = Common.GetNextDmg(DAMAGE);
                    CRIT_RATE += 3.0f;
                    if (ACCURACY < 100)
                    {
                        ACCURACY += 5;
                    }
                    else
                    {
                        if (EFFECT == SideEffect.none)
                        {
                            EFFECT = SideEffect.freeze;
                        }
                        else
                        {

                            CRIT_RATE += 3.0f;
                        }

                    }
                    if (ACCURACY > 100)
                    {
                        ACCURACY = 100;
                    }
                    break;
                case Element.Electric:
                    if (level % 2 == 0)
                        DAMAGE = Common.GetNextDmg(DAMAGE);
                    MAX_HIT++;
                    if (level == 3)
                    {
                        if (EFFECT == SideEffect.none)
                        {
                            EFFECT = SideEffect.paralyze;
                        }
                    }
                    break;
                case Element.Slash:
                    if (level % 2 == 0)
                        DAMAGE = Common.GetNextDmg(DAMAGE);
                    HITS++;
                    break;
                case Element.Pierce:
                    DAMAGE = Common.GetNextDmg(DAMAGE);
                    ACCURACY += 2;
                    if (ACCURACY > 100)
                        ACCURACY = 100;
                    CRIT_RATE += 2.0f;
                    if (level % 2 == 0)
                    {
                        HITS++;
                    }
                    break;
                case Element.Blunt:
                    DAMAGE = Common.GetNextDmg(DAMAGE);
                    ACCURACY += 5;
                    if (ACCURACY > 100)
                        ACCURACY = 100;
                    CRIT_RATE += 5.0f;
                    break;
                case Element.Buff:
                    if (SUBTYPE == SubSkillType.Buff)
                        BUFFVAL += 15.0f;
                    else if (SUBTYPE == SubSkillType.Debuff)
                        BUFFVAL -= 15.0f;

                    break;
                case Element.Force:
                    DAMAGE = Common.GetNextDmg(DAMAGE);
                    //TILES.Add(Common.GetNextTileRange(TILES));
                    if (ACCURACY < 100)
                    {
                        ACCURACY += 5;
                    }
                    else
                    {
                        if (EFFECT == SideEffect.none)
                        {
                            EFFECT = SideEffect.pullin;
                        }
                        else
                        {
                            HITS++;
                        }

                    }
                    if (ACCURACY > 100)
                    {
                        ACCURACY = 100;
                    }
                    break;

                    break;
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
            returnedString += "\n Element: " + ELEMENT;
            returnedString += "\n Damage: " + DAMAGE.ToString() + "";
            returnedString += "\n Accuracy: " + (ACCURACY) + "";
        }
        else
        {
            returnedString = "Max Level ";
            returnedString += "\n Element: " + ELEMENT;
            returnedString += "\n Damage: " + DAMAGE.ToString() + "";
            returnedString += "\n Accuracy: " + (ACCURACY) + "";
        }

        if (level > 0)
        {

            switch (ELEMENT)
            {
                case Element.Water:

                    if (MAX_HIT == 0)
                    {
                        returnedString += "\n Hits: " + HITS + " time(s)";
                    }
                    else
                    {

                        returnedString += "\n Hits: " + HITS + "-" + (HITS) + " times";
                    }

                    break;
                case Element.Pyro:

                    returnedString += "\n Hits: " + HITS + " time(s)";

                    if (EFFECT == SideEffect.burn)
                    {
                        returnedString += "\n Chance of burn";


                    }

                    break;
                case Element.Ice:

                    returnedString += "\n Hits: " + HITS + " time(s)";
                    if (CRIT_RATE > 0)
                        returnedString += "\n Chance of critical hit: " + (CRIT_RATE) + "%";
                    if (EFFECT == SideEffect.freeze)
                    {
                        returnedString += "\n Chance of freeze";
                    }


                    break;
                case Element.Electric:

                    returnedString += "\n Hits: " + MIN_HIT + "-" + (MAX_HIT) + " times";
                    if (EFFECT == SideEffect.paralyze)
                    {
                        returnedString += "\n Chance to paralyze";
                    }
                    break;
                case Element.Slash:

                    returnedString += "\n Hits: " + (HITS) + " time(s)";

                    break;
                case Element.Pierce:

                    returnedString += "\n Hits: " + (HITS) + " time(s)";
                    if (CRIT_RATE > 0)
                        returnedString += "\n Chance of critical hit: " + (CRIT_RATE) + "%";
                    break;
                case Element.Blunt:

                    returnedString += "\n Hits: " + (HITS) + " time(s)";
                    if (CRIT_RATE > 0)
                        returnedString += "\n Chance of critical hit: " + (CRIT_RATE) + "%";

                    break;
                case Element.Buff:
                    if (SUBTYPE == SubSkillType.Buff)
                        returnedString += "\n " + BUFFEDSTAT + " +" + (BUFFVAL) + "% for 3 turns";
                    else if (SUBTYPE == SubSkillType.Debuff)
                        returnedString += "\n " + BUFFEDSTAT + " " + (BUFFVAL) + "% for 3 turns";
                    break;
                case Element.Force:

                    returnedString += "\n Hits: " + HITS + " time(s)";
                    if (CRIT_RATE > 0)
                        returnedString += "\n Chance of critical hit: " + (CRIT_RATE) + "%";
                    if (EFFECT == SideEffect.pullin)
                    {
                        returnedString += "\n Pulls target 1 tile";
                    }

                    break;

            }

        }

        if (level < Common.MaxSkillLevel)
        {
       
            returnedString += "\n " + (exp) + " uses to level up";
            
        }
     

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

        if (level > 0)
        {

            switch (ELEMENT)
            {
                case Element.Water:
                    {

                        if (DAMAGE != DMG.colossal)
                        {
                            returnedString += "\n <color=green>Damage: " + Common.GetNextDmg(DAMAGE).ToString() + "</color>";
                        }
                        else
                        {
                            returnedString += "\n Damage: " + Common.GetNextDmg(DAMAGE).ToString() + "";
                        }

                        if (ACCURACY + 5 <= 100)
                        {
                            returnedString += "\n <color=green>Accuracy: " + (ACCURACY + 5) + "</color>";
                            returnedString += "\n Hits: " + HITS + " time(s)";
                        }
                        else
                        {
                            returnedString += "\n Accuracy: 100";
                            returnedString += "\n <color=green>Hits: " + HITS + "-" + (HITS + 1) + " times</color>";
                        }

                    }
                    break;
                case Element.Pyro:
                    {

                        if (DAMAGE != DMG.colossal)
                        {
                            returnedString += "\n <color=green>Damage: " + Common.GetNextDmg(DAMAGE).ToString() + "</color>";
                        }
                        else
                        {
                            returnedString += "\n Damage: " + Common.GetNextDmg(DAMAGE).ToString() + "";
                        }
                        if (ACCURACY + 5 <= 100)
                        {
                            returnedString += "\n <color=green>Accuracy: " + (ACCURACY + 5) + "</color>";
                            returnedString += "\n Hits: " + HITS + " time(s)";
                        }
                        else
                        {
                            returnedString += "\n Accuracy: " + (ACCURACY) + "";
                            if (EFFECT == SideEffect.none)
                            {
                                returnedString += "\n Hits: " + HITS + " time(s)";
                                returnedString += "\n <color=green>Chance of burn</color>";
                            }
                            else
                            {
                                returnedString += "\n <color=green>Hits: " + (HITS + 1) + " time(s)</color>";
                                returnedString += "\n Chance of burn";
                            }

                        }

                    }
                    break;
                case Element.Ice:
                    {

                        if (DAMAGE != DMG.colossal)
                        {
                            returnedString += "\n <color=green>Damage: " + Common.GetNextDmg(DAMAGE).ToString() + "</color>";
                        }
                        else
                        {
                            returnedString += "\n Damage: " + Common.GetNextDmg(DAMAGE).ToString() + "";
                        }
                        if (ACCURACY + 5 <= 100)
                        {
                            returnedString += "\n <color=green>Accuracy: " + (ACCURACY + 5) + "</color>";
                            returnedString += "\n Hits: " + HITS + " time(s)";
                            returnedString += "\n <color=green>Chance of critical hit: " + (CRIT_RATE + 3.0f) + "%</color>";
                        }
                        else
                        {
                            returnedString += "\n Accuracy: " + (ACCURACY) + "";

                            if (EFFECT == SideEffect.none)
                            {
                                returnedString += "\n Hits: " + HITS + " time(s)";
                                returnedString += "\n <color=green>Chance of freeze</color>";
                            }
                            else
                            {
                                returnedString += "\n Hits: " + HITS + " time(s)";
                                returnedString += "\n <color=green>Chance of critical hit: " + (CRIT_RATE + 6.0f) + "%</color>";
                                returnedString += "\n Chance of freeze";
                            }
                        }

                    }
                    break;
                case Element.Electric:
                    {

                        if (level % 2 == 0)
                        {
                            if (DAMAGE != DMG.colossal)
                            {
                                returnedString += "\n <color=green>Damage: " + Common.GetNextDmg(DAMAGE).ToString() + "</color>";
                            }
                            else
                            {
                                returnedString += "\n Damage: " + DAMAGE.ToString() + "";
                            }
                        }
                        else
                        {
                            returnedString += "\n Damage: " + DAMAGE.ToString() + "";
                        }

                        returnedString += "\n Accuracy: " + (ACCURACY) + "";
                        returnedString += "\n <color=green>Hits: " + MIN_HIT + "-" + (MAX_HIT + 1) + " times</color>";
                        if (level + 1 == 3)
                        {
                            if (EFFECT == SideEffect.none)
                            {
                                returnedString += "\n <color=green>Chance to paralyze</color>";
                            }
                        }
                        else if (level + 1 > 3)
                        {
                            returnedString += "\n Chance to paralyze";
                        }
                    }
                    break;
                case Element.Slash:
                    {

                        if ((level + 1) % 2 == 0)
                        {
                            if (DAMAGE != DMG.colossal)
                            {
                                returnedString += "\n <color=green>Damage: " + Common.GetNextDmg(DAMAGE).ToString() + "</color>";
                            }
                            else
                            {
                                returnedString += "\n Damage: " + Common.GetNextDmg(DAMAGE).ToString() + "";
                            }
                        }
                        else
                        {
                            returnedString += "\n Damage: " + DAMAGE.ToString() + "";
                        }
                        returnedString += "\n Accuracy: " + (ACCURACY) + "";
                        returnedString += "\n <color=green>Hits: " + (HITS + 1) + " time(s)</color>";

                    }
                    break;
                case Element.Pierce:
                    {

                        if (DAMAGE != DMG.colossal)
                        {
                            returnedString += "\n <color=green>Damage: " + Common.GetNextDmg(DAMAGE).ToString() + "</color>";
                        }
                        else
                        {
                            returnedString += "\n Damage: " + Common.GetNextDmg(DAMAGE).ToString() + "";
                        }

                        if (ACCURACY + 2 <= 100)
                        {
                            returnedString += "\n <color=green>Accuracy: " + (ACCURACY + 5) + "</color>";
                        }
                        else
                        {
                            returnedString += "\n Accuracy: " + (ACCURACY) + "";
                        }

                        if ((level + 1) % 2 == 0)
                        {
                            returnedString += "\n <color=green>Hits: " + (HITS + 1) + " time(s)</color>";
                        }
                        else
                        {
                            returnedString += "\n Hits: " + HITS + " time(s)";
                        }
                        returnedString += "\n <color=green>Chance of critical hit: " + (CRIT_RATE + 2.0f) + "%</color>";
                    }
                    break;
                case Element.Blunt:
                    {

                        if (DAMAGE != DMG.colossal)
                        {
                            returnedString += "\n <color=green>Damage: " + Common.GetNextDmg(DAMAGE).ToString() + "</color>";
                        }
                        else
                        {
                            returnedString += "\n Damage: " + Common.GetNextDmg(DAMAGE).ToString() + "";
                        }

                        if (ACCURACY + 5 <= 100)
                        {
                            returnedString += "\n <color=green>Accuracy: " + (ACCURACY + 5) + "</color>";
                            returnedString += "\n Hits: " + HITS + " time(s)";
                        }
                        else
                        {
                            returnedString += "\n Accuracy: " + (ACCURACY) + "";
                            returnedString += "\n Hits: " + HITS + " time(s)";
                        }
                        returnedString += "\n <color=green>Chance of critical hit: " + (CRIT_RATE + 5.0f) + "%</color>";
                        ;
                    }
                    break;
                case Element.Buff:
                    if (SUBTYPE == SubSkillType.Buff)
                        returnedString += "\n <color=green> " + BUFFEDSTAT + " +" + (BUFFVAL + 15.0f) + "% for 3 turns</color>";
                    else if (SUBTYPE == SubSkillType.Debuff)
                        returnedString += "\n <color=green> " + BUFFEDSTAT + " " + (BUFFVAL - 15.0f) + "% for 3 turns</color>";
                    break;

                case Element.Force:
                    {

                        if (DAMAGE != DMG.colossal)
                        {
                            returnedString += "\n <color=green>Damage: " + Common.GetNextDmg(DAMAGE).ToString() + "</color>";
                        }
                        else
                        {
                            returnedString += "\n Damage: " + Common.GetNextDmg(DAMAGE).ToString() + "";
                        }
                        if (ACCURACY + 5 <= 100)
                        {
                            returnedString += "\n <color=green>Accuracy: " + (ACCURACY + 5) + "</color>";
                            returnedString += "\n Hits: " + HITS + " time(s)";
                        }
                        else
                        {
                            returnedString += "\n Accuracy: " + (ACCURACY) + "";
                            if (EFFECT == SideEffect.none)
                            {
                                returnedString += "\n Hits: " + HITS + " time(s)";
                                returnedString += "\n <color=green>Pulls target</color>";
                            }
                            else
                            {
                                returnedString += "\n <color=green>Hits: " + (HITS + 1) + " time(s)</color>";
                                returnedString += "\n Pulls target 1 tile";
                            }

                        }
                    }
                    break;
            }

        }
        return returnedString;
    }

    public override string GetDataString()
    {
        string dataString = base.GetDataString();

        dataString += "," + ELEMENT + "," + cost + "," + accuraccy + "," + effect + "," + damage + "," + rType + "," + critRate + "," + buff + "," + modVal + "," + buffedStat + "," + hitCount + "," + eType + "," + reaction + "," + minHit + "," + maxHit;

        return dataString;
    }

}
