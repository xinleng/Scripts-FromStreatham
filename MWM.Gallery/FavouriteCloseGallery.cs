using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace MWM.Gallery {

	public class FavouriteCloseGallery : MonoBehaviour, IPointerClickHandler {
		
		public Favourite_GridContent favoriteGridContent;
		public ListContent favoriteListContent;

		public void OnPointerClick (PointerEventData eventData){
			if (favoriteGridContent == null || favoriteListContent == null) Debug.LogWarning ("Error! Assign Content!") ;
			RegenerateListOrGrid ();
		}

		public Material orginalMat;
		void RegenerateListOrGrid ()
		{
			foreach (ApartmentBlock apartments in ApartmentManager.instance.allSceneApartments)
				apartments.GetComponent<Renderer>().material = orginalMat;
			ApartmentManager.instance.selectedApartment.GetComponent<Renderer>().material = ApartmentManager.instance.highlightMat;

			if (favoriteGridContent.gameObject.activeInHierarchy == true){
				favoriteGridContent._GenerateGridOnClick ();
			}
			if (favoriteListContent.gameObject.activeInHierarchy == true){
				favoriteListContent._GenerateListOnClick (true);
			}
			Debug.Log ("RegeneratingList");
		}
	}
}