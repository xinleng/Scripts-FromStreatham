using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LinqTools;
using System;
using DaikonForge.Tween.Components;
using MWM.Gallery;

//1.01 xin -- public void InitiliseListOptimized(bool toggle) // added bool is we can call this from a dynamic onValue changed via a toggle 

public class ListContent : MonoBehaviour {
	
	#region VARIABLES
	public enum SortingMethod{
		name,
		beds,
		floor,
		area,
		price,
		status,
		none
	}
	
	public ApartmentManager apartmentManager;
	public GameObject listItemPrefab;
	
	private SortingMethod sortingby;
	public SortingMethod SortingBy {
		set{
			if(sortingby!=value){
				sortingby = value;
			}
		}
	}
	
	[Header ("Favourites List")]
	public bool isFavouriteList = false;
	public GameObject favouriteGalleryPanel;
	
	public List<ApartmentBlock> listToUse;
	#endregion
	
	public void _CleanListView (){
		Debug.Log ("start clean");
		foreach (Transform child in transform) Destroy (child.gameObject);
		Debug.Log ("Finish clean");
	}
	
	#region LIST LOAD
	// Generates visible list items in one frame and then starts a coroutine to fill in the rest
	[Header ("Load Progressively")]
	public GameObject LoadingPanel;
	public int batch1No = 100;
	public int firstLoad = 10;
	public int coroutLinesPerFrame = 100;
	
	public void _GenerateListOnClick(bool toggle){ //1.01 xin added bool is we can call this from a dynamic onValue changed via a toggle  
		if(toggle){
			listToUse = isFavouriteList ? apartmentManager.favoritedApartments : apartmentManager.filteredApartments;
			Debug.Log ("List to use:" + listToUse.Count);
			if (isFavouriteList) DoSortListViewBy (SortingMethod.none, false, listToUse);
			else DoSortListViewBy (SortingMethod.name, false, listToUse);
			Debug.Log ("GenerateListOnClick done!");
		}
	}
	
	public void DoSortListViewBy (SortingMethod sortingMethod, bool sortDecending, List<ApartmentBlock> list){
		//Debug.Log ("DO SORT LIST VIEW BY");
		_CleanListView(); 
		if (sortingMethod == SortingMethod.none){
			InitiliseListInternal(list);
		}
		
		if (sortingMethod == SortingMethod.price){
			//put al filterted apartment in a dictionary ready for sorting
			Dictionary<ApartmentBlock,string> apartmentToSort= new Dictionary<ApartmentBlock,string>();
			foreach (ApartmentBlock apartments in list){
				apartmentToSort.Add(apartments, apartments.unitInfo.unit_price.ToString().Replace(",",""));
			}
			Debug.Log ("Start");
			SortActionString(sortDecending, apartmentToSort);
		}
		
		//-------------------
		
		if (sortingMethod == SortingMethod.floor){
			//put al filterted apartment in a dictionary ready for sorting
			Dictionary<ApartmentBlock,string> apartmentToSort= new Dictionary<ApartmentBlock,string>();
			foreach (ApartmentBlock apartments in list){
				if (apartments.unitInfo.unitFloorName == "G") apartments.unitInfo.unitFloorName = "00";
				Debug.Log (apartments.unitInfo.unitFloorName);
				apartmentToSort.Add(apartments, apartments.unitInfo.unitFloorName );
			}
			Debug.Log ("Start");
			SortActionString(sortDecending, apartmentToSort);
		}
		
		//-------------------
		
		if (sortingMethod == SortingMethod.area){
			//put al filterted apartment in a dictionary ready for sorting
			Dictionary<ApartmentBlock,int> apartmentToSort= new Dictionary<ApartmentBlock,int>();
			foreach (ApartmentBlock apartments in list){
				
				if (String.IsNullOrEmpty(apartments.unitInfo.unit_sqft)) apartments.unitInfo.unit_sqft = "0";
				
				apartmentToSort.Add(apartments, Convert.ToInt32 (apartments.unitInfo.unit_sqft.Replace(",","")) );	
			}
			Debug.Log ("Start");
			SortActionInt(sortDecending, apartmentToSort);
		}
		
		//-------------------
		
		if (sortingMethod == SortingMethod.beds){
			//put al filterted apartment in a dictionary ready for sorting
			Dictionary<ApartmentBlock,string> apartmentToSort= new Dictionary<ApartmentBlock,string>();
			foreach (ApartmentBlock apartments in list){
				apartmentToSort.Add(apartments, apartments.unitInfo.unitBedroomName);
			}
			Debug.Log ("Start");
			SortActionString(sortDecending, apartmentToSort);
		}
		
		//-------------------
		
		if (sortingMethod == SortingMethod.name){
			//put al filterted apartment in a dictionary ready for sorting
			Dictionary<ApartmentBlock,string> apartmentToSort= new Dictionary<ApartmentBlock,string>();
			foreach (ApartmentBlock apartments in list){
				apartmentToSort.Add(apartments,apartments.unitInfo.unit_name);
			}
			Debug.Log ("Start");
			Debug.Log ("Apartment to sort:" + apartmentToSort.Count);
			SortActionString(sortDecending, apartmentToSort);
			
		}
		
		//-------------------
		
		if (sortingMethod == SortingMethod.status){
			//put al filterted apartment in a dictionary ready for sorting
			Dictionary<ApartmentBlock,int> apartmentToSort= new Dictionary<ApartmentBlock,int>();
			foreach (ApartmentBlock apartments in list){
				apartmentToSort.Add(apartments, apartments.unitInfo.unit_status);
			}
			Debug.Log ("Start");
			SortActionInt(sortDecending, apartmentToSort);
		}
	}
	
	
	List<ApartmentBlock> keyList = new List<ApartmentBlock>();
	void SortActionString(bool sortDecendingInt, Dictionary<ApartmentBlock,string> apartmentToSortInt){
		this.keyList.Clear();
		
		if(sortDecendingInt){
			
			foreach (KeyValuePair<ApartmentBlock,string> apartment in apartmentToSortInt.OrderByDescending(key=>key.Value)){
				this.keyList.Add (apartment.Key);
			}
			Debug.Log ("initialize internal");
			InitiliseListInternal(keyList);
			//keyList.Clear ();
			
		}else{
			foreach (KeyValuePair<ApartmentBlock,string> apartment in apartmentToSortInt.OrderBy(key=>key.Value)){
				this.keyList.Add (apartment.Key);
			}
			Debug.Log ("initialize internal");
			InitiliseListInternal(keyList);
			//keyList.Clear ();
		}
	}
	
	void SortActionInt(bool sortDecendingInt, Dictionary<ApartmentBlock,int> apartmentToSortInt){
		this.keyList.Clear();
		if(sortDecendingInt){
			foreach (KeyValuePair<ApartmentBlock,int> apartment in apartmentToSortInt.OrderByDescending(key=>key.Value)){
				this.keyList.Add (apartment.Key);
			}
			InitiliseListInternal(keyList);
			//keyList.Clear ();
			
		}else{
			foreach (KeyValuePair<ApartmentBlock,int> apartment in apartmentToSortInt.OrderBy(key=>key.Value)){
				this.keyList.Add (apartment.Key);
			}
			InitiliseListInternal(keyList);
			//keyList.Clear ();
		}
	}
	
	// load firs batch in one frame.
	void InitiliseListInternal(List<ApartmentBlock> List){
		
		_CleanListView ();
		if (List.Count < batch1No){ 
			for (int firstBatchLow = 0; firstBatchLow < List.Count; firstBatchLow++){ // Load amout smaller than first batch
				InitialiseLine(List[firstBatchLow]);
			}
		}else if (List.Count >= batch1No){
			for (int firstBatch = 0; firstBatch < firstLoad; firstBatch++){ // Load amout bigger than first batch
				InitialiseLine(List[firstBatch]);
			}
			TurnOnLoadingScreen();
			StartCoroutine (CoroutineInitialiseLines(coroutLinesPerFrame, List));
		}
		favouriteID = 0;
	}
	
	public int textDisplayApartmentNo = 0; // used for loading screen text
	IEnumerator CoroutineInitialiseLines(int lineNoPerFrame, List<ApartmentBlock> List){
		
		textDisplayApartmentNo = 0;
		for (int i = firstLoad; i < List.Count - 1; i+=lineNoPerFrame) { // 10, 110, 210, 310, .., 810.
			Debug.Log ("I : " + i);
			if ( (i + lineNoPerFrame) <= List.Count - 1){
				for (int a = 0; a < lineNoPerFrame; a++) {
					textDisplayApartmentNo = i + a;
					InitialiseLine(List[textDisplayApartmentNo]);
					
				}
			} else {
				int newMax = lineNoPerFrame - List.Count - 1;
				for (int b = 0; b < newMax; b++) {
					textDisplayApartmentNo = i + b;
					InitialiseLine(List[textDisplayApartmentNo]);
					
				}
			}
			yield return null;
		}
		TurnOffLoadingScreen();
	}
	
	int favouriteID = 0;
	void InitialiseLine(ApartmentBlock aptBlock){
		//		Debug.Log ("LINE");
		
		GameObject listbutton = Instantiate(listItemPrefab) as GameObject;
		listbutton.transform.SetParent(this.transform, false);
		listbutton.name = aptBlock.unitInfo.unit_name;
		//listbutton
		
		listbutton.GetComponent<ApartmentBlockListButton>().unitInfo = aptBlock.unitInfo;
		listbutton.GetComponent<ApartmentBlockListButton>().UpdateListButtonInfo();
		
		if (isFavouriteList){
			listbutton.GetComponent<SetFavouriteGallery>().selectedBlock = aptBlock;
			listbutton.GetComponent<SetFavouriteGallery>().selectedBlockIDinFavouriteContent = favouriteID;
			favouriteID++;
		} 
	}
	
	void TurnOnLoadingScreen(){
		if (LoadingPanel) LoadingPanel.SetActive(true);
	}
	void TurnOffLoadingScreen(){
		if (LoadingPanel) LoadingPanel.GetComponent<TweenFloatProperty>().Play();
	}
	#endregion
}





































