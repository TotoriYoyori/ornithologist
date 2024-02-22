using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeScrolling : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private ZoomOnClick zoomOnClick;
    [SerializeField] private float scrollSpeed = 1f;

    public int edgeScrollSize = 20;
    void Update()
    {
        if (zoomOnClick.IsZoomed())
        {
            Vector3 inputDir = new Vector3(0, 0, 0);

            if (Input.mousePosition.x < edgeScrollSize) inputDir.x = -1f * scrollSpeed;
            if (Input.mousePosition.y < edgeScrollSize) inputDir.y = -1f * scrollSpeed ;
            if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = +1f * scrollSpeed;
            if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.y = +1f * scrollSpeed;

            mainCamera.transform.position += inputDir * Time.deltaTime;
        }
        
    }
}
