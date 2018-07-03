using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class GestureManager : MonoBehaviour {

    public GameObject TrackingObject;
    public GameObject configuration;
    public GameObject placementAreas;
    public GameObject navigationObjects;
    public GameObject light;
    private HashSet<uint> trackedHands = new HashSet<uint>();
    private GestureRecognizer gestureRecognizer;
    private Dictionary<uint, GameObject> trackingObject = new Dictionary<uint, GameObject>();
    private uint activeId;

    bool objectOnHold = false;
    bool mapPlaced = false;

    void Awake()
    {

        InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
        InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
        InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;

        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
        gestureRecognizer.Tapped += GestureRecognizerTapped;
        gestureRecognizer.StartCapturingGestures();
    }

    private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs args)
    {
        uint id = args.state.source.id;
        // Check to see that the source is a hand.
        if (args.state.source.kind != InteractionSourceKind.Hand)
        {
            return;
        }
        trackedHands.Add(id);
        activeId = id;

        var obj = Instantiate(TrackingObject) as GameObject;
        Vector3 pos;

        if (args.state.sourcePose.TryGetPosition(out pos))
        {
            obj.transform.position = pos;
        }

        trackingObject.Add(id, obj);
    }

    private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs args)
    {
        uint id = args.state.source.id;
        Vector3 pos;
        Quaternion rot;

        if (args.state.source.kind == InteractionSourceKind.Hand)
        {
            if (trackingObject.ContainsKey(id))
            {
                if (args.state.sourcePose.TryGetPosition(out pos))
                {
                    trackingObject[id].transform.position = pos;
                    if (!mapPlaced)
                    {
                        configuration.transform.position = pos + Camera.main.transform.forward * 5f;
                        placementAreas.transform.position = pos + Camera.main.transform.forward * 5f;
                        navigationObjects.transform.position = pos + Camera.main.transform.forward * 5f;
                        light.transform.position = pos + Camera.main.transform.forward * 5f;
                    }
                }

                if (args.state.sourcePose.TryGetRotation(out rot))
                {
                    trackingObject[id].transform.rotation = rot;
                }
            }
        }
        
    }

    private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs args)
    {
        uint id = args.state.source.id;
        // Check to see that the source is a hand.
        if (args.state.source.kind != InteractionSourceKind.Hand)
        {
            return;
        }

        if (trackedHands.Contains(id))
        {
            trackedHands.Remove(id);
        }

        if (trackingObject.ContainsKey(id))
        {
            var obj = trackingObject[id];
            trackingObject.Remove(id);
            Destroy(obj);
        }
        if (trackedHands.Count > 0)
        {
            activeId = trackedHands.First();
        }
    }

    private void GestureRecognizerTapped(TappedEventArgs args)
    {
        uint id = args.source.id;
        

        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        if (!mapPlaced)
        {
            mapPlaced = true;
        }

        /*RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            if (!mapPlaced)
            {
                // If the raycast hit a hologram, use that as the focused object.
                for (int i = 0; i < FocusedObjects.Length; i++)
                {
                    FocusedObjects[i] = hitInfo.collider.gameObject;
                    mapPlaced = true;
                }
            }
            else
            {
                mapPlaced = false;
            }
        }*/
    }

    void OnDestroy()
    {
        InteractionManager.InteractionSourceDetected -= InteractionManager_InteractionSourceDetected;
        InteractionManager.InteractionSourceUpdated -= InteractionManager_InteractionSourceUpdated;
        InteractionManager.InteractionSourceLost -= InteractionManager_InteractionSourceLost;

        gestureRecognizer.Tapped -= GestureRecognizerTapped;
        gestureRecognizer.StopCapturingGestures();
    }
}
