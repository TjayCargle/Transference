using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TurnImageManager : MonoBehaviour
{
    public ManagerScript manager;
    public bool isSetup = false;
    [SerializeField]
    public GameObject imgBar;
    [SerializeField]
    private List<TUrnImg> turnImages = new List<TUrnImg>();
    public void Setup()
    {
        if (!isSetup)
        {
            if (!manager)
                manager = GetComponent<ManagerScript>();
            isSetup = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }
    public void UpdateTurnImgActionCount(List<LivingObject> turnOrder)
    {
        for (int i = 0; i < turnOrder.Count; i++)
        {
            if (turnImages.Count > i)
            {
                turnImages[i].gameObject.SetActive(true);
                turnImages[i].face.GetComponent<ImgObj>().index = i;
                turnImages[i].face.sprite = turnOrder[i].FACE;
                turnImages[i].hiddenFace.sprite = turnOrder[i].FACE;
                turnImages[i].uGUI.text = "" + turnOrder[i].ACTIONS;
                turnImages[i].face.GetComponent<Button>().interactable = true;
            }
        }
        if (turnImages.Count > turnOrder.Count)
        {
            for (int i = 0; i < turnImages.Count; i++)
            {
                if (i > turnOrder.Count - 1)
                {
                    turnImages[i].gameObject.SetActive(false);
                }
            }
        }
    }
    public void LoadTurnImg(List<LivingObject> turnOrder)
    {

        for (int i = 0; i < turnOrder.Count; i++)
        {
            if (turnImages.Count > i)
            {
                turnImages[i].gameObject.SetActive(true);
                turnImages[i].face.GetComponent<ImgObj>().index = i;
                turnImages[i].face.sprite = turnOrder[i].FACE;
                turnImages[i].hiddenFace.sprite = turnOrder[i].FACE;
                turnImages[i].uGUI.text = "" + turnOrder[i].ACTIONS;
                turnImages[i].face.GetComponent<Button>().interactable = true;
            }
            else
            {
                GameObject baseObj = new GameObject();
                baseObj.transform.parent = imgBar.transform;
                Image baseImage = baseObj.gameObject.AddComponent<Image>();
                TUrnImg timg = baseObj.gameObject.AddComponent<TUrnImg>();
                timg.backImg = baseImage;
                HorizontalLayoutGroup noBiz = baseImage.gameObject.AddComponent<HorizontalLayoutGroup>();
                noBiz.childControlHeight = true;
                noBiz.childControlWidth = true;
                noBiz.childForceExpandHeight = true;
                noBiz.childForceExpandWidth = true;

                GameObject subObj = new GameObject();
                subObj.transform.parent = baseObj.transform;
                Image subImage = subObj.AddComponent<Image>();
                subImage.sprite = turnOrder[i].FACE;

                ImgObj imgobject = subObj.AddComponent<ImgObj>();
                imgobject.manager = manager;
                imgobject.index = i;
                Button selectButton = subObj.AddComponent<Button>();
                selectButton.onClick.AddListener(imgobject.SelectMe);

                timg.face = subImage;

                GameObject actionSubObj = new GameObject();
                actionSubObj.transform.parent = baseObj.transform;
                Image actionSubImage = actionSubObj.AddComponent<Image>();
                actionSubImage.sprite = turnOrder[i].FACE;
                actionSubImage.color = Common.trans;

                timg.hiddenFace = actionSubImage;

                GameObject textSubObj = new GameObject();
                textSubObj.transform.parent = actionSubObj.transform;

                TextMeshProUGUI uGUI = textSubObj.AddComponent<TextMeshProUGUI>();
                uGUI.rectTransform.anchorMin = Vector2.zero;
                uGUI.rectTransform.anchorMax = new Vector2(1, 1);
                uGUI.rectTransform.offsetMin = Vector2.zero;
                uGUI.rectTransform.offsetMax = Vector2.zero;
                uGUI.alignment = TextAlignmentOptions.Center;
                uGUI.text = "" + turnOrder[i].ACTIONS;
                uGUI.color = Color.black;

                timg.uGUI = uGUI;
                turnImages.Add(timg);
            }
        }
        if (turnImages.Count > turnOrder.Count)
        {
            for (int i = 0; i < turnImages.Count; i++)
            {
                if (i > turnOrder.Count - 1)
                {
                    turnImages[i].gameObject.SetActive(false);
                }
            }
        }
    }
    public void UpdateSelection(int orderIndex)
    {
        if (orderIndex > turnImages.Count)
            return;
        for (int i = 0; i < turnImages.Count; i++)
        {
            if (i == orderIndex)
            {
                turnImages[i].backImg.color = Common.orange;
            }
            else
            {
                turnImages[i].backImg.color = Color.white;
            }
        }
    }

    public void UpdateInteractivity(int orderIndex)
    {
        if (turnImages.Count == 0)
            return;
        if (orderIndex > turnImages.Count)
            return;
        turnImages[orderIndex].GetComponentsInChildren<Image>()[1].GetComponent<Button>().interactable = false;

    }


}
