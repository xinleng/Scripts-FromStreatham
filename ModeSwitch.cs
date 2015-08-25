using UnityEngine;
using System.Collections;

public class ModeSwitch : MonoBehaviour {

	public GameObject cameraAR;
	public GameObject cameraFinder;
	public static ModeSwitch instance;

	public bool isInLevelOne;

	private bool afModeIsInAR;

	public bool AFModeIsInAR{


		get{

			return afModeIsInAR;

		}

		set{


			afModeIsInAR = value;

		}

	}

	private bool afModeIsInXray;

	public bool AFModeIsInXray{
		
		
		get{
			
			return afModeIsInXray;
			
		}
		
		set{
			
			
			afModeIsInXray = value;
			
		}
		
	}

	void Start()
	{

		instance = this;

		AFModeIsInAR = true;
		//AFModeIsInAR = TurnOffPanelAfterTwee

	}

	public void _SwitchToAReality(){

		cameraFinder.GetComponent<Camera>().enabled = false;
		cameraAR.GetComponent<Camera>().enabled = true;
		//isInAR = true;
	}

	public void _SwitchToAFinder(bool setCameraAngle){


		Debug.Log (" switching to AF  ");
		if(setCameraAngle)

		{

			Debug.Log(" adjustting camera angle    ");
			ARToAFViewConverter.instance.SetAFCamAngle();	

		}




		//camera statues change is depending on if the AF mode was in AR or not.
		cameraAR.GetComponent<Camera>().enabled = AFModeIsInAR;
		cameraFinder.GetComponent<Camera>().enabled = !AFModeIsInAR;


		//CameraControl.instance.distance = 250f;
		isInLevelOne = false;
		
	}



}



