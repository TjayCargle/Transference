﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : LivingObject
{
    public EPType personality;
    GridObject atkTarget = null;
    //public Queue<TileScript> currentPath;
    public bool isPerforming = false;
    public LivingObject currentEnemy;
    public int headCount = 0;
    public int psudeoActions = 0;

    // TileScript targetTile;
    ManagerScript manager;
    public TileScript nextTile;
    private float internalTimer = 1.0f;
    [SerializeField]
    path currentPath;
    protected bool waiting = false;
    private List<AtkContainer> possibleAttacks = new List<AtkContainer>();
    private List<CommandSkill> reflectedSkills = new List<CommandSkill>();
    private List<ItemContainer> possibleItems = new List<ItemContainer>();
    public List<string> atkNames = new List<string>();
    private int chosen = -1;
    public DmgReaction lastReaction;
    protected AtkContainer lastAttack;
    protected List<GridObject> adjacentObjects = new List<GridObject>();
    protected List<int> possibleIndexs = new List<int>();
    public bool enemyHitMyResist = false;
    [SerializeField]
    protected TalkStage talk = TalkStage.initial;
    private Element talkElement;
    private List<TileScript> personalAttackList = new List<TileScript>();
    public TalkStage TALK
    {
        get { return talk; }
        set { talk = value; }
    }

    public void MoveStart(Object checkPath)
    {
        path apath = checkPath as path;
        currentPath = apath;

        internalTimer = 1.0f;
        myManager.CreateEvent(this, this, "Select Camera Event", myManager.CameraEvent, null, 0);
        myManager.ShowSelectedTile(currentPath.realTarget, Color.magenta);
        myManager.myCamera.UpdateCamera();

    }
    public void AttackStart()
    {
        internalTimer = 1.0f;
        myManager.CreateEvent(this, this, "Select Camera Event", myManager.CameraEvent, null, 0);
        myManager.myCamera.UpdateCamera();
        lastReaction = DetermineBestDmgOutput();
    }
    public void ItemStart()
    {
        internalTimer = 1.0f;
        myManager.CreateEvent(this, this, "Select Camera Event", myManager.CameraEvent, null, 0);
        myManager.myCamera.UpdateCamera();

    }
    public override void Setup()
    {
        if (!isSetup)
        {
            base.Setup();

            manager = GameObject.FindObjectOfType<ManagerScript>();
            //currentPath = new Queue<TileScript>();
            isSetup = true;
        }
    }

    public bool MoveEvent(Object target)
    {
        if (internalTimer >= 0)
        {
            internalTimer -= 0.02f;
            return false;
        }

        bool isDone = false;
        path pathTarget = target as path;

        TileScript realTarget = pathTarget.realTarget;

        pathTarget.currentPath = DeterminePath(pathTarget);
        headCount = pathTarget.currentPath.Count;
        if (headCount == 0)
        {
            isDone = true;
            TakeRealAction();
            if (ACTIONS <= 0)
            {
                isPerforming = false;
            }
            myManager.myCamera.currentTile = currentTile;
            return isDone;
        }
        nextTile = pathTarget.currentPath.Peek();
        //  myManager.myCamera.currentTile = nextTile;
        // myManager.myCamera.infoObject = this;

        Vector3 directionVector = (nextTile.transform.position - transform.position);
        directionVector.y = 0.0f;

        float dist = Vector3.Distance(nextTile.transform.position, transform.position);
        if (dist > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextTile.transform.position, 0.09f);
            //transform.Translate(directionVector * 0.5f);
        }
        else
        {
            directionVector = nextTile.transform.position;
            directionVector.y = nextTile.transform.position.y + 0.5f;
            transform.position = directionVector;
        }

        if (transform.position.x == nextTile.transform.position.x)
        {
            if (transform.position.z == nextTile.transform.position.z)
            {

                currentTile.isOccupied = false;
                currentTile = pathTarget.currentPath.Dequeue();
                currentTile.isOccupied = true;


            }
        }
        if (pathTarget.currentPath.Count == 0)
        {
            isDone = true;
            TakeRealAction();
            if (ACTIONS <= 0)
            {
                isPerforming = false;
            }
            //Debug.Log("Move Event Done!");
        }

        myManager.myCamera.currentTile = currentTile;
        return isDone;
    }
    public bool EWaitEvent(Object target)
    {

        bool isDone = true;

        Wait();
       // TakeRealAction();

        return isDone;
    }
    public bool RunAwayEvent(Object target)
    {
        if (!DEAD)
        {
            DEAD = true;
            myManager.gridObjects.Remove(this);
            Wait();
            this.Die();
        }
        TakeRealAction();
        return true;
    }
    public bool EAtkEvent(Object target)
    {
        if (lastReaction.usedSkill == null)
        {
            if (lastAttack != null)
                myManager.ShowWeaponAttackbleTiles(this, lastReaction.usedStrike, lastAttack.dmgObject);
        }
        else
        {
            if (lastAttack != null)
                myManager.ShowSkillAttackbleTiles(this, lastReaction.usedSkill, lastAttack.dmgObject);
        }
        if (internalTimer >= 0)
        {
            internalTimer -= 0.02f;
            return false;
        }

        if (lastReaction.usedStrike)
        {
            WEAPON.Equip(lastReaction.usedStrike);
            myManager.attackableTiles = myManager.GetWeaponAttackableTiles(this);
            //GridObject possibleObject = null;
            myManager.currentAttackList.Clear();
            possibleIndexs.Clear();
            personalAttackList.Clear();
            if (lastAttack.dmgObject)
                personalAttackList.Add(lastAttack.dmgObject.currentTile);

            myManager.currentAttackList.AddRange(personalAttackList);

            if (myManager.currentAttackList.Count > 0)
            {
            }
            else
            {
                Debug.Log("we are confusing");
            }
        }

        if (lastReaction.usedSkill)
        {
            WEAPON.Equip(lastReaction.usedStrike);
            myManager.attackableTiles = myManager.GetSkillsAttackableTiles(this, lastReaction.usedSkill);
           // GridObject possibleObject = null;
            myManager.currentAttackList.Clear();
            personalAttackList.Clear();
            if (lastAttack.dmgObject)
                personalAttackList.Add(lastAttack.dmgObject.currentTile);

            myManager.currentAttackList.AddRange(personalAttackList);

            if (myManager.currentAttackList.Count > 0)
            {
            }
            else
            {
                Debug.Log("we are confusing");
            }
        }
        // Debug.Log(FullName + " atacking");
        bool isDone = true;
        if (ACTIONS > 0)
        {
            //myManager.MoveCameraAndShow(this);
            // LivingObject realTarget = target as LivingObject;
            if (SSTATUS == SecondaryStatus.confusion)
            {
                int chance = Random.Range(0, 2);

                if (chance <= 2)
                {
                    Debug.Log("They hit themselves");
                    if (manager.log)
                    {
                        manager.log.Log(FullName + " hit themselves ");
                    }
                    atkTarget = this;

                }
            }

            DmgReaction bestReaction = lastReaction;//DetermineBestDmgOutput();
            lastReaction = bestReaction;
            manager.CreateTextEvent(this, "" + FullName + " used " + bestReaction.atkName, "enemy atk", manager.CheckText, manager.TextStart);
            if (manager.log)
            {
                manager.log.Log("<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(FACTION)) + ">" + FullName + "</color> used " + bestReaction.atkName);
            }

            int health = atkTarget.STATS.HEALTH;
            if (atkTarget.GetComponent<LivingObject>())
            {
                health = (atkTarget as LivingObject).HEALTH;
            }
            else
            {
                health = atkTarget.STATS.HEALTH;
            }

            if (bestReaction.usedSkill != null)
            {
                //for (int i = 0; i < bestReaction.usedSkill.HITS; i++)
                //{
                //    if (health > 0)
                //    {

                //        if (i == 0)
                //        {

                //            lastAttack.react = bestReaction;
                //            // myManager.ApplyReaction(this, atkTarget, bestReaction, bestReaction.dmgElement, lastAttack.command);
                //            myManager.CreateGridAnimationEvent(lastAttack.dmgObject.currentTile.transform.position, lastAttack.command, lastAttack.dmg);
                //            myManager.CreateEvent(this, lastAttack, "apply reaction event", myManager.ApplyReactionEvent, null, 0);
                //            if (bestReaction.reaction != Reaction.missed && bestReaction.reaction != Reaction.nulled && bestReaction.reaction != Reaction.absorb && bestReaction.reaction != Reaction.reflected)
                //            {

                //                health -= bestReaction.damage;
                //                Debug.Log("first cmd");
                //            }
                //        }
                //        else
                //        {
                //            if (lastAttack == null)
                //            {
                //                Debug.Log("last attack null in enemey");

                //            }
                //            DmgReaction secondReaction = myManager.CalcDamage(lastAttack);
                //            lastAttack.react = secondReaction;
                //            myManager.CreateEvent(this, lastAttack, "apply reaction event", myManager.ApplyReactionEvent, null, 0);
                //            //       Debug.Log("other cmd");
                //            //  myManager.ApplyReaction(this, atkTarget, secondReaction, secondReaction.dmgElement);
                //            if (secondReaction.reaction != Reaction.missed && secondReaction.reaction != Reaction.nulled && secondReaction.reaction != Reaction.absorb && secondReaction.reaction != Reaction.reflected)
                //            {

                //                health -= secondReaction.damage;
                //                Debug.Log("additional hit");
                //            }
                //        }
                //    }

                //}
                //bestReaction.usedSkill.GrantXP(1);

                lastAttack.react = bestReaction;
                myManager.AttackTargets(this, lastReaction.usedSkill, true);
            }
            else if (bestReaction.usedStrike != null)
            {
                lastAttack.react = bestReaction;
                //myManager.CreateGridAnimationEvent(lastAttack.dmgObject.currentTile.transform.position, bestReaction.usedStrike, lastAttack.dmg);
                //myManager.CreateEvent(this, lastAttack, "apply reaction event", myManager.ApplyReactionEvent, null, 0);
                //bestReaction.usedStrike.GrantXP(1);
                myManager.AttackTargets(this, WEAPON, true);
                Debug.Log("a strike");
                //myManager.ApplyReaction(this, atkTarget, bestReaction, bestReaction.dmgElement);
            }
            else
            {
                Debug.Log("Enemy fked up");
            }
            myManager.myCamera.infoObject = atkTarget;
            myManager.myCamera.UpdateCamera();
            if (bestReaction.reaction != Reaction.missed)
            {
                AtkContainer conatiner = ScriptableObject.CreateInstance<AtkContainer>();
                conatiner.attackingObject = this;
                conatiner.dmgObject = atkTarget;
                conatiner.attackingElement = bestReaction.dmgElement;
                //Debug.Log("checkin real" + atkTarget);
                myManager.CreateEvent(this, conatiner, "" + FullName + "enemy opp event", myManager.ECheckForOppChanceEvent, null, 1);

            }
            if (bestReaction.reaction >= Reaction.nulled && bestReaction.reaction <= Reaction.absorb)
            {
                if (!reflectedSkills.Contains(bestReaction.usedSkill))
                {
                    reflectedSkills.Add(bestReaction.usedSkill);
                }
            }
            //  Debug.Log(FullName + " used " + bestReaction.atkName);

            TakeRealAction();

        }
        return isDone;
    }
    public bool UseItemEvent(Object target)
    {
        if (internalTimer >= 0)
        {
            internalTimer -= 0.02f;
            return false;
        }
        bool isDone = true;

        if (possibleItems.Count > 0)
        {
            ItemScript usingItem = possibleItems[0].item;
            if (possibleItems[0].item.useItem(possibleItems[0].target, this))
            {
                this.INVENTORY.ITEMS.Remove(usingItem);
                this.INVENTORY.USEABLES.Remove(usingItem);
            }
        }
        else
        {
            Debug.Log("possibleItems are 0....");
        }
        TakeRealAction();
        return isDone;
    }
    public Queue<TileScript> DeterminePath(path pathTarget)
    {
        float timer = Time.deltaTime;

        if (pathTarget.currentPath.Count == 0)
        {
            int depth = 0;
            currentTile.isOccupied = false;
            //  targ = target;
            // Debug.Log(FullName + " is Determining path");
            bool complete = false;
            TileScript current = currentTile;//myManager.GetTileAtIndex(myManager.GetTileIndex(calcLocation));
            while (complete == false)
            {
                List<TileScript> options = myManager.GetAdjecentTiles(current);
                if (options.Count > 0)
                    current = options[0];
                for (int i = 0; i < options.Count; i++)
                {
                    TileScript checkTile = options[i];

                    if (checkTile.isOccupied == false)
                    {

                        if (Vector3.Distance(pathTarget.realTarget.transform.position, checkTile.transform.position) < Vector3.Distance(pathTarget.realTarget.transform.position, current.transform.position))
                        {
                            current = checkTile;
                        }
                    }
                    else
                    {
                        if (manager.GetObjectAtTile(checkTile))
                        {

                            if (manager.GetObjectAtTile(checkTile))
                            {
                                if (manager.GetObjectAtTile(checkTile).FACTION == this.FACTION)
                                {
                                    GridObject living = manager.GetObjectAtTile(checkTile);
                                    if (Vector3.Distance(pathTarget.realTarget.transform.position, checkTile.transform.position) < Vector3.Distance(pathTarget.realTarget.transform.position, current.transform.position))
                                    {
                                        current = checkTile;
                                    }
                                }
                            }
                        }
                    }
                }

                if (!pathTarget.currentPath.Contains(current))
                {
                    //  Debug.Log(FullName + " Current location = " + current.transform.position);
                    if (current.isOccupied == false)
                    {
                        pathTarget.currentPath.Enqueue(current);

                    }

                }
                if (current == pathTarget.realTarget)
                {
                    complete = true;
                    break;
                }
                depth++;
                if (depth > 200)
                {
                    Debug.Log("TOO MUCH SEARCH");
                    Debug.Log("target location = " + pathTarget.realTarget.transform.position);
                    pathTarget.realTarget = current;
                    complete = true;
                    break;

                }
                //if (timer - Time.deltaTime < 25.0f)
                //{
                //    Debug.Log("Time took longer than 25 seconds");
                //    Wait();
                //    break;
                //}
            }
        }

        return pathTarget.currentPath;
    }
    public bool FindEnemiesInRangeOfAttacks(Vector3 location)
    {
        bool foundEnemy = false;

        LivingObject[] objects = GameObject.FindObjectsOfType<LivingObject>();
        int skip = -1;
        int consider = -1;
        if (personality == EPType.support)
        {
            for (int j = 0; j < INVENTORY.CSKILLS.Count; j++)
            {
                CommandSkill possibleSkill = INVENTORY.CSKILLS[j];
                if (possibleSkill.SUBTYPE == SubSkillType.Buff && possibleSkill.CanUse())
                {
                    if (!INVENTORY.BUFFS.Contains(possibleSkill))
                    {
                        foundEnemy = true;
                        AtkContainer conatiner = ScriptableObject.CreateInstance<AtkContainer>();
                        conatiner.alteration = possibleSkill.REACTION;
                        conatiner.attackingElement = possibleSkill.ELEMENT;
                        conatiner.attackType = possibleSkill.ETYPE;
                        conatiner.attackingObject = this;
                        conatiner.dmgObject = this;
                        conatiner.command = possibleSkill;
                        conatiner.dmg = (int)possibleSkill.DAMAGE;

                        possibleAttacks.Add(conatiner);
                        atkNames.Add(possibleSkill.NAME + " " + this.NAME);

                    }
                }
            }
        }
        for (int i = 0; i < objects.Length; i++)
        {
            if (personality == EPType.finisher)
            {
                if (atkTarget)
                {
                    if (atkTarget.DEAD != true)
                    {

                        if (objects[i] != atkTarget)
                        {
                            continue;
                        }
                    }
                }
            }
            if (objects[i].FACTION != this.FACTION)
            {
                if (!objects[i].DEAD)
                {
                    if (personality == EPType.mystical || personality == EPType.tactical || personality == EPType.support)
                    {
                        consider = Random.Range(0, 3);
                        if (consider != 0)
                        {
                            skip = 0;
                            // Debug.Log("should skip basic");
                        }
                    }
                    if (reflectedSkills.Contains(null))
                    {
                        skip = 0;
                    }
                    if (skip != 0)
                    {
                        // if (Vector3.Distance(location, objects[i].transform.position) == Atk_DIST)
                        for (int j = 0; j < INVENTORY.WEAPONS.Count; j++)
                        {
                            WeaponScript possibleSkill = INVENTORY.WEAPONS[j];
                            if (possibleSkill.CanUse())
                            {
                                bool reflected = false;
                                for (int r = 0; r < reflectedSkills.Count; r++)
                                {
                                    if (reflectedSkills[r].ELEMENT == possibleSkill.ELEMENT)
                                    {
                                        reflected = true;
                                        break;
                                    }
                                }
                                if (reflected == true)
                                {
                                    continue;
                                }
                                //  Debug.Log("Can use " + possibleSkill.NAME + " with " + HEALTH + " hp, " + MANA + " mp, " + FATIGUE + "ft");

                                List<TileScript> skilltiles = myManager.GetWeaponAttackableTilesOneList(currentTile, possibleSkill);

                                if (skilltiles.Contains(objects[i].currentTile))
                                {
                                    foundEnemy = true;
                                    AtkContainer conatiner = ScriptableObject.CreateInstance<AtkContainer>();
                                    conatiner.dmgObject = objects[i];
                                    conatiner.command = null;


                                    conatiner.alteration = Reaction.none; //atkReaction;
                                    conatiner.attackingElement = INVENTORY.WEAPONS[j].ELEMENT;
                                    conatiner.attackType = INVENTORY.WEAPONS[j].ATTACK_TYPE;
                                    conatiner.attackingObject = this;
                                    //WEAPON.Equip(INVENTORY.WEAPONS[j]);
                                    conatiner.dmg = (int)INVENTORY.WEAPONS[j].ATTACK + INVENTORY.WEAPONS[j].LEVEL;// WEAPON.ATTACK;
                                    conatiner.strike = INVENTORY.WEAPONS[j];


                                    possibleAttacks.Add(conatiner);
                                    atkNames.Add(INVENTORY.WEAPONS[j].NAME + " " + objects[i].NAME);
                                }
                                // potentialTargets.Add(objects[i]);
                            }
                        }
                    }
                    for (int j = 0; j < INVENTORY.CSKILLS.Count; j++)
                    {
                        CommandSkill possibleSkill = INVENTORY.CSKILLS[j];
                        if (possibleSkill.CanUse())
                        {
                            //    Debug.Log("Can use " + possibleSkill.NAME + " with " + HEALTH+ " hp, "+ MANA+ " mp, "+  FATIGUE + "ft") ;
                            if (objects[i].INVENTORY.BUFFS.Contains(possibleSkill))
                            {
                                continue;
                            }
                            if (objects[i].INVENTORY.DEBUFFS.Contains(possibleSkill))
                            {
                                continue;
                            }
                            if (reflectedSkills.Contains(possibleSkill))
                            {
                                continue;
                            }

                            if (objects[i].GetComponent<HazardScript>())
                            {
                                if (possibleSkill.ELEMENT == Element.Buff)
                                {
                                    continue;
                                }
                            }
                            if (personality == EPType.support)
                            {
                                if (possibleSkill.SUBTYPE == SubSkillType.Buff)
                                {
                                    continue;
                                }

                                if (possibleSkill.ELEMENT != Element.Buff)
                                {
                                    consider = Random.Range(0, 3);
                                    if (consider != 0)
                                    {

                                        continue;
                                    }
                                }
                                else
                                {
                                    if (objects[i].INVENTORY.BUFFS.Contains(possibleSkill))
                                    {
                                        continue;
                                    }
                                    if (objects[i].INVENTORY.DEBUFFS.Contains(possibleSkill))
                                    {
                                        continue;
                                    }

                                }
                            }
                            else if (possibleSkill.ETYPE == EType.magical)
                            {
                                if (personality == EPType.forceful || personality == EPType.tactical)
                                {
                                    consider = Random.Range(0, 3);
                                    if (consider != 0)
                                    {
                                        //Debug.Log("should skip magic");
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                if (personality == EPType.forceful || personality == EPType.mystical)
                                {
                                    consider = Random.Range(0, 3);
                                    if (consider != 0)
                                    {
                                        //Debug.Log("should skip skill");
                                        continue;
                                    }
                                }
                            }
                            if (possibleSkill.SUBTYPE == SubSkillType.Buff)
                            {
                                if (!INVENTORY.BUFFS.Contains(possibleSkill))
                                {
                                    foundEnemy = true;
                                    AtkContainer conatiner = ScriptableObject.CreateInstance<AtkContainer>();
                                    conatiner.alteration = possibleSkill.REACTION;
                                    conatiner.attackingElement = possibleSkill.ELEMENT;
                                    conatiner.attackType = possibleSkill.ETYPE;
                                    conatiner.attackingObject = this;
                                    conatiner.dmgObject = this;
                                    conatiner.command = possibleSkill;
                                    conatiner.dmg = (int)possibleSkill.DAMAGE;

                                    possibleAttacks.Add(conatiner);
                                    atkNames.Add(possibleSkill.NAME + " " + this.NAME);

                                }
                            }
                            else
                            {
                                List<TileScript> skilltiles = myManager.GetSkillAttackableTilesOneList(currentTile, possibleSkill);

                                if (skilltiles.Contains(objects[i].currentTile))
                                {

                                    foundEnemy = true;
                                    AtkContainer conatiner = ScriptableObject.CreateInstance<AtkContainer>();
                                    conatiner.alteration = possibleSkill.REACTION;
                                    conatiner.attackingElement = possibleSkill.ELEMENT;
                                    conatiner.attackType = possibleSkill.ETYPE;
                                    conatiner.attackingObject = this;
                                    conatiner.dmgObject = objects[i];
                                    conatiner.command = possibleSkill;
                                    conatiner.dmg = (int)possibleSkill.DAMAGE;

                                    possibleAttacks.Add(conatiner);
                                    atkNames.Add(possibleSkill.NAME + " " + objects[i].NAME);

                                }
                            }

                        }
                    }
                }
            }
        }
        return foundEnemy;
    }
    public bool FoundItemsCanUse()
    {
        bool found = false;

        for (int i = 0; i < INVENTORY.ITEMS.Count; i++)
        {

            switch (INVENTORY.ITEMS[i].ITYPE)
            {
                case ItemType.healthPotion:
                    {
                        if (HEALTH < MAX_HEALTH)
                        {
                            ItemContainer container = ScriptableObject.CreateInstance<ItemContainer>();
                            container.item = INVENTORY.ITEMS[i];
                            container.target = this;
                            possibleItems.Add(container);
                            found = true;
                        }
                        for (int j = 0; j < adjacentObjects.Count; j++)
                        {
                            if (adjacentObjects[j].FACTION == this.FACTION && adjacentObjects[j].GetComponent<EnemyScript>())
                            {
                                EnemyScript fellowEnemy = adjacentObjects[j] as EnemyScript;
                                if (fellowEnemy.HEALTH < fellowEnemy.MAX_HEALTH)
                                {
                                    ItemContainer container = ScriptableObject.CreateInstance<ItemContainer>();
                                    container.item = INVENTORY.ITEMS[i];
                                    container.target = this;
                                    possibleItems.Add(container);
                                    found = true;
                                }
                            }
                        }
                    }
                    break;
                case ItemType.manaPotion:
                    {
                        if (MANA < MAX_MANA)
                        {
                            ItemContainer container = ScriptableObject.CreateInstance<ItemContainer>();
                            container.item = INVENTORY.ITEMS[i];
                            container.target = this;
                            possibleItems.Add(container);
                            found = true;
                        }
                        for (int j = 0; j < adjacentObjects.Count; j++)
                        {
                            if (adjacentObjects[j].FACTION == this.FACTION && adjacentObjects[j].GetComponent<EnemyScript>())
                            {
                                EnemyScript fellowEnemy = adjacentObjects[j] as EnemyScript;
                                if (fellowEnemy.MANA < fellowEnemy.MAX_MANA)
                                {
                                    ItemContainer container = ScriptableObject.CreateInstance<ItemContainer>();
                                    container.item = INVENTORY.ITEMS[i];
                                    container.target = this;
                                    possibleItems.Add(container);
                                    found = true;
                                }
                            }
                        }
                    }
                    break;
                case ItemType.fatiguePotion:
                    {
                        if (FATIGUE > 0)
                        {
                            ItemContainer container = ScriptableObject.CreateInstance<ItemContainer>();
                            container.item = INVENTORY.ITEMS[i];
                            container.target = this;
                            possibleItems.Add(container);
                            found = true;
                        }
                        for (int j = 0; j < adjacentObjects.Count; j++)
                        {
                            if (adjacentObjects[j].FACTION == this.FACTION && adjacentObjects[j].GetComponent<EnemyScript>())
                            {
                                EnemyScript fellowEnemy = adjacentObjects[j] as EnemyScript;
                                if (fellowEnemy.FATIGUE > 0)
                                {
                                    ItemContainer container = ScriptableObject.CreateInstance<ItemContainer>();
                                    container.item = INVENTORY.ITEMS[i];
                                    container.target = this;
                                    possibleItems.Add(container);
                                    found = true;
                                }
                            }
                        }
                    }
                    break;
                case ItemType.cure:
                    break;
                case ItemType.buff:
                    break;
                case ItemType.dmg:
                    {

                        for (int j = 0; j < adjacentObjects.Count; j++)
                        {
                            if (adjacentObjects[j].FACTION != this.FACTION)
                            {
                                ItemContainer container = ScriptableObject.CreateInstance<ItemContainer>();
                                container.item = INVENTORY.ITEMS[i];
                                container.target = adjacentObjects[j];
                                possibleItems.Add(container);
                                found = true;
                            }
                        }
                    }
                    break;
                case ItemType.actionBoost:
                    {
                        if (ACTIONS > 0)
                        {
                            ItemContainer container = ScriptableObject.CreateInstance<ItemContainer>();
                            container.item = INVENTORY.ITEMS[i];
                            container.target = this;
                            possibleItems.Add(container);
                            found = true;
                        }
                        for (int j = 0; j < adjacentObjects.Count; j++)
                        {
                            if (adjacentObjects[j].FACTION == this.FACTION && adjacentObjects[j].GetComponent<EnemyScript>())
                            {
                                EnemyScript fellowEnemy = adjacentObjects[j] as EnemyScript;
                                if (fellowEnemy.ACTIONS > 0)
                                {
                                    ItemContainer container = ScriptableObject.CreateInstance<ItemContainer>();
                                    container.item = INVENTORY.ITEMS[i];
                                    container.target = this;
                                    possibleItems.Add(container);
                                    found = true;
                                }
                            }
                        }
                    }
                    break;
                case ItemType.random:
                    {
                        if (ACTIONS > 0)
                        {
                            ItemContainer container = ScriptableObject.CreateInstance<ItemContainer>();
                            container.item = INVENTORY.ITEMS[i];
                            container.target = this;
                            possibleItems.Add(container);
                            found = true;
                        }
                        for (int j = 0; j < adjacentObjects.Count; j++)
                        {
                            if (adjacentObjects[j].FACTION == this.FACTION && adjacentObjects[j].GetComponent<EnemyScript>())
                            {
                                EnemyScript fellowEnemy = adjacentObjects[j] as EnemyScript;
                                if (fellowEnemy.ACTIONS > 0)
                                {
                                    ItemContainer container = ScriptableObject.CreateInstance<ItemContainer>();
                                    container.item = INVENTORY.ITEMS[i];
                                    container.target = this;
                                    possibleItems.Add(container);
                                    found = true;
                                }
                            }
                        }
                    }
                    break;

            }
        }

        return found;
    }
    public TileScript DetermineMoveLocation(TileScript targetTile)
    {
        //Debug.Log(FullName + " " + calcLocation.ToString());
        TileScript newTile = null;
        List<TileScript> myTiles = myManager.GetMoveAbleTiles(transform.position, MOVE_DIST, this);
        for (int i = 0; i < myTiles.Count; i++)
        {
            if (myTiles[i] == targetTile)
            {
                return targetTile;
            }
            if (newTile == null)
            {
                newTile = myTiles[i];
            }
            else
            {
                float dist1 = Vector3.Distance(newTile.transform.position, targetTile.transform.position);
                float dist2 = Vector3.Distance(myTiles[i].transform.position, targetTile.transform.position);

                if (dist2 < dist1)
                {
                    newTile = myTiles[i];
                    if (newTile.isOccupied)
                    {
                        Debug.Log("YOUR A FUCKING IDIOT!");

                    }
                }
            }
        }

        return newTile;
    }
    public TileScript FindNearestDoorTile()
    {
        TileScript returnTile = null;

        List<TileScript> tiles = myManager.tileMap;
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].TTYPE == TileType.door)
            {
                if (returnTile == null)
                {
                    returnTile = tiles[i];
                }
                else
                {
                    float dist1 = Vector3.Distance(returnTile.transform.position, transform.position);
                    float dist2 = Vector3.Distance(tiles[i].transform.position, transform.position);
                    if (dist2 < dist1)
                    {
                        returnTile = tiles[i];
                    }
                }
            }
        }

        return returnTile;
    }
    public LivingObject FindNearestEnemy()
    {
        // Debug.Log(FullName + " is finding near enemies");
        LivingObject newTarget = null;
        LivingObject[] objects = GameObject.FindObjectsOfType<LivingObject>();
        for (int i = 0; i < objects.Length; i++)
        {
            //if(objects[i].GetComponent<LivingObject>())
            {
                LivingObject living = objects[i];//.GetComponent<LivingObject>();
                if (living.FACTION != this.FACTION)
                {
                    if (!living.DEAD)
                    {

                        if (newTarget == null)
                        {
                            newTarget = living;
                        }
                        else
                        {
                            float dist1 = Vector3.Distance(newTarget.transform.position, transform.position);// Mathf.Sqrt(Mathf.Abs(newTarget.transform.position.sqrMagnitude - transform.position.sqrMagnitude));
                            float dist2 = Vector3.Distance(living.transform.position, transform.position); //Mathf.Sqrt(Mathf.Abs(living.transform.position.sqrMagnitude - transform.position.sqrMagnitude));
                            if (dist2 < dist1)
                            {
                                newTarget = living;
                            }
                        }
                    }
                }
            }
        }
        return newTarget;
    }
    public DmgReaction DetermineBestDmgOutput()
    {
        //     Debug.Log("determing atk from " + possibleAttacks.Count);
        DmgReaction bestReaction;
        List<int> attackOptions = new List<int>();
        for (int i = 0; i < possibleAttacks.Count; i++)
        {

            attackOptions.Add(i);
            if (possibleAttacks[i].strike != null)
            {
                WEAPON.Equip(possibleAttacks[i].strike);
                if(possibleAttacks[i].strike.CanUse() == false)
                {
                    Debug.Log("problemo");
                    continue;
                }
            }
            else
            {
                if (possibleAttacks[i].command.CanUse() == false)
                {
                    Debug.Log("problemasd");
                    continue;
                }
            }
            DmgReaction aReaction = myManager.CalcDamage(possibleAttacks[i], false);
            if (aReaction.reaction <= Reaction.weak && aReaction.reaction >= Reaction.leathal)
            {
                attackOptions.Add(i);
            }
            if (possibleAttacks[i].command != null)
            {
                CommandSkill usableskill = possibleAttacks[i].command;
                if (usableskill.HITS > 1 || usableskill.MAX_HIT > 1)
                {
                    attackOptions.Add(i);
                }
                if (usableskill.ETYPE == EType.magical)
                {
                    if (MAGIC > STRENGTH)
                    {
                        attackOptions.Add(i);
                    }
                    if (MAGIC > possibleAttacks[i].dmgObject.BASE_STATS.RESIESTANCE)
                    {
                        attackOptions.Add(i);
                    }

                }
                else
                {
                    if (STRENGTH > MAGIC)
                    {
                        attackOptions.Add(i);
                    }
                    if (STRENGTH > possibleAttacks[i].dmgObject.BASE_STATS.DEFENSE)
                    {
                        attackOptions.Add(i);
                    }
                }
            }

        }
        //   Debug.Log("concluded options " + attackOptions.Count);
        if (attackOptions.Count > 0)
        {

            chosen = Random.Range(0, attackOptions.Count);
            AtkContainer chosenContainer = possibleAttacks[attackOptions[chosen]];
            bestReaction = myManager.CalcDamage(chosenContainer);
            chosen = attackOptions[chosen];
            atkTarget = chosenContainer.dmgObject;
            if (chosenContainer.command)
            {
                bestReaction.atkName = chosenContainer.command.NAME;
                bestReaction.usedSkill = chosenContainer.command;
                
                chosenContainer.command.UseSkill(this);
                bestReaction.dmgElement = chosenContainer.command.ELEMENT;

            }
            else
            {
                WEAPON.Equip(chosenContainer.strike);
                if (WEAPON.EQUIPPED)
                {
                    WEAPON.Use();
                }
                bestReaction.usedSkill = null;
                bestReaction.atkName = WEAPON.NAME;
                bestReaction.dmgElement = WEAPON.ELEMENT;
                bestReaction.usedStrike = chosenContainer.strike;
            }
            lastAttack = chosenContainer;
        }
        else
        {
            Debug.Log("basic atk");
            bestReaction = myManager.CalcDamage(this, atkTarget, WEAPON, Reaction.none, false);
            lastAttack = null;
        }

        return bestReaction;
    }
    private void LoadAdjacentObjects()
    {
        TileScript origin = currentTile;
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 1;

        v2.x += 1;

        v3.z -= 1;

        v4.x -= 1;
        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                GridObject griddy = null;

                if (griddy = myManager.GetObjectAtTile(myManager.tileMap[index]))
                {
                    adjacentObjects.Add(griddy);
                }
            }
        }
    }
    private void PrepareMoveEvent(TileScript targetTile)
    {

        if (targetTile)
        {
            if (myManager.eventManager)
            {
                //calcLocation = targetTile.transform.position;
                //calcLocation.y = 0.5f;

                path p = ScriptableObject.CreateInstance<path>();
                p.realTarget = targetTile;
                p.currentPath = new Queue<TileScript>();
                myManager.CreateEvent(this, p, "" + FullName + "move event ", MoveEvent, null, 0, MoveStart);

            }
            else
            {
                Debug.Log("Could not find event manager");
            }
        }
        else
        {
            Debug.Log("No target tile, waiting...");
            waiting = true;
            myManager.CreateEvent(this, null, "" + FullName + "wait event ", EWaitEvent);
        }
    }
    public bool DetermineNextAction(Object target)
    {
        if (DEAD)
        {
            return true;
        }
        //clear up lists
        {

            adjacentObjects.Clear();
            possibleItems.Clear();
            possibleAttacks.Clear();
            atkNames.Clear();
        }
        LivingObject liveObj = null;
        LoadAdjacentObjects();
        liveObj = FindNearestEnemy();
        //if enemies don't exist
        if (waiting)
        {
            //TakeRealAction();
            return true;
        }
        if (myManager)
        {
            myManager.requestTurnImgUpdate(this);
        }
        if (liveObj)
        {
            if (personality == EPType.scared)
            {
                TileScript targetTile = FindNearestDoorTile();
                if (currentTile != targetTile)
                {
                    targetTile = DetermineMoveLocation(targetTile);
                    PrepareMoveEvent(targetTile);
                }
                else
                {
                    myManager.CreateEvent(this, null, "" + FullName + "run away event ", RunAwayEvent, null, 0);
                }
                return true;
            }
            bool found = FindEnemiesInRangeOfAttacks(transform.position);
            bool foundItems = FoundItemsCanUse();
            if (found == false)
            {

                if (reflectedSkills.Count >= ((float)(INVENTORY.WEAPONS.Count + INVENTORY.CSKILLS.Count) + 1.0f) * 0.5f && personality != EPType.scared)
                {
                    personality = EPType.scared;
                }
                if (personality == EPType.scared)
                {
                    TileScript targetTile = FindNearestDoorTile();
                    if (currentTile != targetTile)
                    {
                        targetTile = DetermineMoveLocation(targetTile);
                        PrepareMoveEvent(targetTile);
                    }
                    else
                    {
                        myManager.CreateEvent(this, null, "" + FullName + "run away event ", RunAwayEvent, null, 0);
                    }
                }
                else
                {


                    //move

                    switch (Common.GetEPCluster(personality))
                    {
                        case EPCluster.physical:
                            {
                                if (FATIGUE >= MAX_FATIGUE - 4)
                                {
                                    waiting = true;
                                    myManager.CreateEvent(this, liveObj, "" + FullName + "wait event ", EWaitEvent);
                                }
                                else
                                {
                                    currentEnemy = liveObj;
                                    TileScript targetTile = DetermineMoveLocation(liveObj.currentTile);
                                    PrepareMoveEvent(targetTile);
                                }
                            }
                            break;
                        case EPCluster.magical:
                            {
                                if (MANA <= 5)
                                {
                                    waiting = true;
                                    myManager.CreateEvent(this, liveObj, "" + FullName + "wait event ", EWaitEvent);
                                }
                                else
                                {


                                    currentEnemy = liveObj;
                                    TileScript targetTile = DetermineMoveLocation(liveObj.currentTile);
                                    PrepareMoveEvent(targetTile);
                                }
                            }
                            break;
                        case EPCluster.logical:
                            {


                                currentEnemy = liveObj;
                                TileScript targetTile = DetermineMoveLocation(liveObj.currentTile);
                                PrepareMoveEvent(targetTile);

                            }
                            break;
                        case EPCluster.natural:
                            {

                                currentEnemy = liveObj;
                                TileScript targetTile = DetermineMoveLocation(liveObj.currentTile);
                                PrepareMoveEvent(targetTile);
                            }
                            break;
                    }


                }
            }
            else
            {
                switch (personality)
                {
                    case EPType.aggro:
                        {
                            myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event ", EAtkEvent, AttackStart, 0);
                        }
                        break;

                    case EPType.tactical:
                        {
                            if (HEALTH > ((20.0f / 100.0f) * MAX_HEALTH))
                            {
                                myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event ", EAtkEvent, AttackStart, 0);
                            }
                            else if (foundItems == true)
                            {
                                myManager.CreateEvent(this, liveObj, "" + FullName + "use item event", UseItemEvent, ItemStart, 0);
                            }
                            else
                            {
                                waiting = true;
                                myManager.CreateEvent(this, liveObj, "" + FullName + "wait event ", EWaitEvent);
                            }
                        }
                        break;
                    case EPType.mystical:
                        if (MANA >= ((20.0f / 100.0f) * MAX_MANA))
                        {
                            myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event ", EAtkEvent, AttackStart, 0);
                        }
                        else if (foundItems == true)
                        {
                            myManager.CreateEvent(this, liveObj, "" + FullName + "use item event", UseItemEvent, ItemStart, 0);
                        }
                        else
                        {
                            waiting = true;
                            myManager.CreateEvent(this, liveObj, "" + FullName + "wait event ", EWaitEvent);
                        }
                        break;
                    case EPType.forceful:

                        if (HEALTH > ((20.0f / 100.0f) * MAX_HEALTH))
                        {
                            myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event ", EAtkEvent, AttackStart, 0);
                        }
                        else if (foundItems == true)
                        {
                            myManager.CreateEvent(this, liveObj, "" + FullName + "use item event", UseItemEvent, ItemStart, 0);
                        }
                        else
                        {
                            waiting = true;
                            myManager.CreateEvent(this, liveObj, "" + FullName + "wait event ", EWaitEvent);
                        }
                        break;
                    case EPType.itemist:
                        if (foundItems == true)
                        {
                            myManager.CreateEvent(this, liveObj, "" + FullName + "use item event", UseItemEvent, ItemStart, 0);
                        }
                        else
                        {
                            myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event ", EAtkEvent, AttackStart, 0);
                        }
                        break;
                    case EPType.optimal:
                        {
                            if (adjacentObjects.Count > 0)
                            {
                                myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event ", EAtkEvent, AttackStart, 0);
                            }
                            else if (myManager.liveEnemies.Count > 1)
                            {
                                for (int i = 0; i < myManager.liveEnemies.Count; i++)
                                {
                                    if (myManager.liveEnemies[i] != this)
                                    {
                                        TileScript targetAllyTile = myManager.liveEnemies[i].currentTile;
                                        PrepareMoveEvent(targetAllyTile);
                                        break;
                                    }
                                }
                            }
                            else if (foundItems == true)
                            {
                                myManager.CreateEvent(this, liveObj, "" + FullName + "use item event", UseItemEvent, ItemStart, 0);
                            }
                            else
                            {

                                personality = EPType.scared;

                                if (personality == EPType.scared)
                                {
                                    TileScript targetTile = FindNearestDoorTile();
                                    if (currentTile != targetTile)
                                    {
                                        targetTile = DetermineMoveLocation(targetTile);
                                        PrepareMoveEvent(targetTile);
                                    }
                                    else
                                    {
                                        myManager.CreateEvent(this, null, "" + FullName + "run away event ", RunAwayEvent, null, 0);
                                    }
                                }
                            }
                        }
                        break;
                    case EPType.finisher:
                        {
                            if (foundItems == true)
                            {
                                for (int i = 0; i < possibleItems.Count; i++)
                                {
                                    if (possibleItems[i].item.ITYPE == ItemType.dmg)
                                    {
                                        myManager.CreateEvent(this, liveObj, "" + FullName + "use item event", UseItemEvent, ItemStart, 0);
                                    }
                                }
                            }
                            else
                            {
                                myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event ", EAtkEvent, AttackStart, 0);
                            }

                        }
                        break;
                    case EPType.biologist:
                        if (MANA >= ((20.0f / 100.0f) * MAX_MANA))
                        {
                            myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event ", EAtkEvent, AttackStart, 0);
                        }
                        else if (foundItems == true)
                        {
                            myManager.CreateEvent(this, liveObj, "" + FullName + "use item event", UseItemEvent, ItemStart, 0);
                        }
                        else
                        {
                            waiting = true;
                            myManager.CreateEvent(this, liveObj, "" + FullName + "wait event ", EWaitEvent);
                        }
                        break;
                    case EPType.support:
                        if (MANA >= ((20.0f / 100.0f) * MAX_MANA))
                        {
                            myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event ", EAtkEvent, AttackStart, 0);
                        }
                        else if (foundItems == true)
                        {
                            myManager.CreateEvent(this, liveObj, "" + FullName + "use item event", UseItemEvent, ItemStart, 0);
                        }
                        else
                        {
                            waiting = true;
                            myManager.CreateEvent(this, liveObj, "" + FullName + "wait event ", EWaitEvent);
                        }
                        break;


                    default:
                        {
                            myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event ", EAtkEvent, AttackStart, 0);
                        }
                        break;

                }

            }
        }
        else
        {
            waiting = true;
            myManager.CreateEvent(this, liveObj, "" + FullName + "wait event ", EWaitEvent);
        }
        return true;
    }
    public void DetermineActions()
    {
        if (DEAD)
        {
            return;
        }
        isPerforming = true;
        //  Debug.Log(FullName + " is Determining actions");

        waiting = false;
        psudeoActions = ACTIONS;

        if (psudeoActions == 0)
        {
            //   TakeAction();
        }
        //  List<EActType> etypes = new List<EActType>();
        //  List<path> possiblePaths = new List<path>();
        //  LivingObject liveObj = null;
        for (int i = 0; i < psudeoActions; i++)
        {
            myManager.CreateEvent(this, null, "" + FullName + "determine action event " + i, DetermineNextAction, null, 0);
            //if (HEALTH > HEALTH * 0.5)
            // {
            // Debug.Log(FullName + " performing action " + i);

            //    liveObj = FindNearestEnemy();
            //    if (liveObj)
            //    {
            //        bool amount = FindEnemiesInRangeOfAttacks(calcLocation);
            //        if (amount == false)
            //        {
            //            TileScript targetTile = new TileScript();
            //            currentEnemy = liveObj;
            //            targetTile = DetermineMoveLocation(liveObj.currentTile);

            //            if (targetTile)
            //            {
            //                if (myManager.eventManager)
            //                {
            //                    calcLocation = targetTile.transform.position;
            //                    calcLocation.y = 0.5f;

            //                    path p = ScriptableObject.CreateInstance<path>();
            //                    p.realTarget = targetTile;
            //                    p.currentPath = new Queue<TileScript>();
            //                    //myManager.CreateEvent(this, this, "Enemy Camera Event", myManager.CameraEvent);
            //                    //myManager.CreateEvent(this, p, "" + FullName + "move event" + i, MoveEvent,null,0);
            //                    etypes.Add(EActType.move);
            //                    possiblePaths.Add(p);
            //                }
            //                else
            //                {
            //                    Debug.Log("Could not find event manager");
            //                }
            //            }
            //            else
            //            {
            //                Debug.Log("No target tile, waiting...");
            //                Wait();
            //            }

            //        }
            //        else
            //        {
            //            //        Debug.Log("Doing an attack event");
            //            //   myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event", EAtkEvent, null,0);
            //            etypes.Insert(0, EActType.atk);
            //            //   TakeAction();
            //        }
            //    }
            //    else
            //    {
            //        // TakeAction();

            //        isPerforming = false;
            //        Wait();
            //    }
            //}
        }
        //int pathNum = possiblePaths.Count - 1;
        //for (int i = 0; i < etypes.Count; i++)
        //{
        //    switch (etypes[i])
        //    {
        //        case EActType.move:
        //            //  myManager.CreateEvent(this, this, "Enemy Camera Event", myManager.CameraEvent);
        //            myManager.CreateEvent(this, possiblePaths[pathNum], "" + FullName + "move event " + i, MoveEvent, null, 0, MoveStart);
        //            pathNum--;
        //            break;

        //        case EActType.atk:
        //            // myManager.CreateEvent(this, this, "Enemy Camera Event", myManager.CameraEvent);
        //            myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event " + i, EAtkEvent, AttackStart, 0);
        //            break;



        //    }
        //}
        //   else

        // myManager.CreateEvent(this, this, "Select Camera Event", myManager.CameraEvent, null, 0);

        //  myManager.ShowGridObjectMoveArea(this);
        isPerforming = false;
    }
    public override void Die()
    {
        base.Die();
    }

    public override IEnumerator FadeOut()
    {
        //   Debug.Log("enemy fade start");
        startedDeathAnimation = true;


        Color subtract = new Color(0, 0, 0, 0.1f);
        int num = 0;
        while (mySR.color.a > 0)
        {
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
        startedDeathAnimation = true;
        myManager.gridObjects.Remove(this);
        gameObject.SetActive(false);
        //Debug.Log("enemy fade out end");
        // Destroy(gameObject);

    }
    public void CheckAttackRequirements(Element atk, LivingObject player)
    {
        if(player.GetComponent<ActorScript>())
        {
            if(talk == TalkStage.Attack)
            {
                if(atk == talkElement)
                {
                    enemyHitMyResist = true;
                    talk = TalkStage.learn;
                }
            }
        }
    }
    public string CheckTalkRequirements(LivingObject invokingObject)
    {
        string returnedString = "no change";

        switch (TALK)
        {
            case TalkStage.initial:
                {
                    switch (Common.GetEPCluster(personality))
                    {
                        case EPCluster.physical:
                            {
                                if(FACTION == Faction.enemy)
                                    returnedString = "Listen hybrid, talk to me again when i'm at half FATIGUE.";
                                else
                                {
                                    returnedString = "Hey Pleb! Talk to me again when i'm at half FATIGUE.";
                                }
                            }
                            break;
                        case EPCluster.magical:
                            {

                                if (FACTION == Faction.enemy)
                                    returnedString = "Magic is everything young one. Talke to me when i'm at half MANA";
                                else
                                {
                                    returnedString = "Horrid! Unpure creature. No! Talk to me at again when I have half MANA.";
                                }

                            }
                            break;
                        case EPCluster.logical:
                            {


                                if (FACTION == Faction.enemy)
                                    returnedString = "You think your Tough? Talke to me when i'm at half HEALTH";
                                else
                                {
                                    returnedString = "Wipe that smirk off your face simpleton! Talk to me at again when I have half HEALTH.";
                                }
                            }
                            break;
                        case EPCluster.natural:
                            {
                               
                                        returnedString = "I'm going to leave for the DOOR now, bye.";
                         

                            }
                            break;
                    }
                    talk = TalkStage.Stats;
                }
                break;

            case TalkStage.Stats:
                {
                    switch (Common.GetEPCluster(personality))
                    {
                        case EPCluster.physical:
                            {
                                if (FATIGUE < ((float)MAX_FATIGUE * 0.5f))
                                    returnedString = "You think im in a mood to talk when I have less than half FATIGUE?";
                                else
                                {
                                    returnedString = "Ok I'm listening. But how do I know your not trying to trick me?";
                                    TALK = TalkStage.Attack;

                                }
                            }
                            break;
                        case EPCluster.magical:
                            {
                                if (MANA > ((float)MAX_MANA * 0.5f))
                                    returnedString = "I still have enough MANA to destroy you. Your words are irrelevent.";
                                else
                                {
                                    returnedString = "Ok I'm listening. But how do I know your not trying to trick me?";
                                    TALK = TalkStage.Attack;

                                }
                            }
                            break;
                        case EPCluster.logical:
                            {
                                if (HEALTH < ((float)MAX_HEALTH * 0.5f))
                                    returnedString = "Why would I talk to you when I have a good amount of HEALTH left?";
                                else
                                {
                                    returnedString = "Ok I'm listening. But how do I know your not trying to trick me?";
                                    TALK = TalkStage.Attack;

                                }
                            }
                            break;
                        case EPCluster.natural:
                            {
                                TileScript tile = FindNearestDoorTile();
                                if (tile)
                                {
                                    if (tile.isOccupied)
                                    {
                                        returnedString = "Ok I'm listening. But u gotta do what I say ok";
                                        TALK = TalkStage.Attack;
                                    }
                                    else
                                    {
                                        returnedString = "Don't hurt me, please don't BLOCK the door.";
                                    }
                                }
                                else
                                {
                                    returnedString = "Don't hurt me please.";

                                }

                            }
                            break;
                    }
                }
                break;
            case TalkStage.Attack:
                {
                    if (enemyHitMyResist == false)
                    {

                        Element bestElement = Element.Water;
                        EHitType bestType = EHitType.normal;
                        for (int i = 0; i < ARMOR.HITLIST.Count; i++)
                        {
                            EHitType testType = ARMOR.HITLIST[i];
                            if (testType < bestType)
                            {
                                bestType = testType;
                                bestElement = (Element)i;
                            }
                        }
                        talkElement = bestElement;
                        returnedString = "If you want to talk, hit me with a " + bestElement.ToString() + "attack";
                    }
                    else
                    {
                        returnedString = "Wow, you listened to me. I didn't expect that.";
                    }
                }
                break;
            case TalkStage.learn:
                {
                    returnedString = "Alright, I'll help you out. Take this summon scroll";
                    DatabaseManager database = Common.GetDatabase();
                    if(database)
                    {
                    database.GenerateScroll(this, invokingObject);

                    }
                    STATS.HEALTH = 0;
                    BASE_STATS.HEALTH = 0;
                    Die();
                }
                break;
            case TalkStage.rejected:
                {
                    returnedString = "I'm done talking to you!";
                }
                break;
            default:
                {

                }
                break;
        }



        return returnedString;
    }
    public void Unset()
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
        ARMOR.unEquip();
        DEFAULT_ARMOR = null;
        PSTATUS = PrimaryStatus.normal;
        reflectedSkills.Clear();
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
