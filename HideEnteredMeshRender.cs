using UnityEngine;
using System.Collections;

public class HideEnteredMeshRender : MonoBehaviour {



	void OnTriggerEnter(Collider other) {
		

		if(other.transform.parent.parent.name == "Center")
			
		{
			
			foreach (MeshRenderer render in other.transform.parent.parent.parent.GetComponentsInChildren<MeshRenderer>())
				
			{
				
				render.enabled = false;
				
			}
			
		}

	}

	void OnTriggerExit(Collider other) {
		
		if(other.transform.parent.parent.name == "Center")

		{

			foreach (MeshRenderer render in other.transform.parent.parent.parent.GetComponentsInChildren<MeshRenderer>())
				
			{
				
				render.enabled = true;
				
			}

		}

	
		
		
	}

}
