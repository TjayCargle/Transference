using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnImageManager : MonoBehaviour
{
    public ManagerScript manager;
    public bool isSetup = false;
    [SerializeField]
    public GameObject imgBar;
    [SerializeField]
    private List<Image> turnImages = new List<Image>();
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
    public void LoadTurnImg(List<LivingObject> turnOrder)
    {

        for (int i = 0; i < turnOrder.Count; i++)
        {
            if (turnImages.Count > i)
            {
                turnImages[i].gameObject.SetActive(true);
                turnImages[i].GetComponentsInChildren<Image>()[1].GetComponent<ImgObj>().index = i;
                turnImages[i].GetComponentsInChildren<Image>()[1].sprite = turnOrder[i].FACE;
                turnImages[i].GetComponentsInChildren<Image>()[1].GetComponent<Button>().interactable = true;
            }
            else
            {
                GameObject baseObj = new GameObject();
                baseObj.transform.parent = imgBar.transform;
                Image baseImage = baseObj.gameObject.AddComponent<Image>();

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
                imgobject.manager = GameObject.FindObjectOfType<ManagerScript>();
                imgobject.index = i;
                Button selectButton = subObj.AddComponent<Button>();
                selectButton.onClick.AddListener(imgobject.SelectMe);

                turnImages.Add(baseImage);
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
                turnImages[i].color = Common.orange;
            }
            else
            {
                turnImages[i].color = Color.white;
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
