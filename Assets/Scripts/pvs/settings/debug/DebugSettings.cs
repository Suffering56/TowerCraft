using System.Diagnostics.CodeAnalysis;
using pvs.attribute;
using pvs.logic.playground.state;
using UnityEngine;

namespace pvs.settings.debug {

	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class DebugSettings : MonoBehaviour, IPlaygroundInitialState {

		[SerializeField]
		[Tooltip("terrain size in unity units/count of grass sprites will be x * y")]
		private Vector2 _terrainSize;
		public Vector2 terrainSize => _terrainSize;

		[SerializeField]
		private GameObject _terrainElementPrefab;
		public GameObject terrainElementPrefab => _terrainElementPrefab;

		[SpaceAttribute(20)]
		[SerializeField]
		private float _isometricGridHeight = 0.25f;
		public float isometricGridHeight => _isometricGridHeight;
		public float isometricGridWidth => isometricGridHeight * 2;

		[SerializeField]
		private bool _buildingModeEnabled = true;
		public bool buildingModeEnabled => _buildingModeEnabled;

		[SerializeField]
		private bool _showDebugGrid = true;
		public bool showDebugGrid => _showDebugGrid;

		[SerializeField]
		private Color _isometricGridDefaultColor = new Color(1, 1, 1, 0.5f);
		public Color isometricGridDefaultColor => _isometricGridDefaultColor;

		[SerializeField]
		private Color _isometricGridSelectedColor = new Color(1, 0, 0, 0.5f);
		public Color isometricGridSelectedColor => _isometricGridSelectedColor;

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

		[SerializeField]
		private KeyCode _cameraStopKey = KeyCode.E;

		public KeyCode cameraStopKey => _cameraStopKey;

		private void OnValidate() {
			GetComponent<DebugSettingsManager>().triggerRefresh = true;
		}
	}
}