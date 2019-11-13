using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateController : MonoBehaviour
{
	Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    Rect rect;
    bool validSwipe = false;

    // Start is called before the first frame update
    void Start()
    {
    	rect = GetComponent<RectTransform>().rect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void rotate_Model(GameObject vehicle) {
    	// rotate vehicle model
    	if (Input.touchCount > 0) {

    		// get touch
    		Touch touch = Input.GetTouch(0);

    		// if touch began, and in the rect, set the prevpos to current pos, to start from zero
    		if (touch.phase == TouchPhase.Began && rect.Contains(touch.position)) {

    			mPrevPos = new Vector3(touch.position.x, touch.position.y);
    			validSwipe = true;
    		}
    		// if continue touch, and swipe began in rect, calculate how much to rotate
    		else if (touch.phase == TouchPhase.Moved && validSwipe) {
    			mPosDelta = new Vector3(touch.position.x, touch.position.y) - mPrevPos;
	    		if (Vector3.Dot(vehicle.transform.up, Vector3.up) >= 0) {
	    			vehicle.transform.Rotate(vehicle.transform.up, -Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
	    		}
	    		else {
	    			vehicle.transform.Rotate(vehicle.transform.up, Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
	    		}
	    		vehicle.transform.Rotate(Camera.main.transform.right, Vector3.Dot(mPosDelta, Camera.main.transform.up), Space.World);

	    		mPrevPos = new Vector3(touch.position.x, touch.position.y);
    		}
    		else if (touch.phase == TouchPhase.Ended) {
    			validSwipe = false;
    		}
    	}
    }
}
