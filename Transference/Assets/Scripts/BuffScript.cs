using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffScript : MonoBehaviour
{
    [SerializeField]
    BuffType buff;
    [SerializeField]
    int countDown;

    [SerializeField]
    CommandSkill refSkill;

    public BuffType BUFF
    {
        get { return buff; }
        set { buff = value; }
    }

    public int COUNT
    {
        get { return countDown; }
        set { countDown = value; }
    }

    public CommandSkill SKILL
    {
        get { return refSkill; }
        set { refSkill = value; }
    }

    public void UpdateCount(LivingObject living)
    {
        if (!living)
        {
            Debug.Log("no target for buff :( ");
            Destroy(this);
        }


        countDown--;
        if(countDown <= 0)
        {
            living.INVENTORY.BUFFS.Remove(refSkill);
            living.UpdateBuffsAndDebuffs();
            Destroy(this);
        }
    }
}
