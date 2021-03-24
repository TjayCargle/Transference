using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorScript : UsableScript
{
    [SerializeField]
    private int myDefense;
    [SerializeField]
    private int myRes;
    [SerializeField]
    private int mySpeed;

    [SerializeField]
    private int turnCount = 2;

    [SerializeField]
    private List<EHitType> hitList;

    public Sprite mySprite;
    //[SerializeField]
    //private int myHealth;

    //[SerializeField]
    //private int myMaxHealth;

    [SerializeField]
    private float healthPercent = 100.0f;

    [SerializeField]
    private float maxHealthPercent = 100.0f;
   

    public List<EHitType> HITLIST
    {
        get { return hitList; }
        set { hitList = value; }
    }

    public Sprite FACE
    {
        get { if (mySprite == null) { mySprite = Resources.LoadAll<Sprite>("Shields/")[INDEX]; } return mySprite; }
        set { mySprite = value; }
    }
    public int DEFENSE
    {
        get { return myDefense; }
        set { myDefense = value; }
    }
    public int RESISTANCE
    {
        get { return myRes; }
        set { myRes = value; }
    }
    public int SPEED
    {
        get { return mySpeed; }
        set { mySpeed = value; }
    }
    public int TURNCOUNT
    {
        get { return turnCount; }
        set { turnCount = value; }
    }
    public float MAX_HEALTH
    {
        get { return maxHealthPercent; }
        set
        {
            maxHealthPercent = value;
            if (maxHealthPercent > 100)
            {
                maxHealthPercent = 100;
            }
        }
    }
    public float HEALTH
    {
        get { return healthPercent; }
        set
        {
            healthPercent = value;
            if (healthPercent > maxHealthPercent)
            {
                healthPercent = maxHealthPercent;
            }

            if (healthPercent < 0)
            {
                healthPercent = 0;
            }
        }
    }
    public override void LevelUP()
    {
    
        if(LEVEL < Common.MaxSkillLevel)
        {
            LEVEL++;
            switch (LEVEL)
            {

                case 2:
                    {
                        DEFENSE++;
                        TURNCOUNT++;
                    }
                    break;
                case 3:
                    {
                        RESISTANCE++;
                    }
                    break;
                case 4:
                    {
                        SPEED++;
                        TURNCOUNT++;
                    }
                    break;
                case 5:
                    {
                        DEFENSE++;
                        RESISTANCE++;
                        SPEED++;
                    }
                    break;
                case 6:
                    {
                        DEFENSE+=2;
                        TURNCOUNT++;
                    }
                    break;
                case 7:
                    {
                        RESISTANCE+=2;
                    }
                    break;
                case 8:
                    {
                        SPEED+=2;
                        TURNCOUNT++;
                    }
                    break;
                case 9:
                    {
                        DEFENSE+=2;
                        RESISTANCE+=2;
                        SPEED+=2;
                    }
                    break;
                case 10:
                    {
                        DEFENSE++;
                        RESISTANCE++;
                        SPEED++;
                        TURNCOUNT++;
                    }
                    break;
            }
        }
    }

    public void Use()
    {

        USECOUNT++;
        if(USECOUNT % 2 == 0)
        {
            if(LEVEL < Common.MaxSkillLevel)
            {
                LevelUP();
               // UpdateDesc();
                ManagerScript manager = GameObject.FindObjectOfType<ManagerScript>();
           
                  
                manager.CreateEvent(this, this, "New Skill Event", manager.CheckCount, null, 0, manager.CountStart);
               // manager.CreateTextEvent(this, "" + owner.FullName + "'s " + NAME + " leveled up!", "new skill event", manager.CheckText, manager.TextStart);
                
            }
        }
    }

    public override void UpdateDesc()
    {
        base.UpdateDesc();
        DESC = "Def +" + DEFENSE + ", Res+" + RESISTANCE + " Spd +" + SPEED + ". Changes resistances for " + TURNCOUNT + " turns.";
        //for (int i = 0; i < HITLIST.Count; i++)
        //{
        //    if (HITLIST[i] != EHitType.normal)
        //    {
        //        if (HITLIST[i] < EHitType.normal)
        //        {
        //            DESC += ", " + HITLIST[i].ToString() + " " + (Element)i;
        //        }
        //        else
        //        {
        //            DESC += ", " + HITLIST[i].ToString() + " to " + (Element)i;
        //        }
        //    }
        //}
    }

    public string GetCurrentLevelStats()
    {
        string returnedString = "";
        if (level < Common.MaxSkillLevel)
        {
            returnedString = "Level " + LEVEL + "";
        }
        else
        {
            returnedString = "Max Level ";
        }

        returnedString += "\n Def +" + DEFENSE.ToString() + "";
        returnedString += "\n Res +" + (RESISTANCE) + "";
        returnedString += "\n Spd +" + (SPEED) + "";
        returnedString += "\n Lasts: " + (TURNCOUNT) + " turns";
        return returnedString;
    }
    public string GetNextLevelStats()
    {
        string returnedString = "";
        if (level + 1 < Common.MaxSkillLevel)
        {
            returnedString = "<color=green>Level " + (LEVEL + 1) + "</color>";
        }
        else if (level + 1 == Common.MaxSkillLevel)
        {
            returnedString = "<color=green>Max Level</color>";
        }
        else
        {
            return GetCurrentLevelStats();
        }
       

            switch ((LEVEL + 1))
            {

                case 2:
                    {
                      
                       
                        returnedString += "\n Def +<color=green>"+ (DEFENSE + 1).ToString() + "</color>";
                        returnedString += "\n Res +" + (RESISTANCE) + "";
                        returnedString += "\n Spd +" + (SPEED) + "";
                        returnedString += "\n Lasts: <color=green>" + (TURNCOUNT + 1).ToString() + "</color> turns";
                  
                    }
                    break;
                case 3:
                    {
                       
                        returnedString += "\n Def +" + DEFENSE.ToString() + "";
                        returnedString += "\n Res +<color=green>" + (RESISTANCE + 1).ToString() + "</color>";
                        returnedString += "\n Spd +" + (SPEED) + "";
                        returnedString += "\n Lasts: <color=green>" + (TURNCOUNT + 1).ToString() + "</color> turns";
                    }
                    break;
                case 4:
                    {
                      
                       
                        returnedString += "\n Def +" + DEFENSE.ToString() + "";
                        returnedString += "\n Res +" + (RESISTANCE) + "";
                        returnedString += "\n Spd +<color=green>" + (SPEED + 1).ToString() + "</color>";
                        returnedString += "\n Lasts: <color=green>" + (TURNCOUNT + 1).ToString() + "</color> turns";
                    }
                    break;
                case 5:
                    {

                      
            
                        returnedString += "\n Def +<color=green>" + (DEFENSE + 1).ToString() + "</color>";
                        returnedString += "\n Res +<color=green>" + (RESISTANCE + 1).ToString() + "</color>";
                        returnedString += "\n Spd +<color=green>" + (SPEED + 1).ToString() + "</color>";
                        returnedString += "\n Lasts: <color=green>" + (TURNCOUNT + 1).ToString() + "</color> turns";
                    }
                    break;
                case 6:
                    {
                       
                      
                        returnedString += "\n Def +<color=green>" + (DEFENSE + 2).ToString() + "</color>";
                        returnedString += "\n Res +" + (RESISTANCE) + "";
                        returnedString += "\n Spd +" + (SPEED) + "";
                        returnedString += "\n Lasts: <color=green>" + (TURNCOUNT + 1).ToString() + "turns </color>";
                    }
                    break;
                case 7:
                    {
                      
                        returnedString += "\n Def +" + DEFENSE.ToString() + "";
                        returnedString += "\n Res +<color=green>" + (RESISTANCE + 2).ToString() + "</color>";
                        returnedString += "\n Spd +" + (SPEED) + "";
                        returnedString += "\n Lasts: <color=green>" + (TURNCOUNT + 1).ToString() + "turns </color>";
                    }
                    break;
                case 8:
                    {
                      
                        
                        returnedString += "\n Def +" + DEFENSE.ToString() + "";
                        returnedString += "\n Res +" + (RESISTANCE) + "";
                        returnedString += "\n Spd +<color=green>" + (SPEED + 2).ToString() + "</color>";
                        returnedString += "\n Lasts: <color=green>" + (TURNCOUNT + 1).ToString() + "turns </color>";
                    }
                    break;
                case 9:
                    {
                       
                       
                       
                        returnedString += "\n Def +<color=green>" + (DEFENSE + 2).ToString() + "</color>";
                        returnedString += "\n Res +<color=green>" + (RESISTANCE + 2).ToString() + "</color>";
                        returnedString += "\n Spd +<color=green>" + (SPEED + 2).ToString() + "</color>";
                        returnedString += "\n Lasts: <color=green>" + (TURNCOUNT + 1).ToString() + " turns </color>";
                    }
                    break;
                case 10:
                    {
                      
                       
                    
                       
                        returnedString += "\n Def +<color=green>" + (DEFENSE + 1).ToString() + "</color>";
                        returnedString += "\n Res +<color=green>" + (RESISTANCE + 2).ToString() + "</color>";
                        returnedString += "\n Spd +<color=green>" + (SPEED + 1).ToString() + "</color>";
                        returnedString += "\n Lasts: <color=green>" + (TURNCOUNT + 1).ToString() + " turns </color> ";
                    }
                    break;
            

        }

        return returnedString;
    }
    public override void ApplyAugment(Augment aug)
    {
        base.ApplyAugment(aug);
        switch (aug)
        {
     
            case Augment.levelAugment:
                for (int i = 0; i < 5; i++)
                {
                    LevelUP();
                }
                break;
           
            case Augment.effectAugment1:
                for (int i = 0; i < HITLIST.Count; i++)
                {
                    if(HITLIST[i] < EHitType.normal && HITLIST[i] > EHitType.absorbs)
                    {
                        HITLIST[i] = HITLIST[i] - 1;
                    }
                    else if (HITLIST[i] > EHitType.normal && HITLIST[i] < EHitType.lethal)
                    {
                        HITLIST[i] = HITLIST[i] + 1;
                    }
                }
                break;
           
            case Augment.strAugment:
                break;
            case Augment.magAugment:
                break;
            case Augment.dexAugment:
                break;
            case Augment.defAugment:
                DEFENSE += 5;
                break;
            case Augment.resAugment:
                RESISTANCE += 5;
                break;
            case Augment.spdAugment:
                SPEED += 5;
                break;
         
        }

    }

    public override string GetDataString()
    {
        string dataString = base.GetDataString();

        dataString += "," + myDefense + "," + myRes + "," + mySpeed + "," + turnCount +","+ healthPercent +","+ maxHealthPercent;
        for (int i = 0; i < HITLIST.Count; i++)
        {
            dataString += "," + HITLIST[i];
        }

        return dataString;
    }
}
