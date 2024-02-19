using UnityEngine;

public class BirdDetector : MonoBehaviour
{
    private bool isBirdInView = false;

    public bool IsBirdInView()
    {
        return isBirdInView;
    }

    private void Update()
    {
        // Check if the bird is within the screen bounds
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        isBirdInView = screenPoint.x >= 0f && screenPoint.x <= 1f &&
                       screenPoint.y >= 0f && screenPoint.y <= 1f && screenPoint.z > 0f;
    }
}

