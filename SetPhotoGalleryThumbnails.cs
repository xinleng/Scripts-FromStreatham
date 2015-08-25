using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MWM.Gallery {
	[ExecuteInEditMode]
	public class SetPhotoGalleryThumbnails : MonoBehaviour {

		[Header ("Gallery")]
		public PhotoGallery photoGallery;
		public string folderPath = "";
		[Header ("Editor Tools")]
		public List<string> imageNameList;
		public List<Texture> imageList;
		public bool order = false;
		public bool grabImageNameNow;
		
		void Update(){		
			if(grabImageNameNow) GrabImageNames();
		}
		
		void GrabImageNames(){
			
			imageNameList.Clear();
			if (folderPath == "") Debug.LogWarning("Enter Subfolder to the images");
			foreach(Texture texture in imageList){
				imageNameList.Add (folderPath + texture.name);	
			}
			if (order) imageNameList.Sort();
			grabImageNameNow = false;
			imageList.Clear();
			
		}
	}
}