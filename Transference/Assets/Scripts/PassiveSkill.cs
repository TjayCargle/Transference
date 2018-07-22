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
}
