using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetFloorPlan : MonoBehaviour {

    RawImage planHolder;
	public string folderPath = "Floorplans/";

	void Start () {
        planHolder = GetComponentInChildren<RawImage>();
	}

	public void LoadFloorPlanIntoResource (){

		string typeName = "Template";

		if(!string.IsNullOrEmpty(MWM_CMS_DatabaseManager.instance.apartmentManager.selectedApartment.GetComponent<ApartmentBlock>().unitInfo.unit_plan))
			 typeName= MWM_CMS_DatabaseManager.instance.apartmentManager.selectedApartment.GetComponent<ApartmentBlock>().unitInfo.unit_plan;

		planHolder.texture = Resources.Load (folderPath + typeName) as Texture;
		Debug.Log ("Apartment selected. Floorplan " + typeName + " loaded");
	}
}
