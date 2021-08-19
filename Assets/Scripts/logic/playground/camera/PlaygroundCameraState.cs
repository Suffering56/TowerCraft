using logic.playground.camera.settings;

namespace logic.playground.camera
{
	public class PlaygroundCameraState
	{
		// [Range(0.01f, 10f)]
		public float CameraMoveSpeed { get; } = PlaygroundCameraSettings.DefaultCameraMoveSpeed;

		// [Range(0.01f, 10f)]
		public float CameraZoomSpeed { get; } = PlaygroundCameraSettings.DefaultCameraZoomSpeed;
	}
}