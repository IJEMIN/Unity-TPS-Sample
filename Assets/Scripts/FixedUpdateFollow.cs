using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedUpdateFollow : MonoBehaviour
{
    public Transform targetToFollow;

    private void LateUpdate()
    {
        transform.position = targetToFollow.position;
        transform.rotation = targetToFollow.rotation;
    }
}