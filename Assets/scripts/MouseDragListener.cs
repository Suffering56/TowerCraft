using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDragListener : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerClickHandler, IPointerMoveHandler, IPointerExitHandler, IPointerEnterHandler, IPointerUpHandler
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("MouseDragListener.Start()");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag by: " + gameObject.name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown by: " + gameObject.name);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick by: " + gameObject.name);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Debug.Log("OnPointerMove by: " + gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit by: " + gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter by: " + gameObject.name);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp by: " + gameObject.name);
    }
}