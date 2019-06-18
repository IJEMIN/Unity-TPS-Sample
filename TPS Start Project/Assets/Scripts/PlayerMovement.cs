using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInput playerInput;
    private Animator animator;
    
    private Camera followCam;
    
    public float speed = 6f;
    public float jumpVelocity = 20f;
    [Range(0.01f, 1f)] public float airControlPercent;

    public float speedSmoothTime = 0.1f;
    public float turnSmoothTime = 0.1f;
    
    private float speedSmoothVelocity;
    private float turnSmoothVelocity;
    
    private float currentVelocityY;
    
    public float currentSpeed =>
        new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;
    
    private void Start()
    {

    }

    private void FixedUpdate()
    {
        if (currentSpeed > 0.2f || playerInput.fire) Rotate();

        Move(playerInput.moveInput);
        
        if (playerInput.jump) Jump();
    }

    private void Update()
    {
        
    }

    public void Move(Vector2 moveInput)
    {

    }

    public void Rotate()
    {

    }

    public void Jump()
    {
        
    }

    private void UpdateAnimation(Vector2 moveInput)
    {

    }
}