using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : LivingObject
{
    private enum Facing
    {
        North,
        East,
        South,
        West,
    }
    //public bool canMove = false;
  
    // Use this for initialization
    new void Start()
    {
        base.Start();
       // MoveDist = 1;
       // MAX_ATK_DIST = 2;
       // SkillManager.CreateSkill(1);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        switch (myManager.currentState)
        {
            case State.PlayerInput:
                if (Input.GetKeyDown(KeyCode.W))
                {
                    myManager.currentMenuitem--;
                    if (myManager.currentMenuitem < 0)
                    {
                        myManager.currentMenuitem = 4;
                    }
                }
         
                if (Input.GetKeyDown(KeyCode.S))
                {
                    myManager.currentMenuitem++;
                    if (myManager.currentMenuitem > 4)
                    {
                        myManager.currentMenuitem = 0;
                    }
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    myManager.CancelMenuAction(this);
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    myManager.SelectMenuItem(this);
                }
                break;
            case State.PlayerMove:
                if (Input.GetKeyDown(KeyCode.W))
                {
                    myManager.MoveGridObject(this, new Vector3(0, 0, 1));
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    myManager.MoveGridObject(this, new Vector3(-1, 0, 0));
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    myManager.MoveGridObject(this, new Vector3(0, 0, -1));
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    myManager.MoveGridObject(this, new Vector3(1, 0, 0));
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    myManager.ComfirmMenuAction(this);
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    myManager.CancelMenuAction(this);
                }
                break;
            case State.PlayerAttacking:

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if(myManager.GetObjectAtTile(myManager.tempObject.GetComponent<GridObject>().currentTile) != null)
                    {
                        GridObject potentialTarget = myManager.GetObjectAtTile(myManager.tempObject.GetComponent<GridObject>().currentTile);
                        if(potentialTarget.GetComponent<LivingObject>())
                        {
                            LivingObject target = potentialTarget.GetComponent<LivingObject>();
                            int dmg = myManager.CalcDamage(target, WEAPON.AFINITY, WEAPON.ATTACK);
                            myManager.DamageLivingObject(target, dmg);
                        }
                    }
                    myManager.ComfirmMenuAction(this);
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    myManager.CancelMenuAction(this);
                }
                break;
            case State.PlayerEquipping:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    myManager.CancelMenuAction(this);
                }
                break;
            case State.PlayerWait:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    myManager.CancelMenuAction(this);
                }
                break;
            case State.FreeCamera:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    myManager.ComfirmMenuAction(this);
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    myManager.CancelMenuAction(this);
                }
                break;
            case State.EnemyTurn:
                break;
            default:
                break;
        }
    

    }
}
