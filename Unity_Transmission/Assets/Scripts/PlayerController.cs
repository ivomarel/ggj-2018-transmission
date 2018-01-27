using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController> {

    /// <summary>
    /// The speed increase when fully pushing the controller
    /// </summary>
    public float speedIncrease;
    public float speedAutoDecrease;
    public float speedBreakDecrease;
    public float maxBackwardsSpeed;
    public float maxSpeed;

    internal float currentSpeed;

    [Header("Steering")]
    [Range(0, 4)]
    public int steerControlIndex;
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
        horizontalInput  = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Speeding();
        Steering();
	}

    void Speeding () {
        currentSpeed = rb.velocity.magnitude;

        currentSpeed -= speedAutoDecrease * Time.deltaTime;

        //When pressing forward, add gas
       // if (verticalInput > 0)
        {
            currentSpeed += speedIncrease * verticalInput * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxBackwardsSpeed, maxSpeed);

		Vector3 rotatedInput = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up) * new Vector3(0, 0, 1);

		//Actually applying to the RB
        rb.velocity = rotatedInput.normalized * currentSpeed;
    }

    void Steering()
    {
        currentSteerRotation += horizontalInput * steeringSpeed;
        currentSteerRotation = Mathf.Clamp(currentSteerRotation, -maxSteer, maxSteer);


        //Actually applying to the RB
        rb.angularVelocity = new Vector3(0, currentSteerRotation * currentSpeed);
    }

}
