using UnityEngine;
using MWM.Gallery;
using System.Collections.Generic;

public class FavouriteGallery : Gallery {

	public static FavouriteGallery instance;

	protected override void OnEnable(){
		instance = this;
	}

	protected override void Start(){
		base.Start();
		instance = this;
	}

	public List<ApartmentBlock> currentGalleryApartments;

	public override void InitializeGallery(int currentIndex){

		currentGalleryName.Clear ();
		currentGalleryApartments.Clear ();
		
		List<ApartmentBlock> tempList = ApartmentManager.instance.favoritedApartments;
		foreach (ApartmentBlock item in tempList) currentGalleryApartments.Add(item);
		foreach (ApartmentBlock line in tempList)

		{

			if(string.IsNullOrEmpty(line.unitInfo.unit_plan))
			{

				currentGalleryName.Add("Template");

			}
			else 
				currentGalleryName.Add (line.unitInfo.unit_plan);


		}

		CurrentImageIndex = currentIndex;

		LoadImages(0);
		InstantiateIndicators();
	}

	public FavouriteStar favouriteStarButton;

	public override void LoadImages (int delta){
		base.LoadImages(delta);
		SimulateApartmentClick(currentImageIndex);
		favouriteStarButton.UpdateFavouriteButtonStatus();
	}

	void SimulateApartmentClick(int newID)
	{
		ApartmentManager.instance.SelectApartment(currentGalleryApartments[newID].gameObject);
		ApartmentManager.instance.selectedApartment.GetComponent<Renderer>().material = ApartmentManager.instance.highlightMat;
	}
}