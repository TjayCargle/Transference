using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatScript : MonoBehaviour {
    private int myLevel = 1;
    [SerializeField]
    private int myHealth = 100;
    [SerializeField]
    private int myBaseAttack = 1;
    [SerializeField]
    private int myBaseDefense = 1;
    [SerializeField]
    private int myBaseSpeed = 1;
    [SerializeField]
    private int myBaseLuck = 1;
    [SerializeField]
    private int myMoveDist = 0;
    [SerializeField]
    private int myMaxAtkDist = 0;
    [SerializeField]
    private int myMinAtkDist = 0;

    private LivingObject owner;
    public LivingObject USER
    {
        get { return owner; }
        set { owner = value; }
    }
    public int MOVE_DIST
    {
        get { return owner.ACCESSORY.STAT == Stat.Movement? owner.ACCESSORY.VALUE + myMoveDist : myMoveDist; }
        set { myMoveDist = value; }
    }
    public int Max_Atk_DIST
    {
        get { return owner.ACCESSORY.STAT == Stat.Atk_Dist ? owner.ACCESSORY.VALUE + myMaxAtkDist + owner.WEAPON.DIST : myMaxAtkDist; }
        set { myMoveDist = value; }
    }
    public int Min_Atk_DIST
    {
        get { return owner.ACCESSORY.STAT == Stat.Atk_Dist ? owner.ACCESSORY.VALUE + myMinAtkDist + owner.WEAPON.DIST : myMinAtkDist; }
        set { myMoveDist = value; }
    }
    public int ATTACK
    {
        get { return owner.ACCESSORY.STAT == Stat.Attack ? owner.ACCESSORY.VALUE + myBaseAttack + owner.WEAPON.ATTACK : myBaseAttack + owner.WEAPON.ATTACK; }
        set { myBaseAttack = value; }
    }
    public int DEFENSE
    {
        get { return owner.ACCESSORY.STAT == Stat.Defense ? owner.ACCESSORY.VALUE + myBaseDefense + owner.ARMOR.DEFENSE : myBaseDefense + owner.ARMOR.DEFENSE; }
        set { myBaseDefense = value; }
    }
    public int SPEED
    {
        get { return owner.ACCESSORY.STAT == Stat.Speed ? owner.ACCESSORY.VALUE + myBaseSpeed + owner.ARMOR.SPEED : myBaseSpeed + owner.ARMOR.SPEED; }
        set { myBaseSpeed = value; }
    }
    public int LUCK
    {
        get { return owner.ACCESSORY.STAT == Stat.Luck ? owner.ACCESSORY.VALUE + myBaseLuck + owner.WEAPON.LUCK : myBaseLuck + owner.WEAPON.LUCK; }
        set { myBaseLuck = value; }
    }
    public int HEALTH
    {
        get { return myHealth; }
        set { myHealth = value; }
    }
    public int LEVEL
    {
        get { return myLevel; }
        set { myLevel = value; }
    }
}
