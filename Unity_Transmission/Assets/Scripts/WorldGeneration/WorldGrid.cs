using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WorldGrid : Singleton<WorldGrid>
{

	public float buildDelay;
	public static int nRooms;
	public float realRoomSize;

	public const int ROOM_SIZE = 3;

    public Room fourWayRoomPrefab;
    public Room threeWayRoomPrefab;
    public Room cornerRoomPrefab;
    public Room straightRoomPrefab;
    public Room deadEndRoomPrefab;

	internal Room[] roomPrefabs;

	internal Room[] activeRooms;

	public int[,] dungeon;

	Queue<GridPos> roomPosQueue;

    int dungeonSize {
        get {
            return nRooms * ROOM_SIZE * 2;
        }
    }

	public void CreateDungeon ()
	{
        roomPrefabs = new Room[5];
        roomPrefabs[0] = fourWayRoomPrefab;
        roomPrefabs[1] = threeWayRoomPrefab;
        roomPrefabs[2] = cornerRoomPrefab;
        roomPrefabs[3] = straightRoomPrefab;
        roomPrefabs[4] = deadEndRoomPrefab;

		foreach (Room r in roomPrefabs) {
			r.gameObject.SetActive (false);
		}

		StartCoroutine (CreateDungeonCor ());

		activeRooms = GetComponentsInChildren<Room> (true);
		foreach (Room r in activeRooms) {
			r.gameObject.SetActive (true);
		}
	}

	IEnumerator CreateDungeonCor ()
	{

		dungeon = new int[dungeonSize, dungeonSize];
		for (int i = 0; i < dungeonSize; i++) {
			for (int j = 0; j < dungeonSize; j++) {
				dungeon [i, j] = -1;
			}
		}

		roomPosQueue = new Queue<GridPos> ();

        int startX = dungeonSize/2 /ROOM_SIZE;
		int startY = dungeonSize/2/ ROOM_SIZE;

		roomPosQueue.Enqueue (new GridPos (startX * ROOM_SIZE, startY * ROOM_SIZE));

		int nRoomsPlaced = 0;

        bool firstTime = true;

		//We add new room positions to queue
		while (roomPosQueue.Count != 0) {
			nRoomsPlaced++;
			GridPos pos = roomPosQueue.Dequeue ();
			//We don't use the last room here, because the last room is supposed to be a dead end.
			//If we place the dead ends too fast, the world often generates too small
			int randomIndex = Random.Range (0, roomPrefabs.Length - 1);

            //Always start with a square
            if (firstTime) {
                randomIndex = 0;
                firstTime = false;
            }
			PlaceRoom (randomIndex, pos.x, pos.y);
			if (buildDelay > 0) {
				yield return new WaitForSeconds (buildDelay);
			}
			if (nRoomsPlaced >= nRooms) {
				break;
			}
		}

        for (int i = 0; i < dungeonSize; i += ROOM_SIZE)
        {
            for (int j = 0; j < dungeonSize; j += ROOM_SIZE)
            {
                if (dungeon[i,j] == 2) {
                    PlaceRoom(4, i, j, false);
                }
            }
        }   

			
		//PrintDungeon ();

		//	PrintQueue ();
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			SceneManager.LoadScene ("GameScene");
		}
	}

	void PlaceRoom (int roomIndex, int x, int y, bool checkSides = true)
	{
		//We instantiate a roomClone. This might be destroyed later (not efficient but keeping it for now)
		Room roomClone = Instantiate (roomPrefabs [roomIndex], transform);

		//For safety, we break out of the array when all rooms (with all rotations) were checked. This should never happen though!
		int breakPoint = 0;
		int rotations = 0;
		//We try puzzling the room in
		while (!CanPlaceRoom (roomClone, x, y)) {
			//We rotate the room 4 times
			rotations++;
			roomClone.Rotate ();
			if (rotations >= 4) {
				//If the room doesn't fit after 4 rotations, we pick the next room
				rotations = 0;
                if (checkSides)
                {
					roomIndex++;
				}
                else {
                    roomIndex--;
                }

                if (roomIndex < 0) {
                    roomIndex = roomPrefabs.Length - 1;
                }

				if (roomIndex >= roomPrefabs.Length) {
					roomIndex = 0;
				}
				//We destroy the old room, since it didn't fit
				Destroy (roomClone.gameObject);
				if (breakPoint > roomPrefabs.Length) {
					roomIndex = 4;
					//Debug.LogError ("Unity would have crashed...");
					//return;
				}
				roomClone = Instantiate (roomPrefabs [roomIndex], transform);

				breakPoint++;
			}
		}

		//At this point we have found a fitting room, and we can enter the data in our grid
		for (int i = 0; i < ROOM_SIZE; i++) {
			for (int j = 0; j < ROOM_SIZE; j++) {
				dungeon [x + i, y + j] = roomClone.grid [i, j];
			}
		}

		//We place the room at the correct position
		roomClone.transform.position = new Vector3 ((x - nRooms * .5f) * realRoomSize, 0, (y - nRooms * .5f) * realRoomSize);

    //We need to check the grid if our room has doors to empty spaces. If so, we need to place extra rooms there.

    if (checkSides)
    {
        //TopDoor
        CheckAddRoom(x, y + 1, x - 1, y + 1, x - 3, y);
        //LeftDoor
        CheckAddRoom(x + 1, y, x + 1, y - 1, x, y - 3);
        //BottomDoor
        CheckAddRoom(x + 2, y + 1, x + 3, y + 1, x + 3, y);
        //RightDoor
        CheckAddRoom(x + 1, y + 2, x + 1, y + 3, x, y + 3);

    }
	}

	void CheckAddRoom (int roomDoorPosX, int roomDoorPosY, int roomConnectPosX, int roomConnectPosY, int newPosX, int newPosY)
	{
		//If this side has a door and the dungeon is still empty
		if (dungeon [roomDoorPosX, roomDoorPosY] == 1 && dungeon [roomConnectPosX, roomConnectPosY] == -1) {
			for (int i = 0; i < 3; i++) {
				for (int j = 0; j < 3; j++) {
					//We mark this grid as 'to be built upon'
					dungeon [newPosX + i, newPosY + j] = 2;
				}
			}
			roomPosQueue.Enqueue (new GridPos (newPosX, newPosY));
		}
	}

	bool CanPlaceRoom (Room room, int x, int y)
	{
		//If any side does not fit, we can't place
		if (!CheckSide (room, 0, 1, x - 1, y + 1))
			return false;
		if (!CheckSide (room, 1, 0, x + 1, y - 1))
			return false;
		if (!CheckSide (room, 2, 1, x + 3, y + 1))
			return false;
		if (!CheckSide (room, 1, 2, x + 1, y + 3))
			return false;

		return true;
	}

	bool CheckSide (Room room, int roomDoorPosX, int roomDoorPosY, int roomConnectPosX, int roomConnectPosY)
	{
		//If the connectingpart is a -1, it always fits
		int connectingPart = dungeon [roomConnectPosX, roomConnectPosY];
		if (connectingPart == -1 || connectingPart == 2)
			return true;
		//If the connectingpart is the same as the door (or not door), it also fits 
		if (room.grid [roomDoorPosX, roomDoorPosY] == connectingPart) {
			return true;
		}
		return false;
	}


	void PrintDungeon ()
	{
		StringBuilder sb = new StringBuilder ();
		for (int i = 0; i < nRooms * ROOM_SIZE; i++) {
			sb.Append ('\n');
			for (int j = 0; j < nRooms * ROOM_SIZE; j++) {
				sb.Append (dungeon [i, j]);
				sb.Append (' ');
			}
		}
		print (sb);
	}

	void PrintQueue ()
	{
		StringBuilder sb = new StringBuilder ();
		foreach (GridPos p in roomPosQueue) {
			sb.Append (p.x);
			sb.Append (',');
			sb.Append (p.y);
			sb.Append ("||");
		}
		print (sb);
	}

}

public class GridPos
{

	public GridPos (int _x, int _y)
	{
		x = _x;
		y = _y;
	}

	public int x;
	public int y;
}