using System;
using UnityEngine;
namespace attribute
{
	[Serializable]
	public class VRange : PropertyAttribute
	{
		public float min;
		public float max;

		public VRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}
	}
}