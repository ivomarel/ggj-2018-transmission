using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public PlayerController player;
	public PartyGuy guyPrefab;
	public CopCar copPrefab;

	// Use this for initialization
	protected override void Awake ()
	{
		base.Awake ();
	
		WorldGrid.instance.CreateDungeon ();
		Instantiate (player, WorldGrid.instance.activeRooms [0].transform.position, Quaternion.identity);
		Instantiate (copPrefab, WorldGrid.instance.activeRooms [0].transform.position + new Vector3 (2, 0, 0), Quaternion.identity);
		//PlayerController.instance.transform.position = ;
		foreach (Room r in WorldGrid.instance.activeRooms) {
			r.SpawnUnits ();
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
