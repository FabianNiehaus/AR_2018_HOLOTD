using System;
using Core.Input;
using Core.Utilities;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.WSA.Input;

namespace HoloTD.Input
{
    /// <summary>
    /// Class to manage tap/drag/pinch gestures and other controls
    /// </summary>
    public class InputController : Singleton<InputController>
    {

        AnimatedCursor gazeCursor;
       
        /// <summary>
        /// Current Gaze pointer info
        /// </summary>
        public PointerInfo BasicGazeInfo { get; private set; }

		/// <summary>
		/// Event called when a pointer is tapped
		/// </summary>
		public event Action<PointerActionInfo> Tapped;

        /// <summary>
		/// Event called whenever the gaze is moved
		/// </summary
        public event Action<PointerInfo> GazeMoved;

        private GestureRecognizer recognizer;

		protected override void Awake()
		{
			base.Awake();

            gazeCursor = FindObjectOfType<AnimatedCursor>();

            BasicGazeInfo = new GazeCursorInfo { currentPosition = gazeCursor.transform.position };

            recognizer = new GestureRecognizer();
            recognizer.SetRecognizableGestures(GestureSettings.Tap);
            recognizer.Tapped += TapEventHandler;
            recognizer.StartCapturingGestures();


        }

        void OnDisable()
        {
            recognizer.Tapped -= TapEventHandler;
        }

		/// <summary>
		/// Update all input
		/// </summary>
		void Update()
		{
             
            BasicGazeInfo.previousPosition = BasicGazeInfo.currentPosition;
            BasicGazeInfo.currentPosition = gazeCursor.transform.localPosition;
            BasicGazeInfo.delta = BasicGazeInfo.currentPosition - BasicGazeInfo.previousPosition;
 
            // Move event
            if (BasicGazeInfo.delta.sqrMagnitude > Mathf.Epsilon)
            {
                if (GazeMoved != null)
                {
                    GazeMoved(BasicGazeInfo);
                }
                
            }

            if (UnityEngine.Input.GetButtonDown("Fire1"))
            {
                GestureInfo gesture = new GestureInfo
                {
                    delta = BasicGazeInfo.delta,
                    previousPosition = BasicGazeInfo.previousPosition,
                    currentPosition = BasicGazeInfo.currentPosition
                };

                if (Tapped != null)
                {
                    Tapped(gesture);
                }
            }

        }

        void TapEventHandler(TappedEventArgs tappedEventArgs)
        {
            GestureInfo gesture = new GestureInfo
            {
                delta = BasicGazeInfo.delta,
                previousPosition = BasicGazeInfo.previousPosition,
                currentPosition = BasicGazeInfo.currentPosition
            };

            if(EventSystem.current.isActiveAndEnabled)
            {
                Tapped(gesture);
            }
        }
    }
}