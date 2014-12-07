using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {
	
	public Transform peekLocation;
	public Room[] connectedRooms;
	public Transform[] pathsToConnectedRoom;
	public Room[] previousRooms;
	public Transform[] pathsToPreviousRoom;
	public BoxCollider roomCollider;	


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
