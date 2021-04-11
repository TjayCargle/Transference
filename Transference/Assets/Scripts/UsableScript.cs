using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableScript : ScriptableObject
{

    [SerializeField]
    protected string myName;

    [SerializeField]
    private string description;

    [SerializeField]
    protected int index;

    [SerializeField]
    protected int level = 1;

    [SerializeField]
    protected int exp = 10;
    public int maxExp = 10;
    protected int refType;

    [SerializeField]
    protected int useCount;

    [SerializeField]
    protected List<Augment> augments;
    [SerializeField]
    protected LivingObject owner;

    [SerializeField]
    protected string additionalDetails;

    [SerializeField]
    protected List<SkillEventContainer> specialEvents = new List<SkillEventContainer>();

    public int EXP
    {
        get { return exp; }
        set { exp = value; }
    }
    public LivingObject USER
    {
        get { return owner; }
        set { owner = value; }
    }
    public List<Augment> AUGMENTS
    {
        get { return augments; }
        set { augments = value; }
    }

    public List<SkillEventContainer> SPECIAL_EVENTS
    {
        get { return specialEvents; }
        set { specialEvents = value; }
    }

    public string DESC
    {
        get { return description; }
        set { description = value; }
    }

    public string DEATS
    {
        get { return additionalDetails; }
        set { additionalDetails = value; }
    }

    public string NAME
    {
        get { return myName; }
        set { myName = value; }
    }
    public int TYPE
    {
        get { return refType; }
        set { refType = value; }
    }
    public int INDEX
    {
        get { return index; }
        set { index = value; }
    }
    public int LEVEL
    {
        get { return level; }
        set { level = value; }
    }
    public int USECOUNT
    {
        get { return useCount; }
        set { useCount = value; }
    }
    public virtual void LevelUP()
    {
        if (LEVEL < Common.MaxSkillLevel)
        {
            LEVEL++;

        }
    }
    public virtual bool GrantXP(int amount, bool show = true)
    {

        exp -= amount;
        if (exp <= 0)
        {
            LevelUP();
            exp = 5 + (level * 3);
            maxExp = exp;
            if (USER)
            {
                ManagerScript manager = GameObject.FindObjectOfType<ManagerScript>();
                if (manager)
                {
                    if (manager.GetState() != State.EnemyTurn && manager.GetState() != State.HazardTurn)
                    {
                        if (show == true)
                        {

                            manager.CreateEvent(this, this, "New Skill Event", manager.CheckCount, null, 0, manager.CountStart);
                            manager.CreateTextEvent(this, "" + USER.FullName + "'s " + NAME + " leveled up!", "new skill event", manager.CheckText, manager.TextStart);
                            if (manager.log)
                            {
                                string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(USER.FACTION)) + ">";

                                manager.log.Log(coloroption + USER.NAME + "</color> " + NAME + " leveled up!");
                            }
                        }
                    }

                }
            }
            return true;
        }
        return false;
    }
    public virtual Reaction Activate(SkillReaction reaction, float amount, GridObject target)
    {
        switch (reaction)
        {
            case SkillReaction.healByDmg:
                USER.ChangeHealth((int)amount);
                return Reaction.none;
            case SkillReaction.healAmount:
                USER.ChangeHealth((int)amount);
                return Reaction.none;

            case SkillReaction.extraAction:
                if (USER.FACTION == Faction.ally)
                    USER.ACTIONS++;
                else
                    USER.GENERATED++;
                return Reaction.none;

            case SkillReaction.GainManaAmount:
                USER.ChangeMana((int)amount);
                return Reaction.none;

            case SkillReaction.HealFTByAmount:
                USER.ChangeFatigue((int)amount);
                return Reaction.none;

            case SkillReaction.reduceStr:
                {

                    if (target)
                    {
                        if (target.GetComponent<LivingObject>())
                        {
                            LivingObject liveTarget = target.GetComponent<LivingObject>();
                            CommandSkill randomDeBuff = ScriptableObject.CreateInstance<CommandSkill>();
                            randomDeBuff.EFFECT = SideEffect.none;
                            randomDeBuff.BUFF = BuffType.Str;
                            randomDeBuff.BUFFVAL = -50;
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
                return Reaction.ApplyEffect;

            case SkillReaction.ChargeFTByAmount:
                USER.ChangeFatigue((int)(-amount));
                return Reaction.none;

            case SkillReaction.discoverItem:
                USER.GetComponent<LivingSetup>().dm.GetItem(UnityEngine.Random.Range(0, 11), USER);
                return Reaction.none;

            case SkillReaction.GainManaByDmg:
                USER.ChangeMana((int)amount);
                break;
            case SkillReaction.ChargeFTByDmg:
                USER.ChangeFatigue((int)-amount);
                break;
            case SkillReaction.HealFTByDmg:
                USER.ChangeFatigue((int)amount);
                break;
            case SkillReaction.reduceDef:
                {

                    if (target)
                    {
                        if (target.GetComponent<LivingObject>())
                        {
                            LivingObject liveTarget = target.GetComponent<LivingObject>();
                            CommandSkill randomDeBuff = ScriptableObject.CreateInstance<CommandSkill>();
                            randomDeBuff.EFFECT = SideEffect.none;
                            randomDeBuff.BUFF = BuffType.Def;
                            randomDeBuff.BUFFVAL = -50;
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
            case SkillReaction.reduceMag:
                {
                    if (target)
                    {
                        if (target.GetComponent<LivingObject>())
                        {
                            LivingObject liveTarget = target.GetComponent<LivingObject>();
                            CommandSkill randomDeBuff = ScriptableObject.CreateInstance<CommandSkill>();
                            randomDeBuff.EFFECT = SideEffect.none;
                            randomDeBuff.BUFF = BuffType.Mag;
                            randomDeBuff.BUFFVAL = -50;
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
            case SkillReaction.reduceRes:
                {
                    if (target)
                    {
                        if (target.GetComponent<LivingObject>())
                        {
                            LivingObject liveTarget = target.GetComponent<LivingObject>();
                            CommandSkill randomDeBuff = ScriptableObject.CreateInstance<CommandSkill>();
                            randomDeBuff.EFFECT = SideEffect.none;
                            randomDeBuff.BUFF = BuffType.Res;
                            randomDeBuff.BUFFVAL = -50;
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
            case SkillReaction.reduceSpd:
                {
                    if (target)
                    {
                        if (target.GetComponent<LivingObject>())
                        {
                            LivingObject liveTarget = target.GetComponent<LivingObject>();
                            CommandSkill randomDeBuff = ScriptableObject.CreateInstance<CommandSkill>();
                            randomDeBuff.EFFECT = SideEffect.none;
                            randomDeBuff.BUFF = BuffType.Spd;
                            randomDeBuff.BUFFVAL = -50;
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

            case SkillReaction.reduceLuck:
                {
                    if (target)
                    {
                        if (target.GetComponent<LivingObject>())
                        {
                            LivingObject liveTarget = target.GetComponent<LivingObject>();
                            CommandSkill randomDeBuff = ScriptableObject.CreateInstance<CommandSkill>();
                            randomDeBuff.EFFECT = SideEffect.none;
                            randomDeBuff.BUFF = BuffType.Dex;
                            randomDeBuff.BUFFVAL = -50;
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
                if (USER)
                    Debug.Log("No reaction error usable from " + USER.FullName);
                else
                    Debug.Log("No reaction error usable from unknown");
                return Reaction.none;
                break;
        }


        return Reaction.none;
    }
    public virtual void ApplyAugment(Augment aug)
    {

    }

    public virtual void UpdateDesc()
    {

    }

    public virtual string GetDataString()
    {
        string returnString = "";

        returnString += "" + index + "," + level + "," + exp + "," + maxExp + "," + useCount;

        return returnString;
    }


}
