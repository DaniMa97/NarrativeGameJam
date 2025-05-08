using UnityEngine;

public class PlayAnimationOnTrigger : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Animator that will play the animation")]
    public Animator animator;

    [Tooltip("Name of the animation trigger to activate")]
    public string animationTrigger = "Play";

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            hasPlayed = true;

            if (animator != null && !string.IsNullOrEmpty(animationTrigger))
            {
                animator.SetTrigger(animationTrigger);
            }
            else
            {
                Debug.LogWarning("Animator or animation trigger not set on PlayAnimationOnTrigger script.");
            }
        }
    }
}
