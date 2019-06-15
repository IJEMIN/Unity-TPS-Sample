using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float jumpVelocity = 20f;

    [Range(0.01f, 1f)] public float airControlPercent;
    public float turnSmoothTime = 0.2f;
    private float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    private float speedSmoothVelocity;

    public float currentSpeed =>
        new Vector2(m_CharacterController.velocity.x, m_CharacterController.velocity.z).magnitude;

    private float currentVelocityY;

    private Animator m_Animator;
    private Camera m_CharacterFollowCam;

    private CharacterController m_CharacterController;

    private PlayerInput playerInput;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        m_CharacterFollowCam = Camera.main;
        m_CharacterController = GetComponent<CharacterController>();
        m_CharacterController.detectCollisions = false;
    }

    private void FixedUpdate()
    {
        if (currentSpeed > 0.2f || playerInput.fire) Rotate();

        Move(playerInput.moveInput);
        if (playerInput.jump) Jump();
    }

    private void Update()
    {
        UpdateAnimation(playerInput.moveInput);
    }

    public void Move(Vector2 moveInput)
    {
        var targetSpeed = speed * moveInput.magnitude;
        var moveDirection = Vector3.Normalize(transform.forward * moveInput.y + transform.right * moveInput.x);
        var smoothTime = m_CharacterController.isGrounded ? speedSmoothTime : speedSmoothTime / airControlPercent;

        targetSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, smoothTime);
        currentVelocityY += Time.deltaTime * Physics.gravity.y;

        var velocity = moveDirection * targetSpeed + Vector3.up * currentVelocityY;

        m_CharacterController.Move(velocity * Time.deltaTime);

        if (m_CharacterController.isGrounded) currentVelocityY = 0;
    }

    public void Rotate()
    {
        var targetRotation = m_CharacterFollowCam.transform.eulerAngles.y;
        var smoothTime = m_CharacterController.isGrounded ? turnSmoothTime : turnSmoothTime / airControlPercent;

        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
                                    ref turnSmoothVelocity, smoothTime);
    }

    public void Jump()
    {
        if (!m_CharacterController.isGrounded) return;
        currentVelocityY = jumpVelocity;
    }

    private void UpdateAnimation(Vector2 moveInput)
    {
        var animationSpeedPercent = currentSpeed / speed;

        m_Animator.SetFloat("Horizontal Move", moveInput.x * animationSpeedPercent, 0.05f, Time.deltaTime);
        m_Animator.SetFloat("Vertical Move", moveInput.y * animationSpeedPercent, 0.05f, Time.deltaTime);
    }
}