using UnityEngine;

public class BirdPhotoController : MonoBehaviour
{
    [SerializeField] private PhotoCapture photoCapture; // Reference to the PhotoCapture script
    [SerializeField] private BirdDetector[] birdDetectors; // Reference to the BirdDetector scripts
    [SerializeField] private ZoomOnClick zoomOnClick; //Reference to the ZoomOnClick script

    private bool viewingPhoto = false; // Flag to track whether a photo is currently being taken

    private void Update()
    {
        // Check if the player is trying to take a photo
        if (Input.GetMouseButtonDown(0))
        {
            
            if (!viewingPhoto && zoomOnClick.IsZoomed())
            {
                viewingPhoto = true;
                photoCapture.StartCoroutine(photoCapture.CapturePhoto());

                if (IsAnyBirdInView())
                {
                    photoCapture.StartCoroutine(photoCapture.CaptureStoredPhoto());
                }
                else
                {
                    Debug.LogWarning("No bird in view. Photo not captured.");
                }
            }
            else if (viewingPhoto)
            {
                StartCoroutine(photoCapture.RemoveShowedPhoto());
                viewingPhoto = false;
            }
        }
    }

    private bool IsAnyBirdInView()
    {
        foreach (var birdDetector in birdDetectors)
        {
            if (birdDetector.IsBirdInView())
            {
                return true;
            }
        }
        return false;
    }
}

