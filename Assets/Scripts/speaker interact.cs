using UnityEngine;

public class PlaySoundOnInteract : MonoBehaviour
{
    public AudioClip soundClip;
    public AudioSource audioSource;
    public float interactionRange = 3f;  // Distance within which E works
    public Transform player;

    private bool playerInRange = false;

    void Start()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);
        playerInRange = distance <= interactionRange;

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (soundClip != null)
                audioSource.PlayOneShot(soundClip);
            else
                Debug.LogWarning("No sound clip assigned.");
        }
    }
}
