using UnityEngine;
using System.Collections;
using MWM.JSON.Standard;
using MWM.Gallery;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ApartmentBlockListButton : MonoBehaviour,IPointerClickHandler{

	public MWMUnit unitInfo;

	new public Text name;
	public Text beds;
	public Text floor;
	public Text area;
	public Text price;
	public Text status;

	public List<Text> textList;

	public bool textIsHidden;

	public bool isFavourite;
	new public GameObject camera;

	void Update(){
		if (textIsHidden == false) childrenTextHide();
		siblingTextUnhide(); 
	}

	#region OPTIMIZATION. SHOWING TEXT ONLY OF THE OBJECTS IN THE SCREEN
	void siblingTextUnhide(){
		if (transform.position.y < Screen.height && transform.position.y > 0f){
			if (transform.GetSiblingIndex() < transform.parent.childCount - 1) transform.parent.GetChild (transform.GetSiblingIndex () + 1 ).GetComponent<ApartmentBlockListButton>().childrenTextUnhide();
			if (transform.GetSiblingIndex() > 0)transform.parent.GetChild (transform.GetSiblingIndex () - 1 ).GetComponent<ApartmentBlockListButton>().childrenTextUnhide();
		}
	}

	// EXECUTED when text is not hidden and obj is out of bounds. 
	void childrenTextHide(){
		if(transform.position.y > (Screen.height + 200f) || transform.position.y < -200f){
			foreach (Text item in GetComponentsInChildren<Text>()) {
				textList.Add (item);
				item.enabled = false;
			}
			textIsHidden = true;
		}
	}

	// Neighbouring sibling calls. when sibling is in the screen.
	public void childrenTextUnhide(){
		foreach (Text item in textList) {
			item.enabled = true;
		}
		textIsHidden = false;
	}
	#endregion

	public void UpdateListButtonInfo (){
		if (name != null) name.text = unitInfo.unit_name;
		if (beds != null) beds.text = unitInfo.unitBedroomName;
		if (floor != null) floor.text = unitInfo.unitFloorName;
		if (area != null) area.text = unitInfo.unit_sqft;
		if (price != null) price.text = unitInfo.unit_price.ToString();
		if (status != null) status.text = unitInfo.unitStatusName;
	}

	public void OnPointerClick (PointerEventData eventData){
		//select apartment;
		if(GetComponent<SetFavouriteGallery>() != true){
			ApartmentManager.instance.SearchApartmentByName(name.text);
			xRayToggleControler.instance.Turn_xRay_ON();
			transform.parent.parent.parent.GetComponent<ToggleCanvasGroup>().DoToggleCancasGroup(false);
		}
	}
}


