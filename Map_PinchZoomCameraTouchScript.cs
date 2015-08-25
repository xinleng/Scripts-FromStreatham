// Has to live on the [MAP] object.
// Object that is being clicked.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TouchScript.Gestures.Simple;
using TouchScript.Hit;

public class Map_PinchZoomCameraTouchScript: MonoBehaviour
//, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
	
	RectTransform rectMap;
	public RectTransform canvas;
	public Transform mapCamera;
	
	float limitHeight;
	float limitWidth;
	float resolutionRatio = 2048f / 1536f;
	float cameraHeightOriginal;
	float cameraHeightNew;
	float cameraHeightTarget;
	
	[Header ("START VALUES")]
	public Vector2 startPos = new Vector2 (0f,0f);
	
	[Header ("MOVE")]
	public float moveSensivity = 1f;
	Vector2 targetPositionTouch;
	public Vector2 targetPosition;
	Vector2 lastFramePosition;
	float smooth = 4f;
	Vector3 moveToPos;
	
	
	[Header ("ZOOM")]
	public bool zoomEnabled;
	[Tooltip ("Up close")] public float zoomMin = 0.5f; // factor, that gets changed to pixels in run time
	[Tooltip ("Far away")] public float zoomMax = 1f;
	public float zoomSensivity = 1f;
	public float smoothZoom = 2f;
	
	float distBetweenFingers;
	float exDistBetweenFingers;
	Vector2 middlePoint;
	float correctFactor = 0.005f;
	
	[Tooltip("Manually enter distance in pixels from each side to compensate Map limit.")]
	[Header("LIMIT BOUNDARIES")]
	public float offestLeft = 0f; 
	public float offestRight = 0f;
	public float offestTop = 0f;
	public float offsetBottom = 0f;
	
	[Header ("INFO")]
	[Tooltip ("Maximum zoom out value before MAP is smaller than the screen")]
	public float maximumZoomMax;
	public float zoomFactor = 1;
	


	void Start (){


		InitializeValues();
		CalculateMinMaxZoom();


		Debug.Log (GetComponent<SimplePanGesture>());
		Debug.Log (GetComponent<SimpleScaleGesture>());

		GetComponent<SimplePanGesture>().Panned += HandlePanned;
		GetComponent<SimpleScaleGesture>().Scaled += HandleScaled;
	}
	
	void HandleScaled (object sender, System.EventArgs e)
	{
		
		Debug.Log (" scalled  ");
		SimpleScaleGesture gesture = sender as SimpleScaleGesture;
		
		Debug.Log (gesture.LocalDeltaScale);
		

	}
	
	void HandlePanned (object sender, System.EventArgs e)
	{
		
		
		Debug.Log (" panned  ");
		SimplePanGesture gesture = sender as SimplePanGesture;

		Debug.Log (gesture.ScreenPosition );

		Debug.Log (gesture.LocalDeltaPosition);
	
		
	}
	

	void OnEnable () {
		
		InitializeValues();
	}
	
	
	void Update () {
		
		Move ();
		if (zoomEnabled) Zoom ();
		
	}

	
	void Move(){




//		if(gesture.State == TouchScript.Gestures.Gesture.GestureState.Recognized)
//
//		{
//			Debug.Log("pan value is " + gesture.LocalDeltaPosition);
//			targetPosition -= new Vector2 (gesture.LocalDeltaPosition.x*moveSensivity*zoomFactor,gesture.LocalDeltaPosition.y*moveSensivity*zoomFactor);
//
//		}
		
		ClampTargetPosition();
		
		targetPosition.x = Mathf.Lerp (targetPosition.x, targetPositionTouch.x, Time.deltaTime * smooth);
		targetPosition.y = Mathf.Lerp (targetPosition.y, targetPositionTouch.y, Time.deltaTime * smooth);
		
		moveToPos = new Vector3 (targetPosition.x, targetPosition.y, -1);
		
		mapCamera.localPosition = moveToPos;
		
	}
	
	
	void Zoom(){



				

//		if(gesture.State == TouchScript.Gestures.Gesture.GestureState.Recognized)
//		{
//
//				Debug.Log("zoom value is " + gesture.LocalDeltaScale);
//				zoomFactor = gesture.LocalDeltaScale*zoomSensivity;
//			
//
//		}
				
				if (zoomMax < zoomFactor) zoomFactor = zoomMax;
				if (zoomFactor < zoomMin) zoomFactor = zoomMin; 
				
				cameraHeightTarget = cameraHeightOriginal * zoomFactor;
			
			//exDistBetweenFingers = distBetweenFingers;
		   
		
		cameraHeightNew = Mathf.Lerp (cameraHeightNew, cameraHeightTarget, Time.deltaTime * smoothZoom );
		
		if ((cameraHeightNew < cameraHeightTarget - 0.1f) || (cameraHeightTarget + 0.1f < cameraHeightNew)){ //to save resources
			
			mapCamera.camera.orthographicSize = cameraHeightNew;
			
			CalculateBaseLimits();
			
			targetPosition += new Vector2 (0.001f, 0.001f);
			
			ClampTargetPosition();
			
			moveToPos = new Vector3 (targetPosition.x, targetPosition.y, -1);
			mapCamera.localPosition = moveToPos;
		}
	}
	
	bool valuesInitialized = false;
	void InitializeValues(){
		if (!valuesInitialized){
			
			Debug.Log ("InitializeValues");
			
			// Assigning values
			rectMap = GetComponent<RectTransform>();
			cameraHeightOriginal = mapCamera.camera.orthographicSize;
			resolutionRatio = (float)Screen.width/ (float)Screen.height;
			
			//Move Values
			startPos = startPos * -1f;
			targetPosition = startPos;
			targetPositionTouch = startPos;
			
			//Zoom Values
			cameraHeightTarget = cameraHeightOriginal * zoomFactor;
			cameraHeightNew = cameraHeightOriginal * zoomFactor;
			
			// Applying values
			mapCamera.camera.orthographicSize = cameraHeightNew;
			
			CalculateBaseLimits();
			
			valuesInitialized = true;
		}
		
	}
	
	void CalculateBaseLimits(){
		limitHeight = rectMap.sizeDelta.y * 0.5f - (float)mapCamera.camera.orthographicSize;
		limitWidth = rectMap.sizeDelta.x * 0.5f - (float)mapCamera.camera.orthographicSize * resolutionRatio ;	
	}
	
	float limL;
	float limR;
	float limBot;
	float limTop;
	
	void ClampTargetPosition(){
		
		limL =  -limitWidth - offestLeft * 0.5f * zoomFactor;
		limR = limitWidth + offestRight * 0.5f * zoomFactor;
		limBot = -limitHeight - offsetBottom * 0.5f * zoomFactor;
		limTop = limitHeight + offestTop * 0.5f * zoomFactor;
		
		targetPositionTouch.x = Mathf.Clamp (targetPositionTouch.x, limL,limR);
		targetPositionTouch.y = Mathf.Clamp (targetPositionTouch.y, limBot, limTop);
		
	}
	
	// CALCULATS RECOMMENDED VALUE FOR ZOOM MAX
	void CalculateMinMaxZoom(){
		
		float maximumY = rectMap.sizeDelta.y / (cameraHeightOriginal * 2f);
		float maximumX = rectMap.sizeDelta.x / (cameraHeightOriginal * resolutionRatio * 2f);
		if (maximumY < maximumX) maximumZoomMax = maximumY;
		else maximumZoomMax = maximumX;
		
	}
	
}