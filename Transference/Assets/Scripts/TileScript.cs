using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour, IComparable
{
    public Color myColor;
    public bool isOccupied;
    public bool canBeOccupied = true;
    MeshRenderer myRender;
    public int listindex = -1;
    [SerializeField]
    Material mat;
   // [SerializeField]
  // Mesh mesh;
    //[SerializeField]
    //Vector3[] vertices;
    //[SerializeField]
    //Vector2[] uvs;
    //[SerializeField]
    //float uFloat;

    [SerializeField]
    float vFloat;
    // Use this for initialization
    float lastU = -1f;
    float lastV = -1f;
    public void Setup()
    {
        if (GetComponent<MeshRenderer>())
        {
            myRender = GetComponent<MeshRenderer>();
            mat = myRender.material;
        }
        myColor = Color.black;

    
    }

    // Update is called once per frame
    void Update()
    {

        if (myRender)
        {
            myRender.material.color = myColor;
        }



    }
    [SerializeField]
    private string mapNmae;
    [SerializeField]
    private int roomIndex;
    public string MAP
    {
        get { return mapNmae; }
        set { mapNmae = value; }
    }

    public int ROOM
    {
        get { return roomIndex; }
        set { roomIndex = value; }
    }
    public Material MAT
    {
        get { return mat; }
        set { mat = value; }
    }
    public void BreakRooms()
    {
        MAP = "";
        ROOM = -1;
    }
    public int CompareTo(object obj)
    {
        TileScript other = obj as TileScript;
        return listindex.CompareTo(other.listindex);
    }
}
