using UnityEngine;

public class KeyCardInteractable : MonoBehaviour, IInteractable
{

    public KeyDoorInteractable doorToUnlock;

    bool interactable = true;

    Vector3 IInteractable.GetPosition()
    {
        return transform.position;
    }

    void IInteractable.Interact()
    {
        if(doorToUnlock != null && interactable)
        {
            interactable = false;
            doorToUnlock.UnlockDoor();
            gameObject.SetActive(false);
        }
    }
}
