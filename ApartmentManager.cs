using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MWM.JSON.Standard;

[ExecuteInEditMode]
public class ApartmentManager:MonoBehaviour { 
	
	public Material highlightMat;
	
	public MWMPhase phase;
	public MWMBlock block;
	public MWMCore core;
	public MWMFloor floor;
	public MWMBedroom bedroom;
	public MWMAspect aspect;
	public MWMType type;
	public MWMPricerange priceRange;
	public MWMStatus status;
	
	public event Action<bool> FilterApartmentsAction;
	public event Action<bool> SelectedApartmentAction;
	public event Action<string> SearchbyNameAction;
	public event Action<bool,PrefToggleType> PlayerPrefChangeAction;

	public event Action<CheckList>FilterOptionChangedAction;
	
	public bool ApartmentFound;
	
	public GameObject selectedApartment;
	
	public List<ApartmentBlock> allSceneApartments;
	public List<ApartmentBlock> filteredApartments;
	public List<ApartmentBlock> favoritedApartments;
	
	public Transform VolumesParent;
	
	public static ApartmentManager instance;
	
	void Start(){
		UpdateFavouriteList ();
	}
	
	void Awake (){
		instance = this;
	}
	
	
	#region FAVOURITES 
	public void UpdateFavouriteList()
	{
		favoritedApartments.Clear();
		foreach(ApartmentBlock apartment in allSceneApartments){
			if(apartment.favorited) favoritedApartments.Add(apartment);
		}
	}

	public void CleanFavourites()

	{

		foreach(ApartmentBlock apartment in favoritedApartments){
			apartment.favorited = false;
		}

		favoritedApartments.Clear();

	}
	#endregion
	
	#region FILTERS
	public void FilterApartments() {
		
		filteredApartments.Clear();
		FilterApartmentsAction.Invoke(true);
		
		//		if(ModeSwitch.instance.AFModeIsInAR){
		//			ModeSwitch.instance.AFModeIsInAR=false;
		//			ModeSwitch.instance._SwitchToAFinder(true);
		//			ModeSwitch.instance.AFModeIsInXray =true;
		//		}
		
		LightingController.instance.TurnOnLEDSets();// Turns on filtered appartments
		
		if(block.block_name == "ALL"){
			CameraControl.instance.MoveOrbitCenterTo(CameraControl.instance.defaultOrbitPoint);
			CameraControl.instance.distance = 200;
			//both block colliders should be on
		}else{
			Debug.Log(" block value is something else  ");
		}
	}
	
	public void ResetSearchConditionAndFilter (bool showAllApartment) 
	{
		DeselectApartment();
		block.block_name = "ALL";
		core.core_name = "ALL";
		floor.floor_name = "ALL";
		bedroom.bedroom_name = "ALL";
		type.type_name = "ALL";
		aspect.aspect_name = "ALL";
		priceRange = new MWMPricerange();
		status.status_name = "ALL";
		
		Debug.Log ("All values RESET");

		if(showAllApartment)
					FilterApartments();
		else HideAllApartments();
		LightingController.instance.TurnOnLEDSets();
	}

	void HideAllApartments()
	{

		foreach (ApartmentBlock aparment in allSceneApartments)
		{

			aparment.ToggleApartmentVisiblity(false);

		}

	}
	
	public void SetSearchCondition (CheckList filterType, string name){

		switch (filterType) {
		case CheckList.check_blocks: {
			block.block_name = name;
			break;
		}

		case CheckList.check_cores: {
			core.core_name = name;
			break;
		}
			
		case CheckList.check_floors: {
			floor.floor_name = name;
			floor.floor_id = FloorNameToId(name);
			break;
		}
			
		case CheckList.check_bedrooms: {
			bedroom.bedroom_name = name;
			bedroom.bedroom_id = BedroomNameToId(name);
			break;
		}
			
		case CheckList.check_aspects: {
			aspect.aspect_name = name;
			aspect.aspect_id = AspectNameToId(name);
			break;
		}
			
		case CheckList.check_types: {
			type.type_name = name;
			break;
			
		}
		case CheckList.check_status:
			status.status_name = name;
			break;
			
		case CheckList.check_pricerange:
			if(name!="ALL"){
				string priceStart = name.Replace("£","");
				priceStart = priceStart.Replace(",","");
				priceStart = priceStart.Split('-')[0];
				
				foreach (MWMPricerange price in MWM_CMS_DatabaseManager.instance.priceRanges.priceranges){
					if(priceRange.pricerange_start.ToString() == priceStart) priceRange = price;
				}
			}
			else priceRange = new MWMPricerange();
			break;
			
		default:
			break;
		}

		FilterOptionChangedAction.Invoke(filterType);

	}
	
	#endregion
	
	public void SelectApartment (GameObject apartment)
	{
		selectedApartment = apartment;
		ApartmentFound = true;
		SelectedApartmentAction.Invoke(true);
		LightingController.instance.TurnOnSingleApartment(selectedApartment);
		
		Debug.Log("Apartment choose is " + selectedApartment.GetComponent<ApartmentBlock>().unitInfo.unit_name);
	}
	
	public void DeselectApartment()
	{
		if(selectedApartment !=null)
		{	
			selectedApartment = null;
			ApartmentFound = false;
			SelectedApartmentAction.Invoke(false);
			FilterApartments(); // FILTER ALL, so they are all visible.
		}
	}
	
	
	public void SearchApartmentByName (string unitName)
	{
		selectedApartment = null;
		ApartmentFound = false;
		
		Debug.Log ("Searching for unit " + unitName);
		foreach (ApartmentBlock apartment in allSceneApartments)	
		{
			if(apartment.unitInfo.unit_name == unitName) SelectApartment(apartment.gameObject);
		}
		
		ApartmentInfoBar.instance.ToggleApartmentInfoBar(!ApartmentFound); 
		
		if(ApartmentFound) SearchbyNameAction.Invoke(unitName);
		else SelectedApartmentAction(false);
		
		
		//		if(ModeSwitch.instance.AFModeIsInAR){
		//			
		//			ModeSwitch.instance.AFModeIsInAR=false;
		//			ModeSwitch.instance._SwitchToAFinder(true);
		//			ModeSwitch.instance.AFModeIsInXray =true;
		//
		//			CameraControl.instance.rotationInertia = 1f;
		//		}
		
	}
	
	public void CollectAllApartmentsInScene (){
		allSceneApartments.Clear ();
		foreach (ApartmentBlock apartment in VolumesParent.GetComponentsInChildren<ApartmentBlock> ()) {
			allSceneApartments.Add (apartment);
		}
	}
	
	#region CONVERSION
	public string BlockIdToName (int id){
		foreach (MWMBlock block in MWM_CMS_DatabaseManager.instance.blocks.blocks) {
			if(block.block_id == id) return block.block_name;
		}
		return "Error!";
	}

	public int BlockNameToId (string name){
		foreach (MWMBlock block in MWM_CMS_DatabaseManager.instance.blocks.blocks){
			if (block.block_name == name) return block.block_id;
		}
		return 0;
	}

	public string FloorIdToName (int id){
		foreach (MWMFloor floor in MWM_CMS_DatabaseManager.instance.floors.floors) {
			if(floor.floor_id == id) return floor.floor_name;
		}
		return "Error!";
	}

	public int CoreNameToBlockID(string name){
		foreach (MWMCore core in MWM_CMS_DatabaseManager.instance.core.cores) {
			if (core.core_name == name) return core.core_block;
		}
		return 0;
	}

	public string CoreNameToBlockName(string name){
		return BlockIdToName(CoreNameToBlockID(name));
	}
	
	public string CoreIdToName (int id)
	{
		foreach (MWMCore core in MWM_CMS_DatabaseManager.instance.core.cores) {
			if(core.core_id == id) return core.core_name;
		}
		return "Error!";
	}
	
	int FloorNameToId (string name){
		foreach (MWMFloor floor in MWM_CMS_DatabaseManager.instance.floors.floors){
			if(floor.floor_core == ApartmentManager.instance.core.core_id && floor.floor_name == name )
				return floor.floor_id;
		}
		return 0;
	}
	
	int BedroomNameToId (string name){
		foreach (MWMBedroom bedroom in MWM_CMS_DatabaseManager.instance.bedrooms.bedrooms) {
			if(bedroom.bedroom_name == name) return bedroom.bedroom_id;
		}
		return 0;
	}
	
	int AspectNameToId (string name){
		foreach (MWMAspect aspect in MWM_CMS_DatabaseManager.instance.aspects.aspects){
			if(aspect.aspect_name == name) return aspect.aspect_id;
		}
		return 0;
	}
	#endregion
	
	
	public void PlayerPrefStatusPriceChanged (bool isOn, PrefToggleType type)
	{

		PlayerPrefChangeAction.Invoke(isOn,type);

	}
}