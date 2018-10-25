using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardSetup : LivingSetup
{
    public int hazardid;
    HazardScript myself;

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

            myself = GetComponent<HazardScript>();
            if (!myself)
            {
                myself = gameObject.AddComponent<HazardScript>();
            }
            isSetup = true;
            myself.Setup();
            myself.FACTION = Faction.enemy;
            // Debug.Log(me.FullName + " is setting up");
            if (dm != null)
            {
                 dm.GetHazard(hazardid, myself);
   
           
                    dm.GetArmor(8, myself);
                

            }
            myself.FACTION = Faction.hazard;
            if (!manager.gridObjects.Contains(myself))
            {
                manager.gridObjects.Add(myself);
                myself.currentTile = manager.GetTile(myself);
                if (myself.currentTile)
                    myself.currentTile.isOccupied = true;
            }
            myself.BASE_STATS.MAX_HEALTH = 100 +  (int)(myself.BASE_STATS.LEVEL * 12.5f);
            myself.BASE_STATS.HEALTH = myself.BASE_STATS.MAX_HEALTH;
            if (myself.INVENTORY.CSKILLS.Count > 0)
            {
                CommandSkill mySkill = myself.INVENTORY.CSKILLS[0];
                myself.dropsSkill = true;
                myself.ARMOR.HITLIST[(int)mySkill.ELEMENT] = EHitType.reflects;
                int amnt = Common.GetDmgIndex(mySkill.DAMAGE) * 10;
                if (mySkill.ETYPE == EType.magical)
                {
                    myself.STATS.MAGIC = 2 * amnt;
                    myself.STATS.RESIESTANCE = 2 * amnt;
                    myself.STATS.STRENGTH = amnt;
                    myself.STATS.DEFENSE =  amnt;
                }
                else
                {
                    myself.STATS.STRENGTH = 2 * amnt;
                    myself.STATS.DEFENSE = 2 * amnt;
                    myself.STATS.MAGIC = amnt;
                    myself.STATS.RESIESTANCE = amnt;
                }
            }
            else
            {
                if (myself.INVENTORY.WEAPONS.Count > 0)
                {
                    myself.dropsSkill = false;
                    myself.ARMOR.HITLIST[(int)myself.WEAPON.AFINITY] = EHitType.reflects;
                    WeaponEquip weapon = myself.WEAPON;
                    int amnt = (1 +(weapon.ATTACK/10) )* 5;
                    if (weapon.ATTACK_TYPE == EType.magical)
                    {
                        myself.BASE_STATS.MAGIC = 2 * amnt;
                        myself.BASE_STATS.RESIESTANCE = 2 * amnt;
                        myself.BASE_STATS.STRENGTH = amnt;
                        myself.BASE_STATS.DEFENSE = amnt;
                    }
                    else
                    {
                        myself.BASE_STATS.STRENGTH = 2 * amnt;
                        myself.BASE_STATS.DEFENSE = 2 * amnt;
                        myself.BASE_STATS.MAGIC = amnt;
                        myself.BASE_STATS.RESIESTANCE = amnt;
                    }
                }
            }

            isSetup = true;
        }
    }

}
