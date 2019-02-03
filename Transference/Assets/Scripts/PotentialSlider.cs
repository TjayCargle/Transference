
using UnityEngine;
using UnityEngine.UI;
public class PotentialSlider : MonoBehaviour {
    [SerializeField]
    Image copyImg;
    [SerializeField]
    Slider copySLider;
    public bool pulsing;
    private Color myColor;
    private int direction = -1;
    RectTransform rect;
    // Use this for initialization
    void Start () {
        myColor = GetComponent<Image>().color;
        rect = GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
		
   
            if(direction == -1)
            {
                if (myColor.a - Time.deltaTime >= 0.0f)
                    myColor.a -= Time.deltaTime;
                else
                    direction = 1;
            }
            if (direction == 1)
            {
                if (myColor.a + Time.deltaTime <= 1.0f)
                    myColor.a += Time.deltaTime;
                else
                    direction = -1;
            }
            GetComponent<Image>().color = myColor;
        
	}
    public void anchor()
    {
        if(!rect)
        {
            rect = GetComponent<RectTransform>();
        }
        if(rect)
        {
        rect.transform.localPosition = copySLider.fillRect.localPosition;
        rect.anchorMin = copySLider.fillRect.anchorMin;
        rect.anchorMax = copySLider.fillRect.anchorMax;
        rect.offsetMax = copySLider.fillRect.offsetMax;
        rect.offsetMin = copySLider.fillRect.offsetMin;
        pulsing = false;
        }
    }
}
