using UnityEngine;
using System.Collections;
using TouchScript.Gestures.Simple;
using TouchScript.Behaviors;
using UnityEngine.UI;

public class FloorPlanController : MonoBehaviour {

	//Transformer2D transformer;
	RectTransform localRect;
	public Vector2 moveBoundary;

	Vector2 targetPosition;

	void Start()
	{
		//transformer = GetComponent<Transformer2D>();
		localRect = GetComponent<RectTransform>();


		GetComponent<SimplePanGesture>().Panned += HandlePanned;
	}

	void HandlePanned (object sender, System.EventArgs e)
	{
		//SimplePanGesture gestrue = GetComponent<SimplePanGesture>();
		localRect.anchoredPosition = new Vector2( Mathf.Clamp(localRect.anchoredPosition.x,moveBoundary.x*-1,moveBoundary.x),
		                                         	  Mathf.Clamp(localRect.anchoredPosition.y,moveBoundary.y*-1,moveBoundary.y));
	
	}
}
