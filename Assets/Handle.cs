using UnityEngine;
using System.Collections;

public class Handle : MonoBehaviour {

	public float maxAngleX = 45;
	public float maxAngleZ = 45;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(-Input.GetAxis ("Vertical") * maxAngleZ, 0, Input.GetAxis ("Horizontal") * maxAngleZ);
	}
}
