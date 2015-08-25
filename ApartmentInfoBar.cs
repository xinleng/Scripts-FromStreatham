using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ApartmentInfoBar : MonoBehaviour {

	CanvasGroup infoBar;

	public static ApartmentInfoBar instance;
	public bool shouldShowWhenApartmentChoosen;
	public GameObject keypad;
	public GameObject filters;

	public ToggleExtended headerSearch;
	public ToggleExtended headerFilter;

	void Start () {
		instance = this;
		infoBar = GetComponent<CanvasGroup>();
	
		MWM_CMS_DatabaseManager.instance.apartmentManager.SelectedApartmentAction += HandleSelectedApartment;
	}

	void HandleSelectedApartment (bool apartmentFound){

		if(apartmentFound){
			ToggleApartmentInfoBar(shouldShowWhenApartmentChoosen);

			if (headerSearch.isOn) keypad.SetActive(!shouldShowWhenApartmentChoosen);
			if (headerFilter.isOn) filters.GetComponent<ToggleCanvasGroup>().DoToggleCancasGroup(!shouldShowWhenApartmentChoosen);
		}
		else{
			ToggleApartmentInfoBar(!shouldShowWhenApartmentChoosen);

			if (headerSearch.isOn) keypad.SetActive(shouldShowWhenApartmentChoosen);
			if (headerFilter.isOn)  filters.GetComponent<ToggleCanvasGroup>().DoToggleCancasGroup(shouldShowWhenApartmentChoosen);
		}
	}

	public void ToggleApartmentInfoBar (bool toggle ){
		if(toggle) infoBar.alpha = 1.0f; 
		else infoBar.alpha = 0f;

		infoBar.interactable = toggle;
		infoBar.blocksRaycasts = toggle;
	}
}
