using UnityEngine;
using UnityEngine.Events;

namespace Lean.Touch
{
	// This script calls the OnFingerTap event when a finger taps the screen
	public class LeanFingerTap : MonoBehaviour
	{
		// Event signature
		[System.Serializable] public class LeanFingerEvent : UnityEvent<LeanFinger> {}

		[Tooltip("Ignore fingers with StartedOverGui?")]
		public bool IgnoreStartedOverGui = true;

		[Tooltip("Ignore fingers with OverGui?")]
		public bool IgnoreIsOverGui;

		[Tooltip("How many times must this finger tap before OnFingerTap gets called? (0 = every time)")]
		public int RequiredTapCount = 0;

		[Tooltip("How many times repeating must this finger tap before OnFingerTap gets called? (e.g. 2 = 2, 4, 6, 8, etc) (0 = every time)")]
		public int RequiredTapInterval;

		[Tooltip("Do nothing if this LeanSelectable isn't selected?")]
		public LeanSelectable RequiredSelectable;

		public LeanFingerEvent OnTap;

		protected virtual void OnEnable()
		{
			// Hook events
			LeanTouch.OnFingerTap += FingerTap;
		}

		protected virtual void OnDisable()
		{
			// Unhook events
			LeanTouch.OnFingerTap -= FingerTap;
		}

		private void FingerTap(LeanFinger finger)
		{
			// Ignore?
			if (IgnoreStartedOverGui == true && finger.StartedOverGui == true)
			{
				return;
			}

			if (IgnoreIsOverGui == true && finger.IsOverGui == true)
			{
				return;
			}

			if (RequiredTapCount > 0 && finger.TapCount != RequiredTapCount)
			{
				return;
			}

			if (RequiredTapInterval > 0 && (finger.TapCount % RequiredTapInterval) != 0)
			{
				return;
			}

			if (RequiredSelectable != null && RequiredSelectable.IsSelected == false)
			{
				return;
			}

			// Call event
			if (OnTap != null)
			{
				OnTap.Invoke(finger);
			}
		}
	}
}