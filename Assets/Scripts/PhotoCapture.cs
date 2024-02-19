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
    private bool viewingPhoto;
    private int storedPhotosCount = 0; // Count of currently stored photos

    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!viewingPhoto)
            {
                StartCoroutine(CapturePhoto());
            }
            else
            {
                StorePhoto();
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

    void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;
        photoFrame.SetActive(true);

        fadeInAnimation.Play("PhotoFade");
    }

    void StorePhoto()
    {
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        if (storedPhotosCount < maxStoredPhotos)
        {
            storedPhotoDisplayAreas[storedPhotosCount].sprite = photoSprite;
            storedPhotoFrames[storedPhotosCount].SetActive(true);

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
