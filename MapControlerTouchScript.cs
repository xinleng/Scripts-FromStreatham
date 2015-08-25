using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TouchScript.Gestures.Simple;
using TouchScript.Hit;

public class MapControlerTouchScript: MonoBehaviour {

	public bool allowRotation;
	public bool allowZoom;
	public bool allowPanning;
	
	public Vector3 panValue;
	public float scaleValue;

	public RectTransform rectLimit;
	float limitX;
	float limitY;
	public Camera mapCam;

	Vector3 targetPosition;
	float targetZoom;
	public float panSmooth;
	public float zoomSmooth;

	public float maxZoomSize;// this needs another look
	public float minZoomSize;// this needs another look

	void Start () {

		targetPosition = mapCam.transform.localPosition;
		targetZoom = mapCam.orthographicSize;
		
		GetComponent<SimplePanGesture>().Panned += HandlePanned;
		GetComponent<SimpleScaleGesture>().Scaled += HandleScaled;
		GetComponent<SimpleRotateGesture>().Rotated += HandleRotated;

	}

	void HandleRotated (object sender, System.EventArgs e)
	{
		if(allowRotation)
			mapCam.transform.Rotate(new Vector3(0,0,GetComponent<SimpleRotateGesture>().DeltaRotation*-1));
	}
	
	void HandleScaled (object sender, System.EventArgs e)
	{
		if(allowZoom){
			Debug.Log ("Scalled");
			SimpleScaleGesture gesture = sender as SimpleScaleGesture;
			Debug.Log (gesture.LocalDeltaScale);
			scaleValue = gesture.LocalDeltaScale;
			targetZoom *= (1/scaleValue);
		}
	}
	
	void HandlePanned (object sender, System.EventArgs e)
	{
		if(allowPanning){
			Debug.Log ("Panned");
			SimplePanGesture gesture = sender as SimplePanGesture;
			panValue = gesture.LocalDeltaPosition;
			targetPosition -= new Vector3 (panValue.x,panValue.y,panValue.z);
		}
	}

	void Update ()
	{
		ApplyLimits();
		if(allowZoom) mapCam.orthographicSize = Mathf.Lerp(mapCam.orthographicSize,targetZoom,zoomSmooth*Time.deltaTime);
		if(allowPanning) mapCam.transform.localPosition = Vector3.Lerp(mapCam.transform.localPosition,targetPosition,panSmooth*Time.deltaTime);
	}

	void ApplyLimits ()
	{
		limitX = (rectLimit.sizeDelta.x*0.5f -  mapCam.orthographicSize*mapCam.aspect);
		limitY =  (rectLimit.sizeDelta.y*0.5f -  mapCam.orthographicSize);

		targetPosition.x = Mathf.Clamp(targetPosition.x,limitX*-1,limitX);
		targetPosition.y = Mathf.Clamp(targetPosition.y,limitY*-1,limitY);

		targetZoom = Mathf.Clamp(targetZoom,minZoomSize,maxZoomSize);
	}
}