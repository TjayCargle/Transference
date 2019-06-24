﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingObject : GridObject
{

    private WeaponEquip equippedWeapon;
    private ArmorEquip equipedArmor;
    private AccessoryEquip equippedAccessory;
    //[SerializeField]
    //private bool isEnenmy;
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
    private skillSlots physicalSlots;
    [SerializeField]
    private skillSlots magicalSlots;
    [SerializeField]
    private skillSlots passiveSlots;
    [SerializeField]
    private skillSlots oppSlots;
    [SerializeField]
    private skillSlots autoSlots;

    [SerializeField]
    private InventoryScript inventory;
    public int refreshState;
    private int physLevel = 1;
    private int magLevel = 1;
    private int skillLevel = 1;
    private bool tookAction = false;
    protected GameObject shadow;
    protected GameObject barrier;

    public GameObject SHADOW
    {
        get { return shadow; }
        set { shadow = value; }
    }
    public GameObject BARRIER
    {
        get { return barrier; }
        set { barrier = value; }
    }
    public InventoryScript INVENTORY
    {
        get { return inventory; }
        set { inventory = value; }
    }

    public PrimaryStatus PSTATUS
    {
        get { return pStatus; }
        set
        {
            pStatus = value;
            if (PSTATUS != PrimaryStatus.normal)
            {
                refreshState = 2;
            }
        }
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
    public skillSlots PHYSICAL_SLOTS
    {
        get { return physicalSlots; }
        set { physicalSlots = value; }
    }
    public skillSlots MAGICAL_SLOTS
    {
        get { return magicalSlots; }
        set { magicalSlots = value; }
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
        set { actions = value; tookAction = false; }
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
        get { return STATS.STRENGTH + BASE_STATS.STRENGTH + (WEAPON.BOOST == ModifiedStat.Str ? WEAPON.BOOSTVAL : 0); }
    }
    public int MAGIC
    {
        get { return STATS.MAGIC + BASE_STATS.MAGIC + (WEAPON.BOOST == ModifiedStat.Mag ? WEAPON.BOOSTVAL : 0); }
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
    public int SKILL
    {
        get { return STATS.SKILL + BASE_STATS.SKILL + (WEAPON.BOOST == ModifiedStat.Skill ? WEAPON.BOOSTVAL : 0); }
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
        get { return BASE_STATS.LEVEL; }
    }
    public int PHYSLEVEL
    {
        get { return physLevel; }
    }
    public int MAGLEVEL
    {
        get { return magLevel; }
    }
    public int SKLEVEL
    {
        get { return skillLevel; }
    }

    //public bool IsEnenmy
    //{
    //    get { return isEnenmy; }
    //    set { isEnenmy = value; }
    //}

    public bool ChangeHealth(int val)
    {

        if (val > 0 && HEALTH >= MAX_HEALTH)
        {
            STATS.HEALTH = 0;
            return false;
        }
        if (val + HEALTH > MAX_HEALTH)
        {
            STATS.HEALTH = STATS.MAX_HEALTH;
            BASE_STATS.HEALTH = BASE_STATS.MAX_HEALTH;
        }
        else
        {
            STATS.HEALTH += val;

        }
        if (HEALTH <= 0)
        {
            STATS.HEALTH = MAX_HEALTH * -1;

            DEAD = true;
            if (currentTile)
            {

                currentTile.isOccupied = false;
                currentTile = null;
            }
            myManager.gridObjects.Remove(this);
            Die();
        }
        return true;
    }
    public bool ChangeMana(int val)
    {
        if (val > 0 && MANA >= MAX_MANA)
        {
            return false;
        }
        STATS.MANA += val;
        if (MANA > MAX_MANA)
        {
            STATS.MANA = STATS.MAX_MANA;
            BASE_STATS.MANA = BASE_STATS.MAX_MANA;
        }
        if (MANA <= 0)
        {
            STATS.MANA = -1 * MAX_MANA;

        }
        return true;
    }
    public bool ChangeFatigue(int val)
    {
        if (val > 0 && STATS.FATIGUE >= MAX_FATIGUE)
        {
            return false;
        }
        STATS.FATIGUE -= val;
        if (FATIGUE > MAX_FATIGUE)
        {
            STATS.FATIGUE = STATS.MAX_FATIGUE;
            BASE_STATS.FATIGUE = BASE_STATS.MAX_FATIGUE;
        }

        if (FATIGUE < 0)
        {
            STATS.FATIGUE = -1 * MAX_FATIGUE;
        }
        return true;
    }
    public override void Setup()
    {

        if (!isSetup)
        {
            base.Setup();
            // Debug.Log("Living setup " + FullName);
            if (!GetComponent<InventoryScript>())
            {
                gameObject.AddComponent<InventoryScript>();
            }
            inventory = GetComponent<InventoryScript>();
            if (!GetComponent<WeaponEquip>())
            {
                gameObject.AddComponent<WeaponEquip>();
                gameObject.GetComponent<WeaponEquip>().NAME = "default";
            }
            equippedWeapon = GetComponent<WeaponEquip>();
            equippedWeapon.USER = this;
            WeaponScript defaultWeapon = Common.noWeapon;
            defaultWeapon.NAME = "default";
            equippedWeapon.Equip(defaultWeapon);
            if (!GetComponent<ArmorEquip>())
            {
                gameObject.AddComponent<ArmorEquip>();
                gameObject.GetComponent<ArmorEquip>().NAME = "default";

            }
            equipedArmor = GetComponent<ArmorEquip>();
            equipedArmor.USER = this;


            ArmorScript defaultArmor = Common.noArmor;
            defaultArmor.NAME = "default";
            defaultArmor.HITLIST = Common.noHitList;
            equipedArmor.Equip(defaultArmor);

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
                physicalSlots = gameObject.AddComponent<skillSlots>();
                magicalSlots = gameObject.AddComponent<skillSlots>();
                passiveSlots = gameObject.AddComponent<skillSlots>();
                oppSlots = gameObject.AddComponent<skillSlots>();
                autoSlots = gameObject.AddComponent<skillSlots>();
                passiveSlots.TYPE = 1;
                autoSlots.TYPE = 2;
                oppSlots.TYPE = 3;
                magicalSlots.TYPE = 4;
            }
            else
            {
                skillSlots[] slots = GetComponents<skillSlots>();
                if (slots.Length < 5)
                {
                    while (slots.Length < 5)
                    {
                        gameObject.AddComponent<skillSlots>();
                    }
                }
                slots = GetComponents<skillSlots>();

                for (int i = 0; i < slots.Length; i++)
                {
                    switch (slots[i].TYPE)
                    {
                        case 0:
                            physicalSlots = slots[i];
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
                        case 4:
                            magicalSlots = slots[i];
                            break;
                    }

                }
                passiveSlots.TYPE = 1;
                autoSlots.TYPE = 2;
                oppSlots.TYPE = 3;
                magicalSlots.TYPE = 4;

            }
            if (GetComponent<LivingSetup>())
            {
                GetComponent<LivingSetup>().Setup();

            }
            if (GetComponent<EnemySetup>())
            {
                GetComponent<EnemySetup>().Setup();

            }
            if (GetComponent<ActorSetup>())
            {
                GetComponent<ActorSetup>().Setup();

            }
            if (GetComponent<HazardSetup>())
            {
                GetComponent<HazardSetup>().Setup();

            }
            if (!gameObject.GetComponent<AnimationScript>())
            {
                gameObject.AddComponent<AnimationScript>();
            }
            gameObject.GetComponent<AnimationScript>().Setup();
            float spd = STATS.SPEED + BASE_STATS.SPEED + ARMOR.SPEED;

            ACTIONS = (int)(spd / 10) + 2;
            if (SHADOW == null)
            {
                shadow = new GameObject();
                shadow.transform.parent = this.transform;
                shadow.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
                shadow.transform.localPosition = new Vector3(0, 0, 0.1f);
                shadow.AddComponent<SpriteRenderer>();
            }
            if (!shadow.GetComponent<AnimationScript>())
            {
                shadow.AddComponent<AnimationScript>();

            }
            AnimationScript ShadowAnimation = shadow.GetComponent<AnimationScript>();
            ShadowAnimation.obj = this;
            if (!shadow.GetComponent<Animator>())
            {
                shadow.gameObject.AddComponent<Animator>();
            }
            Animator shadowAnimator = shadow.gameObject.GetComponent<Animator>();
            shadowAnimator.runtimeAnimatorController = GetComponent<AnimationScript>().anim.runtimeAnimatorController;
            ShadowAnimation.render = shadow.GetComponent<SpriteRenderer>();
            ShadowAnimation.me = this;
            ShadowAnimation.Setup();
            SpriteRenderer shadowRender = shadow.GetComponent<SpriteRenderer>();
            shadowRender.sprite = GetComponent<SpriteRenderer>().sprite;
            shadowRender.color = Common.GetFactionColor(FACTION) - new Color(0, 0, 0, 0.3f);
            shadowRender.material = myManager.ShadowMaterial;
            GetComponent<AnimationScript>().SHADOWANIM = shadow.GetComponent<Animator>();

            if (BARRIER == null)
            {
                barrier = new GameObject();
                barrier.transform.parent = this.transform;
                barrier.transform.localScale = new Vector3(0.25f, 0.25f, 1.0f);
                barrier.transform.localPosition = new Vector3(0.25f, 0.25f, 0.1f);
                barrier.AddComponent<SpriteRenderer>();
            }

            if (ARMOR.HITLIST != Common.noHitList)
            {
                if (ARMOR.ARMORID < 4)
                {

                    barrier.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Shields/")[ARMOR.ARMORID];
                }
                else
                {

                }
            }
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.Rotate(new Vector3(90, 0, 0));

        }
        //  Debug.Log("Setup done");
        name = NAME;
        isSetup = true;


    }

    public void ApplyPassives()
    {
        List<PassiveSkill> atkPassives = PASSIVE_SLOTS.ConvertToPassives();
        List<CommandSkill> buffs = inventory.BUFFS;
        List<CommandSkill> debuffs = inventory.DEBUFFS;
        modifiedStats.MODS.Clear();
        modifiedStats.Reset();

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

        if (debuffs.Count > 0)
        {
            for (int i = 0; i < debuffs.Count; i++)
            {
                modifiedStats.IncreaseStat(debuffs[i].BUFFEDSTAT, (int)debuffs[i].BUFFVAL, this);
            }

        }



    }
    public void TakeAction()
    {
        myManager.CreateEvent(this, null, "Neo take action", TakeActionEvent);

    }
    public void TakeRealAction()
    {
        tookAction = true;
        if (GetComponent<EnemyScript>())
        {

            //   Debug.Log(FullName + " took an action in " + myManager.currentState.ToString());
        }
        ACTIONS--;
        if (myManager)
        {
            //  myManager.doubleAdjOppTiles.Clear();
            if (ACTIONS <= 0)
            {
                myManager.NextTurn(FullName, myManager.currentState);
                //myManager.CreateEvent(this, null, "clean state state event", myManager.BufferedCleanEvent);

                //  myManager.CleanMenuStack(true);
                //myManager.GetComponent<InventoryMangager>().Validate("living obj, action taken");
            }
            else
            {
                myManager.currOppList.Remove(this);//Clear();
            }
        }
    }
    public bool TakeActionEvent(Object data)
    {
        TakeRealAction();
        myManager.CreateEvent(this, null, "return state event", myManager.BufferedCamUpdate);
        return true;
    }
    public void Wait()
    {
        // STATS.HEALTH += 5;
        // STATS.MANA += 2;
        ChangeHealth((int)(0.125 * MAX_HEALTH) * (actions + 1));
        ChangeMana((int)(0.125 * MAX_MANA) * (actions + 1));
        ChangeFatigue((int)(0.125 * MAX_FATIGUE) * (actions + 1));
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
        float spd = STATS.SPEED + BASE_STATS.SPEED + ARMOR.SPEED;
        spd = (int)(spd / 10);

        //   if (actions == spd || actions == (spd + 2))
        if (tookAction == false)
        {
            GENERATED += 2;


        }
        ACTIONS = 0;
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
        BASE_STATS.MAX_HEALTH += 5 + skillLevel;
        BASE_STATS.MAX_MANA += 5 + magLevel;
        BASE_STATS.MAX_FATIGUE += 5 + physLevel;
        BASE_STATS.EXP -= 100;
        BASE_STATS.STRENGTH++;
        BASE_STATS.DEFENSE++;
        BASE_STATS.MAGIC++;
        BASE_STATS.RESIESTANCE++;
        BASE_STATS.SPEED++;
        BASE_STATS.SKILL++;

    }

    public void GainExp(int val)
    {
        BASE_STATS.EXP += val;

    }
    public void GainPhysExp(int val)
    {
        BASE_STATS.PHYSEXP += val;
        if (BASE_STATS.PHYSEXP > 100)
        {
            float chance = Random.Range(0, 2);
            if (chance > 0)
            {
                BASE_STATS.STRENGTH++;

            }
            else
            {
                BASE_STATS.DEFENSE++;
            }
            BASE_STATS.PHYSEXP = 0;
            physLevel++;
        }
    }
    public void GainMagExp(int val)
    {
        BASE_STATS.MAGEXP += val;
        if (BASE_STATS.MAGEXP > 100)
        {
            float chance = Random.Range(0, 2);
            if (chance > 0)
            {
                BASE_STATS.MAGIC++;

            }
            else
            {
                BASE_STATS.RESIESTANCE++;
            }
            BASE_STATS.MAGEXP = 0;
            magLevel++;
        }
    }
    public void GainSklExp(int val)
    {
        BASE_STATS.SKILLEXP += val;
        if (BASE_STATS.SKILLEXP > 100)
        {
            float chance = Random.Range(0, 2);
            if (chance > 0)
            {
                BASE_STATS.SPEED++;

            }
            else
            {
                BASE_STATS.SKILL++;
            }
            BASE_STATS.SKILLEXP = 0;
            skillLevel++;
        }
    }
    public override void Die()
    {

        myManager.CreateEvent(this, null, "death event", DieEvent, DeathStart);
    }

    public override void DeathStart()
    {
        INVENTORY.BUFFS.Clear();
        INVENTORY.DEBUFFS.Clear();
        INVENTORY.EFFECTS.Clear();
        base.DeathStart();

    }
    public override IEnumerator FadeOut()
    {
        startedDeathAnimation = true;
        // Debug.Log("enemy dying");
        if (GetComponent<SpriteRenderer>())
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Color subtract = new Color(0, 0, 0, 0.1f);
            int num = 0;
            while (renderer.color.a > 0)
            {
                num++;
                if (num > 9999)
                {
                    Debug.Log("time expired");
                    break;
                }
                renderer.color = renderer.color - subtract;
                yield return null;
            }
            isdoneDying = true;

        }
    }
    public override bool DieEvent(Object data)
    {

        return isdoneDying;
    }

}
