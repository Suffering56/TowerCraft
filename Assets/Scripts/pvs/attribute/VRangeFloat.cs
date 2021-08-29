using System;
using pvs.utils;
using UnityEngine;
namespace pvs.attribute {
	
	[Serializable]
	public class VRangeFloat : PropertyAttribute {
		public float min;
		public float max;

		public VRangeFloat(float min, float max) {
			this.min = min;
			this.max = max;
		}

		public float Clamp(float value) {
			return VMath.Clamp(value, min, max);
		}
	}
}