using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScript : UsableScript
{
  

    [SerializeField]
    protected Element affinity;

    [SerializeField]
    protected SkillType skilltype;

    [SerializeField]
    protected SubSkillType subskilltype;





    public LivingObject OWNER
    {
        get { return owner; }
        set { owner = value; }
    }

    public Element ELEMENT
    {
        get { return affinity; }
        set { affinity = value; }
    }

    public SkillType SKILLTYPE
    {
        get { return skilltype; }
        set { skilltype = value; }
    }

    public SubSkillType SUBTYPE
    {
        get { return subskilltype; }
        set { subskilltype = value; }
    }



   
    public void Transfer(SkillScript skill)
    {
        skill.NAME = this.NAME;
        skill.DESC = this.DESC;
        skill.OWNER = this.OWNER;
        skill.ELEMENT = this.ELEMENT;
        skill.TYPE = this.TYPE;
        skill.INDEX = this.index;
        skill.SKILLTYPE = this.SKILLTYPE;
        skill.SUBTYPE = this.SUBTYPE;
        skill.AUGMENTS = this.AUGMENTS;
        skill.USER = this.USER;
    }


    public override void UpdateDesc()
    {
        base.UpdateDesc();
    }



}
