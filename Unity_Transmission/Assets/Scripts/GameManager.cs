using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : Singleton<GameManager>
{
	public PlayerController player;
	public PartyGuy guyPrefab;
	public CopCar copPrefab;

	public float copcarSpawnInterval;

	// Use this for initialization
	protected override void Awake ()
	{
		base.Awake ();
	
		WorldGrid.instance.CreateDungeon ();
		Instantiate (player, WorldGrid.instance.activeRooms [0].transform.position, Quaternion.identity);
		//PlayerController.instance.transform.position = ;
		int i = 0;
		foreach (Room r in WorldGrid.instance.activeRooms) {
			if (i >= 1) {
				r.SpawnUnits ();
			}
			i++;
		}

		InvokeRepeating ("SpawnCopCar", copcarSpawnInterval, copcarSpawnInterval);
	}
	
	// Update is called once per frame
	public void SpawnCopCar ()
	{
		List<Room> roomsOrdered = new List<Room> (WorldGrid.instance.activeRooms.Where (r => r != null).OrderBy (r => (r.transform.position - PlayerController.instance.transform.position).sqrMagnitude));

		Instantiate (copPrefab, roomsOrdered [9].transform.position, Quaternion.identity);
	}
}
