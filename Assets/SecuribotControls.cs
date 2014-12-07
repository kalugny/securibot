﻿using UnityEngine;
using System.Collections;

public class SecuribotControls : MonoBehaviour {

	// Movement
	public float turnSpeed = 40;
	public float speed = 4;

	// Screen
	public Screen screen;
	public Stealibot stealibot;

	// Camera feeds

	private CharacterController m_cc;

	// Use this for initialization
	void Start () {
		m_cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {

		float forward = Mathf.Sign(Vector3.Dot(m_cc.velocity, transform.forward));
		if (forward == 0){
			forward = 1;
		}
		transform.Rotate(0, forward * Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);

		m_cc.Move(transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime);   
		transform.position = new Vector3(transform.position.x, 0, transform.position.z);
	}

	void OnTriggerEnter(Collider collider){
		screen.EnteredRoom(collider);
		stealibot.EnteredRoom(collider);

		if (collider.tag == "Trash"){
			CollectTrash(collider.gameObject);
		}

	}

	public void CollectTrash(GameObject trash){

		if (screen.batteryPercentage > 0){
			Destroy(trash);
			screen.batteryPercentage -= 1;
		}
	}
}
