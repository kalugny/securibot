using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Screen : MonoBehaviour {

	[System.Serializable]
	public class CamFeed {
		public RenderTexture texture;
		public BoxCollider roomCollider;
		public string name;
	}

	[System.Serializable]
	public class CamFeedUI {
		public RawImage img;
		public Text text;
	}

	public CamFeed[] feeds;
	public CamFeedUI bigDisplay;
	public CamFeedUI[] smallDisplays;
	public Image trashBar;
	public int trashBarMaxWidth;
	public Color[] trashBarColors;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int trashAmount = GameObject.FindGameObjectsWithTag("Trash").Length;
		trashBar.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Min(trashAmount, trashBarMaxWidth), 30);
		int colorIndex = Mathf.Min (Mathf.FloorToInt((float)trashAmount / trashBarMaxWidth * trashBarColors.Length), trashBarColors.Length - 1);
		trashBar.color = trashBarColors[colorIndex];
	}

	public void EnteredRoom(Collider collider){

		foreach (CamFeed feed in feeds){
			if (feed.roomCollider == collider){
				ShowInBigDisplay(feed);
				break;
			}
		}
	}

	public void ShowInBigDisplay(CamFeed feed){
		bigDisplay.img.texture = feed.texture;
		bigDisplay.text.text = feed.name;

		int i = 0;
		foreach (CamFeed other in feeds){
			if (other != feed){
				smallDisplays[i].img.texture = other.texture;
				smallDisplays[i].text.text = other.name;
				i++;
			}
		}
	}

}
