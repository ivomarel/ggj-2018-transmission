using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.AI;

public class CopCar : MonoBehaviour
{
	public AudioSource source;
	internal NavMeshAgent agent;

	public AudioClip[] sounds;
	// Use this for initialization
	void Start ()
	{
		agent = GetComponent<NavMeshAgent> ();
		source = GetComponent<AudioSource> ();

		InvokeRepeating ("PlayRandomSound", 8, 8);

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

	void PlayRandomSound ()
	{
		int r = Random.Range (0, sounds.Length);
		source.PlayOneShot (sounds [r]);
	}
}
