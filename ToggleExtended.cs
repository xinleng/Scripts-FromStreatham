using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToggleExtended : Toggle { //Exteneded versision with addon functionalitys

	public void ChangeTextToGraphic () {
		graphic.enabled = isOn;
		targetGraphic.enabled = !isOn;
	}
}
