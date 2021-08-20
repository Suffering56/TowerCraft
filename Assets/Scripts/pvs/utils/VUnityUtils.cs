using JetBrains.Annotations;
using UnityEngine;
namespace pvs.utils
{
	public static class VUnityUtils
	{
		public static void CleanChildren([NotNull] Transform transform)
		{
			int childCount = transform.childCount;
			for (int i = childCount - 1; i >= 0; i--)
			{
				Object.DestroyImmediate(transform.GetChild(i).gameObject);
			}
		}
	}
}