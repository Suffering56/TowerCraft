using UnityEngine;
namespace input
{
	public interface IMouseListener
	{
		void OnMouseClick(Vector2 mouseWorldPoint);
	}
}