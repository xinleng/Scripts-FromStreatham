using UnityEngine;
using System.Collections;

using UnityEngine.UI;

/// <summary>
/// /*Animator controller. this provides utilities for manupulating the animator*/
/// </summary>
public class AnimatorController : MonoBehaviour {

	Animator animator;

	public string booleanParameter;



	// Use this for initialization
	void Start () {

		animator = GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Toggles the state. Flips the value of the given paramenter
	/// </summary>
	/// <param name="paramenter">Paramenter.</param>
	public void ToggleState (bool toggle)
	{

		Debug.Log (" toggleing state  ");

		animator.SetBool(booleanParameter,toggle);

	}

	public void SetState ( bool value )

	{

		Debug.Log ("setting state to " + value);

		animator.SetBool(booleanParameter,value);


	}

}
