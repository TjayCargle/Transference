using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Expbar : MonoBehaviour
{

    public LivingObject currentUser;
    public bool updating = false;
    public Slider slider;
    public Text text;
    private void Start()
    {
        slider = GetComponent<Slider>();
        text = GetComponentInChildren<Text>();
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (updating)
        {
            if (currentUser)
            {
                if (slider.value < 100)
                {
                    if (slider.value < currentUser.BASE_STATS.EXP)
                    {
                        slider.value += 0.02f * Mathf.Max(1, Mathf.Abs(slider.value - currentUser.BASE_STATS.EXP) * 1.5f);
                        text.text = ((int)slider.value).ToString() + "/100";
                    }
                    if (slider.value >= 100)
                    {

                        if (currentUser.BASE_STATS.EXP >= 100)
                        {
                            currentUser.LevelUp();
                            slider.value = 0;
                        }
                    }
                    if (slider.value >= currentUser.BASE_STATS.EXP)
                    {
                        slider.value = currentUser.BASE_STATS.EXP;
                        text.text = ((int)slider.value).ToString() + "/100";
                        updating = false;
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
