using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SetCameraCullingMask : MonoBehaviour {

	public LayerMask targetCullingMask;
	new public Camera camera;
	public bool showingInXray;	

	private bool runTest;

	// off Editor
	public static SetCameraCullingMask instance;
	private LayerMask originalCullingMask;


	void OnEnable()	{
		camera = GetComponent<Camera>();
	}

	void Start () {
		originalCullingMask = this.GetComponent<Camera>().cullingMask;
		showingInXray = false;
	}
	
	void Update () {
		if(runTest) {
			SetCullingMaskXray();
			runTest = false;
		}
	}
	
	public void SetCullingMaskXray() {
		if(camera) camera.cullingMask = targetCullingMask;
		if(camera) camera.clearFlags = CameraClearFlags.SolidColor;
		showingInXray = true;
	}
	
	public void SetCullingMaskDefault() {
		if(camera) camera.cullingMask = originalCullingMask;
		if(camera) camera.clearFlags = CameraClearFlags.SolidColor; // Why it changes
		showingInXray = false;
	}
}
