using UnityEngine;
using System.Collections;

public class SecuribotControls : MonoBehaviour {

	// Movement
	public float turnSpeed = 40;
	public float speed = 4;

	// Screen
	public Screen screen;

	// Camera feeds

	private CharacterController m_cc;

	// Use this for initialization
	void Start () {
		m_cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		float forward = Mathf.Sign(Input.GetAxis("Vertical"));
		transform.Rotate(0, forward * Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);

		m_cc.Move(transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime);   
		transform.position = new Vector3(transform.position.x, 0, transform.position.z);
	}

	void OnTriggerEnter(Collider collider){
		screen.EnteredRoom(collider);

		if (collider.tag == "Trash"){
			CollectTrash(collider.gameObject);
		}

	}

	public void CollectTrash(GameObject trash){
		Destroy(trash);
	}
}
