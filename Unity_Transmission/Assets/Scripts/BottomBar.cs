using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomBar: Singleton<BottomBar>
{

	//public Text currrentSteerText;
	//public Text currrentGearText;

	public RectTransform steeringWheelUI;
	public float steeringWheelRotationMultiplier = 1;
	public Text transmissionPlayerFeedback;
	public Text speedPlayerFeedback;
	public Text steerPlayerFeedback;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		// currrentSteerText.text = PlayerController.instance.currentSteerRotation.ToString();
		//currrentGearText.text = PlayerController.instance.currentGearIndex.ToString ();

		steeringWheelUI.localEulerAngles = new Vector3 (0, 0, -PlayerController.instance.currentSteerRotation * steeringWheelRotationMultiplier);
	}

	public void updatePlayersControlsUI(int speedIndex, int steerIndex,int TransmissionIndex){
		speedPlayerFeedback.text = speedIndex.ToString();
		transmissionPlayerFeedback.text = TransmissionIndex.ToString();
		steerPlayerFeedback.text = steerIndex.ToString();
	}
}
