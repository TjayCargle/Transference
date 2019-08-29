using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    protected ManagerScript myManager;
    public TileScript currentTile;
    public string FullName;
    [SerializeField]
    public Faction myFaction;
    protected int MoveDist = 0;
    public bool isSetup = false;
    [SerializeField]
    protected bool isDead;
    [SerializeField]
    protected BaseStats baseStats;
    [SerializeField]
    protected ModifiedStats modifiedStats;
    [SerializeField]
   protected Sprite faceSprite = null;
    public int MapIndex = -1;
    public int id = -1;
    public virtual string NAME
    {
        get { return FullName; }
        set { FullName = value; }
    }

    public virtual Sprite FACE
    {
        get { return faceSprite; }
        set { faceSprite = value; }
    }

    public virtual Faction FACTION
    {
        get { return myFaction; }
        set { myFaction = value; }
    }

    public virtual int MOVE_DIST
    {
        get { return MoveDist; }
        set { MoveDist = value; }
    }
    public bool DEAD
    {
        get { return isDead; }
        set { isDead = value; }
    }

    public BaseStats BASE_STATS
    {
        get { return baseStats; }
        set { baseStats = value; }
    }
    public ModifiedStats STATS
    {
        get { return modifiedStats; }
        set { modifiedStats = value; }
    }
    // Use this for initialization

    public virtual void Setup()
    {
        if (!isSetup)
        {

            if (GameObject.FindObjectOfType<ManagerScript>())
            {
                myManager = GameObject.FindObjectOfType<ManagerScript>();
            }
            if (myManager)
            {
                if (!myManager.isSetup)
                {
                    myManager.Setup();
                }

            }

       
            if(!GetComponent<LivingObject>())
            {
                if (GetComponent<LivingSetup>())
                {
                    GetComponent<LivingSetup>().Setup();
                }

                if (GetComponent<AnimationScript>())
                {
                    GetComponent<AnimationScript>().Setup();
                }
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                transform.Rotate(new Vector3(90, 0, 0));
            }
        }
        isSetup = true;
    }
    protected void Start()
    {
        if (!isSetup)
        {
            Setup();
        }

    }

    public virtual void Die()
    {

        myManager.CreateEvent(this, null, "death event", DieEvent, DeathStart);
    }
    protected bool isdoneDying = false;
    protected bool startedDeathAnimation = false;
    public virtual void DeathStart()
    {
            startedDeathAnimation = false;
            myManager.PlaySquishSnd();
            isdoneDying = false;
            StartCoroutine(FadeOut());      
    }

    public virtual bool DieEvent(Object data)
    {

        return isdoneDying;
    }
    public virtual IEnumerator FadeOut()
    {
        startedDeathAnimation = true;
        //    Debug.Log("living dying");
        if (GetComponent<SpriteRenderer>())
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Color subtract = new Color(0, 0, 0, 0.1f);
            int num = 0;
        
            while (renderer.color.a > 0)
            {
                num++;
                if (num > 999)
                {
                    Debug.Log("time expired");
                    break;
                }
                renderer.color = renderer.color - subtract;
                yield return null;
            }
            if (currentTile)
                currentTile.isOccupied = false;
            myManager.gridObjects.Remove(this);
            gameObject.SetActive(false);
            isdoneDying = true;
        }
      
    }


    public void GenericUnset()
    {
        isSetup = false;
        DEAD = false;
        STATS.Reset(true);
        BASE_STATS.Reset();
        BASE_STATS.HEALTH = BASE_STATS.MAX_HEALTH;

        if (GetComponent<AnimationScript>())
        {
            GetComponent<AnimationScript>().Unset();
        }
    }
}
