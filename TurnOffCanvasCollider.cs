using UnityEngine;
using System.Collections;

public class TurnOffCanvasCollider : MonoBehaviour {


	public void ToggleCavasCollider ( bool toggle)

	{

		GetComponent<CanvasGroup>().interactable = !toggle;
		GetComponent<CanvasGroup>().blocksRaycasts = !toggle;

	}

}
