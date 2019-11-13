using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
public class InfoViewModeController : MonoBehaviour
{
	// VehicleInfoController
	public VehicleInfoController vehicleInfoController;

	// Vehicle Name
	public Text Vehicle_Name_Text;

	// Toggle 
	public bool cutZonesToggleOn = true;
	public bool riskZonesToggleOn = true;
	public bool safetyInfoToggleOn = false;
	public bool extricationInfoToggleOn = false;
	public RawImage cutZonesToggleOnImage;
	public RawImage riskZonesToggleOnImage;
	public RawImage safetyInfoToggleOnImage;
	public RawImage extricationInfoToggleOnImage;

	// Vehicle Model
	GameObject Vehicle_Model;
	GameObject Cut_Zones;
	GameObject Risk_Zones;
	GameObject Battery;
	GameObject FuelTank;
	public Text Battery_Label;
	public Text FuelTank_Label;
	Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;
    public RotateController rotateController;

    // panels
    public GameObject SafetyInfoPanel;
    public GameObject ExtricationInfoPanel;

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


	    }
	    // if vehicleInfoController can't be assigned
	    catch {
	    	// Vehicle Name is initialized as default
	    	Vehicle_Name_Text.text = "Default Vehicle";

	    	// load default vehicle model
		    Object Vehicle_Model_load = Resources.Load("3DModels/2015-HONDA-Civic");
			Vehicle_Model = (GameObject) Instantiate(Vehicle_Model_load, new Vector3(0, 0, 0), Quaternion.identity);
	    }

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
    	// rotate
    	rotateController.rotate_Model(Vehicle_Model);

    	// update Battery_Label
    	Vector3 batteryPos = Camera.main.WorldToScreenPoint(Battery.transform.position);
        Battery_Label.transform.position = batteryPos;

        // update FuelTank_Label
    	Vector3 fueltankPos = Camera.main.WorldToScreenPoint(FuelTank.transform.position);
        FuelTank_Label.transform.position = fueltankPos;

        // touch circle
        if (Input.touchCount > 0) {
        	Touch touch = Input.GetTouch(0);
        	touchCircle.transform.position = touch.position;
        }
        else {
        	touchCircle.transform.position = new Vector2(-300, -300);
        }
    }

    // load AR View Mode 
    public void load_AR_View_Mode_Scene() {
    	SceneManager.LoadScene("AR View Mode");
    }

    // load Vehicle Identification 
    public void load_Vehicle_Identification_Scene() {
    	SceneManager.LoadScene("Vehicle Identification");
    }

    public void rotate_Model() {
    	// rotate vehicle model
    	if (Input.touchCount > 0) {

    		// get touch
    		Touch touch = Input.GetTouch(0);

    		// if touch began, set the prevpos to current pos, to start from zero
    		if (touch.phase == TouchPhase.Began) {
    			mPrevPos = new Vector3(touch.position.x, touch.position.y);
    		}
    		// if continue touch, calculate how much to rotate
    		else if (touch.phase == TouchPhase.Moved) {
    			mPosDelta = new Vector3(touch.position.x, touch.position.y) - mPrevPos;
	    		if (Vector3.Dot(Vehicle_Model.transform.up, Vector3.up) >= 0) {
	    			Vehicle_Model.transform.Rotate(Vehicle_Model.transform.up, -Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
	    		}
	    		else {
	    			Vehicle_Model.transform.Rotate(Vehicle_Model.transform.up, Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
	    		}
	    		Vehicle_Model.transform.Rotate(Camera.main.transform.right, Vector3.Dot(mPosDelta, Camera.main.transform.up), Space.World);

	    		mPrevPos = new Vector3(touch.position.x, touch.position.y);
    		}
    	}
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
    	Battery_Label.gameObject.SetActive(riskZonesToggleOn);
    	FuelTank_Label.gameObject.SetActive(riskZonesToggleOn);
    }

    // click safety info
    public void click_Safety_Info() {

    	// if extrication-info is active, deactivate extrication-info 
    	if (extricationInfoToggleOn) {click_Extrication_Info();}

    	// change safetyInfoToggleOn, change image
    	if (safetyInfoToggleOn) { safetyInfoToggleOn = false; } else { safetyInfoToggleOn = true; }
    	safetyInfoToggleOnImage.gameObject.SetActive(safetyInfoToggleOn);

    	// activate panel
    	SafetyInfoPanel.SetActive(safetyInfoToggleOn);
    }

    // click extrication info
    public void click_Extrication_Info() {

    	// if safety-info is active, deactivate safety-info 
    	if (safetyInfoToggleOn) {click_Safety_Info();}

    	// change extricationInfoToggleOn, change image
    	if (extricationInfoToggleOn) { extricationInfoToggleOn = false; } else { extricationInfoToggleOn = true; }
    	extricationInfoToggleOnImage.gameObject.SetActive(extricationInfoToggleOn);

    	// activate panel
    	ExtricationInfoPanel.SetActive(extricationInfoToggleOn);
    }
}
