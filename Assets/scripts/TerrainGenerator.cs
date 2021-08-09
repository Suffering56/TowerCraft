using UnityEngine;
using UnityEngine.Serialization;

public class TerrainGenerator : MonoBehaviour
{

	[FormerlySerializedAs("size")]
	[TooltipAttribute("terrain size in unity units/count of grass sprites will be x * y")]
	[SerializeField]
	private Vector2 blocksSize;

	[SerializeField]
	private GameObject terrainBlockPrefab;

	[SerializeField]
	private bool initialized;

	private void OnDrawGizmos()
	{
		if (!initialized)
		{
			DrawTerrain();
			initialized = true;
		}
	}

	private void DrawTerrain()
	{
		Reset();

		float xOffset = -blocksSize.x / 2;
		float yOffset = blocksSize.y / 2;

		for (int y = 0; y < blocksSize.y; y++)
		{
			for (int x = 0; x < blocksSize.x; x++)
			{
				var blockInstance = Instantiate(terrainBlockPrefab, gameObject.transform, true);
				blockInstance.name = $"{terrainBlockPrefab.name}[{x},{y}]";
				blockInstance.transform.position = new Vector3(x + xOffset, yOffset - y, 0);
			}
		}
	}

	private void Reset()
	{
		int childCount = transform.childCount;
		for (int i = childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(transform.GetChild(i).gameObject);
		}
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}