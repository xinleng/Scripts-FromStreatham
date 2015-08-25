using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class XRayCameraSwitch : MonoBehaviour {

	public List<SetCameraCullingMask> cameraWithCullingMask;

	public void Toggle_xRay ( bool ToggleState){
		if(ToggleState){
			foreach (SetCameraCullingMask item in cameraWithCullingMask) item.SetCullingMaskXray();
			Debug.Log("Camera CullingMask set to XRAY");
		}else{
			foreach (SetCameraCullingMask item in cameraWithCullingMask) item.SetCullingMaskDefault();
			Debug.Log("Camera CullingMask set to DEFAULT");
		}
	}
}
