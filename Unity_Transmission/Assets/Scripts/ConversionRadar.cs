using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversionRadar : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnTriggerEnter (Collider otherCollider)
	{
		PartyGuy guy = otherCollider.GetComponent<PartyGuy> ();
		if (guy != null && guy.currentState == PartyGuy.State.Trumping) {
			guy.SetState (PartyGuy.State.BeingConverted);
		}
	}

	void OnTriggerExit (Collider otherCollider)
	{
		PartyGuy guy = otherCollider.GetComponent<PartyGuy> ();
		if (guy != null) {
			//When he is not fully converted yet, go back to Trumping
			if (guy.currentState == PartyGuy.State.BeingConverted) {
				guy.SetState (PartyGuy.State.Trumping);
			}
		}
	}
}
