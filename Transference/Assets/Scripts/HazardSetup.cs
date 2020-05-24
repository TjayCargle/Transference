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
            myself.FACTION = Faction.hazard;
            // Debug.Log(me.FullName + " is setting up");
            if (dm != null)
            {
                dm.GetHazard(hazardid, myself);


                dm.GetArmor(200, myself);

                dm.LearnSkill(223, myself);
            }
            // myself.FACTION = Faction.hazard;

            if (!manager.gridObjects.Contains(myself))
            {
                manager.gridObjects.Add(myself);
                myself.currentTile = manager.GetTile(myself);
                if (myself.currentTile)
                {
                    myself.currentTileIndex = myself.currentTile.listindex;

                    myself.currentTile.isOccupied = true;
                }
            }
            myself.BASE_STATS.MAX_HEALTH = 20 + (int)(myself.BASE_STATS.LEVEL * 12f);
            myself.STATS.HEALTH = myself.BASE_STATS.MAX_HEALTH;
            myself.generateSequence();
            myself.BASE_STATS.MAGIC = 2;
            myself.BASE_STATS.RESIESTANCE = 2;
            myself.BASE_STATS.STRENGTH = 2;
            myself.BASE_STATS.DEFENSE = 2;
            myself.BASE_STATS.SPEED = 4;
            myself.BASE_STATS.DEX = 2;
            CommandSkill mySkill = myself.INVENTORY.CSKILLS[0] as CommandSkill;
            int reflectnum = (int)mySkill.ELEMENT;
            int weaknum = 7 - reflectnum;
            for (int i = 0; i < myself.ARMOR.HITLIST.Count; i++)
            {
                if (i == reflectnum)
                {
                    myself.ARMOR.HITLIST[i] = EHitType.reflects;
                }
                else if (i == weaknum)
                {
                    myself.ARMOR.HITLIST[i] = EHitType.weak;
                }
                else
                {
                    int choice = Random.Range(0, 3);
                    if (choice == 0)
                    {
                        myself.ARMOR.HITLIST[i] = EHitType.resists;
                    }
                    else if (choice == 1)
                    {
                        myself.ARMOR.HITLIST[i] = EHitType.normal;
                    }
                    else
                    {

                        myself.ARMOR.HITLIST[i] = EHitType.nulls;
                    }
                }
            }
            //if (myself.INVENTORY.CSKILLS.Count > 0)
            //{
            //    CommandSkill mySkill = myself.INVENTORY.CSKILLS[0] as CommandSkill;




            //    int amnt = Common.GetDmgIndex(mySkill.DAMAGE) * 2;
            //    if (mySkill.ETYPE == EType.magical)
            //    {
            //        myself.BASE_STATS.MAGIC = (2 * Random.Range(1, amnt)) + Random.Range(0, myself.BASE_STATS.LEVEL);
            //        myself.BASE_STATS.RESIESTANCE = (2 * Random.Range(1, amnt)) + Random.Range(0, myself.BASE_STATS.LEVEL);
            //        myself.BASE_STATS.STRENGTH = Random.Range(1, amnt * 2);
            //        myself.BASE_STATS.DEFENSE = Random.Range(1, amnt * 2);
            //        myself.BASE_STATS.SPEED = Random.Range(1, amnt * 2);
            //        myself.BASE_STATS.DEX = Random.Range(1, amnt * 2);
            //    }
            //    else
            //    {
            //        myself.BASE_STATS.STRENGTH = (2 * Random.Range(1, amnt)) + Random.Range(0, myself.BASE_STATS.LEVEL);
            //        myself.BASE_STATS.DEFENSE = (2 * Random.Range(1, amnt)) + Random.Range(0, myself.BASE_STATS.LEVEL);
            //        myself.BASE_STATS.MAGIC = Random.Range(1, amnt * 2);
            //        myself.BASE_STATS.RESIESTANCE = Random.Range(1, amnt * 2);
            //        myself.BASE_STATS.SPEED = Random.Range(1, amnt * 2);
            //        myself.BASE_STATS.DEX = Random.Range(1, amnt * 2);
            //    }
            //}
            //else
            //{
            //    if (myself.INVENTORY.WEAPONS.Count > 0)
            //    {



            //        int reflectnum = (int)myself.WEAPON.ELEMENT;
            //        int weaknum = 7 - reflectnum;
            //        for (int i = 0; i < myself.ARMOR.HITLIST.Count; i++)
            //        {
            //            if (i == reflectnum)
            //            {
            //                myself.ARMOR.HITLIST[i] = EHitType.reflects;
            //            }
            //            else if (i == weaknum)
            //            {
            //                myself.ARMOR.HITLIST[i] = EHitType.weak;
            //            }
            //            else
            //            {
            //                int choice = Random.Range(0, 1);
            //                if (choice == 0)
            //                {
            //                    myself.ARMOR.HITLIST[i] = EHitType.resists;
            //                }
            //                else
            //                    myself.ARMOR.HITLIST[i] = EHitType.nulls;
            //            }
            //        }
            //        WeaponEquip weapon = myself.WEAPON;
            //        int amnt = (1 + (weapon.ATTACK)) * 5;
            //        //   if (weapon.ATTACK_TYPE == EType.magical)
            //        {
            //            myself.BASE_STATS.MAGIC = (2 * Random.Range(1, amnt)) + Random.Range(0, myself.BASE_STATS.LEVEL);
            //            myself.BASE_STATS.RESIESTANCE = (2 * Random.Range(1, amnt)) + Random.Range(0, myself.BASE_STATS.LEVEL);
            //            myself.BASE_STATS.STRENGTH = (2 * Random.Range(1, amnt)) + Random.Range(0, myself.BASE_STATS.LEVEL);
            //            myself.BASE_STATS.DEFENSE = (2 * Random.Range(1, amnt)) + Random.Range(0, myself.BASE_STATS.LEVEL);
            //        }
            //        //else
            //        //{
            //        //    myself.BASE_STATS.STRENGTH = (2 * Random.Range(1, amnt)) + Random.Range(0, myself.BASE_STATS.LEVEL);
            //        //    myself.BASE_STATS.DEFENSE = (2 * Random.Range(1, amnt)) + Random.Range(0, myself.BASE_STATS.LEVEL);
            //        //    myself.BASE_STATS.MAGIC = Random.Range(1, amnt * 2 );
            //        //    myself.BASE_STATS.RESIESTANCE = Random.Range(1, amnt * 2);
            //        //}
            //    }
            //}
          //  myself.updateLastSprites();
            isSetup = true;
        }
    }

}
