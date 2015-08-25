using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackToLevel1 : MonoBehaviour {

	public List<GameObject> listDeactivate;
	public List<GameObject> listActivate;


	public void _Back(){
		Debug.Log ("  AAAAAAAAAAa ");

		foreach (GameObject item in listActivate) item.SetActive(true);
		foreach (GameObject item in listDeactivate) item.SetActive(false);

		ModeSwitch.instance.isInLevelOne =true;

	}


}
