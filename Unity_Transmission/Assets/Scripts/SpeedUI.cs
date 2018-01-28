using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUI : MonoBehaviour
{


	public Transform speedMeter;
	[Range (-180f, 180f)]
	public float nullSpeedAngle = -90;
	[Range (90f, 360f)]
	public float speedRangeAngle = 270f;
	private float maxSpeed;

	// Use this for initialization
	void Start ()
	{
		maxSpeed = PlayerController.instance.gears [PlayerController.instance.gears.Length - 1].maxSpeed;
	}

	// Update is called once per frame
	void Update ()
	{
		float speed = Mathf.Abs (PlayerController.instance.getSpeed ());
		speedMeter.transform.localEulerAngles = Vector3.back * nullSpeedAngle + (Vector3.back * speed / maxSpeed * speedRangeAngle);
		//localRotation = Quaternion.Euler(new Vector3(0f, 0f, speed / maxSpeed * -180f));
		//speedMeter.localRotation = Quaternion.Euler(new Vector3(0f, 0f, speed / maxSpeed * -180f));
	}
}
