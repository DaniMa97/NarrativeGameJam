using UnityEngine;

public class WorldSwitchDoor : MonoBehaviour
{
    public Transform player;
    public float interactRange = 3f;
    public AudioClip openSound;
    public AudioSource audioSource;
    GameObject playerGO;

    private bool doorUsed = false;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; // 2D sound
        }
        playerGO = GameObject.Find("Player");
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
                playerGO.GetComponent<Animator>().SetTrigger("ChangeMask");
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
