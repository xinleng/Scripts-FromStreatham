using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace MWM.Gallery {
	[ExecuteInEditMode]
	public class SetLifeStyleGallery : MonoBehaviour, IPointerClickHandler  {

		[Header ("Gallery")]
		public PhotoGalleryLifeStyle photoGallery;
		public string folderPath = "";
		[Header ("Editor Tools")]
		public List<string> imageNameList;
		public List<Texture> imageList;
		public bool order = false;
		public bool grabImageNameNow;

        public List<GameObject> textPanels;

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

		public void OnPointerClick (PointerEventData eventData){

            Debug.Log("setting up life style photo gallery");
            photoGallery.InitializeGallery(imageNameList);
            photoGallery.textPanels = textPanels;//making sure we update the text panels as we switching gallery.
		}
	}
}