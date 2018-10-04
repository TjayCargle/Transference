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
    EnemyTurn


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
    Luck,
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
    anyarea
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
    attack,
    str,
    mag,
    speed,
    defense,
    resistance,
    luck,
    all


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
public enum EType
{
    physical,
    magical,
}
public enum Reaction
{
    none,
    buff,
    bonusAction,
    knockback,
    snatched,
    reduceAtk,
    reduceDef,
    reduceSpd,
    reduceMag,
    reduceRes,
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
    minute = 10,
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
    extraAction,
    recoverSP,
    reduceFT,
    increaseFT,
    reduceDef,
    reduceAtk,
    reduceSpd,
    reduceMag,
    reduceRes,
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
    bleed

}

[System.Serializable]
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
public struct DmgReaction
{
    public int damage;
    public Reaction reaction;
    public string atkName;
    public Element dmgElement;
}
public struct Modification
{
    public ModifiedStat affectedStat;
    public Element affectedElement;
    public float editValue;
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
    public static Color green = new Color(0.0f, 0.693f, 0.230f);
    public static Color red = new Color(0.693f, 0.0f, 0.230f);
    public static Color semi = new Color(1.0f, 1.0f, 1.0f, 0.183f);
    public static int maxDmg = 999;
    public static List<EHitType> noAmor = new List<EHitType>();
    
}
