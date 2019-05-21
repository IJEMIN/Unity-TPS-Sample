using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(9999)]
public class FixedUpdateFollow : MonoBehaviour
{
    public Transform targetToFollow;

    private void FixedUpdate()
    {
        transform.position = targetToFollow.position;
        transform.rotation = targetToFollow.rotation;
    }
}