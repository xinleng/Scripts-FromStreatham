using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MWM.JSON.Standard;

public class ApartmentInfoDisplay : MonoBehaviour {

	public static ApartmentInfoDisplay instance;
    public enum infoType 
	{
        name,
        type,
        bedRooms,
		aspect,
        floor,
        price,
        status,
    	Level,
		Area_SqFt,
		Area_SqM
    }

    public infoType type;
	ApartmentBlock selectedBlock;
	Text label;

	void OnEnable()
	{
		label = GetComponent<Text>();

		//not sure why this is needed, turnnign off for now
		//if(ApartmentManager.instance.selectedApartment != null) RefreshApartmentInfo();
	}

	void Start () 
	{
		instance = this;
		ApartmentManager.instance.SelectedApartmentAction += apartmentManager_SelectedApartment;
//		MWM_CMS_DatabaseManager.instance.DataBaseUpdated += HandleDataBaseUpdated;


		//not sure why this is needed , turnning off for now.
		//if(ApartmentManager.instance.selectedApartment != null) RefreshApartmentInfo();
	}

//	void HandleDataBaseUpdated (bool obj)
//	{
//		if(MWM_CMS_DatabaseManager.instance.apartmentManager.selectedApartment !=null)
//			RefreshApartmentInfo ();
//	}
//
    void apartmentManager_SelectedApartment(bool obj)
    {
		if(ApartmentManager.instance.selectedApartment != null) RefreshApartmentInfo ();
    }

	public void RefreshApartmentInfo ()
	{
		selectedBlock = ApartmentManager.instance.selectedApartment.GetComponent<ApartmentBlock> ();
		switch (type) {
		
		case infoType.name:
			label.text = selectedBlock.unitInfo.unit_name;
			break;
		case infoType.type:
			label.text = TypeIDToTypeName (selectedBlock.unitInfo.unit_type);
			break;
		case infoType.bedRooms:
			if(TypeIDToTypeName (selectedBlock.unitInfo.unit_type).Substring(0,1) =="S")
				label.text = "Studio";
			else label.text = selectedBlock.unitInfo.unitBedroomName;
			break;
		case infoType.floor:
			label.text = FloorIDToFloorName (selectedBlock.unitInfo.unit_floor);
			break;
		case infoType.Level:
			label.text = ConvertFloorToLevel(FloorIDToFloorName (selectedBlock.unitInfo.unit_floor));
			break;
		case infoType.aspect:
			label.text = selectedBlock.unitInfo.unit_aspect;
			break;
		case infoType.price:
			label.text = selectedBlock.unitInfo.unit_price.ToString ();
			break;
		case infoType.status:
			label.text = StatusIDToString (selectedBlock.unitInfo.unit_status);
			break;
		case infoType.Area_SqFt:
			label.text = selectedBlock.unitInfo.unit_sqft;
			break;
		case infoType.Area_SqM:
			label.text = selectedBlock.unitInfo.unit_sqm;
			break;

		default:
			break;
		}
	}

	//Gets form CMS
	string TypeIDToTypeName (int typeID)
	{
		string typeName = "Unknow";
		foreach (MWMType type in MWM_CMS_DatabaseManager.instance.types.types){
			if (type.type_id == typeID) typeName = type.type_name;
		}
		return typeName;
	}

	string StatusIDToString (int unit_status)
	{
		string statusName = "";
		foreach (MWMStatus status in MWM_CMS_DatabaseManager.instance.status.statuses){
			if(status.status_id ==unit_status) statusName = status.status_name;
		}
		return statusName;
	}
	
	string FloorIDToFloorName (int floorID){
		string floorName = "";
		foreach (MWMFloor floor in MWM_CMS_DatabaseManager.instance.floors.floors){
			if(floor.floor_id ==floorID) floorName = floor.floor_name;
		}
		return floorName;
	}

	// CHANGE VALUES
	string ConvertFloorToLevel (string str)
	{
		switch (str) {
		case "0": return "32";
		case "1": return "35";
		case "2": return "38";
		case "3": return "41";
		case "4": return "44";
		case "5": return "45";
		default: return "XX";
		}
	}
}
