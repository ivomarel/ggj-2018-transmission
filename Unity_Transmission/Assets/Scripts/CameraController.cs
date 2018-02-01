using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float minHeight;
    public float maxHeight;

    Vector3 offset;

    Camera cam;

	// Use this for initialization
	void Start () {
        offset = new Vector3(0, transform.position.y, 0);
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {

        float ratio = PlayerController.instance.currentSpeed / PlayerController.instance.maxSpeed;
        offset.y = Mathf.Lerp(minHeight, maxHeight, ratio);

        transform.position = PlayerController.instance.transform.position + offset;
	}
}
