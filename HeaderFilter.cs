using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HeaderFilter : MonoBehaviour, IPointerClickHandler {
	
	public void OnPointerClick (PointerEventData eventData)
	{
		ApartmentInfoBar.instance.ToggleApartmentInfoBar(false);
		CameraControl.instance.ZoomOut(false);
	}
}
