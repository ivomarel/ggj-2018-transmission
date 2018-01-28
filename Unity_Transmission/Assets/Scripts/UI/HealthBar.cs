using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	Slider s;


	// Use this for initialization
	void Start ()
	{
		s = GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		PlayerController p = PlayerController.instance;
		s.value = p.health / p.maxHealth;
	}
}
