using Core.Input;
using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace HoloTD.Input
{
    /// <summary>
    /// Base control scheme for desktop devices, which performs CameraRig motion
    /// </summary>
    public class HoloInput : InputScheme
    {
        /// <summary>
        /// Gets whether the scheme should be activated or not
        /// </summary>
        public override bool shouldActivate
        {
            get
            {
                bool gazeManagerPresent = HoloToolkit.Unity.InputModule.GazeManager.Instance;
                bool gestureRecognizedThisFrame = InputController.instance.gestureRecognizedThisFrame;
                bool gazeMovedOnThisFrame = InputController.instance.gazeMovedOnThisFrame;

                return (gazeManagerPresent || gestureRecognizedThisFrame || gazeMovedOnThisFrame);
            }
        }

        /// <summary>
        /// This is the default scheme on desktop devices
        /// </summary>
        public override bool isDefault
        {
            get
            {
#if UNITY_STANDALONE || UNITY_EDITOR
                return true;
#else
				return false;
#endif
            }
        }

        /// <summary>
        /// Register input events
        /// </summary>
        protected virtual void OnEnable()
        {
            if (!InputController.instanceExists)
            {
                Debug.LogError("[UI] Gaze and Gesture UI requires InputController");
                return;
            }

            InputController controller = InputController.instance;
        }

        /// <summary>
        /// Deregister input events
        /// </summary>
        protected virtual void OnDisable()
        {
            if (!InputController.instanceExists)
            {
                return;
            }

            InputController controller = InputController.instance;
        }
    }
}