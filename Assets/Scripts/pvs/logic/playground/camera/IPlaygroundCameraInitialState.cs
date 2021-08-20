using pvs.attribute;
using UnityEngine;

namespace pvs.logic.playground.camera {

	public interface IPlaygroundCameraInitialState {

		public VRangeFloat cameraZoomConstraints { get; }

		public KeyCode stopKey { get; }
	}
}