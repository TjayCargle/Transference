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
    protected SpriteRenderer mySR;
    protected AnimationScript animationScript;
    public int currentTileIndex;


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

    public AnimationScript ANIM
    {
        get { return animationScript; }
        set { animationScript = value; }
    }

    public SpriteRenderer RENDERER
    {
        get { return mySR; }
        set { mySR = value; }
    }
    // Use this for initialization

    public virtual void Setup()
    {
        if (!isSetup)
        {
            mySR = GetComponent<SpriteRenderer>();
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
            ANIM = GetComponent<AnimationScript>();
            if (ANIM == null)
            {

                ANIM = gameObject.AddComponent<AnimationScript>();
            }

            ANIM.Setup();
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

        myManager.CreateEvent(this, null, "death event", DieEvent, DeathStart,0);
    }
    protected bool isdoneDying = false;
    protected bool startedDeathAnimation = false;
    public virtual void DeathStart()
    {
       // Debug.Log("griddy death event starting");
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
           // Debug.Log("griddy fade out start");
        startedDeathAnimation = true;
        if (GetComponent<SpriteRenderer>())
        {
      
            Color subtract = new Color(0, 0, 0, 0.1f);
            int num = 0;
        
            while (mySR.color.a > 0)
            {
                num++;
                if (num > 999)
                {
                    Debug.Log("time expired");
                    break;
                }
                mySR.color = mySR.color - subtract;
                yield return null;
            }
            if (currentTile)
                currentTile.isOccupied = false;
            myManager.gridObjects.Remove(this);
            gameObject.SetActive(false);
            isdoneDying = true;
           // Debug.Log("griddy fade out end");
        }
      
    }


    public void GenericUnset()
    {
        isSetup = false;
        DEAD = false;
        STATS.Reset(true);
        BASE_STATS.Reset();
        STATS.HEALTH = BASE_STATS.MAX_HEALTH;

        if (GetComponent<AnimationScript>())
        {
            GetComponent<AnimationScript>().Unset();
        }
    }
}
