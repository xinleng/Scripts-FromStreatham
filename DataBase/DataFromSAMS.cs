using UnityEngine;
using System.Collections;

public class DataFromSAMS : MonoBehaviour {
	public string TheURL;
	public string UserKey;
	public string DevelopmentID;

	/*
	//http://sams.swan.org.uk:8052/API/Developments/List
	IEnumerator Start () {
		WWWForm form = new WWWForm();
		form.AddField("APIKey",TheKey);
		WWW download = new WWW(TheURL,form);
		yield return download;
		Debug.Log(download.responseHeaders);
		if((!string.IsNullOrEmpty(download.error))) {
			print( "Error downloading: " + download.error );
		} else {
			Debug.Log(download.text);
		}
	}
	*/
	/*
	// Development Details
	// http://sams.swan.org.uk:8052/API/Developments/Details
	IEnumerator Start () {
		WWWForm form = new WWWForm();
		form.AddField("APIKey",UserKey);
		form.AddField("DevelopmentID",DevelopmentID);
		WWW download = new WWW(TheURL,form);
		yield return download;
		Debug.Log(download.responseHeaders);
		if((!string.IsNullOrEmpty(download.error))) {
			print( "Error downloading: " + download.error );
		} else {
			Debug.Log(download.text);
		}
	}
	*/
	// Unit List
	// http://sams.swan.org.uk:8052/API/Units/List
	IEnumerator Start () {
		WWWForm form = new WWWForm();
		form.AddField("APIKey",UserKey);
		form.AddField("DevelopmentID",DevelopmentID);
		WWW download = new WWW(TheURL,form);
		yield return download;
		Debug.Log(download.responseHeaders);
		if((!string.IsNullOrEmpty(download.error))) {
			print( "Error downloading: " + download.error );
		} else {
			Debug.Log(download.text);
		}
	}
}
