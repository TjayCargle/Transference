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
    public int ATTACK
    {
        get { return STATS.ATTACK + BASE_STATS.ATTACK; }
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
    public int HEALTH
    {
        get { return STATS.HEALTH + BASE_STATS.HEALTH; }
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
        }



        base.Setup();
    }

    public void ApplyPassives(LivingObject invokingObj)
    {
        List<SkillScript> atkPassives = invokingObj.GetComponent<InventoryScript>().PASSIVES;
        StatScript modStats = invokingObj.STATS;
        modStats.MODS.Clear();
        modStats.Reset();
        if (atkPassives.Count > 0)
        {
            for (int i = 0; i < atkPassives.Count; i++)
            {
                switch (atkPassives[i].ModStat)
                {
                    case ModifiedStat.Health:
                        modStats.HEALTH += (int)atkPassives[i].ModValues[0];
                        break;
                    case ModifiedStat.ElementDmg:
                        break;
                    case ModifiedStat.Movement:
                        modStats.MOVE_DIST += (int)atkPassives[i].ModValues[0];
                        break;
                    case ModifiedStat.Atk:
                        modStats.ATTACK +=(int)atkPassives[i].ModValues[0];
                        break;
                    case ModifiedStat.Def:
                        modStats.DEFENSE += (int)atkPassives[i].ModValues[0];
                        break;
                    case ModifiedStat.Res:

                        modStats.RESIESTANCE += (int)atkPassives[i].ModValues[0];
                        break;
                    case ModifiedStat.Speed:

                        modStats.SPEED += (int)atkPassives[i].ModValues[0];
                        break;
                    case ModifiedStat.Luck:

                        modStats.LUCK += (int)atkPassives[i].ModValues[0];
                        break;
                    default:
                        break;
                }
            }


        }

    }

}
