using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using MWM.JSON.Standard;

public class ApartmentBlock:MonoBehaviour { 

	Material orginalMat;

	public bool visible;
	public bool highlighted;
	public bool favorited;
	public bool clickAble;
	public MWMUnit unitInfo;

	public void Start ()
	{
		orginalMat = this.GetComponent<Renderer>().sharedMaterial;

		ApartmentManager.instance.FilterApartmentsAction += HandleFilterApartments;
		ApartmentManager.instance.SearchbyNameAction += ApartmentManager_SearchbyName;
		ApartmentManager.instance.SelectedApartmentAction += HandleSelectedApartment;

	}

	//=========================== OK =========================
	void OnClick ()
	{
		
		Debug.Log ("OnClick received " + this.name);
		
		foreach (ApartmentBlock apartments in ApartmentManager.instance.allSceneApartments){
			apartments.GetComponent<Renderer>().sharedMaterial =apartments.orginalMat;
		}
		this.GetComponent<Renderer>().sharedMaterial = ApartmentManager.instance.highlightMat;
		
		ApartmentManager.instance.SelectApartment(this.gameObject);
		
	}

	//=========================== OK =========================
	void HandleFilterApartments (bool obj)
	{
		visible = true;
		
		if(ApartmentManager.instance.block.block_name !="ALL"&& visible)
			visible = ApartmentManager.instance.block.block_name == unitInfo.unitBlockName;

		if(ApartmentManager.instance.core.core_name !="ALL"&& visible)
			visible = ApartmentManager.instance.core.core_name == unitInfo.unitCoreName;
		if(ApartmentManager.instance.floor.floor_name !="ALL"&& visible)	
			visible = ApartmentManager.instance.floor.floor_name == unitInfo.unitFloorName;
		
		if( ApartmentManager.instance.bedroom.bedroom_name!="ALL" && visible)
			visible = ApartmentManager.instance.bedroom.bedroom_name==unitInfo.unitBedroomName;
		
		if( ApartmentManager.instance.aspect.aspect_name!="ALL" && visible)
			visible = unitInfo.unit_aspect.Contains(ApartmentManager.instance.aspect.aspect_name.Substring(0,1));	
		
		if( ApartmentManager.instance.status.status_name!="ALL" && visible)
			visible = ApartmentManager.instance.status.status_name==unitInfo.unitStatusName;	
		
		if( ApartmentManager.instance.priceRange.pricerange_start!= 0 && visible)
		{
			int unitPrice = 0;
			if(unitInfo.unit_price !="0")
			{
				Debug.Log(this.name +" " + unitInfo.unit_price);
				unitPrice =Convert.ToInt32(unitInfo.unit_price.Replace(",",""));
			}
			
			if(ApartmentManager.instance.priceRange.pricerange_start <= unitPrice && unitPrice <= ApartmentManager.instance.priceRange.pricerange_end)
				visible = true;
			else visible = false;
		}
		
		this.GetComponent<Renderer>().material = orginalMat;
		ToggleApartmentVisiblity(visible);
		if(visible) ApartmentManager.instance.filteredApartments.Add(this);
	}

	//=========================== OK =========================
	void HandleSelectedApartment (bool selectedApartment)
	{
		if(selectedApartment)		
		{
			if(ApartmentManager.instance.selectedApartment.name == this.gameObject.name)
			{
				ToggleApartmentVisiblity(true);
				this.GetComponent<Renderer>().material = ApartmentManager.instance.highlightMat;
			}			
		}
		else 
		{
			ToggleApartmentVisiblity(visible);
			this.GetComponent<Renderer>().material = orginalMat;
		}
	}

	void ApartmentManager_SearchbyName(string unitName)
	{
		
		if (unitInfo.unit_name == unitName)
		{
			Debug.Log("Apartment found: " + unitInfo.unit_name);
			ToggleApartmentVisiblity(true);
			
			this.GetComponent<Renderer>().material = ApartmentManager.instance.highlightMat;

			ApartmentManager.instance.SelectApartment(this.gameObject);
			
			this.GetComponent<SetCameraPositionOnClick>().OnClick();

		}
		else 
		{
			this.GetComponent<Renderer>().material = orginalMat;
			ToggleApartmentVisiblity(false);
		}
	}

	public void ToggleApartmentVisiblity (bool visible)
	{
		this.GetComponent<Renderer>().enabled = visible;
		this.GetComponent<Collider>().enabled = visible;
	}
}