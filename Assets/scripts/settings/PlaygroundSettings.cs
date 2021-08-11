using UnityEngine;

namespace settings
{
	public class PlaygroundSettings : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("terrain size in unity units/count of grass sprites will be x * y")]
		public Vector2 terrainSize;
	
		[SerializeField]
		public GameObject terrainBlockPrefab;
		
		[SerializeField]
		public float gridBlockHeight = 0.25f;

		[SerializeField]
		public float gridZIndex = 0;

		[SerializeField]
		public bool showGrid = true;
	}
}