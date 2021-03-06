﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelCreationManager : MonoBehaviour
{
    public TextMeshProUGUI mapName;
    public TMP_InputField mapWidthField;
    public TMP_InputField mapHeightField;
    public TMP_InputField mapIndexField;
    public MapData data;

    public List<LevelCreationTiles> tiles;
    public Texture defaultTexture;
    public Texture doorTexture;
    public Texture shopTexture;
    public List<LevelCreationTiles> tileMap;
    public GameObject creatorTilePrefab;
    public GameObject parentTransform;
    public LevelCreationCamera myCamera;

    public bool selectedTile = false;

    public GameObject TilePanel;


    public TextMeshProUGUI selectedTileName;
    public Toggle isVisible;
    public Toggle isInShadow;
    public Dropdown tiletype;
    public Dropdown objectOnTop;
    public GameObject ifdoorTile;
    public GameObject ifHelpTile;

    public LevelCreationTiles currentTile = null;
    //public List
    // Start is called before the first frame update
    void Awake()
    {
        if (myCamera == null)
        {
            myCamera = GameObject.FindObjectOfType<LevelCreationCamera>();
        }
    }
    RaycastHit hit = new RaycastHit();
    private void Update()
    {
        if (selectedTile == false)
        {


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 w = hit.point;
                w.x = Mathf.Round(w.x);
                w.y = Mathf.Round(w.y);
                w.z = Mathf.Round(w.z);

                GameObject hitObj = hit.transform.gameObject;

                if (hitObj.GetComponent<LevelCreationTiles>())
                {
                    LevelCreationTiles hitTile = hitObj.GetComponent<LevelCreationTiles>();


                    if (myCamera.currentTile != null)
                    {
                        if (myCamera.currentTile.myColor != myCamera.currentTile.PREVCOLOR)
                            myCamera.currentTile.MYCOLOR = myCamera.currentTile.PREVCOLOR;
                    }

                    myCamera.currentTile = hitTile;
                    myCamera.currentTile.MYCOLOR = Color.blue;



                }

                if (myCamera.currentTile != myCamera.selectedTile)
                {
                    if (Vector3.Distance(myCamera.selectedTile.transform.position, myCamera.currentTile.transform.position) >= 6.0)
                    {
                        myCamera.selectedTile = myCamera.currentTile;
                    }


                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 w = hit.point;
                w.x = Mathf.Round(w.x);
                w.y = Mathf.Round(w.y);
                w.z = Mathf.Round(w.z);

                GameObject hitObj = hit.transform.gameObject;

                if (hitObj.GetComponent<LevelCreationTiles>())
                {
                    LevelCreationTiles hitTile = hitObj.GetComponent<LevelCreationTiles>();

                    bool alreadySelected = false;

                    if (myCamera.currentTile == hitTile)
                    {
                        alreadySelected = true;

                    }


                    if (TilePanel != null)
                    {
                        if (alreadySelected == true)
                        {

                            TilePanel.gameObject.SetActive(!TilePanel.gameObject.activeSelf);
                            selectedTile = !selectedTile;
                            myCamera.selectedTile = myCamera.currentTile;

                            if (selectedTile == true)
                                currentTile = hitTile;
                            else
                                currentTile = null;

                            if (selectedTileName != null)
                                selectedTileName.text = hitTile.name;

                            if (isVisible != null)
                                isVisible.isOn = hitTile.canBeOccupied;

                            if (isInShadow != null)
                                isInShadow.isOn = hitTile.isInShadow;

                            if (tiletype != null)
                                tiletype.value = (int)hitTile.TTYPE;

                            //(objectOnTop != null)
                            //  objectOnTop


                        }
                        else if (selectedTile == false)
                        {
                            //doesnt hit code?
                            //TilePanel.gameObject.SetActive(true);
                            //selectedTile = true;
                            //if (myCamera.currentTile != null)
                            //    myCamera.currentTile.MYCOLOR = myCamera.currentTile.PREVCOLOR;

                            //myCamera.currentTile = hitTile;
                            //myCamera.currentTile.MYCOLOR = Color.cyan;
                        }
                    }


                }


            }
        }
    }

    public void OpenFilePicker()
    {
        string path = EditorUtility.OpenFilePanel("Select Level to load", "", "csv");
        if (path.Length > 0)
        {
            string[] splitPath = path.Split('/');
            string subpath = splitPath[splitPath.Length - 1];
            string name = subpath.TrimEnd(".csv".ToCharArray());
            data = Common.GetDatabase().GetMapData(name);
            if (data.width > 0)
            {

                if (mapName != null)
                    mapName.text = data.mapName;
                if (mapWidthField != null)
                    mapWidthField.text = data.width.ToString();

                if (mapHeightField != null)
                    mapHeightField.text = data.height.ToString();

                if (mapIndexField != null)
                    mapIndexField.text = data.mapIndex.ToString();




                if (tiles != null)
                {

                    for (int i = tiles.Count - 1; i >= 0; i--)
                    {
                        if (tiles[i])
                        {
                            tiles[i].transform.parent = null;
                            tiles[i].gameObject.SetActive(false);
                        }
                    }
                }


                tileMap.Clear();
                tileMap = getTiles(data.width * data.height);

                int tileIndex = 0;
                float tileHeight = 0;
                float xoffset = 1 / (float)data.width;
                float yoffset = 1 / (float)data.height;
                float yElevation = 0;
                //float xElevation = 0;

                for (int i = 0; i < data.width; i++)
                {

                    for (int j = 0; j < data.height; j++)
                    {
                        int mapIndex = (j * data.width) + i;
                        LevelCreationTiles tile = tileMap[tileIndex];
                        tile.listindex = mapIndex;
                        if (j > data.yMinRestriction && j < data.yMaxRestriction && i > data.xMinRestriction && i < data.xMaxRestriction)
                            tile.transform.position = new Vector3(i * 2, (j * 2 * tileHeight) + yElevation, j * 2);
                        else
                            tile.transform.position = new Vector3(i * 2, (j * 2 * tileHeight), j * 2);
                        tile.transform.parent = parentTransform.transform;
                        tile.name = "Tile " + mapIndex;
                        tile.transform.rotation = Quaternion.Euler(90, 0, 0);
                        tile.setTexture(data.texture);
                        tile.EXTRA = "";
                        tile.isInShadow = false;
                        tile.TTYPE = TileType.regular;
                        tile.setUVs((xoffset * (float)i), (xoffset * (float)(i + 1)), (yoffset * (float)j), (yoffset * (float)(j + 1)));
                        tileIndex++;
                        tile.canBeOccupied = true;
                        if (mapIndex == data.xElevation)
                        {
                            data.yElevation = -1 * data.yElevation;
                        }

                    }
                    yElevation += data.yElevation;

                }
                tileMap.Sort();

                for (int i = 0; i < data.doorIndexes.Count; i++)
                {
                    tileMap[data.doorIndexes[i]].MAT.mainTexture = doorTexture;
                    tileMap[data.doorIndexes[i]].MAP = data.roomNames[i];
                    tileMap[data.doorIndexes[i]].ROOM = data.roomIndexes[i];
                    tileMap[data.doorIndexes[i]].START = data.startIndexes[i];
                    tileMap[data.doorIndexes[i]].setUVs(0, 1, 0, 1);
                    tileMap[data.doorIndexes[i]].TTYPE = TileType.door;
                }
                if (data.shopIndexes.Count > 0)
                {

                    for (int i = 0; i < data.shopIndexes.Count; i++)
                    {
                        tileMap[data.shopIndexes[i]].MAT.mainTexture = shopTexture;
                        tileMap[data.shopIndexes[i]].setUVs(0, 1, 0, 1);
                        tileMap[data.shopIndexes[i]].TTYPE = TileType.shop;
                    }

                }
                int extraIndex = 0;
                if (data.tilesInShadow.Count > 0)
                {
                    for (int i = 0; i < data.tilesInShadow.Count; i++)
                    {
                        tileMap[i].isInShadow = true;
                        tileMap[i].MYCOLOR = Common.dark;
                    }
                }
                if (data.specialTileIndexes.Count > 0)
                {

                    for (int i = 0; i < data.specialTileIndexes.Count; i++)
                    {

                        TileType newtype = data.specialiles[i];
                        int specialIndex = data.specialTileIndexes[i];
                        tileMap[specialIndex].MAT.mainTexture = Common.GetSpecialTexture(newtype);
                        tileMap[specialIndex].setUVs(0, 1, 0, 1);
                        tileMap[specialIndex].TTYPE = newtype;
                        if (newtype == TileType.help)
                        {
                            if (extraIndex < data.specialExtra.Count)
                            {
                                int helpIndex = data.specialExtra[extraIndex];
                                tileMap[data.specialTileIndexes[i]].EXTRA = helpIndex + ";" + Common.GetHelpText(helpIndex);
                                extraIndex++;
                            }
                        }
                        else
                        {
                            switch (newtype)
                            {
                                case TileType.knockback:
                                    tileMap[data.specialTileIndexes[i]].EXTRA = "18 ;" + Common.GetHelpText(18);
                                    break;
                                case TileType.pullin:
                                    tileMap[data.specialTileIndexes[i]].EXTRA = "19 ;" + Common.GetHelpText(19);
                                    break;
                                case TileType.swap:
                                    tileMap[data.specialTileIndexes[i]].EXTRA = "14 ;" + Common.GetHelpText(14);
                                    break;
                                case TileType.reposition:
                                    tileMap[data.specialTileIndexes[i]].EXTRA = "20 ;" + Common.GetHelpText(20);
                                    break;

                            }
                        }
                    }
                }

                if (data.unOccupiedIndexes != null)
                {

                    for (int i = 0; i < data.unOccupiedIndexes.Count; i++)
                    {
                        int tindex = data.unOccupiedIndexes[i];
                        tileMap[tindex].canBeOccupied = false;
                        tileMap[tindex].MYCOLOR = Common.trans;
                    }
                }

                if (myCamera != null)
                {
                    myCamera.selectedTile = tileMap[0];
                }
            }
        }
    }

    public List<LevelCreationTiles> getTiles(int num)
    {
        List<LevelCreationTiles> subTiles = new List<LevelCreationTiles>();
        if (num < tiles.Count)
        {
            for (int i = 0; i < num; i++)
            {
                tiles[i].gameObject.SetActive(true);
                tiles[i].BreakRooms();
                tiles[i].MAT.mainTexture = defaultTexture;
                tiles[i].MYCOLOR = Color.white;
                subTiles.Add(tiles[i]);
            }
        }
        else
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].gameObject.SetActive(true);
                tiles[i].BreakRooms();
                tiles[i].MAT.mainTexture = defaultTexture;
                tiles[i].MYCOLOR = Color.white;
                subTiles.Add(tiles[i]);
            }
            while (tiles.Count < num)
            {
                GameObject temp = Instantiate(creatorTilePrefab, Vector2.zero, Quaternion.identity);
                temp.name = "yo";
                LevelCreationTiles tile = temp.AddComponent<LevelCreationTiles>();
                tile.Setup();
                tile.MYCOLOR = Color.white;
                tiles.Add(tile);
                tile.BreakRooms();
                tile.MAT.mainTexture = defaultTexture;
                subTiles.Add(tile);
            }
        }
        return subTiles;
    }

    public void UpdateCurrentVisibility()
    {
        if (currentTile != null)
        {
            if (isVisible != null)
                currentTile.canBeOccupied = isVisible.isOn;
            if (currentTile.canBeOccupied == true)
            {
                currentTile.MYCOLOR = Color.white;
                currentTile.MYCOLOR = Color.cyan;
            }
            else
            {
                currentTile.MYCOLOR = Common.trans;
                currentTile.MYCOLOR = Color.cyan;
            }
        }
    }

    public void UpdateCurrentShadow()
    {
        if (currentTile != null)
        {
            if (isInShadow != null)
                currentTile.isInShadow = isInShadow.isOn;
            if (currentTile.isInShadow == false)
            {
                currentTile.MYCOLOR = Color.white;
                currentTile.MYCOLOR = Color.cyan;
            }
            else
            {
                currentTile.MYCOLOR = Common.dark;
                currentTile.MYCOLOR = Color.cyan;
            }
        }
    }

    public void UpdateCurrentTileType()
    {
        if (currentTile != null)
        {

            if (tiletype != null)
            {
                currentTile.TTYPE = (TileType)tiletype.value;
                TileType realType = (TileType)tiletype.value;
                switch (realType)
                {
                    case TileType.regular:
                        {
                            if (ifdoorTile != null)
                            {
                                ifdoorTile.SetActive(false);
                            }
                            if (ifHelpTile != null)
                            {
                                ifHelpTile.SetActive(false);
                            }
                            SetCurrentToOriginalTexture();
                        }
                        break;
                    case TileType.door:
                        {
                            if (ifdoorTile != null)
                            {
                                ifdoorTile.SetActive(true);
                            }
                            if (ifHelpTile != null)
                            {
                                ifHelpTile.SetActive(false);
                            }
                            SetCurrentToDoorTexture();
                        }
                        break;
                    case TileType.shop:
                        {
                            if (ifdoorTile != null)
                            {
                                ifdoorTile.SetActive(false);
                            }
                            if (ifHelpTile != null)
                            {
                                ifHelpTile.SetActive(false);
                            }
                            SetCurrentToShopTexture();
                        }
                        break;

                    default:
                        break;
                }
                if (tiletype.value == (int)TileType.door)
                {

                }

            }
        }
    }

    void SetCurrentToDoorTexture()
    {
        currentTile.MAT.mainTexture = doorTexture;
        // currentTile.MAP = data.roomNames[i];
        //currentTile.ROOM = data.roomIndexes[i];
        //currentTile.START = data.startIndexes[i];
        currentTile.setUVs(0, 1, 0, 1);
        currentTile.TTYPE = TileType.door;
    }

    void SetCurrentToShopTexture()
    {
        currentTile.MAT.mainTexture = shopTexture;
        // currentTile.MAP = data.roomNames[i];
        //currentTile.ROOM = data.roomIndexes[i];
        //currentTile.START = data.startIndexes[i];
        currentTile.setUVs(0, 1, 0, 1);
        currentTile.TTYPE = TileType.shop;
    }

    void SetCurrentToSpecialTexture()
    {
        currentTile.MAT.mainTexture = doorTexture;
        // currentTile.MAP = data.roomNames[i];
        //currentTile.ROOM = data.roomIndexes[i];
        //currentTile.START = data.startIndexes[i];
        currentTile.setUVs(0, 1, 0, 1);
        // currentTile.TTYPE = TileType.door;
    }

    void SetCurrentToOriginalTexture()
    {
        float xoffset = 1 / (float)data.width;
        float yoffset = 1 / (float)data.height;
        float i = (currentTile.listindex % data.width);
        float j = (currentTile.listindex / data.width);


        currentTile.setTexture(data.texture);
        currentTile.EXTRA = "";
        currentTile.TTYPE = TileType.regular;
        currentTile.setUVs((xoffset * (float)i), (xoffset * (float)(i + 1)), (yoffset * (float)j), (yoffset * (float)(j + 1)));
    }
}
