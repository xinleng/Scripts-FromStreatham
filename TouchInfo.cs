using UnityEngine;
using System.Collections;
using TouchScript.Gestures.Simple;
using TouchScript.Hit;

public class TouchInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {


		Debug.Log (GetComponent<SimplePanGesture>());

		GetComponent<SimplePanGesture>().Panned += HandlePanned;

		GetComponent<SimpleScaleGesture>().Scaled += HandleScaled;
		

	}

	void HandleScaled (object sender, System.EventArgs e)
	{

		Debug.Log (" scalled  ");
		SimpleScaleGesture gesture = sender as SimpleScaleGesture;

		Debug.Log (gesture.LocalDeltaScale);

	}

	void HandlePanned (object sender, System.EventArgs e)
	{


		Debug.Log (" panned  ");
		SimplePanGesture gesture = sender as SimplePanGesture;


		Debug.Log (gesture.ScreenPosition );


		Debug.Log (gesture.LocalDeltaPosition);

		//gesture.ActiveTouches



	}



}
