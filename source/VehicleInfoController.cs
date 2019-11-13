using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInfoController : MonoBehaviour
{

	static VehicleInfoController instance;

	public string year;
	public string make;
	public string model;
	public bool isCarChosen = false;

	void Awake() {
		if (instance != null) {
			Destroy(gameObject);
		}
		else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
    
}