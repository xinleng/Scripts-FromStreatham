using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DaikonForge.Tween.Components;

public class List_Panel_OptimizedLoading : MonoBehaviour {

	public ListContent manager;
	public Text title;
	public Text aptNumber;
	public string titleText;

	void Update () {
		GetComponentInChildren<Slider>().value = manager.textDisplayApartmentNo;
	}

	void OnEnable(){
		GetComponent<CanvasGroup>().alpha = 1f;
		GetComponentInChildren<TweenFloatProperty>().TweenCompleted += HandleTweenCompleted;
		GetComponentInChildren<Slider>().maxValue = manager.apartmentManager.filteredApartments.Count;
		GetComponentInChildren<Slider>().value = 0;
		string count = manager.apartmentManager.filteredApartments.Count.ToString();
		aptNumber.text = "OF " + count + " APARTMENTS"; 
	}

	void HandleTweenCompleted (TweenPlayableComponent sender){
		transform.gameObject.SetActive(false);
	}
}

// on enabled start indicating generation.
// every frame  update fill. When finished turn thing off.