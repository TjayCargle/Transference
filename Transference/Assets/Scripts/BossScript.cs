using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : EnemyScript
{
    private int healthbars;

    public int HEALTHBARS
    {
        get { return healthbars; }
        set { healthbars = value; }
    }

    private void NextPhase()
    {
        SoftUnset();
    }

    public override bool CheckIfDead()
    {
        if (HEALTH <= 0)
        {
            if (HEALTHBARS <= 1)
            {
                return true;
            }
            else
            {
                NextPhase();
            }
        }
        return false;
    }

    private void SoftUnset()
    {
        if (isSetup == true)
        {

            isSetup = false;
            DEAD = false;
            STATS.Reset(true);
            BASE_STATS.Reset();
            BASE_STATS.MAX_HEALTH += (int)((float)BASE_STATS.MAX_HEALTH * 0.5f);
            STATS.HEALTH = BASE_STATS.MAX_HEALTH;
 
            if (DEFAULT_ARMOR)
            {
                ARMOR.unEquip();
                DEFAULT_ARMOR = null;
            }
            PSTATUS = PrimaryStatus.normal;
  
       
            if (GetComponent<BuffScript>())
            {
                Destroy(GetComponent<BuffScript>());
            }
            if (GetComponent<DebuffScript>())
            {
                Destroy(GetComponent<DebuffScript>());
            }
        }
    }
}
