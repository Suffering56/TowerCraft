using UnityEngine;
using UnityEngine.EventSystems;

namespace input
{
	public class MouseEventsHandler : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerClickHandler, IPointerMoveHandler, IPointerExitHandler, IPointerEnterHandler, IPointerUpHandler
	{

		public void OnPointerClick(PointerEventData eventData)
		{
			if (!Camera.main) return;
			var clickedPoint = Camera.main.ScreenToWorldPoint(eventData.position);

			var mouseListeners = gameObject.GetComponents<IMouseListener>();
			foreach (var mouseListener in mouseListeners)
			{
				mouseListener.OnMouseClick(clickedPoint);
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			// Debug.Log("OnDrag by: " + gameObject.name);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			// Debug.Log("OnPointerDown by: " + gameObject.name);
		}

		// hit.collider будет ближайшим к камере объектом по transform.position.z
		private GameObject GetObjectByZIndex(PointerEventData eventData)
		{
			if (!Camera.main) return null;

			var clickedPoint = Camera.main.ScreenToWorldPoint(eventData.position);
			RaycastHit2D hit = Physics2D.Raycast(clickedPoint, Vector3.forward, 5);
			// Debug.DrawRay(clickedPoint, Vector3.forward * 3, Color.red, 100);

			if (hit.collider != null)
			{
				Debug.Log($"hit[]: {hit.collider.gameObject.name}");
				return hit.collider.gameObject;
			}
			return null;
		}

		public void OnPointerMove(PointerEventData eventData)
		{
			// Debug.Log("OnPointerMove by: " + gameObject.name);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			// Debug.Log("OnPointerExit by: " + gameObject.name);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			// Debug.Log("OnPointerEnter by: " + gameObject.name);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			// Debug.Log("OnPointerUp by: " + gameObject.name);
		}
	}
}