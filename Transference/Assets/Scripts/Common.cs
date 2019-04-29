using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State
{
    PlayerInput,
    PlayerMove,
    PlayerAttacking,
    PlayerEquippingMenu,
    PlayerEquipping,
    playerUsingSkills,
    PlayerEquippingSkills,
    PlayerSkillsMenu,
    PlayerWait,
    FreeCamera,
    PlayerSelectItem,
    PlayerOppSelecting,
    PlayerOppOptions,
    PlayerOppMove,
    ChangeOptions,
    PlayerTransition,
    CheckDetails,
    AquireNewSkill,
    GotoNewRoom,
    EnemyTurn,
    HazardTurn,
    ShopCanvas,
    PlayerUsingItems


}
public enum Facing
{
    North,
    East,
    South,
    West,
}
public enum EHitType
{
    absorbs,
    nulls,
    reflects,
    resists,
    normal,

    weak, //dmg x2
    savage, // dmg x2 + lose a turn
    cripples, // dmg x4 + stats halved
    leathal // dmg x6 + lose a turn + stats halved

}
public enum Resists
{
    absorbs,
    nulls,
    reflects,
    resists,
}
public enum Weaks
{
    weak,
    savage,
    cripples,
    leathal
}
public enum ModifiedStat
{
    none,
    Health,
    SP,
    FT,
    FTCost,
    FTCharge,
    SPCost,
    ElementDmg,
    Movement,
    Str,
    Mag,
    Atk,
    Def,
    Res,
    Guard,
    Speed,
    Skill,
    dmg,
    all,
    ElementBody

}
public enum RangeType
{
    single,
    multi,
    area,
    any,
    anyarea,
    multiarea,
    adjacent,
    pinWheel,
    detached,
    stretched,
    rotator,
    fan,
    spear,
    lance,
    line,
    rect,
    cone,
    tpose,
    clover,
    cross,
    square,
    box,
    diamond,
    crosshair
}

public enum TargetType
{

    ally,
    enemy,
    range,
    adjecent
}

public enum BuffType
{
    none,
    str,
    mag,
    defense,
    resistance,
    speed,
    skill,
    attack,
    guard,
    act,
    all


}
public enum SkillType
{
    Command,
    Passive,
    Auto,
    Opp,
    None
}

public enum SubSkillType
{
    Charge,
    Cost,
    Magic,
    Buff,
    Debuff,
    Ailment,
    Heal,
    RngAtk,
    RngSupp,
    None
}
public enum Augment
{
    none,
    levelAugment,
    accurracyAugment,
    sideEffectAugment,
    rangeAument,
    attackCountAugment,
    costAugment,
    chargeIncreaseAugment,
    chargeDecreaseAugment,
    elementAugment,
    effectAugment1,
    effectAugment2,
    effectAugment3,
    strAugment,
    magAugment,
    sklAugment,
    defAugment,
    resAugment,
    spdAugment,
    end
}
public enum Element
{
    Water,
    Fire,
    Ice,
    Electric,

    Slash,
    Pierce,
    Blunt,
    Buff,
    Support,
    Ailment,
    Passive,
    Opp,
    Auto,
    none

}
public enum DetailType
{
    Command,
    Passive,
    Auto,
    Opportunity,
    BasicAtk,
    Armor,
    Buffs,
    Debuffs,
    Effects,
    Exp
}
public enum ShopWindow
{
    none,
    main,
    learn,
    alter,
    forget,
    augmenting,
    removingItem,
    buying,
    confirm,
}
public enum EType
{
    physical,
    magical,
}
public enum Reaction
{
    none,
    AilmentOnly,
    buff,
    debuff,
    bonusAction,
    knockback,
    snatched,
    ApplyEffect,
    turnloss,
    cripple,
    turnAndcrip,
    weak,
    nulled,
    reflected,
    absorb,
    missed,
    Heal,
    reduceDef,
    reduceRes
}
public enum DMG
{
    tiny = 10,
    small = 20,
    medium = 40,
    heavy = 80,
    severe = 160,
    collassal = 320
}
public enum ItemType
{
    healthPotion,
    manaPotion,
    fatiguePotion,
    cure,
    buff,
    dmg,
    actionBoost,
    random,
    summon
}

public enum IconSet
{
    physical,
    magical,
    health,
    ward,
    weapon,
    command,
    passive,
    auto,
    opp,
    lv,
    shop,
    resists,
    nulls,
    absorbs,
    reflects,
    weak,
    savage,
    cripple,
    leathal,
    item
}

public enum WepSkillType
{
    none,
    boostDmg,
    lifesteal,
    weaken,
    instaKill,
    bonusAction,
    chngHits,
    ailment,


}
public enum MenuItemType
{
    Move = 0,
    Attack,
    chooseSkill,
    Equip,
    Wait,
    Look,
    InventoryWeapon,
    InventoryArmor,
    InventoryAcc,
    equipBS,
    selectBS,
    equipAS,
    equipPS,
    special,
    equipOS,
    generated,
    selectItem,
    trade,
    prevMenu,
    selectAct,
    selectDetails,
    shop,
    door
}
public enum AutoAct
{
    beforeDmg,
    afterDmg,
    afterKilling,
    turnBegin,
    afterSkill,
    turnEnd

}

public enum AutoReact
{
    healByDmg,
    healAmount,
    GainManaByDmg,
    GainManaAmount,
    ChargeFTByDmg,
    ChargeFTByAmount,
    HealFTByDmg,
    HealFTByAmount,
    extraAction,
    reduceStr,
    reduceDef,
    reduceMag,
    reduceRes,
    reduceSpd,
    reduceLuck,
    discoverItem

}

public enum PrimaryStatus
{
    normal,
    crippled,
    great,
    tired,
    knockedOut,
    dead
}
//remove seconf
public enum SecondaryStatus
{
    normal,
    slow,
    rage,
    charm,
    seal,
    confusion
}

public enum StatusEffect
{
    none,
    paralyzed,
    sleep,
    frozen,
    burned,
    poisoned,
    bleeding,
    confused

}
public enum SideEffect
{
    none,
    slow,
    rage,
    charm,
    seal,
    poison,
    confusion,
    paralyze,
    sleep,
    freeze,
    burn,
    bleed,
    reduceStr,
    reduceDef,
    reduceMag,
    reduceRes,
    reduceSpd,
    reduceSkill,
    reduceAtk,
    reduceGuard,
    reduceAct,
    debuff,
    heal,
    barrier

}

public enum StatusIcon
{
    AtkUP,
    DefUP,
    SpdUp,
    MagUp,
    ResUp,
    SklUp,

    AtkDown,
    DefDown,
    SpdDown,
    MagDown,
    ResDown,
    SklDown,

    Poison,
    Burn,
    Sleep,
    Paralyze,
    Freeze,
    Bleed,
    Confuse,

    Crippled
}

public enum TileType
{
    regular = 0,
    door,
    shop,
    unit,
    special
}

public class MassAtkConatiner : ScriptableObject
{
    public List<AtkConatiner> atkConatiners;
}
public class AtkConatiner : ScriptableObject
{
    public LivingObject attackingObject;
    public GridObject dmgObject;
    public Element attackingElement;
    public EType attackType;
    public int dmg;
    public Reaction alteration;
    public CommandSkill command;
    public DmgReaction react;


    public void Inherit (AtkConatiner container)
    {
        this.attackingObject = container.attackingObject;
        this.dmgObject = container.dmgObject;
        this.attackingElement = container.attackingElement;
        this.attackType = container.attackType;
        this.dmg = container.dmg;
        this.alteration = container.alteration;
        this.command = container.command;
        this.react = container.react;

    }
}
public class LearnContainer : ScriptableObject
{
    public LivingObject attackingObject;
    public UsableScript usable;

}
public struct DmgReaction
{
    public int damage;
    public Reaction reaction;
    public string atkName;
    public Element dmgElement;
    public SkillScript usedSkill;
}
public struct Modification
{
    public ModifiedStat affectedStat;
    public Element affectedElement;
    public float editValue;
}
public struct BoolConatainer
{
    public bool result;
    public string name;
}
public enum currentMenu
{
    command,
    invMain,
    skillsMain,
    CmdSkills,
    OppSelection,
    OppOptions,
    OppMove,
    PlayerOptions,
    act,
    CmdItems,
    none
}
public enum Faction
{
    ally,
    enemy,
    hazard,
    other
}
public class tjCompare : ScriptableObject, IComparer<pathNode>
{

    public int Compare(pathNode x, pathNode y)
    {
        return x.gcost.CompareTo(y.gcost) + x.hcost.CompareTo(y.hcost);
    }
}
public class pathNode : ScriptableObject
{
    public TileScript tile;
    public int gcost;
    public int hcost;

}

public class path : ScriptableObject
{
    public Queue<TileScript> currentPath;
    public TileScript realTarget;

}
public struct menuStackEntry
{
    public State state;
    public int index;
    public currentMenu menu;
}

public struct MapDetail
{
    public string mapName;
    public int width;
    public int height;
    public List<int> doorIndexes;
    public List<string> roomNames;
    public List<int> roomIndexes;
    public List<int> startIndexes;
    public List<int> enemyIndexes;
    public List<int> hazardIndexes;
    public List<int> shopIndexes;
    public List<int> objIndexes;
    public Texture texture;
    public int StartingPosition;
}
public enum descState
{
    stats,
    skills,
    equipped,
    mag_affinities,
    phys_affinities,
    status,
    none
}
public struct Debuff
{
    ModifiedStat stat;
    float val;
}
public delegate bool RunableEvent(Object data);
public delegate void StartupEvent();
public delegate void StartupWResourcesEvent(Object data);

public struct GridEvent
{
    public string name;
    public Object caller;
    public Object data;
    public bool isRunning;
    RunableEvent runable;
    StartupEvent start;
    StartupWResourcesEvent startw;
    public RunableEvent RUNABLE
    {
        get { return runable; }

        set { runable = value; }
    }

    public StartupEvent START
    {
        get { return start; }

        set { start = value; }
    }


    public StartupWResourcesEvent STARTW
    {
        get { return startw; }

        set { startw = value; }
    }
}

public struct TextEvent
{
    public string name;
    public Object caller;
    public string data;
    public bool isRunning;
    RunableEvent runable;
    StartupEvent start;
    StartupWResourcesEvent startw;
    public RunableEvent RUNABLE
    {
        get { return runable; }

        set { runable = value; }
    }

    public StartupEvent START
    {
        get { return start; }

        set { start = value; }
    }

}
public enum STATICVAR
{
    minActions = 1,
    maxActions = 5
}

public enum EActType
{
    move,
    atk
}


public class Common : ScriptableObject
{
    public static Color orange = new Color(1.0f, 0.369f, 0.0f);
    public static Color pink = new Color(1, 0.678f, 0.925f);
    public static Color lime = new Color(0.802f, 1, 0.825f);
    public static Color green = new Color(0.220f, 1, 0.230f);
    public static Color cyan = new Color(0.1647f, 0.8215f, 1f);
    public static Color red = new Color(0.693f, 0.0f, 0.230f);
    public static Color semi = new Color(1.0f, 1.0f, 1.0f, 0.183f);
    public static Color trans = new Color(0.0f, 0.0f, 0.0f, 0.0f);

    public static CommandSkill CommonDebuffStr = CreateInstance<CommandSkill>();
    public static CommandSkill CommonDebuffDef = CreateInstance<CommandSkill>();
    public static CommandSkill CommonDebuffSpd = CreateInstance<CommandSkill>();

    public static WeaponScript noWeapon = CreateInstance<WeaponScript>();
    public static ArmorScript noArmor = CreateInstance<ArmorScript>();

    public static BoolConatainer container = new BoolConatainer();

    public static int MaxSkillLevel = 30;
    public static int maxDmg = 999;
    public static int MaxLevel = 99;
    public static List<EHitType> noHitList = new List<EHitType>()
    {
        EHitType.normal,
        EHitType.normal,
        EHitType.normal,
        EHitType.normal,
        EHitType.normal,
        EHitType.normal,
        EHitType.normal
    };
    public static Color GetFactionColor(Faction faction)
    {
        switch (faction)
        {
            case Faction.ally:
                return cyan;
                break;
            case Faction.enemy:
                return pink;
                break;
            case Faction.hazard:
                return Color.yellow;
                break;
            case Faction.other:
                return orange;
                break;
        }
        return Color.magenta;
    }
    public static string GetSideEffectText(SideEffect effect)
    {
        string text = "";
        switch (effect)
        {

            case SideEffect.reduceDef:
                text = "defense";
                break;
            case SideEffect.reduceMag:
                text = "magic";
                break;
            case SideEffect.reduceRes:
                text = "resistance";
                break;
            case SideEffect.reduceSpd:
                text = "speed";
                break;
            case SideEffect.reduceSkill:
                text = "skill";
                break;
            case SideEffect.none:
                break;
            case SideEffect.paralyze:
                text = "Paralyzed characters take 10% of their max health as damage and lose 1 action point at the start of the phase. Also makes target resist elec but weak to water.";
                break;
            case SideEffect.sleep:
                text = "Sleeping characters heal 10% of their max health but lose all action point at the start of the phase. 50% chance to wake up.  Also makes target resist water but weak to elec.";
                break;
            case SideEffect.freeze:
                text = "Frozen charachters gain a 10% defense buff but lose all action point at the start of the phase.  Also makes target resist ice but weak to fire.";
                break;
            case SideEffect.burn:
                text = "Burning characters take  20% of their max health as damage but gain 1 action point at the start of the phase. Also makes target resist fire but weak to ice. ";
                break;
            case SideEffect.poison:
                text = "Poisoned characters take 10% of their max health as damage and debuffs str by 10%.  Also makes target resist blunt but weak to slash.";
                break;
            case SideEffect.bleed:
                text = "Bleeding characters take 5% of their max health as damage and debuffs speed by 10%.  Also makes target resist pierce but weak to blunt.";
                break;
            case SideEffect.confusion:
                text = "Confused characters gain a random amount of action points at start of turn, randomly may attack themselves, allies, or enemies.  Also makes target resist blunt but weak to pierce.";
                break;
        }
        return text;
    }


    public static string GetStatusffectText(StatusEffect effect)
    {
        string text = "";
        switch (effect)
        {
            case StatusEffect.none:
                break;
            case StatusEffect.paralyzed:
                text = "Paralyzed characters take 10% of their max health as damage and lose 1 action point at the start of the phase. Also makes target resist elec but weak to water.";
                break;
            case StatusEffect.sleep:
                text = "Sleeping characters heal 10% of their max health but lose all action point at the start of the phase. 50% chance to wake up.  Also makes target resist water but weak to elec.";
                break;
            case StatusEffect.frozen:
                text = "Frozen charachters gain a 10% defense buff but lose all action point at the start of the phase.  Also makes target resist ice but weak to fire.";
                break;
            case StatusEffect.burned:
                text = "Burning characters take  20% of their max health as damage but gain 1 action point at the start of the phase. Also makes target resist fire but weak to ice. ";
                break;
            case StatusEffect.poisoned:
                text = "Poisoned characters take 10% of their max health as damage and debuffs str by 10%.  Also makes target resist blunt but weak to slash.";
                break;
            case StatusEffect.bleeding:
                text = "Bleeding characters take 5% of their max health as damage and debuffs speed by 10%.  Also makes target resist pierce but weak to blunt.";
                break;
            case StatusEffect.confused:
                text = "Confused characters gain a random amount of action points at start of turn, randomly may attack themselves, allies, or enemies.  Also makes target resist blunt but weak to pierce.";
                break;
        }
        return text;
    }

    public static ModifiedStat GetSideEffectMod(SideEffect effect)
    {
        ModifiedStat stat = ModifiedStat.none;
        switch (effect)
        {

            case SideEffect.reduceDef:
                stat = ModifiedStat.Def;
                break;
            case SideEffect.reduceMag:
                stat = ModifiedStat.Mag;
                break;
            case SideEffect.reduceRes:
                stat = ModifiedStat.Res;
                break;
            case SideEffect.reduceSpd:
                stat = ModifiedStat.Speed;
                break;
            case SideEffect.reduceSkill:
                stat = ModifiedStat.Skill;
                break;
        }
        return stat;
    }
    public static int GetDmgIndex(DMG dmg)
    {
        int returnInt = -1;
        switch (dmg)
        {
            case DMG.tiny:
                returnInt = 1;
                break;
            case DMG.small:
                returnInt = 2;
                break;
            case DMG.medium:
                returnInt = 3;
                break;
            case DMG.heavy:
                returnInt = 4;
                break;
            case DMG.severe:
                returnInt = 5;
                break;
            case DMG.collassal:
                returnInt = 6;
                break;
        }
        return returnInt;
    }

    public static DMG GetNextDmg(DMG damage)
    {
        switch (damage)
        {
            case DMG.tiny:
                return DMG.small;
                break;
            case DMG.small:
                return DMG.medium;
                break;
            case DMG.medium:
                return DMG.heavy;
                break;
            case DMG.heavy:
                return DMG.severe;
                break;
            case DMG.severe:
                return DMG.collassal;
                break;
            case DMG.collassal:
                return DMG.collassal;
                break;
        }
        return DMG.tiny;
    }

    public static List<Augment> GetAttackAugments()
    {
        List<Augment> augs = new List<Augment>();
        augs.Add(Augment.elementAugment);
        augs.Add(Augment.levelAugment);
        augs.Add(Augment.accurracyAugment);
        augs.Add(Augment.rangeAument);
        return augs;
    }
    public static List<Augment> GetArmorAugments()
    {
        List<Augment> augs = new List<Augment>();
        augs.Add(Augment.levelAugment);
        augs.Add(Augment.effectAugment1);
        augs.Add(Augment.defAugment);
        augs.Add(Augment.resAugment);
        augs.Add(Augment.spdAugment);
        return augs;
    }


    public static List<Augment> GetPhysChargeAugments()
    {
        List<Augment> augs = new List<Augment>();
        augs.Add(Augment.elementAugment);
        augs.Add(Augment.levelAugment);
        augs.Add(Augment.accurracyAugment);
        augs.Add(Augment.chargeDecreaseAugment);
        augs.Add(Augment.chargeIncreaseAugment);
        return augs;
    }

    public static List<Augment> GetPhysCostAugments()
    {
        List<Augment> augs = new List<Augment>();
        augs.Add(Augment.elementAugment);
        augs.Add(Augment.levelAugment);
        augs.Add(Augment.accurracyAugment);
        augs.Add(Augment.attackCountAugment);
        augs.Add(Augment.costAugment);
        augs.Add(Augment.sideEffectAugment);
        return augs;
    }

    public static List<Augment> GetMagicAugments()
    {
        List<Augment> augs = new List<Augment>();
        augs.Add(Augment.elementAugment);
        augs.Add(Augment.levelAugment);
        augs.Add(Augment.attackCountAugment);
        augs.Add(Augment.costAugment);
        augs.Add(Augment.sideEffectAugment);
        augs.Add(Augment.rangeAument);

        return augs;
    }

    public static List<Augment> GetSkillAugments()
    {
        List<Augment> augs = new List<Augment>();
        augs.Add(Augment.effectAugment1);
        augs.Add(Augment.effectAugment2);
        augs.Add(Augment.effectAugment3);
        return augs;
    }

    public static string GetAugmentText(Augment aug)
    {
        string returnText = "";

        switch (aug)
        {
            case Augment.levelAugment:
                returnText = "increase the level of ";
                break;
            case Augment.accurracyAugment:
                returnText = "increase the accurracy of ";
                break;
            case Augment.sideEffectAugment:
                returnText = "add an ailment to ";
                break;
            case Augment.rangeAument:
                returnText = "increase the range of ";
                break;
            case Augment.attackCountAugment:
                returnText = "increase the hit count of ";
                break;
            case Augment.costAugment:
                returnText = "reduce the cost of ";
                break;
            case Augment.chargeIncreaseAugment:
                returnText = "increase the charge of ";
                break;
            case Augment.chargeDecreaseAugment:
                returnText = "reduce the charge of ";
                break;
            case Augment.elementAugment:
                returnText = "randomly change the element of ";
                break;
            case Augment.effectAugment1:
                returnText = "boost the effect of ";
                break;
            case Augment.effectAugment2:
                returnText = "boost the effect of ";
                break;
            case Augment.effectAugment3:
                returnText = "boost the effect of ";
                break;
            case Augment.end:
                break;
            case Augment.none:
                break;
            case Augment.strAugment:
                returnText = "increase the strength boost of ";
                break;
            case Augment.magAugment:
                returnText = "increase the magic boost of ";
                break;
            case Augment.sklAugment:
                returnText = "increase the skill boost of ";
                break;
            case Augment.defAugment:
                returnText = "increase the defense boost of ";
                break;
            case Augment.resAugment:
                returnText = "increase the resistance boost of ";
                break;
            case Augment.spdAugment:
                returnText = "increase the speed boost of ";
                break;
            default:
                break;
        }
        return returnText;
    }
    public static string GetAugmentEffectText(Augment aug, UsableScript useable)
    {
        string returnText = "";
        string boostText = "";
        if (useable.GetType().IsSubclassOf(typeof(SkillScript)))
        {
            SkillScript askill = useable as SkillScript;
            switch (askill.ELEMENT)
            {

                case Element.Buff:
                    boostText = "Doubles the effect of the buff. ";
                    break;
                case Element.Support:
                    break;
                case Element.Ailment:
                    boostText = "Increases the chance of inflicting the ailment. ";
                    break;
                case Element.Passive:
                    boostText = "Boosts the effects of the passive skill by 15%. ";
                    break;

                case Element.Auto:
                    boostText = "Increases the chance of the auto skill triggering by 20% ";
                    break;

            }
        }
        else if (useable.GetType() == typeof(ArmorScript))
        {
            boostText = "Gain improved resistances but also greater weaknesses to attacks.";
        }
        switch (aug)
        {
            case Augment.levelAugment:
                returnText = "Increases the level of the skill, ward, or basic attack. Boosting its stats and/or abilities.";
                break;
            case Augment.accurracyAugment:
                returnText = "Increases the accurracy of the skill.";
                break;
            case Augment.sideEffectAugment:
                {
                    string ailmentText = "";
                    if (useable.GetType().IsSubclassOf(typeof(SkillScript)))
                    {
                        SkillScript skill = useable as SkillScript;
                        switch (skill.ELEMENT)
                        {
                            case Element.Water:
                                ailmentText = "confusion";
                                break;
                            case Element.Fire:
                                ailmentText = "burn";
                                break;
                            case Element.Ice:
                                ailmentText = "frozen";
                                break;
                            case Element.Electric:
                                ailmentText = "paralyze";
                                break;
                            case Element.Slash:
                                ailmentText = "bleed";
                                break;
                            case Element.Pierce:
                                ailmentText = "poison";
                                break;
                            case Element.Blunt:
                                ailmentText = "sleep";
                                break;

                        }
                    }
                    returnText = "Adds a chance to inflict " + ailmentText + " on target when using skill.";
                }
                break;
            case Augment.rangeAument:
                returnText = "Increases the range of the skill or basic attack.";
                break;
            case Augment.attackCountAugment:
                returnText = "Increases the amount of times the skill will try to hit the target.";
                break;
            case Augment.costAugment:
                returnText = "Reduces the MP/FT cost of the skill.";
                break;
            case Augment.chargeIncreaseAugment:
                returnText = "Increases the amount of FT that will be charged by the skill.";
                break;
            case Augment.chargeDecreaseAugment:
                returnText = "Reduces the amount of FT that will be charged by the skill.";
                break;
            case Augment.elementAugment:
                returnText = "Randomly change the element of the skill to a new element.";
                break;
            case Augment.effectAugment1:
                returnText = boostText;
                break;
            case Augment.effectAugment2:
                returnText = boostText;
                break;
            case Augment.effectAugment3:
                returnText = boostText;
                break;
            case Augment.end:
                break;
            case Augment.none:
                break;
            case Augment.strAugment:
                returnText = "Increases the strength boost of the basic attack.";
                break;
            case Augment.magAugment:
                returnText = "Increases the magic boost  of the basic attack. ";
                break;
            case Augment.sklAugment:
                returnText = "Increases the skill boost  of the basic attack.";
                break;
            case Augment.defAugment:
                returnText = "Increases the defense boost of the ward.";
                break;
            case Augment.resAugment:
                returnText = "Increases the resistance boost of the ward. ";
                break;
            case Augment.spdAugment:
                returnText = "Increases the speed boost of the ward. ";
                break;
            default:
                break;
        }
        return returnText;
    }
    public static int GetIconType(UsableScript useable)
    {
        if (useable.GetType() == typeof(WeaponScript))
            return (int)IconSet.weapon;

        if (useable.GetType() == typeof(ArmorScript))
            return (int)IconSet.ward;


        if (useable.GetType() == typeof(CommandSkill))
            return (int)IconSet.command;

        if (useable.GetType() == typeof(PassiveSkill))
            return (int)IconSet.passive;

        if (useable.GetType() == typeof(AutoSkill))
            return (int)IconSet.auto;

        if (useable.GetType() == typeof(OppSkill))
            return (int)IconSet.opp;

        if (useable.GetType() == typeof(ItemScript))
            return (int)IconSet.item;
        return -1;
    }

    public static Vector2 GetNextTileRange(List<Vector2> currentTiles)
    {
        Vector2 nextTile = Vector2.zero;
        for (int i = 0; i < currentTiles.Count; i++)
        {
            if (currentTiles[i].x > nextTile.x)
            {
                nextTile.x = currentTiles[i].x + 1;
            }
            if (currentTiles[i].y > nextTile.y)
            {
                nextTile.y = currentTiles[i].y + 1;
            }

        }
        return nextTile;
    }

    public static Element ChangeElement(Element element)
    {
        Element newElement = Element.none;
        newElement = (Element)Random.Range(0, 6);
        if (newElement == element)
        {
            newElement = ChangeElement(element);
        }
        return newElement;
    }

    public static void SetWeaponDistRange(WeaponScript weapon)
    {
        switch (weapon.ATKRANGE)
        {
          
            case RangeType.adjacent:
                weapon.Range = 1;
                weapon.DIST = 1;
                break;
            case RangeType.pinWheel:
                weapon.Range = 2;
                weapon.DIST = 2;
                break;
            case RangeType.detached:
                weapon.Range = 1;
                weapon.DIST = 2;
                break;
            case RangeType.stretched:
                weapon.Range = 2;
                weapon.DIST = 3;
                break;
            case RangeType.spear:
                weapon.Range = 2;
                weapon.DIST = 2;
                break;
            case RangeType.lance:
                weapon.Range = 3;
                weapon.DIST = 3;
                break;
     
            
        }
    }
}
