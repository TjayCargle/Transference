using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NewSkillPrompt : MonoBehaviour {

    public Text text;

    [SerializeField]
    public Text choice1;

    [SerializeField]
    public Text choice2;

    void Start () {
		if(GetComponent<Text>())
        {
            text = GetComponent<Text>();
        }
	}
	

}
