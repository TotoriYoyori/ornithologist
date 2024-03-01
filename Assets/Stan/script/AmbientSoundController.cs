using UnityEngine;

public class AmbientSoundController : MonoBehaviour
{
    // Reference to the existing AudioSource component
    public AudioSource audioSource;

    void Start()
    {
        // Ensure there's an AudioSource component attached
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource assigned to AmbientSoundController!");
            return;
        }

        // Play the ambient sound
        audioSource.loop = true;
        audioSource.Play();
    }
}
