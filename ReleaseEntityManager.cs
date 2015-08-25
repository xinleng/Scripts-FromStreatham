using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MWM.JSON.Standard;

public enum ReleaseType
{
	blocks,
	cores
}

public class ReleaseEntityManager : MonoBehaviour {


	public ReleaseType type;
	public static ReleaseEntityManager instance;
	public List<string> activeBlockList;
	public List<string> activeCoreList;
	public List<GameObject> volumeList;

	void Start () {
		instance = this;
		//if no playerprefs
		CreateListFromPlayerPrefs();
		CreateVolumeList();
		UpdateVolumeState();
		// is cia updatint statusa toggle priklausomai nuo playerprefs
	}

	// only create once on start
	void CreateVolumeList(){
		foreach (BlockCollider item in ApartmentManager.instance.VolumesParent.GetComponentsInChildren<BlockCollider>()) {
			volumeList.Add(item.gameObject);
		}
	}

	public void UpdateVolumeState(){
		switch (type) {
		case ReleaseType.blocks:
			if (activeBlockList.Count != 0){
				foreach (GameObject item in volumeList) {
					if (activeBlockList.Contains(item.name))item.SetActive(true);
					else item.SetActive(false);
				}
			}
			else foreach (GameObject item in volumeList) {
				Debug.Log ("No active Block volumes!!");
				item.SetActive(false);
			}
			break;
		case ReleaseType.cores:
			if (activeCoreList.Count != 0){
				foreach (GameObject item in volumeList) {
					if (activeCoreList.Contains(item.name))item.SetActive(true);
					else item.SetActive(false);
				}
			}
			else foreach (GameObject item in volumeList) {
				Debug.Log ("No active Core volumes!!");
				item.SetActive(false);
			}
			break;
		default:
			break;
		}
	}

	void CreateListFromPlayerPrefs ()
	{
		switch (type) {
		case ReleaseType.blocks:
			foreach (ReleaseEntityToggle toggleB in transform.GetComponentsInChildren<ReleaseEntityToggle>()) 
			{
				if (PlayerPrefs.GetString(toggleB.playerPrefsKey, "Error") == "true"){
					activeBlockList.Add(toggleB.name);
					Debug.Log ("CreateListBlock" + toggleB.name + "True!");
				}
				if (PlayerPrefs.GetString(toggleB.playerPrefsKey, "Error") == "false"){
					activeBlockList.Remove(toggleB.name);
					Debug.Log ("CreateListBlock" + toggleB.name + "False!");
				}
			}
			 
			break;
		
		case ReleaseType.cores:
			foreach (ReleaseEntityToggle toggleC in transform.GetComponentsInChildren<ReleaseEntityToggle>())
			{ 
				if (PlayerPrefs.GetString(toggleC.playerPrefsKey, "Error") == "true"){
					activeCoreList.Add(toggleC.name);
					Debug.Log ("CreateListCore" + toggleC.name + "True!");
				}
				if (PlayerPrefs.GetString(toggleC.playerPrefsKey, "Error") == "false"){
					activeCoreList.Remove(toggleC.name);
					Debug.Log ("CreateListCore" + toggleC.name + "False!");
				}
			}
			break;
		default:
			break;
		}

		//pereit per visus vaikus su entity ir prachekint DB pagal ju string.
	}
}