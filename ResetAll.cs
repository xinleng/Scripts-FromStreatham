using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResetAll : MonoBehaviour {

	public static ResetAll instance;
	public GameObject filterPanel;

	void Start(){
		instance = this;
	}

	public void _Reset(){

		ResetFilterPanel();

		ApartmentManager.instance.ResetSearchConditionAndFilter(false);
		numPadDispalyField.instance.ResetDisplay();
		BlockColliderParent.instance.TurnCollidersOn (true);
		xRayToggleControler.instance.Turn_xRay_OFF();

		SubmitButtons.instance.highlighter.SetActive(false);
		SubmitButtons.instance.highlighterNullResult.SetActive(false);

	}

	void ResetFilterPanel(){
		foreach (ToggleExtended togglebutton in filterPanel.GetComponentsInChildren<ToggleExtended>()){
			togglebutton.isOn = false;
		}
		foreach (Text item in filterPanel.GetComponentsInChildren<Text>()) {
			if (item.name == "TextSelected") item.text = "ALL";
		}
	}
}
