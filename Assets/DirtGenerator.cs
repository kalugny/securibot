using UnityEngine;
using System.Collections;

public class DirtGenerator : MonoBehaviour {

	public Material[] materials;
	public float itemsPerSqUnit;
	public float stdDevPerSqUnit;
	public float sizeMultiplier;
	
	private BoxCollider m_c;

	// Use this for initialization
	void Awake () {
		m_c = GetComponent<BoxCollider>();

		GenerateDirt();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider){
		Stealibot bot = collider.gameObject.GetComponent<Stealibot>();

		if (bot && Random.value < bot.chanceToLitter){
			GenerateDirt();
		}

	}

	public void GenerateDirt(){
		float area = Mathf.Abs(m_c.bounds.size.x * m_c.bounds.size.z);
		float meanNumberOfItems = area * itemsPerSqUnit;
		float stdDev = area * stdDevPerSqUnit;
		int numItems = Mathf.RoundToInt(Random.Range(Mathf.Max(0, meanNumberOfItems - stdDev), meanNumberOfItems + stdDev));

//		Debug.Log ("Area: " + area + " Mean: " + meanNumberOfItems + " StdDev: " + stdDev + " Items: " + numItems);

		for (int i = 0; i < numItems; i++){

			PrimitiveType t = (PrimitiveType)Random.Range(0, 3);
			Material m = materials[Random.Range(0, materials.Length - 1)];
			Vector3 p1 = m_c.bounds.min;
			Vector3 p2 = m_c.bounds.max;
			Vector3 pos = new Vector3(Random.Range (p1.x, p2.x), 0, Random.Range(p1.z, p2.z));

			GameObject g = GameObject.CreatePrimitive(t);
			g.transform.SetParent(transform);
			g.transform.position = pos;
			g.transform.rotation = Random.rotation;
			g.transform.localScale = sizeMultiplier * Vector3.one;
			g.collider.isTrigger = true;
			g.tag = "Trash";
			g.renderer.material = m;
		}
	}
}
