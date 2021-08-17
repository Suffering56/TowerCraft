using System;
using UnityEngine;

namespace attribute
{
	[Serializable]
	public class VRangeFloat : PropertyAttribute
	{
		public float min;
		public float max;

		public VRangeFloat(float min, float max)
		{
			this.min = min;
			this.max = max;
		}
	}
}