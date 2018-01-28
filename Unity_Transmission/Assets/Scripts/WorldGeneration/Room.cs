using UnityEngine;
using System.Collections;
using System.Text;

public class Room : MonoBehaviour
{

	public int maxUnits = 15;
	public int minUnits = 1;

	public bool north;
	public bool east;
	public bool south;
	public bool west;

	public int[,] grid {
		get {
			if (_grid == null) {
				_grid = new int[,] {
					{ 0, 			east ? 1 : 0, 				0 },
					{ south ? 1 : 0, 1, 		north ? 1 : 0 },
					{ 0, 			west ? 1 : 0, 				0 }
				};
			}

			return _grid;
		}
	}

	private int[,] _grid;

	internal float rotation;
	//public Collider[] possibleExits;

	public void Rotate ()
	{
		//Could be more optimized but who cares
		int[,] newGrid = new int[3, 3];
		for (int i = 0; i < 3; ++i) {
			for (int j = 0; j < 3; ++j) {
				newGrid [i, j] = grid [2 - j, i];
			}
		}
		_grid = newGrid;
		transform.Rotate (0, 90, 0);
	}

	IEnumerator Start ()
	{
		//possibleExits = GetComponentsInChildren<SphereCollider> ();
		yield return null;
		if (minUnits >= 2) {
			SpawnUnits ();
		}
	}

	public Exit[] getExits ()
	{
		return GetComponentsInChildren<Exit> ();

		//return GetComponent<Room> ();
	}

	public void SpawnUnits ()
	{
		
		int r = Random.Range (minUnits, maxUnits);
		for (int i = 0; i < r; i++) {
			Instantiate (GameManager.instance.guyPrefab, transform.position + new Vector3 (Random.Range (-1f, 1f), 0, Random.Range (-1f, 1f)), transform.rotation);
		}

	}

	public void SpawnCar ()
	{

	}


}
