using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        float speed = (currentPosition - lastPosition).magnitude / Time.deltaTime;

        animator.SetFloat("speed", speed);

        lastPosition = currentPosition;
    }
}
