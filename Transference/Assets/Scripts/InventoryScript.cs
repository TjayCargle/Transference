using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
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
    List<CommandSkill> activeDeBuffs;
    [SerializeField]
    List<OppSkill> oppSkills;

    [SerializeField]
    List<WeaponScript> weapons;

    [SerializeField]
    List<ArmorScript> armor;


    [SerializeField]
    List<ItemScript> items;

    [SerializeField]
    List<EffectScript> effects;

    [SerializeField]
    List<CommandSkill> commandSpells;

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

    public List<CommandSkill> CSKILLS
    {
        get { return commandSkills; }
        set { commandSkills = value; }
    }
    //public List<CommandSkill> CSPELLS
    //{
    //    get { return commandSkills; }
    //    set { commandSkills = value; }
    //}
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
    public List<CommandSkill> DEBUFFS
    {
        get { return activeDeBuffs; }
        set { activeDeBuffs = value; }
    }

    public List<EffectScript> EFFECTS
    {
        get { return effects; }
        set { effects = value; }
    }
    public List<OppSkill> OPPS
    {
        get { return oppSkills; }
        set { oppSkills = value; }
    }

    public List<ItemScript> ITEMS
    {
        get { return items; }
        set { items = value; }
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
            if (CSKILLS[i].NAME == name)
            {
                return CSKILLS[i];
            }
        }
        return null;
    }

    public CommandSkill ContainsCommandIndex(int index)
    {
        for (int i = 0; i < commandSkills.Count; i++)
        {
            if (CSKILLS[i].INDEX == index)
            {
                return CSKILLS[i];
            }
        }
        return null;
    }
    public bool ContainsUsableIndex(UsableScript useable)
    {
  
        if (useable.GetType() == typeof(CommandSkill))
        {
            if(ContainsCommandIndex(useable.INDEX) != null)
            {
                return true;
            }
        }

        if (useable.GetType() == typeof(PassiveSkill))
        {
            if (ContainsPassiveIndex(useable.INDEX) != null)
            {
                return true;
            }
        }

        if (useable.GetType() == typeof(AutoSkill))
        {
            if (ContainsAutoIndex(useable.INDEX) != null)
            {
                return true;
            }
        }

        if (useable.GetType() == typeof(OppSkill))
        {
            if (ContainsOppIndex(useable.INDEX) != null)
            {
                return true;
            }
        }

        if (useable.GetType() == typeof(OppSkill))
        {
            if (ContainsOppIndex(useable.INDEX) != null)
            {
                return true;
            }
        }

        if (useable.GetType() == typeof(OppSkill))
        {
            if (ContainsOppIndex(useable.INDEX) != null)
            {
                return true;
            }
        }


        if (useable.GetType() == typeof(WeaponScript))
        {
            if (ContainsWeaponIndex(useable.INDEX) != null)
            {
                return true;
            }
        }


        if (useable.GetType() == typeof(ArmorScript))
        {
            if (ContainsArmorIndex(useable.INDEX) != null)
            {
                return true;
            }
        }
        return false;
    }

    public AutoSkill ContainsAutoIndex(int index)
    {
        for (int i = 0; i < autoSkills.Count; i++)
        {
            if (AUTOS[i].INDEX == index)
            {
                return AUTOS[i];
            }
        }
        return null;
    }

    public WeaponScript ContainsWeaponIndex(int index)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].INDEX == index)
            {
                return WEAPONS[i];
            }
        }
        return null;
    }

    public ArmorScript ContainsArmorIndex(int index)
    {
        for (int i = 0; i < armor.Count; i++)
        {
            if (armor[i].INDEX == index)
            {
                return ARMOR[i];
            }
        }
        return null;
    }

    public PassiveSkill ContainsPassiveIndex(int index)
    {
        for (int i = 0; i < passives.Count; i++)
        {
            if (PASSIVES[i].INDEX == index)
            {
                return PASSIVES[i];
            }
        }
        return null;
    }

    public OppSkill ContainsOppIndex(int index)
    {
        for (int i = 0; i < oppSkills.Count; i++)
        {
            if (OPPS[i].INDEX == index)
            {
                return OPPS[i];
            }
        }
        return null;
    }

    public void ChargeShields()
    {
        for (int i = 0; i < ARMOR.Count; i++)
        {
            if (ARMOR[i].HEALTH < ARMOR[i].MAX_HEALTH)
            {

                ARMOR[i].HEALTH += (0.20f * armor[i].MAX_HEALTH);



            }

        }
    }

    public void Clear()
    {
        this.ARMOR.Clear();
        this.AUTOS.Clear();
        this.BUFFS.Clear();
        this.DEBUFFS.Clear();
        this.CSKILLS.Clear();
        //this.CSPELLS.Clear();
        this.EFFECTS.Clear();
        this.ITEMS.Clear();
        this.OPPS.Clear();
        this.PASSIVES.Clear();
        this.SKILLS.Clear();
        this.WEAPONS.Clear();
        this.USEABLES.Clear();
      
    }
}
