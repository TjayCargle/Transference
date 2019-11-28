using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableScript : ScriptableObject
{

    [SerializeField]
    protected string myName;

    [SerializeField]
    private string description;

    [SerializeField]
    protected int index;

    [SerializeField]
    protected int level = 1;

    [SerializeField]
    protected float exp = 2;

    protected int refType;

    [SerializeField]
    protected int useCount;

    [SerializeField]
    protected List<Augment> augments;
    [SerializeField]
    protected LivingObject owner;

    [SerializeField]
    protected string additionalDetails;

    public LivingObject USER
    {
        get { return owner; }
        set { owner = value; }
    }
    public List<Augment> AUGMENTS
    {
        get { return augments; }
        set { augments = value; }
    }

    public string DESC
    {
        get { return description; }
        set { description = value; }
    }

    public string DEATS
    {
        get { return additionalDetails; }
        set { additionalDetails = value; }
    }

    public string NAME
    {
        get { return myName; }
        set { myName = value; }
    }
    public int TYPE
    {
        get { return refType; }
        set { refType = value; }
    }
    public int INDEX
    {
        get { return index; }
        set { index = value; }
    }
    public int LEVEL
    {
        get { return level; }
        set { level = value; }
    }
    public int USECOUNT
    {
        get { return useCount; }
        set { useCount = value; }
    }
    public virtual void LevelUP()
    {
        if (LEVEL < Common.MaxSkillLevel)
        {
            LEVEL++;

        }
    }
    public virtual bool GrantXP(float amount)
    {

        exp -= amount;
        if (exp <= 0)
        {
            LevelUP();
            exp = 1 + (level * 2);

            if (USER)
            {
                ManagerScript manager = GameObject.FindObjectOfType<ManagerScript>();
                if (manager)
                {
                    if (manager.GetState() != State.EnemyTurn && manager.GetState() != State.HazardTurn)
                    {
                        manager.CreateEvent(this, this, "New Skill Event", manager.CheckCount, null, 0, manager.CountStart);
                        manager.CreateTextEvent(this, "" + USER.FullName + "'s " + NAME + " leveled up!", "new skill event", manager.CheckText, manager.TextStart);
                        if (manager.log)
                        {
                            string coloroption = "<color=#" + ColorUtility.ToHtmlStringRGB(Common.GetFactionColor(USER.FACTION)) + ">";

                            manager.log.Log(coloroption + USER.NAME + "</color> lost " + NAME + " leveled up!");
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }

    public virtual void ApplyAugment(Augment aug)
    {

    }

    public virtual void UpdateDesc()
    {

    }

}
