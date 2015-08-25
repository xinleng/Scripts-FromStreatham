using UnityEngine;
using System.Collections;

public class xRayToggleControler : MonoBehaviour {

	public static xRayToggleControler instance;
	ToggleExtended toggle;

	void Start () {
		instance = this;
		toggle = transform.GetComponent<ToggleExtended>();
	}
	
	public void Turn_xRay_ON(){
		toggle.isOn = true;
	}

	public void Turn_xRay_OFF(){
		toggle.isOn = false;
	}
}