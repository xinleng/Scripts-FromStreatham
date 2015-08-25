using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TouchScript.Gestures.Simple;

public class Floorplate_Interaction : MonoBehaviour {

	RectTransform rect;
	RectTransform parentRect;

	//ZOOM
	float zoomSensivity = 0.005f;
	Vector2 mapSizeOriginal;
	Vector2 mapSizeTo;
//	public float zoomMin = 0.5f; // factor, that gets changed to pixels in run time
//	public float zoomMax = 2f;
	
	void Start () {
	
		rect = GetComponent<RectTransform>();
		parentRect = transform.parent.GetComponentInParent<RectTransform>();
		mapSizeOriginal = rect.sizeDelta;
		currentPosition = rect.localPosition;
		targetLocation = rect.localPosition;

		currentMapSize = rect.sizeDelta;

		CalculateMoveLimits();
		CalculateZoomLimits();

		GetComponent<SimpleScaleGesture>().Scaled += HandleScaled;
		GetComponent<SimplePanGesture>().Panned += HandlePanned;
		GetComponent<SimplePanGesture>().PanStarted += HandlePanStarted; 
		GetComponent<SimplePanGesture>().PanCompleted += HandlePanCompleted;
	}

	void HandlePanCompleted (object sender, System.EventArgs e)
	{
		GetComponent<Image>().color = new Color (GetComponent<Image>().color.r,
		                                         GetComponent<Image>().color.g,
		                                         GetComponent<Image>().color.b,
		                                         Random.value);
	}

	void HandlePanStarted (object sender, System.EventArgs e)
	{
		GetComponent<Image>().color = new Color (Random.value,
		                                         Random.value,
		                                         Random.value,
		                                         GetComponent<Image>().color.a);
	}

#region MOVE

	float xLimit;
	float yLimit;
	Vector2 minLimit;
	Vector2 maxLimit;

	void CalculateMoveLimits()
	{
		xLimit = (rect.sizeDelta.x - parentRect.sizeDelta.x) * 0.5f;
		yLimit = (rect.sizeDelta.y - parentRect.sizeDelta.y) * 0.5f;

		minLimit = new Vector2 (-xLimit, -yLimit);
		maxLimit = new Vector2 (xLimit, yLimit);
	}

	Vector2 deltaTouchPan = Vector2.zero;

	void HandlePanned (object sender, System.EventArgs e)
	{
		SimplePanGesture gPan = sender as SimplePanGesture;
		deltaTouchPan = gPan.ScreenPosition - gPan.PreviousScreenPosition;
		MoveTo(deltaTouchPan);
	}

	public Vector2 currentPosition;
	public Vector2 targetLocation = Vector2.zero;

	void MoveTo (Vector2 delta)
	{
		targetLocation += delta;
		targetLocation = new Vector2 (Mathf.Clamp(targetLocation.x, minLimit.x, maxLimit.x),
		                              Mathf.Clamp(targetLocation.y, minLimit.y, maxLimit.y));
	
	}
	
	[Range (1.5f,5f)]
	public float MoveIntertia = 3f;

	void Move()
	{
		rect.transform.localPosition = new Vector2 (Mathf.Lerp(currentPosition.x, targetLocation.x, Time.deltaTime * MoveIntertia),
		                                            Mathf.Lerp(currentPosition.y, targetLocation.y, Time.deltaTime * MoveIntertia));	
	}

#endregion

#region ZOOM

	[Range (1f, 4f)]
	public float zoomMaxLimit = 2f;
	float zoomMinLimit;
	float zoomLimitX;
	float zoomLimitY;

	void CalculateZoomLimits()
	{
		zoomLimitX = parentRect.sizeDelta.x / rect.sizeDelta.x;
		zoomLimitY = parentRect.sizeDelta.y / rect.sizeDelta.y;

		if (zoomLimitX > zoomLimitY) zoomMinLimit = zoomLimitX;
		else zoomMinLimit = zoomLimitY;
	}

	void HandleScaled (object sender, System.EventArgs e)
	{
		Debug.Log ("Scaling");
		SimpleScaleGesture gScale = sender as SimpleScaleGesture;
		ZoomTo(gScale.LocalDeltaScale);

	}

	public float zoomFactor = 1f;
	Vector2 pastMapSize = Vector2.zero;
	Vector2 currentMapSize;
	float targetZoomFactor = 1f;
	float localDelta = 0f;
	void ZoomTo (float localDeltaScale)
	{
		localDelta = (localDeltaScale - 1f) * 0.1f;
		Debug.Log ("Local delta: " + localDelta);
		targetZoomFactor += localDelta;
		targetZoomFactor = Mathf.Clamp (targetZoomFactor, zoomMinLimit, zoomMaxLimit);
		Debug.Log ("Target zoom factor: " + targetZoomFactor);
	}


	void Zoom()
	{
		//do the zoom.
		mapSizeTo = mapSizeOriginal * targetZoomFactor;
		//limits
		Debug.Log ("1");
//		if ((currentMapSize.y + 1f < mapSizeTo.y) || (currentMapSize.y - 1f > mapSizeTo.y)){
			Debug.Log ("1 if Passed!");
			rect.sizeDelta = new Vector2 (Mathf.Lerp (currentMapSize.x, mapSizeTo.x, Time.deltaTime * MoveIntertia),
		    	                          Mathf.Lerp (currentMapSize.y, mapSizeTo.y, Time.deltaTime * MoveIntertia));
			CalculateMoveLimits();
//			Debug.Log ("Calculate limits passed!");
			MoveTo (new Vector2 (0.01f, 0.01f));
//			Debug.Log ("MoveTo location Passed!");
			UpdateColliderSize();
//			Debug.Log ("Update collider size passed!");
//			//
			pastMapSize = rect.sizeDelta;
//			Debug.Log ("Assign past map size passed!");
//		}
	}

	void UpdateColliderSize(){
		transform.GetComponent<BoxCollider>().size = new Vector3( rect.sizeDelta.x, rect.sizeDelta.y, 1f);
	}

#endregion

	void Update () {
		Move();
		Zoom();

		currentPosition = rect.transform.localPosition;
		currentMapSize = rect.sizeDelta;
	}
}
