
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
    SkillEvent actType;

    [SerializeField]
    SkillReaction actReact;

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

    public SkillEvent ACT
    {
        get { return actType; }
        set { actType = value; }
    }

    public SkillReaction REACT
    {
        get { return actReact; }
        set { actReact = value; }
    }

    public override Reaction Activate(SkillReaction reaction, float amount, GridObject target)
    {

        switch (REACT)
        {
            case SkillReaction.healByDmg:
                OWNER.ChangeHealth((int)amount);
                return Reaction.none;
            case SkillReaction.healAmount:
                OWNER.ChangeHealth(VAL);
                return Reaction.none;
           
            case SkillReaction.extraAction:
                OWNER.ACTIONS++;
                return Reaction.none;
           
            case SkillReaction.GainManaAmount:
                OWNER.ChangeMana(VAL);
                return Reaction.none;
          
            case SkillReaction.HealFTByAmount:
                OWNER.ChangeFatigue(VAL);
                return Reaction.none;
          
            case SkillReaction.reduceStr:
                return Reaction.ApplyEffect;
            
            case SkillReaction.ChargeFTByAmount:
                OWNER.ChangeFatigue(-VAL);
                return Reaction.none;
             
            case SkillReaction.discoverItem:
                OWNER.GetComponent<LivingSetup>().dm.GetItem(UnityEngine.Random.Range(0, 11), OWNER);
                return Reaction.none;
           
            case SkillReaction.GainManaByDmg:
                OWNER.ChangeMana((int)amount);
                break;
            case SkillReaction.ChargeFTByDmg:
                OWNER.ChangeFatigue((int)-amount);
                break;
            case SkillReaction.HealFTByDmg:
                OWNER.ChangeFatigue((int)amount);
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
            case SkillReaction.debuff:
                {
                    if (target)
                    {
                        if (target.GetComponent<LivingObject>())
                        {
                            LivingObject liveTarget = target.GetComponent<LivingObject>();
                            CommandSkill randomDeBuff = ScriptableObject.CreateInstance<CommandSkill>();
                            randomDeBuff.EFFECT = SideEffect.none;
                            randomDeBuff.BUFF = (BuffType)Random.Range(1, 6);
                            randomDeBuff.BUFFVAL = Random.Range(-10, -100);
                            randomDeBuff.ELEMENT = Element.Buff;
                            randomDeBuff.SUBTYPE = SubSkillType.Debuff;
                            randomDeBuff.BUFFEDSTAT = Common.BuffToModStat(randomDeBuff.BUFF);
                            randomDeBuff.OWNER = liveTarget;
                            randomDeBuff.NAME = NAME + " " + randomDeBuff.BUFF + " debuff";
                            randomDeBuff.extra = NAME;
                            liveTarget.INVENTORY.DEBUFFS.Add(randomDeBuff);
                            DebuffScript buff = target.gameObject.AddComponent<DebuffScript>();
                            
                            buff.SKILL = randomDeBuff;
                            buff.BUFF = randomDeBuff.BUFF;
                            buff.COUNT = 1;
                        
                            liveTarget.UpdateBuffsAndDebuffs();
                            liveTarget.updateAilmentIcons();
                        }
                    }
                }
                break;
            case SkillReaction.cripple:
                {
                    if (target)
                    {
                        if (target.GetComponent<LivingObject>())
                        {
                            LivingObject liveTarget = target.GetComponent<LivingObject>();
                            if (liveTarget.PSTATUS != PrimaryStatus.guarding)
                                liveTarget.PSTATUS = PrimaryStatus.crippled;
                        }
                    }
                }
                break;
            case SkillReaction.instaKill:
                {
                    if (target)
                    {
                        if (!target.DEAD)
                        {
                            target.DEAD = true;
                            ManagerScript manager = GameObject.FindObjectOfType<ManagerScript>();
                            if (manager)
                            {
                                manager.gridObjects.Remove(target);
                            }

                            target.Die();
                        }
                    }
                }
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
        DESC = CHANCE + " % chance + Dex to ";

        switch (REACT)
        {
            case SkillReaction.healByDmg:
                DESC += "heal the damage you dealt";
                break;

            case SkillReaction.GainManaByDmg:
                DESC += "gain mana by the damage you dealt";
                break;

            case SkillReaction.ChargeFTByDmg:
                DESC += "charge FT by the damage you dealt";
                break;

            case SkillReaction.HealFTByDmg:
                DESC += "reduce FT by the damage you dealt";
                break;

            case SkillReaction.extraAction:
                DESC += "gain an additional action";
                break;

            case SkillReaction.reduceDef:
                DESC += "deal damage as if enemy def has halved";
                break;

            case SkillReaction.reduceRes:
                DESC += "deal damage as if enemy res has halved";
                break;


            case SkillReaction.discoverItem:
                DESC += "discover a random item";
                break;

            case SkillReaction.debuff:
                DESC += "apply a random debuff";
                break;

            case SkillReaction.cripple:
                DESC += "to cripple";
                break;

            case SkillReaction.instaKill:
                DESC += "instantly defeat a non boss enemy";
                break;
        }

        switch (ACT)
        {
            case SkillEvent.beforeDmg:
                DESC += " before doing ";
                break;
            case SkillEvent.afterDmg:
                DESC += " after hitting with ";
                break;

        }
        DESC += " a Strike";
    }

}
