using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : ScriptableObject
{
    public int healthbars;
    public BossPhase currentPhase = BossPhase.inital;
    public BossPhase previousPhase = BossPhase.inital;
    public List<BossCommand> commands;
    public int HEALTHBARS
    {
        get { return healthbars; }
        set { healthbars = value; }
    }
    public BossPhase PHASE
    {
        get { return currentPhase; }
    }
    

   

    //private void SoftUnset()
    //{
    //    if (isSetup == true)
    //    {

    //        isSetup = false;
    //        DEAD = false;
    //        STATS.Reset(true);
    //        BASE_STATS.Reset();
    //        BASE_STATS.MAX_HEALTH += (int)((float)BASE_STATS.MAX_HEALTH * 0.5f);
    //        STATS.HEALTH = BASE_STATS.MAX_HEALTH;

 
    //        PSTATUS = PrimaryStatus.normal;

    //    }
    //}
}
