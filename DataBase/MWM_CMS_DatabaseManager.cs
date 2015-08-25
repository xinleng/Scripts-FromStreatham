using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using MWM.JSON.Standard;
using System.Collections.Generic;
using System;
using DaikonForge.Tween.Components;

/// <summary>
/// Check status.
/// 
/// Values for tn field :
/// 
/// check_phases
/// check_blocks
/// check_cores
/// check_floors
/// check_units
/// 
/// check_aspects
/// check_bedrooms
/// check_duplex
/// check_outside
/// check_status
/// check_tenure
/// check_types
/// check_wheelchair
/// 
/// Structure
/// 
/// phases
/// |_blocks
///   |_cores
///     |_floors
///       |_units
/// 		|_type
/// 		| |_outside
/// 		| |_bedrooms
/// 		| |_wheelchair
/// 		| |_duplex
/// 		| |_aspects
/// 		|_status
/// 		|_tenure
/// 
/// </summary>
/// 

public enum CheckList {
	
	check_phases,
	check_blocks,
	check_cores,
	check_floors,
	check_units,
	check_aspects,
	check_bedrooms,
	check_duplex,
	check_outside,
	check_status,
	check_tenure,
	check_types,
	check_wheelchair,
	check_pricerange,
	check_updates
}

[ExecuteInEditMode]
public class MWM_CMS_DatabaseManager : MonoBehaviour {
	
	public ApartmentManager apartmentManager;
	
	public static MWM_CMS_DatabaseManager instance;
	
	public string TheURL;
	public string TheProject;
	public string UserName;
	public string UserPassword;
	
	public List<CheckList> checkList;
	
	public bool populateAllData;
	
	public MWMPhasesModel phases;
	public MWMBlocksModel blocks;
	public MWMCoresModel core;
	public MWMFloorsModel floors;
	public MWMUnitsModel units;
	public MWMAspectModel aspects;
	public MWMBedroomsModel bedrooms;
	public MWMDuplexModel duplexs;
	public MWMOutsideModel outside;
	public MWMStatusModel status;
	public MWMTenureModel tenure;
	public MWMTypeModel types;
	public MWMWheelchairModel wheelChairs;
	public MWMPricerangeModel priceRanges;
	public MWMUpdateModel updateAll;
	public event Action<bool> DataBaseUpdated;
	
	int currentUpdateNo = 0;
	int lastUpdateNo = 0;
	
	
	public float refreshTime = 60f;
	float timer = 0f;
	
	public PopulateApartmentBlockData populator;
	
	void Awake (){
		instance = this;
	}
	
	void Start () {
		lastUpdateNo = PlayerPrefs.GetInt (CheckList.check_updates.ToString());
		apartmentManager.CollectAllApartmentsInScene();
	}
	
	void Update () {


		if(Application.isPlaying) 
		{

			timer += Time.deltaTime;
			if (timer > refreshTime ){
				timer = 0f;
				Debug.Log ("Timer up, checking status.");
				StartCoroutine(UpdateDatabase());
			}

		}

		if(populateAllData) StartCoroutine(PopulateAllDatabase());
	}
	
	public void UpdateDatabaseEditor(){
		StartCoroutine(PopulateAllDatabase());
	}
	
	public IEnumerator UpdateDatabase(){

		yield return StartCoroutine(PopulateDatabaseElement(CheckList.check_updates));
		
		Debug.Log ("data has returned");
		currentUpdateNo = updateAll.updates[0].updates_status;

		if (currentUpdateNo > lastUpdateNo){
			StartCoroutine(PopulateAllDatabase());
			lastUpdateNo = currentUpdateNo;
		}
		else{
			StartCoroutine(PopulateUnitData_StatusAndPrice());
			lastUpdateNo = currentUpdateNo;
		}
		
	} 
	
	
	public IEnumerator PopulateAllDatabase () {
		foreach (CheckList databaseToCheck in checkList ){
			yield return StartCoroutine (PopulateDatabaseElement(databaseToCheck));
		}
		
		Debug.Log ("All database populated");
		populator.UpdateAllApartmentsInfo();
	}
	
	public IEnumerator PopulateUnitData_StatusAndPrice(){
		yield return StartCoroutine (PopulateDatabaseElement(CheckList.check_units));
		Debug.Log ("Database Unit data populated");
		populator.UpdatePriceAndStatus();
	}
	
	IEnumerator PopulateDatabaseElement (CheckList tableName){
		WWWForm form = new WWWForm();
		form.AddField("tn",tableName.ToString());
		form.AddField("pn",TheProject);
		form.AddField("un",UserName);
		form.AddField("pw",UserPassword);
		WWW download = new WWW(TheURL,form);
		//		Debug.Log ();
		yield return download;
		//Debug.Log(download.responseHeaders);
		if((!string.IsNullOrEmpty(download.error))) {
			print( "Error downloading: " + download.error );
		} else {
			
			//always save to playerPrefs and load from playerPrefs
			PlayerPrefs.SetString(tableName.ToString(),download.text);
			if (download.text.Length > 14000) Debug.LogWarning("String too LONG!!");
			else Debug.Log( download.text);
			
			switch (tableName) {
			case CheckList.check_phases:
				phases = JsonConvert.DeserializeObject<MWMPhasesModel>(PlayerPrefs.GetString(CheckList.check_phases.ToString()));
				break;
			case CheckList.check_blocks:
				blocks = JsonConvert.DeserializeObject<MWMBlocksModel>(PlayerPrefs.GetString(CheckList.check_blocks.ToString()));
				break;
			case CheckList.check_cores:
				core = JsonConvert.DeserializeObject<MWMCoresModel>(PlayerPrefs.GetString(CheckList.check_cores.ToString()));
				break;
			case CheckList.check_floors:
				floors = JsonConvert.DeserializeObject<MWMFloorsModel>(PlayerPrefs.GetString(CheckList.check_floors.ToString()));
				break;
			case CheckList.check_units:
				units = JsonConvert.DeserializeObject<MWMUnitsModel>(PlayerPrefs.GetString(CheckList.check_units.ToString()));
				break;
			case CheckList.check_aspects:
				aspects = JsonConvert.DeserializeObject<MWMAspectModel>(PlayerPrefs.GetString(CheckList.check_aspects.ToString()));
				break;
			case CheckList.check_bedrooms:
				bedrooms = JsonConvert.DeserializeObject<MWMBedroomsModel>(PlayerPrefs.GetString(CheckList.check_bedrooms.ToString()));
				break;
			case CheckList.check_duplex:
				duplexs = JsonConvert.DeserializeObject<MWMDuplexModel>(PlayerPrefs.GetString(CheckList.check_duplex.ToString()));
				break;
			case CheckList.check_outside:
				outside = JsonConvert.DeserializeObject<MWMOutsideModel>(PlayerPrefs.GetString(CheckList.check_outside.ToString()));
				break;
			case CheckList.check_status:
				status = JsonConvert.DeserializeObject<MWMStatusModel>(PlayerPrefs.GetString(CheckList.check_status.ToString()));
				break;
			case CheckList.check_tenure:
				tenure = JsonConvert.DeserializeObject<MWMTenureModel>(PlayerPrefs.GetString(CheckList.check_tenure.ToString()));
				break;
			case CheckList.check_types:
				types = JsonConvert.DeserializeObject<MWMTypeModel>(PlayerPrefs.GetString(CheckList.check_types.ToString()));
				break;
			case CheckList.check_wheelchair:
				wheelChairs = JsonConvert.DeserializeObject<MWMWheelchairModel>(PlayerPrefs.GetString(CheckList.check_wheelchair.ToString()));
				break;
			case CheckList.check_pricerange:
				priceRanges = JsonConvert.DeserializeObject<MWMPricerangeModel>(PlayerPrefs.GetString(CheckList.check_pricerange.ToString()));
				break;
			case CheckList.check_updates:
				updateAll = JsonConvert.DeserializeObject<MWMUpdateModel>(PlayerPrefs.GetString(CheckList.check_updates.ToString()));
				PlayerPrefs.SetInt(tableName.ToString(), updateAll.updates[0].updates_status);
				
				break;
			default:
				break;
			}
			Debug.Log ("done");
		}
	}
	
}