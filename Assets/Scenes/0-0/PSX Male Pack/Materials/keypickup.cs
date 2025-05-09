using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public AudioClip pickupSound;
    public AudioSource audioSource;
    public Transform player;
    public float interactRange = 3f;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; // Set to 1f for 3D sound
        }
    }

    void Update()
    {
        if (player == null) return;

        if (Vector3.Distance(player.position, transform.position) <= interactRange && Input.GetKeyDown(KeyCode.E))
        {
            KeyInventory inv = player.GetComponent<KeyInventory>();
            if (inv != null)
            {
                inv.hasKey = true;

                if (pickupSound != null)
                {
                    Debug.Log("Playing pickup sound: " + pickupSound.name);
                    audioSource.PlayOneShot(pickupSound);
                }
                else
                {
                    Debug.LogWarning("No pickup sound assigned.");
                }

                Destroy(gameObject); // Destroy the keycard
            }
        }
    }
}
