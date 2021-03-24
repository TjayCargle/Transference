using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour, IComparable
{
    private Color myColor;
    private Color previousColor;
    public bool isOccupied;
    public bool canBeOccupied = true;
    MeshRenderer myRender;
    public int listindex = -1;
    public bool isInShadow = false;
    [SerializeField]
    private TileType myType;
    [SerializeField]
    Material mat;
    [SerializeField]
    MeshFilter filter;
    [SerializeField]
    Mesh mesh;
    [SerializeField]
    Vector3[] vertices;
    [SerializeField]
    Vector2[] uvs;
    //[SerializeField]
    //float uFloat;

    [SerializeField]
    float vFloat;

    [SerializeField]
    Texture texture;
    // Use this for initialization
    float lastU = -1f;
    float lastV = -1f;
    public bool isMarked = false;

    public TileType TTYPE
    {
        get { return myType; }
        set { myType = value; }
    }

    public Color MYCOLOR
    {
        get { return myColor; }
        set
        {
            if (value == Color.white)
            {
                if (isMarked == false)
                {
                    previousColor = myColor;
                    myColor = value;
                    if (isInShadow)
                    {
                        if(value != Common.orange)
                        {
                        myColor = value * 0.25f;
                        myColor.a = 1;
                        }
                        else
                        {
                            myColor = value * 0.75f;
                            myColor.a = 1;
                        }
                    }
                }
               
            }
            else
            {
                if (isMarked == false)
                {
                    previousColor = myColor;
                    myColor = value;
                    if (isInShadow)
                    {
                        if (value != Common.orange)
                        {
                            myColor = value * 0.55f;
                            myColor.a = 1;
                        }
                        else
                        {
                            myColor = value * 0.75f;
                            myColor.a = 1;
                        }
                    }
                }
             
            }

        }
    }

    public Color PREVCOLOR
    {
        get { return previousColor; }
    }

    public void Setup()
    {
        if (GetComponent<MeshRenderer>())
        {
            myRender = GetComponent<MeshRenderer>();
            mat = myRender.material;
            texture = mat.mainTexture;
        }
        MYCOLOR = Color.black;

        if (GetComponent<MeshFilter>())
        {

            filter = GetComponent<MeshFilter>();
            mesh = filter.mesh;
            uvs = mesh.uv;

        }

    }

    // Update is called once per frame
    void Update()
    {

        if (myRender)
        {
            myRender.material.color = MYCOLOR;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {

            mesh.uv = uvs;

        }


    }
    [SerializeField]
    private string mapNmae;
    [SerializeField]
    private int roomIndex;
    [SerializeField]
    private int startIndex;
    [SerializeField]
    private string extraInfo;

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

    public int START
    {
        get { return startIndex; }
        set { startIndex = value; }
    }
    public string EXTRA
    {
        get { return extraInfo; }
        set { extraInfo = value; }
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
        START = -1;
    }
    public int CompareTo(object obj)
    {
        TileScript other = obj as TileScript;
        return listindex.CompareTo(other.listindex);
    }

    public void setTexture(Texture t)
    {
        //texture = t;
        //mat.mainTexture = texture;
    }
    public void setUVs(float startX, float finaleX, float startY, float finaleY)
    {
        return;
        if (uvs.Length != 4)
        {
            uvs = new Vector2[4];
        }
        uvs[0].x = startX;
        uvs[0].y = startY;

        uvs[1].x = finaleX;
        uvs[1].y = finaleY;

        uvs[2].x = finaleX;
        uvs[2].y = startY;

        uvs[3].x = startX;
        uvs[3].y = finaleY;

        mesh.uv = uvs;
    }


}
