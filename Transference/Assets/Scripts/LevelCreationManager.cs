using System.Collections;
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

    public TMP_InputField roomNum;
    public TMP_InputField roomName;
    public TMP_InputField playerStart;
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
                        if (myCamera.currentTile.isInShadow)
                        {
                            if (myCamera.currentTile.MYCOLOR != Common.dark)
                            {
                                myCamera.currentTile.MYCOLOR = Common.dark;
                            }
                        }
                        else
                        {
                            if (myCamera.currentTile.MYCOLOR != myCamera.currentTile.PREVCOLOR)
                            {
                                myCamera.currentTile.MYCOLOR = myCamera.currentTile.PREVCOLOR;
                            }
                        }
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
                            {
                                tiletype.value = (int)hitTile.TTYPE;

                                //if(hitTile.TTYPE == TileType.door)
                                {
                                    if(roomNum != null)                                    
                                        roomNum.text = hitTile.ROOM.ToString();

                                    if (roomName != null)
                                        roomName.text = hitTile.MAP;

                                    if (playerStart != null)
                                        playerStart.text = hitTile.START.ToString();

                                }
                            }

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
#if UNITY_EDITOR
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
                //     float yElevation = 0;
                //float xElevation = 0;

                for (int i = 0; i < data.width; i++)
                {

                    for (int j = 0; j < data.height; j++)
                    {
                        int mapIndex = (j * data.width) + i;
                        LevelCreationTiles tile = tileMap[tileIndex];
                        tile.listindex = mapIndex;
                        //   if (j > data.yMinRestriction && j < data.yMaxRestriction && i > data.xMinRestriction && i < data.xMaxRestriction)
                        //     tile.transform.position = new Vector3(i * 2, (j * 2 * tileHeight) + yElevation, j * 2);
                        // else
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
                    //yElevation += data.yElevation;

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
                        int shadowindex = data.tilesInShadow[i];
                        tileMap[shadowindex].isInShadow = true;
                        tileMap[shadowindex].MYCOLOR = Common.dark;
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
    public void SaveFilePanel()
    {

        string path = EditorUtility.SaveFilePanelInProject("Save Level", "", "csv", "Save the level?");
        if (path.Length > 0)
        {

            List<int> shadowTiles = new List<int>();
            List<int> invisibleTiles = new List<int>();
            List<int> doorTiles = new List<int>();
            List<int> specialTiles = new List<int>();
            List<TileType> specialTileTypes = new List<TileType>();

            for (int i = 0; i < tileMap.Count; i++)
            {
                LevelCreationTiles ltile = tileMap[i];
                if (ltile.isInShadow)
                {
                    shadowTiles.Add(ltile.listindex);
                }
                if (ltile.canBeOccupied == false)
                {
                    invisibleTiles.Add(ltile.listindex);
                }
                if (ltile.TTYPE != TileType.regular)
                {
                    if (ltile.TTYPE == TileType.door)
                    {
                        doorTiles.Add(ltile.listindex);
                    }
                    else
                    {
                        specialTiles.Add(ltile.listindex);
                        specialTileTypes.Add(ltile.TTYPE);
                    }
                }
            }

            string addedString = "i," + mapIndexField.text + "\n";
            addedString += "w," + mapWidthField.text + "\n";
            addedString += "h," + mapHeightField.text;
            if(invisibleTiles.Count > 0)
            {
                addedString += "\nr," + invisibleTiles[0];
                for (int i = 1; i < invisibleTiles.Count; i++)
                {
                    addedString += "," + invisibleTiles[i];
                }
            }
            if (shadowTiles.Count > 0)
            {
                addedString += "\nst," + shadowTiles[0];
                for (int i = 1; i < shadowTiles.Count; i++)
                {
                    addedString += "," + shadowTiles[i];
                }
            }
            if (specialTiles.Count > 0)
            {
                addedString += "\nsti," + specialTiles[0];
                for (int i = 1; i < specialTiles.Count; i++)
                {
                    addedString += "," + specialTiles[i];
                }
                
                addedString += "\nstt," + specialTileTypes[0];
                for (int i = 1; i < specialTileTypes.Count; i++)
                {
                    addedString += "," + specialTileTypes[i];
                }
            }

            if (doorTiles.Count > 0)
            {
                addedString += "\nd," + doorTiles[0];
                for (int i = 1; i < doorTiles.Count; i++)
                {
                    addedString += "," + doorTiles[i];
                }

                addedString += "\nm," + tileMap[doorTiles[0]].MAP;
                for (int i = 1; i < doorTiles.Count; i++)
                {
                    addedString += "," + tileMap[doorTiles[i]].MAP;
                }

                addedString += "\nl," + tileMap[doorTiles[0]].ROOM;
                for (int i = 1; i < doorTiles.Count; i++)
                {
                    addedString += "," + tileMap[doorTiles[i]].ROOM;
                }

                addedString += "\nx," + tileMap[doorTiles[0]].START;
                for (int i = 1; i < doorTiles.Count; i++)
                {
                    addedString += "," + tileMap[doorTiles[i]].START;
                }
            }


            string testString = "-UnoccupiedTiles count = r \n" +
                "-event room = n  -event tiles -event nums \n" +
                "-id = i \n" +
                "-texture num = t \n" +
                "-map width = w \n" +
                "-map height = h \n" +
                "-num of doors/door indexs = d [location], [location] \n" +
                "-Room Names = m \n" +
                "- Room locations = l \n" +
                "-Start Indexes = x \n" +
                "-num of enemies / enemy indexs = e [location], [location] \n" +
                "-num of rando glyphs / glyph indexes = g [location], [location] \n" +
                "-num of lock glyphs / lock glyph indexes = gi [location], [location] \n" +
                "-num of shops/shop index = s [location], [location] \n" +
                "-num objs/ obj indexs = o [location], [location] \n" +
                "-obj ids = oi [location], [location] \n" +
                addedString.Trim() +
                "";
            File.WriteAllText(path, testString);
        }

        AssetDatabase.Refresh();
    }

#endif
    public void CreateMap()
    {
#if UNITY_EDITOR
        {

            data = Common.GetDatabase().GetBlankMap();
            System.Int32.TryParse(mapWidthField.text, out data.width);
            System.Int32.TryParse(mapHeightField.text, out data.height);
            System.Int32.TryParse(mapIndexField.text, out data.mapIndex);
            if (data.width > 0)
            {

                if (tiles != null)
                {

                    for (int i = tiles.Count - 1; i >= 0; i--)
                    {
                        if (tiles[i])
                        {
                            tiles[i].isInShadow = false;
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
                //     float yElevation = 0;
                //float xElevation = 0;

                for (int i = 0; i < data.width; i++)
                {

                    for (int j = 0; j < data.height; j++)
                    {
                        int mapIndex = (j * data.width) + i;
                        LevelCreationTiles tile = tileMap[tileIndex];
                        tile.listindex = mapIndex;
                        //   if (j > data.yMinRestriction && j < data.yMaxRestriction && i > data.xMinRestriction && i < data.xMaxRestriction)
                        //     tile.transform.position = new Vector3(i * 2, (j * 2 * tileHeight) + yElevation, j * 2);
                        // else
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
                    //yElevation += data.yElevation;

                }
                tileMap.Sort();


                if (myCamera != null)
                {
                    myCamera.selectedTile = tileMap[0];
                }
            }
        }
#endif
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
                    case TileType.help:
                        {
                            if (ifdoorTile != null)
                            {
                                ifdoorTile.SetActive(false);
                            }
                            if (ifHelpTile != null)
                            {
                                ifHelpTile.SetActive(true);
                            }
                            SetCurrentToSpecialTexture();
                        }
                        break;
                    default:
                        {
                            if (ifdoorTile != null)
                            {
                                ifdoorTile.SetActive(false);
                            }
                            if (ifHelpTile != null)
                            {
                                ifHelpTile.SetActive(false);
                            }
                            SetCurrentToSpecialTexture();
                        }
                        break;
                }
                if (tiletype.value == (int)TileType.door)
                {

                }

            }
        }
    }

    public void UpdateDoorIndex()
    {
        if (currentTile != null)
        {
            if(roomNum != null)
            {
                int outVar = -1;
               System.Int32.TryParse(roomNum.text, out outVar);
                currentTile.ROOM = outVar;
            }

        }
    }

    public void UpdateDoorName()
    {
        if (currentTile != null)
        {
       

            if (roomName != null)
            {
                currentTile.MAP = roomName.text;
            }

       
        }
    }

    public void UpdateDoorStart()
    {
        if (currentTile != null)
        {
    

            if (playerStart != null)
            {
                int outVar = -1;
                System.Int32.TryParse(playerStart.text, out outVar);
                currentTile.START = outVar;
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
        if (currentTile != null)
        {

            currentTile.MAT.mainTexture = Common.GetSpecialTexture(currentTile.TTYPE);
            // currentTile.MAP = data.roomNames[i];
            //currentTile.ROOM = data.roomIndexes[i];
            //currentTile.START = data.startIndexes[i];
            currentTile.setUVs(0, 1, 0, 1);
            // currentTile.TTYPE = TileType.door;
        }
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
