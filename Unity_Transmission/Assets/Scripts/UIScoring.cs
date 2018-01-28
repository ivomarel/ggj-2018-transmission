using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoring : MonoBehaviour {

	private Slider slider;

	// Use this for initialization
	void Start () {
		slider = this.GetComponent<Slider> ();
	}

	// Update is called once per frame
	void Update () {
		slider.value = PlayerController.instance.getPlayerScore() * 100;
	}
}
