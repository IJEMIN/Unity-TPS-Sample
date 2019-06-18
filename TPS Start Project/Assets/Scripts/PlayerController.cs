using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    public AudioClip itemPickupClip;
    public int lifeRemains = 3;
    private AudioSource playerAudioPlayer;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerShooter playerShooter;

    private void Start()
    {
        Cursor.visible = false;
    }
    
    private void HandleDeath()
    {
        Cursor.visible = true;
    }

    public void Respawn()
    {
        Cursor.visible = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        
    }
}