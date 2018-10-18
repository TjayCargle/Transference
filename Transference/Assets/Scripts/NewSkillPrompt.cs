using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NewSkillPrompt : MonoBehaviour {

    public Text text;
	// Use this for initialization
	void Start () {
		if(GetComponent<Text>())
        {
            text = GetComponent<Text>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
