using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ReleaseEntityToggle : MonoBehaviour, IPointerClickHandler {
	
	public string playerPrefsKey;
	public ReleaseType type;

	void Start () {

		if (PlayerPrefs.GetString(playerPrefsKey, "false") == "false") GetComponent<Toggle>().isOn = false;
		else GetComponent<Toggle>().isOn = true;
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		switch (type) {
		case ReleaseType.blocks:
			ClickLogic (ReleaseEntityManager.instance.activeBlockList);
			break;
		case ReleaseType.cores:
			ClickLogic (ReleaseEntityManager.instance.activeCoreList);
			break;
		default:
			break;
		}
	}

	void ClickLogic(List<string> list){
		if (GetComponent<Toggle>().isOn == true){
			//jeigu tokio nera pridet
			if (!list.Contains(this.name)) list.Add(this.name);
			PlayerPrefs.SetString(playerPrefsKey, "true");
			TurnVolumeOff (this.name, true);// instead could invoke event that would be picked up by Manager.
		}
		if (GetComponent<Toggle>().isOn == false){
			//jei toks yra atimt.
			if (list.Contains(this.name)) list.Remove(this.name);
			PlayerPrefs.SetString(playerPrefsKey, "false");
			TurnVolumeOff (this.name, false);// instead could invoke event that would be picked up by Manager.
		}
		ReleaseEntityManager.instance.UpdateVolumeState();
	}

	void TurnVolumeOff(string name, bool value){
		foreach (GameObject item in ReleaseEntityManager.instance.volumeList) {
			if (item.name == name) item.gameObject.SetActive(value);
		}
	}
}