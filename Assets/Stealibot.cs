using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stealibot : MonoBehaviour {

	public float walkSpeed = 100;
	public float distanceFromTarget = 10;

	public float peekFrequency = 20;
	public float peekFrequencySD = 5;
	public float chanceToMove = 0.5f;
	public float chanceToMoveMultiplier = 1.1f;
	public float chanceToLitter = 0.2f;
	public float initalPeekDelay = 15;

	public List<Room> rooms;
	public Room currentRoom;
	public Room securibotRoom;
	public Room chargerRoom;
	public Screen screen;
	public GameObject sparks;
	public Transform deadPosition;

	public Transform path;

	private Animator m_anim;
	private bool m_walking = false;

	// Use this for initialization
	void Start () {
		m_anim = GetComponent<Animator>();

		StartCoroutine(PeekAndRun());
	}

	public IEnumerator PeekAndRun(){

		yield return new WaitForSeconds(initalPeekDelay);

		while (currentRoom != chargerRoom){

			Room nextRoom = currentRoom;
			while (nextRoom == currentRoom){
				transform.position = currentRoom.peekLocation.position;
				transform.rotation = currentRoom.peekLocation.rotation;

				yield return new WaitForSeconds(Random.Range(peekFrequency - peekFrequencySD, peekFrequency + peekFrequencySD));
				m_anim.SetTrigger("Peek");

				yield return new WaitForSeconds(1);

				while (!m_anim.GetCurrentAnimatorStateInfo(0).IsName("Stand")){
					yield return new WaitForEndOfFrame();
				}

				nextRoom = chooseNextRoom();
				Debug.Log ("Next room = " + nextRoom);
			}

			

			StartCoroutine(WalkPath(getPathToRoom(nextRoom)));
			yield return new WaitForEndOfFrame();

			while (m_walking){
				yield return new WaitForEndOfFrame();
			}

			currentRoom = nextRoom;
		}

		screen.EnteredRoom(currentRoom.roomCollider);

		sparks.SetActive(true);

		yield return new WaitForSeconds(2);

		StartCoroutine(screen.Hack());

		yield return new WaitForSeconds(7);

		sparks.SetActive(false);

		transform.position = deadPosition.position;
		transform.rotation = deadPosition.rotation;

	}

	Transform getPathToRoom(Room room){
		for (int i = 0; i < currentRoom.connectedRooms.Length; i++){
			if (currentRoom.connectedRooms[i] == room){
				return currentRoom.pathsToConnectedRoom[i];
			}
		}

		for (int i = 0; i < currentRoom.previousRooms.Length; i++){
			if (currentRoom.previousRooms[i] == room){
				return currentRoom.pathsToPreviousRoom[i];
			}
		}

		throw new System.Exception("This should not happen");
	}

	public void EnteredRoom(Collider collider){
		foreach (Room room in rooms){
			if (room.roomCollider == collider){
				securibotRoom = room;
				return;
			}
		}
	}

	Room chooseNextRoom(){

		if (currentRoom == securibotRoom){
			return currentRoom.previousRooms[Random.Range (0, currentRoom.previousRooms.Length - 1)];
		}

		if (Random.value >= chanceToMove) {
			chanceToMove *= chanceToMoveMultiplier;
			return currentRoom;
		}

		foreach (Room room in currentRoom.connectedRooms){
			if (room != securibotRoom){
				return room;
			}
		}

		return currentRoom;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator WalkPath(Transform path){
		
		m_walking = true;

		List<Vector3> points = new List<Vector3>();
		foreach (Transform p in path){
			points.Add(p.position);
		}

		transform.position = points[0];

		foreach (Vector3 nextPoint in points){
			while (Vector3.Distance(nextPoint, transform.position) > distanceFromTarget) {
				transform.LookAt(nextPoint);
				transform.position = Vector3.MoveTowards(transform.position, nextPoint, Time.deltaTime * walkSpeed);
				yield return new WaitForEndOfFrame();
			}
		}

		m_walking = false;
	}
}
