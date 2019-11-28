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
        if (INVENTORY.CSKILLS.Count > 0)
        {
           
                myManager.ShowSkillAttackbleTiles(this, INVENTORY.CSKILLS[0], lastTarget);
        }
        else if(INVENTORY.WEAPONS.Count > 0)
        {
      
                myManager.ShowWeaponAttackbleTiles(this, INVENTORY.WEAPONS[0], lastTarget);
        }

        if (internalTimer >= 0)
        {
            internalTimer -= 0.02f;
            return false;
        }
        //  myManager.MoveCameraAndShow(this); 
        //  Debug.Log(FullName + " atacking");
        bool isDone = true;
        {

            LivingObject realTarget = target as LivingObject;

            DmgReaction bestReaction = DetermineBestDmgOutput(realTarget);
            myManager.CreateTextEvent(this, "" + FullName + " used " + bestReaction.atkName, "enemy atk", myManager.CheckText, myManager.TextStart);

            if (myManager.log)
            {
                myManager.log.Log("<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(FACTION)) + ">" + FullName + "</color> used " + bestReaction.atkName);
            }
            // myManager.ApplyReaction(this, realTarget, bestReaction, bestReaction.dmgElement);
            //myManager.myCamera.infoObject = realTarget;
            //myManager.myCamera.UpdateCamera();
            
      
            if (bestReaction.reaction != Reaction.missed)
            {
                if(INVENTORY.CSKILLS.Count > 0)
                {
                CommandSkill skill = INVENTORY.CSKILLS[0];             
                AtkContainer conatiner = ScriptableObject.CreateInstance<AtkContainer>();
                conatiner.alteration = skill.REACTION;
                conatiner.attackingElement = skill.ELEMENT;
                conatiner.attackType = skill.ETYPE;
                conatiner.command = skill;
                conatiner.dmg = (int)skill.DAMAGE;
                conatiner.attackingObject = this;
                conatiner.dmgObject = realTarget;
                conatiner.attackingElement = bestReaction.dmgElement;          
                myManager.CreateEvent(this, conatiner, "apply reaction event", myManager.ApplyReactionEvent, null, 0);
                myManager.CreateGridAnimationEvent(conatiner.dmgObject.currentTile.transform.position, conatiner.command, conatiner.dmg);
                }
                else if (INVENTORY.WEAPONS.Count > 0)
                {
                    WeaponScript skill = INVENTORY.WEAPONS[0];
                    AtkContainer conatiner = ScriptableObject.CreateInstance<AtkContainer>();
                    conatiner.alteration = Reaction.none;
                    conatiner.attackingElement = skill.ELEMENT;
                    conatiner.attackType = skill.ATTACK_TYPE;
                    conatiner.strike = skill;
                    conatiner.dmg = (int)skill.ATTACK;
                    conatiner.attackingObject = this;
                    conatiner.dmgObject = realTarget;
                    conatiner.attackingElement = bestReaction.dmgElement;
                    myManager.CreateEvent(this, conatiner, "apply reaction event", myManager.ApplyReactionEvent, null, 0);
                    myManager.CreateGridAnimationEvent(conatiner.dmgObject.currentTile.transform.position, conatiner.strike, conatiner.dmg);
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
            bestReaction = myManager.CalcDamage(this, target, usedSkill);
            bestReaction.atkName = usedSkill.NAME;
            bestReaction.dmgElement = usedSkill.ELEMENT;
        }
        else
        {
            bestReaction = myManager.CalcDamage(this, target, WEAPON);
            bestReaction.atkName = WEAPON.NAME;
            bestReaction.dmgElement = WEAPON.ELEMENT;
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
        if(waiting)
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


        isPerforming = false;
    }

    private void IncreaseHacks(SubSkillType sub)
    {
        switch (sub)
        {

            case SubSkillType.Movement:
                {
                    int rng = Random.Range(0, 1);
                    if (rng == 0)
                        hackSpells++;
                    else
                        hackSkills++;

                }
                break;
            case SubSkillType.Item:
                {
                    int rng = Random.Range(0, 1);
                    if (rng == 0)
                        hackStrikes++;
                    else
                        hackSkills++;

                }
                break;
            case SubSkillType.Strike:
                {
                    hackStrikes++;
                }
                break;
            case SubSkillType.Skill:
                {
                    hackSkills++;
                }
                break;
            case SubSkillType.Spell:
                {
                    hackSpells++;
                }
                break;
            case SubSkillType.None:
                {
                    int rng = Random.Range(0, 2);
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
        hackStrikes = 0;
        hackSkills = 0;
        hackSpells = 0;
        if (LEVEL < 3)
        {
            //3
            hackingTimer = 20.0f;
            for (int i = 0; i < 6; i++)
            {
                SubSkillType sub = (SubSkillType)Random.Range((int)SubSkillType.Movement, ((int)SubSkillType.None) + 1);
                IncreaseHacks(sub);
                hackingSequence.Add(sub);

            }
        }
        else if (LEVEL >= 3 && LEVEL < 8)
        {
            //5
            hackingTimer = 15.0f;
            for (int i = 0; i < 7; i++)
            {
                SubSkillType sub = (SubSkillType)Random.Range((int)SubSkillType.Movement, ((int)SubSkillType.None) + 1);
                IncreaseHacks(sub);
                hackingSequence.Add(sub);
            }
        }
        else if (LEVEL >= 8 && LEVEL < 12)
        {
            //7
            hackingTimer = 10.0f;
            for (int i = 0; i < 8; i++)
            {
                SubSkillType sub = (SubSkillType)Random.Range((int)SubSkillType.Movement, ((int)SubSkillType.None) + 1);
                IncreaseHacks(sub);
                hackingSequence.Add(sub);
            }
        }
        else
        {
            //9
            hackingTimer = 5.0f;
            for (int i = 0; i < 9; i++)
            {
                SubSkillType sub = (SubSkillType)Random.Range((int)SubSkillType.Movement, ((int)SubSkillType.None) + 1);
                IncreaseHacks(sub);
                hackingSequence.Add(sub);
            }
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
        BASE_STATS.HEALTH = BASE_STATS.MAX_HEALTH;
        INVENTORY.Clear();
        PHYSICAL_SLOTS.SKILLS.Clear();
        PASSIVE_SLOTS.SKILLS.Clear();
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
