using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace MWM.Gallery {

	public class SetFavouriteGallery : MonoBehaviour, IPointerClickHandler {

		public ApartmentBlock selectedBlock;
		public int selectedBlockIDinFavouriteContent; //in favList in LU_FavoriteListContent

		public void OnPointerClick (PointerEventData eventData){
			if (GetComponentInParent<ListContent>() != null)GetComponentInParent<ListContent>().favouriteGalleryPanel.SetActive(true);
			else if (GetComponentInParent<Favourite_GridContent>() != null)GetComponentInParent<Favourite_GridContent>().favouriteGalleryPanel.SetActive(true);
			else Debug.LogWarning ("Error! Favourite Content is not set to favourite!");

			ApartmentManager.instance.SelectApartment(selectedBlock.gameObject);
			CameraControl.instance.MoveOrbitCenterTo(ApartmentManager.instance.selectedApartment.transform);
			FavouriteGallery.instance.InitializeGallery(selectedBlockIDinFavouriteContent);
		}
	}
}