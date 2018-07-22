﻿using System.Collections;
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
    normal,
    resists,
    nulls,
    reflects,
    absorbs,

    weak,
    snatched,
    cripples,
    knocked

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
    dmg

}
public enum RanngeType
{
    single,
    multi,
    area,
    any
}

public enum TargetType
{
    enemy,
    ally,
    alliesInRange,
    EnemiesInRange,
    any
}

public enum BuffType
{
    none,
    attack,
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

    Neutral,
    Passive,
    Buff,
    Opp,
    Ailment,
    Auto,
    Heal

}
public enum EType
{
    physical,
    magical,
}
public enum Reaction
{
    none,
    statDrop,
    nulled,
    reflected,
    knockback,
    snatched,
    reduceAtk,
    reduceDef,
    reduceSpd,
    reduceMag,
    reduceRes,
    reduceLuck
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
    atk

}
public enum ItemTarget
{
    self,
    ally,
    any,
    enemy,
    enemies
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
    equipSkill,
    selectBS,
    selectAS,
    selectPS,
    selectOS,
    equipOS
}
public enum AutoAct
{
    beforeDmg,
    afterDmg,
    afterKilling,

}

public enum AutoReact
{
    healByDmg,
    healAmount,
    extraAction,
    recoverSP,
    reduceFT,
    reduceDef,
    reduceAtk,
    reduceSpd,
    reduceMag,
    reduceRes,
    reduceLuck

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
    poisoned,
    confusion
}

public enum StatusEffect
{
    none,
    paralyzed,
    sleep,
    frozen,
    burned,

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
}

public struct DmgReaction
{
    public int damage;
    public Reaction reaction;
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
    CmdSkills
}
public struct menuStackEntry
{
    public State state;
    public int index;
    public currentMenu menu;
}
public delegate bool RunableEvent(Object data);

public struct GridEvent
{
    public string name;
    public Object caller;
    public Object data;
    public bool isRunning;
    RunableEvent runable;

    public RunableEvent RUNABLE
    {
        get { return runable; }

        set { runable = value; }
    }
}

public class Common : ScriptableObject
{


}
