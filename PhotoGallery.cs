using UnityEngine;
using System.Collections.Generic;
using MWM.Gallery;

public class PhotoGallery : Gallery {

	public SetPhotoGallery startGallery;

	protected override void Start(){
		base.Start();
		InitializeGallery(startGallery.imageNameList);
	}

	public override void InitializeGallery(List<string> imageNameList, int imageIndex){

		currentGalleryName = null;
		currentGalleryName = imageNameList;

		CurrentImageIndex = imageIndex;
		LoadImages(0);
		InstantiateIndicators();
	}

	public override void InitializeGallery(int imageIndex){

//		if (currentImageIndex == null) Debug.LogError("No imageList loaded to the gallery!");
		CurrentImageIndex = imageIndex;
		LoadImages(0);
		InstantiateIndicators();
	
	}
}
