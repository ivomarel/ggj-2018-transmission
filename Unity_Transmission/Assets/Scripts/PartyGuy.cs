using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;
using Pathfinding.Examples;

public class PartyGuy : MonoBehaviour
{
	
	public float conversionTime;
	public float hitVelocityToDie;
	public float speed;
	public float rotationSpeed;
	public float stoppingDistance;
	public Blood bloodPrefab;

	public enum State
	{
		Trumping,
		BeingConverted,
		Trancing
	}

	internal State currentState;

	float realSpeed;

	private Rigidbody rb;
	private SpriteRenderer sr;
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody> ();	
		sr = GetComponentInChildren<SpriteRenderer> ();
		SetState (State.Trumping);
	}

	public void SetState (State newState)
	{
		StopAllCoroutines ();
		currentState = newState;
		switch (currentState) {
		case State.Trumping:
			StartCoroutine (OnTrumping ());
			break;
		case State.BeingConverted:
			StartCoroutine (OnConverting ());
			break;
		case State.Trancing:
			StartCoroutine (OnTrancing ());
			break;
		}
	}

	IEnumerator OnTrumping ()
	{
		sr.color = Color.white;
		Vector3 currentDest = Vector3.zero;
		float walkTime = 3;
		float walkTimer = Mathf.Infinity;
		while (true) {
			if (walkTimer >= walkTime) {
				walkTimer = 0;
				walkTime = Random.Range (3f, 5f);
				currentDest = transform.position + new Vector3 (Random.Range (-5f, 5f), 0, Random.Range (-5f, 5f));
			}
			walkTimer += Time.fixedDeltaTime;
			WalkToDestination (currentDest);

			yield return new WaitForFixedUpdate ();
		}
	}

	IEnumerator OnConverting ()
	{
		float timer = 0;
		while (timer < conversionTime) {
			timer += Time.fixedDeltaTime;
			sr.color = Color.Lerp (Color.white, Color.red, timer / conversionTime);
			yield return new WaitForFixedUpdate ();
		}

		SetState (State.Trancing);
	}

	IEnumerator OnTrancing ()
	{
		while (true) {
			WalkToDestination (PlayerController.instance.transform.position);
			yield return new WaitForFixedUpdate ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void WalkToDestination (Vector3 dest)
	{
		Vector3 dir = dest - transform.position;
		dir.y = 0;

		float d = dir.sqrMagnitude;
		if (d < stoppingDistance * stoppingDistance) {
			realSpeed = 0;
		} else if (d > stoppingDistance * stoppingDistance * 1.1f) {
			realSpeed = speed;
		}

		rb.velocity = realSpeed * dir.normalized;
		transform.rotation = (Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (dir), rotationSpeed));
	}

	void OnCollisionEnter (Collision collisionInfo)
	{
		if (collisionInfo.collider.GetComponent<PartyGuy> ()) {
			return;
		}

		CopCar cop = collisionInfo.collider.GetComponent<CopCar> ();

		if (cop) {
			if (cop.agent.velocity.sqrMagnitude > hitVelocityToDie * hitVelocityToDie) {
				Die ();
			}
			return;
		}

		if (collisionInfo.relativeVelocity.sqrMagnitude > hitVelocityToDie * hitVelocityToDie) {
			Die ();
		}
	}

	void Die ()
	{
		Instantiate (bloodPrefab, transform.position, transform.rotation);

		Destroy (gameObject);
	}
}
