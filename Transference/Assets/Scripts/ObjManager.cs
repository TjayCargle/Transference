using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjManager : MonoBehaviour
{
    public List<GridObject> enemies;
    public GameObject enemyPrefab;
    public bool isSetup = false;
    public void Setup()
    {
        if (!isSetup)
        {
            enemies = new List<GridObject>();
            isSetup = true;
        }
    }
    void Start()
    {
        Setup();
    }

    public List<GridObject> getObjects(int num)
    {
        List<GridObject> subenemies = new List<GridObject>();
        if (num < enemies.Count)
        {
            for (int i = 0; i < num; i++)
            {
                LivingSetup enemySetup = enemies[i].GetComponent<LivingSetup>();
               // enemies[i].Unset();
                enemySetup.Unset();
                enemySetup.indexId = Random.Range(0, 2);
                enemySetup.Setup();
                subenemies.Add(enemies[i]);
            }
        }
        else
        {
            for (int i = 0; i < enemies.Count; i++)
            {

                LivingSetup enemySetup = enemies[i].GetComponent<LivingSetup>();
              //  enemies[i].Unset();
                enemySetup.Unset();
                enemySetup.indexId = Random.Range(0, 2);
                enemySetup.Setup();
                subenemies.Add(enemies[i]);
            }
            while (enemies.Count < num)
            {
                GameObject temp = Instantiate(enemyPrefab, Vector2.zero, Quaternion.identity);
                LivingSetup enemySetup = temp.GetComponent<LivingSetup>();
                enemySetup.indexId = Random.Range(0, 2);
                GridObject enemy = temp.GetComponent<GridObject>();
                enemy.Setup();
                if (!enemy.BASE_STATS)
                    enemy.BASE_STATS = enemy.GetComponent<BaseStats>();
                if (!enemy.STATS)
                    enemy.STATS = enemy.GetComponent<ModifiedStats>();
                enemies.Add(enemy);
                subenemies.Add(enemy);

            }
        }
        return subenemies;
    }
}
