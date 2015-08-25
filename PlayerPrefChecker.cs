using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//checks the player pref see if itself should be on, also react to player pef change action event on manager

public class PlayerPrefChecker : MonoBehaviour {

	public PrefToggleType thisType;

	public bool shouldShow;


	void Start ()

	{

		ApartmentManager.instance.PlayerPrefChangeAction += HandlePlayerPrefChangeAction;

		switch (thisType) {
			
		case PrefToggleType.price: shouldShow = PlayerPrefs.GetString("ShowPrice") == "True";break;
			
		case PrefToggleType.status : shouldShow = PlayerPrefs.GetString("ShowStatus") == "True";break;
			
		default:
			break;
		}
		
		
		GetComponent<ToggleCanvasGroup>().DoToggleCancasGroup(shouldShow);
		
		GetComponent<LayoutElement>().ignoreLayout = !shouldShow;

	}

	void OnEnable () {

		ApartmentManager.instance.PlayerPrefChangeAction += HandlePlayerPrefChangeAction;

		switch (thisType) {

		case PrefToggleType.price: shouldShow = PlayerPrefs.GetString("ShowPrice") == "True";break;
		
		case PrefToggleType.status : shouldShow = PlayerPrefs.GetString("ShowStatus") == "True";break;

		default:
			break;
		}


		GetComponent<ToggleCanvasGroup>().DoToggleCancasGroup(shouldShow);
		
		GetComponent<LayoutElement>().ignoreLayout = !shouldShow;

	
	}

	void OnDestroy ()
	{

		ApartmentManager.instance.PlayerPrefChangeAction -= HandlePlayerPrefChangeAction;

	}

	void HandlePlayerPrefChangeAction (bool toggle, PrefToggleType type)
	{

		shouldShow = toggle;

		if(thisType == type)

		{

			GetComponent<ToggleCanvasGroup>().DoToggleCancasGroup(shouldShow);

			GetComponent<LayoutElement>().ignoreLayout = !shouldShow;

		}

	}
	

}
