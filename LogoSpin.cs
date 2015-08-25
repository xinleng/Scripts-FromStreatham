using UnityEngine;
using System.Collections;

public class LogoSpin : MonoBehaviour {

	public float angle = 0f;

	void Update () {
		Vector3 newAngle = new Vector3 (0f,angle,0f);
		transform.localEulerAngles = newAngle;
	}
}
