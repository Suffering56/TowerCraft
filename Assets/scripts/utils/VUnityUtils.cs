using UnityEngine;
namespace utils
{
	public static class VUnityUtils
	{
		public static void CleanChildren(Transform transform)
		{
			int childCount = transform.childCount;
			for (int i = childCount - 1; i >= 0; i--)
			{
				Object.DestroyImmediate(transform.GetChild(i).gameObject);
			}
		}
	}
}