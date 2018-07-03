using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkCameraToCanvas : MonoBehaviour {

    Camera uiCam;

    // Use this for initialization
    void Start () {
        List<Camera> cameras = new List<Camera>(FindObjectsOfType<Camera>());
        foreach (Camera camera in cameras)
        {
            if (camera.name.Equals("UiRaycastCamera"))
            {
                uiCam = camera;
            }
        };

        GetComponent<Canvas>().worldCamera = uiCam;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
