using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class AR_CameraControl : MonoBehaviour, IPointerUpHandler {

	[Tooltip("A function with this name is called on any 3D object that is tapped.")]
	public string clickFunctionName = "OnClick";
	public Camera cameraInsideAR;

	//Private variables
	private RaycastHit hitData;

	public void OnPointerUp(PointerEventData eventData) {
		if((Vector2.Distance(eventData.pressPosition, eventData.position) < EventSystem.current.pixelDragThreshold)) {
			
			if(Physics.Raycast(cameraInsideAR.ScreenPointToRay(eventData.position), out hitData)) {
				hitData.transform.SendMessage(clickFunctionName, SendMessageOptions.DontRequireReceiver);
				
			}
		}
	}
}
