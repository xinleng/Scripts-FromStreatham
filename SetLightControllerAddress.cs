using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetLightControllerAddress : MonoBehaviour {

	InputField input;

	// Use this for initialization
	void Start () {

		input = GetComponent<InputField>();

		input.text = PlayerPrefs.GetString("LightControlAddress");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetLightContollerAddress ()

	{

		LightingController.instance.LightControllerAddress = input.text;

	}
	//
}
