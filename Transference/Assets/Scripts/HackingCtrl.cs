using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HackingCtrl : MonoBehaviour
{
    //currentChipType == SubSkillType.None = silver
    //currentChipType == SubSkillType.Item = blue
    //currentChipType == SubSkillType.Movement = green

    public bool isSetup = false;
    private List<SubSkillType> defaultList = new List<SubSkillType>();
    public LivingObject currentObj;
    public int currentChip = 0;
    public SubSkillType currentChipType = SubSkillType.Strike;
    private ManagerScript manager;
    public List<SubSkillType> currentSequence = new List<SubSkillType>();
    [SerializeField]
    TextMeshProUGUI headerText;

    [SerializeField]
    Image[] accessChips;

    [SerializeField]
    Sprite[] targetSprites;

    [SerializeField]
    Image currentTarget;

    [SerializeField]
    Image nextTarget;

    [SerializeField]
    Image thirdTarget;

    [SerializeField]
    TextMeshProUGUI strikeText;

    public int strikeCount = 3;

    [SerializeField]
    TextMeshProUGUI skillText;

    public int skillCount = 3;

    [SerializeField]
    TextMeshProUGUI spellText;

    [SerializeField]
    Image playerImage;

    public int spellCount = 3;

    public float countDown = 30;

    public bool isPlaying = false;
    public bool won = false;
    private bool instaFail = false;

    public void Setup()
    {
        if (!isSetup)
        {
            defaultList.Add(SubSkillType.None);
            defaultList.Add(SubSkillType.Strike);
            defaultList.Add(SubSkillType.Movement);
            defaultList.Add(SubSkillType.Spell);
            defaultList.Add(SubSkillType.Item);
            defaultList.Add(SubSkillType.Skill);

            LoadSequence(defaultList);
            updateTarget();
            isSetup = true;
            manager = GameObject.FindObjectOfType<ManagerScript>();
        }
    }
    private void Start()
    {
        Setup();
    }

    public void LoadSequence(List<SubSkillType> aSequence)
    {
        if (accessChips.Length == 0)
            return;
        instaFail = false;
        currentSequence.Clear();
        for (int i = 0; i < accessChips.Length; i++)
        {
            if (i < aSequence.Count)
            {
                currentSequence.Add(aSequence[i]);
                accessChips[i].transform.parent.gameObject.SetActive(true);
                accessChips[i].color = Common.denied;

            }
            else
            {
                accessChips[i].transform.parent.gameObject.SetActive(false);
            }
        }

        currentChip = 0;
        currentChipType = currentSequence[0];
        updateTarget();
 
    }

    private void updateTarget()
    {
        return;
        SubSkillType secondChip = SubSkillType.Heal;
        SubSkillType thirdChip = SubSkillType.Heal;
        if ((currentChip + 1) < currentSequence.Count)
            secondChip = currentSequence[(currentChip + 1)];

        if ((currentChip + 2) < currentSequence.Count)
            thirdChip = currentSequence[(currentChip + 2)];

        switch (currentChipType)
        {
            case SubSkillType.Strike:
                {
                    currentTarget.sprite = targetSprites[0];
                }
                break;
            case SubSkillType.Skill:
                {
                    currentTarget.sprite = targetSprites[1];
                }
                break;
            case SubSkillType.Spell:
                {
                    currentTarget.sprite = targetSprites[2];
                }
                break;
            case SubSkillType.Item:
                {
                    currentTarget.sprite = targetSprites[3];
                }
                break;
            case SubSkillType.Movement:
                {
                    currentTarget.sprite = targetSprites[4];
                }
                break;
            case SubSkillType.Ailment:
                {
                    currentTarget.sprite = targetSprites[5];
                }
                break;
            case SubSkillType.None:
                {
                    currentTarget.sprite = targetSprites[6];
                }
                break;

        }
        nextTarget.color = Common.semi;
        switch (secondChip)
        {
            case SubSkillType.Strike:
                {
                    nextTarget.sprite = targetSprites[0];
                }
                break;
            case SubSkillType.Skill:
                {
                    nextTarget.sprite = targetSprites[1];
                }
                break;
            case SubSkillType.Spell:
                {
                    nextTarget.sprite = targetSprites[2];
                }
                break;
            case SubSkillType.Item:
                {
                    nextTarget.sprite = targetSprites[3];
                }
                break;
            case SubSkillType.Movement:
                {
                    nextTarget.sprite = targetSprites[4];
                }
                break;
            case SubSkillType.Ailment:
                {
                    nextTarget.sprite = targetSprites[5];
                }
                break;
            case SubSkillType.None:
                {
                    nextTarget.sprite = targetSprites[6];
                }
                break;
            default:
                {
                    nextTarget.color = Common.trans;
                    nextTarget.sprite = null;
                }
                break;

        }
        thirdTarget.color = Common.moresemi;
        switch (thirdChip)
        {
            case SubSkillType.Strike:
                {
                    thirdTarget.sprite = targetSprites[0];
                }
                break;
            case SubSkillType.Skill:
                {
                    thirdTarget.sprite = targetSprites[1];
                }
                break;
            case SubSkillType.Spell:
                {
                    thirdTarget.sprite = targetSprites[2];
                }
                break;
            case SubSkillType.Item:
                {
                    thirdTarget.sprite = targetSprites[3];
                }
                break;
            case SubSkillType.Movement:
                {
                    thirdTarget.sprite = targetSprites[4];
                }
                break;
            case SubSkillType.Ailment:
                {
                    thirdTarget.sprite = targetSprites[5];
                }
                break;
            case SubSkillType.None:
                {
                    thirdTarget.sprite = targetSprites[6];
                }
                break;
            default:
                {
                    thirdTarget.color = Common.trans;
                    thirdTarget.sprite = null;
                }
                break;

        }
    }
    public void CheckForEnd()
    {
       

        if (currentChip >= currentSequence.Count)
        {
            if (instaFail == true)
            {
                won = false;
                isPlaying = false;
                gameObject.SetActive(false);
            }
            else
            {

            won = true;
            isPlaying = false;
            gameObject.SetActive(false);
            }
        }
        else if (strikeCount == 0 && skillCount == 0 && spellCount == 0)
        {
            won = false;
            isPlaying = false;
            gameObject.SetActive(false);
        }
        else if (countDown <= 0)
        {
            won = false;
            isPlaying = false;
            gameObject.SetActive(false);
        }



    }
    public void useStrike(HackingButton caller)
    {
        if (currentChip >= accessChips.Length)
        {
            return;
        }
        if (strikeCount > 0)
        {
            strikeCount--;

            if (currentChipType == SubSkillType.Strike || currentChipType == SubSkillType.None || currentChipType == SubSkillType.Movement || currentChipType == SubSkillType.Ailment)
            {
                accessChips[currentChip].color = Common.granted;
                currentChip++;
                if (currentChip < currentSequence.Count)
                    currentChipType = currentSequence[currentChip];
                updateTarget();
            
                if(manager)
                {
                    manager.PlayOppSnd();
                }
            }
            else
            {
                accessChips[currentChip].color = Common.errored;
                currentChip++;
                if (currentChip < currentSequence.Count)
                    currentChipType = currentSequence[currentChip];
                instaFail = true;
                updateTarget();
                if (manager)
                {
                    manager.PlayExitSnd();
                }
            }
        }
        if (strikeCount <= 0)
        {
            if (caller.GetComponent<Button>())
            {
                caller.GetComponent<Button>().interactable = false;
            }
        }
        CheckForEnd();
    }



    public void useSkill(HackingButton caller)
    {
        if (currentChip >= accessChips.Length)
        {
            return;
        }
        if (skillCount > 0)
        {
            skillCount--;
            if (currentChipType != SubSkillType.Strike && currentChipType != SubSkillType.Spell && currentChipType != SubSkillType.Ailment)
            {
                accessChips[currentChip].color = Common.granted;
                currentChip++;
                if (currentChip < currentSequence.Count)
                    currentChipType = currentSequence[currentChip];

                updateTarget();
                if (manager)
                {
                    manager.PlayOppSnd();
                }
            }
            else
            {
                accessChips[currentChip].color = Common.errored;
                currentChip++;
                if (currentChip < currentSequence.Count)
                    currentChipType = currentSequence[currentChip];
                instaFail = true;
                updateTarget();
                if (manager)
                {
                    manager.PlayExitSnd();
                }

            }
        }
        if (skillCount <= 0)
        {
            if (caller.GetComponent<Button>())
            {
                caller.GetComponent<Button>().interactable = false;
            }
        }
        CheckForEnd();
    }

    public void useSpell(HackingButton caller)
    {
        if (currentChip >= accessChips.Length)
        {
            return;
        }
        if (spellCount > 0)
        {
            spellCount--;

            if (currentChipType == SubSkillType.Spell || currentChipType == SubSkillType.None || currentChipType == SubSkillType.Item || currentChipType == SubSkillType.Ailment)
            {
                accessChips[currentChip].color = Common.granted;
                currentChip++;
                if (currentChip < currentSequence.Count)
                    currentChipType = currentSequence[currentChip];
                updateTarget();
                if (manager)
                {
                    manager.PlayOppSnd();
                }
            }
            else
            {
                accessChips[currentChip].color = Common.errored;
                currentChip++;
                if (currentChip < currentSequence.Count)
                    currentChipType = currentSequence[currentChip];
                instaFail = true;
                updateTarget();
                if (manager)
                {
                    manager.PlayExitSnd();
                }
            }
        }
        if (spellCount <= 0)
        {
            if (caller.GetComponent<Button>())
            {
                caller.GetComponent<Button>().interactable = false;
            }
        }

        CheckForEnd();
    }

    private void Update()
    {
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;

        }
        else
        {
            CheckForEnd();
        }
        if (currentObj)
        {
            if (playerImage)
            {
                playerImage.sprite = currentObj.GetComponent<SpriteRenderer>().sprite;
            }
        }
        if (headerText)
        {
            headerText.text = ((int)countDown).ToString();
        }
        if (strikeText)
        {
            strikeText.text = "Strikes:" + strikeCount.ToString();
        }
        if (skillText)
        {
            skillText.text = "Skills:" + skillCount.ToString();
        }
        if (spellText)
        {
            spellText.text = "Spells:" + spellCount.ToString();
        }
    }
}