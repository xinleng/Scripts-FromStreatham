using UnityEngine;
using System.Collections;

public class SetCameraPositionOnClick : MonoBehaviour {
	[Header("Attributes to Set on Main Camera")]
	public float newXAngle = 10f;
	public float newYAngle = 20f;
	public float newDistance = 5f;

	public bool setAngle=false;
	
	public void OnClick() {
		Debug.Log("Clicked!");
		
		CameraControl.instance.MoveOrbitCenterTo(transform);
		CameraControl.instance.distance = newDistance;

		if(setAngle)
				CameraControl.instance.MoveCameraTo(newXAngle, newYAngle, newDistance);
	}
}
