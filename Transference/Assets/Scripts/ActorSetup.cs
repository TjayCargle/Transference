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
            manager = GameObject.FindObjectOfType<ManagerScript>();

            me = GetComponent<LivingObject>();
            if (!me)
            {
                me = gameObject.AddComponent<LivingObject>();
            }
           // me.Setup();
          
            // Debug.Log(me.FullName + " is setting up");
            if (dm != null)
            {
                dm.GetActor(characterId, me);
                if (gameObject.GetComponent<AnimationScript>())
                {
                    gameObject.GetComponent<AnimationScript>().Setup();
                }
            }

            isSetup = true;
        }
    }
    
}
