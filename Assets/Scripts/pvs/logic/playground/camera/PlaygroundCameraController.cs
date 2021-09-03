using System;
using pvs.attribute;
using UnityEditor;
using UnityEngine;
using Zenject;
namespace pvs.logic.playground.camera {

	/**
	* Allows player to move camera as in strategy games like "WarCraft 3"
	*/
	public class PlaygroundCameraController : MonoBehaviour {

		private new Camera camera;

		[Inject] private IPlaygroundCameraState dynamicState;
		[Inject] private IPlaygroundCameraInitialState initialState;
		[Inject] private IPlaygroundInitialState playgroundInitialState;

		private VRangeFloat cameraZoomConstraints;
		private Vector2 lastScreenSize;

		private const float CAMERA_SPEED_COEFFICIENT = 0.01f;
		private const float CAMERA_ZOOM_COEFFICIENT = 0.1f;
		private const float MOUSE_EDGE_OFFSET = 5f;

		private Rect terrainSize => playgroundInitialState.terrainRect;

		private void Start() {
			camera = gameObject.GetComponent<Camera>();
			cameraZoomConstraints = initialState.cameraZoomConstraints;

			CorrectCameraPosition(GetScreenSize());
		}

		private void LateUpdate() {
			var actualScreenSize = GetScreenSize();
			var screenSizeChanged = actualScreenSize != lastScreenSize;

			if (screenSizeChanged || TryMoveCamera() | TryZoomCamera()) {
				CorrectCameraPosition(actualScreenSize);
			}
		}

		// Важно вызывать после HandleScreenSizeChanges
		private bool TryMoveCamera() {
			int x = 0;
			int y = 0;

			if (Input.mousePosition.x <= MOUSE_EDGE_OFFSET) {
				x = -1;
			} else if (Input.mousePosition.x >= lastScreenSize.x - MOUSE_EDGE_OFFSET) {
				x = 1;
			}

			if (Input.mousePosition.y <= MOUSE_EDGE_OFFSET) {
				y = -1;
			} else if (Input.mousePosition.y >= lastScreenSize.y - MOUSE_EDGE_OFFSET) {
				y = 1;
			}

			if (x != 0 || y != 0) {
				Vector3 direction = new Vector3(x, y);
				camera.transform.position += direction * (CAMERA_SPEED_COEFFICIENT * dynamicState.cameraMoveSpeed);
				return true;
			}

			return false;
		}

		private bool TryZoomCamera() {
			float delta = Input.mouseScrollDelta.y;
			if (delta == 0) return false;

			var newCameraSize = camera.orthographicSize - delta * (CAMERA_ZOOM_COEFFICIENT * dynamicState.cameraZoomSpeed);
			camera.orthographicSize = cameraZoomConstraints.Clamp(newCameraSize);

			return true;
		}

		private void CorrectCameraPosition(Vector2 actualScreenSize) {
			if (actualScreenSize != lastScreenSize) {
				ClampMaxCameraZoom();
				lastScreenSize = actualScreenSize;
			}

			//
			VRectConstraints cameraConstraints = CalculateCameraConstraints(actualScreenSize);

			Vector2 cameraPivotOffset = CalculateCameraViewSize(camera) / 2f;
			var cameraPivotConstraints = new VRectConstraints(cameraConstraints.min + cameraPivotOffset, cameraConstraints.max - cameraPivotOffset);

			var cameraPosition = camera.transform.position;
			// может не отличаться от оригинальной позиции
			var correctCameraPosition = cameraPivotConstraints.Clamp(cameraPosition);

			camera.transform.position = new Vector3(correctCameraPosition.x, correctCameraPosition.y, cameraPosition.z);
		}

		private void ClampMaxCameraZoom() {
			var originOrthographicSize = camera.orthographicSize;
			camera.orthographicSize = cameraZoomConstraints.max;

			Vector2 playgroundSize = playgroundInitialState.terrainRect.size;
			Vector2 maxCameraViewSize = CalculateCameraViewSize(camera);

			Vector2 ratio = maxCameraViewSize / playgroundSize;
			float maxRatio = Math.Max(ratio.x, ratio.y);

			if (maxRatio > 1) {
				float maxCameraZoom = cameraZoomConstraints.max / maxRatio;
				cameraZoomConstraints.max = maxCameraZoom;
			}

			camera.orthographicSize = cameraZoomConstraints.Clamp(originOrthographicSize);
		}

		private VRectConstraints CalculateCameraConstraints(Vector2 actualScreenSize) {
			var bottomUiPanelScreenHeight = actualScreenSize.y * playgroundInitialState.bottomUIPanelRelativeHeight;

			var minPoint = camera.ScreenToWorldPoint(new Vector3(0, actualScreenSize.y, 0));
			var maxPoint = camera.ScreenToWorldPoint(new Vector3(actualScreenSize.x, actualScreenSize.y + bottomUiPanelScreenHeight, 0));

			var bottomUIPanelSize = maxPoint - minPoint;

			return new VRectConstraints(
				terrainSize.position.x,
				terrainSize.position.y - terrainSize.height - bottomUIPanelSize.y,
				terrainSize.position.x + terrainSize.width,
				terrainSize.position.y
			);
		}

		private static Vector2 CalculateCameraViewSize(Camera camera) {
			float cameraViewHeight = camera.orthographicSize * 2.0f;
			float cameraViewWidth = cameraViewHeight * camera.aspect;
			return new Vector2(cameraViewWidth, cameraViewHeight);
		}

		private static Vector2 GetScreenSize() {

			#if UNITY_EDITOR
			return Handles.GetMainGameViewSize();
			#else
				return new Vector2(Screen.width, Screen.height);
			#endif
		}
	}
}