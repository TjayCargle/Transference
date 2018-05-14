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
    ElementDmg,
    Movement,
    Atk,
    Def,
    Res,
    Speed,
    Luck

}
public enum RanngeType
{
    single,
    multi,
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
    Passive

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
    snatched
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
    fatiguePotion
}
public enum ItemTarget
{
    self,
    ally,
    any,
    enemy,
    enemies
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
    poisoned,
    stunned,
    sleep,
    frozen,
    burned,
    slow,
    rage,
    charm,
    confusion
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
public class Common : ScriptableObject {

	
}
