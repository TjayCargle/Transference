using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

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

    public List<EnemyScript> getEnemies(int num)
    {
        List<EnemyScript> subenemies = new List<EnemyScript>();
        if (num < enemies.Count)
        {
            for (int i = 0; i < num; i++)
            {
                EnemySetup enemySetup = enemies[i].GetComponent<EnemySetup>();
                enemies[i].Unset();
                enemySetup.Unset();
                enemySetup.enemyId = Random.Range(0, 2);
                enemySetup.Setup();
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
                enemySetup.enemyId = Random.Range(0, 2);
                enemySetup.Setup();
                subenemies.Add(enemies[i]);
            }
            while (enemies.Count < num)
            {
                GameObject temp = Instantiate(enemyPrefab, Vector2.zero, Quaternion.identity);
                EnemySetup enemySetup = temp.GetComponent<EnemySetup>();
                enemySetup.enemyId = Random.Range(0, 2);
                EnemyScript enemy = temp.AddComponent<EnemyScript>();
                enemy.Setup();
                enemies.Add(enemy);
                subenemies.Add(enemy);
        
            }
        }
        return subenemies;
    }
}
