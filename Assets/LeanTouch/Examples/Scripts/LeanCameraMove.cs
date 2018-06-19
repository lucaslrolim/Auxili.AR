using UnityEngine;

namespace Lean.Touch
{
	// This script allows you to track & pedestral this GameObject (e.g. Camera) based on finger drags
	public class LeanCameraMove : MonoBehaviour
	{
		[Tooltip("The camera the movement will be done relative to (None = MainCamera)")]
		public Camera Camera;

		[Tooltip("Ignore fingers with StartedOverGui?")]
		public bool IgnoreStartedOverGui = true;

		[Tooltip("Ignore fingers with IsOverGui?")]
		public bool IgnoreIsOverGui;

		[Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
		public int RequiredFingerCount;

		[Tooltip("The sensitivity of the movement, use -1 to invert")]
		public float Sensitivity = 1.0f;

		public LeanScreenDepth ScreenDepth;

		protected virtual void LateUpdate()
		{
			// Get the fingers we want to use
			var fingers = LeanTouch.GetFingers(IgnoreStartedOverGui, IgnoreIsOverGui, RequiredFingerCount);

			// Get the last and current screen point of all fingers
			var lastScreenPoint = LeanGesture.GetLastScreenCenter(fingers);
			var screenPoint     = LeanGesture.GetScreenCenter(fingers);

			// Get the world delta of them after conversion
			var worldDelta = ScreenDepth.ConvertDelta(lastScreenPoint, screenPoint, Camera, gameObject);

			// Pan the camera based on the world delta
			transform.position -= worldDelta * Sensitivity;
		}
	}
}