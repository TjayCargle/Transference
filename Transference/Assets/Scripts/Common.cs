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
    HazardTurn


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
    none,
    all

}
public enum RanngeType
{
    single,
    multi,
    area,
    any,
    anyarea,
    multiarea
}

public enum TargetType
{
    self,
    ally,
    allyAndSelf,
    enemy,
    alliesInRange,
    EnemiesInRange,
    any
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
public enum PhysType
{
    charge,
    cost
}
public enum SubSkillType
{
    Attack,
    Buff,
    Debuff,
    Ailment,
    Heal,
    Revive,
    RngAtk,
    RngSupp,
    None
}
public enum Augment
{
    damageAugment,
    accurracyAugment,
    sideEffectAugment,
    rangeAument,
    attackCountAugment,
    costAugment,
    chargeIncreaseAugment,
    chargeDecreaseAugment,
    buffAugment,
    autoAugment,
    oppAugment,
    randAugment

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
    Heal,
    Passive,
    Opp,
    Ailment,
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
    exp
}
public enum EType
{
    physical,
    magical,
}
public enum Reaction
{
    none,
    buff,
    debuff,
    bonusAction,
    knockback,
    snatched,
    reduceStr,
    reduceDef,
    reduceMag,
    reduceRes,
    reduceSpd,
    reduceLuck,
    turnloss,
    cripple,
    turnAndcrip,
    weak,
    nulled,
    reflected,
    absorb,
    missed
}
public enum DMG
{
    tiny =   10,
    small =    20,
    medium =   40,
    heavy =    80,
    severe =   160,
    collassal = 320
}
public enum ItemType
{
    healthPotion,
    manaPotion,
    fatiguePotion,
    cure,
    buff,
    dmg
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
    selectAct
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
    bleeding

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
    debuff

}
public class MassAtkConatiner : ScriptableObject
{
    public List<AtkConatiner> atkConatiners;
}
public class AtkConatiner : ScriptableObject
{
    public LivingObject attackingObject;
    public LivingObject dmgObject;
    public Element attackingElement;
    public EType attackType;
    public int dmg;
    public Reaction alteration;
    public CommandSkill command;

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
    public List<int> enemyIndexes;
    public List<int> hazardIndexes;
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
    public static Color red = new Color(0.693f, 0.0f, 0.230f);
    public static Color semi = new Color(1.0f, 1.0f, 1.0f, 0.183f);
    public static Color trans = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    public static BoolConatainer container = new BoolConatainer();

    public static int maxDmg = 999;
    public static List<EHitType> noAmor = new List<EHitType>()
    {
        EHitType.normal,
        EHitType.normal,
        EHitType.normal,
        EHitType.normal,
        EHitType.normal,
        EHitType.normal,
        EHitType.normal
    };
    
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
}
