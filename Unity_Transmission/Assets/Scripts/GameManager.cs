using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

	// Use this for initialization
	void Start () {
		AstarPath.active.Scan ();
		//PlayerController.instance.transform.position = new Vector3 (170, 0, 170);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
