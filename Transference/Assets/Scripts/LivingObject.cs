using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingObject : GridObject
{
    [SerializeField]
    private StatScript baseStats;
    [SerializeField]
    private StatScript modifiedStats;
    private WeaponEquip equippedWeapon;
    private ArmorEquip equipedArmor;
    private AccessoryEquip equippedAccessory;
    [SerializeField]
    private bool isEnenmy;
    [SerializeField]
    private PrimaryStatus pStatus = PrimaryStatus.normal;
    [SerializeField]
    private SecondaryStatus sStatus = SecondaryStatus.normal;
    [SerializeField]
    private StatusEffect eStatus = StatusEffect.none;
    [SerializeField]
    private skillSlots battleSlots;
    [SerializeField]
    private skillSlots passiveSlots;

    public PrimaryStatus PSTATUS
    {
        get { return pStatus; }
        set { pStatus = value; }
    }

    public SecondaryStatus SSTATUS
    {
        get { return sStatus; }
        set { sStatus = value; }
    }
    public StatusEffect ESTATUS
    {
        get { return eStatus; }
        set { eStatus = value; }
    }
    public skillSlots BATTLE_SLOTS
    {
        get { return battleSlots; }
        set { battleSlots = value; }
    }
    public skillSlots PASSIVE_SLOTS
    {
        get { return passiveSlots; }
        set { passiveSlots = value; }
    }
    public StatScript BASE_STATS
    {
        get { return baseStats; }
        set { baseStats = value; }
    }
    public StatScript STATS
    {
        get { return modifiedStats; }
        set { modifiedStats = value; }
    }
    public WeaponEquip WEAPON
    {
        get { return equippedWeapon; }
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

    public override int MOVE_DIST
    {
        get { return (base.MOVE_DIST = STATS.MOVE_DIST + BASE_STATS.MOVE_DIST); }
        set { STATS.MOVE_DIST = value; base.MOVE_DIST = value; }
    }


    public int Max_Atk_DIST
    {
        get { return STATS.Max_Atk_DIST + BASE_STATS.Max_Atk_DIST; }
    }
    public int Min_Atk_DIST
    {
        get { return STATS.Min_Atk_DIST + BASE_STATS.Min_Atk_DIST; }
    }
    public int STRENGTH
    {
        get { return STATS.STRENGTH + BASE_STATS.STRENGTH; }
    }
    public int MAGIC
    {
        get { return STATS.MAGIC + BASE_STATS.MAGIC; }
    }
    public int DEFENSE
    {
        get { return STATS.DEFENSE + BASE_STATS.DEFENSE; }
    }
    public int RESIESTANCE
    {
        get { return STATS.RESIESTANCE + BASE_STATS.RESIESTANCE; }
    }
    public int SPEED
    {
        get { return STATS.SPEED + BASE_STATS.SPEED; }
    }
    public int LUCK
    {
        get { return STATS.LUCK + BASE_STATS.LUCK; }
    }
    public int MAX_HEALTH
    {
        get { return STATS.MAX_HEALTH + BASE_STATS.MAX_HEALTH; }
    }
    public int MAX_MANA
    {
        get { return STATS.MAX_MANA + BASE_STATS.MAX_MANA; }
    }
    public int HEALTH
    {
        get { return STATS.HEALTH + BASE_STATS.HEALTH; }
    }
    public int MANA
    {
        get { return STATS.MANA + BASE_STATS.MANA; }
    }
    public int MAX_FATIGUE
    {
        get { return STATS.MAX_FATIGUE + BASE_STATS.MAX_FATIGUE; }
    }
    public int FATIGUE
    {
        get { return STATS.FATIGUE + BASE_STATS.FATIGUE; }
    }
    public int LEVEL
    {
        get { return STATS.LEVEL + BASE_STATS.LEVEL; }
    }

    public bool IsEnenmy
    {
        get { return isEnenmy; }
        set { isEnenmy = value; }
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
            baseStats = gameObject.AddComponent<StatScript>();
            modifiedStats = gameObject.AddComponent<StatScript>();
        baseStats.USER = this;
            modifiedStats.USER = this;
            modifiedStats.Reset(true);
        }
        else
        {
            StatScript[] statsScripts = GetComponents<StatScript>();
            if(statsScripts.Length == 1)
            {
                modifiedStats = gameObject.AddComponent<StatScript>();
                modifiedStats.type = 1;
                modifiedStats.Reset(true);
                modifiedStats.USER = this;
            }
            for (int i = 0; i < statsScripts.Length; i++)
            {
                if(statsScripts[i].type == 0)
                {
                    baseStats = statsScripts[i];
                baseStats.USER = this;
                }
                else
                {
                    modifiedStats = statsScripts[i];
                    modifiedStats.Reset(true);
                modifiedStats.USER = this;
                }
            }

            this.baseStats.HEALTH = this.baseStats.MAX_HEALTH;
            this.baseStats.MANA = this.baseStats.MAX_MANA;
        }

        if (!GetComponent<skillSlots>())
        {
            battleSlots = gameObject.AddComponent<skillSlots>();
            passiveSlots = gameObject.AddComponent<skillSlots>();
            passiveSlots.TYPE = 1;
        }
        else
        {
            skillSlots[] slots = GetComponents<skillSlots>();
            if (slots.Length == 1)
            {
                battleSlots = gameObject.AddComponent<skillSlots>();
            }
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].TYPE == 0)
                {
                    battleSlots = slots[i];
                }
                else
                {
                    passiveSlots = slots[i];
                    passiveSlots.TYPE = 1;
                }
            }

        }


        base.Setup();
    }

    public void ApplyPassives()
    {
        List<PassiveSkill> atkPassives = GetComponent<InventoryScript>().PASSIVES;
        modifiedStats.MODS.Clear();
        modifiedStats.Reset(true);

        if (atkPassives.Count > 0)
        {
            for (int i = 0; i < atkPassives.Count; i++)
            {
                float mod = 0.0f;
                switch (atkPassives[i].ModStat)
                {
                    case ModifiedStat.Health:
                        modifiedStats.MAX_HEALTH += (int)atkPassives[i].ModValues[0];
                        if(modifiedStats.HEALTH == modifiedStats.MAX_HEALTH - (int)atkPassives[i].ModValues[0])
                        {
                            modifiedStats.HEALTH = modifiedStats.MAX_HEALTH;
                        }
                        break;
                    case ModifiedStat.ElementDmg:
                        break;
                    case ModifiedStat.Movement:

                        modifiedStats.MOVE_DIST += (int)atkPassives[i].ModValues[0];
                        break;
                    case ModifiedStat.Str:
                        mod = atkPassives[i].ModValues[0];
                        mod = (mod / 100) * modifiedStats.STRENGTH;
                        modifiedStats.STRENGTH += (int)mod;
                        break;
                    case ModifiedStat.Mag:
                        mod = atkPassives[i].ModValues[0];
                        mod = (mod / 100) * modifiedStats.MAGIC;
                        modifiedStats.MAGIC += (int)mod;
                        break;
                    case ModifiedStat.Atk:
                        mod = atkPassives[i].ModValues[0];
                        mod = (mod / 100) * BASE_STATS.STRENGTH;
                        modifiedStats.STRENGTH += (int)mod;
                        mod = atkPassives[i].ModValues[0];
                        mod = (mod / 100) * BASE_STATS.MAGIC;
                        modifiedStats.MAGIC += (int)mod;
                        break;
                    case ModifiedStat.Def:
                        modifiedStats.DEFENSE += ((((int)atkPassives[i].ModValues[0] ) / 100) * ARMOR.DEFENSE);
                        break;
                    case ModifiedStat.Res:
                        modifiedStats.RESIESTANCE += ((((int)atkPassives[i].ModValues[0] ) / 100) * ARMOR.RESISTANCE);
                        break;
                    case ModifiedStat.Guard:
                        modifiedStats.RESIESTANCE += ((((int)atkPassives[i].ModValues[0]) / 100) * ARMOR.RESISTANCE);
                        break;
                    case ModifiedStat.Speed:

                        modifiedStats.SPEED += ((((int)atkPassives[i].ModValues[0] ) / 100) * ARMOR.SPEED);
                        break;
                    case ModifiedStat.Luck:

                        modifiedStats.LUCK += ((((int)atkPassives[i].ModValues[0] ) / 100) * WEAPON.LUCK);
                        break;
                    default:
                        break;
                }
               // Debug.Log("Post atk " + i + " = " + modifiedStats.STRENGTH);

            }


        }

    }

    public void Wait()
    {
        STATS.HEALTH += 2;
        STATS.MANA += 2;
        STATS.FATIGUE -= 2;
        if (HEALTH > MAX_HEALTH)
            STATS.HEALTH = MAX_HEALTH;
        if (MANA > MAX_MANA)
            STATS.MANA = MAX_MANA;
        if (FATIGUE < 0)
            STATS.FATIGUE = 0;
    }
}
