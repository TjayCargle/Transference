using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{

    public List<HazardScript> hazards;
    public GameObject hazardPrefab;
    public bool isSetup = false;
    private int skilloratk = -1;
    public void Setup()
    {
        if (!isSetup)
        {
            hazards = new List<HazardScript>();
            isSetup = true;
        }
    }
    void Start()
    {
        Setup();
    }

    public List<HazardScript> getHazards(int num, int specialId = 0)
    {
        Debug.Log("finding " + specialId);
        List<HazardScript> subhazards = new List<HazardScript>();
        if (num < hazards.Count)
        {
        Debug.Log("1"); 
            for (int i = 0; i < num; i++)
            {
        Debug.Log("2"); 
                HazardSetup hazardSetup = hazards[i].GetComponent<HazardSetup>();
                hazards[i].Unset();
                hazardSetup.Unset();

                hazardSetup.hazardid = specialId;
                skilloratk = 1;// Random.Range(0, 2);
                if (skilloratk == 0)
                {
                     
                    hazards[i].REWARD = (Random.Range(1, 7) * 10) + Random.Range(0, 5);
                }
                else
                {
 
                    hazards[i].REWARD = (Random.Range(1, 7) * 10) + Random.Range(0, 5);
                }
                hazards[i].Setup();
                subhazards.Add(hazards[i]);
            }
        }
        else
        {
        Debug.Log("3"); 
            for (int i = 0; i < hazards.Count; i++)
            {
        Debug.Log("4"); 

                HazardSetup hazardSetup = hazards[i].GetComponent<HazardSetup>();
                hazards[i].Unset();
                hazardSetup.Unset();
                hazardSetup.hazardid = specialId; skilloratk = Random.Range(0, 2);
                if (skilloratk == 0)
                {
                     
                    hazards[i].REWARD = (Random.Range(1, 7) * 10) + Random.Range(0, 5);
                }
                else
                {
 
                    hazards[i].REWARD = (Random.Range(1, 7) * 10) + Random.Range(0, 5);
                }
                hazards[i].Setup();
                subhazards.Add(hazards[i]);
            }
            while (hazards.Count < num)
            {
        Debug.Log("5"); 
                GameObject temp = Instantiate(hazardPrefab, Vector2.zero, Quaternion.identity);
                HazardSetup hazardSetup = temp.GetComponent<HazardSetup>();
                hazardSetup.hazardid = specialId;
                HazardScript hazard = temp.AddComponent<HazardScript>();
                skilloratk = 1;// Random.Range(0, 2);
                if (skilloratk == 0)
                {
                     
                    hazard.REWARD = (Random.Range(1, 7) * 10) + Random.Range(5, 8);
                }
                else
                {
                     
                    hazard.REWARD = (Random.Range(1, 7) * 10) + Random.Range(5, 8);
                }
                hazard.Setup();
                hazards.Add(hazard);
                subhazards.Add(hazard);

            }
        }
        for (int i = 0; i < subhazards.Count; i++)
        {
        Debug.Log("6"); 
            hazards.Remove((hazards[0]));
        }
        return subhazards;
    }

    public List<HazardScript> getHazards(MapData data)
    {
        int num = data.glyphIndexes.Count;
        List<HazardScript> subhazards = new List<HazardScript>();
        if (num < hazards.Count)
        {
            for (int i = 0; i < num; i++)
            {
                HazardSetup hazardSetup = hazards[i].GetComponent<HazardSetup>();
                hazards[i].Unset();
                hazardSetup.Unset();
                if (data.glyphIds.Count > 0)
                    hazardSetup.hazardid = data.glyphIds[i];
                else
                    hazardSetup.hazardid = 0; 
                skilloratk = 1;// Random.Range(0, 2);
                if (skilloratk == 0)
                {
                     
                    hazards[i].REWARD = (Random.Range(1, 7) * 10) + Random.Range(0, 5);
                }
                else
                {
 
                    hazards[i].REWARD = (Random.Range(1, 7) * 10) + Random.Range(0, 5);
                }
                hazards[i].Setup();
                subhazards.Add(hazards[i]);
            }
        }
        else
        {
            int indx = 0;
            for (int i = 0; i < hazards.Count; i++)
            {

                HazardSetup hazardSetup = hazards[i].GetComponent<HazardSetup>();
                hazards[i].Unset();
                hazardSetup.Unset();
                if (data.glyphIds.Count > 0)
                    hazardSetup.hazardid = data.glyphIds[i];
                else
                    hazardSetup.hazardid = 0;
         
                     
                    hazards[i].REWARD = (Random.Range(1, 7) * 10) + Random.Range(0, 5);
                
        
                hazards[i].Setup();
                subhazards.Add(hazards[i]);
                indx++;
            }
        
            while (hazards.Count < num)
            {
                indx = hazards.Count;
                GameObject temp = Instantiate(hazardPrefab, Vector2.zero, Quaternion.identity);
                HazardSetup hazardSetup = temp.GetComponent<HazardSetup>();
                if (data.glyphIds.Count > 0)
                    hazardSetup.hazardid = data.glyphIds[indx];
                else
                    hazardSetup.hazardid = 0;
                HazardScript hazard = temp.AddComponent<HazardScript>();
                skilloratk = 1;// Random.Range(0, 2);
                if (skilloratk == 0)
                {
                     
                    hazard.REWARD = (Random.Range(1, 7) * 10) + Random.Range(5, 8);
                }
                else
                {
                     
                    hazard.REWARD = (Random.Range(1, 7) * 10) + Random.Range(5, 8);
                }
                hazard.Setup();
                hazards.Add(hazard);
                subhazards.Add(hazard);
                indx++;
            }
        }
        return subhazards;
    }
}
