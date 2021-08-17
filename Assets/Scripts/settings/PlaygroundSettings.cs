using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace settings
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class PlaygroundSettings : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("terrain size in unity units/count of grass sprites will be x * y")]
		public Vector2 terrainSize;

		[SerializeField]
		public GameObject terrainTexturePrefab;

		[Space]
		[SerializeField]
		public float isometricGridHeight = 0.25f;

		[SerializeField]
		public bool buildingModeEnabled = true;

		[SerializeField]
		public Color isometricGridDefaultColor = new Color(1, 1, 1, 0.5f);

		[SerializeField]
		public Color isometricGridSelectedColor = new Color(1, 0, 0, 0.5f);

		[SerializeField]
		public bool showDebugGrid = true;

		public float isometricGridWidth => isometricGridHeight * 2;
	}
}