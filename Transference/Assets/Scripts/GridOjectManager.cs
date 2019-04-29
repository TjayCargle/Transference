using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOjectManager : MonoBehaviour
{
    public List<GridObject> objects;
    public GameObject objectPrefab;
    public bool isSetup = false;
    private int skilloratk = -1;
    public void Setup()
    {
        if (!isSetup)
        {
            objects = new List<GridObject>();
            isSetup = true;
        }
    }

    void Start()
    {
        Setup();
    }

    public List<GridObject> GetObjects(int num)
    {
        List<GridObject> subhazards = new List<GridObject>();
        if (num < objects.Count)
        {
            for (int i = 0; i < num; i++)
            {
             //   HazardSetup hazardSetup = hazards[i].GetComponent<HazardSetup>();
             
              //  skilloratk = 1;// Random.Range(0, 2);
             
              //  hazardSetup.Setup();
                subhazards.Add(objects[i]);
            }
        }
        else
        {
            for (int i = 0; i < objects.Count; i++)
            {

             
            }
            while (objects.Count < num)
            {
                GameObject temp = Instantiate(objectPrefab, Vector2.zero, Quaternion.identity);
                HazardSetup hazardSetup = temp.GetComponent<HazardSetup>();
                hazardSetup.hazardid = 0;
                GridObject hazard = temp.AddComponent<GridObject>();
           
             
                hazard.Setup();
                objects.Add(hazard);
                subhazards.Add(hazard);

            }
        }
        return subhazards;
    }
}
