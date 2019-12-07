using UnityEngine;
using System.Collections;
using ForceField2D.PropertyAttributes;



public class ForceField2DUniversal : MonoBehaviour {

	protected Rigidbody2D _Body;
	protected Transform _transform;
	protected float timeSinceActivated = 0f;

	[HideInInspector] public bool activated = false;

	#region FilterSettings

	[Header("Filter Settings")]
	[Space(2)]
	[Tooltip("Layers that will be affected by the force field.")] // 
	public LayerMask layerFilter = -1;
	[Space(1)]

	[Tooltip("Tags that will be affected by the force field.")] // Tags that will be affected by the explosion.
	[TagDraw] public string[] tagFilter = new string[] {};

	[Space(3)]
	[Tooltip("Only include objects with a Z coordinate (depth) greater than or equal to this value.")]
	[BoolConditionalHide("useGamebjectDepth",false,true)] public float minDepth = -Mathf.Infinity;
	[Tooltip("Only include objects with a Z coordinate (depth) less than or equal to this value.")]
	[BoolConditionalHide("useGamebjectDepth",false,true)] public float maxDepth = Mathf.Infinity;
	[Space(1)]
	[Tooltip("Only include objects with a Z coordinate (depth) that are equal to this Gameobject Z coordinate (depth)")]
	public bool useGamebjectDepth = false;

	[Tooltip("Include triggers colliders")]
	public bool useTriggerColliders = true;
	[Tooltip("Include Solid colliders")]
	[BoolConditionWarrningBox("useTriggerColliders","Chose at least one type of collider!",false)]
	public bool useSolidColliders = true;

	#endregion

	[DrawRectWithColor(4f,0.192f, 0.772f, 0.956f,1f)]
	[Tooltip("Chosse when to activate the force field.")]
	[StringListPopup(new string[]{"Awake","Start","On Enable", "With Delay", "Never"})] public string activate = "Awake";
	[Tooltip("Delay time (In seconds).")]
	[StringConditionalHide("activate",true,false,"With Delay")] public float delayTime = 0f;
	[Tooltip("Loop forever")]
	public bool loop = true;
	[Tooltip("Duration of force field (In seconds).")]
	[BoolConditionalHide("loop",true,true)] public float duration = 10f;

	#region Universal Functions

	protected void AwakeUniversal () {
		_Body = GetComponent<Rigidbody2D> ();
		_transform = transform;
		if (string.Equals (activate, "Awake"))
			Activate ();
		else if (string.Equals (activate, "With Delay"))
			StartCoroutine (ActivateDelay());
	}

	protected void StartUniversal () {
		if (string.Equals (activate, "Start"))
			Activate ();
	}

	protected void OnEnableUniversal () {
		if (string.Equals (activate, "On Enable"))
			Activate ();
	}


	protected void UpdateUniversal () {
		if (activated)
			timeSinceActivated += Time.deltaTime;
	}

	#endregion

	#region Main

	/// <summary>
	/// Activates the force field.
	/// </summary>
	/// <param name="_delayTime">Delay time.</param>
	/// <param name="_duration">Duration.</param>
	public void Activate (float _delayTime = 0f, float _duration = 0f) {
		if (_delayTime != 0f) {
			activate = "With Delay";
			delayTime = _delayTime;
			StartCoroutine (ActivateDelay());
		}
		if (_duration != 0f) {
			loop = false;
			duration = _duration;
		}
		activated = true;
	}

	/// <summary>
	/// Deactivates the force field.
	/// </summary>
	public void Deactivate () {
		activated = false;
		timeSinceActivated = 0f;
	}
	private IEnumerator ActivateDelay () {
		yield return new WaitForSeconds (delayTime);
		Activate ();
	}

	#endregion

	#region Check

	protected bool CheckCollidedRigidbody2D (Rigidbody2D body) {
		if (body != null && !body.isKinematic) {
			if (tagFilter.Length != 0) {
				for (int i = 0; i < tagFilter.Length; i++) {
					if (body.gameObject.tag == tagFilter [i])
						return true;
				}
				return false;
			} else {
				return true;
			}
		}
		return false;
	}

	protected bool CheckCollider (Collider2D coll) {
		if ((coll.isTrigger && useTriggerColliders) || (!coll.isTrigger && useSolidColliders))
			return true;
		return false;
	}

	#endregion
}


