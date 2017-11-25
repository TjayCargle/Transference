using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingObject : GridObject
{
    private StatScript myStats;
    private WeaponScript equippedWeapon;
    private ArmorScript equipedArmor;
    private AccessoryScript equippedAccessory;
    private List<Element> myWeaknesses;
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
    public List<Element> WEAKNESSES
    {
        get { return myWeaknesses; }
    }

    public override int MOVE_DIST
    {
        get { return (base.MOVE_DIST = STATS.MOVE_DIST);}
        set { STATS.MOVE_DIST = value; base.MOVE_DIST = value; }
    }

  
    public override void Setup()
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
        myWeaknesses = new List<Element>();
        myWeaknesses.Add(SkillScript.getWeakness(ARMOR.AFINITY));
        myWeaknesses.Add(SkillScript.getWeakness(WEAPON.AFINITY));
        if (ACCESSORY.STAT == Stat.Affinity)
        {
            for (int i = 0; i < myWeaknesses.Count; i++)
            {

            }
        }
        base.Setup();
    }

 
}
