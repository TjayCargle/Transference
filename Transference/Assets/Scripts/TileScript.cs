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
    Mesh mesh;
    [SerializeField]
    Vector3[] vertices;
    [SerializeField]
    Vector2[] uvs;
    [SerializeField]
    float uFloat;

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
            mesh = GetComponent<MeshFilter>().mesh;
            vertices = mesh.vertices;
            uvs = new Vector2[vertices.Length];
        }
        myColor = Color.black;
        lastU = uFloat;
        lastV = vFloat;
    }

    // Update is called once per frame
    void Update()
    {

        if (myRender)
        {
            myRender.material.color = myColor;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {

            mesh = GetComponent<MeshFilter>().mesh;
            vertices = mesh.vertices;
            mesh.uv = uvs;
            uvs = new Vector2[vertices.Length];

        }



    }

    public int CompareTo(object obj)
    {
        TileScript other = obj as TileScript;
        return listindex.CompareTo(other.listindex);
    }
}
