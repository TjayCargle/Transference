using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardScript : LivingObject
{

    BaseStats myStats;
    private bool destructible = true;
    public int droppedItemNum = -1;
    [SerializeField]
    public bool dropsSkill = false;

    public int REWARD
    {
        get { return droppedItemNum; }
        set { droppedItemNum = value; }
    }

    public bool DESTRUCTIBLE
    {
        get { return destructible; }
        set { destructible = value; }
    }
    public bool isPerforming = false;

    public override void Setup()
    {
        if (!isSetup)
        {

            base.Setup();
            BASE_STATS.MANA = 0;
            BASE_STATS.MAX_MANA = 0;
            FACTION = Faction.hazard;
            FullName = "Glyph";
            isSetup = true;
        }

    }
    public override void Die()
    {
        base.Die();

    }

    public LivingObject FindNearestEnemy()
    {
        LivingObject newTarget = null;
        LivingObject[] objects = GameObject.FindObjectsOfType<LivingObject>();
        List<List<TileScript>> attackbleTiles = null;
        CommandSkill skill = null;
        if (INVENTORY.CSKILLS.Count > 0)
        {
            skill = INVENTORY.CSKILLS[0];
            attackbleTiles = myManager.GetSkillsAttackableTiles(this, skill);
        }
        else
        {
            attackbleTiles = myManager.GetWeaponAttackableTiles(this);
        }
        if (attackbleTiles == null)
            return null;
        Vector3 testLoc = Vector3.zero;
        if (attackbleTiles.Count > 0)
        {
            for (int liveIndex = 0; liveIndex < objects.Length; liveIndex++)
            {
                LivingObject living = objects[liveIndex];
                if (living.FACTION != Faction.hazard)
                {
                    if (!living.DEAD)
                    {

                        for (int i = 0; i < attackbleTiles.Count; i++)
                        {
                            for (int j = 0; j < attackbleTiles[i].Count; j++)
                            {
                                testLoc = attackbleTiles[i][j].transform.position;
                                TileScript aTile = myManager.GetTileAtIndex(myManager.GetTileIndex(testLoc));
                 
                                if (living.currentTile == aTile)
                                {
                                    if (newTarget == null)
                                    {
                                        newTarget = living;
                                        return newTarget;
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }



        return newTarget;
    }

    public bool HAtkEvent(Object target)
    {
        myManager.MoveCameraAndShow(this); 
        Debug.Log(FullName + " atacking");
        bool isDone = true;
        {

            LivingObject realTarget = target as LivingObject;

            DmgReaction bestReaction = DetermineBestDmgOutput(realTarget);
            myManager.CreateTextEvent(this, "" + FullName + " used " + bestReaction.atkName, "enemy atk", myManager.CheckText, myManager.TextStart);
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

    public DmgReaction DetermineBestDmgOutput(LivingObject target, bool checkdistance = true)
    {
      //  Debug.Log("determining best output");
        DmgReaction bestReaction;
        CommandSkill usedSkill = null;
        if (INVENTORY.CSKILLS.Count > 0)
        {
            usedSkill = INVENTORY.CSKILLS[0];
            bestReaction = myManager.CalcDamage(this, target, usedSkill);
            bestReaction.atkName = usedSkill.NAME;
            bestReaction.dmgElement = usedSkill.ELEMENT;
        }
        else
        {
            bestReaction = myManager.CalcDamage(this, target, WEAPON);
            bestReaction.atkName = WEAPON.NAME;
            bestReaction.dmgElement = WEAPON.AFINITY;
        }


        if (usedSkill)
        {
        float modification = 1.0f;

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
            usedSkill.UseSkill(this, modification);
        }
        return bestReaction;
    }
    public void DetermineActions()
    {
      //  Debug.Log("determining actions");

        isPerforming = true;
        int psudeoActions = -1;
        psudeoActions = ACTIONS;

        List<EActType> etypes = new List<EActType>();
      
        LivingObject liveObj = null;
        for (int i = 0; i < psudeoActions; i++)
        {

            liveObj = FindNearestEnemy();
            if (liveObj)
            {
               // Debug.Log("glyph creatign atk event");
                myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event", HAtkEvent);
            }
        }
        if(!liveObj)
        {
        //    Debug.Log("glyph decide to wait");
            //myManager.CreateEvent(this, this, "" + FullName + " Next event", myManager.NextTurnEvent,null, 0);

            Wait();
            TakeAction();
        }

        isPerforming = false;
    }





    public UsableScript GiveReward(LivingObject killer)
    {
        if (droppedItemNum >= 0)
        {
            DatabaseManager database = GameObject.FindObjectOfType<DatabaseManager>();
            if (database)
            {

                if (dropsSkill)
                {
                    return database.LearnSkill(droppedItemNum, killer);
                }
                else
                {
                    return database.GetWeapon(droppedItemNum, killer);
                }
            }
        }
        return null;
    }


    public override IEnumerator FadeOut()
    {
        startedDeathAnimation = true;
        //  Debug.Log("hazard dying");
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
            if(currentTile)
            {
            currentTile.isOccupied = false;

            }

            //   Destroy(gameObject);
        }
    }
    public void Unset()
    {
        isSetup = false;
        isSetup = false;
        DEAD = false;
        STATS.Reset(true);
        BASE_STATS.Reset();
        BASE_STATS.HEALTH = BASE_STATS.MAX_HEALTH;

    }

}
