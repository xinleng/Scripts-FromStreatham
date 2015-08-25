using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MWM.JSON.Standard;

public class FilterOptionList : MonoBehaviour {
	
	public CheckList filterOption;
	public List<string> valueList;
	public GameObject buttonPrefab;
	public Text textDisplay;
	public Toggle dropdownToggle;
	
	void Start () {

		ApartmentManager.instance.FilterOptionChangedAction += HandleFilterOptionChangedAction;
		CreateButtons();
	}

	void HandleFilterOptionChangedAction (CheckList Option)
	{
		//Debug.Log (" filter option changed received by filter button " + filterOption.ToString());
		//Logic to decide which filter needs to regenerate
		switch (Option) {
		
		case CheckList.check_blocks:
			if (filterOption !=  CheckList.check_blocks) CreateButtons() ;
			break;
		case CheckList.check_cores:
			if (filterOption != CheckList.check_blocks && filterOption != CheckList.check_cores) CreateButtons();
			break;
		case CheckList.check_floors:
			if ((filterOption != CheckList.check_blocks && filterOption != CheckList.check_cores) 
			    && filterOption != CheckList.check_floors ) 
					CreateButtons();
			break;

		default:
			break;
		}	
	}
	
	public void CreateButtons(){

		Debug.Log ("Generating buttons for  " + filterOption.ToString());
		if (transform.childCount != 0){ 
			foreach (FilterButtons child in GetComponentsInChildren<FilterButtons>()) {
				Destroy (child.gameObject);	
			}
		}
		
		GenerateValueList(filterOption);
		InstantiateButtons(buttonPrefab);
		
	}
	
	void OnEnable()
	{

		Debug.Log (ApartmentManager.instance);
		switch (filterOption) {
			
		case CheckList.check_floors:
			textDisplay.text = ApartmentManager.instance.floor.floor_name;
			break;
		case CheckList.check_bedrooms:
			textDisplay.text = ApartmentManager.instance.bedroom.bedroom_name;
			break;
		case CheckList.check_types:
			textDisplay.text = ApartmentManager.instance.type.type_name;
			break;
			
		case CheckList.check_aspects:
			textDisplay.text = ApartmentManager.instance.aspect.aspect_name;
			break;
			
		case CheckList.check_status:
			textDisplay.text = ApartmentManager.instance.status.status_name;
			break;
			
		case CheckList.check_blocks:
			textDisplay.text = ApartmentManager.instance.block.block_name;
			break;

		case CheckList.check_cores:
			textDisplay.text = ApartmentManager.instance.core.core_name;
			break;
			
		case CheckList.check_pricerange:
			textDisplay.text = ApartmentManager.instance.priceRange.pricerange_start.ToString();
			break;
			
		default:
			break;
		}
	}
	
	void GenerateValueList (CheckList filterOption)
	{
		
		valueList.Clear ();
		
		switch (filterOption) {

		case CheckList.check_blocks:
			foreach(string block in ReleaseEntityManager.instance.activeBlockList)
			{
				if(!valueList.Contains(block)) valueList.Add(block);
			}
			valueList.Insert(0, "ALL");
			
			break;
			
		case CheckList.check_cores:
			foreach(string core in ReleaseEntityManager.instance.activeCoreList)
			{
				if(!valueList.Contains(core))
				{
					if(ApartmentManager.instance.block.block_name != "ALL")  // if the block filter is set, we need to check if this core exists in that block
					{
						if (ApartmentManager.instance.CoreNameToBlockName(core) == ApartmentManager.instance.block.block_name)
							valueList.Add(core);
					}
					else valueList.Add(core);
				}
			}
			valueList.Insert(0, "ALL");
			
			break;
			
		case CheckList.check_floors:
			foreach (MWMFloor floor in MWM_CMS_DatabaseManager.instance.floors.floors)
			{
				if(!valueList.Contains(floor.floor_name))
				{
					if(ApartmentManager.instance.core.core_name!="ALL")
					{
						Debug.Log(" checking if floor exists in core " + ApartmentManager.instance.core.core_name );
						if(ApartmentManager.instance.CoreIdToName(floor.floor_core) == ApartmentManager.instance.core.core_name)
							valueList.Add(floor.floor_name);
					}
					else valueList.Add(floor.floor_name);
				}
			}

			// sort out ordering for LGs Gs
			if(valueList.Contains("LG"))
			{
				valueList.Remove("LG");
				valueList.Insert(0,"LG");
			}
			
			valueList.Insert(0, "ALL");
			break;
		case CheckList.check_bedrooms:
			foreach(MWMBedroom bedroomType in MWM_CMS_DatabaseManager.instance.bedrooms.bedrooms)
			{
				if(!valueList.Contains(bedroomType.bedroom_name))
					valueList.Add(bedroomType.bedroom_name);
			}
			valueList.Insert(0, "ALL");
			
			break;
		case CheckList.check_types:
			foreach(MWMType type in MWM_CMS_DatabaseManager.instance.types.types)
			{
				if(!valueList.Contains(type.type_name))
					valueList.Add(type.type_name);
			}
			valueList.Insert(0, "ALL");
			
			break;
			
		case CheckList.check_aspects:
			foreach(MWMAspect aspect in MWM_CMS_DatabaseManager.instance.aspects.aspects)
			{
				if(!valueList.Contains(aspect.aspect_name))
					valueList.Add(aspect.aspect_name);
			}
			valueList.Insert(0, "ALL");
			
			break;
			
		case CheckList.check_status:
			foreach(MWMStatus status in MWM_CMS_DatabaseManager.instance.status.statuses)
			{
				if(!valueList.Contains(status.status_name))
					valueList.Add(status.status_name);
			}
			valueList.RemoveAt(0);
			valueList.Insert(0, "ALL");
			
			break;
			
		case CheckList.check_pricerange:
			foreach(MWMPricerange _priceRange in MWM_CMS_DatabaseManager.instance.priceRanges.priceranges)
			{
				string newPriceRange = "£" + _priceRange.pricerange_start.ToString().Insert(3,",") +"-" + "£" +_priceRange.pricerange_end.ToString().Insert(3,",") ;
				
				if(!valueList.Contains(newPriceRange))
					valueList.Add(newPriceRange);
			}
			valueList.RemoveAt(0);
			valueList.Insert(0, "ALL");
			
			break;
			
		default:
			break;
		}
	}
	
	void InstantiateButtons (GameObject _buttonPrefab)
	{
		foreach (string value in valueList)
		{
			GameObject button = Instantiate(_buttonPrefab) as GameObject;
			button.transform.SetParent(this.transform, 	false);
			
			button.name = value;
			button.GetComponentInChildren<Text>().text = value;
			button.transform.localScale = Vector3.one;
		}
	}
	
}