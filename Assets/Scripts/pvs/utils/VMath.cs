using System;
using UnityEngine;
namespace pvs.utils {
	public static class VMath {
		public static float Clamp(float value, float min, float max) {
			return Math.Max(Math.Min(value, max), min);
		}

		/**
		 * Находит ближайшее к value значение, кратное multipleOf
		 */
		public static float RoundTo(float value, float multipleOf) {
			return Mathf.Round(value / multipleOf) * multipleOf;
		}
	}
}