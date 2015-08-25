using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using MWM.JSON.Standard;


//added static instance for access.


[ExecuteInEditMode]
public class PopulateApartmentBlockData : MonoBehaviour {
	
	public MWM_CMS_DatabaseManager databaseManager;
	public ApartmentManager apartmentManager;
	
	[Header ("On All database update push data to:")]
	public List<FilterOptionList> dropdownContent;
	[Header ("POPULATE")]
	public bool populateData = false;

	public static PopulateApartmentBlockData instance;

	void Start ()
	{

		instance = this;

	}
	
	void Update(){
		if (populateData){
			databaseManager.UpdateDatabaseEditor();
			populateData = false;
		}


	}
	
	public void UpdatePriceAndStatus()
	{
		
		foreach (MWMUnit unit in databaseManager.units.units) {
			foreach (ApartmentBlock aptBlock in apartmentManager.allSceneApartments){
				
				if (unit.unit_id.ToString () == aptBlock.name){
					
					aptBlock.unitInfo.unit_price = unit.unit_price;
					aptBlock.unitInfo.unit_status = unit.unit_status;
				}
			}
		}
		if(apartmentManager.selectedApartment != null) ApartmentInfoDisplay.instance.RefreshApartmentInfo ();
		
	}
	
	public void UpdateAllApartmentsInfo ()
	{

		Debug.LogWarning (" udpating all apartment info  ");
		foreach (MWMUnit unit in databaseManager.units.units) {
			foreach (ApartmentBlock aptBlock in apartmentManager.allSceneApartments){
				
				if (unit.unit_id.ToString () == aptBlock.name){
					
					aptBlock.unitInfo = unit;
					aptBlock.unitInfo.unit_name = aptBlock.unitInfo.unit_name.Replace("_","");
					aptBlock.unitInfo.unit_name = aptBlock.unitInfo.unit_name.Replace(".","");
					
					aptBlock.unitInfo.unitFloorName = GetFloorNameFromID(aptBlock.unitInfo.unit_floor);
					aptBlock.unitInfo.unitBedroomName = GetBedroomNameFromID(aptBlock.unitInfo.unit_beds);
					aptBlock.unitInfo.unitBlockName = GetBlockNameFromFloorID(aptBlock.unitInfo.unit_floor);
					Debug.Log(" getting core name  ");
					aptBlock.unitInfo.unitCoreName = GetCoreNameFromFloorID(aptBlock.unitInfo.unit_floor);
					aptBlock.unitInfo.unitStatusName = GetStatusNameFromID(aptBlock.unitInfo.unit_status);
					if (aptBlock.unitInfo.unit_view != null)aptBlock.unitInfo.unitViewsArray = GetUnitViewsToArray(aptBlock.unitInfo.unit_view);
				}
			}
		}
		
		if(apartmentManager.selectedApartment != null) ApartmentInfoDisplay.instance.RefreshApartmentInfo ();
		
		ReGenerateFilterOptions ();
		
	}

	public void ReGenerateFilterOptions ()
	{
		foreach (FilterOptionList item in dropdownContent) {
			// On database update regenerates filter options/buttons.
			item.CreateButtons ();
		}
	}
	
	string GetFloorNameFromID (int unit_floor)
	{
		foreach (MWMFloor floor in databaseManager.floors.floors){		
			if(unit_floor == floor.floor_id) return floor.floor_name;
		}
		return "Unknow";
	}
	
	string GetBedroomNameByID (int bedRoomID)
	{
		foreach(MWMBedroom bedroom in databaseManager.bedrooms.bedrooms){
			if(bedroom.bedroom_id == bedRoomID)	return( bedroom.bedroom_name );
		}
		return "Unknow";
	}
	
	string ConvertTypeIDToTypeName (int unit_type)
	{
		foreach (MWMType type in databaseManager.types.types) {
			if(type.type_id == unit_type) return type.type_name;
		}
		return "Unknow";
	}
	
	string GetBedroomNameFromID (string unit_beds)
	{
		foreach (MWMBedroom bedroomType in databaseManager.bedrooms.bedrooms)
		{
			if(unit_beds == bedroomType.bedroom_id.ToString()) return bedroomType.bedroom_name;	
		}
		
		return "Unknow";
	}
	
	string GetBlockNameFromFloorID (int unit_floor)
	{
		foreach (MWMFloor floor in databaseManager.floors.floors){
			if(floor.floor_id == unit_floor) foreach (MWMCore core in databaseManager.core.cores){
				if(core.core_id == floor.floor_core) foreach(MWMBlock block in databaseManager.blocks.blocks)
					if(block.block_id == core.core_block) return block.block_name;
			}
		}
		return "Unknow";
	}

	string GetCoreNameFromFloorID (int unit_floor)
	{
		foreach (MWMFloor floor in databaseManager.floors.floors){
			if(floor.floor_id == unit_floor) foreach (MWMCore core in databaseManager.core.cores){
				if(core.core_id == floor.floor_core)  
					return core.core_name;
			}
		}

		return "Unknow";
	}
	
	string GetStatusNameFromID (int unit_status)
	{
		foreach (MWMStatus status in databaseManager.status.statuses){
			if(status.status_id == unit_status) return status.status_name;
		}
		return "Unknow";
	}
	
	string[] GetUnitViewsToArray (string unit_view)
	{
		return unit_view.Split(',');
	}
}