using System;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    [Header("Character Movement Property")]
    public float speed = 6;
    public float gravity = -12;
    public float jumpHeight = 1;

    [Range(0, 1)]
    public float airControlPercent;
    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;

    public float moveInputThreshold = 0.2f;


    private PlayerShooter m_PlayerShooter;
    

    Animator m_Animator;
    Transform m_CharacterFollowCam;

    PlayerInput m_PlayerInput;
    CharacterController m_CharacterController;


    public bool Run = false;
    void Start()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
        m_Animator = GetComponent<Animator>();
        m_CharacterFollowCam = Camera.main.transform;
        m_CharacterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (m_PlayerInput.moveInput.magnitude > moveInputThreshold)
        {
            Rotate(m_PlayerInput.moveInput);
            Move(m_PlayerInput.moveInput);
        }
    }

    public void Move(Vector2 inputDir)
    {
        float targetSpeed = speed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed * inputDir.y + transform.right * currentSpeed * inputDir.x + Vector3.up * velocityY;

        m_CharacterController.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(m_CharacterController.velocity.x, m_CharacterController.velocity.z).magnitude;

        if (m_CharacterController.isGrounded)
        {
            velocityY = 0;
        }


        // animator
        float animationSpeedPercent = currentSpeed / speed;

        //  m_Animator.SetFloat("Move", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
    }

    public void Rotate(Vector2 direction)
    {

        var targetRotation = m_CharacterFollowCam.eulerAngles.y;

        if (direction != Vector2.zero && Run)
        {
            targetRotation += Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        }

        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
    }

    public void Jump()
    {
        if (m_CharacterController.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (m_CharacterController.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }
}
