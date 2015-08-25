using UnityEngine;
using System.Collections;

public class CameraFacingBillboardSimple : MonoBehaviour {

	new GameObject camera;
	Vector3 exPos = new Vector3 (0f,0f,0f);
	Quaternion rotation;

	// Use this for initialization
	void Start () {
		camera = GameObject.FindGameObjectWithTag("MainCamera");
		//transform.rotation = Quaternion.Euler(new Vector3 (315f, 0f, 0f));
	}
	
	// Update is called once per frame
	void Update () {


		if (camera.transform.position != exPos){
			transform.LookAt(transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
		}
		exPos = camera.transform.position;
//		Debug.Log (transform.name + " rotation : " + transform.rotation.eulerAngles);
	}
}
