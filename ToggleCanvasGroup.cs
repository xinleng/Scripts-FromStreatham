using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToggleCanvasGroup : MonoBehaviour {
	
	public CanvasGroup canvasGroup;
	public CanvasGroup canvasGroupB;
	
	// Use this for initialization
	void Start () {

		if(canvasGroup==null) //only need to get if not assigned
				canvasGroup = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	public void DoToggleCancasGroup (bool toggle)
		
	{
		
		if(toggle){	
			canvasGroup.alpha = 1f;
			if(canvasGroupB!=null) canvasGroupB.alpha = 1f;
		
		}else{
			canvasGroup.alpha = 0f;
			if(canvasGroupB!=null) canvasGroupB.alpha = 0f;
		}
		
		canvasGroup.interactable = toggle;
		canvasGroup.blocksRaycasts = toggle;
		
		if(canvasGroupB!=null)
		{
			canvasGroupB.interactable = toggle;
			canvasGroupB.blocksRaycasts = toggle;
		}
		
	}
	
}
