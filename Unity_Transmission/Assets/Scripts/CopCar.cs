using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.AI;

public class CopCar : MonoBehaviour
{

	internal NavMeshAgent agent;
	// Use this for initialization
	void Start ()
	{
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		agent.SetDestination (PlayerController.instance.transform.position);
		/*
		Vector3 dest = PlayerController.instance.transform.position;
		dest.y = transform.position.y;
		agent.destination = dest;
		*/
	}
}
