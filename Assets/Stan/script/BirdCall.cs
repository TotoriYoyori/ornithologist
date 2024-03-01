using UnityEngine;

public class BirdCall : MonoBehaviour
{
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Check if the AudioSource component exists and is configured properly
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
