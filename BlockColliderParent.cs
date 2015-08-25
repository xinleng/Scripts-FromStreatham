using UnityEngine;
using System.Collections;

public class BlockColliderParent : MonoBehaviour {

	public static BlockColliderParent instance;

	void Start () {
		instance = this;
	}

	public void TurnCollidersOn (bool value){
		foreach (BlockCollider blockCol in transform.GetComponentsInChildren<BlockCollider>()) {
			blockCol.collider.enabled = value;
		}
	}
}
