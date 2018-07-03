using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCursorCoordinates : MonoBehaviour {

    GameObject cursor;

	// Use this for initialization
	void Start () {
        cursor = GameObject.FindGameObjectWithTag("Cursor");

        GetComponent<TextMesh>().text = 
            "x: " + cursor.transform.position.x.ToString() + "\n" 
            + "y: " + cursor.transform.position.y.ToString() + "\n" 
            + "z: " + cursor.transform.position.z.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<TextMesh>().text =
            "x: " + cursor.transform.position.x.ToString() + "\n"
            + "y: " + cursor.transform.position.y.ToString() + "\n"
            + "z: " + cursor.transform.position.z.ToString();
    }
}
