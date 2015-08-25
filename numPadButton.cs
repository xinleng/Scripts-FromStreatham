using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class numPadButton : MonoBehaviour,IPointerClickHandler {

    public enum numPadButtonType
    {
        letters,
        numbers,
        clear,
        search,
        other
    }

    public numPadButtonType type;
	public string thisText;// to store this text
    
	void Start () {
		thisText = this.name.Substring(this.name.Length-1,1);
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		Debug.Log ("Clicked on " + this.name);

		switch (type)
		{
		case numPadButtonType.letters:
			numPadDispalyField.instance.textMockup.SetActive(false);
			numPadDispalyField.instance.correct.SetActive(true);
			if(numPadDispalyField.instance.text.text.Length < 5){
				numPadDispalyField.instance.text.text = numPadDispalyField.instance.GetComponentInChildren<Text>().text + thisText;	
			}
			break;

		case numPadButtonType.numbers:
			numPadDispalyField.instance.textMockup.SetActive(false);
			numPadDispalyField.instance.correct.SetActive(true);
			if(numPadDispalyField.instance.text.text.Length <5){
				numPadDispalyField.instance.text.text = numPadDispalyField.instance.GetComponentInChildren<Text>().text+ thisText;
			}
			break;

		case numPadButtonType.other:
			break;

		case numPadButtonType.clear:
			numPadDispalyField.instance.ResetDisplay();
			ApartmentManager.instance.DeselectApartment();

			break;
		case numPadButtonType.search:
			if(numPadDispalyField.instance.text.text !=""){

				string unitName = "";
				unitName = numPadDispalyField.instance.text.text;
				Debug.Log(unitName);   
				
				MWM_CMS_DatabaseManager.instance.apartmentManager.ApartmentFound = false;
				MWM_CMS_DatabaseManager.instance.apartmentManager.SearchApartmentByName(unitName);
				xRayToggleControler.instance.Turn_xRay_ON();

				if (!MWM_CMS_DatabaseManager.instance.apartmentManager.ApartmentFound){					
					numPadDispalyField.instance.IncorrectValue();
				}
				else numPadDispalyField.instance.CorrectValue();
			}
			break;
		default:
			break;
		}
	}
}