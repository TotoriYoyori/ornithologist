using System.Collections;
using UnityEngine;

public class ZoomOnClick : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float smoothSpeed = 0.5f; // Speed for smooth zooming

    private bool isZoomed = false; // Flag to track whether the camera is currently zoomed in
    private bool isZooming = false; // Flag to track whether the camera is currently in the process of zooming
    private Vector3[] zoomTargets; // Array to store zoom in and zoom out targets
    private float zoomInSize = 4f; // Orthographic size for zooming in
    private float zoomOutSize = 25f; // Orthographic size for zooming out

    private void Start()
    {
        // Initialize zoom targets and sizes
        zoomTargets = new Vector3[2];
        zoomTargets[0] = mainCamera.transform.position; // Initial position
        zoomTargets[1] = Vector3.zero; // Zoom out target position
    }

    public bool IsZoomed() // Public method to check if the camera is zoomed in
    {
        return isZoomed;
    }

    public bool IsZooming()
    {
        return isZooming;
    }

    private void Update()
    {
        // Check for right mouse button press
        if (Input.GetMouseButtonDown(1)) // 1 corresponds to the right mouse button
        {


            if (!isZoomed)
            {
                // Set zoom in target and size
                zoomTargets[0] = mainCamera.transform.position;
                zoomTargets[1] = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                StartCoroutine(ZoomIn());
                isZoomed = true;
            }
            else
            {
                // Set zoom out target and size
                zoomTargets[0] = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                zoomTargets[1] = mainCamera.transform.position;
                StartCoroutine(ZoomOut());
                isZoomed = false;
            }




        }
    }

    IEnumerator ZoomIn()
    {
        float elapsedTime = 0f;
        Vector3 startingPos = mainCamera.transform.position;
        float startingSize = mainCamera.orthographicSize;

        isZooming = true; // Set the zooming flag to true

        while (elapsedTime < smoothSpeed)
        {
            // Interpolate position and size over time
            mainCamera.transform.position = Vector3.Lerp(startingPos, zoomTargets[isZooming ? 1 : 0], (elapsedTime / smoothSpeed));
            mainCamera.orthographicSize = Mathf.Lerp(startingSize, !isZooming ? zoomOutSize : zoomInSize, (elapsedTime / smoothSpeed));

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        isZooming = false; // Reset the zooming flag
    }
    IEnumerator ZoomOut()
    {
        float elapsedTime = 0f;
        Vector3 startingPos = mainCamera.transform.position;
        float startingSize = mainCamera.orthographicSize;

        isZooming = true; // Set the zooming flag to true

        while (elapsedTime < smoothSpeed)
        {
            // Interpolate position and size over time
            mainCamera.transform.position = Vector3.Lerp(startingPos, zoomTargets[isZooming ? 0 : 1], (elapsedTime / smoothSpeed));
            mainCamera.orthographicSize = Mathf.Lerp(startingSize, !isZooming ? zoomInSize : zoomOutSize, (elapsedTime / smoothSpeed));

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        isZooming = false; // Reset the zooming flag
    }
}

