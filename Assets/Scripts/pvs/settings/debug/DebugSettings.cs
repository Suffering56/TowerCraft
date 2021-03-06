using System;
using System.Diagnostics.CodeAnalysis;
using pvs.attribute;
using pvs.logic.playground;
using pvs.logic.playground.building;
using pvs.utils.code;
using UnityEngine;

namespace pvs.settings.debug {

	[SuppressMessage("ReSharper", "InconsistentNaming")]
	[ZenjectComponent]
	public class DebugSettings : MonoBehaviour, IPlaygroundInitialState {

		[SerializeField] private GameObject _terrainElementPrefab;
		public GameObject terrainElementPrefab => _terrainElementPrefab;

		[Tooltip("terrain size in unity units/count of grass sprites will be x * y")]
		[SerializeField] private Vector2 _terrainSize;
		public Rect terrainRect => new Rect(-_terrainSize.x / 2, _terrainSize.y / 2, _terrainSize.x, _terrainSize.y);

		[SpaceAttribute(20)]
		[SerializeField] private float _isometricGridHeight = 0.25f;
		public Vector2 isometricElementSize => new Vector2(_isometricGridHeight * 2, _isometricGridHeight);

		[SpaceAttribute(20)]
		[SerializeField] private RectTransform _mainUICanvas;
		[SerializeField] private RectTransform _bottomMainUIPanel;
		public float bottomUIPanelRelativeHeight => _bottomMainUIPanel.rect.height / _mainUICanvas.rect.height;

		[SerializeField]
		private bool _buildingModeEnabled = true;
		public bool buildingModeEnabled => _buildingModeEnabled;

		[SerializeField]
		private bool _showDebugGrid = true;
		public bool showDebugGrid => _showDebugGrid;

		[SerializeField]
		private Color _isometricGridDefaultColor = new Color(1, 1, 1, 0.2f);
		[SerializeField]
		private Color _isometricGridAvailableColor = new Color(0, 1, 0, 0.2f);
		[SerializeField]
		private Color _isometricGridUnavailableColor = new Color(1, 0, 0, 0.2f);

		[SpaceAttribute(20)]
		[Range(0.01f, 10f)]
		[SerializeField]
		private float _cameraMoveSpeed = 1;
		public float defaultCameraMoveSpeed => _cameraMoveSpeed;

		[Range(0.01f, 10f)]
		[SerializeField]
		private float _cameraZoomSpeed = 1;
		public float defaultCameraZoomSpeed => _cameraZoomSpeed;

		[SerializeField]
		private VRangeFloat _cameraZoomConstraints = new VRangeFloat(0.4f, 2.1f);
		public VRangeFloat cameraZoomConstraints => _cameraZoomConstraints;

		public Color GetIsometricGridColor(GridPointStatus status) {
			return status switch {
				GridPointStatus.NONE => _isometricGridDefaultColor,
				GridPointStatus.AVAILABLE_FOR_BUILD => _isometricGridAvailableColor,
				GridPointStatus.UNAVAILABLE_FOR_BUILD => _isometricGridUnavailableColor,
				_ => throw new Exception($"Unsupported {nameof(GridPointStatus)} value: {status}")
			};
		}

		private void OnValidate() {
			GetComponent<DebugSettingsManager>().triggerRefresh = true;
		}
	}
}