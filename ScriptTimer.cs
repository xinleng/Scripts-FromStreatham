using UnityEngine;
using System.Collections;

public class ScriptTimer : MonoBehaviour {

	static bool startTimer;
	static float timeElapsed = 0f;

	void Update(){

		if (startTimer){
			timeElapsed += Time.deltaTime;
		} 
	}


	public static void StartTimer(){
		startTimer = true;
	}

	public static void StopTimer(){
		startTimer = false;
		Debug.LogWarning ("Time elapsed to do the action: " + timeElapsed);
		timeElapsed = 0f;

	}

}
