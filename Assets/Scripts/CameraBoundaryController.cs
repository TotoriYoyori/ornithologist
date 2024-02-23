using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    // Update is called once per frame
    void Update()
    {
        // Get the current position of the camera
        Vector3 newPosition = transform.position;

        // Clamp the x position of the camera within the specified range
        newPosition.x = Mathf.Clamp(transform.position.x, minX, maxX);

        // Clamp the y position of the camera within the specified range
        newPosition.y = Mathf.Clamp(transform.position.y, minY, maxY);

        // Update the position of the camera
        transform.position = newPosition;
    }
}

