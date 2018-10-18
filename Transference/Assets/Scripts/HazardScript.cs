using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardScript : LivingObject
{

    BaseStats myStats;
    private bool destructible = true;
    public int Dropskill = -1;
    [SerializeField]
    public bool dropsSkill = false;
    public bool DESTRUCTIBLE
    {
        get { return destructible; }
        set { destructible = value; }
    }


    public override void Setup()
    {
        if (!isSetup)
        {
           
            base.Setup();
            STATS.MANA = 0;
            STATS.MAX_MANA = 0;
            STATS.MAX_FATIGUE = 0;
            FACTION = Faction.hazard;
          //  myStats = GetComponent<BaseStats>();
           // myStats.HEALTH = myStats.MAX_HEALTH;
            //FACTION = faction.hazard;
        }

    }
    public override void Die()
    {
        base.Die();

    }


    public override IEnumerator FadeOut()
    {
        Debug.Log("hazard dying");
        if (GetComponent<SpriteRenderer>())
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Color subtract = new Color(0, 0, 0, 0.1f);
            int num = 0;
            while (renderer.color.a > 0)
            {
                num++;
                if (num > 9999)
                {
                    Debug.Log("time expired");
                    break;
                }
                renderer.color = renderer.color - subtract;
                yield return null;
            }
            isdoneDying = true;

            myManager.gridObjects.Remove(this);
            gameObject.SetActive(false);
            currentTile.isOccupied = false;
           
         //   Destroy(gameObject);
        }
    }

}
