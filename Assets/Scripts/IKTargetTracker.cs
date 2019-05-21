using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTargetTracker : MonoBehaviour
{
    [SerializeField] private AvatarIKGoal targetIK;
    [SerializeField] private Transform targetTransform;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        targetTransform.position = animator.GetIKPosition(targetIK);
        targetTransform.rotation = animator.GetIKRotation(targetIK);
    }
}