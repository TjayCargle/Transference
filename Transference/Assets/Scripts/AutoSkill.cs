
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

    public Reaction Activate(float amount, GridObject target)
    {

        switch (REACT)
        {
            case AutoReact.healByDmg:
                OWNER.ChangeHealth((int)amount);
                return Reaction.none;
            case AutoReact.healAmount:
                OWNER.ChangeHealth(VAL);
                return Reaction.none;
           
            case AutoReact.extraAction:
                OWNER.ACTIONS++;
                return Reaction.none;
           
            case AutoReact.GainManaAmount:
                OWNER.ChangeMana(VAL);
                return Reaction.none;
          
            case AutoReact.HealFTByAmount:
                OWNER.ChangeFatigue(VAL);
                return Reaction.none;
          
            case AutoReact.reduceStr:
                return Reaction.ApplyEffect;
            
            case AutoReact.ChargeFTByAmount:
                OWNER.ChangeFatigue(-VAL);
                return Reaction.none;
             
            case AutoReact.discoverItem:
                OWNER.GetComponent<LivingSetup>().dm.GetItem(UnityEngine.Random.Range(0, 11), OWNER);
                return Reaction.none;
           
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
            case AutoReact.debuff:
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
                        
                            liveTarget.ApplyPassives();
                            liveTarget.updateAilmentIcons();
                        }
                    }
                }
                break;
            case AutoReact.cripple:
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
            case AutoReact.instaKill:
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

            case AutoReact.debuff:
                DESC += "apply a random debuff";
                break;

            case AutoReact.cripple:
                DESC += "to cripple";
                break;

            case AutoReact.instaKill:
                DESC += "instantly defeat a non boss enemy";
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
        DESC += " a Strike";
    }

}
