using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PartyGuy : MonoBehaviour {

	public Blood bloodPrefab;
	NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		agent.SetDestination (PlayerController.instance.transform.position);


	}

	void OnCollisionEnter (Collision collisionInfo ) {
		PlayerController player = collisionInfo.collider.GetComponent<PlayerController> ();
		if (player) {
			Instantiate (bloodPrefab, transform.position, transform.rotation);

			Destroy (gameObject);
		}
	}
}
