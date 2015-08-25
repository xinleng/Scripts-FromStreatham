using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using TouchScript.Gestures.Simple;

[System.Serializable]
public enum CameraControlFingers {
	One = 1,
	Two = 2,
	Three = 3,
	Four = 4
}
//Version 1.32b
/*
 * Changes:
 * 
 * Xin added touchscript intergration
 * 
 * 
 * 
 * 
 * /

//Version 1.32
/*
 * Changes:
 * 1. Fixed rotating more than 360 degrees causes camera to pull back
 * 2. SetCameraPositionOnClick script has changed too, see script for how to set new camera angles
 * 
 */

//Version 1.31
/*
 * Changes:
 * 1. Fixed incorrect forward pan calculation
 * 
 */

public class CameraControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler  {
	public static CameraControl instance;
	
	[Header("Orbit Point and Camera")]
	
	[Tooltip("The default point to orbit.")]
	public Transform defaultOrbitPoint;
	
	[Tooltip("The camera this UI element moves.")]
	public Camera cameraToControl;
	
	[Tooltip("A function with this name is called on any 3D object that is tapped.")]
	public string clickFunctionName = "OnClick";
	
	[Header("Rotation Settings")]
	public bool allowRotation = true;
	public CameraControlFingers fingersForRotation = CameraControlFingers.One;
	public float rotationXsensitivity = 1f;
	public float rotationYsensitivity = 1f;
	[Tooltip("A higher value results in a faster slow-down.")]
	public float rotationInertia = 2f;
	
	[Header("Rotation X-Axis Limits")]
	public bool limitXaxis = false;
	public float xMinimum = 140f;
	public float xMaximum = 190f;
	
	[Header("Rotation Y-Axis Limits")]
	public bool limitYaxis = false;
	public float yMinimum = 20f;
	public float yMaximum = 40f;
	
	[Header("Pan Settings")]
	public bool allowPanning = false;
	public CameraControlFingers fingersForPan = CameraControlFingers.Three;
	public float panXsensitivity = 1f;
	public float panYsensitivity = 1f;
	[Tooltip("A higher value results in a faster slow-down.")]
	public float panInertia = 5f;
	
	[Header("Pan Limits")]
	public bool limitPanning = false;
	public Vector3 panLimits = new Vector3(10f, 0.1f, 10f);
	public Color editorPanLimitsColour = Color.green;
	
	[Header("Zoom Settings")]
	public bool allowZooming = false;
	public CameraControlFingers fingersForZoom = CameraControlFingers.Two;
	[Tooltip("Zoom sensitivity when using a touch screen.")]
	public float touchZoomSensitivity = 7f;
	[Tooltip("Zoom sensitivity when using a mouse scroll wheel.")]
	public float scrollWheelZoomSensitivity = 6f;
	[Tooltip("A higher value results in a faster slow-down.")]
	public float zoomInertia = 4f;
	
	[Header("Zoom Limits")]
	public bool limitZoom = false;
	public float zoomDistanceMinimum = 45f;
	public float zoomDistanceMaximum = 64f;
	
	[Header("Misc Settings")]
	[Tooltip("Stops pan if zooming and stops zooming if panning")]
	public bool exclusivePanAndZoom = true;
	
	[Header("-- Editor Tools --")]
	
	[Tooltip("Moves the editor camera to assist with setting the initial position the camera should start in.")]
	public bool editorSimulation = false;
	public float xAngle = 0f;
	public float yAngle = 0f;
	public float distance = 50f;
	public Vector3 panOffset = Vector3.zero;
	public float shadowDistanceBias;
	
	public Transform compass;

	public float compassAngleOffset;
	
	//Private variables
	private float currentXangle = 0f;
	private float currentYangle = 0f;
	private float currentDistance = 0f;
	private Vector3 currentPanOffset = Vector3.zero;
	
	private Vector3 currentPosition = Vector3.zero;
	private Quaternion currentRotation = Quaternion.identity;
	
	private RaycastHit hitData;
	private Transform cameraTransform;
	
	private bool isPanning = false;
	private bool isZooming = false;
	private int numberOfPointers = 0;
	private int firstPointerId = -1;
	private int secondPointerId = -1;
	private float lastPointerDistance = 0f;
	private float currentPointerDistance = 0f;
	private float gestureStartingDistance = 10f;
	private Vector2 firstPointerDelta = Vector2.zero;
	private Vector3 firstPointerPosition = Vector3.zero;
	private Vector3 lastFirstPointerPosition = Vector3.zero;
	private Vector3 lastSecondPointerPosition = Vector3.zero;
	private Vector3 secondPointerPosition = Vector3.zero;
	
	private Vector3 initialCameraView;
	
	private bool isMovingToNewPosition = false;
	private Coroutine movingCoroutine = null;


	
	void Awake() {
		instance = this;
	}
	
	void Start() {
		#if UNITY_EDITOR
		editorSimulation = false;
		#endif
		
		if(cameraToControl == null) {
			Debug.LogError("No camera to control, self destructing.");
			Destroy(gameObject);
			return;
		} else {
			cameraTransform = cameraToControl.transform;
		}
		
		currentXangle = xAngle;
		currentYangle = yAngle;
		currentDistance = distance;
		currentPanOffset = panOffset;
		
		initialCameraView =new Vector3 (xAngle,yAngle,distance);

		GetComponent<SimpleScaleGesture>().Scaled += HandleScaled;
		GetComponent<SimpleScaleGesture>().ScaleCompleted += HandleScaleCompleted;
		GetComponent<SimpleScaleGesture>().ScaleStarted += HandleScaleStarted;
	}

	void HandleScaleStarted (object sender, System.EventArgs e)
	{
		allowRotation = false;
	}

	void HandleScaleCompleted (object sender, System.EventArgs e)
	{
		allowRotation = true;
	}

	void HandleScaled (object sender, System.EventArgs e)
	{

		SimpleScaleGesture gestture = sender as SimpleScaleGesture;

		if(isMovingToNewPosition) return;

		distance -= (gestture.LocalDeltaScale -1)*touchZoomSensitivity;


		QualitySettings.shadowDistance = distance + shadowDistanceBias;

		Debug.Log (QualitySettings.shadowDistance );

	}
	
	void LateUpdate () {
		RecalculateCurrentParameters(isMovingToNewPosition);
		
		currentRotation = Quaternion.Euler(currentYangle, currentXangle, 0f);
		currentPosition = defaultOrbitPoint.position + currentPanOffset + (currentRotation * new Vector3(0f, 0f, -currentDistance));
		
		cameraTransform.position = currentPosition;
		cameraTransform.rotation = currentRotation;
	}
	
	void Update() {
		if(!isMovingToNewPosition && allowZooming)
			distance -= Input.GetAxis("Mouse ScrollWheel") * scrollWheelZoomSensitivity;
		
		ApplyLimitsToParameters();
		
		if (compass)compass.eulerAngles = new Vector3(compass.eulerAngles.x,compass.eulerAngles.y,(cameraToControl.transform.eulerAngles.y+compassAngleOffset));
	}
	
	public void MoveOrbitCenterTo(Transform targetTransform) {
		panOffset = targetTransform.position - defaultOrbitPoint.position;
	}
	
	public void ResetPanOffset() {
		panOffset = Vector3.zero;
	}
	
	public void OnPointerDown(PointerEventData eventData) {
		numberOfPointers++;
		
		if(firstPointerId == -1) {
			firstPointerId = eventData.pointerId;
			
			if(distance > 5f) {
				gestureStartingDistance = distance;
			} else {
				gestureStartingDistance = 5f;
			}				
			
			firstPointerPosition = eventData.position;
			firstPointerPosition.z = gestureStartingDistance;
			firstPointerPosition = cameraToControl.ScreenToWorldPoint(firstPointerPosition);
			firstPointerPosition = cameraTransform.InverseTransformPoint(firstPointerPosition);
			
			lastFirstPointerPosition = firstPointerPosition;
			return;
		}			
		
		if(secondPointerId == -1) {
			secondPointerId = eventData.pointerId;
			
			secondPointerPosition = eventData.position;
			secondPointerPosition.z = gestureStartingDistance;
			secondPointerPosition = cameraToControl.ScreenToWorldPoint(secondPointerPosition);
			secondPointerPosition = cameraTransform.InverseTransformPoint(secondPointerPosition);
			
			lastPointerDistance = Vector2.Distance(firstPointerPosition, secondPointerPosition);
			lastSecondPointerPosition = secondPointerPosition;
			return;
		}			
	}
	
	public void OnDrag(PointerEventData eventData) {
		//Rotate
		if(!isPanning && !isZooming && (numberOfPointers == (int)fingersForRotation)) {
			if(eventData.pointerId == firstPointerId) {
				firstPointerPosition = eventData.position;
				firstPointerPosition.z = gestureStartingDistance;
				firstPointerPosition = cameraToControl.ScreenToWorldPoint(firstPointerPosition);
				firstPointerPosition = cameraTransform.InverseTransformPoint(firstPointerPosition);
				
				Rotate(firstPointerPosition - lastFirstPointerPosition);
				
				lastFirstPointerPosition = firstPointerPosition;
			}				
		}
		
		//Pan
		if(!isZooming && (numberOfPointers == (int)fingersForPan)) {
			
			if((int)fingersForPan > 1) {
				if(eventData.pointerId == firstPointerId)
				{
					firstPointerDelta = eventData.delta;
					
					firstPointerPosition = eventData.position;
					firstPointerPosition.z = gestureStartingDistance;
					firstPointerPosition = cameraToControl.ScreenToWorldPoint(firstPointerPosition);
					firstPointerPosition = cameraTransform.InverseTransformPoint(firstPointerPosition);
				}
				else if(eventData.pointerId == secondPointerId)
				{
					if((eventData.delta != Vector2.zero) && (firstPointerDelta != Vector2.zero) && (Vector2.Angle(eventData.delta, firstPointerDelta) < 30f)) {
						if(exclusivePanAndZoom)
							isPanning = true;
						
						secondPointerPosition = eventData.position;
						secondPointerPosition.z = gestureStartingDistance;
						secondPointerPosition = cameraToControl.ScreenToWorldPoint(secondPointerPosition);
						secondPointerPosition = cameraTransform.InverseTransformPoint(secondPointerPosition);
						
						Pan(secondPointerPosition - lastSecondPointerPosition);
						
						lastSecondPointerPosition = secondPointerPosition;
					}
				}
			} else {
				if(eventData.pointerId == firstPointerId) {
					if(exclusivePanAndZoom)
						isPanning = true;
					
					firstPointerPosition = eventData.position;
					firstPointerPosition.z = gestureStartingDistance;
					firstPointerPosition = cameraToControl.ScreenToWorldPoint(firstPointerPosition);
					firstPointerPosition = cameraTransform.InverseTransformPoint(firstPointerPosition);
					
					Pan(firstPointerPosition - lastFirstPointerPosition);
					
					lastFirstPointerPosition = firstPointerPosition;
				}
			}
		}
		
		//Zoom
		if(!isPanning && (numberOfPointers == (int)fingersForZoom)) {
			if(eventData.pointerId == firstPointerId)
			{
				firstPointerDelta = eventData.delta;
				
				firstPointerPosition = eventData.position;
				firstPointerPosition.z = gestureStartingDistance;
				firstPointerPosition = cameraToControl.ScreenToWorldPoint(firstPointerPosition);
				firstPointerPosition = cameraTransform.InverseTransformPoint(firstPointerPosition);
			}
			else if(eventData.pointerId == secondPointerId)
			{
				if((eventData.delta != Vector2.zero) && (firstPointerDelta != Vector2.zero) && (Vector2.Angle(eventData.delta, firstPointerDelta) > 120f)) {
					if(exclusivePanAndZoom)
						isZooming = true;
					
					secondPointerPosition = eventData.position;
					secondPointerPosition.z = gestureStartingDistance;
					secondPointerPosition = cameraToControl.ScreenToWorldPoint(secondPointerPosition);
					secondPointerPosition = cameraTransform.InverseTransformPoint(secondPointerPosition);
					
					currentPointerDistance = Vector3.Distance(firstPointerPosition, secondPointerPosition);
					
					Zoom(currentPointerDistance - lastPointerDistance);
					
					lastPointerDistance = currentPointerDistance;
				}
			}
		}
	}
	
	public void OnPointerUp(PointerEventData eventData) {
		if((numberOfPointers == 1) && (Vector2.Distance(eventData.pressPosition, eventData.position) < EventSystem.current.pixelDragThreshold)) {
			
			RenderTexture tempRenderTexture = cameraToControl.targetTexture;
			cameraToControl.targetTexture = null;
			if(Physics.Raycast(cameraToControl.ScreenPointToRay(eventData.position), out hitData)) {
				
				
				
				hitData.transform.SendMessage(clickFunctionName, SendMessageOptions.DontRequireReceiver);
				
			}
			cameraToControl.targetTexture = tempRenderTexture;
			
		}
		
		numberOfPointers--;
		
		if(firstPointerId == eventData.pointerId)
			firstPointerId = -1;
		
		if(secondPointerId == eventData.pointerId)
			secondPointerId = -1;
		
		if(numberOfPointers == 0) {
			isZooming = false;
			isPanning = false;
		}
	}
	
	public void Rotate(Vector2 delta) {
		if(!allowRotation) return;
		if(isMovingToNewPosition) return;
		
		xAngle += delta.x * rotationXsensitivity;
		yAngle -= delta.y * rotationYsensitivity;
		
		ApplyLimitsToParameters();
	}
	
	public void Zoom(float zoomAmount) {
		if(!allowZooming) return;
		if(isMovingToNewPosition) return;

		Debug.Log (" zooming  ");
		if((zoomAmount > 0.05f) || (zoomAmount < -0.05f))
			distance -= zoomAmount * touchZoomSensitivity;
		
		ApplyLimitsToParameters();
	}
	
	public void Pan(Vector2 delta) {
		if(!allowPanning) return;
		if(isMovingToNewPosition) return;
		
		panOffset -= cameraTransform.right * delta.x * panXsensitivity;
		panOffset -= (Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * Vector3.forward) * delta.y * panYsensitivity;
		panOffset.y = 0f;
		
		ApplyLimitsToParameters();
	}
	
	void RecalculateCurrentParameters(bool useLerpAngle = false) {
		if(useLerpAngle) {
			currentXangle = Mathf.LerpAngle(currentXangle, xAngle, Time.deltaTime * rotationInertia);
			currentYangle = Mathf.LerpAngle(currentYangle, yAngle, Time.deltaTime * rotationInertia);
		} else {
			currentXangle = Mathf.Lerp(currentXangle, xAngle, Time.deltaTime * rotationInertia);
			currentYangle = Mathf.Lerp(currentYangle, yAngle, Time.deltaTime * rotationInertia);
		}
		
		currentDistance = Mathf.Lerp(currentDistance, distance, Time.deltaTime * zoomInertia);
		currentPanOffset = Vector3.Lerp(currentPanOffset, panOffset, Time.deltaTime * panInertia);
	}
	
	void ApplyLimitsToParameters() {
		if(limitXaxis)
			xAngle = Mathf.Clamp(xAngle, xMinimum, xMaximum);
		
		if(limitYaxis)
			yAngle = Mathf.Clamp(yAngle, yMinimum, yMaximum);
		
		if(limitZoom)
			distance = Mathf.Clamp(distance, zoomDistanceMinimum, zoomDistanceMaximum);
		
		if(distance < 0)
			distance = 0;
		
		if(limitPanning) {
			panOffset.x = Mathf.Clamp(panOffset.x, -panLimits.x / 2f, panLimits.x / 2f);
			panOffset.z = Mathf.Clamp(panOffset.z, -panLimits.z / 2f, panLimits.z / 2f);
		}
		
		while(xAngle > 360f) {
			xAngle -= 360f;
			currentXangle -= 360f;
		}			
		
		while(xAngle < 0f) {
			xAngle += 360f;
			currentXangle += 360f;
		}
		
		while(yAngle > 360f) {
			yAngle -= 360f;
			currentYangle -= 360f;
		}			
		
		while(yAngle < 0f) {
			yAngle += 360f;
			currentYangle += 360f;
		}
	}
	
	public void ResetCameraView ()
		
	{
		
		xAngle = initialCameraView.x;
		yAngle = initialCameraView.y;
		distance = initialCameraView.z;
		
		ResetPanOffset();
		//ZoomOut(true);
		
	}
	
	public void ZoomOut (bool reCenter)
		
	{
		distance = initialCameraView.z;
		rotationXsensitivity = 1;
		rotationYsensitivity = 1;
		
		if(reCenter) ResetPanOffset();
		
	}
	
	public void ZoomExtent(Vector3 zoomCentre)
		
	{
		distance = 226f;
		panOffset = zoomCentre;
		rotationXsensitivity = 1;
		rotationYsensitivity = 1;
		
	}
	
	public void IncreaseZoominRoationSpeed()
	{
		
		rotationXsensitivity = 2;
		rotationYsensitivity = 2;
		
	}
	
	public void MoveCameraTo(float newXangle, float newYangle, float newDistance) {
		if(movingCoroutine != null)
			StopCoroutine(movingCoroutine);
		
		movingCoroutine = StartCoroutine(DoMoveCameraTo(newXangle, newYangle, newDistance));
	}
	
	IEnumerator DoMoveCameraTo(float newXangle, float newYangle, float newDistance) {
		isMovingToNewPosition = true;
		
		xAngle = newXangle;
		yAngle = newYangle;
		distance = newDistance;
		
		yield return new WaitForEndOfFrame();
		
		movingCoroutine = null;
		isMovingToNewPosition = false;
		yield break;
	}
	
	#if UNITY_EDITOR
	void OnDrawGizmos() {
		if(editorSimulation) {
			if(defaultOrbitPoint == null) {
				UnityEditor.EditorUtility.DisplayDialog("Error!", "Can't preview camera position without a default orbit point assigned!", "Oops, my bad!");
				editorSimulation = false;
				return;
			}
			
			if(cameraToControl == null) {
				UnityEditor.EditorUtility.DisplayDialog("Error!", "Can't preview camera position without a \"Camera to Control\" assigned!", "Oops, my bad!");
				editorSimulation = false;
				return;
			} else {
				cameraTransform = cameraToControl.transform;
			}
			
			ApplyLimitsToParameters();
			
			currentRotation = Quaternion.Euler(yAngle, xAngle, 0f);
			currentPosition = defaultOrbitPoint.position + panOffset + (currentRotation * new Vector3(0f, 0f, -distance));
			
			cameraTransform.position = currentPosition;
			cameraTransform.rotation = currentRotation;
			
			UnityEditor.SceneView.currentDrawingSceneView.AlignViewToObject(cameraTransform);
		}
		
		if(limitPanning && (defaultOrbitPoint != null)) {
			if((panLimits.y > 0.15f) || (panLimits.y < 0f)) {
				panLimits.y = 0.1f;
			}
			
			Gizmos.color = editorPanLimitsColour;
			Gizmos.DrawWireCube(defaultOrbitPoint.position, panLimits);
			
			Color tempColor = Gizmos.color;
			tempColor.a = 0.3f;
			Gizmos.color = tempColor;
			Gizmos.DrawCube(defaultOrbitPoint.position, panLimits);
		}
	}
	#endif
}
