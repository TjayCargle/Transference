using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardScript : LivingObject
{

    BaseStats myStats;

    public int droppedItemNum = -1;

    [SerializeField]
    private HazardType htype = HazardType.attacker;

    public List<SubSkillType> hackingSequence = new List<SubSkillType>();
    public float hackingTimer;
    public int hackStrikes = 0;
    public int hackSkills = 0;
    public int hackSpells = 0;
    public List<int> revealTiles = new List<int>();
    private float internalTimer = 1.0f;
    private bool waiting = false;
    private LivingObject lastTarget;
    private List<TileScript> personalAttackList = new List<TileScript>();
    public int REWARD
    {
        get { return droppedItemNum; }
        set { droppedItemNum = value; }
    }

    public HazardType HTYPE
    {
        get { return htype; }
        set { htype = value; }
    }
    public bool isPerforming = false;

    public override void Setup()
    {
        if (!isSetup)
        {

            base.Setup();
            //  BASE_STATS.MANA = 0;
            BASE_STATS.MAX_MANA = 0;
            BASE_STATS.MAX_FATIGUE = 0;
            FACTION = Faction.hazard;
            //FullName = "Glyph";
            isSetup = true;
        }

    }
    public override void Die()
    {
        base.Die();

    }



    public void AttackStart()
    {
        internalTimer = 1.0f;
        myManager.CreateEvent(this, this, "Select Camera Event", myManager.CameraEvent, null, 0);
        myManager.myCamera.UpdateCamera();

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
            if (!WEAPON.EQUIPPED)
            {
                if (INVENTORY.WEAPONS.Count > 0)
                {
                    WEAPON.Equip(INVENTORY.WEAPONS[0]);
                    attackbleTiles = myManager.GetWeaponAttackableTiles(this);
                }
            }
            else
            {
                return null;
            }
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
    public bool HWaitEvent(Object target)
    {

        bool isDone = true;

        Wait();
        TakeRealAction();

        return isDone;
    }
    public bool HAtkEvent(Object target)
    {
            LivingObject realTarget = target as LivingObject;
        if (INVENTORY.CSKILLS.Count > 0)
        {

            myManager.ShowSkillAttackbleTiles(this, INVENTORY.CSKILLS[0], realTarget);
        }
        else if (INVENTORY.WEAPONS.Count > 0)
        {

            myManager.ShowWeaponAttackbleTiles(this, INVENTORY.WEAPONS[0], realTarget);
        }

        if (internalTimer >= 0)
        {
            internalTimer -= 0.02f;
            return false;
        }
          myManager.MoveCameraAndShow(this); 
        //  Debug.Log(FullName + " atacking");
        bool isDone = true;
        {


            DmgReaction bestReaction = DetermineBestDmgOutput(realTarget);
          //  myManager.CreateTextEvent(this, bestReaction.atkName, "enemy atk", myManager.CheckText, myManager.TextStart);

            //if (myManager.log)
            //{
            //    myManager.log.Log("<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(FACTION)) + ">" + FullName + "</color> used " + bestReaction.atkName);
            //}
            // myManager.ApplyReaction(this, realTarget, bestReaction, bestReaction.dmgElement);
            //myManager.myCamera.infoObject = realTarget;
            //myManager.myCamera.UpdateCamera();


            if (bestReaction.reaction != Reaction.missed)
            {
                if (INVENTORY.CSKILLS.Count > 0)
                {
                    CommandSkill skill = INVENTORY.CSKILLS[0];
                    //AtkContainer conatiner = ScriptableObject.CreateInstance<AtkContainer>();
                    //conatiner.alteration = skill.REACTION;
                    //conatiner.attackingElement = skill.ELEMENT;
                    //conatiner.attackType = skill.ETYPE;
                    //conatiner.command = skill;
                    //conatiner.dmg = (int)skill.DAMAGE;
                    //conatiner.attackingObject = this;
                    //conatiner.dmgObject = realTarget;
                    //conatiner.attackingElement = bestReaction.dmgElement;          
                    //myManager.CreateEvent(this, conatiner, "apply reaction event", myManager.ApplyReactionEvent, null, 0);
                    //myManager.CreateGridAnimationEvent(conatiner.dmgObject.currentTile.transform.position, conatiner.command, conatiner.dmg);


                    myManager.attackableTiles = myManager.GetSkillsAttackableTiles(this, skill);
                    myManager.currentAttackList.Clear();
                    personalAttackList.Clear();
                    personalAttackList.Add(realTarget.currentTile);
                    myManager.currentAttackList.AddRange(personalAttackList);

                    myManager.AttackTargets(this, skill, true);
                }
                else
                {
              
                }
                //Debug.Log("checkin real" + realTarget); > 0
                //myManager.CreateEvent(this, conatiner, "" + FullName + "enemy opp event", myManager.ECheckForOppChanceEvent, null, 1);
            }
            // Debug.Log(FullName + " used " + bestReaction.atkName);

            // TakeAction();
            TakeRealAction();
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
            bestReaction = myManager.CalcDamage(this, target, usedSkill, Reaction.none,false);
            bestReaction.atkName = usedSkill.NAME;
            bestReaction.dmgElement = usedSkill.ELEMENT;
        }
        else
        {
            bestReaction = myManager.CalcDamage(this, target, WEAPON, Reaction.none, false);
            bestReaction.atkName = WEAPON.NAME;
            bestReaction.dmgElement = WEAPON.ELEMENT;
        }


        if (usedSkill)
        {
            float modification = 1.0f;

            if (usedSkill.ETYPE == EType.magical)
                modification = STATS.MANACHANGE;
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
            //usedSkill.UseSkill(this, modification);
        }
        return bestReaction;
    }

    public bool DetermineNextAction(Object target)
    {
        if (DEAD)
        {
            return true;
        }
        if (waiting)
        {
            return true;
        }

        LivingObject liveObj = null;
        liveObj = FindNearestEnemy();
        if (liveObj)
        {
            // Debug.Log("glyph creatign atk event");
            myManager.CreateEvent(this, liveObj, "" + FullName + "Atk event", HAtkEvent, AttackStart);
        }
        else
        {
            waiting = true;
            myManager.CreateEvent(this, liveObj, "" + FullName + "wait event ", HWaitEvent);
        }

        return true;
    }

    public void DetermineActions()
    {
        //  Debug.Log("determining actions");

        isPerforming = true;
        int psudeoActions = -1;
        psudeoActions = ACTIONS;
        waiting = false;


        for (int i = 0; i < psudeoActions; i++)
        {
            myManager.CreateEvent(this, null, "" + FullName + "determine action event " + i, DetermineNextAction, null, 0);

        }
        if(ACTIONS == 0)
        {
            myManager.NextTurn(FullName);
        }


        isPerforming = false;
    }

private void IncreaseHacks(SubSkillType sub)
{
    switch (sub)
    {

        case SubSkillType.Movement:
            {
                //auto
                int rng = Random.Range(0, 2);
                if (rng == 0)
                    hackStrikes++;
                else
                    hackSkills++;

            }
            break;
        case SubSkillType.Item:
            {
                //barrier
                int rng = Random.Range(0, 2);
                if (rng == 0)
                    hackSkills++;
                else
                    hackSpells++;

            }
            break;
        case SubSkillType.Ailment:
            {
                //passive
                int rng = Random.Range(0, 2);
                if (rng == 0)
                    hackStrikes++;
                else
                    hackSpells++;

            }
            break;
        case SubSkillType.Strike:
            {
                //strikes
                hackStrikes++;
            }
            break;
        case SubSkillType.Skill:
            {
                //skills
                hackSkills++;
            }
            break;
        case SubSkillType.Spell:
            {
                //spells
                hackSpells++;
            }
            break;
        case SubSkillType.None:
            {
                //anything
                int rng = Random.Range(0, 3);
                if (rng == 0)
                    hackStrikes++;
                else if (rng == 1)
                    hackSkills++;
                else
                    hackSpells++;
            }
            break;
    }
}
    public void generateSequence()
    {
        hackingSequence.Clear();
 
        hackStrikes = 3;
        hackSkills = 3;
        hackSpells = 3;
        SubSkillType sub1 = SubSkillType.Strike;
        SubSkillType sub2 = SubSkillType.Skill;
        SubSkillType sub3 = SubSkillType.Spell;
        SubSkillType sub4 = SubSkillType.Item;
        SubSkillType sub5 = SubSkillType.Movement;
        SubSkillType sub6 = SubSkillType.Ailment;
        SubSkillType sub7 = SubSkillType.None;
        int rand = 0;
        if (LEVEL < 3)
        {
            //3
            hackingTimer = 60.0f;
            //for strikes
            for (int i = 0; i < 3; i++)
            {
                rand = Random.Range(0, 2);
                if(rand == 0)
                {
                hackingSequence.Add(sub1);
                }
                else
                {
                    hackingSequence.Add(sub5);
                }

            }

            //for skills
            for (int i = 0; i < 3; i++)
            {
                rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    hackingSequence.Add(sub2);
                }
                else if (rand == 1)
                {
                    hackingSequence.Add(sub5);
                }
                else
                {
                    hackingSequence.Add(sub4);
                }

            }
            //for spells
            for (int i = 0; i < 3; i++)
            {
                rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    hackingSequence.Add(sub3);
                }
                else
                {
                    hackingSequence.Add(sub4);
                }


            }

        }
        else if (LEVEL >= 3 && LEVEL < 9)
        {
            //5
            hackingTimer = 45.0f;
            //for strikes
            for (int i = 0; i < 3; i++)
            {
                rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    hackingSequence.Add(sub1);
                }
                else if (rand == 1)
                {
                    hackingSequence.Add(sub5);
                }
                else
                {
                    hackingSequence.Add(sub6);

                }

            }

            //for skills
            for (int i = 0; i < 3; i++)
            {
                rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    hackingSequence.Add(sub2);
                }
                else if (rand == 1)
                {
                    hackingSequence.Add(sub5);
                }
                else
                {
                    hackingSequence.Add(sub4);
                }


            }
            //for spells
            for (int i = 0; i < 3; i++)
            {
                rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    hackingSequence.Add(sub3);
                }
                else if (rand == 1)
                {
                    hackingSequence.Add(sub4);
                }
                else
                {
                    hackingSequence.Add(sub6);
                }


            }
        }
        else if (LEVEL >= 9 && LEVEL < 12)
        {
            //7
            hackingTimer = 30.0f;
            for (int i = 0; i < 3; i++)
            {
                rand = Random.Range(0, 4);
                if (rand == 0)
                {
                    hackingSequence.Add(sub1);
                }
                else if (rand == 1)
                {
                    hackingSequence.Add(sub5);
                }
                else if (rand == 2)
                {
                    hackingSequence.Add(sub6);

                }
                else
                {
                    hackingSequence.Add(sub7);
                }

            }

            //for skills
            for (int i = 0; i < 3; i++)
            {
                rand = Random.Range(0, 4);
                if (rand == 0)
                {
                    hackingSequence.Add(sub2);
                }
                else if (rand == 1)
                {
                    hackingSequence.Add(sub5);
                }
                else if(rand == 2)
                {
                    hackingSequence.Add(sub4);
                }
                else
                {
                    hackingSequence.Add(sub7);
                }


            }
            //for spells
            for (int i = 0; i < 3; i++)
            {
                rand = Random.Range(0, 4);
                if (rand == 0)
                {
                    hackingSequence.Add(sub3);
                }
                else if (rand == 1)
                {
                    hackingSequence.Add(sub4);
                }
                else if (rand == 2)
                {
                    hackingSequence.Add(sub6);
                }
                else
                {
                    hackingSequence.Add(sub7);
                }


            }
        }
        else
        {
            //9
            hackingTimer = 15.0f;
            for (int i = 0; i < 3; i++)
            {
                rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    hackingSequence.Add(sub7);
                }
                else if (rand == 1)
                {
                    hackingSequence.Add(sub5);
                }
                else 
                {
                    hackingSequence.Add(sub6);

                }
          

            }

            //for skills
            for (int i = 0; i < 3; i++)
            {
                rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    hackingSequence.Add(sub7);
                }
                else if (rand == 1)
                {
                    hackingSequence.Add(sub5);
                }
                else 
                {
                    hackingSequence.Add(sub4);
                }
     


            }
            //for spells
            for (int i = 0; i < 3; i++)
            {
                rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    hackingSequence.Add(sub7);
                }
                else if (rand == 1)
                {
                    hackingSequence.Add(sub4);
                }
                else 
                {
                    hackingSequence.Add(sub6);
                }
         


            }
        }


        //time to randomize the list

        int randLength = Random.Range(9, 18);

        for (int i = 0; i < randLength; i++)
        {
            rand = Random.Range(0, 6);
            SubSkillType temp = hackingSequence[rand];

            int rand2 = Random.Range(3, hackingSequence.Count);
            SubSkillType temp2 = hackingSequence[rand2];

            hackingSequence[rand2] = temp;
            hackingSequence[rand] = temp2;

        }
    }


    public UsableScript GiveReward(LivingObject killer)
    {
        if (droppedItemNum >= 0)
        {
            DatabaseManager database = Common.GetDatabase();
            if (database)
            {

                return database.LearnSkill(droppedItemNum, killer);

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

            myManager.gridObjects.Remove(this);
            gameObject.SetActive(false);
            if (currentTile)
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
        STATS.HEALTH = BASE_STATS.MAX_HEALTH;
        INVENTORY.Clear();
        PHYSICAL_SLOTS.SKILLS.Clear();
        PASSIVE_SLOTS.SKILLS.Clear();
        OPP_SLOTS.SKILLS.Clear();
        AUTO_SLOTS.SKILLS.Clear();
        dexLevel = 1;
        magLevel = 1;
        physLevel = 1;

        if (DEFAULT_ARMOR)
        {
            ARMOR.unEquip();
            DEFAULT_ARMOR = null;

        }
        refreshState = 0;
        PSTATUS = PrimaryStatus.normal;

        shadow.GetComponent<AnimationScript>().Unset();
        if (GetComponent<AnimationScript>())
        {
            GetComponent<AnimationScript>().Unset();
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

    private void Update()
    {
      if(Input.GetKeyDown(KeyCode.G))
        {
            generateSequence();
        }
    }
}
