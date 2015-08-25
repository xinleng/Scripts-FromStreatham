using UnityEngine;
using System.Collections;

public class DisableOnPlay : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Debug.Log (" turnnign self off  ");
		this.gameObject.SetActive(false);
	
	}
	

}
