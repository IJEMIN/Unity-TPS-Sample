using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour, IDamageable
{
    [Range(0.2f, 2f)] public float damageApplyScale = 1f;

    private LivingEntity entity;

    void Awake()
    {
        entity = GetComponentInParent<LivingEntity>();
    }
    
    public void ApplyDamage(DamageMessage message)
    {
        EffectManager.Instance.PlayHitEffect(message.hitPoint,message.hitNormal,transform,EffectManager.EffectType.Flesh);
        message.amount *= damageApplyScale;
        entity.ApplyDamage(message);
    }
}
