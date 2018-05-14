using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour {
    [SerializeField]
    List<UsableScript> usables;
    [SerializeField]
    List<SkillScript> skills;
    [SerializeField]
    List<SkillScript> passives;
    [SerializeField]
    List<WeaponScript> weapons;
    [SerializeField]
    List<ArmorScript> armor;
    [SerializeField]
    List<AccessoryScript> accessories;


    public List<UsableScript> USEABLES
    {
        get { return usables; }
        set { usables = value; }
    }
    public List<WeaponScript> WEAPONS
    {
        get { return weapons; }
        set { weapons = value; }
    }
    public List<ArmorScript> ARMOR
    {
        get { return armor; }
        set { armor = value; }
    }
    public List<AccessoryScript> ACCESSORIES
    {
        get { return accessories; }
        set { accessories = value; }
    }
    public List<SkillScript> SKILLS
    {
        get { return skills; }
        set { skills = value; }
    }
    public List<SkillScript> PASSIVES
    {
        get { return passives; }
        set { passives = value; }
    }


}
