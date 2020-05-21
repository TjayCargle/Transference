using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowObject : MonoBehaviour
{
    public bool isSetup = false;
    public bool detached = false;

    private GridObject owner;
    private LivingObject realOwner;
    private SpriteObject mySprite;
    private AnimationScript script;
    private Animator shadowAnimator;

    public GridObject USER
    {
        get { return owner; }
        set { owner = value; }

    }


    public LivingObject REALUSER
    {
        get { return realOwner; }
        set { realOwner = value; }

    }

    public AnimationScript SCRIPT
    {
        get { return script; }
        set { script = value; }

    }

    public SpriteObject SPRITE
    {
        get { return mySprite; }
        set { mySprite = value; }

    }

    public Animator ANIM
    {
        get { return shadowAnimator; }
        set { shadowAnimator = value; }

    }


    public void Setup()
    {
        if (!isSetup)
        {


            if (USER)
            {
                name = USER.NAME + "Shadow";
                transform.parent = USER.transform;
                transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
                transform.localPosition = new Vector3(0, 0, 0.1f);

            }

            mySprite = GetComponent<SpriteObject>();
            if (mySprite == null)
            {
                mySprite = gameObject.AddComponent<SpriteObject>();

            }

            script = GetComponent<AnimationScript>();
            if (script == null)
            {
                script = gameObject.AddComponent<AnimationScript>();
            }

            shadowAnimator = GetComponent<Animator>();
            if (shadowAnimator == null)
            {
                shadowAnimator = gameObject.AddComponent<Animator>();
            }

            script.obj = USER;
            mySprite.Setup();


            script.render = mySprite.sr;
            script.me = realOwner;

            script.Setup();
            isSetup = true;
        }
    }

    public void UpdateRealOwner(LivingObject living)
    {
        if(!isSetup)
        {
            Setup();
        }
        realOwner = living;
        script.me = living;
    }
    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    
}
