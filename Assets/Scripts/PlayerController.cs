using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the main camera used for the player")]
    public Camera playerCamera;

    [Header("Movement")]
    [Tooltip("Walk speed")]
    public float speed = 5f;
    [Tooltip("Sprint speed")]
    public float sprintSpeed = 8f;
    [Tooltip("Crouch speed")]
    public float crouchSpeed = 2f;
    [Tooltip("Smooth acceleration factor")]
    public float smoothTime = 0.1f; // How quickly the player accelerates/decelerates
    private Vector3 currentVelocity; // Used internally for SmoothDamp

    [Tooltip("Time to wait after putting/removing the mask to do it again")]
    public float maskDelay = 2f;

    [Header("Rotation")]
    [Tooltip("Rotation speed for moving the camera")]
    public float rotationSpeed = 2f;

    [Header("Gravity")]
    [Tooltip("Gravity force")]
    public float gravity = -9.81f; // Realistic gravity acceleration

    [Header("Crouch Settings")]
    [Tooltip("Height while crouching")]
    public float crouchHeight = 1f;
    [Tooltip("Height when standing")]
    public float standHeight = 2f;
    [Tooltip("How fast height changes when crouching")]
    public float crouchTransitionSpeed = 8f;

    [Header("Interactables")]
    [Tooltip("Angle from which the player can interact with an object")]
    public float interactAngle = 60f;
    [Tooltip("Distance from which a player can interact with an object")]
    public float interactDistance = 15f;

    CharacterController m_controller;
    Animator m_animator;
    float m_cameraVerticalAngle = 0f;
    bool m_isAllowedMaskChange = true;
    Vector3 m_moveInput = Vector3.zero;
    Vector3 m_moveInputWorld = Vector3.zero;
    float verticalVelocity = 0f; // Used to simulate gravity
    bool isMaskFunctionalityEnabled = true;

    List<IInteractable> m_interactableObjects = new List<IInteractable>();

    // Crouch state
    bool isCrouching = false;

    // Mask anim
    bool m_puttingMask = false;

    bool isPlayerDead = false;

    void Start()
    {
        m_controller = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();

        // ✅ Auto-assign camera if it's not manually set in the Inspector
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogError("PlayerController: No camera assigned and no camera tagged as MainCamera was found.");
            }
        }

        // Lock the mouse to the center of the screen and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MonoBehaviour[] allScripts = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        for (int i = 0; i < allScripts.Length; i++)
        {
            if (allScripts[i] is IInteractable)
            {
                m_interactableObjects.Add(allScripts[i] as IInteractable);
            }
        }
    }

    void Update()
    {
        if(m_puttingMask || isPlayerDead)
        {
            return;
        }

        // Camera rotation
        transform.Rotate(new Vector3(0f, (Input.GetAxisRaw("Mouse X") * rotationSpeed), 0f), Space.Self);
        m_cameraVerticalAngle += Input.GetAxisRaw("Mouse Y") * rotationSpeed;
        m_cameraVerticalAngle = Mathf.Clamp(m_cameraVerticalAngle, -89f, 89f);

        if (playerCamera != null)
        {
            playerCamera.transform.localEulerAngles = new Vector3(m_cameraVerticalAngle, 0, 0);
        }

        // Handle crouching
        isCrouching = Input.GetKey(KeyCode.LeftControl);
        float targetHeight = isCrouching ? crouchHeight : standHeight;
        m_controller.height = Mathf.Lerp(m_controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);

        // Determine target speed based on input
        float currentSpeed = speed;

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            currentSpeed = sprintSpeed;
        }
        else if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }

        // Get player input
        Vector3 targetInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        targetInput = Vector3.ClampMagnitude(targetInput, 1f);

        // Smooth the movement
        m_moveInput = Vector3.SmoothDamp(m_moveInput, targetInput, ref currentVelocity, smoothTime);

        // Convert to world space
        m_moveInputWorld = transform.TransformDirection(m_moveInput);

        // Apply gravity
        if (m_controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Final move vector
        Vector3 fullMove = m_moveInputWorld * currentSpeed + Vector3.up * verticalVelocity;

        // Move the player
        m_controller.Move(fullMove * Time.deltaTime);

        // Mask control
        if (m_isAllowedMaskChange && Input.GetMouseButtonDown(0) && isMaskFunctionalityEnabled)
        {
            m_animator.SetTrigger("ChangeMask");
            m_isAllowedMaskChange = false;
        }

        // Interact control
        if(Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < m_interactableObjects.Count; i++)
            {
                if(m_interactableObjects[i] == null)
                {
                    print(i);
                    continue;
                }
                Vector3 playerDir = m_interactableObjects[i].GetPosition() - transform.position;
                float playerAngle = Vector3.Angle(transform.forward, playerDir);
                float playerDistance = Vector3.Distance(transform.position, m_interactableObjects[i].GetPosition());

                if(playerAngle <= interactAngle && playerDistance <= interactDistance)
                {
                    m_interactableObjects[i].Interact();
                }
            }
        }
    }

    // This is called from the animation event after the mask animation is complete
    public void MaskChanged()
    {
        m_puttingMask = true;
        GameController.GetInstance().ChangeMask();
        StartCoroutine(MaskWaiting());
    }

    public void MaskAnimationEnd()
    {
        m_puttingMask = false;
    }

    // Coroutine that waits for mask delay before allowing another change
    IEnumerator MaskWaiting()
    {
        yield return new WaitForSeconds(maskDelay);
        m_isAllowedMaskChange = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            KillPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        if(!isPlayerDead)
        {
            //Release the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            isPlayerDead = true;
            GameController.GetInstance().KillPlayer();
        }
    }

    public void ChangeMaskSettings(bool isEnabled)
    {
        isMaskFunctionalityEnabled = isEnabled;
    }
}
