using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExtraEffects : MonoBehaviour
{
    [SerializeField] ZoomOnClick zoomOnClick;
    [SerializeField] PhotoCapture photoCapture;
    [SerializeField] BirdPhotoController birdPhotoController;

    [Header("CloseUp View Effects")]
    public GameObject closeUpCamera;
    public AudioSource zoomingSound;
    public GameObject bookButton;

    private bool isZoomingSoundPlaying = false;

    void Start()
    {

    }
    void Update()
    {
        if (zoomOnClick.IsZooming() && !isZoomingSoundPlaying)
        {
            zoomingSound.Play();
            isZoomingSoundPlaying = true;
            closeUpCamera.SetActive(true);
        }
        else if (!zoomOnClick.IsZooming() && isZoomingSoundPlaying)
        {
            zoomingSound.Stop();
            isZoomingSoundPlaying = false;
        }
        if (zoomOnClick.IsZoomed()) bookButton.SetActive(false);
        if (!zoomOnClick.IsZoomed())
        {
            closeUpCamera.SetActive(false);
            bookButton.SetActive(true);
        }
    }

   
}

