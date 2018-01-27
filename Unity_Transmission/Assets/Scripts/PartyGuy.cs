using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;
using Pathfinding.Examples;

public class PartyGuy : MonoBehaviour {

	public float stoppingDistance;
	public Blood bloodPrefab;
	RVOExampleAgent agent;

	float s;

	// Use this for initialization
	void Start () {
		agent = GetComponent<RVOExampleAgent> ();
		s = agent.maxSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		agent.SetTarget(PlayerController.instance.transform.position);

		float d = Vector3.Distance (transform.position, PlayerController.instance.transform.position);
		if (d < stoppingDistance) {
			agent.maxSpeed = 0;
		} else if (d > stoppingDistance * 1.1f){
			agent.maxSpeed = s;
		}
	}

	void OnCollisionEnter (Collision collisionInfo ) {
		PlayerController player = collisionInfo.collider.GetComponent<PlayerController> ();
		if (player) {
		//	Instantiate (bloodPrefab, transform.position, transform.rotation);

		//	Destroy (gameObject);
		}
	}
}
