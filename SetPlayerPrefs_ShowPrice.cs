using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum PrefToggleType 
{	
	price,
	status
}

public class SetPlayerPrefs_ShowPrice : MonoBehaviour {

	public PrefToggleType type;
	void Start () {

		ApartmentManager.instance.PlayerPrefChangeAction += HandlePlayerPrefChangeAction;

		if(type == PrefToggleType.price) 
		{
			if(!string.IsNullOrEmpty(PlayerPrefs.GetString("ShowPrice")))		
			{
				switch (PlayerPrefs.GetString("ShowPrice")) {
				case "True" : GetComponent<Toggle>().isOn = true;
					break;
				case "False": GetComponent<Toggle>().isOn = false;
					break;
				default:
					break;
				}
			}
		}
		else 
		{
			if(!string.IsNullOrEmpty(PlayerPrefs.GetString("ShowStatus")))		
			{
				switch (PlayerPrefs.GetString("ShowStatus")) {
				case "True" : GetComponent<Toggle>().isOn = true;
					break;
				case "False": GetComponent<Toggle>().isOn = false;
					break;
				default:
					break;
				}
			}
		}
	}

	void HandlePlayerPrefChangeAction (bool arg1, PrefToggleType arg2)
	{
		 Debug.Log ("Status and price player pref has been chagned");
	}

	public void SetPlayerPrefsStatusPrice ()
	{
		if(type == PrefToggleType.price) PlayerPrefs.SetString("ShowPrice",GetComponent<Toggle>().isOn.ToString());
		else PlayerPrefs.SetString("ShowStatus",GetComponent<Toggle>().isOn.ToString());

		ApartmentManager.instance.PlayerPrefStatusPriceChanged(GetComponent<Toggle>().isOn,type);

		Debug.Log ("Pref set to " + GetComponent<Toggle>().isOn.ToString());
	}
}