using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerScript : MonoBehaviour
{

    public GameObject Tile;
    public GameObject[] tileMap;
    public List<GridObject> gridObjects;
    public List<TileScript> currentAttackList;
    public List<List<TileScript>> attackableTiles;
    int selectedAttackingTile = 0;
    public int MapWidth = 0;
    public int MapHeight = 0;
    public State currentState = State.PlayerInput;
    public State prevState = State.PlayerInput;
    public int currentMenuitem = 0;
    public int itemMenuitem = 0;
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
            if (objs[i].gameObject == tempObject)
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
    //DMG * (100/100+DEF)
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
                    bool hitkey = false;
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        ShowWhite();
                        selectedAttackingTile = 0;
                        hitkey = true;
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        ShowWhite();
                        selectedAttackingTile = 1;
                        hitkey = true;
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        ShowWhite();
                        selectedAttackingTile = 2;
                        hitkey = true;
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        ShowWhite();
                        selectedAttackingTile = 3;
                        hitkey = true;
                    }
                    if (hitkey == true)
                    {
                        if (attackableTiles.Count > 0)
                        {
                            showAttackableTiles();
                            currentAttackList = attackableTiles[selectedAttackingTile];
                            bool foundSomething = false;
                            for (int i = 0; i < currentAttackList.Count; i++)
                            {
                                if (GetObjectAtTile(currentAttackList[i]) != null)
                                {
                                    foundSomething = true;
                                    if (SetGridObjectPosition(tempObject.GetComponent<GridObject>(), currentAttackList[i].transform.position) == true)
                                    {
                                        ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));

                                    }

                                }
                                currentAttackList[i].myColor = Color.green;
                            }
                            if (foundSomething == false)
                            {
                                if (SetGridObjectPosition(tempObject.GetComponent<GridObject>(), currentAttackList[0].transform.position) == true)
                                {
                                    ComfirmMoveGridObject(tempObject.GetComponent<GridObject>(), GetTileIndex(tempObject.GetComponent<GridObject>()));

                                }

                            }
                            //  ShowSelectedTile(tempObject.GetComponent<GridObject>());
                        }
                    }
                    break;
                case State.PlayerEquippingMenu:
                    if (Input.GetKeyDown(KeyCode.W))
                    {

                        updateCurrentMenuPosition(currentMenuitem);
                    }
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        updateCurrentMenuPosition(currentMenuitem);
                    }
                    break;
                case State.PlayerEquipping:
                    {

                    }
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



    }

    public void showAttackableTiles()
    {
        for (int i = 0; i < attackableTiles.Count; i++)
        {
            for (int j = 0; j < attackableTiles[i].Count; j++)
            {
                attackableTiles[i][j].myColor = Color.red;
            }
        }
    }
    public void updateCurrentMenuPosition(int currentAnchor)
    {
        for (int i = 0; i < commandItems.Length; i++)
        {
            if (commandItems[i].myRect)
            {


                if (commandItems[i].itemType == currentAnchor)
                {
                    commandItems[i].myRect.anchoredPosition = new Vector2(commandItems[i].myRect.anchoredPosition.x + 10, commandItems[i].myRect.anchoredPosition.y);
                }
                else
                {
                    commandItems[i].myRect.anchoredPosition = new Vector2(100, commandItems[i].myRect.anchoredPosition.y);
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

                int MoveDist = 0;
                int attackDist = 0;
                if (obj.GetComponent<LivingObject>())
                {
                    LivingObject liveObj = obj.GetComponent<LivingObject>();
                    MoveDist = liveObj.MOVE_DIST;
                    attackDist = liveObj.WEAPON.DIST;
                }
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
                else if (xDist + yDist <= MoveDist + attackDist)
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

                int attackDist = 0;

                if (obj.GetComponent<LivingObject>())
                {
                    LivingObject liveObj = obj.GetComponent<LivingObject>();
                    attackDist = liveObj.WEAPON.DIST;
                }

                xDist = Mathf.Abs(tempX - objX);
                yDist = Mathf.Abs(tempY - objY);
                if (xDist == 0 && yDist == 0)
                {
                    myCamera.currentTile = temp.GetComponent<TileScript>();
                    myCamera.infoObject = GetObjectAtTile(temp.GetComponent<TileScript>());
                    temp.GetComponent<TileScript>().myColor = Color.white;
                }
                else if (xDist + yDist <= attackDist)
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
    public bool SetGridObjectPosition(GridObject obj, Vector3 newLocation)
    {

        Vector3 curPos = GetTile(obj).transform.position;
        int TileIndex = TwoToOneD((int)newLocation.z, MapWidth, (int)newLocation.x);
        if (TileIndex >= MapHeight * MapWidth)
            return false;
        if (TileIndex < 0)
            return false;
        if (obj.GetComponent<LivingObject>())
        {
            if (IsTileOccupied(tileMap[TileIndex].GetComponent<TileScript>()) == true)
            {
                return false;
            }
        }

        obj.transform.position = tileMap[TileIndex].transform.position + new Vector3(0, 0.5f, 0);
        myCamera.currentTile = tileMap[TileIndex].GetComponent<TileScript>();
        myCamera.infoObject = GetObjectAtTile(tileMap[TileIndex].GetComponent<TileScript>());
        myCamera.currentTile = tileMap[TileIndex].GetComponent<TileScript>();
        return true;
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
            myCamera.currentTile = tileMap[TileIndex].GetComponent<TileScript>();
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
        if (TileIndex < 0)
            return -1;
        return TileIndex;
    }
    public int GetTileIndex(Vector3 checkPosition)
    {
        int TileIndex = TwoToOneD((int)checkPosition.z, MapWidth, (int)checkPosition.x);
        if (TileIndex >= MapHeight * MapWidth)
            return -1;
        if (TileIndex < 0)
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
    public TileScript GetTileAtIndex(int checkIndex)
    {
        if (checkIndex >= MapHeight * MapWidth)
            return null;
        if (checkIndex < 0)
            return null;
        return tileMap[checkIndex].GetComponent<TileScript>();
    }
    public GridObject GetObjectAtTile(TileScript checkTile)
    {
        GridObject returnedObject = null;
        if (checkTile.isOccupied == false)
        {
            return returnedObject;
        }
        else
        {
            for (int i = 0; i < gridObjects.Count; i++)
            {
                if (gridObjects[i].gameObject == tempObject)
                {
                    continue;
                }
                if (gridObjects[i].currentTile == checkTile)
                {
                    returnedObject = gridObjects[i];
                    break;
                }
            }
        }
        return returnedObject;
    }
    public List<List<TileScript>> GetSkillsAttackableTiles(GridObject obj, SkillScript skill)
    {
        int checkIndex = GetTileIndex(obj);
        if (checkIndex == -1)
            return null;

        List<List<TileScript>> returnList = new List<List<TileScript>>();
        List<Vector2> affectedTiles = skill.TILES;

        Vector2 checkDist = Vector2.zero;
        for (int i = 0; i < 4; i++)
        {
            List<TileScript> tiles = new List<TileScript>();
            if (affectedTiles != null)
            {
                for (int j = 0; j < affectedTiles.Count; j++)
                {
                    Vector2 Dist = affectedTiles[j];
                    switch (i)
                    {
                        case 0:
                            checkDist.x = Dist.x;
                            checkDist.y = Dist.y;
                            break;

                        case 1:
                            checkDist.x = Dist.y;
                            checkDist.y = Dist.x * -1;
                            break;

                        case 2:

                            checkDist.x = Dist.x * -1;
                            checkDist.y = Dist.y * -1;

                            break;

                        case 3:
                            checkDist.x = Dist.y * -1; //Yes x = y
                            checkDist.y = Dist.x;
                            break;
                    }


                    Vector3 checkPos = obj.transform.position;
                    checkPos.x += checkDist.x;
                    checkPos.z += checkDist.y;
                    int testIndex = GetTileIndex(checkPos);
                    if (testIndex >= 0)
                    {
                        TileScript realTile = GetTileAtIndex(testIndex);
                        if (!tiles.Contains(realTile))
                            tiles.Add(realTile);
                    }
                }
            }

            returnList.Add(tiles);

        }
        return returnList;
    }

    public List<int> GetTargetList()
    {
        if (currentAttackList.Count > 0)
        {
            
            List<int> targetIndicies = new List<int>();

            for (int i = 0; i < currentAttackList.Count; i++)
            {
                if (GetObjectAtTile(currentAttackList[i]) != null)
                {
           
                    targetIndicies.Add(i);

                }
            }
            return targetIndicies;
        }
        return null;
}
    public List<List<TileScript>> GetWeaponAttackableTiles(LivingObject liveObj)
    {
        int checkIndex = GetTileIndex(liveObj);
        if (checkIndex == -1)
            return null;

        List<List<TileScript>> returnList = new List<List<TileScript>>();
        int Dist = liveObj.WEAPON.DIST;
        int Range = liveObj.WEAPON.Range;

        Vector2 checkDist = Vector2.zero;

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    checkDist.x = 0;
                    checkDist.y = 1;
                    break;

                case 1:
                    checkDist.x = 1;
                    checkDist.y = 0;
                    break;

                case 2:
                    checkDist.x = 0;
                    checkDist.y = -1;
                    break;

                case 3:
                    checkDist.x = -1;
                    checkDist.y = 0;
                    break;
            }
            List<TileScript> tiles = new List<TileScript>();
            for (int j = 0; j < Range; j++)
            {
                Vector3 checkPos = liveObj.transform.position;
                checkPos.x += (checkDist.x * Dist);
                checkPos.z += (checkDist.y * Dist);

                checkPos.x -= (checkDist.x * j);
                checkPos.z -= (checkDist.y * j);
                int testIndex = GetTileIndex(checkPos);

                if (testIndex >= 0)
                {
                    TileScript realTile = GetTileAtIndex(testIndex);
                    if (!tiles.Contains(realTile))
                        tiles.Add(realTile);
                }
            }
            if (tiles.Count > 0)
            {
                returnList.Add(tiles);
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
    public void CancelMenuAction(GridObject invokingObject)
    {
        for (int i = 0; i < commandItems.Length; i++)
        {

            if (commandItems[i].itemType == currentMenuitem)
            {
                commandItems[i].CancelAction(invokingObject);
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
            if (gridTile == checkTile)
            {
                result = true;
                break;
            }
        }

        return result;
    }
    public void DamageLivingObject(LivingObject dmgObject, int dmg)
    {
        dmgObject.STATS.HEALTH -= dmg;
    }
    public DmgReaction CalcDamage(LivingObject attackingObject, LivingObject dmgObject, Element attackingElement, EType attackType, int dmg)
    {
        DmgReaction react = new DmgReaction();
        float mod = 0.0f;
        switch (dmgObject.ARMOR.HITLIST[(int)attackingElement])
        {
            case EHitType.normal:
                mod = 1.0f;
                react.reaction = Reaction.none;
                Debug.Log("normal damage");
                break;
            case EHitType.resists:
                mod = 0.5f;
                react.reaction = Reaction.none;
                Debug.Log("resists damage");
                break;
            case EHitType.nulls:
                react.damage = 0;
                react.reaction = Reaction.nulled;
                Debug.Log("nulls damage");
                return react;

            case EHitType.reflects:
                react = CalcDamage(dmgObject, attackingObject, attackingElement, attackType, dmg);
                react.reaction = Reaction.reflected;
                Debug.Log("reflects damage");
                return react;
            case EHitType.absorbs:
                mod = -1.0f;
                react.reaction = Reaction.none;
                Debug.Log("reaction damage");
                break;
            case EHitType.weak:
                mod = 1.5f;
                react.reaction = Reaction.none;
                Debug.Log("weak damage");
                break;
            case EHitType.snatched:
                mod = 1.5f;
                react.reaction = Reaction.snatched;
                Debug.Log("snatched damage");
                break;
            case EHitType.cripples:
                mod = 1.5f;
                react.reaction = Reaction.statDrop;
                Debug.Log("crippling damage");
                break;
            case EHitType.knocked:
                mod = 1.5f;
                react.reaction = Reaction.knockback;
                Debug.Log("knockback damage");
                break;
            default:
                break;
        }

        int returnInt = 0;
        float calc = 0.0f;

        if (attackType == EType.physical)
        {
            Debug.Log("physical");
            calc = (float)attackingObject.ATTACK / (float)dmgObject.DEFENSE;
        }
        else
        {
            calc = (float)attackingObject.ATTACK / (float)dmgObject.RESIESTANCE;

        }

        calc = dmg * calc;

        calc = calc * (attackingObject.LEVEL / dmgObject.LEVEL);

        calc = Mathf.Sqrt(calc);


        calc = calc * mod;
        returnInt = (Mathf.FloorToInt(calc));

        react.damage = returnInt;
        return react;
    }
}
