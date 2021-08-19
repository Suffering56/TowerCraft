using attribute;
using UnityEngine;
namespace logic.playground.camera.settings
{
	public static class PlaygroundCameraSettings
	{
		// [Range(0.01f, 10f)]
		public static float DefaultCameraMoveSpeed { get; } = 1;

		// [Range(0.01f, 10f)]
		public static float DefaultCameraZoomSpeed { get; } = 1;

		public static VRangeFloat CameraZoomConstraints { get; } = new VRangeFloat(0.4f, 2.1f);

		public static KeyCode StopKey { get; } = KeyCode.E;
	}
}