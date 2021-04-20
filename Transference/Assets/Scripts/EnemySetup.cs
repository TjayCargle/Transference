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
            manager = Common.GetManager();

            myself = GetComponent<EnemyScript>();
            if (!myself)
            {
                myself = gameObject.AddComponent<EnemyScript>();
            }
            isSetup = true;
            //myself.Setup();
            myself.FACTION = Faction.enemy;
            // Debug.Log(me.FullName + " is setting up");
            if (dm != null)
            {
                dm.GetEnemy(enemyId, myself);
              
            }

            if (!manager.gridObjects.Contains(myself))
            {
                manager.gridObjects.Add(myself);
                myself.currentTile = manager.GetTile(myself);
                if(myself.currentTile)
                {

                myself.currentTileIndex = myself.currentTile.listindex;
                if (myself.currentTile)
                    myself.currentTile.isOccupied = true;
                }
            }
            isSetup = true;
        }
  
    }


}
