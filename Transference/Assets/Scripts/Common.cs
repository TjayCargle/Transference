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
    PlayerDead,
    PlayerAllocate



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
    moveAct,
    pushBonus


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
    Combo,
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
    mental
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
    Slamback,
    ApplyEffect,
    leathal,
    cripple,
    savage,
    weak,
    resist,
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
    summon,
    dart
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
public enum BossPhase
{
    none,
    inital,
    angry,
    desperate
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
    Skills,
    equipAS,
    equipPS,
    chooseOptions,
    equipOS,
    generated,
    Items,
    trade,
    prevMenu,
    Battle,
    Details,
    Shop,
    Door,
    anEvent,
    Spells,
    Strikes,
    openBattleLog,
    forceEnd,
    yesPrompt,
    noPrompt,
    Hack,
    Guard,
    Talk,
    Tip,
    Interact,
    heal,
    restore,
    charge,
    drain,
    overload,
    shield,
    allocate

}



public enum SkillEvent
{
    beforeDmg,
    afterDmg,
    afterKilling,
    turnBegin,
    turnEnd,
    onUse,
    onHit,
    onMiss,
    onKill,
    onLevelUp,
    onHitWeakness,
    onHitResistance

}


public enum SkillReaction
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
    confuse,
    increaseAccurracy,
    decreaseAccurracy,
    resetAccurracy,
    increaseStr,
    increaseDef,
    increaseMag,
    increaseRes,
    increaseSpd,
    increaseDex,
    maxMana,
    maxHealth,
    MaxFatigue,
    FatigueZero,
    enterGuardState,
    increaseCrit,
    decreaseCrit,
    resetCrit,
    becomeWater,
    becomePyro,
    becomeIce,
    becomeElec,
    becomeSlash,
    becomePierce,
    becomeBlunt,
    becomeForce,
    gainBecomeEvent,
    removeEvent




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
    slamback,
    swap,
    cripple,
    death


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

    Crippled,
    Guard
}

public enum TileType
{
    regular = 0,
    door,
    shop,
    tevent,
    help,
    knockback,
    pullin,
    swap,
    reposition
}

public enum HazardType
{
    attacker,
    lockDoor,
    redirect,
    controller,
    movement,
    zeroExp,
    protection,
    time

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
    skillful,//prefers skills over spells or strikes
    itemist,//prefers to use items
    optimal,//priortizes using moves that will trigger opportunity attacks
    forceful,//prefers stikes over spells or skills
    aggro,//constantly tries to attack
    finisher,//never changes target until dead
    mystical,//prefers spells over skills or strikes
    biologist,//attempts to use status effect before anything else
    support,//will attempt to buff/debuff before anything else
    scared,//runs away to nearest door
    custom
}
public enum EPState
{
    patrol,//moves back and forth between 2 tiles, upon seeing enemy switch personality states
    scared, // runs away
    aggro, // only attack with no option to talk
    neutral // still attacks but open to talking

}
public enum EPCluster
{
    physical,
    magical,
    logical,
    natural
}
public enum BossCommand
{
    strike,
    skill,
    spell,
    barrier,
    item,
    heal,
    restore,
    drain,
    charge,
    shield,
    overload
}
public class MassAtkConatiner : ScriptableObject
{
    public List<AtkContainer> atkContainers;
}

public struct ScriptableContainer
{
    public bool inUse;
    public ScriptableObject scriptable;
}
public struct BossProfile
{
    public BossPhase currentPhase;
    public BossPhase previousPhase;
    public int healthbars;
    public List<BossCommand> commands;
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

public class SkillEventContainer : ScriptableObject
{

    public SkillEvent theEvent;
    public SkillReaction theReaction;
    public UsableScript theSkill;

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


public struct Tutorial
{
    public bool isActive;
    public List<tutorialStep> steps;
    public int currentStep;
    public List<int> clarifications;
}

public enum tutorialStep
{
    moveToPosition,
    useStrike,
    useSkill,
    useSpell,
    useBarrier,
    useItem,
    allocate,
    defeatEnemy,
    hackGlyph

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

public enum Action
{
    strike,
    spell,
    skill,
    allocate,
    move,
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
    inflictsDmg,
    interactable
}

public enum Interaction
{
    none,
    dropChandelier,
    drink,
    strUp,
    defUp,
    spdUp,
    slashDmg,
    pierceDmg,
    bluntDmg
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

public struct FullMap
{
    List<int> roomstypes;
    List<MapDetail> actualRooms;
}
public struct MapDetail
{
    public string mapName;
    public int width;
    public int height;
    public int mapIndex;
    public int roomType;
    public int StartingPosition;
    public Texture texture;
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
    public List<int> specialExtra;
    public List<int> tileIndexes;
    public List<int> unOccupiedIndexes;
    public List<TileType> specialiles;
    public List<int> tilesInShadow;
}
public enum SceneEvent
{
    move,
    showimage,
    hideimage,
    scaleimage,
    shake,
    blackout,
    dim
}
public struct SceneEventContainer
{
    public int intercept;
    public SceneEvent scene;
    public int data;
}
public struct SceneContainer
{
    public bool isRunning;
    public int index;
    public int soundTrack;
    public List<string> speakerNames;
    public List<string> speakertext;
    public List<Sprite> speakerFace;
    public List<int> eventIndexs;
    public List<SceneEventContainer> sceneEvents;

}
public struct EventDetails
{
    public string eventTitle;
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
    public int StartingPosition;
    public int yMinRestriction;
    public int yMaxRestriction;
    public int xMinRestriction;
    public int xMaxRestriction;
    public int revealCount;

    public float yElevation;
    public float xElevation;

    public Texture texture;
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
    public List<int> specialTileIndexes;
    public List<int> EnemyIds;
    public List<TileType> specialiles;
    public List<int> specialExtra;
    public List<int> tilesInShadow;
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
public enum ObjectiveType
{
    none,
    reachLocation,
    defeatSpecificEnemy,
    defeatAllEnemies,
    defeatSpecificGlyph,
    defeatAllGlyphs,
    storyRelated
}

public class Common : ScriptableObject
{
    public static Color gold = new Color(0.8f, 0.569f, 0.2f);
    public static Color orange = new Color(1.0f, 0.369f, 0.0f);
    public static Color pink = new Color(1, 0.678f, 0.625f);
    public static Color lime = new Color(0.802f, 1, 0.825f);
    public static Color green = new Color(0.220f, 1, 0.230f);
    public static Color cyan = new Color(0.1647f, 0.8215f, 1f);
    // public static Color red = new Color(0.693f, 0.0f, 0.230f);
    public static Color red = new Color(1, 0.0f, 0.230f);
    public static Color semi = new Color(1.0f, 1.0f, 1.0f, 0.183f);
    public static Color moresemi = new Color(1.0f, 1.0f, 1.0f, 0.083f);
    public static Color trans = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    public static Color selcted = new Color(0.4f, 0.6f, 0.4f);
    public static Color blackened = new Color(0.117f, 0.0f, 0.0f);
    public static Color dark = new Color(0.278f, 0.278f, 0.278f);


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
    private static Texture swapTexture = null;
    private static Texture knockbackTexture = null;
    private static Texture repositionTexture = null;
    private static Texture pullinTexture = null;
    private static Texture helpTexture = null;

    public static BoolConatainer container = new BoolConatainer();

    public static EventDetails eventdetail = new EventDetails();
    public static bool summonedJax = false;
    public static bool summonedZeffron = false;
    public static int MaxSkillLevel = 10;
    public static int maxDmg = 999;
    public static int MaxLevel = 99;

    public static ManagerScript GetManager()
    {
        if (manager == null)
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

    public static Texture GetSpecialTexture(TileType type)
    {
        Texture returnedTexture = null;

        switch (type)
        {
            case TileType.regular:
                break;
            case TileType.door:
                break;
            case TileType.shop:
                break;
            case TileType.tevent:
                break;
            case TileType.help:
                {
                    if (helpTexture != null)
                    {
                        return helpTexture;
                    }
                    helpTexture = Resources.Load<Texture>("Tiles/Help");
                    return helpTexture;
                }
                break;
            case TileType.knockback:
                {
                    if (knockbackTexture != null)
                    {
                        return knockbackTexture;
                    }
                    knockbackTexture = Resources.Load<Texture>("Tiles/Knockback");
                    return knockbackTexture;
                }
                break;
            case TileType.pullin:
                {
                    if (pullinTexture != null)
                    {
                        return pullinTexture;
                    }
                    pullinTexture = Resources.Load<Texture>("Tiles/Pullin");
                    return pullinTexture;
                }
                break;
            case TileType.swap:
                {
                    if (swapTexture != null)
                    {
                        return swapTexture;
                    }
                    swapTexture = Resources.Load<Texture>("Tiles/Swap");
                    return swapTexture;
                }
                break;
            case TileType.reposition:
                {
                    if (repositionTexture != null)
                    {
                        return repositionTexture;
                    }
                    repositionTexture = Resources.Load<Texture>("Tiles/Reposition");
                    return repositionTexture;
                }
                break;
        }

        return returnedTexture;
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

    public static int GetElementIndex(Element e)
    {
        int indx = 0;
        switch (e)
        {
            case Element.Water:
                {
                    indx = 0;
                }
                break;
            case Element.Pyro:
                {
                    indx = 1;
                }
                break;
            case Element.Ice:
                {
                    indx = 2;
                }
                break;
            case Element.Electric:
                {
                    indx = 3;
                }
                break;
            case Element.Slash:
                {
                    indx = 4;
                }
                break;
            case Element.Pierce:
                {
                    indx = 5;
                }
                break;
            case Element.Blunt:
                {
                    indx = 6;
                }
                break;
            case Element.Force:
                {
                    indx = 7;
                }
                break;
            case Element.Buff:
                {
                    indx = 8;
                }
                break;
            case Element.Support:
                {
                    indx = 9;
                }
                break;
            case Element.Ailment:
                {
                    indx = 10;
                }
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
        return indx;
    }

    public static string GetSkillEventText(SkillEvent se, SkillReaction sr)
    {
        string returnedString = "";

        switch (sr)
        {

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
            case SkillReaction.increaseAccurracy:
                {
                    returnedString += " Accurracy Increases ";
                }
                break;
                break;
            case SkillReaction.resetAccurracy:
                {
                    returnedString += " Accurracy resets to 1 ";
                }
                break;
            case SkillReaction.increaseStr:
                {
                    returnedString += " Character Str increases ";
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
            case SkillReaction.decreaseAccurracy:
                {
                    returnedString += " Accurracy decreases ";
                }
                break;
            case SkillReaction.increaseDef:
                {
                    returnedString += " Character Def increases ";
                }
                break;
            case SkillReaction.increaseMag:
                {
                    returnedString += " Character Mag increases ";
                }
                break;
            case SkillReaction.increaseRes:
                {
                    returnedString += " Character Res increases ";
                }
                break;
            case SkillReaction.increaseSpd:
                {
                    returnedString += " Character Spd increases ";
                }
                break;
            case SkillReaction.increaseDex:
                {
                    returnedString += " Character Dex increases ";
                }
                break;
            case SkillReaction.maxMana:
                {
                    returnedString += " Character Mana returns to full ";
                }
                break;
            case SkillReaction.maxHealth:
                {
                    returnedString += " Character Health returns to full ";
                }
                break;
            case SkillReaction.MaxFatigue:
                {
                    returnedString += " Character Fatigue returns to full ";
                }
                break;
            case SkillReaction.FatigueZero:
                {
                    returnedString += " Character Fatigue becomes 0 ";
                }
                break;
            case SkillReaction.enterGuardState:
                {
                    returnedString += " Character enters guard staate ";
                }
                break;
            case SkillReaction.increaseCrit:
                {
                    returnedString += " Critical chance increases ";
                }
                break;
            case SkillReaction.decreaseCrit:
                {
                    returnedString += " Critical chance decreases ";
                }
                break;
            case SkillReaction.resetCrit:
                {
                    returnedString += " Critical chance resets to 0 ";
                }
                break;
            case SkillReaction.becomeWater:
                {
                    returnedString += " Change element to Water ";
                }
                break;
            case SkillReaction.becomePyro:
                {
                    returnedString += " Change element to Pyro ";
                }
                break;
            case SkillReaction.becomeIce:
                {
                    returnedString += " Change element to Ice ";
                }
                break;
            case SkillReaction.becomeElec:
                {
                    returnedString += " Change element to Elec ";
                }
                break;
            case SkillReaction.becomeSlash:
                {
                    returnedString += " Change element to Slash ";
                }
                break;
            case SkillReaction.becomePierce:
                {
                    returnedString += " Change element to Pierce ";
                }
                break;
            case SkillReaction.becomeBlunt:
                {
                    returnedString += " Change element to Blunt ";
                }
                break;
            case SkillReaction.becomeForce:
                {
                    returnedString += " Change element to Force ";
                }
                break;
            case SkillReaction.gainBecomeEvent:
                break;
            case SkillReaction.removeEvent:
                {
                    returnedString += " Remove special ability ";
                }
                break;
            default:
                break;
        }

        switch (se)
        {
            case SkillEvent.beforeDmg:
                break;
            case SkillEvent.afterDmg:
                break;
            case SkillEvent.afterKilling:
                break;
            case SkillEvent.turnBegin:
                break;
            case SkillEvent.turnEnd:
                break;
            case SkillEvent.onUse:
                {
                    returnedString += "upon use.";
                }
                break;
            case SkillEvent.onHit:
                {
                    returnedString += "upon hit.";
                }
                break;
            case SkillEvent.onMiss:
                {
                    returnedString += "upon miss.";
                }
                break;
            case SkillEvent.onKill:
                {
                    returnedString += "upon defeating enemy with this skill.";
                }
                break;
            case SkillEvent.onLevelUp:
                {
                    returnedString += "upon leveling up.";
                }
                break;
            default:
                break;
        }


        return returnedString;
    }

    public static string GetElementSpriteIndex(Element el)
    {
        string returnedString = "";

        switch (el)
        {
            case Element.Water:
                {
                    returnedString = "<sprite=35>";
                }
                break;
            case Element.Pyro:
                {
                    returnedString = "<sprite=34>";
                }
                break;
            case Element.Ice:
                {
                    returnedString = "<sprite=36>";
                }
                break;
            case Element.Electric:
                {
                    returnedString = "<sprite=37>";
                }
                break;
            case Element.Slash:
                {
                    returnedString = "<sprite=38>";
                }
                break;
            case Element.Pierce:
                {
                    returnedString = "<sprite=39>";
                }
                break;
            case Element.Blunt:
                {
                    returnedString = "<sprite=40>";
                }
                break;
            case Element.Force:
                {
                    returnedString = "<sprite=43>";
                }
                break;
            case Element.Buff:
                break;
            case Element.Support:
                {
                    returnedString = "<sprite=41>";
                }
                break;
            case Element.Ailment:
                {
                    returnedString = "<sprite=42>";
                }
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

        return returnedString;
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
                text = "Paralyzed characters take 10% of their max health as damage and lose 1 action point at the start of the phase. ";
                break;
            case SideEffect.sleep:
                text = "Sleeping characters heal 10% of their max health but lose all action point at the start of the phase. 50% chance to wake up.  ";
                break;
            case SideEffect.freeze:
                text = "Frozen charachters gain a 10% defense buff but lose all action point at the start of the phase.  ";
                break;
            case SideEffect.burn:
                text = "Burning characters take  20% of their max health as damage but gain 1 action point at the start of the phase. ";
                break;
            case SideEffect.poison:
                text = "Poisoned characters take 10% of their max health as damage and debuffs str by 10%. ";
                break;
            case SideEffect.bleed:
                text = "Bleeding characters take 5% of their max health as damage and debuffs speed by 10%. ";
                break;
            case SideEffect.confusion:
                text = "Confused characters gain a random amount of action points at start of turn, randomly may attack themselves, allies, or enemies. ";
                break;
        }
        return text;
    }

    public static bool LogicCheckStatus(StatusEffect effect, LivingObject living)
    {
        bool canUse = true;
        for (int i = 0; i < living.INVENTORY.EFFECTS.Count; i++)
        {
            SideEffect liveAffect = living.INVENTORY.EFFECTS[i].EFFECT;
            switch (effect)
            {
                case StatusEffect.none:
                    break;
                case StatusEffect.paralyzed:
                    {
                        if (liveAffect == SideEffect.freeze)
                        {
                            return false;
                        }
                    }
                    break;
                case StatusEffect.sleep:
                    {
                        if (liveAffect == SideEffect.freeze)
                        {
                            return false;
                        }
                    }
                    break;
                case StatusEffect.frozen:
                    {
                        if (liveAffect != SideEffect.none)
                        {
                            return false;
                        }
                    }
                    break;
                case StatusEffect.burned:
                    {
                        if (liveAffect == SideEffect.freeze)
                        {
                            return false;
                        }
                    }
                    break;
                case StatusEffect.poisoned:
                    {
                        if (liveAffect == SideEffect.freeze)
                        {
                            return false;
                        }
                    }
                    break;
                case StatusEffect.bleeding:
                    {
                        if (liveAffect == SideEffect.freeze)
                        {
                            return false;
                        }
                    }
                    break;
                case StatusEffect.confused:
                    {
                        if (liveAffect == SideEffect.freeze)
                        {
                            return false;
                        }
                    }
                    break;

            }
        }

        return canUse;
    }
    public static bool IsEnemy(Faction testFaction)
    {
        switch (testFaction)
        {

            case Faction.enemy:
                return true;
                break;
            case Faction.hazard:
                return true;
                break;

            case Faction.fairy:
                return true;
                break;

        }
        return false;
    }
    public static string GetStatusffectText(StatusEffect effect)
    {
        string text = "";
        switch (effect)
        {
            case StatusEffect.none:
                break;
            case StatusEffect.paralyzed:
                text = "Paralyzed characters take 10% of their max health as damage and lose 1 action point at the start of the phase.";
                break;
            case StatusEffect.sleep:
                text = "Sleeping characters heal 10% of their max health but lose all action point at the start of the phase. 50% chance to wake up.";
                break;
            case StatusEffect.frozen:
                text = "Frozen charachters gain a 10% defense buff but lose all action point at the start of the phase.";
                break;
            case StatusEffect.burned:
                text = "Burning characters take  20% of their max health as damage but gain 1 action point at the start of the phase. ";
                break;
            case StatusEffect.poisoned:
                text = "Poisoned characters take 10% of their max health as damage and debuffs str by 10%.";
                break;
            case StatusEffect.bleeding:
                text = "Bleeding characters take 5% of their max health as damage and debuffs speed by 10%.";
                break;
            case StatusEffect.confused:
                text = "Confused characters gain a random amount of action points at start of turn, randomly may attack themselves, allies, or enemies.";
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
    public static string GetAugmentNameText(Augment aug)
    {
        string text = "";

        switch (aug)
        {
            case Augment.none:
                text = "Purchased";
                break;
            case Augment.levelAugment:
                text = "Increase Level";
                break;
            case Augment.accurracyAugment:
                text = "Increase Accurracy";
                break;
            case Augment.sideEffectAugment:
                text = "Add Effect";
                break;
            case Augment.rangeAugment:
                text = "Change Range";
                break;
            case Augment.attackCountAugment:
                text = "Increase Hit Count";
                break;
            case Augment.costAugment:
                text = "Reduce Cost";
                break;
            case Augment.chargeIncreaseAugment:
                text = "Increase Charge Amount";
                break;
            case Augment.chargeDecreaseAugment:
                text = "Decrease Charge Amount";
                break;
            case Augment.elementAugment:
                text = "Change Element";
                break;
            case Augment.effectAugment1:
                text = "Upgrade Effects";
                break;
            case Augment.effectAugment2:
                text = "Upgrade Effects";
                break;
            case Augment.effectAugment3:
                text = "Upgrade Effects";
                break;
            case Augment.strAugment:
                text = "Increase Strength";
                break;
            case Augment.magAugment:
                text = "Increase Magic";
                break;
            case Augment.dexAugment:
                text = "Increase Dexterity";
                break;
            case Augment.defAugment:
                text = "Increase Defense";
                break;
            case Augment.resAugment:
                text = "Increase Resistance";
                break;
            case Augment.spdAugment:
                text = "Increase Speed";
                break;
            case Augment.end:
                text = "If you see this, contact Jtrev";
                break;
        }

        return text;
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
                    boostText = "Increase actions gained when combo is executed";
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

    public static void UpdateBossProfile(int index, EnemyScript enemy)
    {

        if (index == 101)
        {
            BossScript someProfile = enemy.specialProfile;
            if(someProfile == null)
            {
                someProfile = enemy.specialProfile = ScriptableObject.CreateInstance<BossScript>();
                someProfile.currentPhase = BossPhase.none;
                someProfile.healthbars = 3;
            }
            switch (enemy.specialProfile.currentPhase)
            {
                case BossPhase.none:
                    {
                        someProfile.currentPhase = BossPhase.inital;
                        if (someProfile.commands == null)
                            someProfile.commands = new List<BossCommand>();
                        someProfile.commands.Add(BossCommand.strike);
                        someProfile.commands.Add(BossCommand.strike);
                        someProfile.commands.Add(BossCommand.strike);
                        someProfile.commands.Add(BossCommand.item);
                        someProfile.commands.Add(BossCommand.heal);
                    }
                    break;
                case BossPhase.inital:
                    {
                    someProfile.currentPhase = BossPhase.angry;
                        if (someProfile.commands == null)
                            someProfile.commands = new List<BossCommand>();
                        else
                            someProfile.commands.Clear();
                        someProfile.commands.Add(BossCommand.strike);
                        someProfile.commands.Add(BossCommand.strike);
                        someProfile.commands.Add(BossCommand.item);
                        someProfile.commands.Add(BossCommand.item);
                        someProfile.commands.Add(BossCommand.heal);
                        someProfile.commands.Add(BossCommand.heal);
                        someProfile.commands.Add(BossCommand.shield);
                    }
                    break;
                case BossPhase.angry:
                    {
                    someProfile.currentPhase = BossPhase.desperate;
                        if (someProfile.commands == null)
                            someProfile.commands = new List<BossCommand>();
                        else
                            someProfile.commands.Clear();
                        someProfile.commands.Add(BossCommand.strike);
                        someProfile.commands.Add(BossCommand.item);
                        someProfile.commands.Add(BossCommand.item);
                        someProfile.commands.Add(BossCommand.item);
                        someProfile.commands.Add(BossCommand.heal);
                        someProfile.commands.Add(BossCommand.heal);
                        someProfile.commands.Add(BossCommand.shield);
                        someProfile.commands.Add(BossCommand.shield);
                    }
                    break;
                case BossPhase.desperate:
                    Debug.Log("boss healthbars not equal to phases");
                    break;

            }
        }
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


        if (useable.GetType() == typeof(ComboSkill))
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
    public static string GetTutorialText(tutorialStep someStep)
    {
        string returnedString = "";

        switch (someStep)
        {
            case tutorialStep.moveToPosition:
                returnedString = "Move to position";
                break;
            case tutorialStep.useStrike:
                returnedString = "Attack with a strike";
                break;
            case tutorialStep.useSkill:
                returnedString = "Attack with a skill";
                break;
            case tutorialStep.useSpell:
                returnedString = "Attack with a spell";
                break;
            case tutorialStep.useBarrier:
                returnedString = "Use a barrier";
                break;
            case tutorialStep.useItem:
                returnedString = "Use an item";
                break;
            case tutorialStep.allocate:
                returnedString = "Allocate your Action Points";
                break;
            case tutorialStep.defeatEnemy:
                returnedString = "Defeat the enemy";
                break;
            case tutorialStep.hackGlyph:
                returnedString = "Hack the Glyph";
                break;
            default:
                Debug.Log("Couldn't find tutorial text");
                break;
        }

        return returnedString;
    }

    public static string GetHelpText(int helpnum)
    {
        string returnedString = "";

        switch (helpnum)
        {
            case 0:
                {
                    returnedString = "Coffins; Coffins contain random skills or items for a character to use. The character that destroys the coffin will then gain that skill or item.";
                }
                break;
            case 1:
                {
                    returnedString = "Attacking; There are 3 ways of attacking: Physical Skills, Magical Spells, and Spiritual Strikes. Each use different resources and are the primary way to deal damage to enemies and objects.";
                }
                break;
            case 2:
                {
                    returnedString = "Crippling; The 'Crippled' status is applied for 1 turn when a character takes 'Crippling' or 'Leathal' damage (As noted by the character's weakness chart in the bottom left). While crippled the character deals half damage and takes double the damage they would take in addtion to only being able to move 1 tile.";
                }
                break;
            case 3:
                {
                    returnedString = "Guarding; Choosing to 'Guard Charge' will use up all remaining actions for a character and put them in a Guard state. While guarding: all damage is halved and that character cannot be crippled. Also using 'Guard Charge' will increase the Fatigue meter based on how many actions the character had left before using.";
                }
                break;
            case 4:
                {
                    returnedString = "Movement Tiles; While standing on a movement tile, all attacks effects will be overwritten with that movement effect. A swap tile for example will prevent a fire move from burning but will exchange places with target regardless of range. Standing on a movement tile will give the option for a pro tip with more details.";
                }
                break;
            case 5:
                {
                    returnedString = "Glyphs; Glyph technology is the latest advancemenet in home security. Though Glyphs cannot move, they have many features that will be a hindrance to intruders. Movement Glyphs for example restrict movement, preventing intruders from getting in or out too quickly. ";
                }
                break;
            case 6:
                {
                    returnedString = "Control Glyphs; Control Glyphs are a standard for keeping intruders from accessing your valuables. Unless hacked or destroyed, Control Glyphs are able to remove entire parts of your home from being accessible to malicious attackers.";
                }
                break;
            case 13:
                {
                    returnedString = "Movement Glyphs; Movement Glyphs can slow intruders down and cause them to move only 1 tile at a time! However if someone destroys it or hacks it by matching its genetic code sequence, then it will cease to function.";
                }
                break;
            case 14:
                {
                    returnedString = "Swap Tiles; Swap Tiles are a type of movement tile that causes the attacker to echange places with the target of their attack. The swap does not occur if the attack misses or an item is used.";
                }
                break;
            case 18:
                {
                    returnedString = "Knockback Tiles; Knockback Tiles are a type of movement tile that causes the attacker to push the target back 1 tile  away from them regardless of range. The knockback does not occur if the attack misses or an item is used.";
                }
                break;
            case 19:
                {
                    returnedString = "Pullback Tiles; Pullin Tiles are a type of movement tile that causes the attacker pull their target 1 tile closer to them. The pull does not occur if the attack misses or an item is used.";
                }
                break;
            case 20:
                {
                    returnedString = "Reposition Tiles; Reposition Tiles are a type of movement tile that causes the attacker to vault over a target in front of them. The vault  does not occur if the attack misses or an item is used.";
                }
                break;
            case 7:
                {
                    returnedString = "Scorpiees; Thanks to the Great Cataclysm, Bees and Scorpions have now fused into the Scorpiee: a tough inscect that now have a 50% chance to resist fire or be weak to it! To think back in the day people thought roaches would be the bugs to survive anything!";
                }
                break;
            case 8:
                {
                    returnedString = "Ankhs; Ankhs are ancient artifacts based on the Egyptian word for 'life'. At the price of their own destruction, they hold the power to change someone's expertise. Usually this is done in an even exchange but if there is nothing to lose, the Ankh will grant power to those in need anyway.";
                }
                break;
            case 9:
                {
                    returnedString = "Exp Glyphs; Despite their name, Exp Glyphs don't give out any exp. In Fact they have the uncanny ability to prevent exp growth of any kind while active.";
                }
                break;
            case 10:
                {
                    returnedString = "Hacking; Glyph Technology is relatively new and while they are hard to destroy by attacking, they are highly subseptible to hacking if you can get close enough. By putting in the self destruct sequence you may even be able to find a Glyph Core which some may find useful. ";
                }
                break;
            case 11:
                {
                    returnedString = "Movement; Characters can move freely while there are no enemies in the room, otherwise their movement space is reduced based on the character.";
                }
                break;
            case 12:
                {
                    returnedString = "Action Points; At the begining of the phase 2 Action Points are distributed to each of the characters. Attacking, Moving, using items, and other things take 1 action point. Additional action points are given for every 10 speed a character has. If there are no enemies in the room however, no action points are used. ";
                }
                break;
            case 15:
                {
                    returnedString = "Extra Action Points; Choosing either Wait or Guard as the only action for a character's turn will grant that character +2 action points for the next phase. Since action points are distributed at the begining of the phase, this will not stack with itself. ";
                }
                break;
            case 16:
                {
                    returnedString = "Barriers; Summoning a Barrier will change the resistances of the character that summonned it for a set period of time. They also give bonus Def/Res/Spd stats while active. However, a barrier can only take damage up to 40% of the user's max Health which is showcased by the barrier's 'Strength'. ";
                }
                break;
            case 17:
                {
                    returnedString = "Weaknesses and Resistances; There are 4 kinds of weaknesses and 4 kinds of resistances to elements. As noted by a characters weakness chart in the bottom left of the screen: weaknesses are showcased in red and can be 'Weak', 'Savage', 'Cripling', or 'Lethal' damage (which all will be explained later). Resistances are showcased in blue and can 'Resist', 'Null', 'Absorb', or 'Repel' damage.";
                }
                break;
            case 21:
                {
                    returnedString = "Lock Glyphs; Lock Glyphs are an anomaly. Instead of keeping someone out, they are designed to lock intruders in, that way authorities have long enough to come and arrest them. Or you can just never let them out. Its your choice.";
                }
                break;
            case 22:
                {
                    returnedString = "Wait & Heal; Choosing to Wait & Heal will recover Health, Mana, and lower Fatigue. It is reccommended to wait and heal before moving on to new rooms.";
                }
                break;
            default:
                break;
        }

        return returnedString;
    }
    public static EventDetails GetEventText(int eventnum, LivingObject living)
    {
        eventdetail.eventTitle = "";
        eventdetail.eventText = "";
        eventdetail.choice1 = "";
        eventdetail.choice2 = "";
        eventdetail.affectedObject = null;
        eventdetail.eventNum = -1;
        switch (eventnum)
        {
            case -1:
                {
                    eventdetail.eventTitle = "Help Event";
                    eventdetail.eventText = "You shoul not see this text";
                    eventdetail.choice1 = "Ok";
                    eventdetail.choice2 = "Who Cares";
                    eventdetail.affectedObject = living;
                    eventdetail.eventNum = 1;
                }
                break;
            case 1:
                {
                    eventdetail.eventTitle = "Exchange Event";
                    eventdetail.eventText = "Will you give up Strength in return for magic?";
                    eventdetail.choice1 = "Yes";
                    eventdetail.choice2 = "No";
                    eventdetail.affectedObject = living;
                    eventdetail.eventNum = 1;
                }
                break;

            case 2:
                {
                    eventdetail.eventTitle = "Exchange Event";
                    eventdetail.eventText = "Will you give up magic in return for strength?";
                    eventdetail.choice1 = "Yes";
                    eventdetail.choice2 = "No";
                    eventdetail.affectedObject = living;
                    eventdetail.eventNum = 2;
                }
                break;
            case 3:
                {
                    eventdetail.eventTitle = "Reset Event";
                    eventdetail.eventText = "Do you want to reset the maps?";
                    eventdetail.choice1 = "Yes";
                    eventdetail.choice2 = "No";
                    eventdetail.affectedObject = living;
                    eventdetail.eventNum = 3;
                }
                break;

            default:
                {
                    eventdetail.eventTitle = "Random Event";
                    eventdetail.eventText = "????";
                    eventdetail.choice1 = "Yes";
                    eventdetail.choice2 = "No";
                    eventdetail.affectedObject = living;
                    eventdetail.eventNum = Random.Range(1, 3);
                }
                break;
        }

        return eventdetail;
    }

    public static ModifiedStat BuffToModStat(BuffType buff)
    {
        ModifiedStat mod = ModifiedStat.none;

        switch (buff)
        {

            case BuffType.Str:
                mod = ModifiedStat.Str;
                break;
            case BuffType.Mag:
                mod = ModifiedStat.Mag;
                break;
            case BuffType.Def:
                mod = ModifiedStat.Def;
                break;
            case BuffType.Res:
                mod = ModifiedStat.Res;
                break;
            case BuffType.Spd:
                mod = ModifiedStat.Str;
                break;
            case BuffType.Dex:
                mod = ModifiedStat.Dex;
                break;
            case BuffType.attack:
                mod = ModifiedStat.Atk;
                break;
            case BuffType.guard:
                mod = ModifiedStat.Guard;
                break;

            case BuffType.all:
                mod = ModifiedStat.all;
                break;
        }

        return mod;
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
            case EPType.skillful:
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
                                return EPType.skillful;
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
                                return EPType.forceful;
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
                            return EPType.forceful;
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
    public static StatusIcon EffectToIcon(SideEffect effect)
    {
        switch (effect)
        {

            case SideEffect.poison:
                return StatusIcon.Poison;
                break;
            case SideEffect.confusion:
                return StatusIcon.Confuse;
                break;
            case SideEffect.paralyze:
                return StatusIcon.Paralyze;
                break;
            case SideEffect.sleep:
                return StatusIcon.Sleep;
                break;
            case SideEffect.freeze:
                return StatusIcon.Freeze;
                break;
            case SideEffect.burn:
                return StatusIcon.Burn;
                break;
            case SideEffect.bleed:
                return StatusIcon.Bleed;
                break;

            case SideEffect.cripple:
                return StatusIcon.Crippled;
                break;
        }
        return StatusIcon.Bleed;
    }
    public static StatusIcon BuffToIcon(BuffType effect, bool debuff = false)
    {
        if (debuff == true)
        {
            switch (effect)
            {
                case BuffType.Str:
                    return StatusIcon.AtkDown;
                    break;
                case BuffType.Mag:
                    return StatusIcon.MagDown;
                    break;
                case BuffType.Def:
                    return StatusIcon.DefDown;
                    break;
                case BuffType.Res:
                    return StatusIcon.ResDown;
                    break;
                case BuffType.Spd:
                    return StatusIcon.SpdDown;
                    break;
                case BuffType.Dex:
                    return StatusIcon.SklDown;
                    break;

            }

        }
        else
        {

            switch (effect)
            {
                case BuffType.Str:
                    return StatusIcon.AtkUP;
                    break;
                case BuffType.Mag:
                    return StatusIcon.MagUp;
                    break;
                case BuffType.Def:
                    return StatusIcon.DefUP;
                    break;
                case BuffType.Res:
                    return StatusIcon.ResUp;
                    break;
                case BuffType.Spd:
                    return StatusIcon.SpdUp;
                    break;
                case BuffType.Dex:
                    return StatusIcon.SklUp;
                    break;

            }
        }
        return StatusIcon.Bleed;
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


    public static Reaction SpecialTileToReaction(TileScript tile)
    {

        switch (tile.TTYPE)
        {
            case TileType.knockback:
                return Reaction.knockback;
                break;
            case TileType.pullin:
                return Reaction.pullin;
                break;
            case TileType.swap:
                return Reaction.Swap;
                break;
            case TileType.reposition:
                return Reaction.reposition;
                break;

        }

        return Reaction.none;
    }
    public static bool isOverrideTile(TileScript tile)
    {
        bool overrider = false;

        switch (tile.TTYPE)
        {
            case TileType.knockback:
                overrider = true;
                break;
            case TileType.pullin:
                overrider = true;
                break;
            case TileType.swap:
                overrider = true;
                break;
            case TileType.reposition:
                overrider = true;
                break;
        }

        return overrider;
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
        detail.specialiles.Clear();
        detail.specialExtra.Clear();
        detail.tileIndexes.Clear();

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

        detail.specialiles.AddRange(data.specialiles);
        detail.specialExtra.AddRange(data.specialExtra);
        detail.tilesInShadow.AddRange(data.tilesInShadow);
        detail.tileIndexes.AddRange(data.specialTileIndexes);

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
        detail.specialiles.Clear();
        detail.tilesInShadow.Clear();
        detail.specialExtra.Clear();
        detail.specialTileIndexes.Clear();

        //    if (data.hazardIds.Count > 0)
        {
            detail.glyphIds.Clear();
            detail.glyphIds.AddRange(data.hazardIds);
        }
        //  if (data.unOccupiedIndexes.Count > 0)
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

        detail.specialiles.AddRange(data.specialiles);
        detail.specialExtra.AddRange(data.specialExtra);
        detail.tilesInShadow.AddRange(data.tilesInShadow);
        detail.specialTileIndexes.AddRange(data.tileIndexes);

        detail.StartingPosition = data.StartingPosition;

        return detail;
    }
}
