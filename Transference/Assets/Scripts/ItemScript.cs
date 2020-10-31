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

    [SerializeField]
    private DMG possibleDMG;

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

    public DMG PDMG
    {
        get { return possibleDMG; }
        set { possibleDMG = value; }
    }

    private LivingObject lastTarget = null;

    public bool useItem(LivingObject target, LivingObject user, TileScript targetTile = null)
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
                    //switch (effect)
                    //{
                    //    case SideEffect.slow:
                    //        if (target.SSTATUS == SecondaryStatus.slow)
                    //        {
                    //            target.SSTATUS = SecondaryStatus.normal;
                    //            usedEffect = true;
                    //        }
                    //        break;
                    //    case SideEffect.rage:
                    //        if (target.SSTATUS == SecondaryStatus.rage)
                    //        {
                    //            target.SSTATUS = SecondaryStatus.normal;
                    //            usedEffect = true;
                    //        }
                    //        break;
                    //    case SideEffect.charm:
                    //        if (target.SSTATUS == SecondaryStatus.charm)
                    //        {
                    //            target.SSTATUS = SecondaryStatus.normal;
                    //            usedEffect = true;
                    //        }
                    //        break;
                    //    case SideEffect.seal:
                    //        if (target.SSTATUS == SecondaryStatus.seal)
                    //        {
                    //            target.SSTATUS = SecondaryStatus.normal;
                    //            usedEffect = true;
                    //        }
                    //        break;
                    //    case SideEffect.poison:
                    //        if (target.ESTATUS == StatusEffect.poisoned)
                    //        {
                    //            target.ESTATUS = StatusEffect.none;
                    //            usedEffect = true;
                    //        }
                    //        break;
                    //    case SideEffect.confusion:
                    //        if (target.SSTATUS == SecondaryStatus.confusion)
                    //        {
                    //            target.SSTATUS = SecondaryStatus.normal;
                    //            usedEffect = true;
                    //        }
                    //        break;
                    //    case SideEffect.paralyze:
                    //        if (target.ESTATUS == StatusEffect.paralyzed)
                    //        {
                    //            target.ESTATUS = StatusEffect.none;
                    //            usedEffect = true;
                    //        }
                    //        break;
                    //    case SideEffect.sleep:
                    //        if (target.ESTATUS == StatusEffect.sleep)
                    //        {
                    //            target.ESTATUS = StatusEffect.none;
                    //            usedEffect = true;
                    //        }
                    //        break;
                    //    case SideEffect.freeze:
                    //        if (target.ESTATUS == StatusEffect.frozen)
                    //        {
                    //            target.ESTATUS = StatusEffect.none;
                    //            usedEffect = true;
                    //        }
                    //        break;
                    //    case SideEffect.burn:
                    //        if (target.ESTATUS == StatusEffect.burned)
                    //        {
                    //            target.ESTATUS = StatusEffect.none;
                    //            usedEffect = true;
                    //        }
                    //        break;
                    //}
                }
                break;
            case ItemType.buff:
                break;
            case ItemType.dmg:
                {
                    if (target)
                    {
                        if (target.FACTION != user.FACTION)
                        {
                            ManagerScript manager = GameObject.FindObjectOfType<ManagerScript>();
                            if (manager)
                            {

                                CommandSkill itemskill = Common.GenericSkill;

                                if (manager.AttackTarget(user, target, itemskill))
                                {
                                    usedEffect = true;
                                }
                            }
                        }
                    }
                }
                break;
            case ItemType.actionBoost:
                float chance = Random.value;
                if (chance < trueValue)
                {
                    target.ACTIONS += 3;
                    usedEffect = true;
                    if (trueValue == 1.0)
                        target.ACTIONS++;
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
                                target.UpdateBuffsAndDebuffs();
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
                                target.UpdateBuffsAndDebuffs();
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
                                target.UpdateBuffsAndDebuffs();

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
                                target.UpdateBuffsAndDebuffs();
                            }
                            break;
                    }
                }
                break;
            case ItemType.summon:
                {
                    if (targetTile == null)
                    {
                        usedEffect = false;
                        return usedEffect;
                    }
                    EnemyManager enemyManager = GameObject.FindObjectOfType<EnemyManager>();
                    ManagerScript manager = GameObject.FindObjectOfType<ManagerScript>();
                    if (enemyManager)
                    {
                        if (manager)
                        {
                            if (manager.GetObjectAtTile(targetTile))
                            {
                                usedEffect = false;
                                return usedEffect;
                            }

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
                                LivingSetup setup = temp.gameObject.AddComponent<LivingSetup>();
                                setup.indexId = (int)VALUE;
                            }

                            ActorScript actor = temp.gameObject.AddComponent<ActorScript>();
                            Debug.Log("idk");

                            actor.FACTION = Faction.ally;

                            actor.Setup();

                            SpriteRenderer shadowRender = actor.SHADOW.GetComponent<SpriteRenderer>();
                            shadowRender.color = Common.GetFactionColor(actor.FACTION) - new Color(0, 0, 0, 0.3f);
                            actor.transform.position = targetTile.transform.position + new Vector3(0, 0.5f, 0);
                            actor.currentTile = targetTile;
                            actor.currentTile.isOccupied = true;
                            actor.currentTileIndex = actor.currentTile.listindex;
                            actor.gameObject.SetActive(true);
                            for (int i = 1; i < target.LEVEL; i++)
                            {
                                actor.LevelUp();
                            }

                            string[] additionals = DEATS.Split(',');
                            int deatindex = 0;
                            int MAX_HEALTH = System.Int32.Parse(additionals[deatindex]);
                            deatindex++;
                            int MAX_MANA = System.Int32.Parse(additionals[deatindex]);
                            deatindex++;
                            int MAX_FATIGUE = System.Int32.Parse(additionals[deatindex]);
                            deatindex++;
                            int MOVE_DIST = System.Int32.Parse(additionals[deatindex]);
                            deatindex++;
                            int STRENGTH = System.Int32.Parse(additionals[deatindex]);
                            deatindex++;
                            int MAGIC = System.Int32.Parse(additionals[deatindex]);
                            deatindex++;
                            int DEX = System.Int32.Parse(additionals[deatindex]);
                            deatindex++;
                            int DEFENSE = System.Int32.Parse(additionals[deatindex]);
                            deatindex++;
                            int RESIESTANCE = System.Int32.Parse(additionals[deatindex]);
                            deatindex++;
                            int SPEED = System.Int32.Parse(additionals[deatindex]);
                            deatindex++;



                            actor.BASE_STATS.MAX_HEALTH = MAX_HEALTH;
                            actor.BASE_STATS.MAX_MANA = MAX_MANA;
                            actor.BASE_STATS.MAX_FATIGUE = MAX_FATIGUE;
                            actor.STATS.HEALTH = MAX_HEALTH;
                            actor.STATS.MANA = MAX_MANA;
                            actor.BASE_STATS.STRENGTH = STRENGTH;
                            actor.BASE_STATS.MAGIC = MAGIC;
                            actor.BASE_STATS.DEX = DEX;
                            actor.BASE_STATS.DEFENSE = DEFENSE;
                            actor.BASE_STATS.RESIESTANCE = RESIESTANCE;
                            actor.BASE_STATS.SPEED = SPEED;
                            DatabaseManager dm = Common.GetDatabase();
                            if (dm)
                            {

                                int weaponCount = System.Int32.Parse(additionals[deatindex]);
                                deatindex++;
                                for (int i = 0; i < weaponCount; i++)
                                {
                                    int weapNum = System.Int32.Parse(additionals[deatindex]);
                                    deatindex++;
                                    int weapLevel = System.Int32.Parse(additionals[deatindex]);
                                    deatindex++;
                                    if (i < actor.INVENTORY.WEAPONS.Count)
                                    {
                                        WeaponScript wep = actor.INVENTORY.WEAPONS[i];
                                        if (wep.INDEX == weapNum)
                                        {
                                            if (wep.LEVEL < weapLevel)
                                            {
                                                while (wep.LEVEL < weapLevel)
                                                {
                                                    wep.LevelUP();
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        WeaponScript wep2 = dm.GetWeapon(weapNum, actor);
                                        if (wep2)
                                        {
                                            if (wep2.LEVEL < weapLevel)
                                            {
                                                while (wep2.LEVEL < weapLevel)
                                                {
                                                    wep2.LevelUP();
                                                }
                                            }
                                        }
                                    }

                                }

                                int armorCount = System.Int32.Parse(additionals[deatindex]);
                                deatindex++;
                                for (int i = 0; i < armorCount; i++)
                                {
                                    int armNum = System.Int32.Parse(additionals[deatindex]);
                                    deatindex++;
                                    int armLevel = System.Int32.Parse(additionals[deatindex]);
                                    deatindex++;
                                    if (i < actor.INVENTORY.ARMOR.Count)
                                    {
                                        ArmorScript arm = actor.INVENTORY.ARMOR[i];
                                        if (arm.INDEX == armNum)
                                        {
                                            if (arm.LEVEL < armLevel)
                                            {
                                                while (arm.LEVEL < armLevel)
                                                {
                                                    arm.LevelUP();
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ArmorScript arm2 = dm.GetArmor(armNum, actor);
                                        if (arm2)
                                        {
                                            if (arm2.LEVEL < armLevel)
                                            {
                                                while (arm2.LEVEL < armLevel)
                                                {
                                                    arm2.LevelUP();
                                                }
                                            }
                                        }
                                    }


                                }


                                int cmdCount = System.Int32.Parse(additionals[deatindex]);
                                deatindex++;
                                for (int i = 0; i < cmdCount; i++)
                                {
                                    int cmdNum = System.Int32.Parse(additionals[deatindex]);
                                    deatindex++;
                                    int cmdLevel = System.Int32.Parse(additionals[deatindex]);
                                    deatindex++;

                                    if (i < actor.INVENTORY.CSKILLS.Count)
                                    {
                                        CommandSkill cmd = actor.INVENTORY.CSKILLS[i];
                                        if (cmd.INDEX == cmdNum)
                                        {
                                            if (cmd.LEVEL < cmdLevel)
                                            {
                                                while (cmd.LEVEL < cmdLevel)
                                                {
                                                    cmd.LevelUP();
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        CommandSkill cmd2 = dm.LearnSkill(cmdNum, actor) as CommandSkill;
                                        if (cmd2)
                                        {
                                            if (cmd2.LEVEL < cmdLevel)
                                            {
                                                while (cmd2.LEVEL < cmdLevel)
                                                {
                                                    cmd2.LevelUP();
                                                }
                                            }
                                        }
                                    }



                                }

                                int passCount = System.Int32.Parse(additionals[deatindex]);
                                deatindex++;
                                for (int i = 0; i < passCount; i++)
                                {
                                    int passNum = System.Int32.Parse(additionals[deatindex]);
                                    deatindex++;
                                    int passLevel = System.Int32.Parse(additionals[deatindex]);
                                    deatindex++;


                                    if (actor.INVENTORY.COMBOS.Count < i)
                                    {
                                        ComboSkill pass = actor.INVENTORY.COMBOS[i];
                                        if (pass.INDEX == passNum)
                                        {
                                            if (pass.LEVEL < passLevel)
                                            {
                                                while (pass.LEVEL < passLevel)
                                                {
                                                    pass.LevelUP();
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ComboSkill pass2 = dm.LearnSkill(passNum, actor) as ComboSkill;
                                        if (pass2)
                                        {
                                            if (pass2.LEVEL < passLevel)
                                            {
                                                while (pass2.LEVEL < passLevel)
                                                {
                                                    pass2.LevelUP();
                                                }
                                            }
                                        }
                                    }


                                }

                                int autoCount = System.Int32.Parse(additionals[deatindex]);
                                deatindex++;
                                for (int i = 0; i < autoCount; i++)
                                {
                                    int autoNum = System.Int32.Parse(additionals[deatindex]);
                                    deatindex++;
                                    int autoLevel = System.Int32.Parse(additionals[deatindex]);
                                    deatindex++;

                                    if (actor.INVENTORY.AUTOS.Count < i)
                                    {
                                        AutoSkill autos = actor.INVENTORY.AUTOS[i];
                                        if (autos.INDEX == autoNum)
                                        {
                                            if (autos.LEVEL < autoLevel)
                                            {
                                                while (autos.LEVEL < autoLevel)
                                                {
                                                    autos.LevelUP();
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        AutoSkill autos2 = dm.LearnSkill(autoNum, actor) as AutoSkill;
                                        if (autos2)
                                        {
                                            if (autos2.LEVEL < autoLevel)
                                            {
                                                while (autos2.LEVEL < autoLevel)
                                                {
                                                    autos2.LevelUP();
                                                }
                                            }
                                        }
                                    }
                                }

                                int oppCount = System.Int32.Parse(additionals[deatindex]);
                                deatindex++;
                                for (int i = 0; i < oppCount; i++)
                                {
                                    int oppNum = System.Int32.Parse(additionals[deatindex]);
                                    deatindex++;
                                    int oppLevel = System.Int32.Parse(additionals[deatindex]);
                                    deatindex++;

                                    if (actor.INVENTORY.OPPS.Count < i)
                                    {
                                        OppSkill autos = actor.INVENTORY.OPPS[i];
                                        if (autos.INDEX == oppNum)
                                        {
                                            if (autos.LEVEL < oppLevel)
                                            {
                                                while (autos.LEVEL < oppLevel)
                                                {
                                                    autos.LevelUP();
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        OppSkill autos2 = dm.LearnSkill(oppNum, actor) as OppSkill;
                                        if (autos2)
                                        {
                                            if (autos2.LEVEL < oppLevel)
                                            {
                                                while (autos2.LEVEL < oppLevel)
                                                {
                                                    autos2.LevelUP();
                                                }
                                            }
                                        }
                                    }

                                }

                            }
                            else
                            {
                                Debug.Log("no dm...");
                            }
                            actor.ACTIONS = 0;
                            actor.STATS.FATIGUE = 0;
                            actor.BASE_STATS.EXP = 0;
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

    public bool useItem(GridObject target, LivingObject user, TileScript targetTile = null)
    {
        bool usedEffect = false;
        switch (ITYPE)
        {

            case ItemType.dmg:
                {
                    if (target)
                    {
                        if (target.FACTION != user.FACTION)
                        {
                            ManagerScript manager = GameObject.FindObjectOfType<ManagerScript>();
                            if (manager)
                            {

                                CommandSkill itemskill = Common.GenericSkill;
                                itemskill.ACCURACY = 100;
                                itemskill.COST = 0;
                                itemskill.ELEMENT = ELEMENT;
                                itemskill.DAMAGE = PDMG;
                                itemskill.HITS = 1;
                                itemskill.SKILLTYPE = SkillType.Command;
                                itemskill.SUBTYPE = SubSkillType.Item;
                                itemskill.OWNER = user;
                                itemskill.NAME = NAME;
                                if (manager.AttackTarget(user, target, itemskill))
                                {
                                    usedEffect = true;
                                }
                            }
                        }
                    }
                }
                break;

            case ItemType.dart:
                {
                    if (!target.GetComponent<LivingObject>())
                    {
                        return false;
                    }

                    LivingObject liveTarget = target.GetComponent<LivingObject>();
                    ManagerScript manager = GameObject.FindObjectOfType<ManagerScript>();
                    CommandSkill itemskill = Common.GenericSkill;
                    itemskill.ACCURACY = 100;
                    itemskill.EFFECT = EFFECT;
                    itemskill.SUBTYPE = SubSkillType.Ailment;
                    itemskill.OWNER = user;
                    itemskill.NAME = NAME;
                    if (manager)
                    {
                        GridAnimationObj gao = null;
                        gao = manager.PrepareGridAnimation(null, target);
                        gao.type = 5;
                        gao.magnitute = 0;
                        gao.LoadGridAnimation();

                        manager.menuManager.ShowNone();
                        manager.CreateEvent(this, gao, "Animation request: " + manager.AnimationRequests + "", manager.CheckAnimation, gao.StartCountDown, 0);

                        manager.CreateTextEvent(this, "", "wait event", manager.CheckText, manager.TextStart);
                        if (manager.log)
                        {
                            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(owner.FACTION)) + ">";
                            manager.log.Log(coloroption + NAME + "</color> used " + myName);
                        }

                        lastTarget = liveTarget;
                        manager.CreateEvent(this, gao, "Neo wait action", ExecuteEffect,null,0);
                        usedEffect = true;
                    }
                }
                break;
            default:
                if (target.GetComponent<LivingObject>())
                {
                    return useItem(target as LivingObject, user, targetTile);
                }
                break;
        }
        return usedEffect;
    }
    public bool ExecuteEffect(Object data)
    {
        CommandSkill itemskill = data as CommandSkill;
        ManagerScript manager = GameObject.FindObjectOfType<ManagerScript>();
        manager.ApplyEffect(lastTarget, EFFECT, 100, itemskill);
        return true;
    }
    public override void UpdateDesc()
    {
        base.UpdateDesc();
        switch (ITYPE)
        {
            case ItemType.healthPotion:
                DESC = "Restores " + (100.0f * trueValue) + "% of Health to ally or self.";
                break;
            case ItemType.manaPotion:
                DESC = "Restores " + (100.0f * trueValue) + "% of Mana to ally or self.";
                break;
            case ItemType.fatiguePotion:
                {
                    if (trueValue < 0)
                    {
                        DESC = "Increases " + (100.0f * trueValue) + "% of Fatigue to ally or self.";
                    }
                    else
                    {
                        DESC = "Restores " + (100.0f * trueValue) + "% of Fatigue to ally or self.";
                    }
                }
                break;
            case ItemType.cure:
                break;
            case ItemType.buff:
                break;
            case ItemType.dmg:
                DESC = "Deals heavy magical " + ELEMENT + " Damage to target";
                break;
            case ItemType.dart:
                DESC = "Throws a " + EFFECT + " dart at target. " + Common.GetSideEffectText(EFFECT);
                break;
            case ItemType.actionBoost:
                if (trueValue == 1.0f)
                {
                    DESC = "Grants 3 additional action points to ally or self.";
                }
                else
                {
                    DESC = "" + (100.0f * trueValue) + "% chance to grant 2 additional action points to ally or self.";
                }
                break;
            case ItemType.random:
                DESC = "Grants " + trueValue + "random effect to ally or self";
                break;
            case ItemType.summon:
                DESC = "Summons a " + NAME.Split(' ')[0] + " as an ally. Cannot be used if you already have a summon.";

                break;
        }
    }
}
