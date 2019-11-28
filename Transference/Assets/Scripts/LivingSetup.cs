using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingSetup : MonoBehaviour
{

    //[SerializeField]
    //protected int[] startingSkills;
    //[SerializeField]
    //protected int[] startingWeapons;
    //[SerializeField]
    //protected int[] startingArmors;
    //[SerializeField]
    //int[] startingAccessories;
    //[SerializeField]
    //int[] startingItems;
    public int indexId = 0;
    public bool isSetup = false;
    public LivingObject me;
    public GridObject otherMe;
    public DatabaseManager dm;
    public ManagerScript manager;
    public Faction DataFaction;
    public virtual void Setup()
    {
        if (!isSetup)
        {
            dm = Common.GetDatabase();
            if (!dm.isSetup)
            {
                dm.Setup();
            }
            manager = GameObject.FindObjectOfType<ManagerScript>();

            me = GetComponent<LivingObject>();
            otherMe = GetComponent<GridObject>();
            if(me)
            {
            me = dm.GetLiving(indexId, me);

            }
            else if (otherMe)
            {
                otherMe = dm.GetObject(indexId, otherMe);
            }
            if (me)
            {


                // Debug.Log(me.FullName + " is setting up");
                if (dm != null)
                {
                    //if (me)
                    //{

                    //    for (int i = 0; i < startingSkills.Length; i++)
                    //    {
                    //        dm.LearnSkill(startingSkills[i], me, true);
                    //    }
                    //    for (int i = 0; i < startingWeapons.Length; i++)
                    //    {
                    //        dm.GetWeapon(startingWeapons[i], me);
                    //    }
                    //    for (int i = 0; i < startingArmors.Length; i++)
                    //    {
                    //        dm.GetArmor(startingArmors[i], me);
                    //    }

                    //    for (int i = 0; i < startingItems.Length; i++)
                    //    {
                    //        dm.GetItem(startingItems[i], me);
                    //    }

                    //}
                }
            }
            isSetup = true;
        }

    }
    void Start()
    {
        if (GetComponent<LivingObject>())
        {
            if (!GetComponent<LivingObject>().isSetup)
            {
                GetComponent<LivingObject>().Setup();
            }
        }

        Setup();
    }
    public void Unset()
    {
        isSetup = false;
    }
}
