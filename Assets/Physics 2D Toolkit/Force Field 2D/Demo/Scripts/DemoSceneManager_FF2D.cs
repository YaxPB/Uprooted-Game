using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DemoSceneManager_FF2D : MonoBehaviour {

	[SerializeField] private GameObject forceFieldObject = null;
	[Space(4)]
	[SerializeField] private Sprite circleSprite = null;
	[SerializeField] private Sprite rectSprite = null;
	[SerializeField] private Sprite capsuleSprite = null;
	[Space(4)]
	[SerializeField] private Toggle followMouseToggle = null;


	private Transform transformRef;
	private SpriteRenderer firceFieldRenderer;

	private int currentMode = 1;
	private Vector3 mousePosition = default(Vector3);

	void Awake () {
		transformRef = forceFieldObject.transform;
		firceFieldRenderer = forceFieldObject.GetComponent<SpriteRenderer> ();
	}

	void Start () {
		#if UNITY_EDITOR
		Selection.activeGameObject = forceFieldObject;
		#endif
	}

	void Update () {
		if(followMouseToggle.isOn)
			MovementHandler ();
		
		if (Input.GetKeyDown (KeyCode.Alpha1))
			AddNewForceField (1);
		else if (Input.GetKeyDown (KeyCode.Alpha2))
			AddNewForceField (2);
		else if (Input.GetKeyDown (KeyCode.Alpha3))
			AddNewForceField (3);
	}

	private void AddNewForceField (int _newMode) {
		if (currentMode == _newMode)
			return;
		if (currentMode == 1)
			Destroy(forceFieldObject.GetComponent<CircleForceField2D> ());
		else if (currentMode == 2)
			Destroy(forceFieldObject.GetComponent<RectForceField2D> ());
		else if (currentMode == 3)
			Destroy(forceFieldObject.GetComponent<CapsuleForceField2D> ());

		if (_newMode == 1) {
			forceFieldObject.AddComponent (typeof(CircleForceField2D));
			firceFieldRenderer.sprite = circleSprite;
		} else if (_newMode == 2) {
			forceFieldObject.AddComponent (typeof(RectForceField2D));
			firceFieldRenderer.sprite = rectSprite;
		} else if (_newMode == 3) {
			forceFieldObject.AddComponent (typeof(CapsuleForceField2D));
			firceFieldRenderer.sprite = capsuleSprite;
		}

		currentMode = _newMode;

		#if UNITY_EDITOR
		Selection.activeGameObject = forceFieldObject;
		#endif

	}

	private void MovementHandler () {
		mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePosition.z = 0f;
		transformRef.position = mousePosition;
	}

}
