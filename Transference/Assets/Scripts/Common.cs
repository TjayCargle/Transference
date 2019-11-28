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
    PlayerUsingItems,
    EventRunning,
    SceneRunning,
    FairyPhase,
    PlayerAct,
    PlayerDead



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
    lethal // dmg x6 + lose a turn + stats halved

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
    Spd,
    Dex,
    dmg,
    all,
    ElementBody,
    deathAct,
    oppAct,
    moveAct


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
    Str,
    Mag,
    Def,
    Res,
    Spd,
    Dex,
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
    Movement,
    Item,
    //for opps
    Strike,
    Skill,
    Spell,
    None
}
public enum Augment
{
    none,
    levelAugment,
    accurracyAugment,
    sideEffectAugment,
    rangeAugment,
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
    dexAugment,
    defAugment,
    resAugment,
    spdAugment,
    end
}
public enum Element
{
    Water,
    Pyro,
    Ice,
    Electric,

    Slash,
    Pierce,
    Blunt,
    Force,
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
    BasicAtk,
    Armor,
    Physical,
    Magical,
    Passive,
    Auto,
    Opportunity,
    Items,
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
    natural
}
public enum Reaction
{
    none,
    AilmentOnly,
    buff,
    debuff,
    bonusAction,
    knockback,
    pullin,
    pushforward,
    pullback,
    jumpback,
    reposition,
    Swap,
    ApplyEffect,
    leathal,
    cripple,
    savage,
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
    tiny = 2,
    small = 4,
    medium = 8,
    heavy = 16,
    severe = 32,
    collassal = 64
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
public enum AtkType
{
    basic,
    skillSpell,
    opportunity,
    other
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
    chooseOptions,
    equipOS,
    generated,
    selectItem,
    trade,
    prevMenu,
    selectAct,
    selectDetails,
    shop,
    door,
    anEvent,
    selectSpells,
    selectStrikes,
    openBattleLog,
    forceEnd,
    yesPrompt,
    noPrompt,
    hack,
    guard,
    talk

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
    discoverItem,
    debuff,
    cripple,
    instaKill,
    poison,
    burn,
    freeze,
    confuse



}

public enum PrimaryStatus
{
    normal,
    crippled,
    great,
    tired,
    dead,
    guarding
}
//remove seconf
public enum SecondaryStatus
{
    normal,
    slow,
    rage,
    charm,
    seal,
    confusion,
    Summon
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
    reduceDex,
    reduceAtk,
    reduceGuard,
    reduceAct,
    debuff,
    heal,
    barrier,
    knockback,
    pullin,
    pushforward,
    pullback,
    jumpback,
    reposition,
    swap,


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

public enum HazardType
{
    attacker,
    zeroExp,
    lockDoor,
    controller,
    redirect

}
public enum TalkStage
{
    initial,
    Stats,
    Attack,
    learn,
    rejected
}
public enum EPType
{
    tactical,//prefers skills over spells or strikes
    itemist,//prefers to use items
    optimal,//priortizes using moves that will trigger opportunity attacks
    forceful,//prefers stikes over spells or skills
    aggro,//constantly tries to attack
    finisher,//never changes target until dead
    mystical,//prefers spells over skills or strikes
    biologist,//attempts to use status effect before anything else
    support,//will attempt to buff/debuff before anything else
    scared,//runs away to nearest door
    patrol,//moves back and forth between 2 tiles, upon seeing enemy switch personality types
    custom
}
public enum EPCluster
{
    physical,
    magical,
    logical,
    natural
}
public class MassAtkConatiner : ScriptableObject
{
    public List<AtkContainer> atkConatiners;
}


public class AtkContainer : ScriptableObject
{
    public LivingObject attackingObject;
    public GridObject dmgObject;
    public Element attackingElement;
    public EType attackType;
    public int dmg;
    public Reaction alteration;
    public CommandSkill command;
    public DmgReaction react;
    public bool crit = false;
    public WeaponScript strike;
    public void Inherit(AtkContainer container)
    {
        this.attackingObject = container.attackingObject;
        this.dmgObject = container.dmgObject;
        this.attackingElement = container.attackingElement;
        this.attackType = container.attackType;
        this.dmg = container.dmg;
        this.alteration = container.alteration;
        this.command = container.command;
        this.react = container.react;
        this.strike = container.strike;
    }
}
public class ItemContainer : ScriptableObject
{

    public GridObject target;
    public ItemScript item;

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
    public CommandSkill usedSkill;
    public WeaponScript usedStrike;
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
public struct BoolConatainerWTileList
{
    public bool result;
    public string name;
    public List<TileScript> tiles;
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
    CmdSpells,
    Strikes,
    none
}
public enum Faction
{
    ally,
    enemy,
    hazard,
    ordinary,
    eventObj,
    fairy,
    dropsSkill,
    dropsSpell,
    dropsStrike,
    dropsBarrier,
    dropsItem,
    dropsPassive,
    dropsAuto,
    inflictsAilment,
    inflictsDmg
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
    public int mapIndex;
    public List<int> doorIndexes;
    public List<string> roomNames;
    public List<int> roomIndexes;
    public List<int> startIndexes;
    public List<int> enemyIndexes;
    public List<int> hazardIndexes;
    public List<int> hazardIds;
    public List<int> shopIndexes;
    public List<int> objMapIndexes;
    public List<int> objIds;
    public List<int> enemyIds;
    public Texture texture;
    public int StartingPosition;
    public List<int> unOccupiedIndexes;
}

public struct SceneContainer
{
    public bool isRunning;
    public int index;
    public List<string> speakerNames;
    public List<string> speakertext;
    public List<Sprite> speakerFace;

}
public struct EventDetails
{
    public string eventText;
    public string choice1;
    public string choice2;
    public LivingObject affectedObject;
    public int eventNum;
}
public struct EventPair
{
    int eventNum;
    TileScript tile;
}
public struct MapData
{
    public List<int> unOccupiedIndexes;
    public bool eventMap;
    public List<EventPair> events;


    public string mapName;
    public int width;
    public int height;
    public int mapIndex;
    public List<int> doorIndexes;
    public List<string> roomNames;
    public List<int> roomIndexes;
    public List<int> startIndexes;
    public List<int> enemyIndexes;
    public List<int> glyphIndexes;
    public List<int> glyphIds;
    public List<int> shopIndexes;
    public List<int> objMapIndexes;
    public List<int> objIds;
    public Texture texture;
    public int StartingPosition;
    public List<int> EnemyIds;


    public float yElevation;
    public float xElevation;
    public int yMinRestriction;
    public int yMaxRestriction;
    public int xMinRestriction;
    public int xMaxRestriction;

    public int revealCount;

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

    public static Color errored = new Color(0.81569f, 0.25f, 0.00784f);
    public static Color denied = new Color(0.21569f, 0.0f, 0.00784f);
    public static Color granted = new Color(0.03137f, 0.91765f, 0.14902f);


    public static CommandSkill CommonDebuffStr = CreateInstance<CommandSkill>();
    public static CommandSkill CommonDebuffDef = CreateInstance<CommandSkill>();
    public static CommandSkill CommonDebuffSpd = CreateInstance<CommandSkill>();
    public static UsableScript GenericUsable = CreateInstance<UsableScript>();
    public static CommandSkill GenericSkill = CreateInstance<CommandSkill>();
    //public static WeaponScript noWeapon = CreateInstance<WeaponScript>();
    // public static readonly ArmorScript noArmor = new ArmorScript();

    private static ManagerScript manager = null;
    private static DatabaseManager database = null;

    public static BoolConatainer container = new BoolConatainer();

    public static EventDetails eventdetail = new EventDetails();
    public static bool summonedJax = false;
    public static bool summonedZeffron = false;
    public static int MaxSkillLevel = 10;
    public static int maxDmg = 999;
    public static int MaxLevel = 99;

    public static ManagerScript GetManager()
    {
        if(manager == null)
        {
            manager = GameObject.FindObjectOfType<ManagerScript>();
        }

        return manager;
    }

    public static DatabaseManager GetDatabase()
    {
        if (database == null)
        {
            database = GameObject.FindObjectOfType<DatabaseManager>();
        }

        return database;
    }



    public static List<EHitType> noHitList = new List<EHitType>()
    {
        EHitType.normal,
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
            case Faction.ordinary:
                return orange;
                break;
            case Faction.fairy:
                return lime;
            case Faction.eventObj:
                return Color.magenta;
                break;

            default:
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
            case SideEffect.reduceDex:
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
                text = "Frozen charachters gain a 10% defense buff but lose all action point at the start of the phase.  Also makes target resist ice but weak to pyro.";
                break;
            case SideEffect.burn:
                text = "Burning characters take  20% of their max health as damage but gain 1 action point at the start of the phase. Also makes target resist pyro but weak to ice. ";
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
                text = "Frozen charachters gain a 10% defense buff but lose all action point at the start of the phase.  Also makes target resist ice but weak to pyro.";
                break;
            case StatusEffect.burned:
                text = "Burning characters take  20% of their max health as damage but gain 1 action point at the start of the phase. Also makes target resist pyro but weak to ice. ";
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
                stat = ModifiedStat.Spd;
                break;
            case SideEffect.reduceDex:
                stat = ModifiedStat.Dex;
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
        augs.Add(Augment.rangeAugment);
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
        augs.Add(Augment.rangeAugment);

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
            case Augment.rangeAugment:
                returnText = "randomly change the range of ";
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
            case Augment.dexAugment:
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
                returnText = "Increases the level of the skill, Barrier, or Strike. Boosting its stats and/or abilities.";
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
                            case Element.Pyro:
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
            case Augment.rangeAugment:
                returnText = "randomly changes the range of the skill or Strike.";
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
                returnText = "Increases the strength boost of the Strike.";
                break;
            case Augment.magAugment:
                returnText = "Increases the magic boost  of the Strike. ";
                break;
            case Augment.dexAugment:
                returnText = "Increases the skill boost  of the Strike.";
                break;
            case Augment.defAugment:
                returnText = "Increases the defense boost of the Barrier.";
                break;
            case Augment.resAugment:
                returnText = "Increases the resistance boost of the Barrier. ";
                break;
            case Augment.spdAugment:
                returnText = "Increases the speed boost of the Barrier. ";
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
        {
            CommandSkill cmd = useable as CommandSkill;
            if (cmd.ETYPE == EType.magical)
                return (int)IconSet.magical;
            else
                return (int)IconSet.physical;
        }


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

    public static int GetnAtkDist(LivingObject livingObject)
    {

        int higestRange = 0;
        for (int i = 0; i < livingObject.INVENTORY.WEAPONS.Count; i++)
        {

            int check = GetWeaponAtkDist(livingObject.INVENTORY.WEAPONS[i]);
            if (higestRange < check)
                higestRange = check;

        }
        for (int i = 0; i < livingObject.INVENTORY.CSKILLS.Count; i++)
        {

            int check = GetWeaponAtkDist(livingObject.INVENTORY.CSKILLS[i]);
            if (higestRange < check)
                higestRange = check;

        }
        return higestRange;
    }
    public static int GetAtkRange(LivingObject livingObject)
    {

        int higestRange = 0;
        for (int i = 0; i < livingObject.INVENTORY.WEAPONS.Count; i++)
        {
            int check = GetWeaponAtkRange(livingObject.INVENTORY.WEAPONS[i]);
            if (higestRange < check)
                higestRange = check;
        }
        for (int i = 0; i < livingObject.INVENTORY.CSKILLS.Count; i++)
        {

            int check = GetWeaponAtkRange(livingObject.INVENTORY.CSKILLS[i]);
            if (higestRange < check)
                higestRange = check;

        }
        return higestRange;
    }
    public static int GetWeaponAtkDist(WeaponScript weapon)
    {
        switch (weapon.ATKRANGE)
        {

            case RangeType.adjacent:
                return 1;
                break;
            case RangeType.pinWheel:
                return 2;
                break;
            case RangeType.detached:
                return 2;
                break;
            case RangeType.stretched:
                return 3;
                break;
            case RangeType.rotator:
                return 1;
                break;
            case RangeType.fan:
                return 1;
                break;
            case RangeType.spear:
                return 2;
                break;
            case RangeType.lance:
                return 3;
                break;
            case RangeType.line:
                return 3;
                break;

        }
        return 0;
    }
    public static int GetWeaponAtkDist(CommandSkill weapon)
    {
        switch (weapon.RTYPE)
        {

            case RangeType.adjacent:
                return 1;
                break;
            case RangeType.pinWheel:
                return 2;
                break;
            case RangeType.detached:
                return 2;
                break;
            case RangeType.stretched:
                return 3;
                break;
            case RangeType.rotator:
                return 1;
                break;
            case RangeType.fan:
                return 1;
                break;
            case RangeType.spear:
                return 2;
                break;
            case RangeType.lance:
                return 3;
                break;
            case RangeType.line:
                return 3;
                break;

            case RangeType.rect:
                return 4;
                break;
            case RangeType.cone:
                return 3;
                break;
            case RangeType.tpose:
                return 4;
                break;
            case RangeType.clover:
                return 1;
                break;
            case RangeType.cross:
                return 1;
                break;
            case RangeType.square:
                return 2;
                break;
            case RangeType.box:
                return 5;
                break;
            case RangeType.diamond:
                return 1;
                break;
            case RangeType.crosshair:
                return 2;
                break;
        }
        return 0;
    }
    public static int GetWeaponAtkRange(WeaponScript weapon)
    {
        switch (weapon.ATKRANGE)
        {

            case RangeType.adjacent:
                return 1;
                break;
            case RangeType.pinWheel:
                return 2;
                break;
            case RangeType.detached:
                return 2;
                break;
            case RangeType.stretched:
                return 2;
                break;
            case RangeType.rotator:
                return 1;
                break;
            case RangeType.fan:
                return 1;
                break;
            case RangeType.spear:
                return 1;
                break;
            case RangeType.lance:
                return 1;
                break;
            case RangeType.line:
                return 1;
                break;

        }
        return 0;
    }

    public static int GetWeaponAtkRange(CommandSkill weapon)
    {
        switch (weapon.RTYPE)
        {

            case RangeType.adjacent:
                return 1;
                break;
            case RangeType.pinWheel:
                return 2;
                break;
            case RangeType.detached:
                return 2;
                break;
            case RangeType.stretched:
                return 2;
                break;
            case RangeType.rotator:
                return 1;
                break;
            case RangeType.fan:
                return 1;
                break;
            case RangeType.spear:
                return 1;
                break;
            case RangeType.lance:
                return 1;
                break;
            case RangeType.line:
                return 1;
                break;

            case RangeType.rect:
                return 1;
                break;
            case RangeType.cone:
                return 1;
                break;
            case RangeType.tpose:
                return 1;
                break;
            case RangeType.clover:
                return 1;
                break;
            case RangeType.cross:
                return 1;
                break;
            case RangeType.square:
                return 1;
                break;
            case RangeType.box:
                return 1;
                break;
            case RangeType.diamond:
                return 2;
                break;
            case RangeType.crosshair:
                return 2;
                break;

        }
        return 0;
    }

    public static EventDetails GetEventText(int eventnum, LivingObject living)
    {
        eventdetail.eventText = "";
        eventdetail.choice1 = "";
        eventdetail.choice2 = "";
        eventdetail.affectedObject = null;
        eventdetail.eventNum = -1;
        switch (eventnum)
        {
            case 1:
                {
                    eventdetail.eventText = "Will you give up Strength in return for magic?";
                    eventdetail.choice1 = "Yes";
                    eventdetail.choice2 = "No";
                    eventdetail.affectedObject = living;
                    eventdetail.eventNum = 1;
                }
                break;

            case 2:
                {
                    eventdetail.eventText = "Will you give up magic in return for strength?";
                    eventdetail.choice1 = "Yes";
                    eventdetail.choice2 = "No";
                    eventdetail.affectedObject = living;
                    eventdetail.eventNum = 2;
                }
                break;
            case 3:
                {
                    eventdetail.eventText = "Do you want to reset the maps?";
                    eventdetail.choice1 = "Yes";
                    eventdetail.choice2 = "No";
                    eventdetail.affectedObject = living;
                    eventdetail.eventNum = 3;
                }
                break;
        }

        return eventdetail;
    }

    public static string GetShortName(string name)
    {
        string shrtname = name;
        string[] subs = shrtname.Split(' ');
        shrtname = "";
        for (int i = 0; i < subs.Length; i++)
        {
            shrtname += subs[i];
        }

        return shrtname;
    }
    public static EPCluster GetEPCluster(EPType type)
    {

        switch (type)
        {
            case EPType.tactical:
                return EPCluster.physical;
                break;
            case EPType.itemist:
                return EPCluster.logical;
                break;
            case EPType.optimal:
                return EPCluster.logical;
                break;
            case EPType.forceful:
                return EPCluster.logical;
                break;
            case EPType.aggro:
                return EPCluster.physical;
                break;
            case EPType.finisher:
                return EPCluster.physical;
                break;
            case EPType.mystical:
                return EPCluster.magical;
                break;
            case EPType.biologist:
                return EPCluster.magical;
                break;
            case EPType.support:
                return EPCluster.magical;
                break;
            case EPType.scared:
                return EPCluster.natural;
                break;
            case EPType.custom:
                return EPCluster.natural;
                break;
            default:
                return EPCluster.natural;
                break;
        }
    }
    public static EPType GetRandomType(EPCluster cluster)
    {
        switch (cluster)
        {
            case EPCluster.physical:
                {
                    int ptye = Random.Range(0, 2);
                    switch (ptye)
                    {
                        case 0:
                            {
                                return EPType.forceful;
                            }
                            break;
                        case 1:
                            {
                                return EPType.finisher;
                            }
                            break;
                        case 2:
                            {
                                return EPType.aggro;
                            }
                            break;
                        default:
                            return EPType.aggro;
                            break;
                    }
                }
                break;
            case EPCluster.magical:
                {
                    int ptye = Random.Range(0, 2);
                    switch (ptye)
                    {
                        case 0:
                            {
                                return EPType.mystical;
                            }
                            break;
                        case 1:
                            {
                                return EPType.biologist;
                            }
                            break;
                        case 2:
                            {
                                return EPType.support;
                            }
                            break;
                        default:
                            return EPType.mystical;
                            break;
                    }
                }
                break;
            case EPCluster.logical:
                {
                    int ptye = Random.Range(0, 2);
                    switch (ptye)
                    {
                        case 0:
                            {
                                return EPType.tactical;
                            }
                            break;
                        case 1:
                            {
                                return EPType.itemist;
                            }
                            break;
                        case 2:
                            {
                                return EPType.optimal;
                            }
                            break;
                        default:
                            return EPType.tactical;
                            break;
                    }
                }
                break;
            case EPCluster.natural:
                {
                    return EPType.scared;
                }
                break;
            default:
                return EPType.scared;
                break;
        }
    }
    public static Reaction EffectToReaction(SideEffect effect)
    {

        switch (effect)
        {

            case SideEffect.swap:
                return Reaction.Swap;
                break;

            case SideEffect.knockback:
                return Reaction.knockback;
                break;
            case SideEffect.pullin:
                return Reaction.pullin;
                break;
            case SideEffect.pushforward:
                return Reaction.pushforward;
                break;
            case SideEffect.pullback:
                return Reaction.pullback;
                break;
            case SideEffect.jumpback:
                return Reaction.jumpback;
                break;
            case SideEffect.reposition:
                return Reaction.reposition;
                break;
        }

        return Reaction.none;
    }

    public static MapDetail ConvertMapData2Detail(MapData data, MapDetail detail)
    {

        // detail.mapName = data.mapName;
        detail.width = data.width;
        detail.height = data.height;
        detail.mapIndex = data.mapIndex;


        detail.doorIndexes.Clear();
        detail.roomNames.Clear();
        detail.roomIndexes.Clear();
        detail.enemyIndexes.Clear();
        detail.hazardIndexes.Clear();
        detail.shopIndexes.Clear();
        detail.startIndexes.Clear();
        detail.objMapIndexes.Clear();
        detail.objIds.Clear();
        detail.enemyIds.Clear();
        detail.hazardIds.Clear();

        detail.unOccupiedIndexes.Clear();
        detail.unOccupiedIndexes.AddRange(data.unOccupiedIndexes);

        detail.doorIndexes.AddRange(data.doorIndexes);
        detail.roomNames.AddRange(data.roomNames);
        detail.roomIndexes.AddRange(data.roomIndexes);

        detail.startIndexes.AddRange(data.startIndexes);
        detail.enemyIndexes.AddRange(data.enemyIndexes);
        detail.hazardIndexes.AddRange(data.glyphIndexes);

        detail.shopIndexes.AddRange(data.shopIndexes);
        detail.objMapIndexes.AddRange(data.objMapIndexes);
        detail.objIds.AddRange(data.objIds);
        detail.enemyIds.AddRange(data.EnemyIds);
        detail.StartingPosition = data.StartingPosition;

        detail.hazardIds.AddRange(data.glyphIds);

        return detail;
    }

    public static MapData ConvertMapDetail2Data(MapDetail data, MapData detail)
    {

        detail.mapName = data.mapName;
        detail.width = data.width;
        detail.height = data.height;
        detail.mapIndex = data.mapIndex;



        detail.doorIndexes.Clear();
        detail.roomNames.Clear();
        detail.roomIndexes.Clear();
        detail.enemyIndexes.Clear();
        detail.glyphIndexes.Clear();
        detail.glyphIds.Clear();
        detail.shopIndexes.Clear();
        detail.startIndexes.Clear();
        detail.objMapIndexes.Clear();
        detail.objIds.Clear();
        if (data.hazardIds.Count > 0)
        {
            detail.glyphIds.Clear();
            detail.glyphIds.AddRange(data.hazardIds);
        }
        if (data.unOccupiedIndexes.Count > 0)
        {
            detail.unOccupiedIndexes.Clear();
            detail.unOccupiedIndexes.AddRange(data.unOccupiedIndexes);
        }

        detail.doorIndexes.AddRange(data.doorIndexes);
        detail.roomNames.AddRange(data.roomNames);
        detail.roomIndexes.AddRange(data.roomIndexes);

        detail.startIndexes.AddRange(data.startIndexes);
        detail.enemyIndexes.AddRange(data.enemyIndexes);
        detail.glyphIndexes.AddRange(data.hazardIndexes);

        detail.shopIndexes.AddRange(data.shopIndexes);
        detail.objMapIndexes.AddRange(data.objMapIndexes);
        detail.EnemyIds.AddRange(data.enemyIds);
        detail.objIds.AddRange(data.objIds);
        detail.StartingPosition = data.StartingPosition;

        return detail;
    }
}
