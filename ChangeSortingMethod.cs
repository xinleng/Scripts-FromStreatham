using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class ChangeSortingMethod : MonoBehaviour,IPointerClickHandler {

	public ListContent.SortingMethod sortingMethod;
	public ListContent ListConent;
	public bool sortDecending;

	Image sortingDirectionIndicator;

	void Start (){
		sortingDirectionIndicator = GetComponentInChildren<Image>();
	}

	public void OnPointerClick (PointerEventData eventData){
		if(GetComponent<Toggle>().isOn)	sortDecending=!sortDecending;

		// CHANGES INDICATOR
		if(sortDecending) sortingDirectionIndicator.rectTransform.localEulerAngles  =new Vector3(0f,0f,90f);
		else sortingDirectionIndicator.rectTransform.localEulerAngles = new Vector3(0f,0f,270f);
		//
		ListConent.DoSortListViewBy(sortingMethod,sortDecending, ListConent.listToUse);
	}
}
