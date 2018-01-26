using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = new Vector3(0, transform.position.y, 0);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = PlayerController.instance.transform.position + offset;
	}
}
