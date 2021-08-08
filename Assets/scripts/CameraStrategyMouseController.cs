using System;
using attribute;
using UnityEditor;
using UnityEngine;

/**
 * Allows player to move camera as in strategy games like "WarCraft 3"
 */
public class CameraStrategyMouseController : MonoBehaviour
{
	[SerializeField]
	[Range(0.01f, 10f)]
	private float cameraMoveSpeed = 1;
	[SerializeField]
	[Range(0.01f, 10f)]
	private float cameraZoomSpeed = 1;
	[SerializeField]
	public VRange cameraZoomConstraints = new VRange(0.3f, 1.7f);
	[SerializeField]
	private new bool enabled = true;
	[SerializeField]
	[TooltipAttribute("key to turn enabled/disabled script state")]
	private KeyCode switchKey = KeyCode.E;

	private new Camera camera;
	private Vector2 screenSize;

	private const float CAMERA_SPEED_COEFFICIENT = 0.01f;
	private const float CAMERA_ZOOM_COEFFICIENT = 0.1f;

	private void Start()
	{
		camera = gameObject.GetComponent<Camera>();

		#if UNITY_EDITOR
		screenSize = Handles.GetMainGameViewSize();
		#else
            screenSize = new Vector2(Screen.width, Screen.height);
		#endif
	}

	private void LateUpdate()
	{
		TrySwitchCameraState();

		if (!enabled) return;

		TryMoveCamera();
		TryZoomCamera();
		CorrectCameraPosition();
	}

	private void TryMoveCamera()
	{

		int x = 0;
		int y = 0;

		if (Input.mousePosition.x <= -1 || Input.mousePosition.x >= screenSize.x - 1)
		{
			x = Math.Sign(Input.mousePosition.x);
		}

		if (Input.mousePosition.y <= -1 || Input.mousePosition.y >= screenSize.y - 1)
		{
			y = Math.Sign(Input.mousePosition.y);
		}

		if (x != 0 || y != 0)
		{
			Vector3 direction = new Vector3(x, y);
			camera.transform.position += direction * (CAMERA_SPEED_COEFFICIENT * cameraMoveSpeed);
		}
	}

	private void TryZoomCamera()
	{
		float delta = Input.mouseScrollDelta.y;
		if (delta == 0) return;

		var newCameraSize = camera.orthographicSize - delta * (CAMERA_ZOOM_COEFFICIENT * cameraZoomSpeed);
		camera.orthographicSize = Clamp(newCameraSize, cameraZoomConstraints);
	}

	private void CorrectCameraPosition()
	{
	}

	private void TrySwitchCameraState()
	{

		if (Input.GetKeyUp(switchKey))
		{
			enabled = !enabled;
		}
	}

	private static float Clamp(float value, VRange constraints)
	{
		return Math.Max(Math.Min(value, constraints.max), constraints.min);
	}
}