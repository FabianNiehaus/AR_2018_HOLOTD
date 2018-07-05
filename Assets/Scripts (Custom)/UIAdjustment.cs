using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdjustment : MonoBehaviour {
    GameObject gameUI;
    float uiDistance;

    float x_change;
    float z_change;
    float x_rot_change;
    float y_rot_change;
    float z_rot_change;
    Vector3 previousPosition;
    Vector3 previousAngle;
    // Use this for initialization
    void Start () {
        uiDistance = 30f;
        x_change = 0f;
        z_change = 0f;
        x_rot_change = 0f;
        y_rot_change = 0f;
        z_rot_change = 0f;
        previousPosition = this.transform.position;
        previousAngle = this.transform.rotation.eulerAngles;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameObject.FindGameObjectWithTag("GameUI") != null)
        {
            gameUI = GameObject.FindGameObjectWithTag("GameUI");
        }

        x_change += this.transform.position.x - previousPosition.x;
        z_change += this.transform.position.z - previousPosition.z;

        x_rot_change += this.transform.rotation.eulerAngles.x - previousAngle.x;
        y_rot_change += this.transform.rotation.eulerAngles.y - previousAngle.y;
        z_rot_change += this.transform.rotation.eulerAngles.z - previousAngle.z;

        previousPosition = this.transform.position;
        previousAngle = this.transform.rotation.eulerAngles;

        if (x_change > 60f || x_change < -60f || z_change > 60f || z_change < -60f)
        {
            gameUI.transform.position = previousPosition + uiDistance * this.transform.forward;
            gameUI.transform.rotation = this.transform.rotation;
            x_change = 0f;
            z_change = 0f;
        }
        if(x_rot_change > 15f || y_rot_change > 15f || z_rot_change > 15f || x_rot_change < -15f || y_rot_change < -15f || z_rot_change < -15f)
        {
            gameUI.transform.position = previousPosition + uiDistance * this.transform.forward;
            gameUI.transform.rotation = this.transform.rotation;
            x_rot_change = 0f;
            y_rot_change = 0f;
            z_rot_change = 0f;
        }
    }
}
