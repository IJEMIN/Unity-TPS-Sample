using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    public float health { get; protected set; }
    public bool dead { get; protected set; }
    
    public event Action OnDeath;
    
    private const float minTimeBetDamaged = 0.1f;
    private float lastDamagedTime;

    protected bool IsInvulnerabe
    {
        get
        {
            if (Time.time >= lastDamagedTime + minTimeBetDamaged) return false;

            return true;
        }
    }
    
    protected virtual void OnEnable()
    {
        dead = false;
        health = startingHealth;
    }

    public virtual bool ApplyDamage(DamageMessage damageMessage)
    {
        if (IsInvulnerabe || damageMessage.damager == gameObject || dead) return false;

        lastDamagedTime = Time.time;
        health -= damageMessage.amount;
        
        if (health <= 0) Die();

        return true;
    }
    
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead) return;
        
        health += newHealth;
    }
    
    public virtual void Die()
    {
        if (OnDeath != null) OnDeath();
        
        dead = true;
    }
}