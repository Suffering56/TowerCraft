using pvs.attribute;
using pvs.settings.debug;
using pvs.utils.code;
namespace pvs.logic.playground.camera {

	[ZenjectComponent]
	public class PlaygroundCameraState : IPlaygroundCameraState, IPlaygroundCameraInitialState {

		private readonly DebugSettings debugSettings;

		public float cameraMoveSpeed { get; }
		public float cameraZoomSpeed { get; }
		public VRangeFloat cameraZoomConstraints { get; }

		public PlaygroundCameraState(DebugSettings debugSettings) {
			this.debugSettings = debugSettings;
			cameraMoveSpeed = debugSettings.defaultCameraMoveSpeed;
			cameraZoomSpeed = debugSettings.defaultCameraZoomSpeed;
			
			cameraZoomConstraints = debugSettings.cameraZoomConstraints;
		}
	}
}