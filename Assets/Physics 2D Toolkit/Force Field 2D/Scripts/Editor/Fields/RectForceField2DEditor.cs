using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

[CanEditMultipleObjects]
[CustomEditor(typeof(RectForceField2D))]
public class RectForceField2DEditor : ForceField2DUniversalEditor {

	private RectForceField2D scriptRef;
	private readonly BoxBoundsHandle m_BoundsHandle = new BoxBoundsHandle();

	void OnEnable () {
		scriptRef = (RectForceField2D)target;
		transformRef = scriptRef.transform;
		useGamebjectDepthPreviousValue = scriptRef.useGamebjectDepth;
		m_BoundsHandle.axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y;
	}

	public override void OnInspectorGUI() {
		if (isAnylossyScaleAxisEqualToZero (transformRef))
			EditorGUILayout.HelpBox("One of the axis(x,y,z) of global scale of the object is zero.\nThe Rect Field cannot work in this case.", MessageType.Error);
		base.OnInspectorGUI ();
		if (useGamebjectDepthPreviousValue) {
			scriptRef.minDepth = transformRef.position.z;
			scriptRef.maxDepth = transformRef.position.z;
		}
		if (scriptRef.useGamebjectDepth != useGamebjectDepthPreviousValue) {
			if (!useGamebjectDepthPreviousValue) {
				useGamebjectDepthPreviousValue = true;
				scriptRef.minDepth = transformRef.position.z;
				scriptRef.maxDepth = transformRef.position.z;
			} else {
				useGamebjectDepthPreviousValue = false;
				scriptRef.minDepth = -Mathf.Infinity;
				scriptRef.maxDepth = Mathf.Infinity;
			}
		}
		if (scriptRef.useAdjustmentAngle && !string.Equals(scriptRef.directionMode , "Attractive") && !string.Equals(scriptRef.directionMode , "Push")) {
			scriptRef.useAdjustmentAngle = false;
		}
	}

	void OnSceneGUI() {
		m_BoundsHandle.wireframeColor = ForceField2DPreferences.wireFrameColor;
		m_BoundsHandle.handleColor = ForceField2DPreferences.handlesColor;

		if (!isAnylossyScaleAxisEqualToZero (transformRef)) {
			Matrix4x4 matrix4x = transformRef.localToWorldMatrix;
			matrix4x.SetRow (0, Vector4.Scale (matrix4x.GetRow (0), new Vector4 (1f, 1f, 0f, 1f)));
			matrix4x.SetRow (1, Vector4.Scale (matrix4x.GetRow (1), new Vector4 (1f, 1f, 0f, 1f)));
			Vector3 position = transformRef.position;
			matrix4x.SetRow (2, new Vector4 (0f, 0f, 1f, position.z));

			using (new Handles.DrawingScope (matrix4x)) {
				

				m_BoundsHandle.center = scriptRef.offset;
				m_BoundsHandle.size = scriptRef.size;

				EditorGUI.BeginChangeCheck ();
				m_BoundsHandle.DrawHandle ();
				if (EditorGUI.EndChangeCheck ()) {
					Undo.RecordObject (scriptRef, "ForceField2D: Changed Scale");
					Vector2 size = scriptRef.size;
					scriptRef.size = m_BoundsHandle.size;
					if (scriptRef.size != size)
						scriptRef.offset = m_BoundsHandle.center;
				}
			}
		}
	}




}