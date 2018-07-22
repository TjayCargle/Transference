using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : LivingObject
{

    public List<GridObject> potentialTargets;
    public Queue<TileScript> currentPath;

    public LivingObject currentEnemy;
    public int headCount = 0;
    public int psudeoActions = 0;
    public Vector3 calcLocation;
    public bool MoveEvent(Object target)
    {
        bool isDone = false;
        TileScript realTarget = target as TileScript;
   
        headCount = currentPath.Count;
        DeterminePath(realTarget);
        TileScript nextTile = currentPath.Peek();
        Vector3 directionVector = (nextTile.transform.position - transform.position);
        directionVector.y = 0.0f;
        float dist = Vector3.Distance(nextTile.transform.position, currentTile.transform.position);
        if (dist > 0.5f)
        {
            transform.Translate(directionVector * 0.5f);
        }
        else
        {
            transform.Translate(directionVector * 0.2f);
        }
        if (transform.position.x == nextTile.transform.position.x)
        {
            if (transform.position.z == nextTile.transform.position.z)
            {
                currentTile = currentPath.Dequeue();
            }
        }
        if (currentPath.Count == 0)
        {
            isDone = true;
            TakeAction();
            Debug.Log("Event Done!");
        }
        return isDone;
    }
    public override void Setup()
    {
        if (!isSetup)
        {
            base.Setup();
            potentialTargets = new List<GridObject>();
            currentPath = new Queue<TileScript>();
            isSetup = true;
        }
    }
    public bool BasicAtkEvent(Object target)
    {
        bool isDone = false;
        TakeAction();
        return isDone;
    }

    public bool UseSkillEvent(Object skillAndTarget)
    {
        bool isDone = false;
        TakeAction();
        return isDone;
    }
    public void DeterminePath(TileScript target)
    {
        float timer = 5.0f;
        if (currentPath.Count == 0)
        {
            //  targ = target;
            Debug.Log(FullName + " is Determining path");
            bool complete = false;
            TileScript current = currentTile;
            while (complete == false)
            {
                List<TileScript> options = myManager.GetAdjecentTiles(current);
                for (int i = 0; i < options.Count; i++)
                {
                    if (Vector3.Distance(target.transform.position, options[i].transform.position) < Vector3.Distance(target.transform.position, current.transform.position))
                    {
                        current = options[i];
                    }
                }
                currentPath.Enqueue(current);
                if (current == target)
                {
                    complete = true;
                    break;
                }
                if (timer - Time.deltaTime < 0)
                {
                    Debug.Log("Time took longer than 5 seconds");
                    break;
                }
            }
        }
    }
    public int DetermineEnemiesInRange()
    {
        int count = 0;

        return count;
    }
    public TileScript DetermineMoveLocation(TileScript targetTile)
    {
        Debug.Log(FullName + " is Determining move location");
        TileScript newTile = null;
        List<TileScript> myTiles = myManager.GetMoveAbleTiles(calcLocation, MOVE_DIST);
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
                }
            }
        }
        return newTile;
    }
    public LivingObject FindNearestEnemy()
    {
        Debug.Log(FullName + " is finding near enemies");
        LivingObject newTarget = null;
        LivingObject[] objects = GameObject.FindObjectsOfType<LivingObject>();
        for (int i = 0; i < objects.Length; i++)
        {
            //if(objects[i].GetComponent<LivingObject>())
            {
                LivingObject living = objects[i];//.GetComponent<LivingObject>();
                if (!living.IsEnenmy)
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
        return newTarget;
    }
    public void DetermineActions()
    {

        Debug.Log(FullName + " is Determining actions");
        psudeoActions = ACTIONS;
        calcLocation = transform.position;
        for (int i = 0; i < psudeoActions; i++)
        {
            int amount = DetermineEnemiesInRange();
            if (amount == 0)
            {
                if (HEALTH > HEALTH * 0.5)
                {

                    LivingObject liveObj = FindNearestEnemy();
                    if (liveObj)
                    {
                        currentEnemy = liveObj;
                        TileScript targetTile = DetermineMoveLocation(liveObj.currentTile);

                        if (targetTile)
                        {
                            if (myManager.eventManager)
                            {
                                calcLocation = targetTile.transform.position;
                                calcLocation.y = 0.5f;
                                Debug.Log("EVENT: go towards " + calcLocation);
                                GridEvent EnemyEvent = new GridEvent();
                                EnemyEvent.caller = this;
                                EnemyEvent.data = targetTile;
                                EnemyEvent.RUNABLE = MoveEvent;
                                EnemyEvent.name = FullName + " move event";
                                myManager.eventManager.gridEvents.Add(EnemyEvent);
                            }
                            else
                            {
                                Debug.Log("Could not find event manager");
                            }
                        }

                    }
                    else
                    {

                    }
                }
            }
            else
            {

            }

        }
    }
}
