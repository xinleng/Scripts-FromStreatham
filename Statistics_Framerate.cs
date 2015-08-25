using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Statistics_Framerate : MonoBehaviour {

	public enum infoType{
		FPS,
		MS
	}
	private Text text;
	private float fps;
	private float deltatime;
	private float timePassed = 0f;
	public float updateEverySeconds;
	public infoType infoT;

	void Start () {
		text = GetComponent<Text>();
	}

	void Update () {
		timePassed += Time.deltaTime;
		if (timePassed > updateEverySeconds){
			FPS();
			timePassed = 0f;
		}
	}

	void FPS(){
		deltatime = Mathf.Round(Time.deltaTime * 1000f) / 1000f;
		fps = 1 / deltatime;
		fps = Mathf.Round(fps * 100f) / 100f;

		if (infoT == infoType.MS) text.text = deltatime + " ms";
		if (infoT == infoType.FPS) text.text = "FPS: " + fps; 
	}
}
