using UnityEngine;

public class LateUpdateFollow : MonoBehaviour
{
    public Transform targetToFollow;

    private void LateUpdate()
    {
        transform.position = targetToFollow.position;
        transform.rotation = targetToFollow.rotation;
    }
}