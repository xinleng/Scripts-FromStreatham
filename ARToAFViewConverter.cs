using UnityEngine;
using System.Collections;

public class ARToAFViewConverter : MonoBehaviour {

	public static ARToAFViewConverter instance;

	public float intentedViewAngle;

	float rotInertia;

	// Use this for initialization
	void Start () {

		instance =this;
		
	}

	void OnEnable () {
		
		instance =this;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetAFCamAngle()

	{
		Debug.Log (" setting AF view angle to " + this.transform.localEulerAngles.y+230f );

		intentedViewAngle = this.transform.localEulerAngles.y+230f;
		//rotInertia = CameraControl.instance.rotationInertia;
		CameraControl.instance.rotationInertia = 10;
		CameraControl.instance.xAngle = intentedViewAngle;

		StartCoroutine(ResumeCamRotationInertia());

	}

	IEnumerator ResumeCamRotationInertia ()
	{
		yield return new WaitForSeconds(1);

		CameraControl.instance.rotationInertia = 1f;
	}
}
