using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace MWM.Gallery{
[ExecuteInEditMode]
	public class ImageText : MonoBehaviour {

		public Text titleText;
		public Text contentText;

		public List<string> titleList;
		public List<string> contentList;

		[Header ("Assign to Lists")]
		public TextAsset textFile;
		public enum ListToAdd{
			Title,
			Content
		}
		public ListToAdd addToList;
		public bool populateList;

		public void LoadCaptionText(int currentID){
			if ((titleList.Count > 0) && (currentID <= titleList.Count-1)) titleText.text = titleList[currentID];
			else Debug.LogWarning ("Error! Text List is too short!!");

			if ((contentList.Count > 0) && (currentID <= contentList.Count-1)) contentText.text = contentList[currentID];
			else Debug.LogWarning ("Error! Text List is too short!!");
		}
		
		void Update(){
			if (populateList){
				if (addToList == ListToAdd.Title){
					titleList.Clear();
					string[] lines = textFile.text.Split("\n"[0]);
					foreach (string item in lines) titleList.Add (item);
				}
				else if (addToList == ListToAdd.Content){
					contentList.Clear();
					string[] lines = textFile.text.Split("\n"[0]);
					foreach (string item in lines) contentList.Add (item);
				}
				populateList = false;
				textFile = null;
			}
		}
	}
}