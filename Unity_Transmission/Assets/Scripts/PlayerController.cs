using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerController : Singleton<PlayerController> {

    /// <summary>
    /// The speed increase when fully pushing the controller
    /// </summary>

	//public AnimationCurve speedUpCurve;
	[Header("Speeding")]
	public PlayerIndex speedIndex;
    public float speedIncrease;
    public float speedAutoDecrease;
    public float speedBreakDecrease;
    public float maxBackwardsSpeed;
    public float maxSpeed;

    internal float currentSpeed;

    [Header("Steering")]
	public PlayerIndex steeringIndex;
	public float steeringSpeed;
	public float maxSteer;

    internal float currentSteerRotation;

    //Components
    Rigidbody rb;

    float horizontalInput;
    float verticalInput;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {
       // horizontalInput  = Input.GetAxis("Horizontal");
       // verticalInput = Input.GetAxis("Vertical");

        Speeding();
        Steering();
	}

    void Speeding () {
		GamePadState state = GamePad.GetState (speedIndex);

		verticalInput = state.ThumbSticks.Left.Y;

		currentSpeed = transform.InverseTransformVector(rb.velocity).z;

       // currentSpeed -= speedAutoDecrease * Time.deltaTime;

        //When pressing forward, add gas
       // if (verticalInput > 0)
        
        currentSpeed += speedIncrease * verticalInput * Time.deltaTime;
        

        currentSpeed = Mathf.Clamp(currentSpeed, -maxBackwardsSpeed, maxSpeed);

		//Actually applying to the RB
		rb.velocity =  transform.TransformVector( new Vector3(0,0, currentSpeed));
    }

    void Steering()
    {
		GamePadState state = GamePad.GetState (steeringIndex);

		horizontalInput = -state.Triggers.Left + state.Triggers.Right;

		currentSteerRotation += horizontalInput * steeringSpeed * Time.deltaTime;
        currentSteerRotation = Mathf.Clamp(currentSteerRotation, -maxSteer, maxSteer);


        //Actually applying to the RB
        rb.angularVelocity = new Vector3(0, currentSteerRotation * currentSpeed);
    }

}
