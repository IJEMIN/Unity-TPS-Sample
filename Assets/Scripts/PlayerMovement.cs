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

    private PlayerShooter m_PlayerShooter;
    

    Animator m_Animator;
    Camera m_CharacterFollowCam;

    PlayerInput m_PlayerInput;
    CharacterController m_CharacterController;

    void Start()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
        m_Animator = GetComponent<Animator>();
        m_CharacterFollowCam = Camera.main;
        m_CharacterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        UpdateAnimation(m_PlayerInput.moveInput);
    }
    private void FixedUpdate()
    {
        if(currentSpeed > 0.2f)
        {
            Rotate();
        }

        if (m_PlayerInput.jump) Jump();
        
        Move(m_PlayerInput.moveInput);   
    }

    public void Move(Vector2 moveInput)
    {
        float targetSpeed = speed * moveInput.magnitude;


        Vector3 moveDirection = Vector3.Normalize(transform.forward * moveInput.y + transform.right * moveInput.x);
        


        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;

        Vector3 velocity = moveDirection * currentSpeed;
        velocity += Vector3.up * velocityY;

        m_CharacterController.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(m_CharacterController.velocity.x, m_CharacterController.velocity.z).magnitude;

        if (m_CharacterController.isGrounded)
        {
            velocityY = 0;
        }


        // animator

        //  m_Animator.SetFloat("Move", animationSpeedPercent, speedSmoothTime, Time.deltaTime);


    }

    public void Rotate()
    {
        var targetRotation = m_CharacterFollowCam.transform.eulerAngles.y;
        
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

    void UpdateAnimation(Vector2 moveInput)
    {
        float animationSpeedPercent = currentSpeed / speed;

        m_Animator.SetFloat("Horizontal Move", moveInput.x * animationSpeedPercent);
        m_Animator.SetFloat("Vertical Move", moveInput.y * animationSpeedPercent);
    }
    
}
