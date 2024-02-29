using UnityEngine;
using System.Collections.Generic;

public class BirdPhotoController : MonoBehaviour
{
    [SerializeField] private PhotoCapture photoCapture; // Reference to the PhotoCapture script
    private List<BirdDetector> birdDetectors = new List<BirdDetector>(); // Reference to the BirdDetector scripts
    [SerializeField] private ZoomOnClick zoomOnClick; //Reference to the ZoomOnClick script
    [SerializeField] private PauseGame pauseManager;

    private bool viewingPhoto = false; // Flag to track whether a photo is currently being taken

    private void Update()
    {
        if (!pauseManager.IsPaused())
        {

            // Check if the player is trying to take a photo
            if (Input.GetMouseButtonDown(0))
            {
                if (!viewingPhoto && zoomOnClick.IsZoomed())
                {
                    if (!zoomOnClick.IsZooming())
                    {
                        viewingPhoto = true;
                        photoCapture.StartCoroutine(photoCapture.HideCamera());
                        photoCapture.StartCoroutine(photoCapture.CapturePhoto());

                        if (IsAnyBirdInView())
                        {
                            // Iterate through each bird detector to find the first bird in view and capture its photo
                            foreach (var birdDetector in birdDetectors)
                            {
                                if (birdDetector.IsAnyBirdInView())
                                {
                                    // Get the bird species from the BirdDetector attached to the bird game object
                                    string birdSpecies = birdDetector.birdSpecies;

                                    // Capture the photo and pass the bird species
                                    photoCapture.StartCoroutine(photoCapture.CaptureStoredPhoto(birdSpecies));
                                    return; // Exit the loop after capturing the photo for the first bird found
                                }
                            }
                        }
                        else
                        {
                            Debug.LogWarning("No bird in view. Photo not captured.");
                        }
                    }
                }
                else if (viewingPhoto)
                {
                    StartCoroutine(photoCapture.RemoveShowedPhoto());
                    viewingPhoto = false;
                }
            }
        }
    }

    private bool IsAnyBirdInView()
    {
        // Check if any bird is in view by iterating through each BirdDetector
        foreach (var birdDetector in birdDetectors)
        {
            if (birdDetector.IsAnyBirdInView())
            {
                return true;
            }
        }
        return false;
    }

    // Method to add a BirdDetector reference
    public void AddBirdDetector(BirdDetector birdDetector)
    {
        birdDetectors.Add(birdDetector);
        Debug.Log("Added BirdDetector: " + birdDetector.gameObject.name);
    }
}
