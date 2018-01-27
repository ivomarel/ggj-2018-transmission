using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerController : Singleton<PlayerController> {

   	/// <summary>
    /// The speed increase when fully pushing the controller
    /// </summary>

	[Header("Gears")]
	public PlayerIndex gearPlayerIndex;
	public Gear[] gears;

	internal int currentGearIndex;

	Gear currentGear {
		get {
			return gears [currentGearIndex];
		}
	}

	//public AnimationCurve speedUpCurve;
	[Header("Speeding")]
	public PlayerIndex speedPlayerIndex;
    public float speedIncrease;
    public float speedAutoDecrease;
    public float speedBreakDecrease;
    public float maxBackwardsSpeed;
   // public float maxSpeed;

    internal float currentSpeed;

    [Header("Steering")]
	public PlayerIndex steerPlayerIndex;
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

	}
	
	// Update is called once per frame
	void Update () {

        Speeding();
        Steering();
		TempGearing ();

	}

	void TempGearing () {
		GamePadState state = Xbox.GetState (speedPlayerIndex);
		GamePadState prevState = Xbox.GetPrevState (speedPlayerIndex);

		if (state.Buttons.Y == ButtonState.Pressed && prevState.Buttons.Y == ButtonState.Released) {
			if (currentGearIndex < gears.Length - 1) {
				currentGearIndex++;
			}
		}

		if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released) {
			
			if (currentGearIndex > 0) {
				currentGearIndex--;
			}
		}


	}

    void Speeding () {
		GamePadState state = GamePad.GetState (speedPlayerIndex);

		verticalInput = state.Triggers.Right;
	//	verticalInput = Mathf.Max (0, verticalInput);

		currentSpeed = transform.InverseTransformVector(rb.velocity).z;

		currentSpeed *= (1-speedAutoDecrease * Time.deltaTime);

        //Only adding gas when 
		if (currentGearIndex == 0) {
			currentSpeed -= speedIncrease * verticalInput * Time.deltaTime;
		} else if (currentSpeed >= currentGear.minSpeed && currentSpeed <= currentGear.maxSpeed){
			currentSpeed += speedIncrease * verticalInput * Time.deltaTime;
		}

		//currentSpeed = Mathf.Clamp(currentSpeed, -maxBackwardsSpeed, currentGear.maxSpeed);

		//Actually applying to the RB
		rb.velocity =  transform.TransformVector( new Vector3(0,0, currentSpeed));
    }

    void Steering()
    {
		GamePadState state = GamePad.GetState (steerPlayerIndex);

		horizontalInput = state.ThumbSticks.Left.X;

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
}