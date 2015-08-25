using UnityEngine;
using System.Collections;
using DaikonForge.Tween.Components;
using UnityEngine.UI;

public class TurnOffPanelAfterTween : MonoBehaviour {

	public TweenComponentBase tween;
	public Button button;

	public bool activate;

	// Use this for initialization
	void Start () {

		if (tween == null) tween = GetComponent<TweenComponentBase>();
	
		tween.TweenCompleted += HandleTweenCompleted;	

	}

	void HandleTweenCompleted (TweenPlayableComponent sender)
	{

		if (activate)TurnOnButton(); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void TurnOffPanle ()
	{
		this.gameObject.SetActive(false);

	}

	void TurnOnButton(){
		button.gameObject.SetActive(true);
	}

}
