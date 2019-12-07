using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

[CanEditMultipleObjects]
[CustomEditor(typeof(CapsuleForceField2D))]
public class CapsuleForceField2DEditor : ForceField2DUniversalEditor {

	private CapsuleForceField2D scriptRef;
	private readonly CapsuleBoundsHandle m_BoundsHandle = new CapsuleBoundsHandle();
	private PrimitiveBoundsHandle boundsHandle { get { return m_BoundsHandle;} }
		
	void OnEnable () {
		scriptRef = (CapsuleForceField2D)target;
		transformRef = scriptRef.transform;
		useGamebjectDepthPreviousValue = scriptRef.useGamebjectDepth;
		boundsHandle.axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y;
	}

	public override void OnInspectorGUI() {
		if (isAnylossyScaleAxisEqualToZero (transformRef))
			EditorGUILayout.HelpBox("One of the axis(x,y,z) of global scale of the object is zero.\nThe Capsule Field cannot work in this case.", MessageType.Error);
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
			using (new Handles.DrawingScope (Matrix4x4.TRS (transformRef.position, GetHandleRotation (), Vector3.one))) {
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

	private Quaternion GetHandleRotation() {
		Vector3 upwards;
		Vector3 _;
		GetHandleVectorsInWorldSpace(out upwards, out _);
		return Quaternion.LookRotation(Vector3.forward, upwards);
	}


	private bool CopyHandleSizeToCollider() {
		Vector3 vector;
		Vector3 vector2;
		if (scriptRef.capsuleDirection == CapsuleDirection2D.Horizontal) {
			vector = Vector3.up;
			vector2 = Vector3.right;
		} else {
			vector = Vector3.right;
			vector2 = Vector3.up;
		}
		Vector3 vector3 = Handles.matrix * (Vector4)(vector2 * m_BoundsHandle.height);
		Vector3 vector4 = Handles.matrix * (Vector4)(vector * m_BoundsHandle.radius * 2f);
		Matrix4x4 localToWorldMatrix = transformRef.localToWorldMatrix;
		Vector3 worldVector = ProjectOntoWorldPlane (localToWorldMatrix * (Vector4)vector).normalized * vector4.magnitude;
		Vector3 worldVector2 = ProjectOntoWorldPlane (localToWorldMatrix * (Vector4)vector2).normalized * vector3.magnitude;
		worldVector = ProjectOntoColliderPlane (worldVector, localToWorldMatrix);
		worldVector2 = ProjectOntoColliderPlane (worldVector2, localToWorldMatrix);
		Vector2 size = scriptRef.size;
		scriptRef.size = localToWorldMatrix.inverse * (Vector4)(worldVector + worldVector2);
		return scriptRef.size != size;
	}



	private void CopyColliderSizeToHandle() {
		Vector3 vec;
		Vector3 vec2;
		GetHandleVectorsInWorldSpace(out vec, out vec2);
		CapsuleBoundsHandle boundsHandle = m_BoundsHandle;
		float num = 0f;
		m_BoundsHandle.radius = num;
		boundsHandle.height = num;
		m_BoundsHandle.height = vec.magnitude;
		m_BoundsHandle.radius = vec2.magnitude * 0.5f;
	}

	private void GetHandleVectorsInWorldSpace( out Vector3 handleHeightVector, out Vector3 handleDiameterVector) {
		Matrix4x4 localToWorldMatrix = transformRef.localToWorldMatrix;
		Matrix4x4 lhs = localToWorldMatrix;
		Vector3 right = Vector3.right;
		Vector2 size = scriptRef.size;
		Vector3 vector = ProjectOntoWorldPlane (lhs * (Vector4)(right * size.x));
		Matrix4x4 lhs2 = localToWorldMatrix;
		Vector3 up = Vector3.up;
		Vector2 size2 = scriptRef.size;
		Vector3 vector2 = ProjectOntoWorldPlane (lhs2 * (Vector4)(up * size2.y));
		if (scriptRef.capsuleDirection == CapsuleDirection2D.Horizontal) {
			handleDiameterVector = vector2;
			handleHeightVector = vector;
		} else {
			handleDiameterVector = vector;
			handleHeightVector = vector2;
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
}
