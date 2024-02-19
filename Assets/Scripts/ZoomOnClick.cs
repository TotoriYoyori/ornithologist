using UnityEngine;

public class ZoomOnClick : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float zoomFactor = 2f; // Factor by which to zoom in

    private bool isZoomed = false; // Flag to track whether the camera is currently zoomed in

    public bool IsZoomed() // Public method to check if the camera is zoomed in
    {
        return isZoomed;
    }

    private void Update()
    {
        // Check for right mouse button press
        if (Input.GetMouseButtonDown(1)) // 1 corresponds to the right mouse button
        {
            if (!isZoomed)
            {
                // Get the position of the cursor in screen coordinates
                Vector3 cursorPosition = Input.mousePosition;

                // Convert screen coordinates to world coordinates at the depth of the camera
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(cursorPosition.x, cursorPosition.y, mainCamera.nearClipPlane));

                // Zoom in on the area around the cursor position
                mainCamera.transform.position = new Vector3(worldPosition.x, worldPosition.y, mainCamera.transform.position.z);
                mainCamera.orthographicSize /= zoomFactor; // Zoom in by reducing the orthographic size

                isZoomed = true;
            }
            else
            {
                // Reset the camera to its original state (normal zoom level)
                mainCamera.transform.position = new Vector3(0f, 0f, mainCamera.transform.position.z);
                mainCamera.orthographicSize *= zoomFactor; // Reset orthographic size to original value

                isZoomed = false;
            }
        }
    }
}


