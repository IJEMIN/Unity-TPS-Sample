using UnityEngine;

public class IKTargetTracker : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AvatarIKGoal targetIK;
    [SerializeField] private Transform targetTransform;

    private void Start()
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