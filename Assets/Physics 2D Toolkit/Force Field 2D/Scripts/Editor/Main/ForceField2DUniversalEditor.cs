using UnityEngine;
using UnityEditor;

public class ForceField2DUniversalEditor : Editor {

	protected bool useGamebjectDepthPreviousValue = false;
	protected Transform transformRef;

	protected bool isAnylossyScaleAxisEqualToZero (Transform _transform) {
		if (_transform.lossyScale.x == 0f || _transform.lossyScale.y == 0f || _transform.lossyScale.z == 0f)
			return true;
		return false;
	}
}
