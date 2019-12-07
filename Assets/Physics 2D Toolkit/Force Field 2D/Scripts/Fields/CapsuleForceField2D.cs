using UnityEngine;
using System.Collections;
using ForceField2D;
using ForceField2D.PropertyAttributes;

[HelpURL("https://pulsarxstudio.com/force-field-2d/")]
[AddComponentMenu("Physics 2D Toolkit/Capsule Force Field 2D",order:0)]
public class CapsuleForceField2D : ForceField2DUniversal {

	[Header("Capsule")]
	public Vector2 offset = default(Vector2);
	public Vector2 size = new Vector2(6.6f,2.7f);
	public CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Horizontal;

	[Header("Direction")]
	[Tooltip("Direction mode for rigidbodies affected with force field.")]
	[StringListPopup(new string[]{"Attractive","Push","Constant", "Curve"})] public string directionMode = "Attractive";
	[StringConditionalHide("directionMode",true,false,"Constant")] public Vector2 direction = default(Vector2);
	[StringConditionalHide("directionMode",true,false,"Curve")] public AnimationCurve x = new AnimationCurve (){};
	[StringConditionalHide("directionMode",true,false,"Curve")] public AnimationCurve y = new AnimationCurve () {};
	[Tooltip("Adjusting the angle of direction is modifying the original direction by degrees.")]
	[StringConditionalHide2("directionMode",true,false,new string[]{"Attractive","Push"})] public bool useAdjustmentAngle = false;
	[Tooltip("Angle (In degrees)")]
	[BoolConditionalHide("useAdjustmentAngle",true,false)]  public float adjustmentAngle = 0f;

	[Header("Force")]
	[StringListPopup(new string[]{"Constant","Curve"})] public string forceMode = "Constant";
	[Tooltip("Force to apply on rigidbodies.")]
	[StringConditionalHide("forceMode",true,false,"Constant")] public float force = 50f;
	[StringConditionalHide("forceMode",true,false,"Curve")] public AnimationCurve forceCurve = new AnimationCurve () {};
	[Tooltip("The method used to apply the force to its targets.")]
	public ForceMode2D forceMode2D = ForceMode2D.Force;


	// Local
	private Collider2D[] colliders = new Collider2D[]{};
	private Vector2 finalCapsuleSize = default(Vector2);
	private float angle = 0f;
	private Vector2 point;
	private float finalForce = 0f;
	private Vector2 finalDirection = default(Vector2);

	void Awake () {
		AwakeUniversal ();
	}
	void Start () {
		StartUniversal ();
	}
	void OnEnable () {
		OnEnableUniversal ();
	}
	void Update () {
		UpdateUniversal ();
	}


	void FixedUpdate () {
		if (!activated)
			return;
		HandleBasicCalculations ();
		CalcualteFinalForce ();
		colliders = Physics2D.OverlapCapsuleAll (point,finalCapsuleSize ,capsuleDirection, angle, layerFilter, minDepth, maxDepth);
		foreach (Collider2D hit in colliders) {
			Rigidbody2D Rb = hit.GetComponent<Rigidbody2D> ();
			if (CheckCollider (hit) && CheckCollidedRigidbody2D (Rb)) {
				CalculateFinalDirection (Rb);
				Rb.AddForce (finalDirection * finalForce, forceMode2D);
			}
		}
	}

	private void CalculateFinalDirection (Rigidbody2D _body) {
		Vector2 _bodyPosition = (Vector2)_body.transform.position;
		if (string.Equals (directionMode, "Constant")) {
			finalDirection = direction;
		} else if (string.Equals (directionMode, "Curve")) {
			finalDirection.x = x.Evaluate (timeSinceActivated);
			finalDirection.y = y.Evaluate (timeSinceActivated);
		} else if (string.Equals (directionMode, "Push")) {
			if (useAdjustmentAngle)
				finalDirection = (_bodyPosition - point).normalized.RotateVector2 (adjustmentAngle);
			else
				finalDirection = (_bodyPosition - point).normalized;
		} else if (string.Equals (directionMode, "Attractive")) {
			if (useAdjustmentAngle)
				finalDirection = (point - _bodyPosition).normalized.RotateVector2 (adjustmentAngle);
			else
				finalDirection = (point - _bodyPosition).normalized;
		}
	}

	private void CalcualteFinalForce () {
		if (string.Equals (forceMode, "Constant"))
			finalForce = force;
		else
			finalForce = forceCurve.Evaluate (timeSinceActivated);
	}

	private void HandleBasicCalculations () {
		finalCapsuleSize = size;
		finalCapsuleSize.x *= _transform.lossyScale.x;
		finalCapsuleSize.y *= _transform.lossyScale.y;
		angle = _transform.eulerAngles.z;
		point = (Vector2)_transform.position + (Vector2)transform.TransformVector(offset);
	}
}
