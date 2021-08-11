using attribute;
using UnityEngine;
namespace settings
{
	public class CameraSettings : MonoBehaviour
	{
		[SerializeField]
		[Range(0.01f, 10f)]
		public float cameraMoveSpeed = 1;
		[SerializeField]
		[Range(0.01f, 10f)]
		public float cameraZoomSpeed = 1;
		[SerializeField]
		public VRangeFloat cameraZoomConstraints = new VRangeFloat(0.4f, 2.1f);
		[SerializeField]
		public new bool enabled = true;
		[SerializeField]
		[TooltipAttribute("key to turn enabled/disabled script state")]
		public KeyCode stopKey = KeyCode.E;
	}
}