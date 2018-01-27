using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomBar : MonoBehaviour {

    public Text currrentSteerText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        currrentSteerText.text = PlayerController.instance.currentSteerRotation.ToString();
	}
}
