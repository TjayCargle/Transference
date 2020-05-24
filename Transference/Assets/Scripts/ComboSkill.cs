using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSkill : SkillScript
{
    [SerializeField]
    private int actionGrant = 1;

    [SerializeField]
    private Element myFirstElement;

    [SerializeField]
    private Element mySecondElement;

    [SerializeField]
    private Element myThirdElement;

    public Element FIRST
    {
        get { return myFirstElement; }

        set { myFirstElement = value; }
    }
    public Element SECOND
    {
        get { return mySecondElement; }

        set { mySecondElement = value; }
    }
    public Element THIRD
    {
        get { return myThirdElement; }

        set { myThirdElement = value; }
    }

    public int GAIN
    {
        get { return actionGrant; }
        set { actionGrant = value; }
    }


    public override void ApplyAugment(Augment augment)
    {

        switch (augment)
        {

            case Augment.effectAugment1:

                {
                    actionGrant++;
                }

                break;
            case Augment.effectAugment2:
                {
                    actionGrant++;
                }

                break;
            case Augment.effectAugment3:
                {
                    actionGrant++;
                }

                break;

        }
    }

    public override void UpdateDesc()
    {
        base.UpdateDesc();
        DESC = "Combo: " + FIRST.ToString() + ", " + SECOND.ToString() + ", " + THIRD.ToString() + ". ";
        if (actionGrant > 0)
            DESC += "Grants +" + actionGrant + " actions on the following turn if above sequence is used.";
        else if (SPECIAL_EVENTS.Count > 0)
        {
            for (int i = 0; i < SPECIAL_EVENTS.Count; i++)
            {
                SkillEventContainer sec = SPECIAL_EVENTS[i];
                DESC += Common.GetSkillEventText(sec.theEvent, sec.theReaction) + " ";


            }
        }
    }

    public override Reaction Activate(SkillReaction reaction, float amount, GridObject target)
    {
        if (actionGrant > 0)
        {
            if (OWNER)
            {
                OWNER.GENERATED++;
            }
            else
            {
                Debug.Log("no owner");
            }
        }

        switch (reaction)
        {


            case SkillReaction.maxMana:
                {
                    if (OWNER)
                    {
                        OWNER.ChangeMana(OWNER.MAX_MANA);
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;
            case SkillReaction.maxHealth:
                {
                    if (OWNER)
                    {
                        OWNER.ChangeHealth(OWNER.MAX_HEALTH);
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;
            case SkillReaction.MaxFatigue:
                {
                    if (OWNER)
                    {
                        OWNER.ChangeFatigue(OWNER.MAX_FATIGUE * -1);
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;
            case SkillReaction.FatigueZero:
                {
                    if (OWNER)
                    {
                        OWNER.ChangeFatigue(OWNER.MAX_FATIGUE);
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;
            case SkillReaction.enterGuardState:
                {
                    if (OWNER)
                    {
                        OWNER.PSTATUS = PrimaryStatus.guarding;
                        OWNER.updateAilmentIcons();
                    }
                    else
                    {
                        Debug.Log("no owner");
                    }
                }
                break;

            default:
                {
                }
                break;
        }
        UpdateDesc();
        return Reaction.none;
    }
}
