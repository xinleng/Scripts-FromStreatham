using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FilterButtons : MonoBehaviour, IPointerClickHandler {

	public void OnPointerClick (PointerEventData eventData){
		ApartmentManager.instance.SetSearchCondition(GetComponentInParent<FilterOptionList>().filterOption, this.name);

	
		//FILTER TOGGLE
		if(this.name != GetComponentInParent<FilterOptionList>().textDisplay.text){
			GetComponentInParent<FilterOptionList>().textDisplay.text = this.name;
			SubmitButtons.instance.highlighter.SetActive(true);// when ever a search value changed, need to turn the highlighter on

			ShowFloorPlate.instance.ToggleAllowClicking(false); // when ever a serch value changed, need to disable the floorflate button - until user presses one of the submitt buttons.

		}
		GetComponentInParent<FilterOptionList>().dropdownToggle.isOn = false;	


	}
}
