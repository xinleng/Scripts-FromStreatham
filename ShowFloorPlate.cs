using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowFloorPlate : MonoBehaviour,IPointerClickHandler {



	CanvasGroup canvasGroup;

	public GameObject floorPlatePanel;

	public RawImage floorPlateHolder;

	public string floorPlateFolderpath= "Floorplates/";

	public static ShowFloorPlate instance;

	// Use this for initialization
	void Start () {

		instance = this;

		canvasGroup = GetComponent<CanvasGroup>();
		ApartmentManager.instance.FilterApartmentsAction +=	 HandleFilterApartmentsAction;
	
	}

	void HandleFilterApartmentsAction (bool obj)
	{
		if(ApartmentManager.instance.core.core_name != "ALL" && ApartmentManager.instance.floor.floor_name != "ALL")

		{
			ToggleAllowClicking (true);
		}

		else {
			ToggleAllowClicking (false);
		}
	}

	public void ToggleAllowClicking (bool toggle)
	{
		canvasGroup.interactable = toggle;
		canvasGroup.blocksRaycasts = toggle;

		if(toggle)
				canvasGroup.alpha = 1f;
		else canvasGroup.alpha = 0.5f;
	}

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{

		Debug.Log (" shoul show floor plate panel  ");
		//floorPlatePanel.GetComponent<ToggleCanvasGroup>().DoToggleCancasGroup(true);
		floorPlatePanel.SetActive(true);

	}


	#endregion

	public void DoShowFloorPlate ()
	{
		
		Debug.Log (" shoul show floor plate panel  ");
		LoadFloorPlateIntoResource();
		floorPlatePanel.GetComponent<ToggleCanvasGroup>().DoToggleCancasGroup(true);
		//floorPlatePanel.SetActive(true);
		
	}


	public void LoadFloorPlateIntoResource (){
		
		string typeName = "Template";
		

		typeName= ApartmentManager.instance.core.core_name+"_"+ApartmentManager.instance.floor.floor_name;
		floorPlateHolder.texture = Resources.Load (floorPlateFolderpath + typeName) as Texture;
		Debug.Log ("core and floor selected. Floorplan " + typeName + " loaded");
	}
}
