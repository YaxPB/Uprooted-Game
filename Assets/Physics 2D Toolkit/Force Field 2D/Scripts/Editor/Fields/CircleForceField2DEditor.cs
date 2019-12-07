using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;


[CanEditMultipleObjects]
[CustomEditor(typeof(CircleForceField2D))]
public class CircleForceField2DEditor : ForceField2DUniversalEditor {

	private CircleForceField2D scriptRef;
	private readonly SphereBoundsHandle m_BoundsHandle = new SphereBoundsHandle();
	private PrimitiveBoundsHandle boundsHandle { get { return m_BoundsHandle;} }

	void OnEnable () {
		scriptRef = (CircleForceField2D)target;
		transformRef = scriptRef.transform;
		useGamebjectDepthPreviousValue = scriptRef.useGamebjectDepth;
		boundsHandle.axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y;
	}

	public override void OnInspectorGUI() {
		if (isAnylossyScaleAxisEqualToZero (transformRef))
			EditorGUILayout.HelpBox("One of the axis(x,y,z) of global scale of the object is zero.\nThe Circle Field cannot work in this case.", MessageType.Error);
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
		boundsHandle.wireframeColor = ForceField2DPreferences.wireFrameColor;
		boundsHandle.handleColor = ForceField2DPreferences.handlesColor;

		if (!isAnylossyScaleAxisEqualToZero (transformRef)) {
			using (new Handles.DrawingScope (Matrix4x4.TRS (transformRef.position, Quaternion.identity, Vector3.one))) {
				Matrix4x4 localToWorldMatrix = transformRef.localToWorldMatrix;
				boundsHandle.center = ProjectOntoWorldPlane (Handles.inverseMatrix * (localToWorldMatrix * (Vector4)scriptRef.offset));
				CopyColliderSizeToHandle ();
				EditorGUI.BeginChangeCheck ();
				boundsHandle.DrawHandle ();
				if (EditorGUI.EndChangeCheck ()) {
					Undo.RecordObject (scriptRef, "ForceField2D: Changed Scale");
					if (CopyHandleSizeToCollider ()) {
						scriptRef.offset = localToWorldMatrix.inverse * (Vector4)ProjectOntoColliderPlane (Handles.matrix * (Vector4)boundsHandle.center, localToWorldMatrix);
					}
				}
			}
		}
	}



	private Vector3 ProjectOntoWorldPlane(Vector3 worldVector) {
		worldVector.z = 0f;
		return worldVector;
	}

	private Vector3 ProjectOntoColliderPlane(Vector3 worldVector, Matrix4x4 colliderTransformMatrix) {
		Plane plane = new Plane(Vector3.Cross(colliderTransformMatrix * (Vector4)Vector3.right, colliderTransformMatrix * (Vector4)Vector3.up), Vector3.zero);
		Ray ray = new Ray(worldVector, Vector3.forward);
		float distance;
		if (plane.Raycast(ray, out distance))
			return ray.GetPoint(distance);
		ray.direction = Vector3.back;
		plane.Raycast(ray, out distance);
		return ray.GetPoint(distance);
	}


	private void CopyColliderSizeToHandle() {
		scriptRef = (CircleForceField2D)target;
		m_BoundsHandle.radius = scriptRef.radius * GetRadiusScaleFactor();
	}

	private bool CopyHandleSizeToCollider() {
		scriptRef = (CircleForceField2D)target;
		float radius = scriptRef.radius;
		float radiusScaleFactor = GetRadiusScaleFactor();
		scriptRef.radius = ((!Mathf.Approximately(radiusScaleFactor, 0f)) ? (m_BoundsHandle.radius / GetRadiusScaleFactor()) : 0f);
		return scriptRef.radius != radius;
	}

	private float GetRadiusScaleFactor() {
		Vector3 lossyScale = transformRef.lossyScale;
		return Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.y));
	}
}
