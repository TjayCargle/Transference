using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingSetup : MonoBehaviour
{

    [SerializeField]
    int[] startingSkills;
    [SerializeField]
    int[] startingWeapons;
    [SerializeField]
    int[] startingArmors;
    [SerializeField]
    int[] startingAccessories;
    bool isSetup = false;
    LivingObject me;
    DatabaseManager dm;
    ManagerScript manager;
    public virtual void Setup()
    {
        if (!isSetup)
        {
             dm = GameObject.FindObjectOfType<DatabaseManager>();
            if(!dm.isSetup)
            {
                dm.Setup();
            }
            manager = GameObject.FindObjectOfType<ManagerScript>();

            me = GetComponent<LivingObject>();
            if (me)
            {


                Debug.Log(me.FullName + " is setting up");
                if (dm != null)
                {
                    if (me)
                    {
                        
                        for (int i = 0; i < startingSkills.Length; i++)
                        {
                            dm.LearnSkill(startingSkills[i], me, true);
                        }
                        for (int i = 0; i < startingWeapons.Length; i++)
                        {
                            dm.GetWeapon(startingWeapons[i], me);
                        }
                        for (int i = 0; i < startingArmors.Length; i++)
                        {
                            dm.GetArmor(startingArmors[i], me);
                        }
                        if (startingWeapons.Length > 0)
                        {
                            me.WEAPON.Equip(GetComponent<InventoryScript>().WEAPONS[0]);
                        }
                        if (startingArmors.Length > 0)
                        {
                            me.ARMOR.Equip(GetComponent<InventoryScript>().ARMOR[0]);
                        }
                    }
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
            Setup();
        }

    }

}
