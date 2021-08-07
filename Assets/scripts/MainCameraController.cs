using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCameraController : MonoBehaviour
{
    // private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("MainCameraController.gameObject.name=" + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        // inputCamera.transform.position = Vector2.Lerp(inputCamera.transform.position, targetCameraPosition, acceleration * Time.deltaTime);

        // camera.transform.position += new Vector3(eventData.delta.x, eventData.delta.y);
    }
}
