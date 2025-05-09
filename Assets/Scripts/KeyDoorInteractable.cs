using UnityEngine;

public class KeyDoorInteractable : MonoBehaviour, IInteractable
{
    bool isUnlocked = false;
    bool isOpened = false;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void IInteractable.Interact()
    {
        if(isUnlocked && !isOpened)
        {
            animator.SetTrigger("Open");
            isOpened = true;
        }
    }

    public void UnlockDoor()
    {
        isUnlocked = true;
    }

    Vector3 IInteractable.GetPosition()
    {
        return transform.position;
    }
}
