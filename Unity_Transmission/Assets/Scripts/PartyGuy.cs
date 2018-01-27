using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class PartyGuy : MonoBehaviour {

	public float stoppingDistance;
	public Blood bloodPrefab;
	AILerp agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<AILerp> ();
	}
	
	// Update is called once per frame
	void Update () {
		agent.destination = PlayerController.instance.transform.position;

		float d = Vector3.Distance (transform.position, PlayerController.instance.transform.position);
		if (d < stoppingDistance) {
			agent.isStopped = true;
		} else if (d > stoppingDistance * 1.1f){
			agent.isStopped = false;
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
