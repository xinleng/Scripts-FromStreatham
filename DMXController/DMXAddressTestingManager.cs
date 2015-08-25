using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MWM.JSON.Standard;
using UnityEngine.UI;

public enum DMXOption {
	A,B,C
}

[ExecuteInEditMode]
public class DMXAddressTestingManager : HouseInfoPasser {

	public DMXOption dMXOption;// this should be the only place that we need to change the DMX options
	
	public Toggle optionA;
	public Toggle optionB;
	public Toggle optionC;

	public static DMXAddressTestingManager instance;

	int currentDMXIndex;
	public int CurrentDMXIndex {
		get { return currentDMXIndex;}
		set { currentDMXIndex = value;}
	}

	public List<string> DMXList;
	public List<string> DMXListOrdered;
	public List<int> tempAddressInt;

	string allDMXaddress;

	void OnEnable() {
		instance = this;
	}

	void Start () {
		instance = this;
		string dmxOption = PlayerPrefs.GetString("dMXOption");

		if(!String.IsNullOrEmpty(dmxOption)){
			switch (dmxOption) {
			case "A":dMXOption = DMXOption.A; 
						optionA.isOn = true;
						break;
			case "B":dMXOption = DMXOption.B;
						optionB.isOn = true;
						break;
			case "C":dMXOption = DMXOption.C;
						optionC.isOn = true;
						break;
			default:
			break;
			}
		}
		GetAllDMXAddress();
	}

	void Update () {
		if(DoSetUpNow) {
			Debug.Log("Setting up DMX");
			GetAllDMXAddress();
			OrderDMXAddress();
			DoSetUpNow = false;
		}
	}

	public void GetAllDMXAddress (){

		DMXList.Clear();
		DMXListOrdered.Clear();

		switch (dMXOption) {
			case DMXOption.A:{
				foreach (MWMUnit unit in MWM_CMS_DatabaseManager.instance.units.units){
					if(!DMXList.Contains(unit.unit_DMX_A)) DMXList.Add(unit.unit_DMX_A);
				}
				break;
			}

			case DMXOption.B:{
				foreach (MWMUnit unit in MWM_CMS_DatabaseManager.instance.units.units){
					if(!DMXList.Contains(unit.unit_DMX_B)) DMXList.Add(unit.unit_DMX_B);
				}
				break;
			}

			case DMXOption.C:{
				
				foreach (MWMUnit unit in MWM_CMS_DatabaseManager.instance.units.units){
					if(!DMXList.Contains(unit.unit_DMX_C)) DMXList.Add(unit.unit_DMX_C);
				}
				break;
			}

			default:
				break;
			}
		Debug.Log ("DMX List has " + DMXList.Count + " addresses");
	}

	public void OrderDMXAddress(){

		tempAddressInt.Clear();
		foreach (string address in DMXList) {
			if(address.Length>1) tempAddressInt.Add(Int32.Parse(address.Substring(2)));
			else tempAddressInt.Add(Int32.Parse(address));
		}

		tempAddressInt.Sort();
		Debug.Log (tempAddressInt);
		foreach (int dmxNumber in tempAddressInt){
			DMXListOrdered.Add("1."+dmxNumber.ToString());
		}
	}

	public void SwitchDMXOption (string optionName){
		Debug.Log ("Switching DMX option to Model " + optionName);
		switch (optionName) {

		case "A":
			dMXOption = DMXOption.A;
			PlayerPrefs.SetString("dMXOption","A");
			break;
		case "B":
			dMXOption = DMXOption.B;
			PlayerPrefs.SetString("dMXOption","B");
			break;
		case "C":
			dMXOption = DMXOption.C;
			PlayerPrefs.SetString("dMXOption","C");
			break;
		default:
			break;
		}
		GetAllDMXAddress();	
		OrderDMXAddress();
	}
}