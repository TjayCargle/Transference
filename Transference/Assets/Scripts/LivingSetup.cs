using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingSetup : MonoBehaviour {

    [SerializeField]
    int[] startingSkills;
    [SerializeField]
    int[] startingWeapons;
    [SerializeField]
    int[] startingArmors;
    [SerializeField]
    int[] startingAccessories;
    bool isSetup = false;
    public virtual void Setup()
    {
        if(!isSetup)
        {
            DatabaseManager dm = GameObject.FindObjectOfType<DatabaseManager>();
            if (dm != null)
            {
                if (GetComponent<LivingObject>())
                {
                    for (int i = 0; i < startingSkills.Length; i++)
                    {
                        dm.LearnSkill(startingSkills[i], GetComponent<LivingObject>(),true);
                    }
                    for (int i = 0; i < startingWeapons.Length; i++)
                    {
                        dm.GetWeapon(startingWeapons[i], GetComponent<LivingObject>());
                    }
                    for (int i = 0; i < startingArmors.Length; i++)
                    {
                        dm.GetArmor(startingArmors[i], GetComponent<LivingObject>());
                    }
                    if (startingWeapons.Length > 0)
                    {
                        GetComponent<LivingObject>().WEAPON.Equip(GetComponent<InventoryScript>().WEAPONS[0]);
                    }
                    if (startingArmors.Length > 0)
                    {
                        GetComponent<LivingObject>().ARMOR.Equip(GetComponent<InventoryScript>().ARMOR[0]);
                    }
                }
            }
            isSetup = true;
        }
    }
    void Start () {
       if(GetComponent<LivingObject>())
        {
            if(!GetComponent<LivingObject>().isSetup)
            {
                GetComponent<LivingObject>().Setup();
            }
            Setup();
        }
       
	}
	
}
