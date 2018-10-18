using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScript : UsableScript
{
    [SerializeField]
    protected LivingObject owner;

    [SerializeField]
    protected Element affinity;

    [SerializeField]
    protected int index;

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

    public int INDEX
    {
        get { return index; }
        set { index = value; }
    }
    public void Transfer(SkillScript skill)
    {
        skill.NAME = this.NAME;
        skill.DESC = this.DESC;
        skill.OWNER = this.OWNER;
        skill.ELEMENT = this.ELEMENT;
        skill.TYPE = this.TYPE;
        skill.INDEX = this.index;
    }

}
