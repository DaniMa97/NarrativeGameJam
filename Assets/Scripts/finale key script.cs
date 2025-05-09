using UnityEngine;

public class WorldSwitchKeyPickup : MonoBehaviour
{
    public AudioClip pickupSound;
    public AudioSource audioSource;
    public Transform player;
    public float interactRange = 3f;

    public static bool hasWorldSwitchKey = false;

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
        if (player == null) return;

        if (Vector3.Distance(player.position, transform.position) <= interactRange && Input.GetKeyDown(KeyCode.E))
        {
            hasWorldSwitchKey = true;

            if (pickupSound != null)
                audioSource.PlayOneShot(pickupSound);

            Debug.Log("Key picked up.");
            Destroy(gameObject);
        }
    }
}
