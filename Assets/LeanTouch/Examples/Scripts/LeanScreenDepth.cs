using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Lean.Touch
{
	[CustomPropertyDrawer(typeof(LeanScreenDepth))]
	public class P3D_Texture_Drawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var conversion = (LeanScreenDepth.ConversionType)property.FindPropertyRelative("Conversion").enumValueIndex;
			var height     = base.GetPropertyHeight(property, label);

			switch (conversion)
			{
				case LeanScreenDepth.ConversionType.CameraDistance: return height * 2;
				case LeanScreenDepth.ConversionType.DepthIntercept: return height * 2;
				case LeanScreenDepth.ConversionType.PhysicsRaycast: return height * 3;
				case LeanScreenDepth.ConversionType.PlaneIntercept: return height * 3;
			}

			return height;
		}

		public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			var conversion = (LeanScreenDepth.ConversionType)property.FindPropertyRelative("Conversion").enumValueIndex;
			var height     = base.GetPropertyHeight(property, label);

			rect.height = height;

			DrawProperty(ref rect, property, label, "Conversion", label.text, label.tooltip);

			EditorGUI.indentLevel++;
			{
				switch (conversion)
				{
					case LeanScreenDepth.ConversionType.CameraDistance:
					{
						var color = GUI.color; if (property.FindPropertyRelative("Distance").floatValue == 0.0f) GUI.color = Color.red;
						DrawProperty(ref rect, property, label, "Distance", "Distance", "The world space distance from the camera the point will be placed. This should be greater than 0.");
						GUI.color = color;
					}
					break;

					case LeanScreenDepth.ConversionType.DepthIntercept:
						DrawProperty(ref rect, property, label, "Distance", "Z =", "The world space point along the Z axis the plane will be placed. For normal 2D scenes this should be 0.");
					break;

					case LeanScreenDepth.ConversionType.PhysicsRaycast:
					{
						var color = GUI.color; if (property.FindPropertyRelative("Layers").intValue == 0) GUI.color = Color.red;
						DrawProperty(ref rect, property, label, "Layers");
						GUI.color = color;
						DrawProperty(ref rect, property, label, "Distance", "Offset", "The world space offset from the raycast hit point.");
					}
					break;

					case LeanScreenDepth.ConversionType.PlaneIntercept:
					{
						var color = GUI.color; if (property.FindPropertyRelative("Plane").objectReferenceValue == null) GUI.color = Color.red;
						DrawProperty(ref rect, property, label, "Plane");
						GUI.color = color;
						DrawProperty(ref rect, property, label, "Distance", "Offset", "The world space offset from the intercept hit point.");
					}
					break;
				}
			}
			EditorGUI.indentLevel--;
		}

		private void DrawProperty(ref Rect rect, SerializedProperty property, GUIContent label, string childName, string overrideName = null, string overrideTooltip = null)
		{
			var childProperty = property.FindPropertyRelative(childName);

			if (string.IsNullOrEmpty(overrideName) == false)
			{
				label.text    = overrideName;
				label.tooltip = overrideTooltip;

				EditorGUI.PropertyField(rect, childProperty, label);
			}
			else
			{
				EditorGUI.PropertyField(rect, childProperty);
			}

			rect.y += rect.height;
		}
	}
}
#endif

namespace Lean.Touch
{
	// This struct allows you to convert from a screen point to a world point using a variety of different methods
	[System.Serializable]
	public struct LeanScreenDepth
	{
		public enum ConversionType
		{
			CameraDistance,
			DepthIntercept,
			PhysicsRaycast,
			PlaneIntercept,
		}

		[Tooltip("The conversion method used to find a world point from a screen point")]
		public ConversionType Conversion;

		[Tooltip("The plane that will be intercepted")]
		public LeanPlane Plane;

		[Tooltip("The layers used in the raycast")]
		public LayerMask Layers;

		// Toolips are modified at runtime based on Conversion setting
		public float Distance;

		// This will do the actual conversion
		public Vector3 Convert(Vector2 screenPoint, Camera camera, GameObject gameObject = null)
		{
			camera = LeanTouch.GetCamera(camera, gameObject);

			if (camera != null)
			{
				switch (Conversion)
				{
					case ConversionType.CameraDistance:
					{
						var screenPoint3 = new Vector3(screenPoint.x, screenPoint.y, Distance);

						return camera.ScreenToWorldPoint(screenPoint3);
					}

					case ConversionType.DepthIntercept:
					{
						var ray   = camera.ScreenPointToRay(screenPoint);
						var slope = -ray.direction.z;

						if (slope != 0.0f)
						{
							var scale = (ray.origin.z + Distance) / slope;

							return ray.GetPoint(scale);
						}
					}
					break;

					case ConversionType.PhysicsRaycast:
					{
					}
					break;
					//return ray.GetPoint(Distance);

					case ConversionType.PlaneIntercept:
					{
						var ray = camera.ScreenPointToRay(screenPoint);
						var hit = default(Vector3);

						if (Plane.TryRaycast(ray, ref hit, Distance) == true)
						{
							return hit;
						}
					}
					break;
				}
			}

			return default(Vector3);
		}

		// This will return the delta between two converted screenPoints
		public Vector3 ConvertDelta(Vector2 lastScreenPoint, Vector2 screenPoint, Camera camera, GameObject gameObject = null)
		{
			var lastWorldPoint = Convert(lastScreenPoint, camera, gameObject);
			var     worldPoint = Convert(    screenPoint, camera, gameObject);

			return worldPoint - lastWorldPoint;
		}
	}
}