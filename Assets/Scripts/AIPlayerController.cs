using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;
    private PlayerHealth playerHealth;


    public Transform target;
    
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
        playerHealth = GetComponent<PlayerHealth>();
        playerHealth.onDeath += HandleDeath;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    private void FixedUpdate()
    {
        if (playerHealth.dead) return;


        var localVelocity  = transform.InverseTransformVector(agent.desiredVelocity);
        var moveInput = new Vector2(localVelocity.x, localVelocity.z);
        
        playerMovement.Move(moveInput.normalized);

    }

    private void HandleDeath()
    {
        playerMovement.enabled = false;
        playerShooter.enabled = false;
        animator.enabled = false;
    }
}
