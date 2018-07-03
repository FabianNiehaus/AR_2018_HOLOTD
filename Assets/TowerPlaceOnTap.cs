using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlaceOnTap : MonoBehaviour, IInputClickHandler
{

    public GameObject original;
    private float lastClickTime = 0;
    private float debounceDelay = 0.115f;
    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (Time.time - lastClickTime < debounceDelay)
        {
            return;
        }

        lastClickTime = Time.time;
        GameObject cube = GameObject.Instantiate(original);
        cube.transform.position = GetComponent<Transform>().position;
        Debug.Log("AirTap!");

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
