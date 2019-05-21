using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerWeaponController : MonoBehaviour
{
    private Animator playerAnimator; // 애니메이터 컴포넌트

    [SerializeField] private MeleeWeapon weapon;

    public bool isAttacking { get; private set; }

    private void Awake()
    {
        // 사용할 컴포넌트들을 가져오기
        playerAnimator = GetComponent<Animator>();

        weapon.SetOwner(gameObject);
    }


    public void MeleeAttackStart()
    {
        ActiveAttackState(true);
    }
    
    public void MeleeAttackEnd()
    {
        ActiveAttackState(false);
    }
    
    public void Attack()
    {
        if (isAttacking)
        {
            return;
        }

        playerAnimator.SetTrigger("Attack");
    }


    private void ActiveAttackState(bool activeAttack)
    {
        isAttacking = activeAttack;

        if (activeAttack)
        {
            weapon.BeginAttack();
        }
        else
        {
            weapon.EndAttack();
        }
    }
}