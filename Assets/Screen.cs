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
	public Color[] gaugesColors;
	public Image batteryBar;
	public Image batteryImg;
	public float batteryPercentage = 100;
	public int batterBarMaxWidth;
	public Text timeLeft;
	public int totalTimeSecs = 5 * 60;

	private int m_maxTrash;
	private System.TimeSpan m_timeSpan;


	// Use this for initialization
	void Start () {
		m_maxTrash = GameObject.FindGameObjectsWithTag("Trash").Length;
		m_timeSpan = new System.TimeSpan(0, 0, totalTimeSecs);
	}
	
	// Update is called once per frame
	void Update () {

		m_timeSpan -= new System.TimeSpan(Mathf.RoundToInt(Time.deltaTime * 10000 * 1000));
		timeLeft.text = string.Format("{0:00}:{1:00}",  m_timeSpan.Minutes, m_timeSpan.Seconds);
		if (m_timeSpan.TotalSeconds < 30){
			timeLeft.color = new Color(1, 61.0f/255.0f, 61.0f/255.0f);
		}

		int trashAmount = GameObject.FindGameObjectsWithTag("Trash").Length;
		float trashPercentage = Mathf.Min ((float)trashAmount / m_maxTrash, 1);
		trashBar.GetComponent<RectTransform>().sizeDelta = new Vector2(trashPercentage * trashBarMaxWidth, 30);
		trashBar.color = getColor(trashPercentage, 1, true);

		int batteryPixels = Mathf.FloorToInt(batteryPercentage / 100.0f * batterBarMaxWidth);
		batteryBar.GetComponent<RectTransform>().sizeDelta = new Vector2(batteryPixels, 17);
		batteryBar.color = getColor(batteryPercentage, 100, false);

		batteryImg.color = (batteryPixels <= 0) ? Color.red : Color.white;

	}

	private Color getColor(float value, int maxValue, bool lessIsBetter){
		int colorIndex = Mathf.Min (Mathf.FloorToInt(value / maxValue * gaugesColors.Length), gaugesColors.Length - 1);
		if (lessIsBetter){
			colorIndex = gaugesColors.Length - 1 - colorIndex;
		}
		return gaugesColors[colorIndex];
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
