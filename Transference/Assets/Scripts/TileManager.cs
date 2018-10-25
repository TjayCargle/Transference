using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {
   public List<TileScript> tiles;
    public GameObject Tile;
    public bool isSetup = false;
    public void Setup()
    {
        if(!isSetup)
        {
            tiles = new List<TileScript>();
            isSetup = true;
        }
    }
    void Start () {
        Setup();
	}
	
    public List<TileScript> getTiles(int num)
    {
        List<TileScript> subTiles = new List<TileScript>();
        if(num < tiles.Count)
        {
            for (int i = 0; i < num; i++)
            {
                tiles[i].gameObject.SetActive(true);
                subTiles.Add(tiles[i]);
            }
        }
        else
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].gameObject.SetActive(true);
                subTiles.Add(tiles[i]);
            }
            while (tiles.Count < num)
            {
                GameObject temp = Instantiate(Tile, Vector2.zero, Quaternion.identity);
                TileScript tile = temp.AddComponent<TileScript>();
                tile.Setup();
                tiles.Add(tile);
                subTiles.Add(tile);
            }
        }
        return subTiles;
    }
}
