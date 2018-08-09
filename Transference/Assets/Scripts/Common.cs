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
    PlayerOppSelecting,
    PlayerOppOptions,
    PlayerOppMove,
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
    knocked, // dmg x2 + lose a turn
    cripples, // dmg x4 + stats halved
    leathal // dmg x4 + lose a turn + stats halved

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
    knockback,
    snatched,
    reduceAtk,
    reduceDef,
    reduceSpd,
    reduceMag,
    reduceRes,
    reduceLuck,
    cripple,
    nulled,
    reflected,
    absorb
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
    equipOS,
    generated
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
    OppMove
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
public delegate bool RunableEvent(Object data);
public delegate void StartupEvent();
public delegate void StartupWResourcesEvent(Object data);

public enum descState
{
    stats,
    skills,
    equipped,
    affinities,
    status,
    none
}
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
public enum STATICVAR
{
    minActions = 1,
    maxActions = 5
}

public class test
{
    public string name;
    public static bool Check(test one, test two)
    {
        if (!System.Object.Equals(one, null))
        {
            //one is not null
            if (!System.Object.Equals(two, null))
            {
                // two is not null, so now compare names.
                return one.name.Equals(two.name);
            }
        }
        else if (System.Object.Equals(two, null))
        {
            //both objects are null so return true;
            return true;
        }
        //one object is null and the other isn't, return false
        return false;
    }
    public override bool Equals(object obj)
    {
        if (obj != null)
        {

            if (obj.GetType() == typeof(test))
            {
                return Check(this, obj as test);
            }
            else
                return base.Equals(obj);
        }
        else
            return base.Equals(obj);
    }
    public static bool operator ==(test one, test two)
    {
        return Check(one, two);
    }
    public static bool operator !=(test one, test two)
    {
        return Check(one, two);
    }
}


public class Common : ScriptableObject
{

}
