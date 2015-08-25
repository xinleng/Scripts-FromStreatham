using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InfoButtonBack : MonoBehaviour, IPointerClickHandler {
	
	public void OnPointerClick (PointerEventData eventData)
	{
		ApartmentManager.instance.DeselectApartment();
		numPadDispalyField.instance.ResetDisplay();	
	}
}
