using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace MWM.Gallery {
	public class SetPhotoGallery : SetPhotoGalleryThumbnails, IPointerClickHandler  {

		public virtual void OnPointerClick (PointerEventData eventData){
			photoGallery.InitializeGallery(imageNameList, 0);
		}
	}
}