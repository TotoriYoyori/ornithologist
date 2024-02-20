using UnityEngine;

public class BirdDetector : MonoBehaviour
{
    private bool isBirdInView = false;

    [Header("AcceptableBoundaries")]
    // Define the acceptable screen area for the bird
    public float minScreenX = 0.3f; // Minimum X value (left side of the screen)
    public float maxScreenX = 0.7f; // Maximum X value (right side of the screen)
    public float minScreenY = 0.3f; // Minimum Y value (bottom side of the screen)
    public float maxScreenY = 0.7f; // Maximum Y value (top side of the screen)
    public bool IsAnyBirdInView()
    {
        return isBirdInView;
    }

    private void Update()
    {

        // Check if the bird is within the defined screen area
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        isBirdInView = screenPoint.x >= minScreenX && screenPoint.x <= maxScreenX &&
                       screenPoint.y >= minScreenY && screenPoint.y <= maxScreenY &&
                       screenPoint.z > 0f;
    }
}

