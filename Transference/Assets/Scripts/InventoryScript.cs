using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour {
    [SerializeField]
    List<UsableScript> usables;

    [SerializeField]
    List<SkillScript> skills;

    [SerializeField]
    List<CommandSkill> commandSkills;

    [SerializeField]
    List<AutoSkill> autoSkills;

    [SerializeField]
    List<PassiveSkill> passives;

    [SerializeField]
    List<CommandSkill> activeBuffs;

    [SerializeField]
    List<OppSkill> oppSkills;

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
    public List<SkillScript> SKILLS
    {
        get { return skills; }
        set { skills = value; }
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
    public List<CommandSkill> CSKILLS
    {
        get { return commandSkills; }
        set { commandSkills = value; }
    }
    public List<AutoSkill> AUTOS
    {
        get { return autoSkills; }
        set { autoSkills = value; }
    }
    public List<PassiveSkill> PASSIVES
    {
        get { return passives; }
        set { passives = value; }
    }

    public List<CommandSkill> BUFFS
    {
        get { return activeBuffs; }
        set { activeBuffs = value; }
    }

    public List<OppSkill> OPPS
    {
        get { return oppSkills; }
        set { oppSkills = value; }
    }
    
    public SkillScript ContainsSkillName(string name)
    {
        for (int i = 0; i < commandSkills.Count; i++)
        {
            if (CSKILLS[i].NAME == name)
            {
                return CSKILLS[i];
            }

        }

        for (int i = 0; i < passives.Count; i++)
        {
            if (PASSIVES[i].NAME == name)
            {
                return PASSIVES[i];
            }
        }

        for (int i = 0; i < autoSkills.Count; i++)
        {
            if (AUTOS[i].NAME == name)
            {
                return AUTOS[i];
            }
        }

        for (int i = 0; i < oppSkills.Count; i++)
        {
            if (OPPS[i].NAME == name)
            {
                return OPPS[i];
            }
        }
        return null;
    }

    public SkillScript ContainsCommandNames(string name)
    {
        for (int i = 0; i < commandSkills.Count; i++)
        {
            if(CSKILLS[i].NAME == name)
            {
                return CSKILLS[i];
            }
        }
        return null;
    }

    public AutoSkill ContainsAutoName(string name)
    {
        for (int i = 0; i < autoSkills.Count; i++)
        {
            if (AUTOS[i].NAME == name)
            {
                return AUTOS[i];
            }
        }
        return null;
    }

    public PassiveSkill ContainsPassiveName(string name)
    {
        for (int i = 0; i < passives.Count; i++)
        {
            if (PASSIVES[i].NAME == name)
            {
                return PASSIVES[i];
            }
        }
        return null;
    }

    public OppSkill ContainsOppName(string name)
    {
        for (int i = 0; i < oppSkills.Count; i++)
        {
            if (OPPS[i].NAME == name)
            {
                return OPPS[i];
            }
        }
        return null;
    }
}
