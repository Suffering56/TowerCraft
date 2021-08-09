using System;
using attribute;
using UnityEditor;
using UnityEngine;
using utils;

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
	public VRangeFloat cameraZoomConstraints = new VRangeFloat(0.4f, 2.1f);
	[SerializeField]
	private new bool enabled = true;
	[SerializeField]
	[TooltipAttribute("key to turn enabled/disabled script state")]
	private KeyCode switchKey = KeyCode.E;
	[SerializeField]
	private VRectConstraints playgroundConstraints = new VRectConstraints(-5, -5, 5, 5);

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

		if (TryMoveCamera() || TryZoomCamera())
		{
			CorrectCameraPosition();
		}
	}

	private bool TryMoveCamera()
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
			return true;
		}

		return false;
	}

	private bool TryZoomCamera()
	{
		float delta = Input.mouseScrollDelta.y;
		if (delta == 0) return false;

		var newCameraSize = camera.orthographicSize - delta * (CAMERA_ZOOM_COEFFICIENT * cameraZoomSpeed);
		camera.orthographicSize = VMath.Clamp(newCameraSize, cameraZoomConstraints.min, cameraZoomConstraints.max);

		return true;
	}

	private void CorrectCameraPosition()
	{
		VRectConstraints cameraCenterConstraints = CalculateCameraPositionConstraints(camera, playgroundConstraints);

		var cameraPosition = camera.transform.position;
		// может не отличаться от оригинальной позиции
		var correctCameraPosition = cameraCenterConstraints.Clamp(cameraPosition);

		camera.transform.position = new Vector3(correctCameraPosition.x, correctCameraPosition.y, cameraPosition.z);
	}

	private static VRectConstraints CalculateCameraPositionConstraints(Camera camera, VRectConstraints visibleSceneConstraints)
	{
		/*
		 * На такое расстояние нужно сдвинуть камеру относительно любого края playground-а, чтобы не выйти за его пределы.
		 * Для лучшего понимания откуда взялись эти формулы оставляю этот код:
		 * 
		 * float cameraViewHeight = camera.orthographicSize * 2.0f;
		 * float cameraViewWidth = cameraViewHeight * camera.aspect;
		 */
		Vector2 cameraOffset = new Vector2(camera.orthographicSize * camera.aspect, camera.orthographicSize);

		return new VRectConstraints(
			visibleSceneConstraints.min + cameraOffset,
			visibleSceneConstraints.max - cameraOffset
		);
	}

	private void TrySwitchCameraState()
	{

		if (Input.GetKeyUp(switchKey))
		{
			enabled = !enabled;
		}
	}
}