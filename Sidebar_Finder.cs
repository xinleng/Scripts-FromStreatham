using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Sidebar_Finder : MonoBehaviour, IPointerClickHandler  {

	public GameObject cameraControl;
	public GameObject cameraFinder;
	public List<FilterOptionList> filters;
	
	public void OnPointerClick (PointerEventData eventData)
	{
		foreach (FilterOptionList filter in filters) {
			filter.CreateButtons();
		}

		cameraControl.SetActive(GetComponent<Toggle>().isOn);
		cameraFinder.SetActive(GetComponent<Toggle>().isOn);
	}
}
