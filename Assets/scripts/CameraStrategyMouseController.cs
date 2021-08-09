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
	private VRangeFloat cameraZoomConstraints = new VRangeFloat(0.4f, 2.1f);
	[SerializeField]
	private new bool enabled = true;
	[SerializeField]
	[TooltipAttribute("key to turn enabled/disabled script state")]
	private KeyCode switchKey = KeyCode.E;
	[SerializeField]
	private VRectConstraints playgroundConstraints = new VRectConstraints(-5, -5, 5, 5);

	private new Camera camera;
	private Vector2 lastScreenSize;

	private const float CAMERA_SPEED_COEFFICIENT = 0.01f;
	private const float CAMERA_ZOOM_COEFFICIENT = 0.1f;
	private const float MOUSE_EDGE_OFFSET = 5f;

	private void Start()
	{
		camera = gameObject.GetComponent<Camera>();
		HandleScreenSizeChanges();
	}

	private void LateUpdate()
	{
		bool screenSizeChanged = HandleScreenSizeChanges();
		TrySwitchCameraState();

		if (!enabled) return;

		if (screenSizeChanged | TryMoveCamera() | TryZoomCamera())
		{
			CorrectCameraPosition();
		}
	}

	// Важно вызывать после HandleScreenSizeChanges
	private bool TryMoveCamera()
	{
		int x = 0;
		int y = 0;

		if (Input.mousePosition.x <= MOUSE_EDGE_OFFSET)
		{
			x = -1;
		}
		else if (Input.mousePosition.x >= lastScreenSize.x - MOUSE_EDGE_OFFSET)
		{
			x = 1;
		}

		if (Input.mousePosition.y <= MOUSE_EDGE_OFFSET)
		{
			y = -1;
		}
		else if (Input.mousePosition.y >= lastScreenSize.y - MOUSE_EDGE_OFFSET)
		{
			y = 1;
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
	private void TrySwitchCameraState()
	{

		if (Input.GetKeyUp(switchKey))
		{
			enabled = !enabled;
		}
	}

	private bool HandleScreenSizeChanges()
	{
		var actualScreenSize = GetScreenSize();
		if (actualScreenSize != lastScreenSize)
		{
			ClampMaxCameraZoom();
			lastScreenSize = actualScreenSize;
			return true;
		}
		return false;
	}
	
	private void ClampMaxCameraZoom()
	{
		var originOrthographicSize = camera.orthographicSize;
		camera.orthographicSize = cameraZoomConstraints.max;

		Vector2 playgroundSize = playgroundConstraints.max - playgroundConstraints.min;
		Vector2 maxCameraViewSize = CalculateCameraViewSize(camera);

		Vector2 ratio = maxCameraViewSize / playgroundSize;
		float maxRatio = Math.Max(ratio.x, ratio.y);

		if (maxRatio > 1)
		{
			float maxCameraZoom = cameraZoomConstraints.max / maxRatio;
			cameraZoomConstraints.max = maxCameraZoom;
		}

		camera.orthographicSize = originOrthographicSize;
	}

	private static VRectConstraints CalculateCameraPositionConstraints(Camera camera, VRectConstraints visibleSceneConstraints)
	{
		// На такое расстояние нужно сдвинуть камеру относительно любого края playground-а, чтобы не выйти за его пределы.
		Vector2 cameraOffset = CalculateCameraViewSize(camera) / 2f;

		return new VRectConstraints(
			visibleSceneConstraints.min + cameraOffset,
			visibleSceneConstraints.max - cameraOffset
		);
	}

	private static Vector2 CalculateCameraViewSize(Camera camera)
	{
		float cameraViewHeight = camera.orthographicSize * 2.0f;
		float cameraViewWidth = cameraViewHeight * camera.aspect;
		return new Vector2(cameraViewWidth, cameraViewHeight);
	}

	private static Vector2 GetScreenSize()
	{

		#if UNITY_EDITOR
			return Handles.GetMainGameViewSize();
		#else
            return new Vector2(Screen.width, Screen.height);
		#endif
	}
}