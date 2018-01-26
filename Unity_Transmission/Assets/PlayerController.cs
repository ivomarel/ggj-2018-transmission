using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float steeringSpeed;

    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.angularVelocity = new Vector3(0, horizontalInput * steeringSpeed *  verticalInput);

        Vector3 rotatedInput = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up) * new Vector3(0, 0, verticalInput);

        rb.velocity = rotatedInput * speed;
	}
}
