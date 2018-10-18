using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySetup : LivingSetup
{
    public int enemyId;
     EnemyScript myself;


    public override void Setup()
    {
        if (!isSetup)
        {
            dm = GameObject.FindObjectOfType<DatabaseManager>();
            if (!dm.isSetup)
            {
                dm.Setup();
            }
            manager = GameObject.FindObjectOfType<ManagerScript>();

            myself = GetComponent<EnemyScript>();
            if (!myself)
            {
                myself = gameObject.AddComponent<EnemyScript>();
            }
            // me.Setup();
            myself.FACTION = Faction.enemy;
            // Debug.Log(me.FullName + " is setting up");
            if (dm != null)
            {
                dm.GetEnemy(enemyId, myself);
                if (gameObject.GetComponent<AnimationScript>())
                {
                    gameObject.GetComponent<AnimationScript>().Setup();
                }
            }

            isSetup = true;
        }
    }
   

}
