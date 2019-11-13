using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Vuforia;

public class ARViewModeController : MonoBehaviour
{
	// Vehicle Info
	public VehicleInfoController vehicleInfoController;

	// Vehicle Name
	public Text Vehicle_Name_Text;

	// Vehicle Orientation
	int vehicleOrientationIndex = 2;
	string[] vehicleOrientationArray = {"Back", "Driver Side", "Front", "Passenger Side"};
	public Text Vehicle_Orientation_Text;

	// Toggle 
	public bool cutZonesToggleOn = true;
	public bool riskZonesToggleOn = true;
	public RawImage cutZonesToggleOnImage;
	public RawImage riskZonesToggleOnImage;

	// Vehicle Model
	GameObject Vehicle_Model;
	GameObject Cut_Zones;
	GameObject Risk_Zones;
	GameObject Battery;
	GameObject FuelTank;
	public Text Battery_Label;
	public Text FuelTank_Label;

	// Model Target
	public GameObject Model_Target_GameObject;
	public ModelTargetBehaviour Model_Target;
	public bool trackingOn = false;
	public DefaultTrackableEventHandler trackableEventHandler;

	// FPS
	public Text FPS_Text;
	float elapsed = 0f;

	// touch circle
	public GameObject touchCircle;


    // Start is called before the first frame update
    void Start()
    {
    	try {
	    	// assign vehicleInfoController
	    	vehicleInfoController = FindObjectsOfType<VehicleInfoController>()[0];

	    	// initialize Vehicle Name 
	    	string year = vehicleInfoController.year;
	    	string make = vehicleInfoController.make;
	    	string model = vehicleInfoController.model;
	    	Vehicle_Name_Text.text = year + " " + make + " " + model;

	    	// load vehicle model
		    Object Vehicle_Model_load = Resources.Load("3DModels/" + year + "-" + make + "-" + model);
			Vehicle_Model = (GameObject) Instantiate(Vehicle_Model_load, new Vector3(0, 0, 0), Quaternion.identity);
			Vehicle_Model.transform.SetParent(Model_Target_GameObject.transform);

	    }
	    // if vehicleInfoController can't be assigned
	    catch {
	    	Vehicle_Name_Text.text = "Default Vehicle";

	    	// load default vehicle model
		    Object Vehicle_Model_load = Resources.Load("3DModels/2015-HONDA-Civic");
			Vehicle_Model = (GameObject) Instantiate(Vehicle_Model_load, new Vector3(0, 0, 0), Quaternion.identity);
			Vehicle_Model.transform.SetParent(Model_Target_GameObject.transform);
	    }


    	// initialize Vehicle Orientation
    	Vehicle_Orientation_Text.text = vehicleOrientationArray[vehicleOrientationIndex];

	    // assign cut zones
	    Cut_Zones = Vehicle_Model.transform.Find("Cut Zones").gameObject;

	    // assign risk zones
	    Risk_Zones = Vehicle_Model.transform.Find("Risk Zones").gameObject;

	    // assign battery label
	    Battery = Risk_Zones.transform.Find("Battery").gameObject;

	    // assign fueltank label
	    FuelTank = Risk_Zones.transform.Find("Fuel Tank").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
    	// update Battery_Label
    	Vector3 batteryPos = Camera.main.WorldToScreenPoint(Battery.transform.position);
        Battery_Label.transform.position = batteryPos;

        // update FuelTank_Label
    	Vector3 fueltankPos = Camera.main.WorldToScreenPoint(FuelTank.transform.position);
        FuelTank_Label.transform.position = fueltankPos;

        // calculate FPS
        int avgFrameRate = (int)(1f / Time.unscaledDeltaTime);

        // update FPS every second
        elapsed += Time.deltaTime;
     	if (elapsed >= 0.5f) {
        	elapsed = elapsed % 0.5f;
        	FPS_Text.text = avgFrameRate.ToString() + " FPS";
     	}

     	// touch circle
        if (Input.touchCount > 0) {
        	Touch touch = Input.GetTouch(0);
        	touchCircle.transform.position = touch.position;
        }
        else {
        	touchCircle.transform.position = new Vector2(-300, -300);
        }
    }


    // load Vehicle Identification Scene
    public void load_Vehicle_Identification_Scene() {
    	SceneManager.LoadScene("Vehicle Identification");
    }

    // load AR View Mode Scene
    public void load_AR_View_Mode_Scene() {
    	SceneManager.LoadScene("AR View Mode");
    }

    // load Info View Mode Scene
    public void load_Info_View_Mode_Scene() {
    	SceneManager.LoadScene("Info View Mode");
    }

    // change vehicle orientation based on integer passed in
    public void change_Vehicle_Orientation(int num) {

    	if (num > 0) {
    		// increase vehicle orientation index
    		if (vehicleOrientationIndex < 3) {
    			vehicleOrientationIndex = vehicleOrientationIndex + 1;
	    	}
	    	else {
	    		vehicleOrientationIndex = 0;
	    	}
    	}
    	else if (num < 0) {
    		// decrease vehicle orientation index
    		if (vehicleOrientationIndex > 0){
    			vehicleOrientationIndex = vehicleOrientationIndex - 1;
    		}
    		else {
	    		vehicleOrientationIndex = 3;
	    	}
    	}
    	
    	// display text
    	Vehicle_Orientation_Text.text = vehicleOrientationArray[vehicleOrientationIndex];

    	// change guide view
    	Model_Target.ModelTarget.SetActiveGuideViewIndex(vehicleOrientationIndex);

    	Debug.Log(Model_Target.Trackable.Name);
    }

    // click cut zones
    public void click_Cut_Zones() {

    	// change cutZonesToggleOn, change image
    	if (cutZonesToggleOn) { cutZonesToggleOn = false; } else { cutZonesToggleOn = true; }
    	cutZonesToggleOnImage.gameObject.SetActive(cutZonesToggleOn);

    	// toggle Cut_Zones on/off
    	Cut_Zones.SetActive(cutZonesToggleOn);
    }

    // click risk zones
    public void click_Risk_Zones() {

    	// change cutZonesToggleOn, change image
    	if (riskZonesToggleOn) { riskZonesToggleOn = false; } else { riskZonesToggleOn = true; }
    	riskZonesToggleOnImage.gameObject.SetActive(riskZonesToggleOn);

    	// toggle Cut_Zones on/off
    	Risk_Zones.SetActive(riskZonesToggleOn);

    	// toggle labels on/off
    	if (trackableEventHandler.trackingOn == true) {
	    	Battery_Label.gameObject.SetActive(riskZonesToggleOn);
	    	FuelTank_Label.gameObject.SetActive(riskZonesToggleOn);
	    }
    }
}
