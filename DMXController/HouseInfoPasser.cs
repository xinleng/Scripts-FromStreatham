using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class HouseInfoPasser : MonoBehaviour {

	//public CSVReader dataSource;
	public TextAsset dataSource;

	public bool DoSetUpNow;

	public string[,] dataMatrix;

	/// <summary>
	/// The index details. use this to define what each rows are.
	/// </summary>
	public List<string> indexDetails;

	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if(DoSetUpNow) {

			dataMatrix = CSVReader.SplitCsvGrid(dataSource.text);


			//GeneratePlotNumberBasedOnName();

		
			DoSetUpNow = false;

		}
	
	}


	

}
