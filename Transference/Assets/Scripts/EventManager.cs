using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : EventRunner
{

    // public delegate bool RunEvent();
    public List<GridEvent> gridEvents;
    public int activeEvents = 0;
    public bool isSetup = false;
    // public List<string> completed;
    public void Setup()
    {
        if (!isSetup)
        {
            gridEvents = new List<GridEvent>();
            isSetup = true;
        }
    }
    public void Start()
    {
        Setup();
        // completed = new List<string>();
    }

    private void Update()
    {
        activeEvents = gridEvents.Count;
        if (gridEvents.Count > 0)
        {
            GridEvent eve = gridEvents[0];
            if (eve.isRunning == false)
            {
                eve.isRunning = true;

            }
            if (eve.RUNABLE(eve.data) == true)
            {
                // completed.Add(eve.name);
                eve.isRunning = false;
                gridEvents.Remove(eve);
            }
        }
    }
}
