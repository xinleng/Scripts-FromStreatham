using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubmitButtons : MonoBehaviour {

	public static SubmitButtons instance;

	public GameObject collidersParent;
	public GameObject highlighter;
	public GameObject highlighterNullResult;

	public ToggleCanvasGroup p_List;
	public ListContent p_ListContent;

	void Start () {
		instance = this;
	}

	public void _FilterSearch3D()
	{
		ApartmentManager.instance.FilterApartments();
		highlighter.SetActive(false);
		xRayToggleControler.instance.Turn_xRay_ON();
		//AllowCameraOnClick - BLOCK
		if (BlockColliderParent.instance != null) BlockColliderParent.instance.TurnCollidersOn(false);
		if (ApartmentManager.instance.filteredApartments.Count == 0) highlighterNullResult.SetActive(true);
	}

	public void _FilterSearchList()
	{
		//Would make sense to combine both but not sure why p_list has to be kept under CanvasGroup.
		ApartmentManager.instance.FilterApartments();
		p_List.DoToggleCancasGroup(true);
		Debug.Log ("Generating List");
		p_ListContent._GenerateListOnClick(true);
		Debug.Log ("Generating List - complete");

		_FilterSearch3D();
	}
}