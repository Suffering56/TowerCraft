using System.Diagnostics.CodeAnalysis;
using pvs.attribute;
using pvs.logic.playground.camera;
using pvs.logic.playground.state;
using UnityEngine;
namespace pvs.settings.debug {

	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class DebugSettings : MonoBehaviour, IPlaygroundInitialState, IPlaygroundCameraInitialState {

		[SerializeField]
		[Tooltip("terrain size in unity units/count of grass sprites will be x * y")]
		public Vector2 _terrainSize;
		public Vector2 terrainSize => _terrainSize;

		[SerializeField]
		public GameObject _terrainElementPrefab;
		public GameObject terrainElementPrefab => _terrainElementPrefab;

		[Space]
		[SerializeField]
		public float _isometricGridHeight = 0.25f;
		public float isometricGridHeight => _isometricGridHeight;
		public float isometricGridWidth => isometricGridHeight * 2;

		[SerializeField]
		public bool _buildingModeEnabled = true;
		public bool buildingModeEnabled => _buildingModeEnabled;

		[SerializeField]
		public Color _isometricGridDefaultColor = new Color(1, 1, 1, 0.5f);
		public Color isometricGridDefaultColor => _isometricGridDefaultColor;

		[SerializeField]
		public Color _isometricGridSelectedColor = new Color(1, 0, 0, 0.5f);
		public Color isometricGridSelectedColor => _isometricGridSelectedColor;

		[SerializeField]
		public bool _showDebugGrid = true;
		public bool showDebugGrid => _showDebugGrid;

		[Range(0.01f, 10f)]
		[SerializeField]
		public float _cameraMoveSpeed = 1;
		public float defaultCameraMoveSpeed => _cameraMoveSpeed;

		[Range(0.01f, 10f)]
		[SerializeField]
		public float _cameraZoomSpeed = 1;
		public float defaultCameraZoomSpeed => _cameraZoomSpeed;

		[SerializeField]
		public VRangeFloat _cameraZoomConstraints = new VRangeFloat(0.4f, 2.1f);
		public VRangeFloat cameraZoomConstraints => _cameraZoomConstraints;

		[SerializeField]
		public KeyCode _stopKey = KeyCode.E;

		public KeyCode stopKey => _stopKey;
	}
}