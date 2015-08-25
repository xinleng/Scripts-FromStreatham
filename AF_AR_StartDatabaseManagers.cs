using UnityEngine;
using System.Collections;

public class AF_AR_StartDatabaseManagers : MonoBehaviour {

	public GameObject databaseManager;
	public GameObject apartmentManager;

	void Start () {
		if (databaseManager == null) Debug.LogError ("_DatabaseManager not assigned!");
		else databaseManager.SetActive(true);

		if (apartmentManager == null) Debug.LogError ("_ApartmentManager not assigned!");
		else apartmentManager.SetActive(true);
	}
}
