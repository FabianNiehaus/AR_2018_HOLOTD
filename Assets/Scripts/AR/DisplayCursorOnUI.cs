using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCursorOnUI : MonoBehaviour {

    // Object assignment
    public GameObject cursor;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        //
        // If on GUI menu control structure
        if (GazeManager.Instance.IsGazingAtObject && GazeManager.Instance.HitObject != null &&
            (GazeManager.Instance.HitObject.transform.name == "icon" ||
            GazeManager.Instance.HitObject.transform.name == "guiManager"))
        {
            // Scale
            //cursor.transform.localScale = halfScale;

            // Move the cursor to the point where the raycast hit
            //  to display it on the gui element
            cursor.transform.position = GazeManager.Instance.HitInfo.point;

            // Enable the cursor mesh
            cursor.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    


}
