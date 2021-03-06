﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillSlots : MonoBehaviour {

    private string slotName;
    private int type;
    [SerializeField]
    List<SkillScript> skills = new List<SkillScript>();

    public string NAME
    {
        get
        {
            return slotName;
        }

        set
        {
            slotName = value;
        }
    }

    public int TYPE
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    public List<SkillScript> SKILLS
    {
        get
        {
            return skills;
        }

        set
        {
            skills = value;
        }
    }

    public bool Contains(SkillScript skill)
    {
        for (int i = 0; i < SKILLS.Count; i++)
        {
            if (SKILLS[i] == skill)
            {
                return true;
            }
        }
        return false;
    }

    public bool CanAdd()
    {
        if (SKILLS.Count < 6)
            return true;
        return false;
    }
    public List<ComboSkill> ConvertToPassives()
    {
        List<ComboSkill> passives = new List<ComboSkill>();
        for (int i = 0; i < SKILLS.Count; i++)
        {
            if(skills[i].GetType() == typeof(ComboSkill))
            {
                passives.Add(skills[i] as ComboSkill);
            }
        }

        return passives;
    }
}
