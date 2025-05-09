using UnityEngine;

public class EnableMask : MonoBehaviour
{
    PlayerController player;

    private void Start()
    {
        player = GameObject.FindFirstObjectByType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.ChangeMaskSettings(true);
        }
    }
}
