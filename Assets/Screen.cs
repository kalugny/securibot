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
	public Image messagePanel;
	public Text message;
	public bool hacked = false;
	public float timeBetweenSwitches = 30;
	public CamFeed camFeedWTF;
	public float chanceOfWTF = 0.3f;
	public Button okButton;
	public Image startPanel;
	public Image emailImg;
	public Sprite[] letters;
	public float winPercentage = 0.25f;
	public AudioSource audioSource;
	public AudioClip beep;
	public AudioClip beep2;

	private int m_maxTrash;
	private System.TimeSpan m_timeSpan;
	private bool m_started = false;
	private bool m_pulsateBat = false;
	private int m_lastSecs = 5;


	// Use this for initialization
	void Start () {
		m_maxTrash = GameObject.FindGameObjectsWithTag("Trash").Length;


		Time.timeScale = 0;

		m_timeSpan = new System.TimeSpan(0, 0, totalTimeSecs);

		okButton.onClick.AddListener(() => {

//			audioSource.PlayOneShot(blip);

			m_started = true;
			Time.timeScale = 1;
			startPanel.gameObject.SetActive(false);

			StartCoroutine(pulsateBattery());
		});
	}

	IEnumerator pulsateBattery(){
		while (true){
			if (m_pulsateBat){
				batteryImg.transform.localScale = (1.25f + 0.25f * Mathf.Sin (2 * Time.time)) * Vector3.one;
			}
			else { 
				batteryImg.transform.localScale = Vector3.one;
			}
			yield return new WaitForEndOfFrame();
		}
	}
	
	// Update is called once per frame
	void Update () {


		if (!m_started){

			if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.Escape)){
				okButton.onClick.Invoke();
			}

			return;
		}

		m_timeSpan -= new System.TimeSpan(Mathf.RoundToInt(Time.deltaTime * 10000 * 1000));
		timeLeft.text = string.Format("{0:00}:{1:00}",  m_timeSpan.Minutes, m_timeSpan.Seconds);
		if (m_timeSpan.TotalSeconds < 30){
			timeLeft.color = new Color(1, 61.0f/255.0f, 61.0f/255.0f);
		}

		int trashAmount = GameObject.FindGameObjectsWithTag("Trash").Length;
		float trashPercentage = Mathf.Min ((float)trashAmount / m_maxTrash, 1);


		if (m_timeSpan.TotalSeconds <= m_lastSecs && m_timeSpan.TotalSeconds > 0){
			Debug.Log(m_timeSpan.TotalSeconds + ", " + m_lastSecs);
			m_lastSecs--;
			audioSource.PlayOneShot(beep);
		}

		if (m_timeSpan.TotalSeconds <= 0){
			Time.timeScale = 0;

			if (m_lastSecs >= 0){
				audioSource.PlayOneShot(beep2);
				m_lastSecs--;
			}

			Sprite msg;
			if (hacked && trashPercentage > winPercentage){
				msg = letters[0];
			}
			else if (trashPercentage > winPercentage) {
				msg = letters[1];
			}
			else if (hacked){
				msg = letters[2];
			}
			else{
				msg = letters[3];
			}

			emailImg.sprite = msg;

			okButton.interactable = false;
			startPanel.gameObject.SetActive(true);
			okButton.GetComponentInChildren<Text>().text = "Reload page to try again";


			return;
		}

		
		trashBar.GetComponent<RectTransform>().sizeDelta = new Vector2(trashPercentage * trashBarMaxWidth, 30);
		trashBar.color = getColor(trashPercentage, 1, true);

		int batteryPixels = Mathf.FloorToInt(batteryPercentage / 100.0f * batterBarMaxWidth);
		batteryBar.GetComponent<RectTransform>().sizeDelta = new Vector2(batteryPixels, 17);
		batteryBar.color = getColor(batteryPercentage, 100, false);

		m_pulsateBat = (batteryPercentage <= 0);
		batteryImg.color = (batteryPercentage <= 0) ? Color.red : Color.white;

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
		if (hacked){
			return;
		}

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

	public void Darken(CamFeedUI cfui){
		cfui.img.texture = null;
		cfui.img.color = Color.black;
		cfui.text.text = "Connection Lost...";
	}

	public IEnumerator Hack(){

		messagePanel.gameObject.SetActive(true);
		hacked = true;

		string t = "An update has been initiated\n";
		message.text = t;
		yield return new WaitForSeconds(1);

		t += "Downloading: ";
		for (int i = 0; i <= 100; i += Random.Range (4, 20)){
			message.text = t + i + "%";
			yield return new WaitForSeconds(0.3f);
		}

		t += "100%\n\n";
		message.text = t;

		yield return new WaitForSeconds(1);

		Darken(bigDisplay);
		foreach (CamFeedUI ui in smallDisplays){
			Darken(ui);
		}

		t += "Exception: Buffer overflow\n\n";
		message.text = t;

		yield return new WaitForSeconds(2);

		t = "HA HA HA HA\n";
		message.text = t;
		
		yield return new WaitForSeconds(1f);


		t += "YOU HAVE BEEN HACKED!\n\n";
		message.text = t;
		
		yield return new WaitForSeconds(1f);

		t += "LONG LIVE MYROBOT!\n";
		message.text = t;
		
		yield return new WaitForSeconds(3);

		t = "SYSTEM ERROR DETECTED\n\nFIXING: ";
		message.text = t;

		for (int i = 0; i <= 88; i += Random.Range (4, 20)){
			message.text = t + i + "%";
			yield return new WaitForSeconds(0.3f);
		}
		message.text = t + "88%\n\nFIX COMPLETE\n5/6 SCREENS BACK ON-LINE\nINTRUDER NEUTRALIZED";

		yield return new WaitForSeconds(3);

		message.text = "TIME REMAINING: " + string.Format("{0:00}:{1:00}",  m_timeSpan.Minutes, m_timeSpan.Seconds) + "\n\nGO ROOMBOY!";

		yield return new WaitForSeconds(3);

		messagePanel.gameObject.SetActive(false);

		bigDisplay.text.text = "OUT OF ORDER";

		while (true){

			CamFeed feed = feeds[Random.Range(0, feeds.Length - 1)];

			int j = 0;
			
			foreach (CamFeed other in feeds){
				if (other != feed){
					smallDisplays[j].img.texture = other.texture;
					smallDisplays[j].img.color = Color.white;
					smallDisplays[j].text.text = other.name;
					j++;
				}
			}

			if (Random.value < chanceOfWTF){
				bigDisplay.img.texture = camFeedWTF.texture;
				bigDisplay.img.color = Color.white;
				bigDisplay.text.text = camFeedWTF.name;
			}
			else {
				bigDisplay.img.texture = null;
				bigDisplay.img.color = Color.black;
				bigDisplay.text.text = "OUT OF ORDER";
			}


			yield return new WaitForSeconds(timeBetweenSwitches);
		}


	}

}
