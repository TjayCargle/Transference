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
public enum Status
{
    normal,
    crippled,
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
public class Common : ScriptableObject {

	
}
