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

    
    private void HandleDeath()
    {
        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }
}
