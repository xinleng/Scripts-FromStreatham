using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MWM.Gallery;

[ExecuteInEditMode]
public class Editor_GalleryThumbsGenerate : MonoBehaviour {

	public bool generateThumbnails;
	public SetPhotoGalleryThumbnails setPhotoGallery;
	public Transform contentPanel;
	public GameObject buttonPrefab;
	
	void Update(){		
		if(generateThumbnails) GenerateThumbnails();
	}

	void GenerateThumbnails ()
	{
		for (int i = 0; i < setPhotoGallery.imageNameList.Count ; i++) {
			GameObject button = Instantiate (buttonPrefab) as GameObject;
			button.transform.SetParent(contentPanel, false);
			button.name = setPhotoGallery.imageNameList[i];
			
			button.GetComponent<ThumbnailGalleryButton>().imageID = i;
			button.GetComponent<ThumbnailGalleryButton>().setGallery = setPhotoGallery;

			//SETTING GALLERY THUMBNAILS
			if (!Resources.Load("Thumbnail_Gallery/" + setPhotoGallery.imageNameList[i])){
				button.GetComponent<RawImage>().texture = Resources.Load("Thumbnail_Gallery/" + "Template") as Texture;
				Debug.Log ("Gallery thumbnail: Template laoded");
			}
			else{
				button.GetComponent<RawImage>().texture = Resources.Load("Thumbnail_Gallery/" + setPhotoGallery.imageNameList[i]) as Texture;
				Debug.Log ("Gallery thumbnail: " + setPhotoGallery.imageNameList[i] + " loaded");
			} 
		}

		generateThumbnails = false;
	}
}
