using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LivingObject : GridObject
{

    private WeaponEquip equippedWeapon;
    private ArmorEquip equipedArmor;

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
    [SerializeField]
    protected int physLevel = 1;
    [SerializeField]
    protected int magLevel = 1;
    [SerializeField]
    protected int dexLevel = 1;
    private bool tookAction = false;
    protected ShadowObject shadow;
    protected SpriteObject barrier;
    protected List<SpriteObject> lastUsedSprites = new List<SpriteObject>();
    protected SpriteObject highestDamage;
    protected List<SpriteObject> ailments;
    [SerializeField]
    protected List<StatIcon> statusIcons;
    private ArmorScript defaultArmor;
    public List<TileScript> moveableTiles = new List<TileScript>();

    [SerializeField]
    protected List<Element> usedSkills = new List<Element>();

    [SerializeField]
    protected TextMeshPro actionCounterText;

    [SerializeField]
    protected TextMeshPro generatedCounterText;

    protected int shields = 0;

    [SerializeField]
    protected UnityEngine.UI.Slider myWorldHealthBar = null;

    [SerializeField]
    protected TextMeshProUGUI blackWeaknessText = null;
    protected TextMeshProUGUI WeaknessText = null;

    public TextMeshPro TEXT
    {
        get { return actionCounterText; }
    }

    public TextMeshPro GEN_TEXT
    {
        get { return generatedCounterText; }
    }

    public List<Element> LAST_USED
    {
        get { return usedSkills; }
    }

    public ShadowObject SHADOW
    {
        get { return shadow; }
        set { shadow = value; }
    }
    public SpriteObject BARRIER
    {
        get { return barrier; }
        set { barrier = value; }
    }

    public List<SpriteObject> LAST_SPRITES
    {
        get { return lastUsedSprites; }
        set { lastUsedSprites = value; }
    }

    public SpriteObject DMGICON
    {
        get { return highestDamage; }
        set { highestDamage = value; }
    }

    public List<SpriteObject> AILMENTS
    {
        get { return ailments; }
        set { ailments = value; }
    }

    public List<StatIcon> STATUSS
    {
        get { return statusIcons; }
        set { statusIcons = value; }
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
            if (PSTATUS == PrimaryStatus.crippled)
            {
                refreshState = 2;
            }
            if (PSTATUS == PrimaryStatus.guarding)
            {
                refreshState = 1;
            }
        }
    }

    public SecondaryStatus SSTATUS
    {
        get { return sStatus; }
        set { sStatus = value; }
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
    public skillSlots COMBO_SLOTS
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
                    else
                    {
                        for (int i = 0; i < myManager.liveEnemies.Count; i++)
                        {
                            if (myManager.liveEnemies[i].GetComponent<HazardScript>())
                            {
                                if ((myManager.liveEnemies[i] as HazardScript).HTYPE == HazardType.movement)
                                {
                                    bypass = 1;

                                }
                            }
                        }
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
            //if (actions < 0)
            //{
            //    actions = 0;
            //}

            if (actionCounterText)
            {

                actionCounterText.text = actions.ToString();
                if (actions < 0)
                    actionCounterText.color = Color.red;
                else
                    actionCounterText.color = Color.white;

            }
        }
    }
    public int GENERATED
    {
        get { return generatedActions; }
        set { generatedActions = value; }
    }

    public int SHIELDS
    {
        get { return shields; }
        set { shields = value; }
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
        get { return STATS.HEALTH; }
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
    protected string getArrowIcon(EHitType hitType)
    {
        string returnString = "<size=0.2><sprite=";
        switch (hitType)
        {
            case EHitType.absorbs:
                returnString += 26;
                break;
            case EHitType.nulls:
                returnString += 29;
                break;
            case EHitType.reflects:
                returnString += 30;
                break;
            case EHitType.resists:
                returnString += 31;
                break;
            case EHitType.normal:
                break;
            case EHitType.weak:
                returnString += 17;
                break;
            case EHitType.savage:
                returnString += 20;
                break;
            case EHitType.cripples:
                returnString += 22;
                break;
            case EHitType.lethal:
                returnString += 24;
                break;
        }
        returnString += ">";

        return returnString;
    }
    public void ShowHideWeakness(EHitType hitType, bool show = true)
    {
        if (WeaknessText != null)
        {
            if (blackWeaknessText != null)
            {
                if (show == true)
                {
                    if (hitType == EHitType.normal)
                    {
                        WeaknessText.gameObject.SetActive(false);
                        blackWeaknessText.gameObject.SetActive(false);
                    }
                    else
                    {

                        string middleStrin = "" + hitType;//.ToString().Substring(0, 1).ToUpper();
                                                          //middleStrin += hitType.ToString().Substring(1, hitType.ToString().Length);
                        middleStrin += getArrowIcon(hitType);
                        WeaknessText.gameObject.SetActive(true);
                        blackWeaknessText.gameObject.SetActive(true);
                        WeaknessText.text = "" + middleStrin;
                        blackWeaknessText.text = "" + middleStrin;
                        if (hitType > EHitType.normal)
                            WeaknessText.color = Common.red;
                        else
                            WeaknessText.color = Common.cyan;
                    }
                }
                else
                {
                    WeaknessText.gameObject.SetActive(false);
                    blackWeaknessText.gameObject.SetActive(false);
                }
            }
        }
    }


    protected UnityEngine.UI.Image healthFill = null;
    public void UpdateHealthbar()
    {
        if (myWorldHealthBar != null)
        {
            if (healthFill == null)
            {
                healthFill = myWorldHealthBar.fillRect.GetComponent<UnityEngine.UI.Image>();
            }
            float trueMax = MAX_HEALTH;
            myWorldHealthBar.wholeNumbers = false;
            myWorldHealthBar.maxValue = trueMax;
            float fakeMax = trueMax * 0.5f;
            float fakeValue = (float)HEALTH * 0.5f;
            myWorldHealthBar.value = fakeValue;
            if (healthFill != null)
            {
                float middle = fakeMax * 0.5f;

                if (fakeValue >= middle)
                {

                    healthFill.color = Color.Lerp(Color.yellow, Color.green, fakeValue / middle);

                }
                else
                {
                    healthFill.color = Color.Lerp(Color.red, Color.yellow, fakeValue / fakeMax);

                }
            }
        }
    }

    public bool ChangeHealth(int val, bool showchange = true)
    {
        int healedVal = 0;
        int prevHealth = HEALTH;
        if (val > 0 && HEALTH >= MAX_HEALTH)
        {

            return false;
        }

        STATS.HEALTH += val;

        if (STATS.HEALTH > MAX_HEALTH)
        {
            STATS.HEALTH = BASE_STATS.MAX_HEALTH;

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
                myManager.CreateDmgTextEvent("<sprite=2> " + healedVal.ToString(), Color.green, this);
            }
            else if (healedVal != 0)
            {
                healedVal = postHealth - prevHealth;
                myManager.CreateDmgTextEvent(healedVal.ToString(), Common.red, this);
                // Debug.Log("lost health");
            }
        }
        UpdateHealthbar();
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


        if (STATS.MANA > MAX_MANA)
        {
            STATS.MANA = BASE_STATS.MAX_MANA;
            // BASE_STATS.MANA = BASE_STATS.MAX_MANA;
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
            modifiedStats.HEALTH = this.baseStats.MAX_HEALTH;
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

            if (animationScript == null)
            {


                animationScript = GetComponent<AnimationScript>();
            }
            if (animationScript != null)
            {


                animationScript.Setup();
            }


            if (BASE_STATS.STRENGTH > BASE_STATS.MAGIC && BASE_STATS.STRENGTH > BASE_STATS.DEX)
                physLevel++;
            else if (BASE_STATS.MAGIC > BASE_STATS.STRENGTH && BASE_STATS.MAGIC > BASE_STATS.DEX)
                magLevel++;
            else if (BASE_STATS.DEX > BASE_STATS.MAGIC && BASE_STATS.DEX > BASE_STATS.STRENGTH)
                dexLevel++;
            else
            {
                dexLevel++;
            }

            if (SHADOW == null)
            {
                shadow = new GameObject().AddComponent<ShadowObject>();
                shadow.name = "shadow";

            }
            shadow.USER = this;
            shadow.REALUSER = this;
            shadow.Setup();

            shadow.ANIM.runtimeAnimatorController = ANIM.anim.runtimeAnimatorController;

            SpriteRenderer shadowRender = shadow.SPRITE.sr;
            shadowRender.sprite = mySR.sprite;
            shadowRender.color = Common.GetFactionColor(FACTION) - new Color(0, 0, 0, 0.3f);
            shadowRender.material = myManager.ShadowMaterial;
            ANIM.SHADOWANIM = shadow.ANIM;

            if (INVENTORY.WEAPONS.Count > 0)
            {
                equippedWeapon.Equip(INVENTORY.WEAPONS[0]);
            }

            if (BARRIER == null)
            {
                barrier = new GameObject().AddComponent<SpriteObject>();
                barrier.name = " Barrier ";
                barrier.transform.parent = this.transform;
                barrier.transform.localScale = new Vector3(-0.25f, 0.25f, 1.0f);
                barrier.transform.localPosition = new Vector3(-0.216f, 0.25f, 0.1f);
                barrier.gameObject.AddComponent<SpriteRenderer>();
            }
            barrier.Setup();
            //MeshRenderer meshy = new GameObject().AddComponent<MeshRenderer>();

            if (actionCounterText == null)
            {
                actionCounterText = new GameObject().AddComponent<TextMeshPro>();
                actionCounterText.name = "Action Counter";
            }

            actionCounterText.transform.SetParent(this.transform);
            actionCounterText.transform.localScale = new Vector3(0.2f, 0.2f, -0.1f);
            actionCounterText.transform.localPosition = new Vector3(0.85f, 0.25f, -0.1f);
            // tmp.text = "10";
            actionCounterText.rectTransform.sizeDelta = new Vector2(6.2f, 4.2f);
            transform.hasChanged = false;
            actionCounterText.enableAutoSizing = true;


            LAST_SPRITES.Clear();
            float startingY = -0.2f;
            for (int i = 0; i < 3; i++)
            {



                SpriteObject sprObj = new GameObject().AddComponent<SpriteObject>();
                sprObj.name = FullName + " Last Sprite " + i;
                sprObj.transform.parent = this.transform;
                sprObj.transform.localScale = new Vector3(0.25f, 0.25f, 1.0f);
                sprObj.transform.localPosition = new Vector3(0.5f, startingY, -0.1f);
                SpriteRenderer worstSprite = sprObj.gameObject.AddComponent<SpriteRenderer>();
                worstSprite.color = new Color(1, 1, 1, 0.8f);
                sprObj.Setup();

                LAST_SPRITES.Add(sprObj);
                startingY -= 0.2f;
            }

            //if (WEAKNESS == null)
            //{
            //    worstWeakness = new GameObject().AddComponent<SpriteObject>();
            //    worstWeakness.name = "Weakness Icon";
            //    worstWeakness.transform.parent = this.transform;
            //    worstWeakness.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
            //    worstWeakness.transform.localPosition = new Vector3(-0.35f, 0.35f, -0.1f);
            //    SpriteRenderer worstSprite = worstWeakness.gameObject.AddComponent<SpriteRenderer>();
            //    worstSprite.color = new Color(1, 1, 1, 0.6f);
            //    worstWeakness.Setup();
            //    updateWeaknessIcon();
            //}
            if (STATUSS == null)
            {
                STATUSS = new List<StatIcon>();
            }
            STATUSS.Clear();
            StatIcon[] myIcons = GetComponentsInChildren<StatIcon>();
            for (int i = 0; i < myIcons.Length; i++)
            {
                myIcons[i].Setup();
                STATUSS.Add(myIcons[i]);
            }

            if (AILMENTS == null)
            {
                AILMENTS = new List<SpriteObject>();
                float startingXyz = -0.35f;
                for (int i = 0; i < 6; i++)
                {
                    SpriteObject anAilment = new GameObject().AddComponent<SpriteObject>();
                    anAilment.name = FullName + " Ailment " + i;
                    anAilment.transform.parent = this.transform;
                    anAilment.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
                    anAilment.transform.localPosition = new Vector3(startingXyz, 0.65f, -0.1f);
                    SpriteRenderer ailmentSprite = anAilment.gameObject.AddComponent<SpriteRenderer>();
                    anAilment.Setup();
                    ailmentSprite.color = new Color(1, 1, 1, 0.8f);
                    startingXyz += 0.15f;
                    AILMENTS.Add(anAilment);
                }
            }
            updateAilmentIcons();
            //if (DMGICON == null)
            //{
            //    highestDamage = new GameObject();
            //    highestDamage.name = "Damage Icon";
            //    highestDamage.transform.parent = this.transform;
            //    highestDamage.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
            //    highestDamage.transform.localPosition = new Vector3(-0.35f, -0.15f, 0.1f);
            //    highestDamage.AddComponent<SpriteRenderer>();
            //    // updateDmgIcon();
            //}

            Canvas myHealthCanvas = GetComponentInChildren<Canvas>();
            if (myHealthCanvas != null)
            {
                myHealthCanvas.worldCamera = Camera.main;
                myWorldHealthBar = GetComponentInChildren<UnityEngine.UI.Slider>();
                if (GetComponentsInChildren<TextMeshProUGUI>().Length >= 2)
                {

                    blackWeaknessText = GetComponentsInChildren<TextMeshProUGUI>()[0];
                    WeaknessText = GetComponentsInChildren<TextMeshProUGUI>()[1];
                    ShowHideWeakness(EHitType.normal, false);
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
            float spd = STATS.SPEED + BASE_STATS.SPEED + ARMOR.SPEED;

            ACTIONS = (int)(spd / 10.0f);
            if (spd == 0)
                ACTIONS = 1;
            else
                ACTIONS += 3;

            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.Rotate(new Vector3(90, 0, 0));
            transform.localScale = new Vector3(2, 2, 2);

        }
        //  Debug.Log("Setup done");
        name = NAME;
        isSetup = true;


    }

    public void updateLastSprites()
    {

        if (LAST_SPRITES != null)
        {
            InventoryMangager invmger = GameObject.FindObjectOfType<InventoryMangager>();
            for (int i = 0; i < LAST_SPRITES.Count; i++)
            {
                SpriteObject spriteObject = LAST_SPRITES[i];
                if (LAST_USED.Count > i)
                {
                    //Debug.Log("yonx");
                    int indx = Common.GetElementIndex(LAST_USED[i]);
                    // Debug.Log(" " + indx);
                    spriteObject.sr.sprite = invmger.ELEMENTS[indx];
                }
                else
                {
                    // Debug.Log("yon");
                    spriteObject.sr.sprite = null;
                }
            }

        }
    }

    public void updateAilmentIcons()
    {

        ManagerScript manager = Common.GetManager();
        if (manager)
        {
            StatusIconManager iconManager = manager.iconManager;

            if (iconManager != null && STATUSS != null)
            {

                if (STATUSS.Count < 6)
                    return;

                int almentIndex = 0;
                if (PSTATUS == PrimaryStatus.crippled)
                {
                    // AILMENTS[almentIndex].sr.sprite = iconManager.statusIconImages[(int)StatusIcon.Crippled];
                    STATUSS[almentIndex].childImage.sprite = iconManager.statusIconImages[(int)StatusIcon.Crippled];
                    STATUSS[almentIndex].myImage.color = Color.white;
                    STATUSS[almentIndex].childImage.color = Color.white;
                    STATUSS[almentIndex].myProText.text = "x1";

                    STATUSS[almentIndex].tooltip.defaultString = "*Status: @Crippled! \nWhile crippled target will do half damage, take double damage, and movement will be reduced to 1";
                    STATUSS[almentIndex].tooltip.UpdateTooltip();
                    almentIndex++;
                }
                //if (PSTATUS == PrimaryStatus.guarding)
                //{
                //    AILMENTS[almentIndex].sr.sprite = iconManager.statusIconImages[(int)StatusIcon.Guard];
                //    almentIndex++;
                //}
                if (SHIELDS > 0)
                // if (almentIndex < 6)
                {
                    // AILMENTS[almentIndex].sr.sprite = iconManager.statusIconImages[(int)StatusIcon.Guard];

                    STATUSS[almentIndex].childImage.sprite = iconManager.statusIconImages[(int)StatusIcon.Guard];
                    STATUSS[almentIndex].myImage.color = Color.white;
                    STATUSS[almentIndex].childImage.color = Color.white;
                    STATUSS[almentIndex].myProText.text = "x" + SHIELDS;

                    STATUSS[almentIndex].tooltip.defaultString = "*Status: @Guarding! \nWhile guarding you take half damage and can't be crippled.\nLasts for " + SHIELDS + " hit";
                    STATUSS[almentIndex].tooltip.UpdateTooltip();
                    if (SHIELDS > 1)
                        STATUSS[almentIndex].tooltip.defaultString += "s.";
                    else
                        STATUSS[almentIndex].tooltip.defaultString += ".";
                    STATUSS[almentIndex].tooltip.UpdateTooltip();


                    almentIndex++;
                }

                for (int i = 0; i < INVENTORY.EFFECTS.Count; i++)
                {
                    if (almentIndex < 6)
                    {
                        // AILMENTS[almentIndex].sr.sprite = iconManager.statusIconImages[(int)Common.EffectToIcon(INVENTORY.EFFECTS[i].EFFECT)];

                        STATUSS[almentIndex].childImage.sprite = iconManager.statusIconImages[(int)Common.EffectToIcon(INVENTORY.EFFECTS[i].EFFECT)];
                        STATUSS[almentIndex].myImage.color = Color.white;
                        STATUSS[almentIndex].childImage.color = Color.white;
                        STATUSS[almentIndex].myProText.text = "x" + INVENTORY.EFFECTS[i].TURNS;

                        EffectScript cmd = INVENTORY.EFFECTS[i];
                        STATUSS[almentIndex].tooltip.defaultString = "*Ailment: @" + cmd.EFFECT.ToString() + "\n @" + Common.GetSideEffectText(cmd.EFFECT) + "\nEnds in " + INVENTORY.EFFECTS[i].TURNS + " turn";
                        if (INVENTORY.EFFECTS[i].TURNS > 1)
                            STATUSS[almentIndex].tooltip.defaultString += "s.";
                        else
                            STATUSS[almentIndex].tooltip.defaultString += ".";
                        STATUSS[almentIndex].tooltip.UpdateTooltip();

                        almentIndex++;
                    }
                }

                for (int i = 0; i < INVENTORY.BUFFS.Count; i++)
                {
                    if (almentIndex < 6)
                    {
                        // AILMENTS[almentIndex].sr.sprite = iconManager.statusIconImages[(int)Common.BuffToIcon(INVENTORY.BUFFS[i].BUFF)];

                        STATUSS[almentIndex].childImage.sprite = iconManager.statusIconImages[(int)Common.BuffToIcon(INVENTORY.BUFFS[i].BUFF)];
                        STATUSS[almentIndex].myImage.color = Color.white;
                        STATUSS[almentIndex].childImage.color = Color.white;
                        STATUSS[almentIndex].myProText.text = "x" + INVENTORY.TBUFFS[i].COUNT;

                        CommandSkill buffskill = INVENTORY.BUFFS[i];
                        STATUSS[almentIndex].tooltip.defaultString = "*Buff: @" + buffskill.NAME + "\n @" + "Increases " + buffskill.BUFFEDSTAT + " by " + buffskill.BUFFVAL + "%. \nEnds in " + INVENTORY.TBUFFS[i].COUNT + " turn";
                        if (INVENTORY.TBUFFS[i].COUNT > 1)
                            STATUSS[almentIndex].tooltip.defaultString += "s.";
                        else
                            STATUSS[almentIndex].tooltip.defaultString += ".";
                        STATUSS[almentIndex].tooltip.UpdateTooltip();
                        almentIndex++;
                    }
                }

                for (int i = 0; i < INVENTORY.DEBUFFS.Count; i++)
                {
                    if (almentIndex < 6)
                    {
                        //  AILMENTS[almentIndex].sr.sprite = iconManager.statusIconImages[(int)Common.BuffToIcon(INVENTORY.DEBUFFS[i].BUFF, true)];

                        STATUSS[almentIndex].childImage.sprite = iconManager.statusIconImages[(int)Common.BuffToIcon(INVENTORY.DEBUFFS[i].BUFF, true)];
                        STATUSS[almentIndex].myImage.color = Color.white;
                        STATUSS[almentIndex].childImage.color = Color.white;
                        STATUSS[almentIndex].myProText.text = "x" + INVENTORY.TDEBUFFS[i].COUNT;

                        CommandSkill buffskill = INVENTORY.DEBUFFS[i];

                        STATUSS[almentIndex].tooltip.defaultString = "*DeBuff: @" + buffskill.NAME + "\n @" + "Decreases " + buffskill.BUFFEDSTAT + " by " + buffskill.BUFFVAL + "%. \nEnds in " + INVENTORY.TDEBUFFS[i].COUNT + " turn";
                        if (INVENTORY.TDEBUFFS[i].COUNT > 1)
                            STATUSS[almentIndex].tooltip.defaultString += "s.";
                        else
                            STATUSS[almentIndex].tooltip.defaultString += ".";
                        STATUSS[almentIndex].tooltip.UpdateTooltip();

                        almentIndex++;
                    }
                }
                if (almentIndex < 6)
                {
                    for (int i = almentIndex; i < 6; i++)
                    {
                        //AILMENTS[almentIndex].sr.sprite = null;

                        STATUSS[almentIndex].childImage.sprite = null;// iconManager.statusIconImages[(int)Common.BuffToIcon(INVENTORY.BUFFS[i].BUFF)];
                        STATUSS[almentIndex].myImage.color = Common.trans;
                        STATUSS[almentIndex].childImage.color = Common.trans;
                        STATUSS[almentIndex].myProText.text = "";
                    }
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
    public void UpdateBuffsAndDebuffs()
    {
        List<CommandSkill> buffs = inventory.BUFFS;
        List<CommandSkill> debuffs = inventory.DEBUFFS;
        modifiedStats.MODS.Clear();
        modifiedStats.Reset();



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
        if (FACTION != Faction.ally)
        {
            TakeRealAction();
        }
        else
        {

            myManager.CreateEvent(this, null, "Neo take action", TakeActionEvent);
        }

    }
    public void TakeRealAction()
    {
        tookAction = true;
        if (GetComponent<EnemyScript>())
        {

            //   Debug.Log(FullName + " took an action in " + myManager.currentState.ToString());
        }
        if (myManager.liveEnemies.Count > 0)
        {
            if (ACTIONS > 0)
                ACTIONS--;
        }
        if (myManager)
        {
            //  myManager.doubleAdjOppTiles.Clear();
            if (ACTIONS <= 0)
            {
                tookAction = false;
                myManager.NextTurn(FullName, myManager.currentState);
                if (RENDERER)
                {
                    RENDERER.color = Color.gray;
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

        myManager.UpdateTurns();

    }
    public bool TakeActionEvent(Object data)
    {
        TakeRealAction();
        myManager.myCamera.UpdateCamera();

        myManager.CreateEvent(this, null, "return state event", myManager.BufferedCamUpdate);
        return true;
    }
    public virtual bool HealEvent(Object data)
    {
        TrueHeal();
        //  myManager.CreateEvent(this, null, "return state event", myManager.BufferedCamUpdate);
        TakeAction();
        return true;
    }
    public virtual bool RestoreEvent(Object data)
    {
        TrueRestore();
        //  myManager.CreateEvent(this, null, "return state event", myManager.BufferedCamUpdate);
        TakeAction();
        return true;
    }

    public virtual bool ChargeEvent(Object data)
    {
        TrueCharge();
        //  myManager.CreateEvent(this, null, "return state event", myManager.BufferedCamUpdate);
        TakeAction();
        return true;
    }
    public virtual bool DrainEvent(Object data)
    {
        TrueDrain();
        //  myManager.CreateEvent(this, null, "return state event", myManager.BufferedCamUpdate);
        TakeAction();
        return true;
    }

    public virtual bool ShieldEvent(Object data)
    {
        TrueShield();
        //  myManager.CreateEvent(this, null, "return state event", myManager.BufferedCamUpdate);
        TakeAction();
        return true;
    }

    public virtual bool OverloadEvent(Object data)
    {
        //lose health but gain additional action next turn
        TrueOverload();
        //  myManager.CreateEvent(this, null, "return state event", myManager.BufferedCamUpdate);
        TakeAction();
        return true;
    }

    public virtual bool WaitEvent(Object data)
    {
        TrueWait();
        //  myManager.CreateEvent(this, null, "return state event", myManager.BufferedCamUpdate);
        TakeRealAction();
        return true;
    }

    public void Refresh()
    {
        tookAction = false;
        PSTATUS = PrimaryStatus.normal;
        refreshState = 0;
        updateAilmentIcons();
    }
    public virtual void Heal()
    {
        GridAnimationObj gao = null;
        gao = myManager.PrepareGridAnimation(null, this);
        gao.type = -3;
        gao.magnitute = 0;
        gao.LoadGridAnimation();

        myManager.menuManager.ShowNone();
        myManager.CreateEvent(this, gao, "Animation request: " + myManager.AnimationRequests + "", myManager.CheckAnimation, gao.StartCountDown, 0);

        myManager.CreateTextEvent(this, " Heal Health", "wait event", myManager.CheckText, myManager.TextStart);
        if (myManager.log)
        {
            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(FACTION)) + ">";
            myManager.log.Log(coloroption + NAME + "</color> decided to heal!");
        }

        myManager.CreateEvent(this, gao, "Neo wait action", HealEvent);

    }
    public virtual void Restore()
    {
        GridAnimationObj gao = null;
        gao = myManager.PrepareGridAnimation(null, this);
        gao.type = -3;
        gao.magnitute = 0;
        gao.LoadGridAnimation();

        myManager.menuManager.ShowNone();
        myManager.CreateEvent(this, gao, "Animation request: " + myManager.AnimationRequests + "", myManager.CheckAnimation, gao.StartCountDown, 0);

        myManager.CreateTextEvent(this, "Restore Mana", "wait event", myManager.CheckText, myManager.TextStart);
        if (myManager.log)
        {
            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(FACTION)) + ">";
            myManager.log.Log(coloroption + NAME + "</color> decided to restore!");
        }

        myManager.CreateEvent(this, gao, "Neo wait action", RestoreEvent);

    }
    public virtual void Charge()
    {
        GridAnimationObj gao = null;
        gao = myManager.PrepareGridAnimation(null, this);
        gao.type = -4;
        gao.magnitute = 0;
        gao.LoadGridAnimation();

        myManager.menuManager.ShowNone();
        myManager.CreateEvent(this, gao, "Animation request: " + myManager.AnimationRequests + "", myManager.CheckAnimation, gao.StartCountDown, 0);

        myManager.CreateTextEvent(this, "Charge", "wait event", myManager.CheckText, myManager.TextStart);
        if (myManager.log)
        {
            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(FACTION)) + ">";
            myManager.log.Log(coloroption + NAME + "</color> gained fatigue!");
        }

        myManager.CreateEvent(this, gao, "Neo wait action", ChargeEvent);

    }
    public virtual void Drain()
    {
        GridAnimationObj gao = null;
        gao = myManager.PrepareGridAnimation(null, this);
        gao.type = -4;
        gao.magnitute = 0;
        gao.LoadGridAnimation();

        myManager.menuManager.ShowNone();
        myManager.CreateEvent(this, gao, "Animation request: " + myManager.AnimationRequests + "", myManager.CheckAnimation, gao.StartCountDown, 0);

        myManager.CreateTextEvent(this, "Drain", "wait event", myManager.CheckText, myManager.TextStart);
        if (myManager.log)
        {
            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(FACTION)) + ">";
            myManager.log.Log(coloroption + NAME + "</color> reduced fatigue!");
        }

        myManager.CreateEvent(this, gao, "Neo wait action", DrainEvent);

    }
    public virtual void Shield()
    {
        GridAnimationObj gao = null;
        gao = myManager.PrepareGridAnimation(null, this);
        gao.type = -4;
        gao.magnitute = 0;
        gao.LoadGridAnimation();

        myManager.menuManager.ShowNone();
        myManager.CreateEvent(this, gao, "Animation request: " + myManager.AnimationRequests + "", myManager.CheckAnimation, gao.StartCountDown, 0);

        myManager.CreateTextEvent(this, "Shield", "wait event", myManager.CheckText, myManager.TextStart);
        if (myManager.log)
        {
            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(FACTION)) + ">";
            myManager.log.Log(coloroption + NAME + "</color> gained a shield!");
        }

        myManager.CreateEvent(this, gao, "Neo wait action", ShieldEvent);

    }
    public virtual void Overload()
    {
        GridAnimationObj gao = null;
        gao = myManager.PrepareGridAnimation(null, this);
        gao.type = -4;
        gao.magnitute = 0;
        gao.LoadGridAnimation();

        myManager.menuManager.ShowNone();
        myManager.CreateEvent(this, gao, "Animation request: " + myManager.AnimationRequests + "", myManager.CheckAnimation, gao.StartCountDown, 0);

        myManager.CreateTextEvent(this, "Overload", "wait event", myManager.CheckText, myManager.TextStart);
        if (myManager.log)
        {
            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(FACTION)) + ">";
            myManager.log.Log(coloroption + NAME + "</color> stored an action point!");
        }

        myManager.CreateEvent(this, gao, "Neo wait action", OverloadEvent);

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

        myManager.CreateTextEvent(this, "Wait & Heal", "wait event", myManager.CheckText, myManager.TextStart);
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
        gao.type = -4;
        gao.magnitute = 0;
        gao.LoadGridAnimation();

        myManager.menuManager.ShowNone();
        myManager.CreateEvent(this, gao, "Animation request: " + myManager.AnimationRequests + "", myManager.CheckAnimation, gao.StartCountDown, 0);

        myManager.CreateTextEvent(this, "Guard Charge", "guard event", myManager.CheckText, myManager.TextStart);
        if (myManager.log)
        {
            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(FACTION)) + ">";
            myManager.log.Log(coloroption + NAME + "</color> decided to guard!");
        }

        myManager.CreateEvent(this, gao, "Neo guard action", ChargeEvent);

    }
    public void TrueHeal()
    {
        ChangeHealth((int)((0.20f * MAX_HEALTH)));
        if (myManager != null)
        {
            if (myManager.liveEnemies.Count == 0)
            {
                ChangeHealth(100);

            }
        }
        UpdateHealthbar();

        if (Common.hasAllocated == false)
        {
            Common.hasAllocated = true;
            myManager.DidCompleteTutorialStep();
        }
    }
    public void TrueRestore()
    {
        ChangeMana((int)((0.20f * MAX_MANA)));
        if (myManager != null)
        {
            if (myManager.liveEnemies.Count == 0)
            {
                ChangeMana(100);

            }
        }
        if (Common.hasAllocated == false)
        {
            Common.hasAllocated = true;
            myManager.DidCompleteTutorialStep();
        }
    }
    public void TrueCharge()
    {
        ChangeFatigue(-1 * (int)((0.20f * MAX_FATIGUE)));
        if (myManager != null)
        {
            if (myManager.liveEnemies.Count == 0)
            {
                ChangeFatigue(-100);

            }
        }
        if (Common.hasAllocated == false)
        {
            Common.hasAllocated = true;
            myManager.DidCompleteTutorialStep();
        }
    }
    public void TrueDrain()
    {
        ChangeFatigue((int)((0.20f * MAX_FATIGUE)));
        if (myManager != null)
        {
            if (myManager.liveEnemies.Count == 0)
            {
                ChangeFatigue(100);

            }
        }
        if (Common.hasAllocated == false)
        {
            Common.hasAllocated = true;
            myManager.DidCompleteTutorialStep();
        }
    }
    public void TrueShield()
    {
        SHIELDS++;
        PSTATUS = PrimaryStatus.guarding;
        updateAilmentIcons();

        if (Common.hasAllocated == false)
        {
            Common.hasAllocated = true;
            myManager.DidCompleteTutorialStep();
        }
    }
    public void TrueOverload()
    {
        ChangeHealth(-1 * (int)((0.30f * MAX_HEALTH)));
        GENERATED++;

        if (Common.hasAllocated == false)
        {
            Common.hasAllocated = true;
            myManager.DidCompleteTutorialStep();
        }
    }
    public void TrueWait()
    {
        ChangeHealth((int)((0.10f * MAX_HEALTH) * (actions + 1)));
        ChangeMana((int)((0.10f * MAX_MANA) * (actions + 1)));
        ChangeFatigue((int)((0.10f * MAX_FATIGUE) * (actions + 1)));
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

    public void turnUpdate(int bonus = 0)
    {
        //ChangeHealth(2 + bonus);
        //ChangeMana(2 + bonus);
        //ChangeFatigue(2 + bonus);

        if (FACTION == Faction.ally)
        {
            if (bonus == 0)
            {
                actionCounterText.text = "";
            }
        }
    }

    //public void TrueCharge()
    //{
    //    ChangeFatigue(-1 * ((int)((0.80f * MAX_FATIGUE) * (actions + 1))));
    //    //int amtft = ((int)(0.30f * (float)MAX_FATIGUE) * (actions + 1));
    //    ////Debug.Log(amtft);
    //    //STATS.FATIGUE += amtft;
    //    //if (FATIGUE > MAX_FATIGUE)
    //    //{
    //    //    STATS.FATIGUE = BASE_STATS.MAX_FATIGUE;
    //    //    BASE_STATS.FATIGUE = 0;
    //    //}
    //    float spd = STATS.SPEED + BASE_STATS.SPEED + ARMOR.SPEED;
    //    spd = (int)(spd / 10);

    //    //   if (actions == spd || actions == (spd + 2))
    //    if (tookAction == false)
    //    {
    //        GENERATED += 2;


    //    }

    //    PSTATUS = PrimaryStatus.guarding;
    //    ACTIONS = 0;
    //    updateAilmentIcons();
    //    tookAction = false;

    //}

    private int GetExpCap(int expType)
    {
        //0 = exp
        //1 = phys
        //2 = mag
        //3 = sprt

        switch (expType)
        {
            case 0:
                {
                    return (int)((float)BASE_STATS.LEVEL * 100.0f * 1.25f);
                }
                break;

            case 1:
                {
                    return (int)((float)PHYSLEVEL * 100.0f * 1.25f);
                }
                break;

            case 2:
                {
                    return (int)((float)MAGLEVEL * 100.0f * 1.25f);
                }
                break;

            case 3:
                {
                    return (int)((float)DEXLEVEL * 100.0f * 1.25f);
                }
                break;
        }




        return 100;
    }
    public void LevelUp(bool show = true)
    {
        if (BASE_STATS.LEVEL + 1 < Common.MaxLevel)
        {
            BASE_STATS.LEVEL++;
            BASE_STATS.MAX_HEALTH += dexLevel;
            STATS.HEALTH = BASE_STATS.MAX_HEALTH;
            //  BASE_STATS.MAX_MANA += magLevel;
            // BASE_STATS.MAX_MANA = BASE_STATS.MAX_MANA;
            // BASE_STATS.MAX_FATIGUE += physLevel;

            BASE_STATS.EXP = 0;
            BASE_STATS.STRENGTH += Random.Range(0, physLevel + 1);
            BASE_STATS.DEFENSE += Random.Range(0, physLevel + 1);
            BASE_STATS.MAGIC += Random.Range(0, magLevel + 1);
            BASE_STATS.RESIESTANCE += Random.Range(0, magLevel + 1);
            BASE_STATS.SPEED += Random.Range(0, dexLevel);
            BASE_STATS.DEX += Random.Range(0, dexLevel);

            if (show)
            {
                myManager.CreateDmgTextEvent("LV UP ", Color.yellow, this, 1.2f);
            }
            UpdateHealthbar();
        }

    }

    public void GainExp(int val)
    {
        BASE_STATS.EXP += val;
        if (BASE_STATS.EXP >= GetExpCap(0))
        {
            LevelUp();

        }
    }
    public void GainPhysExp(int val, bool show = true)
    {
        BASE_STATS.PHYSEXP += val;
        if (BASE_STATS.PHYSEXP > GetExpCap(1))
        {
            //float chance = Random.Range(0, 2);
            //if (chance > 0)
            //{

            //}
            //else
            //{
            //}
            BASE_STATS.STRENGTH += Random.Range(1, physLevel + 1);
            BASE_STATS.DEFENSE += Random.Range(1, physLevel + 1);
            //           BASE_STATS.MAGIC += 1 + magLevel;
            //            BASE_STATS.RESIESTANCE += 1 + magLevel;
            //            BASE_STATS.SPEED += 1 + dexLevel;
            //            BASE_STATS.DEX += 1 + dexLevel;

            BASE_STATS.PHYSEXP = 0;
            physLevel++;
            BASE_STATS.FTCOSTCHANGE += 1;


            //BASE_STATS.MAX_FATIGUE += 5 + physLevel;

            // BASE_STATS.MAX_MANA += 2 + magLevel;
            BASE_STATS.MAX_HEALTH += LEVEL * dexLevel;
            if (show)
            {
                // myManager.CreateDmgTextEvent("<sprite=1>  + " + (2 + magLevel), Color.magenta, this);
                // myManager.CreateDmgTextEvent("<sprite=2> + " + (2 + dexLevel), Color.green, this);
                //myManager.CreateDmgTextEvent("<sprite=0>  + " + (5 + physLevel), Color.yellow, this);
                //myManager.CreateDmgTextEvent("DEF + " + 2, Color.yellow, this);
                //myManager.CreateDmgTextEvent("STR + " + 2, Color.yellow, this);
                myManager.CreateDmgTextEvent("PHYS LV UP " + 1, Color.yellow, this, 1.2f);
            }

            UpdateHealthbar();
        }
    }
    public void GainMagExp(int val, bool show = true)
    {
        BASE_STATS.MAGEXP += val;
        if (BASE_STATS.MAGEXP > GetExpCap(2))
        {
            //float chance = Random.Range(0, 2);
            //if (chance > 0)
            //{

            //}
            //else
            //{
            //}
            //  BASE_STATS.STRENGTH += 1 + physLevel;
            // BASE_STATS.DEFENSE += 1 + physLevel;
            BASE_STATS.MAGIC += Random.Range(1, magLevel + 1);
            BASE_STATS.RESIESTANCE += Random.Range(1, magLevel + 1);
            //   BASE_STATS.SPEED += 1 + dexLevel;
            //   BASE_STATS.DEX += 1 + dexLevel;

            BASE_STATS.MAGEXP = 0;
            magLevel++;
            BASE_STATS.MANACHANGE += 1;

            //BASE_STATS.MAX_MANA += 5 + magLevel;
            STATS.MANA = BASE_STATS.MAX_MANA;

            //BASE_STATS.MAX_FATIGUE += 2 + physLevel;
            BASE_STATS.MAX_HEALTH += LEVEL * dexLevel;
            if (show)
            {
                // myManager.CreateDmgTextEvent("<sprite=0>  + " + (2 + physLevel), Color.yellow, this);
                //  myManager.CreateDmgTextEvent("<sprite=2> + " + (2 + dexLevel), Color.green, this);
                //  myManager.CreateDmgTextEvent("<sprite=1>  + " + (5 +magLevel), Color.magenta, this);
                //myManager.CreateDmgTextEvent("RES + " + 2, Color.magenta, this);
                //myManager.CreateDmgTextEvent("MAG + " + 2, Color.magenta, this);
                myManager.CreateDmgTextEvent("MYST LV UP ", Color.magenta, this, 1.2f);
            }
            UpdateHealthbar();
        }
    }
    public void GainDexExp(int val, bool show = true)
    {
        BASE_STATS.SKILLEXP += val;
        if (BASE_STATS.SKILLEXP > GetExpCap(3))
        {

            // BASE_STATS.STRENGTH += 1 + physLevel;
            // BASE_STATS.DEFENSE += 1 + physLevel;
            // BASE_STATS.MAGIC += 2 + magLevel;
            // BASE_STATS.RESIESTANCE += 2 + magLevel;
            BASE_STATS.SPEED += Random.Range(1, dexLevel + 1);
            BASE_STATS.DEX += Random.Range(1, dexLevel + 1);

            BASE_STATS.SKILLEXP = 0;
            dexLevel++;
            BASE_STATS.HPCOSTCHANGE += 1;

            BASE_STATS.MAX_HEALTH += LEVEL * dexLevel;
            STATS.HEALTH = BASE_STATS.MAX_HEALTH;

            // BASE_STATS.MAX_FATIGUE += 2 + physLevel;
            // BASE_STATS.MAX_MANA += 2 + magLevel;
            if (show)
            {
                //   myManager.CreateDmgTextEvent("<sprite=0>  + " + (2+physLevel), Color.yellow, this);
                //   myManager.CreateDmgTextEvent("<sprite=1>  + " + (2+magLevel), Color.magenta, this);
                //myManager.CreateDmgTextEvent("<sprite=2> + " + (5 + dexLevel), Color.green, this);
                //myManager.CreateDmgTextEvent("Spd + " + 2, Color.green, this);
                //myManager.CreateDmgTextEvent("Dex + " + 2, Color.green, this);
                myManager.CreateDmgTextEvent("SPRT LV UP", Color.green, this, 1.2f);
            }
            UpdateHealthbar();
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
        for (int i = 0; i < INVENTORY.EFFECTS.Count; i++)
        {
            EffectScript anEffect = INVENTORY.EFFECTS[i];
            PoolManager.GetManager().ReleaseEffect(anEffect);
        }
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

    public virtual bool SummonBarrier(Object data)
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


    public virtual bool PrepareBarrier(Object data)
    {
        ArmorScript newArmor = data as ArmorScript;
        GridAnimationObj gao = null;
        gao = myManager.PrepareGridAnimation(null, this);
        gao.type = -4;
        gao.magnitute = 0;
        gao.LoadGridAnimation();

        myManager.menuManager.ShowNone();
        myManager.CreateEvent(this, gao, "Animation request: " + myManager.AnimationRequests + "", myManager.CheckAnimation, gao.StartCountDown, 0);

        myManager.CreateTextEvent(this, newArmor.NAME, "wait event", myManager.CheckText, myManager.TextStart);
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
        UsableScript useable = null;
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
            if (possibility.GetType() == typeof(CommandSkill))
            {
                if ((possibility as CommandSkill).SUBTYPE == SubSkillType.Enemy)
                {
                    continue;
                }
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

            useable = possibleUseables[cmdnum];
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
                                if (attackingObject.COMBO_SLOTS.SKILLS.Count > 6)
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
                            database.GetWeapon(useable.INDEX, attackingObject);
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
                            database.GetArmor(useable.INDEX, attackingObject);
                        }
                    }
                    if (attackingObject.FACTION == Faction.ally && useable != null)
                    {
                        useable.USER = attackingObject;
                        myManager.CreateEvent(this, useable, "New Skill Event", myManager.CheckCount, null, 0, myManager.CountStart);
                        //myManager.CreateTextEvent(this, "" + attackingObject.FullName + " learned " + useable.NAME, "new skill event", myManager.CheckText, myManager.TextStart);

                        if (myManager.log)
                        {
                            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(attackingObject.FACTION)) + ">";
                            myManager.log.Log(coloroption + attackingObject.NAME + "</color> learned " + useable.NAME);
                        }
                    }
                }
            }
        }

        return useable;
    }

    public void snapToCurrentTile()
    {
        if (currentTile)
            transform.position = currentTile.transform.position + new Vector3(0, 0.5f, 0);
        else
            Debug.Log("ive lost my tile");
    }

    public void snapTotTile(TileScript tileScript)
    {
        if (tileScript)
            transform.position = tileScript.transform.position + new Vector3(0, 0.5f, 0);
        else
            Debug.Log("ive lost my tile");
    }

    public void LivingUnset(bool removeDefault = true)
    {
        isSetup = false;
        DEAD = false;
        STATS.Reset(true);
        BASE_STATS.Reset();
        STATS.HEALTH = BASE_STATS.MAX_HEALTH;
        INVENTORY.Clear();
        PHYSICAL_SLOTS.SKILLS.Clear();
        COMBO_SLOTS.SKILLS.Clear();
        MAGICAL_SLOTS.SKILLS.Clear();
        OPP_SLOTS.SKILLS.Clear();
        AUTO_SLOTS.SKILLS.Clear();
        if (removeDefault == true)
            DEFAULT_ARMOR = null;
        ARMOR.unEquip();
        PSTATUS = PrimaryStatus.normal;
        LAST_USED.Clear();
        shadow.SCRIPT.Unset();
        if (ANIM)
        {
            ANIM.Unset();
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
    public virtual bool CheckIfDead()
    {
        if (HEALTH <= 0)
        {
            return true;
        }
        return false;
    }
    public virtual string GetClassType()
    {
        string defaultString = "unknown";
        if (BASE_STATS.STRENGTH > BASE_STATS.MAGIC && BASE_STATS.STRENGTH > BASE_STATS.DEX)
        {
            return "Fighter";
        }
        if (BASE_STATS.MAGIC > BASE_STATS.STRENGTH && BASE_STATS.MAGIC > BASE_STATS.DEX)
        {
            return "Mage";
        }
        if (BASE_STATS.DEX > BASE_STATS.MAGIC && BASE_STATS.DEX > BASE_STATS.STRENGTH)
        {
            return "Shaman";
        }
        if (BASE_STATS.STRENGTH == BASE_STATS.MAGIC && BASE_STATS.STRENGTH == BASE_STATS.DEX)
        {
            return "Balanced";
        }

        return defaultString;
    }

    public void UpdateLastUsed(Element anElement)
    {
        LAST_USED.Add(anElement);
        if (LAST_USED.Count > 3)
        {
            LAST_USED.Remove(LAST_USED[0]);
        }
        updateLastSprites();
        if (LAST_USED.Count == 3)
        {
            CheckForCombos();
        }
    }

    public void CheckForCombos()
    {
        if (LAST_USED.Count == 3)
        {

            for (int i = 0; i < inventory.COMBOS.Count; i++)
            {
                ComboSkill aCombo = inventory.COMBOS[i];
                if (aCombo.FIRST == LAST_USED[0])
                {
                    if (aCombo.SECOND == LAST_USED[1])
                    {
                        if (aCombo.THIRD == LAST_USED[2])
                        {
                            myManager.CreateEvent(this, aCombo, "skill activation", myManager.ReturnTrue, null, -1, myManager.ComboActivation);
                            myManager.CreateDmgTextEvent("<sprite=6> " + aCombo.NAME, Color.green, this);
                        }
                    }
                }
            }
        }
    }

    protected Vector2 prev = Vector2.zero;
    private void Update()
    {

        if (actionCounterText)
        {

            if (prev != actionCounterText.rectTransform.sizeDelta)
            {

                if (prev == Vector2.zero)
                {
                    actionCounterText.rectTransform.sizeDelta = new Vector2(6.2f, 4.2f);
                    prev = actionCounterText.rectTransform.sizeDelta;
                }
                else
                {
                    actionCounterText.rectTransform.sizeDelta = new Vector2(6.2f, 4.2f);
                    prev = actionCounterText.rectTransform.sizeDelta;

                }

            }
        }
    }

    public string GeneratteSaveString()
    {
        string saveString = "";

        return saveString;
    }
}
