using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolutionsUI : MonoBehaviour
{

	public Transform revMeter;
	[Range (-180f, 180f)]
	public float nullRevAngle = -90;
	[Range (90f, 360f)]
	public float RevRangeAngle = 270f;

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
		float minRev = PlayerController.instance.currentGear.minSpeed;
		float maxRev = PlayerController.instance.currentGear.maxSpeed;
		float revSpeed = Mathf.Abs (PlayerController.instance.getSpeed ());
		if (maxRev != minRev) {
			revMeter.transform.localEulerAngles = Vector3.back * nullRevAngle + (Vector3.back * RevRangeAngle * (revSpeed - minRev) / (maxRev - minRev));

		}
	}
}
