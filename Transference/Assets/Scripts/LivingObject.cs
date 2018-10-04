using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingObject : GridObject
{
    [SerializeField]
    private BaseStats baseStats;
    [SerializeField]
    private ModifiedStats modifiedStats;
    private WeaponEquip equippedWeapon;
    private ArmorEquip equipedArmor;
    private AccessoryEquip equippedAccessory;
    [SerializeField]
    private bool isEnenmy;
    [SerializeField]
    private int actions;
    [SerializeField]
    private int generatedActions;
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
    [SerializeField]
    private skillSlots oppSlots;
    [SerializeField]
    private skillSlots autoSlots;
    [SerializeField]
    private bool isDead;
    [SerializeField]
    private InventoryScript inventory;

    public InventoryScript INVENTORY
    {
        get { return inventory; }
        set { inventory = value; }
    }
    public bool DEAD
    {
        get { return isDead; }
        set { isDead = value; }
    }
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

    public skillSlots OPP_SLOTS
    {
        get { return oppSlots; }
        set { oppSlots = value; }
    }

    public skillSlots AUTO_SLOTS
    {
        get { return autoSlots; }
        set { autoSlots = value; }
    }
    public BaseStats BASE_STATS
    {
        get { return baseStats; }
        set { baseStats = value; }
    }
    public ModifiedStats STATS
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
        get
        {
            if (PSTATUS != PrimaryStatus.crippled)
            {
                return (base.MOVE_DIST = STATS.MOVE_DIST + BASE_STATS.MOVE_DIST);
            }
            else
            {
                return (int)(((float)STATS.MOVE_DIST + (float)BASE_STATS.MOVE_DIST) * 0.5f);
            }
        }

    }

    public int ACTIONS
    {
        get { return actions; }
        set { actions = value; }
    }
    public int GENERATED
    {
        get { return generatedActions; }
        set { generatedActions = value; }
    }

    public int Atk_DIST
    {
        get { return equippedWeapon.DIST; }
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
        get { return STATS.DEFENSE + BASE_STATS.DEFENSE + ARMOR.DEFENSE; }
    }
    public int RESIESTANCE
    {
        get { return STATS.RESIESTANCE + BASE_STATS.RESIESTANCE + ARMOR.RESISTANCE; }
    }
    public int SPEED
    {
        get { return STATS.SPEED + BASE_STATS.SPEED + ARMOR.SPEED; }
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

    public bool ChangeHealth(int val)
    {

        if (val > 0 && HEALTH >= BASE_STATS.MAX_HEALTH)
        {
            STATS.HEALTH = 0;
            return false;
        }
        STATS.HEALTH += val;

        if (HEALTH < 0)
        {
            STATS.HEALTH = BASE_STATS.MAX_HEALTH * -1;
            DEAD = true;
        }
        return true;
    }
    public bool ChangeMana(int val)
    {
        if (val > 0 && STATS.MANA > STATS.MAX_MANA)
        {
            return false;
        }
        STATS.MANA += val;
        if (STATS.MANA < 0)
        {
            STATS.MANA = 0;

        }
        return true;
    }
    public bool ChangeFatigue(int val)
    {
        if (val > 0 && STATS.FATIGUE > STATS.MAX_FATIGUE)
        {
            return false;
        }
        STATS.FATIGUE -= val;
        if (STATS.FATIGUE > MAX_FATIGUE)
        {
            STATS.FATIGUE = MAX_FATIGUE;
        }
        return true;
    }
    public override void Setup()
    {

        if (!isSetup)
        {
            // Debug.Log("Living setup " + FullName);
            if (!GetComponent<InventoryScript>())
            {
                gameObject.AddComponent<InventoryScript>();
            }
            inventory = GetComponent<InventoryScript>();
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

            if (!GetComponent<BaseStats>())
            {
                gameObject.AddComponent<BaseStats>();
            }

            if (!GetComponent<ModifiedStats>())
            {

                gameObject.AddComponent<ModifiedStats>();
            }
            baseStats = GetComponent<BaseStats>();
            modifiedStats = GetComponent<ModifiedStats>();
            baseStats.USER = this;
            modifiedStats.USER = this;
            this.baseStats.HEALTH = this.baseStats.MAX_HEALTH;
            this.baseStats.MANA = this.baseStats.MAX_MANA;
            modifiedStats.Reset(true);
            modifiedStats.type = 1;

            if (!GetComponent<skillSlots>())
            {
                battleSlots = gameObject.AddComponent<skillSlots>();
                passiveSlots = gameObject.AddComponent<skillSlots>();
                oppSlots = gameObject.AddComponent<skillSlots>();
                autoSlots = gameObject.AddComponent<skillSlots>();
                passiveSlots.TYPE = 1;
                autoSlots.TYPE = 2;
                oppSlots.TYPE = 3;
            }
            else
            {
                skillSlots[] slots = GetComponents<skillSlots>();
                if (slots.Length < 4)
                {
                    while (slots.Length < 4)
                    {
                        gameObject.AddComponent<skillSlots>();
                        slots = GetComponents<skillSlots>();
                    }
                }

                for (int i = 0; i < slots.Length; i++)
                {
                    switch (slots[i].TYPE)
                    {
                        case 0:
                            battleSlots = slots[i];
                            break;

                        case 1:
                            passiveSlots = slots[i];
                            break;

                        case 2:
                            autoSlots = slots[i];
                            break;

                        case 3:
                            oppSlots = slots[i];
                            break;
                    }

                }
                passiveSlots.TYPE = 1;
                autoSlots.TYPE = 2;
                oppSlots.TYPE = 3;

            }
            GetComponent<LivingSetup>().Setup();
            float spd = STATS.SPEED + BASE_STATS.SPEED + ARMOR.SPEED;

            ACTIONS = Mathf.RoundToInt(spd / 10) + 1;
            base.Setup();
        }
        //  Debug.Log("Setup done");
        isSetup = true;
    }

    public void ApplyPassives()
    {
        List<PassiveSkill> atkPassives = PASSIVE_SLOTS.ConvertToPassives();
        List<CommandSkill> buffs = inventory.BUFFS;
        modifiedStats.MODS.Clear();
        modifiedStats.Reset(true);

        if (atkPassives.Count > 0)
        {
            for (int i = 0; i < atkPassives.Count; i++)
            {
                modifiedStats.IncreaseStat(atkPassives[i].ModStat, (int)atkPassives[i].ModValues[0], this);
            }

        }

        if (buffs.Count > 0)
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                modifiedStats.IncreaseStat(buffs[i].BUFFEDSTAT, (int)buffs[i].BUFFVAL, this);
            }

        }

        if (HEALTH > MAX_HEALTH)
        {
            STATS.HEALTH = 0;
        }

        if (MANA > MAX_MANA)
        {
            STATS.MANA = 0;
        }

        if (FATIGUE > MAX_FATIGUE)
        {
            STATS.FATIGUE = MAX_FATIGUE;
        }

    }

    public void TakeAction()
    {
        Debug.Log(FullName + " took an action in " + myManager.currentState.ToString());
        ACTIONS--;
        if (myManager)
        {
          //  myManager.doubleAdjOppTiles.Clear();
            if (ACTIONS <= 0)
            {
                myManager.NextTurn(FullName);
                //myManager.CreateEvent(this, null, "clean state state event", myManager.BufferedCleanEvent);

                //  myManager.CleanMenuStack(true);
                //myManager.GetComponent<InventoryMangager>().Validate("living obj, action taken");
            }
            else
            {
                myManager.currOppList.Clear();
            }
        }
    }
    public void Wait()
    {
        // STATS.HEALTH += 5;
        // STATS.MANA += 2;
        STATS.HEALTH += (int)(0.2 * MAX_HEALTH) * (actions + 1);
        STATS.MANA += (int)(0.2 * MAX_MANA) * (actions + 1);
        STATS.FATIGUE -= (int)(0.2 * MAX_FATIGUE) * (actions + 1);
        if (HEALTH > MAX_HEALTH)
        {
            STATS.HEALTH = STATS.MAX_HEALTH;
        }
        if (MANA > MAX_MANA)
        {
            STATS.MANA = STATS.MAX_MANA;
        }
        if (FATIGUE < 0)
        {
            STATS.FATIGUE = 0;
        }
        ACTIONS = 0;
        float spd = STATS.SPEED + BASE_STATS.SPEED + ARMOR.SPEED;
        if (actions == spd || actions == spd + 2)
        {
            GENERATED += 2;

        }
        if (HEALTH > MAX_HEALTH)
            STATS.HEALTH = MAX_HEALTH;
        if (MANA > MAX_MANA)
            STATS.MANA = MAX_MANA;
        if (FATIGUE < 0)
            STATS.FATIGUE = 0;
    }
    public void LevelUp()
    {
        BASE_STATS.LEVEL++;
        BASE_STATS.MAX_HEALTH += 5;
        BASE_STATS.EXP -= 100;
    }

    public void GainExp(int val)
    {
        BASE_STATS.EXP += val;

    }
}
