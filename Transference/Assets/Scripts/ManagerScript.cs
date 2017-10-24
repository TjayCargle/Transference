using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum State
{
    PlayerInput,
    PlayerMove,
    PlayerAttacking,
    PlayerEquipping,
    PlayerWait,
    FreeCamera,
    EnemyTurn


}
public class ManagerScript : MonoBehaviour
{

    public GameObject Tile;
    public GameObject[] tileMap;
    public List<GridObject> gridObjects;

    public int MapWidth = 0;
    public int MapHeight = 0;
    public State currentState = State.PlayerInput;
    public Canvas commandCanvas;
    public int currentMenuitem = 0;
    CameraScript myCamera;
    public bool isSetup = false;
    public GridObject currentObject;
    public GameObject tempObject;// = new GridObject();
    MenuItem[] commandItems;
    public bool freeCamera = false;
    public int testShow = 0;
    float xDist = 0.0f;
    float yDist = 0.0f;
    public int TwoToOneD(int y, int width, int x)
    {
        return y * width + x;
    }
    // Use this for initialization
    public void Setup()
    {
        tileMap = new GameObject[MapWidth * MapHeight];
        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)] = Instantiate(Tile, new Vector3(i, 0, j), Quaternion.identity);
                temp.AddComponent<TileScript>();
                temp.GetComponent<TileScript>().Setup();
            }
        }
        if (GameObject.FindObjectOfType<CameraScript>())
        {
            myCamera = GameObject.FindObjectOfType<CameraScript>();
        }
        commandItems = GameObject.FindObjectsOfType<MenuItem>();
        currentObject = GameObject.FindObjectOfType<PlayerController>();
        currentObject.transform.position = tileMap[TwoToOneD(MapHeight / 2, MapWidth, MapWidth / 2)].transform.position + new Vector3(0, 0.5f, 0);
        tempObject = new GameObject();
        tempObject.AddComponent<GridObject>();
        tempObject.GetComponent<GridObject>().MOVE_DIST = 10000;
        GridObject[] objs = GameObject.FindObjectsOfType<GridObject>();
        for (int i = 0; i < objs.Length; i++)
        {
            if(objs[i].gameObject == tempObject)
            {
                continue;
            }
            gridObjects.Add(objs[i]);
            objs[i].currentTile = GetTile(objs[i]);
            objs[i].currentTile.isOccupied = true;
        }

   
        isSetup = true;
    }
    void Start()
    {
        if (!isSetup)
        {
            Setup();
        }
        //MaxAttackDist = MoveDist + 2;
    }
    // Update is called once per frame
    void Update()
    {
        if (currentObject)
        {
            switch (currentState)
            {
                case State.PlayerInput:
                    if (!Input.GetKey(KeyCode.None))
                    {
                        ShowGridObjectAffectArea(currentObject);
                    }
                    break;
                case State.PlayerMove:
                    if (!Input.GetKey(KeyCode.None))
                    {
                        ShowGridObjectMoveArea(currentObject);
                    }
                        break;
                case State.PlayerAttacking:
                    if (!Input.GetKey(KeyCode.None))
                    {
                        ShowGridObjectAttackArea(currentObject);
                    }
                    break;
                case State.PlayerEquipping:
                    break;
                case State.PlayerWait:
                    break;
                case State.FreeCamera:
                   
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        ShowWhite();
                        MoveGridObject(tempObject.GetComponent<GridObject>(), new Vector3(0, 0, 1));
                        ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));
                        if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                            ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                        ShowSelectedTile(tempObject.GetComponent<GridObject>());
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        ShowWhite();
                        MoveGridObject(tempObject.GetComponent<GridObject>(), new Vector3(-1, 0, 0));
                        ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));
                        if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                            ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                        ShowSelectedTile(tempObject.GetComponent<GridObject>());
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        ShowWhite();
                        MoveGridObject(tempObject.GetComponent<GridObject>(), new Vector3(0, 0, -1));
                        ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));
                        if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                            ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                        ShowSelectedTile(tempObject.GetComponent<GridObject>());
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        ShowWhite();
                        MoveGridObject(tempObject.GetComponent<GridObject>(), new Vector3(1, 0, 0));
                        ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));
                        if (GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile) != null)
                            ShowGridObjectAffectArea(GetObjectAtTile(tempObject.GetComponent<GridObject>().currentTile));
                        ShowSelectedTile(tempObject.GetComponent<GridObject>());
                    }

                    break;
                case State.EnemyTurn:
                    break;
                default:
                    break;
            }

            //switch (testShow)
            //{
            //    case 2:
            //        ShowGridObjectAttackArea(currentObject.GetComponent<GridObject>());
            //        break;
            //    case 1:
            //        ShowGridObjectMoveArea(currentObject.GetComponent<GridObject>());
            //        break;
            //    default:
            //        ShowGridObjectAffectArea(currentObject.GetComponent<GridObject>());
            //        break;
            //}

        }
        for (int i = 0; i < commandItems.Length; i++)
        {
            if (commandItems[i].myRect)
            {
                if (commandItems[i].itemType == currentMenuitem)
                {
                    commandItems[i].myRect.anchoredPosition = new Vector2(10, commandItems[i].myRect.anchoredPosition.y);
                }
                else
                {
                    commandItems[i].myRect.anchoredPosition = new Vector2(0, commandItems[i].myRect.anchoredPosition.y);
                }
            }
        }


    }
    public void ShowGridObjectAffectArea(GridObject obj)
    {
        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)];
                float tempX = temp.transform.position.x;
                float tempY = temp.transform.position.z;

                float objX = obj.currentTile.transform.position.x;
                float objY = obj.currentTile.transform.position.z;

                int MoveDist = obj.MOVE_DIST;
                int MaxAttackDist = obj.MAX_ATK_DIST;

                xDist = Mathf.Abs(tempX - objX);
                yDist = Mathf.Abs(tempY - objY);
                if (xDist == 0 && yDist == 0)
                {
                    myCamera.currentTile = temp.GetComponent<TileScript>();
                    myCamera.infoObject = GetObjectAtTile(temp.GetComponent<TileScript>());
                    temp.GetComponent<TileScript>().myColor = Color.white;
                }
                else if (xDist + yDist <= MoveDist)
                {
                    temp.GetComponent<TileScript>().myColor = Color.blue;
                }
                else if (xDist + yDist == MoveDist + MaxAttackDist)
                {
                    temp.GetComponent<TileScript>().myColor = Color.red;
                }
                else
                {
                    temp.GetComponent<TileScript>().myColor = Color.white;
                }
            }
        }
    }
    public void ShowWhite()
    {
        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)];
                temp.GetComponent<TileScript>().myColor = Color.white;
            }
        }

    }
    public void ShowSelectedTile(GridObject obj)
    {
        TileScript theTile = GetTile(obj);
        theTile.myColor = Color.grey;

    }
    public void ShowGridObjectMoveArea(GridObject obj)
    {
        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)];
                float tempX = temp.transform.position.x;
                float tempY = temp.transform.position.z;

                float objX = obj.currentTile.transform.position.x;
                float objY = obj.currentTile.transform.position.z;

                int MoveDist = obj.MOVE_DIST;
                int MaxAttackDist = obj.MAX_ATK_DIST;

                xDist = Mathf.Abs(tempX - objX);
                yDist = Mathf.Abs(tempY - objY);
                if (xDist == 0 && yDist == 0)
                {
                    myCamera.currentTile = temp.GetComponent<TileScript>();
                    myCamera.infoObject = GetObjectAtTile(temp.GetComponent<TileScript>());
                    temp.GetComponent<TileScript>().myColor = Color.white;
                }
                else if (xDist + yDist <= MoveDist)
                {
                    temp.GetComponent<TileScript>().myColor = Color.blue;
                }
                else
                {
                    temp.GetComponent<TileScript>().myColor = Color.white;
                }
            }
        }
    }
    public void ShowGridObjectAttackArea(GridObject obj)
    {
        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)];
                float tempX = temp.transform.position.x;
                float tempY = temp.transform.position.z;

                float objX = obj.currentTile.transform.position.x;
                float objY = obj.currentTile.transform.position.z;

                int MoveDist = obj.MOVE_DIST;
                int MaxAttackDist = obj.MAX_ATK_DIST;

                xDist = Mathf.Abs(tempX - objX);
                yDist = Mathf.Abs(tempY - objY);
                if (xDist == 0 && yDist == 0)
                {
                    myCamera.currentTile = temp.GetComponent<TileScript>();
                    myCamera.infoObject = GetObjectAtTile(temp.GetComponent<TileScript>());
                    temp.GetComponent<TileScript>().myColor = Color.white;
                }
                else if (xDist + yDist == MaxAttackDist)
                {
                    temp.GetComponent<TileScript>().myColor = Color.red;
                }
                else
                {
                    temp.GetComponent<TileScript>().myColor = Color.white;
                }
            }
        }
    }
    public void MoveGridObject(GridObject obj, Vector3 direction)
    {
        direction.Normalize();
        Vector3 curPos = GetTile(obj).transform.position;
        Vector3 newPos = curPos + direction;
        int TileIndex = TwoToOneD((int)newPos.z, MapWidth, (int)newPos.x);
        if (TileIndex >= MapHeight * MapWidth)
            return;
        if (TileIndex < 0)
            return;
      if (obj.GetComponent<LivingObject>())
      {
          if (IsTileOccupied(tileMap[TileIndex].GetComponent<TileScript>()) == true)
          {
              return;
          }
      }
        GameObject temp = tileMap[TileIndex];
        float tempX = temp.transform.position.x;
        float tempY = temp.transform.position.z;

        float objX = obj.currentTile.transform.position.x;
        float objY = obj.currentTile.transform.position.z;


        xDist = Mathf.Abs(tempX - objX);
        yDist = Mathf.Abs(tempY - objY);
        if (xDist + yDist <= obj.MOVE_DIST)
        {
        obj.transform.position = tileMap[TileIndex].transform.position + new Vector3(0, 0.5f, 0);
        myCamera.currentTile = tileMap[TileIndex].GetComponent<TileScript>();
        myCamera.infoObject = GetObjectAtTile(tileMap[TileIndex].GetComponent<TileScript>());
        }
    }
    public void ComfirmMoveGridObject(GridObject obj, int tileIndex)
    {
        if (obj.gameObject != tempObject)
        {
            obj.currentTile.isOccupied = false;
            obj.currentTile = tileMap[tileIndex].GetComponent<TileScript>();
            obj.currentTile.isOccupied = true;
        }
        else
        {
            obj.currentTile = tileMap[tileIndex].GetComponent<TileScript>();
        }
        myCamera.currentTile = tileMap[tileIndex].GetComponent<TileScript>();
        myCamera.infoObject = GetObjectAtTile(tileMap[tileIndex].GetComponent<TileScript>());
    }
    public int GetTileIndex(GridObject checkTile)
    {
        int TileIndex = TwoToOneD((int)checkTile.transform.position.z, MapWidth, (int)checkTile.transform.position.x);
        if (TileIndex >= MapHeight * MapWidth)
            return -1;

        return TileIndex;
    }
    public TileScript GetTile(GridObject checkTile)
    {
        int index = GetTileIndex(checkTile);
        if (index < 0)
            return null;
        return tileMap[index].GetComponent<TileScript>();
    }
    public GridObject GetObjectAtTile(TileScript checkTile)
    {
        GridObject returnedObject = null;
        if(checkTile.isOccupied == false)
        {
            return returnedObject;
        }
        else
        {
            for (int i = 0; i < gridObjects.Count; i++)
            {
                if(gridObjects[i].gameObject == tempObject)
                {
                    continue;
                }
                if(gridObjects[i].currentTile == checkTile)
                {
                    returnedObject = gridObjects[i];
                    break;
                }
            }
        }
        return returnedObject;
    }
    public List<GridObject> GetAttackableTiles(GridObject obj,SkillScript skill)
    {
        List<GridObject> returnList = new List<GridObject>();

        for (int i = 0; i < MapHeight; i++)
        {
            for (int j = 0; j < MapWidth; j++)
            {
                GameObject temp = tileMap[TwoToOneD(j, MapWidth, i)];
                float tempX = temp.transform.position.x;
                float tempY = temp.transform.position.z;

                float objX = obj.currentTile.transform.position.x;
                float objY = obj.currentTile.transform.position.z;

                int MoveDist = obj.MOVE_DIST;
                int MaxAttackDist = obj.MAX_ATK_DIST;
                int MinAttackDist = obj.MIN_ATK_DIST;
                xDist = Mathf.Abs(tempX - objX);
                yDist = Mathf.Abs(tempY - objY);
               
               if (xDist + yDist == MaxAttackDist )
                {
                    temp.GetComponent<TileScript>().myColor = Color.red;
                    
                }
                if (xDist + yDist == MinAttackDist)
                {
                    temp.GetComponent<TileScript>().myColor = Color.red;
                }
            }
        }
        return returnList;
    }
    public void SelectMenuItem(GridObject invokingObject)
    {
        for (int i = 0; i < commandItems.Length; i++)
        {

            if (commandItems[i].itemType == currentMenuitem)
            {
                commandItems[i].ApplyAction(invokingObject);
                break;
            }


        }

    }
    public void ComfirmMenuAction(GridObject invokingObject)
    {
        for (int i = 0; i < commandItems.Length; i++)
        {

            if (commandItems[i].itemType == currentMenuitem)
            {
                commandItems[i].ComfirmAction(invokingObject);
                break;
            }


        }
    }
    public bool IsTileOccupied(TileScript checkTile)
    {
        bool result = false;
        int tileX = (int)checkTile.transform.position.x;
        int tileY = (int)checkTile.transform.position.y;
        for (int k = 0; k < gridObjects.Count; k++)
        {
            if (gridObjects[k].gameObject == tempObject)
            {
                continue;
            }
            TileScript gridTile = GetTile(gridObjects[k]);
            if (gridTile == checkTile )
            {
                result = true;
                break;
            } 
        }
    
                return result;
    }
}
