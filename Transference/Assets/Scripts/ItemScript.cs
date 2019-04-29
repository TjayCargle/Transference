using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : UsableScript
{

    [SerializeField]
    private ItemType iType;
    [SerializeField]
    private TargetType tType;
    [SerializeField]
    private float trueValue;
    [SerializeField]
    private SideEffect effect;
    [SerializeField]
    private Element dmgElement;

    [SerializeField]
    private ModifiedStat modStat;
    [SerializeField]
    private RangeType rType;

    public ItemType ITYPE
    {
        get { return iType; }
        set { iType = value; }
    }
    public TargetType TTYPE
    {
        get { return tType; }
        set { tType = value; }
    }
    public RangeType RTYPE
    {
        get { return rType; }
        set { rType = value; }
    }

    public float VALUE
    {
        get { return trueValue; }
        set { trueValue = value; }
    }

    public Element ELEMENT
    {
        get { return dmgElement; }
        set { dmgElement = value; }
    }
    public SideEffect EFFECT
    {
        get { return effect; }
        set { effect = value; }
    }
    public ModifiedStat STAT
    {
        get { return modStat; }
        set { modStat = value; }
    }
    public bool useItem(LivingObject target, TileScript targetTile = null)
    {
        bool usedEffect = false;
        switch (ITYPE)
        {
            case ItemType.healthPotion:

                int amoint = (int)(target.MAX_HEALTH * trueValue);
                usedEffect = target.ChangeHealth(amoint);

                break;
            case ItemType.manaPotion:
                usedEffect = target.ChangeMana((int)(target.MAX_MANA * trueValue));
                break;
            case ItemType.fatiguePotion:
                usedEffect = target.ChangeFatigue((int)(target.MAX_FATIGUE * trueValue));
                break;
            case ItemType.cure:
                {
                    switch (effect)
                    {
                        case SideEffect.slow:
                            if (target.SSTATUS == SecondaryStatus.slow)
                            {
                                target.SSTATUS = SecondaryStatus.normal;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.rage:
                            if (target.SSTATUS == SecondaryStatus.rage)
                            {
                                target.SSTATUS = SecondaryStatus.normal;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.charm:
                            if (target.SSTATUS == SecondaryStatus.charm)
                            {
                                target.SSTATUS = SecondaryStatus.normal;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.seal:
                            if (target.SSTATUS == SecondaryStatus.seal)
                            {
                                target.SSTATUS = SecondaryStatus.normal;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.poison:
                            if (target.ESTATUS == StatusEffect.poisoned)
                            {
                                target.ESTATUS = StatusEffect.none;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.confusion:
                            if (target.SSTATUS == SecondaryStatus.confusion)
                            {
                                target.SSTATUS = SecondaryStatus.normal;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.paralyze:
                            if (target.ESTATUS == StatusEffect.paralyzed)
                            {
                                target.ESTATUS = StatusEffect.none;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.sleep:
                            if (target.ESTATUS == StatusEffect.sleep)
                            {
                                target.ESTATUS = StatusEffect.none;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.freeze:
                            if (target.ESTATUS == StatusEffect.frozen)
                            {
                                target.ESTATUS = StatusEffect.none;
                                usedEffect = true;
                            }
                            break;
                        case SideEffect.burn:
                            if (target.ESTATUS == StatusEffect.burned)
                            {
                                target.ESTATUS = StatusEffect.none;
                                usedEffect = true;
                            }
                            break;
                    }
                }
                break;
            case ItemType.buff:
                break;
            case ItemType.dmg:
                break;
            case ItemType.actionBoost:
                float chance = Random.value;
                if (chance < trueValue)
                {
                    target.ACTIONS += 3;
                    usedEffect = true;
                }
                else
                {
                    usedEffect = false;
                }
                break;
            case ItemType.random:
                int resultEffect = Random.Range(0, 10);
                int rngVal = (int)trueValue;
                for (int i = 0; i < rngVal; i++)
                {

                    switch (resultEffect)
                    {
                        case 0:
                            usedEffect = target.ChangeHealth((int)(target.MAX_HEALTH * Random.Range(-1.0f, 1.0f)));
                            break;
                        case 1:
                            usedEffect = target.ChangeMana((int)(target.MAX_MANA * Random.Range(-1.0f, 1.0f)));
                            break;
                        case 2:
                            usedEffect = target.ChangeFatigue((int)(target.MAX_FATIGUE * Random.Range(-1.0f, 1.0f)));
                            break;
                        case 3:
                            break;
                        case 4:
                            target.ACTIONS += 1;
                            usedEffect = true;
                            break;
                        case 5:
                            target.ACTIONS += 2;
                            usedEffect = true;
                            break;
                        case 6:
                            target.ACTIONS += 3;
                            usedEffect = true;
                            break;
                        case 7:
                            {

                                CommandSkill randomBuff = ScriptableObject.CreateInstance<CommandSkill>();
                                randomBuff.EFFECT = SideEffect.none;
                                randomBuff.BUFF = (BuffType)Random.Range(1, 6);
                                randomBuff.BUFFVAL = Random.Range(10, 100);
                                randomBuff.ELEMENT = Element.Buff;
                                randomBuff.SUBTYPE = SubSkillType.Buff;

                                target.INVENTORY.BUFFS.Add(randomBuff);
                                BuffScript buff = target.gameObject.AddComponent<BuffScript>();
                                buff.SKILL = randomBuff;
                                buff.BUFF = randomBuff.BUFF;
                                buff.COUNT = 1;
                                target.ApplyPassives();
                            }

                            break;
                        case 8:
                            {
                                CommandSkill randomDeBuff = ScriptableObject.CreateInstance<CommandSkill>();
                                randomDeBuff.EFFECT = SideEffect.none;
                                randomDeBuff.BUFF = (BuffType)Random.Range(1, 6);
                                randomDeBuff.BUFFVAL = Random.Range(-10, -100);
                                randomDeBuff.ELEMENT = Element.Buff;
                                randomDeBuff.SUBTYPE = SubSkillType.Debuff;

                                target.INVENTORY.BUFFS.Add(randomDeBuff);
                                DebuffScript buff = target.gameObject.AddComponent<DebuffScript>();
                                buff.SKILL = randomDeBuff;
                                buff.BUFF = randomDeBuff.BUFF;
                                buff.COUNT = 1;
                                target.ApplyPassives();
                            }
                            break;
                        case 9:
                            usedEffect = target.ChangeHealth((int)(target.MAX_HEALTH * Random.Range(-1.0f, 1.0f)));
                            usedEffect = target.ChangeMana((int)(target.MAX_MANA * Random.Range(-1.0f, 1.0f)));
                            usedEffect = target.ChangeFatigue((int)(target.MAX_FATIGUE * Random.Range(-1.0f, 1.0f)));
                            break;
                        case 10:
                            {
                                CommandSkill randomBuff = ScriptableObject.CreateInstance<CommandSkill>();
                                randomBuff.EFFECT = SideEffect.none;
                                randomBuff.BUFF = (BuffType)Random.Range(1, 6);
                                randomBuff.BUFFVAL = Random.Range(10, 100);
                                randomBuff.ELEMENT = Element.Buff;
                                randomBuff.SUBTYPE = SubSkillType.Buff;

                                target.INVENTORY.BUFFS.Add(randomBuff);
                                BuffScript buff = target.gameObject.AddComponent<BuffScript>();
                                buff.SKILL = randomBuff;
                                buff.BUFF = randomBuff.BUFF;
                                buff.COUNT = 1;
                                target.ApplyPassives();

                                CommandSkill randomDeBuff = ScriptableObject.CreateInstance<CommandSkill>();
                                randomDeBuff.EFFECT = SideEffect.none;
                                randomDeBuff.BUFF = (BuffType)Random.Range(1, 6);
                                randomDeBuff.BUFFVAL = Random.Range(-10, -100);
                                randomDeBuff.ELEMENT = Element.Buff;
                                randomDeBuff.SUBTYPE = SubSkillType.Debuff;

                                target.INVENTORY.BUFFS.Add(randomDeBuff);
                                DebuffScript debuff = target.gameObject.AddComponent<DebuffScript>();
                                debuff.SKILL = randomDeBuff;
                                debuff.BUFF = randomDeBuff.BUFF;
                                debuff.COUNT = 1;
                                target.ApplyPassives();
                            }
                            break;
                    }
                }
                break;
            case ItemType.summon:
                {
                    EnemyManager enemyManager = GameObject.FindObjectOfType<EnemyManager>();
                    ManagerScript manager = GameObject.FindObjectOfType<ManagerScript>();
                    if (enemyManager)
                    {
                        if (manager)
                        {

                            GameObject temp = Instantiate(enemyManager.enemyPrefab, Vector2.zero, Quaternion.identity);
                            if (temp.GetComponent<EnemySetup>())
                            {
                                Destroy(temp.GetComponent<EnemySetup>());
                            }
                            if (temp.GetComponent<EnemySetup>())
                            {
                                EnemySetup es = temp.GetComponent<EnemySetup>();
                                GameObject.DestroyImmediate(es);
                            }
                       
                            if (!temp.GetComponent<LivingSetup>())
                            {
                                temp.gameObject.AddComponent<LivingSetup>();
                            }

                            ActorScript actor = temp.gameObject.AddComponent<ActorScript>();
                            actor.FACTION = Faction.ally;

                            actor.Setup();
                            DatabaseManager database = GameObject.FindObjectOfType<DatabaseManager>();

                            SpriteRenderer shadowRender = actor.SHADOW.GetComponent<SpriteRenderer>();
                            shadowRender.color = Common.GetFactionColor(actor.FACTION) - new Color(0, 0, 0, 0.3f);
                            actor.transform.position = targetTile.transform.position + new Vector3(0, 0.5f, 0);
                            actor.currentTile = targetTile;
                            actor.currentTile.isOccupied = true;
                            actor.gameObject.SetActive(true);
                            for (int i = 0; i < target.LEVEL; i++)
                            {
                                actor.LevelUp();
                            }
                            actor.BASE_STATS.HEALTH = actor.BASE_STATS.MAX_HEALTH;
                            actor.BASE_STATS.MANA = actor.BASE_STATS.MAX_MANA;
                            actor.ACTIONS = 0;
                            actor.BASE_STATS.FATIGUE = 0;
                            manager.gridObjects.Add(actor);
                            manager.SoftReset();
                        }
                        usedEffect = true;
                    }
                }
                break;
        }
        return usedEffect;
    }
}
