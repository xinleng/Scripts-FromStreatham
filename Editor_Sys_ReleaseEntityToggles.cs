using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MWM.JSON.Standard;

[ExecuteInEditMode]
public class Editor_Sys_ReleaseEntityToggles : MonoBehaviour {

	public ReleaseType type;
	public MWM_CMS_DatabaseManager dbManager;
	public GameObject togglePrefab;
	public bool generateToggles;

	void Update () {
		if (generateToggles)
		{
			GenerateToggles();
			generateToggles = false;
		}
	}
	
	void GenerateToggles ()
	{
		if (type == ReleaseType.blocks)
		{//
			foreach (MWMBlock block in dbManager.blocks.blocks)
			{
				GameObject toggle = Instantiate (togglePrefab) as GameObject;
				toggle.transform.SetParent (this.transform, false);
				toggle.transform.localScale = Vector3.one;

				toggle.GetComponentInChildren<Text>().text = block.block_name;
				toggle.name = block.block_name;
				toggle.GetComponent<ReleaseEntityToggle>().type = ReleaseType.blocks;

				string playerPrefsKey = "Block" + toggle.name;
				toggle.GetComponent<ReleaseEntityToggle>().playerPrefsKey = playerPrefsKey;
				PlayerPrefs.SetString(playerPrefsKey, "true");
			}
		}
		if (type == ReleaseType.cores)
		{
			foreach (MWMCore core in dbManager.core.cores)
			{	
				//Generic
				GameObject toggle = Instantiate (togglePrefab) as GameObject;
				toggle.transform.SetParent (this.transform, false);
				toggle.transform.localScale = Vector3.one;
				//Personal
				toggle.GetComponentInChildren<Text>().text = core.core_name;
				toggle.name = core.core_name;		
				toggle.GetComponent<ReleaseEntityToggle>().type = ReleaseType.cores;
				//Settings
				string playerPrefsKey = "Core" + toggle.name;
				toggle.GetComponent<ReleaseEntityToggle>().playerPrefsKey = playerPrefsKey;
				PlayerPrefs.SetString(playerPrefsKey, "true");
			}
		}
	}
}
