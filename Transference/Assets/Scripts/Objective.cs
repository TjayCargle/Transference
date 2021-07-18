using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : ScriptableObject
{
    [SerializeField]
    private ObjectiveType myObjective;
    [SerializeField]
    private int targetTile;
    [SerializeField]
    private int targetEnemy;
    [SerializeField]
    private int targetGlyph;
    [SerializeField]
    private string displayString;

    public ObjectiveType TYPE
    {
        get { return myObjective; }
        set { myObjective = value; }
    }

    public int TILE
    {
        get { return targetTile; }
        set { targetTile = value; }
    }

    public int ENEMY
    {
        get { return targetEnemy; }
        set { targetEnemy = value; }
    }
    public int GLYPH
    {
        get { return targetGlyph; }
        set { targetGlyph = value; }
    }

    public string DISPLAY
    {
        get { return displayString; }
        set { displayString = value; }
    }

    public void CompleteObjective()
    {
        displayString = "Explore the mansion.";
        targetTile = -1;
        targetEnemy = -1;
        targetGlyph = -1;
        myObjective = ObjectiveType.none;
    }
    public void SetObjective(ObjectiveType newObjective, string display, int target = -1)
    {
        myObjective = newObjective;
        displayString = display;

        switch (myObjective)
        {
            case ObjectiveType.reachLocation:
                targetTile = target;
                break;
            case ObjectiveType.defeatSpecificEnemy:
                targetEnemy = target;
                break;
            case ObjectiveType.defeatSpecificGlyph:
                targetGlyph = target;
                break;
            case ObjectiveType.defeatAllFaction:
                targetEnemy = target;
                break;
        }
    }
}
