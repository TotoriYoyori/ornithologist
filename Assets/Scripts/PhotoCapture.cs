using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    [Header("Photo Taker")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;

    [Header("Stored Photo Displays")]
    [SerializeField] private Image[] storedPhotoDisplayAreas; // Array of stored photo displays
    [SerializeField] private GameObject[] storedPhotoFrames; // Array of stored photo frames
    [SerializeField] private int maxStoredPhotos = 5; // Maximum number of stored photos

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadeInAnimation;

    private Texture2D screenCapture;
    private Texture2D storedPhotoTexture;
    private bool viewingPhoto;
    private int storedPhotosCount = 0; // Count of currently stored photos
    private ZoomOnClick zoomScript; // Reference to the ZoomOnClick script

    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        storedPhotoTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        zoomScript = GetComponent<ZoomOnClick>(); // Get the ZoomOnClick component
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!viewingPhoto && zoomScript.IsZoomed()) // Check if camera is zoomed in
            {
                StartCoroutine(CapturePhoto());
                StartCoroutine(CaptureStoredPhoto());
            }
            else if (viewingPhoto)
            {
                RemovePhoto();
            }
        }
    }

    IEnumerator CapturePhoto()
    {
        viewingPhoto = true;

        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();
        ShowPhoto();
    }
    IEnumerator CaptureStoredPhoto()
    {
        viewingPhoto = true;

        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        // Create a new Texture2D instance for the stored photo
        Texture2D storedPhotoTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        storedPhotoTexture.ReadPixels(regionToRead, 0, 0, false);
        storedPhotoTexture.Apply();
        StorePhoto(storedPhotoTexture); // Pass the captured texture to the StorePhoto method
    }

    void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;
        photoFrame.SetActive(true);

        fadeInAnimation.Play("PhotoFade");
    }
    void StorePhoto(Texture2D photoTexture)
    {
        if (storedPhotosCount < maxStoredPhotos)
        {
            // Create a sprite from the new texture
            Sprite photoSprite = Sprite.Create(photoTexture, new Rect(0.0f, 0.0f, photoTexture.width, photoTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

            // Assign the sprite to the current stored photo display area
            storedPhotoDisplayAreas[storedPhotosCount].sprite = photoSprite;

            // Activate the corresponding stored photo frame
            storedPhotoFrames[storedPhotosCount].SetActive(true);

            // Increment the count of stored photos
            storedPhotosCount++;
        }
        else
        {
            Debug.LogWarning("Maximum number of stored photos reached.");
        }
    }

    void RemovePhoto()
    {
        viewingPhoto = false;
        photoFrame.SetActive(false);

        // Hide current photo frame
        photoDisplayArea.sprite = null;
    }
}