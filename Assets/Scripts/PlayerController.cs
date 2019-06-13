using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;
    private PlayerHealth playerHealth;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
        playerHealth = GetComponent<PlayerHealth>();
        playerHealth.onDeath += HandleDeath;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.dead) return;
        
        if (playerInput.fire)
        {
            playerShooter.Shoot();
        }
        else if (playerInput.reload)
        {
            playerShooter.Reload();
        }
    }

    private void FixedUpdate()
    {
        if (playerHealth.dead) return;

        if (playerMovement.currentSpeed > 0.2f)
        {
            playerMovement.Rotate();
        }
        
        playerMovement.Move(playerInput.moveInput);
        if(playerInput.jump) playerMovement.Jump();
    }

    private void HandleDeath()
    {
        playerMovement.enabled = false;
        playerShooter.enabled = false;
        animator.enabled = false;
    }
}
