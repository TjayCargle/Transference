using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingObject : GridObject
{
    private StatScript myStats;
    private WeaponEquip equippedWeapon;
    private ArmorEquip equipedArmor;
    private AccessoryEquip equippedAccessory;
    private List<Element> myWeaknesses;
    public StatScript STATS
    {
        get { return myStats; }
        set { myStats = value; }
    }
    public WeaponEquip WEAPON
    {
        get { return equippedWeapon ; }
        set { equippedWeapon = value; }
    }
    public ArmorEquip ARMOR
    {
        get { return equipedArmor; }
        set { equipedArmor = value; }
    }
    public AccessoryEquip ACCESSORY
    {
        get { return equippedAccessory; }
        set { equippedAccessory = value; }
    }
    public List<Element> WEAKNESSES
    {
        get { return myWeaknesses; }
    }

    public override int MOVE_DIST
    {
        get { return (base.MOVE_DIST = STATS.MOVE_DIST);}
        set { STATS.MOVE_DIST = value; base.MOVE_DIST = value; }
    }


    public int Max_Atk_DIST
    {
        get { return STATS.Max_Atk_DIST; }
    }
    public int Min_Atk_DIST
    {
        get { return STATS.Min_Atk_DIST; }
    }
    public int ATTACK
    {
        get { return STATS.ATTACK; }
    }
    public int DEFENSE
    {
        get { return STATS.DEFENSE; }
    }
    public int RESIESTANCE
    {
        get { return STATS.RESIESTANCE; }
    }
    public int SPEED
    {
        get { return STATS.SPEED; }
    }
    public int LUCK
    {
        get { return STATS.LUCK; }
    }
    public int HEALTH
    {
        get { return STATS.HEALTH; }
    }
    public int LEVEL
    {
        get { return STATS.LEVEL; }
    }
    public override void Setup()
    {
        if (!GetComponent<InventoryScript>())
        {
            gameObject.AddComponent<InventoryScript>();
        }
        if (!GetComponent<WeaponEquip>())
        {
            gameObject.AddComponent<WeaponEquip>();
            gameObject.GetComponent<WeaponEquip>().NAME = "default weapon";
        }
        equippedWeapon = GetComponent<WeaponEquip>();
        equippedWeapon.USER = this;
        if (!GetComponent<ArmorEquip>())
        {
            gameObject.AddComponent<ArmorEquip>();
            gameObject.GetComponent<ArmorEquip>().NAME = "default armor";

        }
        equipedArmor = GetComponent<ArmorEquip>();
        equipedArmor.USER = this;

        if (!GetComponent<AccessoryEquip>())
        {
            gameObject.AddComponent<AccessoryEquip>();
            gameObject.GetComponent<AccessoryEquip>().NAME = "default accessory";

        }
        equippedAccessory = GetComponent<AccessoryEquip>();
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
