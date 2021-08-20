using System;
namespace pvs.utils
{
	public static class VMath
	{
		public static float Clamp(float value, float min, float max)
		{
			return Math.Max(Math.Min(value, max), min);
		}
	}
}