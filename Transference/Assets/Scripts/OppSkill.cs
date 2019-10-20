using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppSkill : SkillScript
{

    // [SerializeField]
    // CommandSkill equippedSkill;

    [SerializeField]
    List<Element> triggers = new List<Element>();

    [SerializeField]
    Element reaction;



    [SerializeField]
    protected int next;

    [SerializeField]
    private DMG damage = DMG.tiny;

    [SerializeField]
    private RangeType rType = RangeType.pinWheel;

    public RangeType RTYPE
    {
        get { return rType; }
        set { rType = value; }
    }

    public DMG DAMAGE
    {
        get { return damage; }
        set { damage = value; }
    }

    public List<Element> TRIGGERS
    {
        get { return triggers; }
        set { triggers = value; }
    }

    public Element REACTION
    {
        get { return reaction; }
        set { reaction = value; }
    }


    public int NEXT
    {
        get { return next; }
        set { next = value; }
    }


    public bool CanUse()
    {
        return true;
        //       return equippedSkill.CanUse();
    }

    public void UseSkill(LivingObject user)
    {
        //       equippedSkill.UseSkill(user);
    }

    public override void UpdateDesc()
    {
        base.UpdateDesc();
        DESC = "Grants access to a free " + REACTION.ToString() + " " + SUBTYPE.ToString() + " after an ally hits with a ";
        if (triggers.Count > 1)
        {

            for (int i = 0; i < TRIGGERS.Count - 1; i++)
            {
                DESC += TRIGGERS[i] + ", ";                
            }
            DESC += "or "+TRIGGERS[TRIGGERS.Count -1]+ " attack 1-2 spaces away.";
        }
        else
        {
            DESC += TRIGGERS[0] + " attack 1-2 spaces away.";
        }


        DESC += " Cannot trigger another opportunity skill until " + OWNER.NAME + " moves";
    }

}
