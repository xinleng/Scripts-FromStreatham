using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using MWM.JSON.Standard;

[ExecuteInEditMode]
public class LightingController : MonoBehaviour {
	
	new public bool enabled;
	public static LightingController instance;
	
	#region DMX varaiables	
	public string lightControllerAddress;
	public string LightControllerAddress {//making this read only
		get {	return lightControllerAddress;}
		set { 	lightControllerAddress = value;
				PlayerPrefs.SetString("LightControlAddress",lightControllerAddress);}
	}
	public List  <string> targetDMXaddress;	
	public bool useMultipleStrings;

	#region apartmentTypeColors
	public Color LEDColorSIngle;
	#endregion

	public Color HighLightColor;

	public DMXAddressTestingManager DMXlist;
	public List<string> ledCommandQueue;
	public string lastLEDCommand;

	public void AddLedCommandToQueue (string newCommand)
	{
		Debug.Log (" new Led command added ");
		
		if(newCommand != lastLEDCommand)
		{
			if (ledCommandQueue.Count == 0) ledCommandQueue.Add(newCommand);
			else if(ledCommandQueue[ledCommandQueue.Count-1]!=newCommand) ledCommandQueue.Add(newCommand);
		}
		lastLEDCommand = newCommand;
		StartCoroutine(StartLEDCommandQueue());
		
	}
	
	public IEnumerator ExecuteLEDCommandQueue ()
	{
		WWW www = new WWW (ledCommandQueue[0]);
		Debug.Log (ledCommandQueue[0]);

		if(ledCommandQueue.Count>0)
		{
			Debug.Log ("Removing the first line of command now  ");
			ledCommandQueue.RemoveRange(0,1);
		}
		yield return www;
		
	}
	
	public IEnumerator StartLEDCommandQueue()
	{
		Debug.Log ("Led command queue started  ");
		while(true)
		{
			if(ledCommandQueue.Count>0)	yield return StartCoroutine(ExecuteLEDCommandQueue());
			yield return 0;
		}
	}
	
	void Start ()
	{
		if(!String.IsNullOrEmpty(PlayerPrefs.GetString("LightControlAddress")))	
			LightControllerAddress = PlayerPrefs.GetString("LightControlAddress");

		StartCoroutine(StartLEDCommandQueue());
		CurrentDMXMode = DMXMode.scene;
	}
	
	public enum  DMXMode{
		led, //application controlled mode
		scene // automatic demo mode
	}

	public DMXMode CurrentDMXMode {
		get {	return currentDMXMode;}
		set {	currentDMXMode = value;
			if (currentDMXMode == DMXMode.scene) SetLEDScene("1");
			else SwitchLEDMode(currentDMXMode);}
	}

	DMXMode currentDMXMode;

	public void SwitchLEDsOff () // maybe dont allow this to be done from outside
	{
		if (enabled) {
			string url = "http://" + lightControllerAddress + "/LightControl/controllights.aspx?cmd=setallledoff";
			ledCommandQueue.Add(url);
		}
	}
	
	public void SwitchLEDMode (DMXMode mode) 
	{	
		if (enabled) {
			string url = "http://" + lightControllerAddress + "/LightControl/controllights.aspx?cmd=setmode&mode=" + mode.ToString ();
			ledCommandQueue.Add(url);
		} 
	}
	
	public void SwitchLEDsOn (string DMXadress ,Color targetLedColor ) {

		if (enabled && !string.IsNullOrEmpty(DMXadress)) 
		{
			string address = lightControllerAddress;
			var colorValueInt = ConvertColor (targetLedColor);		
			string url = "http://" + address + "/LightControl/controllights.aspx?cmd=switchled&id=" + DMXadress + "&r=" + colorValueInt [0] + "&g=" + colorValueInt [1] + "&b=" + colorValueInt [2]+"&alloff=true";
			AddLedCommandToQueue(url);
		}
	}
	
	static Vector3 ConvertColor (Color targetLedColor)
	{
		Vector4 colorValueFloat = new Vector4 (targetLedColor.r * 255, targetLedColor.g * 255, targetLedColor.b * 255);
		Vector3 colorValueInt = new Vector3 (colorValueFloat [0].RoundToNearest (1), colorValueFloat [1].RoundToNearest (1), colorValueFloat [2].RoundToNearest (1));
		return colorValueInt;
	}	
	
	/// Gets the target DMX address. Generate string for all the types of apartment currently showing
	/// each differnt color in a different string as the trun on LED command only deals with one type of color at a time.
	string GetTargetDMXAddress ( string targetLedColor )
	{
		string _dmxAddress = "";
		if(_dmxAddress.Length>1) return _dmxAddress.Substring(0,_dmxAddress.Length-1);
		else return null;
	}
	
	public void GenerateDMXAddressStrings ()
	{
	
		targetDMXaddress.Clear ();
		Debug.Log ("Generating new addresses");
		targetDMXaddress.Add(GenerateDMXAddressForAllVisible()); // Filtered?
	
	}

	public void TurnOnLEDSets ()
	{

		GenerateDMXAddressStrings ();	
		if(targetDMXaddress[0]!=null) SwitchLEDsOn(targetDMXaddress [0], LEDColorSIngle);
		else SwitchLEDsOff(); // if address is empty then should turn off
	
	}

	public void TurnOnSingleApartment (GameObject selectedApartment)
	{
		MWMUnit unitInfo = selectedApartment.GetComponent<ApartmentBlock>().unitInfo;

		Debug.Log ("Turning on single Apartment LEDs");
	
		switch (LightingController.instance.DMXlist.dMXOption)
		{
		case DMXOption.A:
			SwitchLEDsOn (unitInfo.unit_DMX_A, HighLightColor);
			break;
		case DMXOption.B:
			SwitchLEDsOn (unitInfo.unit_DMX_B, HighLightColor);
			break;
		case DMXOption.C:
			SwitchLEDsOn (unitInfo.unit_DMX_C, HighLightColor);
			break;
		default: break;
		}
	}

	//Filtered?
	string GenerateDMXAddressForAllVisible ()// this is key to use 
	{
		Debug.Log ("Starting generating DMX addresses for all visible");
		string _dmxAddress = "";
		
		if(DMXlist.dMXOption == DMXOption.A)
		{
			foreach (ApartmentBlock apartment in ApartmentManager.instance.allSceneApartments) {
				if(apartment.GetComponent<Renderer>().enabled ) 
					_dmxAddress = _dmxAddress + apartment.unitInfo.unit_DMX_A + "+";
			}
		}
		
		if(DMXlist.dMXOption == DMXOption.B)
		{
			foreach (string address in DMXlist.DMXListOrdered){				
				foreach (ApartmentBlock apartment in ApartmentManager.instance.allSceneApartments) {
					if(apartment.GetComponent<Renderer>().enabled && apartment.unitInfo.unit_DMX_B == address )
						_dmxAddress = _dmxAddress + address + "+";		
				}
			}
		}
		
		if(DMXlist.dMXOption == DMXOption.C)	
		{
			foreach (string address in DMXlist.DMXListOrdered){
				foreach (ApartmentBlock apartment in ApartmentManager.instance.allSceneApartments) {
					if(apartment.GetComponent<Renderer>().enabled && apartment.unitInfo.unit_DMX_C == address )
						_dmxAddress = _dmxAddress + address + "+";
				}
			}
		}

		if(_dmxAddress.Length>1) return _dmxAddress.Substring(0,_dmxAddress.Length-1);
		else return null;
		
	}
	
	public void SetLEDScene(string sceneName)
	{
		if(enabled)
		{
			SwitchLEDMode(DMXMode.scene);
			string url = "http://" + lightControllerAddress + "/LightControl/controllights.aspx?cmd=setscene&scene="+sceneName+"&status=on";
			ledCommandQueue.Add(url);
		}
	}

	#endregion

	void OnEnable () {	
		instance = this;
	}

	public void SetLEDMode(string ledMode)
	{
		
		switch(ledMode)
		{
		case "LED":		
			CurrentDMXMode = DMXMode.led;
			break;
			
		case "Scene":
			CurrentDMXMode = DMXMode.scene;
			break;
			
		default:
			break;
		}
	}
}