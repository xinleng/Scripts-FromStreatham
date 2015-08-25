using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MWM.Gallery;

// Lives on CONTENT of scroll rect for grid.
public class Favourite_GridContent : MonoBehaviour {

	public GameObject buttonPrefab;
	public GameObject favouriteGalleryPanel;

	public void _GenerateGridOnClick(){
		Debug.Log ("Generating Grid");
		_CleanGridView();
		InitialiseGrid(ApartmentManager.instance.favoritedApartments);
	}

	public void _CleanGridView(){
		foreach (Transform child in transform) Destroy (child.gameObject);
	}

	void InitialiseGrid (List<ApartmentBlock> List){

		for (int i = 0; i < List.Count ; i++) {
			GameObject button = Instantiate (buttonPrefab) as GameObject;
			button.transform.SetParent(this.transform, false);
			button.name = List[i].unitInfo.unit_name;
			button.GetComponent<SetFavouriteGallery>().selectedBlock = List[i];
			button.GetComponent<SetFavouriteGallery>().selectedBlockIDinFavouriteContent = i;
			if(button.GetComponentInChildren<Text>()){
					button.GetComponentInChildren<Text>().text = List[i].unitInfo.unit_name;
			}
			//SETTING FLOORPLANS THUMBNAILS

			if(string.IsNullOrEmpty(List[i].unitInfo.unit_plan))

				button.GetComponent<RawImage>().texture = Resources.Load("Thumbnails/" + "Template") as Texture;// use temp one if there is no value
				else button.GetComponent<RawImage>().texture = Resources.Load("Thumbnails/" + List[i].unitInfo.unit_plan) as Texture;// ASSIGN THE RIGHT NAME AND LOCATION

		}
	}
}
