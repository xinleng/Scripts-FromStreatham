using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class List_Button_Back : MonoBehaviour, IPointerClickHandler {

	public ToggleCanvasGroup listPanel;
	public ListContent content;

	public void OnPointerClick (PointerEventData eventData){
		_Back();
	}

	// Actions when leaving the List Panel
	public void _Back(){
		listPanel.DoToggleCancasGroup(false);
		content._CleanListView();
	}
}
