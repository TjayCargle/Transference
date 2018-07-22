using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppSkill : SkillScript {

   // [SerializeField]
   // CommandSkill equippedSkill;

    [SerializeField]
    Element trigger;

    [SerializeField]
    float modifier;

    [SerializeField]
    protected int next;

    [SerializeField]
    protected int nextCount;

    public Element TRIGGER
    {
        get { return trigger; }
        set { trigger = value; }
    }

    public float MOD
    {
        get { return modifier; }
        set { modifier = value; }
    }


    public int NEXT
    {
        get { return next; }
        set { next = value; }
    }

    public int NEXTCOUNT
    {
        get { return nextCount; }
        set { nextCount = value; }
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


}
