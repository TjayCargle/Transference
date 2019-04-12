﻿using System.Collections;
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
    protected List<Augment> augments;

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
    public virtual void LevelUP()
    {
        if (LEVEL < Common.MaxSkillLevel)
        {
            LEVEL++;
       
        }
    }
    public virtual void GrantXP(float amount)
    {
        exp -= amount;
        if (exp <= 0)
        {
            LevelUP();
            exp = 2 + (level * 2);
        }
    }
  
    public virtual void ApplyAugment(Augment aug)
    {

    }

    public virtual void UpdateDesc()
    {

    }

}
