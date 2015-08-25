using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class numPadDispalyField : MonoBehaviour {

    public GameObject incorrect;
	public GameObject correct;

	public Text text;
	public int currentIndex = 0;
	public GameObject textMockup;

	public static numPadDispalyField instance;

	void Start () 
	{
        text = GetComponentInChildren<Text>();
		instance = this;
		MWM_CMS_DatabaseManager.instance.apartmentManager.SelectedApartmentAction += HandleSelectedApartment;
	}

	void HandleSelectedApartment (bool apartmentSelected)
	{
		if(apartmentSelected)
		{
			text.text = MWM_CMS_DatabaseManager.instance.apartmentManager.selectedApartment.GetComponent<ApartmentBlock>().unitInfo.unit_name;
		}
		else text.text = "";// need to remove text if nothing is selected
	}

	public void ResetDisplay ()
	{
		if (incorrect != null) incorrect.SetActive(false);
		if (correct != null) correct.SetActive(false);
		text.color = Color.white;
		Debug.Log ("Reseting display  " + text);
		text.text = "";
		textMockup.SetActive (true);
	}

	public void IncorrectValue (){
		if (incorrect != null) incorrect.SetActive(true);
	}

	public void CorrectValue() 
	{
		if (correct != null) correct.SetActive(true);
		if (incorrect != null) incorrect.SetActive(false);
	}	
}