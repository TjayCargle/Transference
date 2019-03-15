using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : LivingObject
{

    public List<GridObject> potentialTargets;
    //public Queue<TileScript> currentPath;
    public bool isPerforming = false;
    public LivingObject currentEnemy;
    public int headCount = 0;
    public int psudeoActions = 0;
    public Vector3 calcLocation;
    TileScript targetTile;
    ManagerScript manager;
    public TileScript nextTile;

    public void MoveStart()
    {
        //  Debug.Log(FullName+" move start");
  
        myManager.MoveCameraAndShow(this);


    }
    public bool MoveEvent(Object target)
    {

        bool isDone = false;
        path pathTarget = target as path;

        TileScript realTarget = pathTarget.realTarget;

        pathTarget.currentPath = DeterminePath(pathTarget);
        headCount = pathTarget.currentPath.Count;
        if(headCount == 0)
        {
            isDone = true;
            TakeAction();
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
            transform.Translate(directionVector * 0.5f);
        }
        else
        {
            directionVector = nextTile.transform.position;
            directionVector.y = 0.5f;
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
            TakeAction();
            if (ACTIONS <= 0)
            {
                isPerforming = false;
            }
            //Debug.Log("Move Event Done!");
        }

        myManager.myCamera.currentTile = currentTile;
        return isDone;
    }
    public override void Setup()
    {
        if (!isSetup)
        {
            base.Setup();
            potentialTargets = new List<GridObject>();
            manager = GameObject.FindObjectOfType<ManagerScript>();
            //currentPath = new Queue<TileScript>();
            isSetup = true;
        }
    }



    public bool EAtkEvent(Object target)
    {
       // Debug.Log(FullName + " atacking");
        bool isDone = true;
        //if(ACTIONS > 0)
        {
            myManager.MoveCameraAndShow(this);
            LivingObject realTarget = target as LivingObject;
            if (SSTATUS == SecondaryStatus.confusion)
            {
                int chance = Random.Range(0, 2);

                if (chance <= 2)
                {
                    Debug.Log("They hit themselves");

                    realTarget = this;
                }
            }

            DmgReaction bestReaction = DetermineBestDmgOutput(realTarget);
            manager.CreateTextEvent(this, "" + FullName + " used " + bestReaction.atkName, "enemy atk", manager.CheckText, manager.TextStart);
            myManager.ApplyReaction(this, realTarget, bestReaction, bestReaction.dmgElement);
            myManager.myCamera.infoObject = realTarget;
            myManager.myCamera.UpdateCamera();
            if (bestReaction.reaction != Reaction.missed)
            {
                AtkConatiner conatiner = ScriptableObject.CreateInstance<AtkConatiner>();
                conatiner.attackingObject = this;
                conatiner.dmgObject = realTarget;
                conatiner.attackingElement = bestReaction.dmgElement;
                myManager.CreateEvent(this, conatiner, "" + FullName + "enemy opp event", myManager.ECheckForOppChanceEvent, null, 1);

            }
            // Debug.Log(FullName + " used " + bestReaction.atkName);

            TakeAction();

        }
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

                            if (manager.GetObjectAtTile(checkTile).GetComponent<LivingObject>())
                            {
                                if (manager.GetObjectAtTile(checkTile).GetComponent<LivingObject>().FACTION == Faction.enemy)
                                {
                                    LivingObject living = manager.GetObjectAtTile(checkTile).GetComponent<LivingObject>();
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
    public int DetermineEnemiesInRange(Vector3 location)
    {
        int count = 0;
        LivingObject[] objects = GameObject.FindObjectsOfType<LivingObject>();
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].FACTION != Faction.enemy)
            {
                if (Vector3.Distance(location, objects[i].transform.position) <= Atk_DIST)
                {
                    count++;
                }
            }
        }
        return count;
    }
    public TileScript DetermineMoveLocation(TileScript targetTile)
    {
        // Debug.Log(FullName + " is Determining move location");
        TileScript newTile = null;
        List<TileScript> myTiles = myManager.GetMoveAbleTiles(calcLocation, MOVE_DIST, this);
        for (int i = 0; i < myTiles.Count; i++)
        {

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
                if (living.FACTION != Faction.enemy)
                {
                    if (!living.DEAD)
                    {

                        if (newTarget == null)
                        {
                            newTarget = living;
                        }
                        else
                        {
                            float dist1 = Vector3.Distance(newTarget.transform.position, calcLocation);// Mathf.Sqrt(Mathf.Abs(newTarget.transform.position.sqrMagnitude - transform.position.sqrMagnitude));
                            float dist2 = Vector3.Distance(living.transform.position, calcLocation); //Mathf.Sqrt(Mathf.Abs(living.transform.position.sqrMagnitude - transform.position.sqrMagnitude));
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
    public DmgReaction DetermineBestDmgOutput(LivingObject target, bool checkdistance = true)
    {
        DmgReaction bestReaction = myManager.CalcDamage(this, target, WEAPON);
        bestReaction.atkName = WEAPON.NAME;
        bestReaction.dmgElement = WEAPON.AFINITY;
        CommandSkill usedSkill = null;
        EHitType currHit = target.ARMOR.HITLIST[(int)WEAPON.AFINITY];
        float modification = 1.0f;
        for (int j = 0; j < BATTLE_SLOTS.SKILLS.Count; j++)
        {
            CommandSkill skill = BATTLE_SLOTS.SKILLS[j] as CommandSkill;
            if (skill.ELEMENT != Element.Buff)
            {
                if (skill.ETYPE == EType.magical)
                    modification = STATS.SPCHANGE;
                if (skill.ETYPE == EType.physical)
                {
                    if (skill.COST > 0)
                    {
                        modification = STATS.FTCHARGECHANGE;
                    }
                    else
                    {
                        modification = STATS.FTCOSTCHANGE;
                    }
                }
                if (skill.CanUse(modification))
                {
                    bool inrange = false;
                    if (checkdistance)
                    {
                        inrange = (Vector3.Distance(target.transform.position, calcLocation) <= skill.TILES.Count);
                    }
                    else
                    {
                        inrange = true;
                    }
                    if (inrange)// Mathf.Sqrt(Mathf.Abs(newTarget.transform.position.sqrMagnitude - transform.position.sqrMagnitude));
                    {

                        DmgReaction aReaction = myManager.CalcDamage(this, target, skill, skill.REACTION);
                        aReaction.dmgElement = skill.ELEMENT;

                        if (aReaction.damage > bestReaction.damage)
                        {
                            bestReaction = aReaction;
                            bestReaction.atkName = skill.NAME;
                            usedSkill = skill;

                            if (usedSkill.ETYPE == EType.magical)
                                modification = STATS.SPCHANGE;
                            if (usedSkill.ETYPE == EType.physical)
                            {
                                if (usedSkill.COST > 0)
                                {
                                    modification = STATS.FTCHARGECHANGE;
                                }
                                else
                                {
                                    modification = STATS.FTCOSTCHANGE;
                                }
                            }
                        }
                        if ((int)target.ARMOR.HITLIST[(int)skill.ELEMENT] > (int)currHit)
                        {
                            bestReaction = aReaction;
                            bestReaction.atkName = skill.NAME;
                        }
                    }
                }
            }
        }
        if (usedSkill)
        {
            usedSkill.UseSkill(this, modification);
        }
        return bestReaction;
    }
    public void DetermineActions()
    {
        isPerforming = true;
        //  Debug.Log(FullName + " is Determining actions");

        psudeoActions = ACTIONS;
        calcLocation = transform.position;
        if (psudeoActions == 0)
        {
            //   TakeAction();
        }
        List<EActType> etypes = new List<EActType>();
        path p = null;
        LivingObject liveObj = null;
        for (int i = 0; i < psudeoActions; i++)
        {
            //if (HEALTH > HEALTH * 0.5)
            {
                // Debug.Log(FullName + " performing action " + i);

                liveObj = FindNearestEnemy();
                if (liveObj)
                {
                    int amount = DetermineEnemiesInRange(calcLocation);
                    if (amount == 0)
                    {

                        currentEnemy = liveObj;
                        targetTile = DetermineMoveLocation(liveObj.currentTile);

                        if (targetTile)
                        {
                            if (myManager.eventManager)
                            {
                                calcLocation = targetTile.transform.position;
                                calcLocation.y = 0.5f;

                                p = ScriptableObject.CreateInstance<path>();
                                p.realTarget = targetTile;
                                p.currentPath = new Queue<TileScript>();
                                //myManager.CreateEvent(this, this, "Enemy Camera Event", myManager.CameraEvent);
                                //myManager.CreateEvent(this, p, "" + FullName + "move event" + i, MoveEvent,null,0);
                                etypes.Add(EActType.move);
                            }
                            else
                            {
                                Debug.Log("Could not find event manager");
                            }
                        }
                        else
                        {
                            Debug.Log("No target tile, waiting...");
                            Wait();
                        }

                    }
                    else
                    {
                        //        Debug.Log("Doing an attack event");
                        //   myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event", EAtkEvent, null,0);
                        etypes.Add(EActType.atk);
                        //   TakeAction();
                    }
                }
                else
                {
                    // TakeAction();

                    isPerforming = false;

                }
            }
        }

        for (int i = 0; i < etypes.Count; i++)
        {
            switch (etypes[i])
            {
                case EActType.move:
                    //  myManager.CreateEvent(this, this, "Enemy Camera Event", myManager.CameraEvent);
                    myManager.CreateEvent(this, p, "" + FullName + "move event" + i, MoveEvent, MoveStart);
                    break;

                case EActType.atk:
                    // myManager.CreateEvent(this, this, "Enemy Camera Event", myManager.CameraEvent);
                    myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event", EAtkEvent);
                    break;
            }
        }
        //   else


        myManager.ShowGridObjectMoveArea(this);
        isPerforming = false;
    }
    public override void Die()
    {
        base.Die();
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

            myManager.gridObjects.Remove(this);
            gameObject.SetActive(false);

            // Destroy(gameObject);
        }
    }

    public void Unset()
    {
        isSetup = false;
        DEAD = false;
        STATS.Reset(true);
        BASE_STATS.Reset();
        BASE_STATS.HEALTH = BASE_STATS.MAX_HEALTH;

    }
}
