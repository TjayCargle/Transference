using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public List<TileScript> tiles;
    private List<TileScript> skillTiles = new List<TileScript>();
    private List<TileScript> askTiles = new List<TileScript>();
    public List<TileScript> currentMoveableTiles = new List<TileScript>();
    public Texture defaultTexture;

    public GameObject Tile;
    public bool isSetup = false;
    private ManagerScript myManager;
    public void Setup()
    {
        if (!isSetup)
        {
            tiles = new List<TileScript>();
            myManager = GetComponent<ManagerScript>();
            isSetup = true;
        }
    }
    void Start()
    {
        Setup();
    }
    public List<TileScript> EDITABLE_TILES
    {
        get { return askTiles; }
    }
    public List<TileScript> getTiles(int num)
    {
        List<TileScript> subTiles = new List<TileScript>();
        if (num < tiles.Count)
        {
            for (int i = 0; i < num; i++)
            {
                tiles[i].gameObject.SetActive(true);
                tiles[i].BreakRooms();
                tiles[i].MAT.mainTexture = defaultTexture;
                tiles[i].isOccupied = false;
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
                tiles[i].isOccupied = false;
                subTiles.Add(tiles[i]);
            }
            while (tiles.Count < num)
            {
                GameObject temp = Instantiate(Tile, Vector2.zero, Quaternion.identity);
                temp.name = "yo";
                TileScript tile = temp.AddComponent<TileScript>();
                tile.Setup();
                tiles.Add(tile);
                tile.BreakRooms();
                tile.MAT.mainTexture = defaultTexture;
                subTiles.Add(tile);
            }
        }
        return subTiles;
    }



    public List<List<TileScript>> GetLeftTileList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;

        //v1.z += 1;

        //v2.x += 1;

        //v3.z -= 1;

        //v1.x -= 1;
        possiblePossitions.Add(v1);


        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                List<TileScript> tiles = new List<TileScript>();
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
                returntiles.Add(tiles);
            }
        }

        return returntiles;
    }


    public List<List<TileScript>> GetUpTileList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
   
        v1.z += 1;

        possiblePossitions.Add(v1);


        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                List<TileScript> tiles = new List<TileScript>();
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
                returntiles.Add(tiles);
            }
        }

        return returntiles;
    }

    public List<List<TileScript>> GetRightTileList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;

        v1.x += 1;
        
        possiblePossitions.Add(v1);


        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                List<TileScript> tiles = new List<TileScript>();
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
                returntiles.Add(tiles);
            }
        }

        return returntiles;
    }

    public List<List<TileScript>> GetDownTileList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;

        v1.z -= 1;
     
        possiblePossitions.Add(v1);


        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                List<TileScript> tiles = new List<TileScript>();
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
                returntiles.Add(tiles);
            }
        }

        return returntiles;
    }

    public List<List<TileScript>> GetAdjecentTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 2;

        v2.x += 2;

        v3.z -= 2;

        v4.x -= 2;
        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);

        for (int i = 0; i < possiblePossitions.Count; i++)
        {

            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                List<TileScript> tiles = new List<TileScript>();
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
                returntiles.Add(tiles);
            }
        }

        return returntiles;
    }

    public List<List<TileScript>> GetPinWheelTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;

        v1.z += 2;
        v2.x += 2;
        v3.z -= 2;
        v4.x -= 2;

        v5.x += 4;
        v6.z += 4;
        v7.z -= 4;
        v8.x -= 4;

        v9.x -= 2;
        v9.z += 2;

        v10.x += 2;
        v10.z += 2;

        v11.x -= 2;
        v11.z -= 2;

        v12.x += 2;
        v12.z -= 2;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                List<TileScript> tiles = new List<TileScript>();
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
                returntiles.Add(tiles);
            }
        }
        return returntiles;
    }

    public List<List<TileScript>> GetDetachedTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        v1.z += 4;

        v2.x += 4;

        v3.z -= 4;

        v4.x -= 4;



        v5.x += 2;
        v5.z += 2;

        v6.x += -2;
        v6.z += 2;

        v7.x += -2;
        v7.z += -2;

        v8.x += 2;
        v8.z += -2;


        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);


        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);


        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                List<TileScript> tiles = new List<TileScript>();
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
                returntiles.Add(tiles);
            }
        }

        return returntiles;
    }

    public List<List<TileScript>> GetStretchedTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;


        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;

        Vector3 v13 = origin.transform.position;
        Vector3 v14 = origin.transform.position;
        Vector3 v15 = origin.transform.position;
        Vector3 v16 = origin.transform.position;

        Vector3 v17 = origin.transform.position;
        Vector3 v18 = origin.transform.position;
        Vector3 v19 = origin.transform.position;
        Vector3 v20 = origin.transform.position;

        v1.z += 4;

        v2.x += 4;

        v3.z -= 4;

        v4.x -= 4;



        v5.x += 2;
        v5.z += 2;

        v6.x += -2;
        v6.z += 2;

        v7.x += -2;
        v7.z += -2;

        v8.x += 2;
        v8.z += -2;







        v9.z += 6;

        v10.x += 2;
        v10.z += 4;

        v11.x += 4;
        v11.z += 2;

        v12.x += 6;



        v13.x += 4;
        v13.z += -2;

        v14.x += 2;
        v14.z += -4;

        v15.z += -6;


        v16.x += -2;
        v16.z += -4;


        v17.x += -4;
        v17.z += -2;

        v18.x += -6;


        v19.x += -4;
        v19.z += 2;

        v20.x += -2;
        v20.z += 4;


        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);


        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);


        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);


        possiblePossitions.Add(v13);
        possiblePossitions.Add(v14);
        possiblePossitions.Add(v15);
        possiblePossitions.Add(v16);


        possiblePossitions.Add(v17);
        possiblePossitions.Add(v18);
        possiblePossitions.Add(v19);
        possiblePossitions.Add(v20);


        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                List<TileScript> tiles = new List<TileScript>();
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
                returntiles.Add(tiles);
            }
        }

        return returntiles;
    }

    public List<List<TileScript>> GetRotatorTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;


        v1.z += 2;
        v2.x += 2;
        v3.z -= 2;
        v4.x -= 2;
                
        v5.x -= 2;
        v5.z += 2;
                
        v6.x += 2;
        v6.z += 2;
                
        v7.x -= 2;
        v7.z -= 2;
                
        v8.x += 2;
        v8.z -= 2;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);


        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                List<TileScript> tiles = new List<TileScript>();
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
                returntiles.Add(tiles);
            }
        }
        return returntiles;
    }

    public List<List<TileScript>> GetFanTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;

        Vector3 v12 = origin.transform.position;
        Vector3 v13 = origin.transform.position;
        Vector3 v14 = origin.transform.position;

        Vector3 v15 = origin.transform.position;
        Vector3 v16 = origin.transform.position;
        Vector3 v17 = origin.transform.position;

        Vector3 v18 = origin.transform.position;
        Vector3 v19 = origin.transform.position;
        Vector3 v20 = origin.transform.position;

        v1.z += 2;
        v2.x += 2;
        v3.z -= 2;
        v4.x -= 2;

        v5.x -= 2;
        v5.z += 2;

        v6.x += 2;
        v6.z += 2;

        v7.x -= 2;
        v7.z -= 2;

        v8.x += 2;
        v8.z -= 2;



        v9.x += -2;
        v9.z += 4;

        v10.z += 4;

        v11.x += 2;
        v11.z += 4;


        v12.x += 4;
        v12.z += 2;

        v13.x += 4;

        v14.x += 4;
        v14.z += -2;


        v15.x += 2;
        v15.z += -4;

        v16.z += -4;

        v17.x += -2;
        v17.z += -4;


        v18.x += -4;
        v18.z += -2;

        v19.x += -4;


        v20.x += -4;
        v20.z += 2;





        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);
        possiblePossitions.Add(v13);
        possiblePossitions.Add(v14);
        possiblePossitions.Add(v15);
        possiblePossitions.Add(v16);
        possiblePossitions.Add(v17);
        possiblePossitions.Add(v18);
        possiblePossitions.Add(v19);
        possiblePossitions.Add(v20);


        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                List<TileScript> tiles = new List<TileScript>();
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
                returntiles.Add(tiles);
            }
        }
        return returntiles;
    }

    public List<List<TileScript>> GetSpearTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;

        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;

        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;



        v1.z += 2;
        v2.z += 4;


        v3.x += 2;
        v4.x += 4;


        v5.z += -2;
        v6.z += -4;


        v7.x += -2;
        v8.x += -4;




        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);


        int currindex = 0;
        for (int i = 0; i < 4; i++)
        {
            List<TileScript> tiles = new List<TileScript>();
            for (int j = currindex; j < currindex + 2; j++)
            {
                index = myManager.GetTileIndex(possiblePossitions[j]);
                if (index >= 0)
                {
                    TileScript newTile = myManager.GetTileAtIndex(index);
                    tiles.Add(newTile);
                }

            }
            currindex += 2;
            if (tiles.Count > 0)
                returntiles.Add(tiles);
        }


        return returntiles;
    }

    public List<List<TileScript>> GetLanceTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;

        Vector3 v13 = origin.transform.position;
        Vector3 v14 = origin.transform.position;
        Vector3 v15 = origin.transform.position;
        Vector3 v16 = origin.transform.position;



        v1.z += 2;
        v2.z += 4;
        v3.z += 6;
        v4.z += 8;


        v5.x += 2;
        v6.x += 4;
        v7.x += 6;
        v8.x += 8;


        v9.z += -2;
        v10.z += -4;
        v11.z += -6;
        v12.z += -8;


        v13.x += -2;
        v14.x += -4;
        v15.x += -6;
        v16.z += -8;



        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);
        possiblePossitions.Add(v13);
        possiblePossitions.Add(v14);
        possiblePossitions.Add(v15);
        possiblePossitions.Add(v16);


        int currindex = 0;
        for (int i = 0; i < 4; i++)
        {
            List<TileScript> tiles = new List<TileScript>();
            for (int j = currindex; j < currindex + 4; j++)
            {
                index = myManager.GetTileIndex(possiblePossitions[j]);
                if (index >= 0)
                {
                    TileScript newTile = myManager.GetTileAtIndex(index);
                    tiles.Add(newTile);
                }

            }
            currindex += 4;
            if (tiles.Count > 0)
                returntiles.Add(tiles);
        }


        return returntiles;
    }

    public List<List<TileScript>> GetLineTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;

        Vector3 v4 = origin.transform.position;
        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;

        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;
        Vector3 v9 = origin.transform.position;

        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;


        v1.x += -2;
        v1.z += 2;

        v2.z += 2;

        v3.x += 2;
        v3.z += 2;

        v4.x += 2;
        v4.z += 2;

        v5.x += 2;

        v6.x += 2;
        v6.z += -2;


        v7.x += 2;
        v7.z += -2;

        v8.z += -2;

        v9.x += -2;
        v9.z += -2;

        v10.x += -2;
        v10.z += -2;

        v11.x += -2;

        v12.x += -2;
        v12.z += 2;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);

        int currindex = 0;
        for (int i = 0; i < 4; i++)
        {
            List<TileScript> tiles = new List<TileScript>();
            for (int j = currindex; j < currindex + 3; j++)
            {
                index = myManager.GetTileIndex(possiblePossitions[j]);
                if (index >= 0)
                {
                    TileScript newTile = myManager.GetTileAtIndex(index);
                    tiles.Add(newTile);
                }

            }
            currindex += 3;
            if (tiles.Count > 0)
                returntiles.Add(tiles);
        }


        return returntiles;
    }

    public List<List<TileScript>> GetRectTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;

        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;
        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;

        Vector3 v13 = origin.transform.position;
        Vector3 v14 = origin.transform.position;
        Vector3 v15 = origin.transform.position;
        Vector3 v16 = origin.transform.position;
        Vector3 v17 = origin.transform.position;
        Vector3 v18 = origin.transform.position;

        Vector3 v19 = origin.transform.position;
        Vector3 v20 = origin.transform.position;
        Vector3 v21 = origin.transform.position;
        Vector3 v22 = origin.transform.position;
        Vector3 v23 = origin.transform.position;
        Vector3 v24 = origin.transform.position;


        v1.x +=-2;
        v1.z += 2;
        v2.z += 2;
        v3.x += 2;
        v3.z += 2;
        v4.x +=-2;
        v4.z += 4;
        v5.z += 4;
        v6.x += 2;
        v6.z += 4;

        v7.x += 2;
        v7.z += 2;
        v8.x += 2;
        v9.x += 2;
        v9.z +=-2;
        v10.x += 4;
        v10.z += 2;
        v11.x += 4;
        v12.x += 4;
        v12.z += -2;


        v13.x +=  2;
        v13.z += -2;
        v14.z += -2;
        v15.x += -2;
        v15.z += -2;
        v16.x +=  2;
        v16.z += -4;
        v17.z += -4;
        v18.x += -2;
        v18.z += -4;

        v19.x += -2;
        v19.z += -2;
        v20.x += -2;
        v21.x += -2;
        v21.z +=  2;
        v22.x += -4;
        v22.z += -2;
        v23.x += -4;
        v24.x += -4;
        v24.z += 2;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);

        possiblePossitions.Add(v13);
        possiblePossitions.Add(v14);
        possiblePossitions.Add(v15);
        possiblePossitions.Add(v16);
        possiblePossitions.Add(v17);
        possiblePossitions.Add(v18);
        possiblePossitions.Add(v19);
        possiblePossitions.Add(v20);
        possiblePossitions.Add(v21);
        possiblePossitions.Add(v22);
        possiblePossitions.Add(v23);
        possiblePossitions.Add(v24);

        int currindex = 0;
        for (int i = 0; i < 4; i++)
        {
            List<TileScript> tiles = new List<TileScript>();
            for (int j = currindex; j < currindex + 6; j++)
            {
                index = myManager.GetTileIndex(possiblePossitions[j]);
                if (index >= 0)
                {
                    TileScript newTile = myManager.GetTileAtIndex(index);
                    tiles.Add(newTile);
                }

            }
            currindex += 6;
            if (tiles.Count > 0)
                returntiles.Add(tiles);
        }


        return returntiles;
    }

    public List<List<TileScript>> GetConeTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;

        Vector3 v13 = origin.transform.position;
        Vector3 v14 = origin.transform.position;
        Vector3 v15 = origin.transform.position;
        Vector3 v16 = origin.transform.position;


        v1.x += -2;
        v1.z += 2;

        v2.z += 2;

        v3.x += 2;
        v3.z += 2;

        v4.z += 4;




        v5.x += 2;
        v5.z += 2;

        v6.x += 2;

        v7.x += 2;
        v7.z += -2;

        v8.x += 4;



        v9.x += 2;
        v9.z += -2;

        v10.z += -2;

        v11.x += -2;
        v11.z += -2;

        v12.z += -4;




        v13.x += -2;
        v13.z += 2;

        v14.x += -2;

        v15.x += -2;
        v15.z += -2;

        v16.x += -4;



        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);
        possiblePossitions.Add(v13);
        possiblePossitions.Add(v14);
        possiblePossitions.Add(v15);
        possiblePossitions.Add(v16);

        int currindex = 0;
        for (int i = 0; i < 4; i++)
        {
            List<TileScript> tiles = new List<TileScript>();
            for (int j = currindex; j < currindex + 4; j++)
            {
                index = myManager.GetTileIndex(possiblePossitions[j]);
                if (index >= 0)
                {
                    TileScript newTile = myManager.GetTileAtIndex(index);
                    tiles.Add(newTile);
                }

            }
            currindex += 4;
            if (tiles.Count > 0)
                returntiles.Add(tiles);
        }


        return returntiles;
    }

    public List<List<TileScript>> GetTPoseTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;

        Vector3 v8 = origin.transform.position;
        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;
        Vector3 v13 = origin.transform.position;
        Vector3 v14 = origin.transform.position;

        Vector3 v15 = origin.transform.position;
        Vector3 v16 = origin.transform.position;
        Vector3 v17 = origin.transform.position;
        Vector3 v18 = origin.transform.position;
        Vector3 v19 = origin.transform.position;
        Vector3 v20 = origin.transform.position;
        Vector3 v21 = origin.transform.position;

        Vector3 v22 = origin.transform.position;
        Vector3 v23 = origin.transform.position;
        Vector3 v24 = origin.transform.position;
        Vector3 v25 = origin.transform.position;
        Vector3 v26 = origin.transform.position;
        Vector3 v27 = origin.transform.position;
        Vector3 v28 = origin.transform.position;


        v1.x += -2;
        v1.z += 2;
        v2.z += 2;
        v3.x += 2;
        v3.z += 2;
        v4.z += 4;
        v5.x += -4;
        v5.z += 2;
        v6.z += 6;
        v7.x += 4;
        v7.z += 2;



        v8.x += 2;
        v8.z += 2;
        v9.x += 2;
        v10.x += 2;
        v10.z += -2;
        v11.x += 4;
        v12.x += 2;
        v12.z += 4;
        v13.x += 6;
        v14.x += 2;
        v14.z += -4;


        v15.x += 2;
        v15.z += -2;
        v16.z += -2;
        v17.x += -2;
        v17.z += -2;
        v18.z += -4;
        v19.x += 4;
        v19.z += -2;
        v20.z += -6;
        v21.x += -4;
        v21.z += -2;


        v22.x += -2;
        v22.z += 2;
        v23.x += -2;
        v24.x += -2;
        v24.z += -2;
        v25.x += -4;
        v26.x += -2;
        v26.z += 4;
        v27.x += -6;
        v28.x += -2;
        v28.z += -4;


        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);
        possiblePossitions.Add(v13);
        possiblePossitions.Add(v14);
        possiblePossitions.Add(v15);
        possiblePossitions.Add(v16);
        possiblePossitions.Add(v17);
        possiblePossitions.Add(v18);
        possiblePossitions.Add(v19);
        possiblePossitions.Add(v20);
        possiblePossitions.Add(v21);
        possiblePossitions.Add(v22);
        possiblePossitions.Add(v23);
        possiblePossitions.Add(v24);
        possiblePossitions.Add(v25);
        possiblePossitions.Add(v26);
        possiblePossitions.Add(v27);
        possiblePossitions.Add(v28);

        int currindex = 0;
        for (int i = 0; i < 4; i++)
        {
            List<TileScript> tiles = new List<TileScript>();
            for (int j = currindex; j < currindex + 7; j++)
            {
                index = myManager.GetTileIndex(possiblePossitions[j]);
                if (index >= 0)
                {
                    TileScript newTile = myManager.GetTileAtIndex(index);
                    tiles.Add(newTile);
                }

            }
            currindex += 7;
            if (tiles.Count > 0)
                returntiles.Add(tiles);
        }


        return returntiles;
    }

    public List<List<TileScript>> GetCloverTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 2;

        v2.x += 2;

        v3.z -= 2;

        v4.x -= 2;
        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);

        List<TileScript> tiles = new List<TileScript>();
        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
            }
        }
        if (tiles.Count > 0)
            returntiles.Add(tiles);
        return returntiles;
    }

    public List<List<TileScript>> GetCrossTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        v1.z += 2;

        v2.x += 2;

        v3.z -= 2;

        v4.x -= 2;


        v5.z += 4;

        v6.x += 4;

        v7.z -= 4;

        v8.x -= 4;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);


        List<TileScript> tiles = new List<TileScript>();
        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
            }
        }
        if (tiles.Count > 0)
            returntiles.Add(tiles);
        return returntiles;
    }

    public List<List<TileScript>> GetSquareTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;


        v1.z += 2;
        v2.x += 2;
        v3.z -= 2;
        v4.x -= 2;
                
        v5.x -= 2;
        v5.z += 2;
                
        v6.x += 2;
        v6.z += 2;
                
        v7.x -= 2;
        v7.z -= 2;
                
        v8.x += 2;
        v8.z -= 2;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);

        List<TileScript> tiles = new List<TileScript>();

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
            }
        }
        if (tiles.Count > 0)
            returntiles.Add(tiles);
        return returntiles;
    }

    public List<List<TileScript>> GetBoxTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;

        Vector3 v12 = origin.transform.position;
        Vector3 v13 = origin.transform.position;
        Vector3 v14 = origin.transform.position;

        Vector3 v15 = origin.transform.position;
        Vector3 v16 = origin.transform.position;
        Vector3 v17 = origin.transform.position;

        Vector3 v18 = origin.transform.position;
        Vector3 v19 = origin.transform.position;
        Vector3 v20 = origin.transform.position;

        v1.z += 2;
        v2.x += 2;
        v3.z -= 2;
        v4.x -= 2;

        v5.x -= 2;
        v5.z += 2;

        v6.x += 2;
        v6.z += 2;

        v7.x -= 2;
        v7.z -= 2;

        v8.x += 2;
        v8.z -= 2;


        v9.x += -2;
        v9.z += 4;

        v10.z += 4;

        v11.x += 2;
        v11.z += 4;


        v12.x += 4;
        v12.z += 2;

        v13.x += 4;

        v14.x += 4;
        v14.z += -2;


        v15.x += 2;
        v15.z += -4;

        v16.z += -4;

        v17.x += -2;
        v17.z += -4;


        v18.x += -4;
        v18.z += 2;

        v19.x += -4;

        v20.x += -4;
        v20.z += -2;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);
        possiblePossitions.Add(v13);
        possiblePossitions.Add(v14);
        possiblePossitions.Add(v15);
        possiblePossitions.Add(v16);
        possiblePossitions.Add(v17);
        possiblePossitions.Add(v18);
        possiblePossitions.Add(v19);
        possiblePossitions.Add(v20);

        List<TileScript> tiles = new List<TileScript>();

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
            }
        }
        if (tiles.Count > 0)
            returntiles.Add(tiles);
        return returntiles;
    }

    public List<List<TileScript>> GetDiamondTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 4;

        v2.x += 4;

        v3.z -= 4;

        v4.x -= 4;
        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);

        List<TileScript> tiles = new List<TileScript>();
        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
            }
        }
        if (tiles.Count > 0)
            returntiles.Add(tiles);
        return returntiles;
    }

    public List<List<TileScript>> GetCrosshairTilesList(TileScript origin)
    {
        List<List<TileScript>> returntiles = new List<List<TileScript>>();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        v1.z += 4;

        v2.x += 4;

        v3.z -= 4;

        v4.x -= 4;

        v5.x += 2;
        v5.z += 2;

        v6.x += -2;
        v6.z += 2;

        v7.x += 2;
        v7.z += -2;

        v8.x += -2;
        v8.z += -2;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);

        List<TileScript> tiles = new List<TileScript>();
        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = myManager.GetTileAtIndex(index);
                tiles.Add(newTile);
            }
        }
        if (tiles.Count > 0)
            returntiles.Add(tiles);
        return returntiles;
    }




    public List<TileScript> GetAdjecentTiles(TileScript origin)
    {

       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 2;

        v2.x += 2;

        v3.z -= 2;

        v4.x -= 2;
        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {

                TileScript newTile = myManager.tileMap[index];
             
               skillTiles.Add(newTile);
            }
        }

        return skillTiles;
    }

    public List<TileScript> GetPinWheelTiles(TileScript origin)
    {

       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;

        v1.z += 2;
        v2.x += 2;
        v3.z -= 2;
        v4.x -= 2;

        v5.x += 4;
        v6.z += 4;
        v7.z -= 4;
        v8.x -= 4;

        v9.x -= 2;
        v9.z += 2;

        v10.x += 2;
        v10.z += 2;

        v11.x -= 2;
        v11.z -= 2;

        v12.x += 2;
        v12.z -= 2;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
              
                TileScript newTile = myManager.GetTileAtIndex(index);
               skillTiles.Add(newTile);
              
            }
        }
        return skillTiles;
    }

    public List<TileScript> GetDetachedTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        v1.z += 4;

        v2.x += 4;

        v3.z -= 4;

        v4.x -= 4;



        v5.x += 2;
        v5.z += 2;

        v6.x += -2;
        v6.z += 2;

        v7.x += -2;
        v7.z += -2;

        v8.x += 2;
        v8.z += -2;


        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);


        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);


        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                List<TileScript> tiles = new List<TileScript>();
                TileScript newTile = myManager.GetTileAtIndex(index);
               skillTiles.Add(newTile);
              
            }
        }

        return skillTiles;
    }

    public List<TileScript> GetStretchedTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;


        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;

        Vector3 v13 = origin.transform.position;
        Vector3 v14 = origin.transform.position;
        Vector3 v15 = origin.transform.position;
        Vector3 v16 = origin.transform.position;

        Vector3 v17 = origin.transform.position;
        Vector3 v18 = origin.transform.position;
        Vector3 v19 = origin.transform.position;
        Vector3 v20 = origin.transform.position;

        v1.z += 4;

        v2.x += 4;

        v3.z -= 4;

        v4.x -= 4;



        v5.x += 2;
        v5.z += 2;

        v6.x += -2;
        v6.z += 2;

        v7.x += -2;
        v7.z += -2;

        v8.x += 2;
        v8.z += -2;







        v9.z += 6;

        v10.x += 2;
        v10.z += 4;

        v11.x += 4;
        v11.z += 2;

        v12.x += 6;



        v13.x += 4;
        v13.z += -2;

        v14.x += 2;
        v14.z += -4;

        v15.z += -6;


        v16.x += -2;
        v16.z += -4;


        v17.x += -4;
        v17.z += -2;

        v18.x += -6;


        v19.x += -4;
        v19.z += 2;

        v20.x += -2;
        v20.z += 4;


        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);


        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);


        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);


        possiblePossitions.Add(v13);
        possiblePossitions.Add(v14);
        possiblePossitions.Add(v15);
        possiblePossitions.Add(v16);


        possiblePossitions.Add(v17);
        possiblePossitions.Add(v18);
        possiblePossitions.Add(v19);
        possiblePossitions.Add(v20);


        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
            
                TileScript newTile = myManager.GetTileAtIndex(index);
               skillTiles.Add(newTile);
               
            }
        }

        return skillTiles;
    }

    public List<TileScript> GetRotatorTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;


        v1.z += 2;
        v2.x += 2;
        v3.z -= 2;
        v4.x -= 2;

        v5.x -= 2;
        v5.z += 2;

        v6.x += 2;
        v6.z += 2;

        v7.x -= 2;
        v7.z -= 2;

        v8.x += 2;
        v8.z -= 2;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);


        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
      
                TileScript newTile = myManager.GetTileAtIndex(index);
               skillTiles.Add(newTile);
               
            }
        }
        return skillTiles;
    }

    public List<TileScript> GetFanTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;

        Vector3 v12 = origin.transform.position;
        Vector3 v13 = origin.transform.position;
        Vector3 v14 = origin.transform.position;

        Vector3 v15 = origin.transform.position;
        Vector3 v16 = origin.transform.position;
        Vector3 v17 = origin.transform.position;

        Vector3 v18 = origin.transform.position;
        Vector3 v19 = origin.transform.position;
        Vector3 v20 = origin.transform.position;
        v1.z += 2;
        v2.x += 2;
        v3.z -= 2;
        v4.x -= 2;

        v5.x -= 2;
        v5.z += 2;

        v6.x += 2;
        v6.z += 2;

        v7.x -= 2;
        v7.z -= 2;

        v8.x += 2;
        v8.z -= 2;



        v9.x += -2;
        v9.z += 4;

        v10.z += 4;

        v11.x += 2;
        v11.z += 4;


        v12.x += 4;
        v12.z += 2;

        v13.x += 4;

        v14.x += 4;
        v14.z += -2;


        v15.x += 2;
        v15.z += -4;

        v16.z += -4;

        v17.x += -2;
        v17.z += -4;


        v18.x += -4;
        v18.z += -2;

        v19.x += -4;


        v20.x += -4;
        v20.z += 2;





        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);
        possiblePossitions.Add(v13);
        possiblePossitions.Add(v14);
        possiblePossitions.Add(v15);
        possiblePossitions.Add(v16);
        possiblePossitions.Add(v17);
        possiblePossitions.Add(v18);
        possiblePossitions.Add(v19);
        possiblePossitions.Add(v20);


        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
              
                TileScript newTile = myManager.GetTileAtIndex(index);
               skillTiles.Add(newTile);
            
            }
        }
        return skillTiles;
    }

    public List<TileScript> GetSpearTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;

        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;

        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;



        v1.z += 2;
        v2.z += 4;


        v3.x += 2;
        v4.x += 4;


        v5.z += -2;
        v6.z += -4;


        v7.x += -2;
        v8.x += -4;




        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);


        int currindex = 0;
        for (int i = 0; i < 4; i++)
        {
          
            for (int j = currindex; j < currindex + 2; j++)
            {
                index = myManager.GetTileIndex(possiblePossitions[j]);
                if (index >= 0)
                {
                    TileScript newTile = myManager.GetTileAtIndex(index);
                   skillTiles.Add(newTile);
                }

            }
            currindex += 2;
         
        }


        return skillTiles;
    }

    public List<TileScript> GetLanceTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;

        Vector3 v13 = origin.transform.position;
        Vector3 v14 = origin.transform.position;
        Vector3 v15 = origin.transform.position;
        Vector3 v16 = origin.transform.position;


        v1.z += 2;
        v2.z += 4;
        v3.z += 6;
        v4.z += 8;


        v5.x += 2;
        v6.x += 4;
        v7.x += 6;
        v8.x += 8;


        v9.z += -2;
        v10.z += -4;
        v11.z += -6;
        v12.z += -8;


        v13.x += -2;
        v14.x += -4;
        v15.x += -6;
        v16.z += -8;



        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);
        possiblePossitions.Add(v13);
        possiblePossitions.Add(v14);
        possiblePossitions.Add(v15);
        possiblePossitions.Add(v16);


        int currindex = 0;
        for (int i = 0; i < 4; i++)
        {
          
            for (int j = currindex; j < currindex + 4; j++)
            {
                index = myManager.GetTileIndex(possiblePossitions[j]);
                if (index >= 0)
                {
                    TileScript newTile = myManager.GetTileAtIndex(index);
                   skillTiles.Add(newTile);
                }

            }
            currindex += 4;
          
        }


        return skillTiles;
    }

    public List<TileScript> GetLineTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;

        Vector3 v4 = origin.transform.position;
        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;

        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;
        Vector3 v9 = origin.transform.position;

        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;


        v1.x += -2;
        v1.z += 2;

        v2.z += 2;

        v3.x += 2;
        v3.z += 2;

        v4.x += 2;
        v4.z += 2;

        v5.x += 2;

        v6.x += 2;
        v6.z += -2;


        v7.x += 2;
        v7.z += -2;

        v8.z += -2;

        v9.x += -2;
        v9.z += -2;

        v10.x += -2;
        v10.z += -2;

        v11.x += -2;

        v12.x += -2;
        v12.z += 2;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);

        int currindex = 0;
        for (int i = 0; i < 4; i++)
        {
           
            for (int j = currindex; j < currindex + 3; j++)
            {
                index = myManager.GetTileIndex(possiblePossitions[j]);
                if (index >= 0)
                {
                    TileScript newTile = myManager.GetTileAtIndex(index);
                   skillTiles.Add(newTile);
                }

            }
            currindex += 3;
       
        }


        return skillTiles;
    }

    public List<TileScript> GetRectTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;

        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;
        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;

        Vector3 v13 = origin.transform.position;
        Vector3 v14 = origin.transform.position;
        Vector3 v15 = origin.transform.position;
        Vector3 v16 = origin.transform.position;
        Vector3 v17 = origin.transform.position;
        Vector3 v18 = origin.transform.position;

        Vector3 v19 = origin.transform.position;
        Vector3 v20 = origin.transform.position;
        Vector3 v21 = origin.transform.position;
        Vector3 v22 = origin.transform.position;
        Vector3 v23 = origin.transform.position;
        Vector3 v24 = origin.transform.position;


        v1.x += -2;
        v1.z += 2;
        v2.z += 2;
        v3.x += 2;
        v3.z += 2;
        v4.x += -2;
        v4.z += 4;
        v5.z += 4;
        v6.x += 2;
        v6.z += 4;

        v7.x += 2;
        v7.z += 2;
        v8.x += 2;
        v9.x += 2;
        v9.z += -2;
        v10.x += 4;
        v10.z += 2;
        v11.x += 4;
        v12.x += 4;
        v12.z += -2;


        v13.x += 2;
        v13.z += -2;
        v14.z += -2;
        v15.x += -2;
        v15.z += -2;
        v16.x += 2;
        v16.z += -4;
        v17.z += -4;
        v18.x += -2;
        v18.z += -4;

        v19.x += -2;
        v19.z += -2;
        v20.x += -2;
        v21.x += -2;
        v21.z += 2;
        v22.x += -4;
        v22.z += -2;
        v23.x += -4;
        v24.x += -4;
        v24.z += 2;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);

        possiblePossitions.Add(v13);
        possiblePossitions.Add(v14);
        possiblePossitions.Add(v15);
        possiblePossitions.Add(v16);
        possiblePossitions.Add(v17);
        possiblePossitions.Add(v18);
        possiblePossitions.Add(v19);
        possiblePossitions.Add(v20);
        possiblePossitions.Add(v21);
        possiblePossitions.Add(v22);
        possiblePossitions.Add(v23);
        possiblePossitions.Add(v24);

        int currindex = 0;
        for (int i = 0; i < 4; i++)
        {
      
            for (int j = currindex; j < currindex + 6; j++)
            {
                index = myManager.GetTileIndex(possiblePossitions[j]);
                if (index >= 0)
                {
                    TileScript newTile = myManager.GetTileAtIndex(index);
                   skillTiles.Add(newTile);
                }

            }
            currindex += 6;
           
        }


        return skillTiles;
    }

    public List<TileScript> GetConeTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;

        Vector3 v13 = origin.transform.position;
        Vector3 v14 = origin.transform.position;
        Vector3 v15 = origin.transform.position;
        Vector3 v16 = origin.transform.position;


        v1.x += -2;
        v1.z += 2;

        v2.z += 2;

        v3.x += 2;
        v3.z += 2;

        v4.z += 4;




        v5.x += 2;
        v5.z += 2;

        v6.x += 2;

        v7.x += 2;
        v7.z += -2;

        v8.x += 4;



        v9.x += 2;
        v9.z += -2;

        v10.z += -2;

        v11.x += -2;
        v11.z += -2;

        v12.z += -4;




        v13.x += -2;
        v13.z += 2;

        v14.x += -2;

        v15.x += -2;
        v15.z += -2;

        v16.x += -4;



        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);
        possiblePossitions.Add(v13);
        possiblePossitions.Add(v14);
        possiblePossitions.Add(v15);
        possiblePossitions.Add(v16);

        int currindex = 0;
        for (int i = 0; i < 4; i++)
        {
   
            for (int j = currindex; j < currindex + 4; j++)
            {
                index = myManager.GetTileIndex(possiblePossitions[j]);
                if (index >= 0)
                {
                    TileScript newTile = myManager.GetTileAtIndex(index);
                   skillTiles.Add(newTile);
                }

            }
            currindex += 4;
        
        }


        return skillTiles;
    }

    public List<TileScript> GetTPoseTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;

        Vector3 v8 = origin.transform.position;
        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;
        Vector3 v12 = origin.transform.position;
        Vector3 v13 = origin.transform.position;
        Vector3 v14 = origin.transform.position;

        Vector3 v15 = origin.transform.position;
        Vector3 v16 = origin.transform.position;
        Vector3 v17 = origin.transform.position;
        Vector3 v18 = origin.transform.position;
        Vector3 v19 = origin.transform.position;
        Vector3 v20 = origin.transform.position;
        Vector3 v21 = origin.transform.position;

        Vector3 v22 = origin.transform.position;
        Vector3 v23 = origin.transform.position;
        Vector3 v24 = origin.transform.position;
        Vector3 v25 = origin.transform.position;
        Vector3 v26 = origin.transform.position;
        Vector3 v27 = origin.transform.position;
        Vector3 v28 = origin.transform.position;


        v1.x += -2;
        v1.z += 2;
        v2.z += 2;
        v3.x += 2;
        v3.z += 2;
        v4.z += 4;
        v5.x += -4;
        v5.z += 2;
        v6.z += 6;
        v7.x += 4;
        v7.z += 2;



        v8.x += 2;
        v8.z += 2;
        v9.x += 2;
        v10.x += 2;
        v10.z += -2;
        v11.x += 4;
        v12.x += 2;
        v12.z += 4;
        v13.x += 6;
        v14.x += 2;
        v14.z += -4;


        v15.x += 2;
        v15.z += -2;
        v16.z += -2;
        v17.x += -2;
        v17.z += -2;
        v18.z += -4;
        v19.x += 4;
        v19.z += -2;
        v20.z += -6;
        v21.x += -4;
        v21.z += -2;


        v22.x += -2;
        v22.z += 2;
        v23.x += -2;
        v24.x += -2;
        v24.z += -2;
        v25.x += -4;
        v26.x += -2;
        v26.z += 4;
        v27.x += -6;
        v28.x += -2;
        v28.z += -4;


        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);
        possiblePossitions.Add(v13);
        possiblePossitions.Add(v14);
        possiblePossitions.Add(v15);
        possiblePossitions.Add(v16);
        possiblePossitions.Add(v17);
        possiblePossitions.Add(v18);
        possiblePossitions.Add(v19);
        possiblePossitions.Add(v20);
        possiblePossitions.Add(v21);
        possiblePossitions.Add(v22);
        possiblePossitions.Add(v23);
        possiblePossitions.Add(v24);
        possiblePossitions.Add(v25);
        possiblePossitions.Add(v26);
        possiblePossitions.Add(v27);
        possiblePossitions.Add(v28);

        int currindex = 0;
        for (int i = 0; i < 4; i++)
        {
          
            for (int j = currindex; j < currindex + 7; j++)
            {
                index = myManager.GetTileIndex(possiblePossitions[j]);
                if (index >= 0)
                {
                    TileScript newTile = myManager.GetTileAtIndex(index);
                   skillTiles.Add(newTile);
                }

            }
            currindex += 7;
        
        }


        return skillTiles;
    }

    public List<TileScript> GetCloverTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 2;

        v2.x += 2;

        v3.z -= 2;

        v4.x -= 2;
        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);

     
        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = myManager.GetTileAtIndex(index);
               skillTiles.Add(newTile);
            }
        }
     
        return skillTiles;
    }

    public List<TileScript> GetCrossTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        v1.z += 2;

        v2.x += 2;

        v3.z -= 2;

        v4.x -= 2;


        v5.z += 4;

        v6.x += 4;

        v7.z -= 4;

        v8.x -= 4;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);


    
        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = myManager.GetTileAtIndex(index);
               skillTiles.Add(newTile);
            }
        }
      
        return skillTiles;
    }

    public List<TileScript> GetSquareTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;


        v1.z += 2;
        v2.x += 2;
        v3.z -= 2;
        v4.x -= 2;

        v5.x -= 2;
        v5.z += 2;

        v6.x += 2;
        v6.z += 2;

        v7.x -= 2;
        v7.z -= 2;

        v8.x += 2;
        v8.z -= 2;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);



        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = myManager.GetTileAtIndex(index);
               skillTiles.Add(newTile);
            }
        }
     
        return skillTiles;
    }

    public List<TileScript> GetBoxTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();

        int index = -1;
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        Vector3 v9 = origin.transform.position;
        Vector3 v10 = origin.transform.position;
        Vector3 v11 = origin.transform.position;

        Vector3 v12 = origin.transform.position;
        Vector3 v13 = origin.transform.position;
        Vector3 v14 = origin.transform.position;

        Vector3 v15 = origin.transform.position;
        Vector3 v16 = origin.transform.position;
        Vector3 v17 = origin.transform.position;

        Vector3 v18 = origin.transform.position;
        Vector3 v19 = origin.transform.position;
        Vector3 v20 = origin.transform.position;

        v1.z += 2;
        v2.x += 2;
        v3.z -= 2;
        v4.x -= 2;

        v5.x -= 2;
        v5.z += 2;

        v6.x += 2;
        v6.z += 2;

        v7.x -= 2;
        v7.z -= 2;

        v8.x += 2;
        v8.z -= 2;


        v9.x += -2;
        v9.z += 4;

        v10.z += 4;

        v11.x += 2;
        v11.z += 4;


        v12.x += 4;
        v12.z += 2;

        v13.x += 4;

        v14.x += 4;
        v14.z += -2;


        v15.x += 2;
        v15.z += -4;

        v16.z += -4;

        v17.x += -2;
        v17.z += -4;


        v18.x += -4;
        v18.z += 2;

        v19.x += -4;

        v20.x += -4;
        v20.z += -2;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);
        possiblePossitions.Add(v9);
        possiblePossitions.Add(v10);
        possiblePossitions.Add(v11);
        possiblePossitions.Add(v12);
        possiblePossitions.Add(v13);
        possiblePossitions.Add(v14);
        possiblePossitions.Add(v15);
        possiblePossitions.Add(v16);
        possiblePossitions.Add(v17);
        possiblePossitions.Add(v18);
        possiblePossitions.Add(v19);
        possiblePossitions.Add(v20);

  

        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = myManager.GetTileAtIndex(index);
               skillTiles.Add(newTile);
            }
        }
   
        return skillTiles;
    }

    public List<TileScript> GetDiamondTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;
        v1.z += 4;

        v2.x += 4;

        v3.z -= 4;

        v4.x -= 4;
        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);

       
        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = myManager.GetTileAtIndex(index);
               skillTiles.Add(newTile);
            }
        }

        return skillTiles;
    }

    public List<TileScript> GetCrosshairTiles(TileScript origin)
    {
       skillTiles.Clear();
        List<Vector3> possiblePossitions = new List<Vector3>();
        Vector3 v1 = origin.transform.position;
        Vector3 v2 = origin.transform.position;
        Vector3 v3 = origin.transform.position;
        Vector3 v4 = origin.transform.position;

        Vector3 v5 = origin.transform.position;
        Vector3 v6 = origin.transform.position;
        Vector3 v7 = origin.transform.position;
        Vector3 v8 = origin.transform.position;

        v1.z += 4;

        v2.x += 4;

        v3.z -= 4;

        v4.x -= 4;

        v5.x += 2;
        v5.z += 2;

        v6.x += -2;
        v6.z += 2;

        v7.x += 2;
        v7.z += -2;

        v8.x += -2;
        v8.z += -2;

        possiblePossitions.Add(v1);
        possiblePossitions.Add(v2);
        possiblePossitions.Add(v3);
        possiblePossitions.Add(v4);
        possiblePossitions.Add(v5);
        possiblePossitions.Add(v6);
        possiblePossitions.Add(v7);
        possiblePossitions.Add(v8);

    
        for (int i = 0; i < possiblePossitions.Count; i++)
        {
            int index = myManager.GetTileIndex(possiblePossitions[i]);
            if (index >= 0)
            {
                TileScript newTile = myManager.GetTileAtIndex(index);
               skillTiles.Add(newTile);
            }
        }
 
        return skillTiles;
    }
}
