using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MWM.Gallery;

namespace MWM.Gallery{
	public class Gallery : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
	
#region _VARIABLES
		public int currentImageIndex = 0;
		
		public int CurrentImageIndex 
		{
			get { 	return currentImageIndex; 	}
			set { 	currentImageIndex = value;
					SetActiveIndicator(); 		}
		}
		
		public RawImage leftImage;
		public RawImage centerImage;
		public RawImage rightImage;
		
		RectTransform rectL;
		RectTransform rectC;
		RectTransform rectR;
		
		public GameObject indicatorPanel;
		public GameObject indicatorPrefab;
		
		public Color32 colorInactive;
		public Color colorActive;
		
		public List<string> currentGalleryName;
		
		public float imageGap = 100f;
		
		float sideImagePosStart;
		[Tooltip("Distance in pixels to trigger image slide to the next.")]
		float distanceToTrigger = 300f;
		int triggerStateID;
		
		bool shouldSlowDown = false;
		float target;
		float xCurrent;
		float movedTo;
		bool interupted;
		
		//MoveImageToPlace
		float speedToPos; // pixels per sec
		float distanceToBoundary;
		
		float maxDistance = 1000f;// max distance to boundary from which it calculates speed max
		float slideToPlaceSpeed = 300f;
		
		float flickDistTrigger = 10f;
#endregion

		protected virtual void OnEnable(){}

		protected virtual void Start(){	
			rectC = centerImage.GetComponent<RectTransform>();
			rectL = leftImage.GetComponent<RectTransform>();
			rectR = rightImage.GetComponent<RectTransform>();
			
			xCurrent = rectC.localPosition.x;
			sideImagePosStart = rectC.sizeDelta.x + imageGap;
		}
		
#region _INPUT
		public void OnPointerDown (PointerEventData eventData){
			
			if (shouldSlowDown){
				shouldSlowDown = false;
				interupted = true;
			} 
			xCurrent = rectC.localPosition.x;
		}
		
		// Adds finger distance
		// Moves images to respond
		// ?Assigns value for interuption
		public void OnDrag (PointerEventData eventData){
			// allows move if more than one image
			if(currentGalleryName.Count >1){
				if (interupted){
					movedTo = xCurrent;		
					interupted = false;
				}
				movedTo += eventData.delta.x;
				target = movedTo;
				Move (target);
				xCurrent = rectC.localPosition.x;
			}
		}
		
		public void OnPointerUp (PointerEventData eventData){
			// allows move if more than one image
			if(currentGalleryName.Count>1){
				shouldSlowDown = true;
				if ((eventData.delta.x > flickDistTrigger) || (movedTo > distanceToTrigger)) triggerStateID = 1; // Move to the right 
				else if ((eventData.delta.x < -flickDistTrigger) || (movedTo < -distanceToTrigger)) triggerStateID = 2;// Move to the left
				else triggerStateID = 3; // Centre
				
				movedTo = 0f;
			}
		}
#endregion
	
		void Update () {
			
			if(shouldSlowDown && currentGalleryName.Count>1) {
				if (triggerStateID == 1) MoveImageToPlace(sideImagePosStart,(-1)); 	// From Left to Right
				if (triggerStateID == 2) MoveImageToPlace(-sideImagePosStart,1);
				if (triggerStateID == 3) MoveImageToPlace(0f, 0);
			}
		}

		void Move (float x){
			rectC.localPosition = new Vector3 (x, 0, 0);
			rectL.localPosition = new Vector3 (x - sideImagePosStart, 0, 0 );
			rectR.localPosition = new Vector3 (x + sideImagePosStart, 0, 0 );
		}
		
		void MoveImageToPlace (float positionTo, int position){
			if (position == -1){
				if (rectC.localPosition.x < sideImagePosStart){
					target += Time.deltaTime * SpeedDistance(positionTo) * slideToPlaceSpeed; // float changes speed 
					Move(target);
				}else{
					Move (0f);
					LoadImages(position);
					shouldSlowDown = false;
				}
			}
			if (position == 1){
				if (rectC.localPosition.x > -sideImagePosStart){
					target += Time.deltaTime * -SpeedDistance(positionTo) * slideToPlaceSpeed; // float changes speed 
					Move(target);
				}else{
					Move (0f);
					LoadImages(position);
					shouldSlowDown = false;
				}
			}
			if (position == 0){
				//Debug.Log (rectC.localPosition.x);
				if (rectC.localPosition.x < 0){
					target += Time.deltaTime * SpeedDistance(positionTo) * slideToPlaceSpeed; // float changes speed 
					Move(target);
					CheckAndStop("MovingRight");
				}else if (rectC.localPosition.x > 0){
					target -= Time.deltaTime * SpeedDistance(positionTo) * slideToPlaceSpeed; // float changes speed 
					Move(target);
					CheckAndStop("MovingLeft");
					
				}	
			}
		}
		
		void CheckAndStop (string direction){
			
			if ((direction == "MovingLeft") && (rectC.localPosition.x < 0)){
				Move (0f);
				shouldSlowDown = false;
				return;
			}
			if ((direction == "MovingRight") && (rectC.localPosition.x > 0)){
				Move (0f);
				shouldSlowDown = false;
				return;
			}
		}
		
		// speed depending on distance from boundary
		float SpeedDistance(float boundary){
			distanceToBoundary = Mathf.Abs (Mathf.Abs(boundary) - Mathf.Abs (rectC.localPosition.x));
			speedToPos = distanceToBoundary / maxDistance;
			speedToPos = Mathf.Clamp (speedToPos, 0, 1);
			speedToPos = Mathf.Lerp (1, 20, speedToPos); // changes the speed difference depending on distance
			return speedToPos;
		}
		
		//========================================================================================================================================
		public void InstantiateIndicators(){
			if (indicatorPanel.transform.childCount != 0){
				foreach (Transform children in indicatorPanel.transform){
					Destroy(children.gameObject);
				}
			}else Debug.Log ("No children found to destroy!");
			
			int id=0; 
			for (int i = 0; i < currentGalleryName.Count; i++) {
				GameObject indicator = GameObject.Instantiate(indicatorPrefab) as GameObject;
				
				indicator.name = id.ToString();
				indicator.transform.SetParent (indicatorPanel.transform, false);
				indicator.transform.localScale = Vector3.one;
				id++;
			}
			SetActiveIndicator();
		}
		
		void SetActiveIndicator(){
			foreach (Image IndicatorDot in indicatorPanel.GetComponentsInChildren<Image>()) {
				if(IndicatorDot.name == CurrentImageIndex.ToString()) IndicatorDot.color = colorActive;
				else IndicatorDot.color = colorInactive;
			}
		}

		public virtual void InitializeGallery(List<string> imageNameList, int imageIndex){
			Debug.Log ("Error! Using Base Class!");
		}
		public virtual void InitializeGallery(List<string> imageNameList){
			Debug.Log ("Error! Using Base Class!");
		}
		public virtual void InitializeGallery(int currentIndex){
			Debug.Log ("Error! Using Base Class!");
		}

		int Normalize(int pastImgID, int delta){
			int currentID = pastImgID + delta;
			if (currentID == currentGalleryName.Count) return currentID - currentGalleryName.Count;
			else if (currentID == -1 ) return currentGalleryName.Count -1;
			else return currentID;
		}

		public ImageText imageCaptionText;

		public virtual void LoadImages (int delta){

			if (currentGalleryName.Count == 1){
				centerImage.texture = Resources.Load(currentGalleryName[0]) as Texture;
			}else{
				int pastImageIndex = currentImageIndex;
				currentImageIndex = Normalize(pastImageIndex, delta);
				if(Resources.Load(currentGalleryName[0]) as Texture == null) Debug.Log(currentGalleryName[0] + "not found");
				leftImage.texture 	= Resources.Load(currentGalleryName[Normalize(currentImageIndex, -1)]) 	as Texture;
				centerImage.texture = Resources.Load(currentGalleryName[currentImageIndex]) 				as Texture;
				rightImage.texture 	= Resources.Load(currentGalleryName[Normalize(currentImageIndex, 1)]) 	as Texture;
				Debug.Log (currentImageIndex);
				SetActiveIndicator();
				if (imageCaptionText) imageCaptionText.LoadCaptionText(currentImageIndex);							
			}
		}
	}
}