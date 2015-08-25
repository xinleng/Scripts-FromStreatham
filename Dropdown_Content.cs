using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// 	Does the action with passed on values from DD_Content_Button
public class Dropdown_Content : MonoBehaviour {

	public Text DD_TextDisplay;

	[Tooltip("Toggle Object from Dropdown Menu [THAT GETS SELECTED ON START]")]
	public GameObject toggleStart;
	string textStart;

	[HideInInspector]	public string textString;
	[HideInInspector]	public GameObject toggleSelected; 
	[HideInInspector]	private GameObject toggleEx;

	void Start () {

		toggleSelected = toggleStart;
		textString = toggleSelected.GetComponent<Text>().text;
		DD_Content_Action ();

	}
	
	public void DD_Content_Action(){

		transform.GetComponentInParent<ToggleExtended>().isOn = false; // Getting DD_CurentSelected
		DD_TextDisplay.text = textString;
		if (toggleEx != null) toggleEx.SetActive(true);
		if (toggleEx != null) toggleEx.GetComponent<Toggle>().isOn = false;
		toggleSelected.SetActive(false);

		toggleEx = toggleSelected;

	}
}
