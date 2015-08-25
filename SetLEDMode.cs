using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SetLEDMode : MonoBehaviour, IPointerClickHandler {

	public LightingController.DMXMode dmxMode;

	public void OnPointerClick (PointerEventData eventData)
	{

		DoSetLedMode();
	
	}

	public void DoSetLedMode () 
	{
	
		LightingController.instance.CurrentDMXMode = dmxMode;
		if(dmxMode == LightingController.DMXMode.led)MWM_CMS_DatabaseManager.instance.apartmentManager.FilterApartments();
	
	}
}
