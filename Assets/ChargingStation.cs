using UnityEngine;
using System.Collections;

public class ChargingStation : MonoBehaviour {

	public GameObject sparks;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter(Collider collider){
		sparks.SetActive(true);
	}

	public void OnTriggerStay(Collider collider){
		SecuribotControls sc = collider.gameObject.GetComponent<SecuribotControls>();

		if (sc){
			sc.screen.batteryPercentage++;
			if (sc.screen.batteryPercentage > 100){
				sc.screen.batteryPercentage = 100;
			}
		}
	}

	public void OnTriggerExit(Collider collider){
		sparks.SetActive(false);
	}
}
