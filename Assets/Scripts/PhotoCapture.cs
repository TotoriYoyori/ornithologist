using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    [Header("Photo Taker")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;
    [SerializeField] private GameObject cameraUI;

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

    public GameObject winScreen;

    private Dictionary<string, int[]> speciesToSlotRange = new Dictionary<string, int[]>(); // Dictionary to map bird species to slot ranges
    private Dictionary<string, int> speciesToCurrentSlotIndex = new Dictionary<string, int>(); // Dictionary to track current slot index for each bird species

    [Header("Bird Slot Allocation")]
    [SerializeField] private SpeciesSlotRange[] speciesSlotRanges; // Serialized field to define slot ranges for each species
    [System.Serializable]
    public class SpeciesSlotRange
    {
        public string speciesName;
        public int startSlot;
        public int endSlot;
    }



    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        storedPhotoTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        zoomScript = GetComponent<ZoomOnClick>(); // Get the ZoomOnClick component

        InitializeSpeciesToCurrentSlotIndex(); // Initialize the species to current slot index dictionary
        InitializeSpeciesToSlotRange();

    }

    public bool ViewingPhoto()
    {  
     return viewingPhoto;
    }

    public IEnumerator CapturePhoto()
    {
        cameraUI.SetActive(false);

        viewingPhoto = true;

        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();
        ShowPhoto();
    }
    public IEnumerator CaptureStoredPhoto(string birdSpecies)
    {
        viewingPhoto = true;

        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        // Create a new Texture2D instance for the stored photo
        Texture2D storedPhotoTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        storedPhotoTexture.ReadPixels(regionToRead, 0, 0, false);
        storedPhotoTexture.Apply();
        StorePhoto(storedPhotoTexture, birdSpecies); // Pass the captured texture to the StorePhoto method
    }

    public IEnumerator RemoveShowedPhoto()
    {
        viewingPhoto = false;
        
        yield return new WaitForEndOfFrame();

        RemovePhoto();
    }

    void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;
        photoFrame.SetActive(true);

        fadeInAnimation.Play("PhotoFade");
    }
    void StorePhoto(Texture2D photoTexture, string birdSpecies)
    {
        if (speciesToSlotRange.ContainsKey(birdSpecies)) // Check if the species has a defined slot range
        {
            var slotRange = speciesToSlotRange[birdSpecies]; // Get the slot range for the bird species

            if (slotRange != null && slotRange.Length >= 2)
            {
                int startSlot = slotRange[0];
                int endSlot = slotRange[1];

                if (startSlot >= 0 && startSlot < maxStoredPhotos)
                {
                    int currentSlotIndex = GetNextAvailableSlotIndex(birdSpecies, startSlot, endSlot);

                    // Check if the current slot index is within the specified range
                    if (currentSlotIndex >= startSlot && currentSlotIndex <= endSlot)
                    {
                        Sprite photoSprite = Sprite.Create(photoTexture, new Rect(0.0f, 0.0f, photoTexture.width, photoTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

                        storedPhotoDisplayAreas[currentSlotIndex].sprite = photoSprite;
                        storedPhotoFrames[currentSlotIndex].SetActive(true);

                        // Update the current slot index for the bird species
                        speciesToCurrentSlotIndex[birdSpecies] = currentSlotIndex;

                        // Increment the count of stored photos if necessary
                        if (currentSlotIndex == storedPhotosCount)
                        {
                            storedPhotosCount++;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No available slots in the specified range for bird species: " + birdSpecies);
                    }
                }
                else
                {
                    Debug.LogWarning("Invalid start slot index for bird species: " + birdSpecies);
                }
            }
            else
            {
                Debug.LogWarning("Invalid slot range for bird species: " + birdSpecies);
            }
        }
        else
        {
            Debug.LogWarning("No slot range defined for bird species: " + birdSpecies);
        }
    }


    void RemovePhoto()
    {
        cameraUI.SetActive(true);

        photoFrame.SetActive(false);

        // Hide current photo frame
        photoDisplayArea.sprite = null;
    }

    public IEnumerator HideCamera()
    {
        if (ViewingPhoto())
        {
            cameraUI.SetActive(false);
        }
        yield return new WaitForEndOfFrame();
    }

    private int GetNextAvailableSlotIndex(string birdSpecies, int startSlot, int endSlot)
    {
        if (!speciesToCurrentSlotIndex.ContainsKey(birdSpecies))
        {
            speciesToCurrentSlotIndex[birdSpecies] = startSlot;
            return startSlot;
        }

        int currentSlotIndex = speciesToCurrentSlotIndex[birdSpecies] + 1;

        // If the current slot index exceeds the end slot, wrap around to the start slot
        if (currentSlotIndex > endSlot)
        {
            currentSlotIndex = startSlot;
        }

        return currentSlotIndex;
    }

    private void InitializeSpeciesToCurrentSlotIndex()
    {
        // Initialize current slot index for each bird species to the start of their slot range
        foreach (var pair in speciesToSlotRange)
        {
            speciesToCurrentSlotIndex[pair.Key] = pair.Value[0];
        }
    }

    private void InitializeSpeciesToSlotRange()
    {
        // Initialize species to slot range dictionary from serialized fields
        foreach (var range in speciesSlotRanges)
        {
            speciesToSlotRange[range.speciesName] = new int[] { range.startSlot, range.endSlot };
        }
    }
}