using UnityEngine;

public class PlaySoundAndDestroy : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("The sound that will play when the player enters the trigger")]
    public AudioClip soundToPlay;

    [Header("Objects to Destroy (Optional)")]
    [Tooltip("Optional GameObject to destroy after sound plays")]
    public GameObject objectToDestroy1;

    [Tooltip("Optional GameObject to destroy after sound plays")]
    public GameObject objectToDestroy2;

    [Tooltip("Optional GameObject to destroy after sound plays")]
    public GameObject objectToDestroy3;

    private AudioSource audioSource;
    private bool hasPlayed = false;

    void Start()
    {
        // Add AudioSource if not already attached
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundToPlay;
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger once, and only when the Player enters
        if (!hasPlayed && other.CompareTag("Player"))
        {
            hasPlayed = true;
            audioSource.Play();
            StartCoroutine(DestroyAfterSound());
        }
    }

    private System.Collections.IEnumerator DestroyAfterSound()
    {
        // Wait for the audio clip to finish
        yield return new WaitForSeconds(audioSource.clip.length);

        // Destroy optional objects if assigned
        if (objectToDestroy1 != null) Destroy(objectToDestroy1);
        if (objectToDestroy2 != null) Destroy(objectToDestroy2);
        if (objectToDestroy3 != null) Destroy(objectToDestroy3);

        // Finally destroy this object
        Destroy(gameObject);
    }
}
