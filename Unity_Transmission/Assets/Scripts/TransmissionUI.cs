using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class TransmissionUI : MonoBehaviour {

	public Transform knob;
	public int gearCount;
	public Vector3 gearBoxSize;
	public float gearBoxPadding;

	// The gear area the stick is in. we cannot switch directly to another gear
	private int currentGearArea = -1;

	private float gearRad;

	// Use this for initialization
	void Start () {
		gearRad = 360.0f / (float) gearCount * Mathf.Deg2Rad;
		gearBoxSize = GetComponent<RectTransform> ().sizeDelta / 2 + new Vector2(gearBoxPadding, gearBoxPadding);
	}
	
	// Update is called once per frame
	void Update () {
		GamePadState state = GamePad.GetState (PlayerController.instance.transmissionIndex);
		GamePadThumbSticks.StickValue transmissionInput = state.ThumbSticks.Right;

		if (inputStrength(transmissionInput) <= PlayerController.instance.freeGearThreshold) {
			//Debug.LogWarning ("Input strength" + inputStrength(transmissionInput));
			freeGearHandler (transmissionInput);
		} else {
			lockedGearHandler (transmissionInput);
		}
	}

	float getGearRad(int gear) {
		return gearRad + Mathf.PI / 2 - gear*gearRad;
	}

	float inputStrength(GamePadThumbSticks.StickValue input) {
		return Mathf.Sqrt (input.X * input.X + input.Y * input.Y);
	}

	float inputAngle(GamePadThumbSticks.StickValue input) {
		return Mathf.Atan2 (input.Y, input.X) % (2 * Mathf.PI);
	}

	void freeGearHandler(GamePadThumbSticks.StickValue input) {
		knob.localPosition = Vector3.Scale(new Vector3 (input.X, input.Y), gearBoxSize);
		PlayerController.instance.updateGear(-1);
		currentGearArea = -1;

		GamePad.SetVibration(PlayerController.instance.transmissionIndex, 0f, 0f);
	}

	void lockedGearHandler(GamePadThumbSticks.StickValue input) {
		if (currentGearArea == -1) {
			float normalizedAngle = (Mathf.PI / 2 + gearRad + 2 * Mathf.PI - inputAngle (input)) / gearRad;
			currentGearArea = Mathf.FloorToInt(normalizedAngle + 0.5f) % gearCount;
		}
	//	Debug.Log ("Closest gear area" + currentGearArea);

		float maxGearAreaAngle = getGearRad(currentGearArea) + PlayerController.instance.gearRestrictedRoamingProportion * gearRad / 2 ;
		float minGearAreaAngle = getGearRad(currentGearArea) - PlayerController.instance.gearRestrictedRoamingProportion * gearRad / 2 ;

		//Debug.Log(minGearAreaAngle + ", " + inputAngle (input) + ", " + maxGearAreaAngle);

		float clampedAngle = Mathf.Clamp(inputAngle(input) + 2*Mathf.PI, minGearAreaAngle + 2*Mathf.PI, maxGearAreaAngle + 2*Mathf.PI);

		setTransmissionRumble(inputAngle(input) + 2 * Mathf.PI, minGearAreaAngle + 2 * Mathf.PI, maxGearAreaAngle + 2 * Mathf.PI);

		knob.localPosition = Vector3.Scale(
			new Vector3 (
				Mathf.Cos(clampedAngle) * inputStrength(input),
				Mathf.Sin(clampedAngle) * inputStrength(input),
				0
			),
			gearBoxSize
		);

		if ( inputStrength(input) > PlayerController.instance.acceptedGearThreshold) {
			if (currentGearArea == -1) {
				Debug.LogError("Gear validated without valid Gear area !!!");
			}
			PlayerController.instance.updateGear(currentGearArea);
		}
	}

	void setTransmissionRumble(float angle, float min, float max) {
		float powerX = 0f;
		float powerY = 0f;
		if (angle < min) {
			powerY = min - angle * 0.2f;
		}
		if (angle > max) {
			powerX = angle - max * 0.2f;
		}

		// We match the gearbox display
		if ((angle % 2 * Mathf.PI - Mathf.PI) < 0) {
			float tmp = powerX;
			powerX = powerY;
			powerY = tmp;
		}

		GamePad.SetVibration(
			PlayerController.instance.transmissionIndex,
			powerX,
			powerY
		);
	}

	void SelectGear() {
	}
}
 