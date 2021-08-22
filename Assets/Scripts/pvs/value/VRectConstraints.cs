using System;
using pvs.utils;
using UnityEngine;
namespace pvs.value
{
	[Serializable]
	public class VRectConstraints : PropertyAttribute
	{
		public Vector2 min;
		public Vector2 max;

		public VRectConstraints(Vector2 min, Vector2 max)
		{

			this.min = min;
			this.max = max;
		}

		public VRectConstraints(float minX, float minY, float maxX, float maxY)
		{
			this.min = new Vector2(minX, minY);
			this.max = new Vector2(maxX, maxY);
		}

		public Vector3 Clamp(Vector3 value)
		{
			return Clamp((Vector2)value);
		}

		public Vector2 Clamp(Vector2 value)
		{
			float x = VMath.Clamp(value.x, min.x, max.x);
			float y = VMath.Clamp(value.y, min.y, max.y);
			return new Vector2(x, y);
		}
	}
}