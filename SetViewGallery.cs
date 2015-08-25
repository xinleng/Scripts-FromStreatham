using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace MWM.Gallery {
	public class SetViewGallery : MonoBehaviour {

		public GameObject panelViews;
		public string folderPath = "Views";
		PhotoGallery viewGallery;
		List<string> imageNameList = new List<string>();
		
		void Start (){
			viewGallery = panelViews.GetComponentInChildren<PhotoGallery>();
			ApartmentManager.instance.SelectedApartmentAction += HandleSelectedApartment;
		}
		
		void HandleSelectedApartment (bool value){
			DisableButtonIfApartmentHasNoViews(value);
		}

		void DisableButtonIfApartmentHasNoViews(bool hasSelected){
			if(hasSelected){
				if(string.IsNullOrEmpty(ApartmentManager.instance.selectedApartment.GetComponent<ApartmentBlock>().unitInfo.unit_view)){
					GetComponent<Button>().interactable = false;
				}
				else{
					GetComponent<Button>().interactable = true;
				}
			}
		}

		void CreateNameList(){
			imageNameList.Clear();
			foreach (string imageName in ApartmentManager.instance.selectedApartment.GetComponent<ApartmentBlock>().unitInfo.unitViewsArray)
				imageNameList.Add(folderPath + imageName);
		}

		public void _OpenViews(){
			panelViews.SetActive(true);
			CreateNameList();
			viewGallery.InitializeGallery(imageNameList);
		}
	}
}
