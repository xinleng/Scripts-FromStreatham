using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// It's actually a toggle
// and it needs to be disable and enabled to work. No canvas group

namespace MWM.Gallery{
	public class FavouriteStar : MonoBehaviour, IPointerClickHandler {

		bool toggleStatus;
		public bool inFavouriteGallery = false;

		void Start () {
			ApartmentManager.instance.SelectedApartmentAction += UpdateFavouriteButtonStatusOnEvent;
		}

		 void UpdateFavouriteButtonStatusOnEvent (bool selected){
			if(ApartmentManager.instance.selectedApartment!=null)
				UpdateFavouriteButtonStatus();
		}

		public void UpdateFavouriteButtonStatus(){
			if(inFavouriteGallery){
				int currentID = FavouriteGallery.instance.currentImageIndex;
				GetComponent<Toggle>().isOn = FavouriteGallery.instance.currentGalleryApartments[currentID].favorited;
			}
			else{
				GetComponent<Toggle>().isOn = ApartmentManager.instance.selectedApartment.GetComponent<ApartmentBlock>().favorited;
			}
		}

		public void OnPointerClick (PointerEventData eventData){
			//if off - take name and assign to the list
			if(inFavouriteGallery){
				int currentID = FavouriteGallery.instance.currentImageIndex;
				if (GetComponent<Toggle>().isOn){
					FavouriteGallery.instance.currentGalleryApartments[currentID].favorited = true;
					ApartmentManager.instance.favoritedApartments.Add(FavouriteGallery.instance.currentGalleryApartments[currentID]);
				}
				else {
					FavouriteGallery.instance.currentGalleryApartments[currentID].favorited = false;
					ApartmentManager.instance.favoritedApartments.Remove(FavouriteGallery.instance.currentGalleryApartments[currentID]);
				}
			}
			else {
				if (GetComponent<Toggle>().isOn == true){
					ApartmentManager.instance.selectedApartment.GetComponent<ApartmentBlock>().favorited = true;
					ApartmentManager.instance.favoritedApartments.Add(ApartmentManager.instance.selectedApartment.GetComponent<ApartmentBlock>());
				}
				//if on - find an delete the name
				if (GetComponent<Toggle>().isOn == false){
					ApartmentManager.instance.selectedApartment.GetComponent<ApartmentBlock>().favorited = false;
					ApartmentManager.instance.favoritedApartments.Remove(ApartmentManager.instance.selectedApartment.GetComponent<ApartmentBlock>());
				}
			}
		}
	}
}