using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController> {

   	/// <summary>
    /// The speed increase when fully pushing the controller
    /// </summary>

	[Header("Gears")]
	public PlayerIndex gearPlayerIndex;
	public Text transmissionPlayerFeedback;
	public Gear[] gears;

	[Range(0,1)]
	public float acceptedGearThreshold = 0.8f;
	[Range(0,1)]
	public float freeGearThreshold = 0.4f;
	[Range(0,1)]
	public float gearRestrictedRoamingProportion = 0.15f;

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
	[Header("Speeding")]
	public PlayerIndex speedPlayerIndex;
	public Text speedPlayerFeedback;
    public float speedIncrease;
    public float speedAutoDecrease;
    public float speedBreakDecrease;
    public float maxBackwardsSpeed;
   // public float maxSpeed;

    internal float currentSpeed;

    [Header("Steering")]
	public PlayerIndex steerPlayerIndex;
	public Text steerPlayerFeedback;
	public float steeringSpeed;
	public float steerAutoDecrease;
	public float maxSteer;

    internal float currentSteerRotation;

    //Components
    Rigidbody rb;

	ButtonState prevState;

    float horizontalInput;
    float verticalInput;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
		updatePlayersPos();
	}


	// Update is called once per frame
	void Update () {
        Speeding();
        Steering();
		 // FIXME should not be done every update when menu is setup
	}

	private void updatePlayersPos() {
		print(speedPlayerIndex.ToString());
		speedPlayerFeedback.text = speedPlayerIndex.ToString();
		steerPlayerFeedback.text = steerPlayerIndex.ToString();
		transmissionPlayerFeedback.text = gearPlayerIndex.ToString();
	}

	public void updateGear(int gear) {
		currentGearIndex = gear;
	}

	public float getSpeed(){
		return currentSpeed;
	}

    void Speeding () {
		if (keyboardMode) {
			verticalInput = Input.GetAxis ("Vertical");
		}else{
			GamePadState state = GamePad.GetState (speedPlayerIndex);

			verticalInput = state.Triggers.Right;
		}
	//	verticalInput = Mathf.Max (0, verticalInput);

		currentSpeed = transform.InverseTransformVector(rb.velocity).z;

		currentSpeed *= (1-speedAutoDecrease * Time.deltaTime);

		if (!currentGear.isNeutral) {
			//Only adding gas when not neutral
			//0 is reverse
			if (currentGearIndex == 0) {
				currentSpeed -= speedIncrease * verticalInput * Time.deltaTime;
			} else if (currentSpeed >= currentGear.minSpeed && currentSpeed <= currentGear.maxSpeed) {
				currentSpeed += speedIncrease * verticalInput * Time.deltaTime;
			}
		}
		//currentSpeed = Mathf.Clamp(currentSpeed, -maxBackwardsSpeed, currentGear.maxSpeed);

		//Actually applying to the RB
		rb.velocity =  transform.TransformVector( new Vector3(0,0, currentSpeed));
    }

    void Steering()
	{	
		if (keyboardMode) {
			horizontalInput = Input.GetAxis ("Horizontal");
		} else {
			GamePadState state = GamePad.GetState (steerPlayerIndex);

			horizontalInput = state.ThumbSticks.Left.X;
		}
		//if (Mathf.Abs (horizontalInput) < 0.1f) {
		currentSteerRotation *= (1-steerAutoDecrease * Time.deltaTime);
		//}

		currentSteerRotation += horizontalInput * steeringSpeed * Time.deltaTime;
        currentSteerRotation = Mathf.Clamp(currentSteerRotation, -maxSteer, maxSteer);


        //Actually applying to the RB
        rb.angularVelocity = new Vector3(0, currentSteerRotation * currentSpeed);
    }


}

[System.Serializable]
public struct Gear  {
	public float minSpeed;
	public float maxSpeed;
	public float speedIncrease;
	internal bool isNeutral;
}