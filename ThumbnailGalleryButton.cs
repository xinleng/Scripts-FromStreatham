using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using MWM.Gallery;

public class ThumbnailGalleryButton : MonoBehaviour, IPointerClickHandler {

	public int imageID;
	public SetPhotoGalleryThumbnails setGallery;

	public void OnPointerClick (PointerEventData eventData)
	{
		setGallery.photoGallery.InitializeGallery(setGallery.imageNameList, imageID);
		this.transform.parent.parent.gameObject.SetActive(false);
	}
}
