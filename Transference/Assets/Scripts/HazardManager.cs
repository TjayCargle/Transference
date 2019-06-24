using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour {

    public List<HazardScript> hazards;
    public GameObject hazardPrefab;
    public bool isSetup = false;
    private int  skilloratk = -1;
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

    public List<HazardScript> getHazards(int num)
    {
        List<HazardScript> subhazards = new List<HazardScript>();
        if (num < hazards.Count)
        {
            for (int i = 0; i < num; i++)
            {
                HazardSetup hazardSetup = hazards[i].GetComponent<HazardSetup>();
                hazards[i].Unset();
                hazardSetup.Unset();
                hazardSetup.hazardid = 0;
                skilloratk = 1;// Random.Range(0, 2);
                if(skilloratk == 0)
                {
                    hazards[i].dropsSkill = false;
                    hazards[i].REWARD = Random.Range(0, 11);
                }
                else
                {
                    hazards[i].dropsSkill = true;
                    hazards[i].REWARD = Random.Range(0, 11);
                }
                hazards[i].Setup();
                subhazards.Add(hazards[i]);
            }
        }
        else
        {
            for (int i = 0; i < hazards.Count; i++)
            {

                HazardSetup hazardSetup = hazards[i].GetComponent<HazardSetup>();
                hazards[i].Unset();
                hazardSetup.Unset();
                hazardSetup.hazardid = 0; skilloratk = Random.Range(0, 2);
                if (skilloratk == 0)
                {
                    hazards[i].dropsSkill = false;
                    hazards[i].REWARD = Random.Range(0, 11);
                }
                else
                {
                    hazards[i].dropsSkill = true;
                    hazards[i].REWARD = Random.Range(0, 11);
                }
                hazards[i].Setup();
                subhazards.Add(hazards[i]);
            }
            while (hazards.Count < num)
            {
                GameObject temp = Instantiate(hazardPrefab, Vector2.zero, Quaternion.identity);
                HazardSetup hazardSetup = temp.GetComponent<HazardSetup>();
                hazardSetup.hazardid = 0;
                HazardScript hazard = temp.AddComponent<HazardScript>();
                skilloratk = 1;// Random.Range(0, 2);
                if (skilloratk == 0)
                {
                    hazard.dropsSkill = false;
                    hazard.REWARD = (Random.Range(1, 7) * 10) + Random.Range(0,5);
                }
                else
                {
                    hazard.dropsSkill = true;
                    hazard.REWARD = (Random.Range(1, 7) * 10) + Random.Range(0, 5);
                }
                hazard.Setup();
                hazards.Add(hazard);
                subhazards.Add(hazard);

            }
        }
        return subhazards;
    }
}
