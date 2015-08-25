using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Editor_setMapPinColor : MonoBehaviour {

    public Color pinColor;

    public List<Image> mapPinsToChangeColor;

    public bool doChange;
	
	// Update is called once per frame
	void Update () {

        if(doChange)
        {
            foreach (Image pin in mapPinsToChangeColor)

            {

                pin.color = pinColor;

            }

            doChange = false;
        }
	
	}
}
