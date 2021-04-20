using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSetup: LivingSetup
{
    public int characterId;

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

            me = GetComponent<LivingObject>();
            if (!me)
            {
                me = gameObject.AddComponent<LivingObject>();
            }
            isSetup = true;
            me.Setup();
          
            // Debug.Log(me.FullName + " is setting up");
            if (dm != null)
            {
                dm.GetActor(characterId, me);
             
            }
            if (!manager.gridObjects.Contains(me))
            {
                manager.gridObjects.Add(me);
                me.currentTile = manager.GetTile(me);
                   if (me.currentTile)
                    me.currentTile.isOccupied = true;
            }
            isSetup = true;
        }
    }
    
}
