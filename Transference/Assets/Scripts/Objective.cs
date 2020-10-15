using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : ScriptableObject
{
    private ObjectiveType myObjective;
    private int targetTile;
    private int targetEnemy;
    private int targetGlyph;
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
}
