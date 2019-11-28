using System.Collections;
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
    private int dexLevel = 1;
    private bool tookAction = false;
    protected GameObject shadow;
    protected GameObject barrier;
    protected GameObject worstWeakness;
    protected GameObject highestDamage;

    private ArmorScript defaultArmor;
    public List<TileScript> moveableTiles = new List<TileScript>();
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

    public GameObject WEAKNESS
    {
        get { return worstWeakness; }
        set { worstWeakness = value; }
    }

    public GameObject DMGICON
    {
        get { return highestDamage; }
        set { highestDamage = value; }
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

    public ArmorScript DEFAULT_ARMOR
    {
        get { return defaultArmor; }
        set { defaultArmor = value; }
    }
    public override int MOVE_DIST
    {
        get
        {
            int bypass = base.MOVE_DIST = STATS.MOVE_DIST + BASE_STATS.MOVE_DIST;
            if (myManager)
            {
                if (myManager.isSetup)
                {

                    if (myManager.liveEnemies.Count == 0)
                    {
                        bypass = 100;
                    }
                }
            }
            if (PSTATUS != PrimaryStatus.crippled)
            {
                return (bypass);
            }
            else
            {
                return 1;
            }
        }

    }

    public int ACTIONS
    {
        get { return actions; }
        set
        {
            actions = value;
            if (actions < 0) { actions = 0; }
        }
    }
    public int GENERATED
    {
        get { return generatedActions; }
        set { generatedActions = value; }
    }

    //public int Atk_DIST
    //{
    //    get { return equippedWeapon.DIST; }
    //}

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
    public int DEX
    {
        get { return STATS.DEX + BASE_STATS.DEX; }
    }
    public int MAX_HEALTH
    {
        get { return STATS.MAX_HEALTH + BASE_STATS.MAX_HEALTH; }
    }
    public int HEALTH
    {
        get { return STATS.HEALTH + BASE_STATS.HEALTH; }
    }
    public int MAX_MANA
    {
        get { return STATS.MAX_MANA + BASE_STATS.MAX_MANA; }
    }

    public int MANA
    {
        get { return STATS.MANA; }
    }
    public int MAX_FATIGUE
    {
        get { return STATS.MAX_FATIGUE + BASE_STATS.MAX_FATIGUE; }
    }
    public int FATIGUE
    {
        get { return STATS.FATIGUE; }
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
    public int DEXLEVEL
    {
        get { return dexLevel; }
    }

    //public bool IsEnenmy
    //{
    //    get { return isEnenmy; }
    //    set { isEnenmy = value; }
    //}

    public bool ChangeHealth(int val, bool showchange = true)
    {
        int healedVal = 0;
        int prevHealth = HEALTH;
        if (val > 0 && HEALTH >= MAX_HEALTH)
        {
            STATS.HEALTH = 0;
            BASE_STATS.HEALTH = BASE_STATS.MAX_HEALTH;
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
        if (showchange == true)
        {

            int postHealth = HEALTH;
            healedVal = postHealth - prevHealth;

            if (prevHealth < postHealth)
            {
                myManager.CreateDmgTextEvent( "<sprite=2> "+ healedVal.ToString(), Color.green, this);
            }
            else if (healedVal != 0)
            {
                healedVal = postHealth - prevHealth;
                myManager.CreateDmgTextEvent(healedVal.ToString(), Common.red, this);
                // Debug.Log("lost health");
            }
        }

        return true;
    }
    public bool ChangeMana(int val)
    {
        int healedVal = 0;
        int prevHealth = MANA;

        if (val > 0 && MANA >= MAX_MANA)
        {
            return false;
        }
        STATS.MANA += val;


        if (MANA > MAX_MANA)
        {
            STATS.MANA = BASE_STATS.MAX_MANA;
           // BASE_STATS.MANA = BASE_STATS.MAX_MANA;
        }
        else
        {
            STATS.MANA += val;

        }
        if (MANA <= 0)
        {
            STATS.MANA = 0;// -1 * MAX_MANA;

        }

        int postHealth = MANA;
        healedVal = postHealth - prevHealth;

        if (prevHealth < postHealth)
        {
            myManager.CreateDmgTextEvent("<sprite=1> " + healedVal.ToString(), Color.magenta, this);
        }
        else if (healedVal != 0)
        {
            healedVal = postHealth - prevHealth;
            myManager.CreateDmgTextEvent(healedVal.ToString(), Color.grey, this);
            Debug.Log("lost mana");
        }

        return true;
    }
    public bool ChangeFatigue(int val)
    {
        int healedVal = 0;
        int prevHealth = FATIGUE;

        if (val > 0 && FATIGUE <= 0)
        {
            return false;
        }
        STATS.FATIGUE -= val;
        if (FATIGUE > MAX_FATIGUE)
        {
            STATS.FATIGUE = BASE_STATS.MAX_FATIGUE;
         
        }

        if (FATIGUE < 0)
        {
            
            STATS.FATIGUE = 0;// -1 * MAX_FATIGUE;
        }

        int postHealth = FATIGUE;
        healedVal = postHealth - prevHealth;

        if (prevHealth > postHealth)
        {
            healedVal *= -1;
            myManager.CreateDmgTextEvent("<sprite=0> -" + healedVal.ToString(), Color.yellow, this);
        }
        else if (healedVal != 0)
        {
            healedVal = postHealth - prevHealth;
            myManager.CreateDmgTextEvent("<sprite=0> +" + healedVal.ToString(), Color.yellow, this);
            //   Debug.Log("lost fatigue");
        }
        return true;
    }

    public CommandSkill GetMostUsedSkill()
    {
        if (PHYSICAL_SLOTS.SKILLS.Count <= 1)
        {
            return null;
        }

        CommandSkill mostUsed = PHYSICAL_SLOTS.SKILLS[0] as CommandSkill;
        for (int i = 0; i < PHYSICAL_SLOTS.SKILLS.Count; i++)
        {
            CommandSkill aSkill = PHYSICAL_SLOTS.SKILLS[i] as CommandSkill;
            if (aSkill.USECOUNT > mostUsed.USECOUNT)
            {
                mostUsed = aSkill;
            }
        }
        return mostUsed;

    }
    public CommandSkill GetMostUsedSpell()
    {
        if (MAGICAL_SLOTS.SKILLS.Count <= 1)
        {
            return null;
        }

        CommandSkill mostUsed = MAGICAL_SLOTS.SKILLS[0] as CommandSkill;
        for (int i = 0; i < MAGICAL_SLOTS.SKILLS.Count; i++)
        {
            CommandSkill aSkill = MAGICAL_SLOTS.SKILLS[i] as CommandSkill;
            if (aSkill.USECOUNT > mostUsed.USECOUNT)
            {
                mostUsed = aSkill;
            }
        }
        return mostUsed;

    }

    public WeaponScript GetMostUsedStrike()
    {
        if (INVENTORY.WEAPONS.Count <= 1)
        {
            return null;
        }

        WeaponScript mostUsed = INVENTORY.WEAPONS[0] as WeaponScript;
        for (int i = 0; i < INVENTORY.WEAPONS.Count; i++)
        {
            WeaponScript aSkill = INVENTORY.WEAPONS[i] as WeaponScript;
            if (aSkill.USECOUNT > mostUsed.USECOUNT)
            {
                mostUsed = aSkill;
            }
        }
        return mostUsed;

    }

    public ArmorScript GetMostUsedBarrier()
    {
        if (INVENTORY.ARMOR.Count <= 1)
        {
            return null;
        }

        ArmorScript mostUsed = INVENTORY.ARMOR[0] as ArmorScript;
        for (int i = 0; i < INVENTORY.ARMOR.Count; i++)
        {
            ArmorScript aSkill = INVENTORY.ARMOR[i] as ArmorScript;
            if (aSkill.USECOUNT > mostUsed.USECOUNT)
            {
                mostUsed = aSkill;
            }
        }
        return mostUsed;

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
            // WeaponScript defaultWeapon = Common.noWeapon;
            //defaultWeapon.NAME = "default";
            //equippedWeapon.Equip(defaultWeapon);
            if (!GetComponent<ArmorEquip>())
            {
                gameObject.AddComponent<ArmorEquip>();
                gameObject.GetComponent<ArmorEquip>().NAME = "default";

            }
            equipedArmor = GetComponent<ArmorEquip>();
            equipedArmor.USER = this;




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
            baseStats.HEALTH = this.baseStats.MAX_HEALTH;
            modifiedStats.Reset(true);
            modifiedStats.type = 1;

            // modifiedStats.MANA = this.baseStats.MAX_MANA;

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

            if (SHADOW == null)
            {
                shadow = new GameObject();
                shadow.name = "Shadow";
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

            if (INVENTORY.WEAPONS.Count > 0)
            {
                equippedWeapon.Equip(INVENTORY.WEAPONS[0]);
            }

            if (BARRIER == null)
            {
                barrier = new GameObject();
                barrier.name = "Barrier";
                barrier.transform.parent = this.transform;
                barrier.transform.localScale = new Vector3(0.25f, 0.25f, 1.0f);
                barrier.transform.localPosition = new Vector3(0.25f, 0.25f, 0.1f);
                barrier.AddComponent<SpriteRenderer>();
            }

            if (WEAKNESS == null)
            {
                worstWeakness = new GameObject();
                worstWeakness.name = "Weakness Icon";
                worstWeakness.transform.parent = this.transform;
                worstWeakness.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
                worstWeakness.transform.localPosition = new Vector3(-0.35f, 0.35f, 0.5f);
             SpriteRenderer worstSprite =  worstWeakness.AddComponent<SpriteRenderer>();
                worstSprite.color = new Color(1, 1, 1, 0.4f);
                updateWeaknessIcon();
            }

            if (DMGICON == null)
            {
                highestDamage = new GameObject();
                highestDamage.name = "Damage Icon";
                highestDamage.transform.parent = this.transform;
                highestDamage.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
                highestDamage.transform.localPosition = new Vector3(-0.35f, -0.15f, 0.1f);
                highestDamage.AddComponent<SpriteRenderer>();
                // updateDmgIcon();
            }

            if (ARMOR.HITLIST != Common.noHitList)
            {
                if (ARMOR.ARMORID < 200)
                {

                    barrier.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Shields/")[ARMOR.ARMORID];
                }
                else
                {

                }
            }
            if (!ARMOR.SCRIPT)
            {
                if (DEFAULT_ARMOR)
                {
                    ARMOR.Equip(DEFAULT_ARMOR);
                }
                else
                {
                    ARMOR.Equip(new ArmorScript());
                }
            }
            float spd = STATS.SPEED + BASE_STATS.SPEED + ARMOR.SPEED;

            ACTIONS = (int)(spd / 10) + 2;

            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.Rotate(new Vector3(90, 0, 0));

        }
        //  Debug.Log("Setup done");
        name = NAME;
        isSetup = true;


    }

    public void updateWeaknessIcon()
    {
        if (WEAKNESS != null)
        {
            InventoryMangager invmger = GameObject.FindObjectOfType<InventoryMangager>();
            if (invmger)
            {
                if (invmger.ELEMENTS.Length > 0)
                {
                    int worstIndex = 0;
                    EHitType worstType = EHitType.normal;
                    for (int i = 0; i < ARMOR.HITLIST.Count; i++)
                    {
                        EHitType testType = ARMOR.HITLIST[i];
                        if (testType > worstType)
                        {
                            worstType = testType;
                            worstIndex = i;
                        }
                    }
                    WEAKNESS.GetComponent<SpriteRenderer>().sprite = invmger.ELEMENTS[worstIndex];
                }
            }
        }
    }

    public void updateDmgIcon()
    {
        if (DMGICON != null)
        {
            InventoryMangager invmger = GameObject.FindObjectOfType<InventoryMangager>();
            if (invmger)
            {
                if (invmger.DMGTYPES.Length > 0)
                {
                    int worstIndex = 0;
                    int lowestDef = SPEED;

                    if (DEFENSE < lowestDef)
                    {
                        worstIndex = 1;
                        lowestDef = DEFENSE;
                    }

                    if (RESIESTANCE < lowestDef)
                    {
                        worstIndex = 2;
                        lowestDef = RESIESTANCE;
                    }

                    DMGICON.GetComponent<SpriteRenderer>().sprite = invmger.DMGTYPES[worstIndex];
                }
            }
        }
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
        if (myManager.liveEnemies.Count > 0)
            ACTIONS--;
        if (myManager)
        {
            //  myManager.doubleAdjOppTiles.Clear();
            if (ACTIONS <= 0)
            {
                tookAction = false;
                myManager.NextTurn(FullName, myManager.currentState);
                if (GetComponent<SpriteRenderer>())
                {
                    GetComponent<SpriteRenderer>().color = Color.gray;
                }
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
        myManager.myCamera.UpdateCamera();
        myManager.CreateEvent(this, null, "return state event", myManager.BufferedCamUpdate);
        return true;
    }

    public virtual bool WaitEvent(Object data)
    {
        TrueWait();
        //  myManager.CreateEvent(this, null, "return state event", myManager.BufferedCamUpdate);
        TakeRealAction();
        return true;
    }

    public virtual bool ChargeEvent(Object data)
    {
        TrueCharge();
        //  myManager.CreateEvent(this, null, "return state event", myManager.BufferedCamUpdate);
        TakeRealAction();
        return true;
    }
    public void Wait()
    {
        GridAnimationObj gao = null;
        gao = myManager.PrepareGridAnimation(null, this);
        gao.type = -3;
        gao.magnitute = 0;
        gao.LoadGridAnimation();

        myManager.menuManager.ShowNone();
        myManager.CreateEvent(this, gao, "Animation request: " + myManager.AnimationRequests + "", myManager.CheckAnimation, gao.StartCountDown, 0);

        myManager.CreateTextEvent(this, NAME + " decided to wait", "wait event", myManager.CheckText, myManager.TextStart);
        if (myManager.log)
        {
            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(FACTION)) + ">";
            myManager.log.Log(coloroption + NAME + "</color> decided to wait!");
        }

        myManager.CreateEvent(this, gao, "Neo wait action", WaitEvent);

    }

    public void GuardCharge()
    {
        GridAnimationObj gao = null;
        gao = myManager.PrepareGridAnimation(null, this);
        gao.type = -3;
        gao.magnitute = 0;
        gao.LoadGridAnimation();

        myManager.menuManager.ShowNone();
        myManager.CreateEvent(this, gao, "Animation request: " + myManager.AnimationRequests + "", myManager.CheckAnimation, gao.StartCountDown, 0);

        myManager.CreateTextEvent(this, NAME + " decided to guard", "guard event", myManager.CheckText, myManager.TextStart);
        if (myManager.log)
        {
            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(FACTION)) + ">";
            myManager.log.Log(coloroption + NAME + "</color> decided to guard!");
        }

        myManager.CreateEvent(this, gao, "Neo guard action", ChargeEvent);

    }

    public void TrueWait()
    {
        ChangeHealth((int)(0.25f * MAX_HEALTH) * (actions + 1));
        ChangeMana((int)(0.25f * MAX_MANA) * (actions + 1));
        ChangeFatigue((int)(0.25f * MAX_FATIGUE) * (actions + 1));
     //   Debug.Log(NAME + " pre: " + modifiedStats.MANA);
       // float t1 = ((int)(0.15 * MAX_MANA) * (actions + 1));
       // Debug.Log(NAME + " test: " + t1 + " actions:" + actions);

       // modifiedStats.HEALTH += (int)(0.25f * MAX_HEALTH) * (actions + 1);
       // modifiedStats.MANA += ((int)(0.25f * MAX_MANA) * (actions + 1));
       // modifiedStats.FATIGUE -= ((int)(0.25f * MAX_FATIGUE) * (actions + 1));
    //    Debug.Log(NAME + " post: " + modifiedStats.MANA);
  


        float spd = STATS.SPEED + BASE_STATS.SPEED + ARMOR.SPEED;
        spd = (int)(spd / 10);

        //   if (actions == spd || actions == (spd + 2))
        if (tookAction == false)
        {
            GENERATED += 2;


        }

        ACTIONS = 0;
        tookAction = false;

    }

    public void TrueCharge()
    {
        ChangeFatigue(-1 * ((int)(0.25f * MAX_FATIGUE) * (actions + 1)));
        //int amtft = ((int)(0.30f * (float)MAX_FATIGUE) * (actions + 1));
        ////Debug.Log(amtft);
        //STATS.FATIGUE += amtft;
        //if (FATIGUE > MAX_FATIGUE)
        //{
        //    STATS.FATIGUE = BASE_STATS.MAX_FATIGUE;
        //    BASE_STATS.FATIGUE = 0;
        //}
        float spd = STATS.SPEED + BASE_STATS.SPEED + ARMOR.SPEED;
        spd = (int)(spd / 10);

        //   if (actions == spd || actions == (spd + 2))
        if (tookAction == false)
        {
            GENERATED += 2;


        }

        PSTATUS = PrimaryStatus.guarding;
        ACTIONS = 0;
        tookAction = false;

    }
    public void LevelUp()
    {
        if (BASE_STATS.LEVEL + 1 < Common.MaxLevel)
        {
            BASE_STATS.LEVEL++;
            BASE_STATS.MAX_HEALTH += dexLevel;
            BASE_STATS.HEALTH = BASE_STATS.MAX_HEALTH;
            BASE_STATS.MAX_MANA += magLevel;
            BASE_STATS.MAX_MANA = BASE_STATS.MAX_MANA;
            BASE_STATS.MAX_FATIGUE += physLevel;

            BASE_STATS.EXP -= 100;
            BASE_STATS.STRENGTH++;
            BASE_STATS.DEFENSE++;
            BASE_STATS.MAGIC++;
            BASE_STATS.RESIESTANCE++;
            BASE_STATS.SPEED++;
            BASE_STATS.DEX++;
        }

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
            //float chance = Random.Range(0, 2);
            //if (chance > 0)
            //{

            //}
            //else
            //{
            //}
            BASE_STATS.STRENGTH++;
            BASE_STATS.DEFENSE++;
            BASE_STATS.PHYSEXP = 0;
            physLevel++;
            BASE_STATS.FTCOSTCHANGE -= 0.05f;
        }
    }
    public void GainMagExp(int val)
    {
        BASE_STATS.MAGEXP += val;
        if (BASE_STATS.MAGEXP > 100)
        {
            //float chance = Random.Range(0, 2);
            //if (chance > 0)
            //{

            //}
            //else
            //{
            //}
            BASE_STATS.MAGIC++;
            BASE_STATS.RESIESTANCE++;
            BASE_STATS.MAGEXP = 0;
            magLevel++;
            BASE_STATS.SPCHANGE -= 0.05f;
        }
    }
    public void GainDexExp(int val)
    {
        BASE_STATS.SKILLEXP += val;
        if (BASE_STATS.SKILLEXP > 100)
        {
            //float chance = Random.Range(0, 2);
            //if (chance > 0)
            {
                BASE_STATS.SPEED++;
                BASE_STATS.DEX++;

            }
            //else
            {
            }
            BASE_STATS.SKILLEXP = 0;
            dexLevel++;
            BASE_STATS.HPCOSTCHANGE -= 0.05f;
        }
    }
    public override void Die()
    {
        //  Debug.Log("liv die");
        myManager.CreateEvent(this, null, "death event for " + NAME, DieEvent, DeathStart, 0);
    }

    public override void DeathStart()
    {
        // Debug.Log("Live death event starting");
        INVENTORY.BUFFS.Clear();
        INVENTORY.DEBUFFS.Clear();
        INVENTORY.EFFECTS.Clear();
        base.DeathStart();

    }
    public override IEnumerator FadeOut()
    {
        startedDeathAnimation = true;
        // Debug.Log("livr fade start");


        Color subtract = new Color(0, 0, 0, 0.1f);
        int num = 0;
        while (mySR.color.a > 0)
        {
            if (SHADOW)
            {
                if (SHADOW.GetComponent<SpriteRenderer>())
                {

                    SHADOW.GetComponent<SpriteRenderer>().color = mySR.color;
                }
            }
            num++;
            if (num > 9999)
            {
                Debug.Log("time expired");
                break;
            }
            mySR.color = mySR.color - subtract;
            yield return null;
        }
        isdoneDying = true;

        //Debug.Log("livr fade end");
    }
    public override bool DieEvent(Object data)
    {

        return isdoneDying;
    }

    public bool SummonBarrier(Object data)
    {
        ArmorScript newArmor = data as ArmorScript;
        if (newArmor != ARMOR.SCRIPT)
        {
            ARMOR.Equip(newArmor);
            newArmor.Use();
            TakeAction();
            myManager.CreateEvent(this, null, "waiting for sfx", myManager.WaitForSFXEvent);
        }
        myManager.CreateEvent(this, null, "return state", myManager.BufferedReturnEvent);
        return true;
    }


    public bool PrepareBarrier(Object data)
    {
        ArmorScript newArmor = data as ArmorScript;
        GridAnimationObj gao = null;
        gao = myManager.PrepareGridAnimation(null, this);
        gao.type = -4;
        gao.magnitute = 0;
        gao.LoadGridAnimation();

        myManager.menuManager.ShowNone();
        myManager.CreateEvent(this, gao, "Animation request: " + myManager.AnimationRequests + "", myManager.CheckAnimation, gao.StartCountDown, 0);

        myManager.CreateTextEvent(this, NAME + " summoned a a " + newArmor.NAME, "wait event", myManager.CheckText, myManager.TextStart);
        if (myManager.log)
        {
            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(FACTION)) + ">";
            myManager.log.Log(coloroption + NAME + "</color> summoned a " + newArmor.NAME);
        }

        myManager.CreateEvent(this, newArmor, "Neo barrier equip", SummonBarrier);

        return true;
    }

    public virtual UsableScript TransferSkill(LivingObject attackingObject)
    {
        UsableScript usable = null;
        LivingObject enemy = this;
        DatabaseManager database = Common.GetDatabase();
        List<UsableScript> possibleUseables = new List<UsableScript>();
        for (int i = 0; i < enemy.INVENTORY.USEABLES.Count; i++)
        {
            UsableScript possibility = enemy.INVENTORY.USEABLES[i];
            if (possibility.GetType() == typeof(ItemScript))
            {
                continue;
            }
            if (possibility.GetType() == typeof(ArmorScript))
            {
                //200 + armor is charcter specific
                if ((possibility as ArmorScript).INDEX < 200)
                {
                    possibleUseables.Add(possibility);
                }
            }
            if (!attackingObject.INVENTORY.ContainsUsableIndex(possibility))
            {
                possibleUseables.Add(possibility);
            }
        }
        if (possibleUseables.Count > 0)
        {

            int cmdnum = Random.Range(0, possibleUseables.Count - 1);
            if (cmdnum < 0)
            {
                cmdnum = 0;
            }

            UsableScript useable = possibleUseables[cmdnum];
            bool overFlow = false;
            if (useable != null)
            {
                if (!attackingObject.INVENTORY.USEABLES.Contains(useable))
                {

                    if (useable.GetType().IsSubclassOf(typeof(SkillScript)))
                    {
                        SkillScript skill = useable as SkillScript;
                        useable = database.GetSkill(useable.INDEX);
                        switch (skill.ELEMENT)
                        {

                            case Element.Buff:
                                if (attackingObject.PHYSICAL_SLOTS.SKILLS.Count > 6)
                                {
                                    overFlow = true;
                                    LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                                    learnContainer.attackingObject = attackingObject;
                                    learnContainer.usable = useable;
                                    myManager.CreateEvent(this, learnContainer, "New Skill Event", myManager.CheckNewSKillEvent, null, 0, myManager.NewSkillStart);
                                }
                                break;
                            case Element.Passive:
                                if (attackingObject.PASSIVE_SLOTS.SKILLS.Count > 6)
                                {
                                    overFlow = true;
                                    LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                                    learnContainer.attackingObject = attackingObject;
                                    learnContainer.usable = useable;
                                    myManager.CreateEvent(this, learnContainer, "New Skill Event", myManager.CheckNewSKillEvent, null, 0, myManager.NewSkillStart);

                                }
                                break;
                            case Element.Opp:
                                if (attackingObject.INVENTORY.OPPS.Count > 6)
                                {
                                    overFlow = true;
                                    LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                                    learnContainer.attackingObject = attackingObject;
                                    learnContainer.usable = useable;
                                    myManager.CreateEvent(this, learnContainer, "New Skill Event", myManager.CheckNewSKillEvent, null, 0, myManager.NewSkillStart);

                                }
                                break;
                            case Element.Auto:
                                if (attackingObject.AUTO_SLOTS.SKILLS.Count > 6)
                                {
                                    overFlow = true;
                                    LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                                    learnContainer.attackingObject = attackingObject;
                                    learnContainer.usable = useable;
                                    myManager.CreateEvent(this, learnContainer, "New Skill Event", myManager.CheckNewSKillEvent, null, 0, myManager.NewSkillStart);

                                }
                                break;
                            case Element.none:
                                break;
                            default:
                                if (attackingObject.PHYSICAL_SLOTS.SKILLS.Count > 6)
                                {
                                    overFlow = true;
                                    LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                                    learnContainer.attackingObject = attackingObject;
                                    learnContainer.usable = useable;
                                    myManager.CreateEvent(this, learnContainer, "New Skill Event", myManager.CheckNewSKillEvent, null, 0, myManager.NewSkillStart);

                                }
                                break;

                        }
                        if (overFlow == false)
                        {
                            database.LearnSkill(useable.INDEX, attackingObject);
                        }
                    }
                    else if (useable.GetType() == typeof(WeaponScript))
                    {

                        if (attackingObject.INVENTORY.WEAPONS.Count > 6)
                        {
                            overFlow = true;
                            LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                            learnContainer.attackingObject = attackingObject;
                            learnContainer.usable = useable;
                            myManager.CreateEvent(this, learnContainer, "New Skill Event", myManager.CheckNewSKillEvent, null, 0, myManager.NewSkillStart);

                        }
                        if (overFlow == false)
                        {
                            database.LearnSkill(useable.INDEX, attackingObject);
                        }
                    }
                    else if (useable.GetType() == typeof(ArmorScript))
                    {
                        if (attackingObject.INVENTORY.ARMOR.Count > 6)
                        {
                            overFlow = true;
                            LearnContainer learnContainer = ScriptableObject.CreateInstance<LearnContainer>();
                            learnContainer.attackingObject = attackingObject;
                            learnContainer.usable = useable;
                            myManager.CreateEvent(this, learnContainer, "New Skill Event", myManager.CheckNewSKillEvent, null, 0, myManager.NewSkillStart);

                        }
                        if (overFlow == false)
                        {
                            database.LearnSkill(useable.INDEX, attackingObject);
                        }
                    }
                    if (attackingObject.FACTION == Faction.ally && useable != null)
                    {
                        useable.USER = attackingObject;
                        myManager.CreateEvent(this, useable, "New Skill Event", myManager.CheckCount, null, 0, myManager.CountStart);
                        myManager.CreateTextEvent(this, "" + attackingObject.FullName + " learned " + useable.NAME, "new skill event", myManager.CheckText, myManager.TextStart);

                        if (myManager.log)
                        {
                            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(attackingObject.FACTION)) + ">";
                            myManager.log.Log(coloroption + attackingObject.NAME + "</color> learned " + useable.NAME);
                        }
                    }
                }
            }
        }
        return usable;
    }
    public void LivingUnset()
    {
        isSetup = false;
        DEAD = false;
        STATS.Reset(true);
        BASE_STATS.Reset();
        BASE_STATS.HEALTH = BASE_STATS.MAX_HEALTH;
        INVENTORY.Clear();
        PHYSICAL_SLOTS.SKILLS.Clear();
        PASSIVE_SLOTS.SKILLS.Clear();
        MAGICAL_SLOTS.SKILLS.Clear();
        OPP_SLOTS.SKILLS.Clear();
        AUTO_SLOTS.SKILLS.Clear();
        DEFAULT_ARMOR = null;
        ARMOR.unEquip();
        PSTATUS = PrimaryStatus.normal;

        shadow.GetComponent<AnimationScript>().Unset();
        if (GetComponent<AnimationScript>())
        {
            GetComponent<AnimationScript>().Unset();
        }
        if (GetComponent<EffectScript>())
        {
            Destroy(GetComponent<EffectScript>());
        }
        if (GetComponent<BuffScript>())
        {
            Destroy(GetComponent<BuffScript>());
        }
        if (GetComponent<DebuffScript>())
        {
            Destroy(GetComponent<DebuffScript>());
        }
    }
}
