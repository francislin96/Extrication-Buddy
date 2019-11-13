using System;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VehicleIdentificationController : MonoBehaviour
{
	// VehicleInfoController
	public VehicleInfoController vehicleInfoController;

	// UI variables
	public Dropdown YearDropdown;
	public Dropdown MakeDropdown;
	public Dropdown ModelDropdown;
	public InputField VINInput;
	public Text VINInputError;

	// touch circle
	public GameObject touchCircle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // touch circle
        if (Input.touchCount > 0) {
        	Touch touch = Input.GetTouch(0);
        	touchCircle.transform.position = touch.position;
        }
        else {
        	touchCircle.transform.position = new Vector2(-300, -300);
        }
    }

    public void submit_User_Input() {
    	// get info from dropdowns
    	string year_value = YearDropdown.options[YearDropdown.value].text;
    	string make_value = MakeDropdown.options[MakeDropdown.value].text;
    	string model_value = ModelDropdown.options[ModelDropdown.value].text;

    	// submit user input
    	vehicleInfoController.year = year_value;
    	vehicleInfoController.make = make_value;
    	vehicleInfoController.model = model_value;

    	// load AR scene
    	SceneManager.LoadScene("AR View Mode");
    }

    public void submit_VIN_Input() {

    	// get VIN Input
    	string vin = VINInput.text;

    	try {
    		// get request
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://vpic.nhtsa.dot.gov/api/vehicles/decodevinvaluesextended/{0}?format=json", vin));
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			StreamReader reader = new StreamReader(response.GetResponseStream());
			string jsonResponse = reader.ReadToEnd();
			VehicleInfo info = JsonUtility.FromJson<VehicleInfo>(jsonResponse);

			// submit VIN input
	    	vehicleInfoController.year = info.Results[0].ModelYear;
	    	vehicleInfoController.make = info.Results[0].Make;
	    	vehicleInfoController.model = info.Results[0].Model;

	    	// test if VIN Input is complete
	    	if (vehicleInfoController.year == "" | vehicleInfoController.make == "" | vehicleInfoController.model == "") {
	    		Debug.Log("VIN Number is incomplete");
	    		VINInputError.text = "VIN Number is incomplete";
	    	}
	    	else {
	    		Debug.Log("AR");
	    		SceneManager.LoadScene("AR View Mode");
	    	}
	    }
	    catch {
	    	// invalid VIN
	    	Debug.Log("VIN Number is invalid");
	    	VINInputError.text = "VIN Number is invalid";
	    }
	}
}
