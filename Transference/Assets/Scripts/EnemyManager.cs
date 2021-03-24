using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public List<EnemyScript> enemies;
    public GameObject enemyPrefab;
    public bool isSetup = false;
    public void Setup()
    {
        if (!isSetup)
        {
            enemies = new List<EnemyScript>();
            isSetup = true;
        }
    }
    void Start()
    {
        Setup();
    }

    public List<EnemyScript> getEnemies(int num, int specialId = -1)
    {

        List<EnemyScript> subenemies = new List<EnemyScript>();
        if (num < enemies.Count)
        {
            for (int i = 0; i < num; i++)
            {
                EnemySetup enemySetup = enemies[i].GetComponent<EnemySetup>();
                enemies[i].Unset();
                enemySetup.Unset();
                if (specialId == -1)
                    specialId = Random.Range(0, 6);
                enemySetup.enemyId = specialId;
                enemies[i].Setup();
                //if(enemySetup.enemyId == 2)
                //{
                //    enemies[i].FACTION = Faction.fairy;
                //}
                //else
                //{
                //    enemies[i].FACTION = Faction.enemy;
                //}
                subenemies.Add(enemies[i]);
            }
        }
        else
        {
            for (int i = 0; i < enemies.Count; i++)
            {

                EnemySetup enemySetup = enemies[i].GetComponent<EnemySetup>();
                enemies[i].Unset();
                enemySetup.Unset();

                if (specialId == -1)
                    specialId = Random.Range(0, 6);
                enemySetup.enemyId = specialId;
                enemies[i].Setup();

                subenemies.Add(enemies[i]);
            }
            while (enemies.Count < num)
            {
                GameObject temp = Instantiate(enemyPrefab, Vector2.zero, Quaternion.identity);
                EnemySetup enemySetup = temp.GetComponent<EnemySetup>();
                enemySetup.Unset();

                if (specialId == -1)
                    specialId = Random.Range(0, 6);
                enemySetup.enemyId = specialId;
                EnemyScript enemy = temp.AddComponent<EnemyScript>();
                enemy.Setup();
                if (enemySetup.enemyId == 2)
                {
                    enemy.FACTION = Faction.fairy;
                }
                else
                {
                    enemy.FACTION = Faction.enemy;
                }
                enemies.Add(enemy);
                subenemies.Add(enemy);

            }
        }
        for (int i = 0; i < subenemies.Count; i++)
        {
            enemies.Remove((enemies[0]));
        }
        return subenemies;
    }

    public List<EnemyScript> getEnemies(MapData data)
    {
        int num = data.enemyIndexes.Count;
        List<EnemyScript> subenemies = new List<EnemyScript>();
        if (num < enemies.Count)
        {
            for (int i = 0; i < num; i++)
            {
                EnemySetup enemySetup = enemies[i].GetComponent<EnemySetup>();
                enemies[i].Unset();
                enemySetup.Unset();
                // Debug.Log("4");

                if (i < data.EnemyIds.Count)
                {
                    enemySetup.enemyId = data.EnemyIds[i];
                }
                else
                {
                    enemySetup.enemyId = Random.Range(0, 6);
                }
                enemies[i].Setup();
                if (enemySetup.enemyId >= 100)
                {
                    enemies[i].FACTION = Faction.fairy;
                    Common.UpdateBossProfile(enemySetup.enemyId, enemies[i]);
                }
                subenemies.Add(enemies[i]);
            }
        }
        else
        {
            for (int i = 0; i < enemies.Count; i++)
            {

                EnemySetup enemySetup = enemies[i].GetComponent<EnemySetup>();
                enemies[i].Unset();
                enemySetup.Unset();


                if (i < data.EnemyIds.Count)
                {
                    enemySetup.enemyId = data.EnemyIds[i];
                }
                else
                {
                    enemySetup.enemyId = Random.Range(0, 6);
                }

                enemies[i].Setup();
                if (enemySetup.enemyId >= 100)
                {
                    enemies[i].FACTION = Faction.fairy;
                    Common.UpdateBossProfile(enemySetup.enemyId, enemies[i]);
                }
                subenemies.Add(enemies[i]);
            }
            while (enemies.Count < num)
            {
                int indx = enemies.Count;
                GameObject temp = Instantiate(enemyPrefab, Vector2.zero, Quaternion.identity);
                EnemySetup enemySetup = temp.GetComponent<EnemySetup>();
                enemySetup.Unset();

                if (indx < data.EnemyIds.Count)
                {
                    enemySetup.enemyId = data.EnemyIds[indx];

                }
                else
                {
                    enemySetup.enemyId = Random.Range(0, 6);
                }
                EnemyScript enemy = temp.AddComponent<EnemyScript>();
                enemy.Setup();
                //enemy.FACTION = Faction.fairy;
                if (enemySetup.enemyId >= 100)
                {
                    Common.UpdateBossProfile(enemySetup.enemyId, enemy);
                }
                enemies.Add(enemy);
                subenemies.Add(enemy);

            }
        }
        return subenemies;
    }

}
