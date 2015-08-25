using UnityEngine;
using System.Collections.Generic;
using MWM.Gallery;

//new sub class create for lifeStyle Gallery to support pre-laid out text panels 
public class PhotoGalleryLifeStyle: Gallery {

	public SetLifeStyleGallery startGallery;
    public List<GameObject> textPanels;


	protected override void Start(){
		base.Start();
		InitializeGallery(startGallery.imageNameList);
	}

	public override void InitializeGallery(List<string> imageNameList){

		currentGalleryName = null;
		currentGalleryName = imageNameList;

		CurrentImageIndex = 0;
		LoadImages(0);
		InstantiateIndicators();
	}

    public override void LoadImages(int delta)
    {
        base.LoadImages(delta);

        if(textPanels.Count>0)
        {
            foreach (GameObject textPanel in textPanels)

            {
                textPanel.SetActive(textPanel.transform.GetSiblingIndex() == currentImageIndex);
            }


        }


    }
}
