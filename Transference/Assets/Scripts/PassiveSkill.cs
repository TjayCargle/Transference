using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkill : SkillScript
{
    [SerializeField]
    private ModifiedStat modStat;

    [SerializeField]
    private List<Element> modElements;

    [SerializeField]
    private List<float> modValues;

    [SerializeField]
    private float percent;

    public ModifiedStat ModStat
    {
        get { return modStat; }
        set { modStat = value; }
    }

    public List<Element> ModElements
    {
        get { return modElements; }

        set { modElements = value; }
    }

    public List<float> ModValues
    {
        get { return modValues; }

        set { modValues = value; }
    }

    public float PERCENT
    {
        get { return percent; }

        set { percent = value; }
    }

    public override void ApplyAugment(Augment augment)
    {
        float enhanceAmount = 15.0f;
        
        switch (augment)
        {

            case Augment.effectAugment1:
                for (int i = 0; i < ModValues.Count; i++)
                {
                    ModValues[i] += enhanceAmount;
                }
          
                break;
            case Augment.effectAugment2:
                for (int i = 0; i < ModValues.Count; i++)
                {
                    ModValues[i] += enhanceAmount;
                }
                break;
            case Augment.effectAugment3:
                for (int i = 0; i < ModValues.Count; i++)
                {
                    ModValues[i] += enhanceAmount;
                }
                break;

        }
    }

    public override void UpdateDesc()
    {
        base.UpdateDesc();
        switch (ModStat)
        {
            case ModifiedStat.Health:
                break;
            case ModifiedStat.SP:
                break;
            case ModifiedStat.FT:
                break;
            case ModifiedStat.FTCost:
                DESC = "Decreases FT cost by " + ModValues[0].ToString() + "%";

                break;
            case ModifiedStat.FTCharge:
                if (PERCENT < 0)
                {
                    DESC = "Decreases FT charge by " + ModValues[0].ToString() + "%";
                }
                else
                {
                    DESC = "Increases FT charge by " + ModValues[0].ToString() + "%";
                }
                break;
            case ModifiedStat.SPCost:
                DESC = "Decreases mana cost by " + ModValues[0].ToString() + "%";

                break;
            case ModifiedStat.ElementDmg:

                // DESC = ModElements[0] + " and "+ModElements[1]+" attacks do "+ModValues[0].ToString()+"% more dmg ";
                DESC = ModElements[0] + " and " + ModElements[1] + " attacks do " + (ModValues[0] / 100) + "x more dmg. This DOES stack with other passives. ";
                break;
            case ModifiedStat.Movement:
                break;
            case ModifiedStat.Str:
                DESC = "Increases Strength by " + ModValues[0].ToString() + "%"; ;
                break;
            case ModifiedStat.Mag:
                DESC = "Increases Magic by " + ModValues[0].ToString() + "%"; ;
                break;
            case ModifiedStat.Atk:
                DESC = "Increases Strength, Magic, and Dexterity by " + ModValues[0].ToString() + "%"; ;
                break;
            case ModifiedStat.Def:
                DESC = "Increases Defense by " + ModValues[0].ToString() + "%"; ;
                break;
            case ModifiedStat.Res:
                DESC = "Increases Resistance by " + ModValues[0].ToString() + "%"; ;
                break;
            case ModifiedStat.Guard:
                DESC = "Increases Defense, Resistance, and Speed  by " + ModValues[0].ToString() + "%"; ;
                break;
            case ModifiedStat.Spd:
                DESC = "Increases Speed by " + ModValues[0].ToString() + "%"; ;
                break;
            case ModifiedStat.Dex:
                DESC = "Increases Dexterity by " + ModValues[0].ToString() + "%"; ;
                break;
            case ModifiedStat.dmg:
                break;
            case ModifiedStat.none:
                break;
            case ModifiedStat.all:
                break;
            case ModifiedStat.ElementBody:
                break;
            case ModifiedStat.deathAct:
                DESC = "Gain an additional action point after defeating an enemy";
                break;
            case ModifiedStat.oppAct:
                break;
            case ModifiedStat.moveAct:
                DESC = "No action points are used when moving.";
                break;
        }
    }
}
