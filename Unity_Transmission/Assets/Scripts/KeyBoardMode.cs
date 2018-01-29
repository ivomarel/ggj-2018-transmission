using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardMode : MonoBehaviour {

    public bool enabledInKeyboard;

	// Use this for initialization
	void Start () {
        gameObject.SetActive(enabledInKeyboard == GameManager.keyboardMode);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
