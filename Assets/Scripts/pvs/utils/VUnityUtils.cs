using JetBrains.Annotations;
using UnityEngine;

namespace pvs.utils {
	public static class VUnityUtils {

		private static readonly ChildPredicate ALWAYS_TRUE_PREDICATE = child => true;

		public static void CleanChildren([NotNull] Transform transform) {
			CleanChildren(transform, ALWAYS_TRUE_PREDICATE);
		}

		public static void CleanChildren([NotNull] Transform transform, ChildPredicate childPredicate) {
			int childCount = transform.childCount;
			for (int i = childCount - 1; i >= 0; i--) {
				var child = transform.GetChild(i).gameObject;
				if (childPredicate.Invoke(child)) {
					Object.DestroyImmediate(child);
				}
			}
		}

		public delegate bool ChildPredicate(GameObject child);

	}
}