using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Positioning : MonoBehaviour {
    public GameObject ui;
    float uiDistance;

    float x_change;
    float z_change;
    Vector3 previousPosition;
	// Use this for initialization
	void Start () {
        uiDistance = 90f;
        x_change = 0f;
        z_change = 0f;
        previousPosition = this.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        x_change += this.transform.position.x - previousPosition.x;
        z_change += this.transform.position.z - previousPosition.z;

        previousPosition = this.transform.position;

        if(x_change > 60f || x_change < -60f || z_change > 60f || z_change < -60f)
        {
            ui.transform.position = previousPosition + uiDistance * this.transform.forward;
            ui.transform.rotation = this.transform.rotation;
            x_change = 0f;
            z_change = 0f;
        }

    }
}
