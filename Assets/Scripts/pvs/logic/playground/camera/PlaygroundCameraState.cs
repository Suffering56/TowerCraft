using pvs.attribute;
using pvs.settings.debug;
using UnityEngine;
namespace pvs.logic.playground.camera {

	public class PlaygroundCameraState : IPlaygroundCameraState, IPlaygroundCameraInitialState {

		public float cameraMoveSpeed { get; }
		public float cameraZoomSpeed { get; }
		public VRangeFloat cameraZoomConstraints { get; }
		public KeyCode cameraStopKey { get; }

		public PlaygroundCameraState(DebugSettings debugSettings) {
			cameraMoveSpeed = debugSettings.defaultCameraMoveSpeed;
			cameraZoomSpeed = debugSettings.defaultCameraZoomSpeed;
			
			cameraZoomConstraints = debugSettings.cameraZoomConstraints;
			cameraStopKey = debugSettings.cameraStopKey;
		}
	}
}