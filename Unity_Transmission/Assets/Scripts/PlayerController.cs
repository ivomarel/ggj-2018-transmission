using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{

	/// <summary>
	/// The speed increase when fully pushing the controller
	/// </summary>

	[Header ("Gears")]
	public PlayerIndex gearPlayerIndex;
	public Text transmissionPlayerFeedback;
	public Gear[] gears;

	[Range (0, 1)]
	public float acceptedGearThreshold = 0.8f;
	[Range (0, 1)]
	public float freeGearThreshold = 0.4f;
	[Range (0, 1)]
	public float gearRestrictedRoamingProportion = 0.15f;

	public float badGearRumbleMultiplier;

	internal int currentGearIndex;


	public Gear currentGear {
		get {
			if (currentGearIndex == -1) {
				if (keyboardMode) {
					return gears [1];
				}
				return new Gear () {
					isNeutral = true
				};
			}
			return gears [currentGearIndex];
		}
	}

	public bool keyboardMode;

	//public AnimationCurve speedUpCurve;
	[Header ("Speeding")]
	public PlayerIndex speedPlayerIndex;
	public Text speedPlayerFeedback;
	public float speedIncrease;
	public float speedAutoDecrease;
	public float speedBreakDecrease;
	public float maxBackwardsSpeed;
	// public float maxSpeed;

	internal float currentSpeed;

	[Header ("Steering")]
	public PlayerIndex steerPlayerIndex;
	public Text steerPlayerFeedback;
	public float steeringSpeed;
	public float steerAutoDecrease;
	public float maxSteer;

	private float score;
	private float maxScore = 100f;

	internal float currentSteerRotation;

	//Components
	Rigidbody rb;

	ButtonState prevState;

	float horizontalInput;
	float verticalInput;

	private PartyGuy[] guys;

	// Use this for initialization
	IEnumerator Start ()
	{
		rb = GetComponent<Rigidbody> ();
		updatePlayersPos ();
		yield return null;
		yield return null;
		guys = FindObjectsOfType<PartyGuy> ();
	}


	// Update is called once per frame
	void Update ()
	{
		Speeding ();
		Steering ();

		UpdateScore ();
		// FIXME should not be done every update when menu is setup
	}

	void UpdateScore ()
	{
		if (guys == null)
			return;
		
		float converted = 0;
		float total = 0;
		foreach (PartyGuy guy in guys) {
			if (guy == null)
				continue;
			if (guy.currentState == PartyGuy.State.Trancing) {
				converted++;
			}
			total++;
		}
		score = converted;
		maxScore = total;
	}

	public float getPlayerScore ()
	{
		return score / maxScore;
	}

	private void updatePlayersPos ()
	{
		print (speedPlayerIndex.ToString ());
		if (speedPlayerFeedback != null) {
			speedPlayerFeedback.text = speedPlayerIndex.ToString ();
			steerPlayerFeedback.text = steerPlayerIndex.ToString ();
			transmissionPlayerFeedback.text = gearPlayerIndex.ToString ();
		}
	}

	public void updateGear (int gear)
	{
		currentGearIndex = gear;
	}

	public float getSpeed ()
	{
		return currentSpeed;
	}

	void Speeding ()
	{
		if (keyboardMode) {
			verticalInput = Input.GetAxis ("Vertical");
		} else {
			GamePadState state = GamePad.GetState (speedPlayerIndex);

			verticalInput = state.Triggers.Right;
		}
		//	verticalInput = Mathf.Max (0, verticalInput);

		currentSpeed = transform.InverseTransformVector (rb.velocity).z;

		currentSpeed *= (1 - speedAutoDecrease * Time.deltaTime);

		if (!currentGear.isNeutral) {
			//Only adding gas when not neutral
			//0 is reverse
			if (currentGearIndex == 0) {
				currentSpeed -= speedIncrease * verticalInput * Time.deltaTime;
			} else if (currentSpeed >= currentGear.minSpeed && currentSpeed <= currentGear.maxSpeed) {
				currentSpeed += speedIncrease * verticalInput * Time.deltaTime;
				GamePad.SetVibration (gearPlayerIndex, 0f, 0f);
			} else {
				// Rumble we reach the max or min speed, gear is not fit
				setClutchRumble (currentSpeed, currentGear.minSpeed, currentGear.maxSpeed);
			}
		} else {
			GamePad.SetVibration (gearPlayerIndex, 0f, 0f);
		}
		//currentSpeed = Mathf.Clamp(currentSpeed, -maxBackwardsSpeed, currentGear.maxSpeed);

		//Actually applying to the RB
		rb.velocity = transform.TransformVector (new Vector3 (0, 0, currentSpeed));
	}

	void setClutchRumble (float speed, float minSpeed, float maxSpeed)
	{
		float powerX = 0f;
		float powerY = 0f;
		if (speed < minSpeed) {
			powerX = minSpeed - speed;
		}
		if (maxSpeed < speed) {
			powerY = speed - maxSpeed;
		}
		GamePad.SetVibration (gearPlayerIndex, powerX, powerY);
	}

	void Steering ()
	{	
		if (keyboardMode) {
			horizontalInput = Input.GetAxis ("Horizontal");
		} else {
			GamePadState state = GamePad.GetState (steerPlayerIndex);

			horizontalInput = state.ThumbSticks.Left.X;
		}
		//if (Mathf.Abs (horizontalInput) < 0.1f) {
		currentSteerRotation *= (1 - steerAutoDecrease * Time.deltaTime);
		//}

		currentSteerRotation += horizontalInput * steeringSpeed * Time.deltaTime;
		currentSteerRotation = Mathf.Clamp (currentSteerRotation, -maxSteer, maxSteer);


		//Actually applying to the RB
		rb.angularVelocity = new Vector3 (0, currentSteerRotation * currentSpeed);
	}


}

[System.Serializable]
public struct Gear
{
	public float minSpeed;
	public float maxSpeed;
	public float speedIncrease;
	internal bool isNeutral;
}