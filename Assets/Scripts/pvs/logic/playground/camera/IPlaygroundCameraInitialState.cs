using pvs.attribute;

namespace pvs.logic.playground.camera {

	public interface IPlaygroundCameraInitialState {

		public VRangeFloat cameraZoomConstraints { get; }
	}
}