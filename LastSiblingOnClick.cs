using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using DaikonForge.Tween.Components;

public class LastSiblingOnClick : MonoBehaviour, IPointerDownHandler {

	void Start()
	{

		GetComponent<TweenVector3Property>().Target.GetComponent<RectTransform>().localScale = Vector3.zero;

	}

	public void OnPointerDown (PointerEventData eventData){
		transform.parent.transform.SetAsLastSibling();
	}
}
