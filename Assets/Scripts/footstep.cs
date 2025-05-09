using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [Header("References")]
    public AudioSource audioSource;        // Reference to the AudioSource for playing sounds
    public AudioClip footstepSound;        // The footstep sound clip
    public float footstepInterval = 0.5f;  // Time between footstep sounds
    public float walkSpeedThreshold = 1f;  // Speed threshold for footstep sounds (optional)
    
    private CharacterController characterController;  // Character controller to check the movement
    private float timeSinceLastStep = 0f;              // Timer to play the next footstep sound
    private Vector3 previousPosition;                  // To track movement

    void Start()
    {
        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();
        
        // Ensure we have an AudioSource to play footsteps
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;  // 3D sound
        }
        
        previousPosition = transform.position;
    }

    void Update()
    {
        // Only play footsteps if the character is moving
        if (characterController.velocity.magnitude > walkSpeedThreshold)
        {
            timeSinceLastStep += Time.deltaTime;

            // If enough time has passed, play the footstep sound
            if (timeSinceLastStep >= footstepInterval)
            {
                PlayFootstepSound();
                timeSinceLastStep = 0f;  // Reset timer
            }
        }
        else
        {
            timeSinceLastStep = 0f;  // Reset timer if player stops moving
        }
    }

    void PlayFootstepSound()
    {
        if (footstepSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(footstepSound);  // Play the footstep sound once
        }
    }
}
