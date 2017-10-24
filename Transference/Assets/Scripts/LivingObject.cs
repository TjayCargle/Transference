using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingObject : GridObject
{
    private StatScript myStats;
    private WeaponScript equippedWeapon;
    private ArmorScript equipedArmor;
    private AccessoryScript equippedAccessory;
    public StatScript STATS
    {
        get { return myStats; }
        set { myStats = value; }
    }
    public WeaponScript WEAPON
    {
        get { return equippedWeapon ; }
        set { equippedWeapon = value; }
    }
    public ArmorScript ARMOR
    {
        get { return equipedArmor; }
        set { equipedArmor = value; }
    }
    public AccessoryScript ACCESSORY
    {
        get { return equippedAccessory; }
        set { equippedAccessory = value; }
    }

    public override int MOVE_DIST
    {
        get { return (base.MOVE_DIST = STATS.MOVE_DIST);}
        set { STATS.MOVE_DIST = value; base.MOVE_DIST = value; }
    }

    public override int MIN_ATK_DIST
    {
        get { return STATS.Min_Atk_DIST; }
        set { STATS.Min_Atk_DIST = value; }
    }
    public override int MAX_ATK_DIST
    {
        get { return STATS.Max_Atk_DIST; }
        set { STATS.Max_Atk_DIST = value; }
    }
    protected override void Setup()
    {
       
        if (!GetComponent<WeaponScript>())
        {
            gameObject.AddComponent<WeaponScript>();
        }
        equippedWeapon = GetComponent<WeaponScript>();
        equippedWeapon.USER = this;
        if (!GetComponent<ArmorScript>())
        {
            gameObject.AddComponent<ArmorScript>();
        }
        equipedArmor = GetComponent<ArmorScript>();
        equipedArmor.USER = this;
        if (!GetComponent<AccessoryScript>())
        {
            gameObject.AddComponent<AccessoryScript>();
        }
        equippedAccessory = GetComponent<AccessoryScript>();
        equippedAccessory.USER = this;
        if (!GetComponent<StatScript>())
        {
            gameObject.AddComponent<StatScript>();
        }
        myStats = GetComponent<StatScript>();
        myStats.USER = this;
        base.Setup();
    }

 
}
