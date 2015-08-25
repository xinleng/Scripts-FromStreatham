using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

// Button passes on values/Invokes Manager.
public class Dropdown_ContentButton : MonoBehaviour, IPointerDownHandler {

	public Dropdown_Content manager;
	// Just passes the values needed for the action:
	// 1. Change text to selected items text
	// 2. selected object
	public void OnPointerDown (PointerEventData eventData){

		GetComponent<Toggle>().isOn = true;

		manager.textString = this.GetComponent<Text>().text;
		manager.toggleSelected = this.transform.gameObject;
		
		manager.DD_Content_Action();
	}
}
