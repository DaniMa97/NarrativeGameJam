using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public Transform player;               // Reference to the player
    public float interactRange = 3f;       // How close the player needs to be
    public AudioClip openSound;            // Sound to play when door opens
    public AudioSource audioSource;        // Optional AudioSource

    private bool doorOpened = false;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; // Set to 1f if using 3D sound
        }
    }

    void Update()
    {
        if (doorOpened || player == null) return;

        if (Vector3.Distance(player.position, transform.position) <= interactRange && Input.GetKeyDown(KeyCode.E))
        {
            KeyInventory inv = player.GetComponent<KeyInventory>();
            if (inv != null && inv.hasKey)
            {
                OpenDoor();
                doorOpened = true;
            }
            else
            {
                Debug.Log("The door is locked. You need a keycard.");
            }
        }
    }

    void OpenDoor()
    {
        if (openSound != null)
        {
            Debug.Log("Playing door open sound: " + openSound.name);
            audioSource.PlayOneShot(openSound);
        }
        else
        {
            Debug.LogWarning("No door open sound assigned.");
        }

        Destroy(gameObject); // Remove the door
    }
}
