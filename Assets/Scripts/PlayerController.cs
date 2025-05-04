using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("References")] [Tooltip("Reference to the main camera used for the player")]
    public Camera playerCamera;

    [Header("Movement")] [Tooltip("Max movement speed")]
    public float speed = 10f;

    [Header("Movement")] [Tooltip("Time to wait after putting/removing the mask to do it again")]
    public float maskDelay = 1f;

    [Header("Rotation")] [Tooltip("Rotation speed for moving the camera")]
    public float rotationSpeed = 2f;


    CharacterController m_controller;
    Animator m_animator;
    float m_cameraVerticalAngle = 0f;
    bool m_isAllowedMaskChange = true;
    Vector3 m_moveInput = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_controller = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Camera rotation
        transform.Rotate(new Vector3(0f, (Input.GetAxisRaw("Mouse X") * rotationSpeed), 0f), Space.Self);
        m_cameraVerticalAngle += Input.GetAxisRaw("Mouse Y") * rotationSpeed;
        m_cameraVerticalAngle = Mathf.Clamp(m_cameraVerticalAngle, -89f, 89f);
        playerCamera.transform.localEulerAngles = new Vector3(m_cameraVerticalAngle, 0, 0);

        // Player Movement
        m_moveInput.x = Input.GetAxisRaw("Horizontal");
        m_moveInput.z = Input.GetAxisRaw("Vertical");
        m_moveInput = Vector3.ClampMagnitude(m_moveInput, 1);
        m_controller.Move(m_moveInput * speed * Time.deltaTime);

        // Mask control
        if(m_isAllowedMaskChange && Input.GetKeyDown(KeyCode.E))
        {
            m_animator.SetTrigger("ChangeMask");
            m_isAllowedMaskChange = false;
        }
    }

    public void MaskChanged()
    {
        StartCoroutine(MaskWaiting());
    }

    IEnumerator MaskWaiting()
    {
        yield return new WaitForSeconds(maskDelay);
        m_isAllowedMaskChange = true;
    }
}
