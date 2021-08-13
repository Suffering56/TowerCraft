using input;
using settings;
using UnityEngine;
namespace logic
{
	public class IsometricGridElementState : MonoBehaviour, IMouseListener
	{
		[SerializeField]
		public int xName;

		[SerializeField]
		public int yName;

		public PlaygroundSettings playgroundSettings;

		private bool selected = false;

		private void Start()
		{
			var spriteRenderer = GetSpriteRenderer();
			spriteRenderer.color = playgroundSettings.isometricGridBlockDefaultColor;
		}

		private SpriteRenderer GetSpriteRenderer()
		{
			return GetComponentInChildren<SpriteRenderer>();
		}

		public void OnMouseClick(Vector2 mouseWorldPoint)
		{
			selected = !selected;

			GetSpriteRenderer().color = selected
			? playgroundSettings.isometricGridBlockSelectedColor
			: playgroundSettings.isometricGridBlockDefaultColor;

			Debug.Log($"OnMouseClick: {name}");
		}
	}
}