using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlockCollider : MonoBehaviour {
	
	public int ID;
	public bool isCore;

	public Text filterSelectedText;

	void Start()

	{
		ApartmentManager.instance.FilterOptionChangedAction += HandleFilterOptionChangedAction;
	
	}


	//temoprary - needed to avoid empty action call
	void HandleFilterOptionChangedAction (CheckList option)
	{



	}


	void OnClick(){

		if(isCore)
		{

			this.name = ApartmentManager.instance.CoreIdToName (ID);
			//Filter core by name.
			ApartmentManager.instance.SetSearchCondition(CheckList.check_cores, this.name);

		}

		else 

		{

			this.name = ApartmentManager.instance.BlockIdToName (ID);
			//Filter block by name.
			ApartmentManager.instance.SetSearchCondition(CheckList.check_blocks, this.name);

		}

		filterSelectedText.text = this.name;

		ApartmentManager.instance.DeselectApartment();
		ApartmentManager.instance.FilterApartments();

		CameraControl.instance.MoveOrbitCenterTo(transform);
		CameraControl.instance.distance = 80f;

		foreach (BlockCollider blockCol in transform.parent.GetComponentsInChildren<BlockCollider>()) {
			blockCol.collider.enabled = true;
		}
		collider.enabled = false;
		xRayToggleControler.instance.Turn_xRay_ON();

		
	}
}
