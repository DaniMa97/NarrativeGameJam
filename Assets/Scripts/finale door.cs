using UnityEngine;

public class WorldSwitchDoor : MonoBehaviour
{
    public Transform player;
    public float interactRange = 3f;
    public AudioClip openSound;
    public AudioSource audioSource;

    private bool doorUsed = false;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; // 2D sound
        }
    }

    void Update()
    {
        if (doorUsed || player == null) return;

        if (Vector3.Distance(player.position, transform.position) <= interactRange && Input.GetKeyDown(KeyCode.E))
        {
            if (WorldSwitchKeyPickup.hasWorldSwitchKey)
            {
                Debug.Log("World switching...");
                if (openSound != null)
                    audioSource.PlayOneShot(openSound);

                GameController.GetInstance().ChangeMask(); // Force switch
                doorUsed = true;
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("You need the key to use this door.");
            }
        }
    }
}
