using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MatchRecSize : MonoBehaviour {

	public RectTransform rectToMatch;

	void Start ()

	{
		this.GetComponent<RectTransform>().sizeDelta = new Vector2 (rectToMatch.sizeDelta.x,rectToMatch.sizeDelta.y);
	}

}
