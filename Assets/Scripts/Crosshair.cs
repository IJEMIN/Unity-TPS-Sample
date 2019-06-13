using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Image reticle;
    public Image lookPoint;

    private RectTransform crossHairRectTransform;

    private Vector2 targetPoint;
    private Vector3 m_CurrentVelocity;

    private Camera screenCamera;

    public float smoothTime = 10f;
    
    private void Awake()
    {
        screenCamera = Camera.main;
        crossHairRectTransform = reticle.GetComponent<RectTransform>();
    }

    public void SetActiveReticle(bool active)
    {
        reticle.enabled = active;
        lookPoint.enabled = active;
    }

    public void UpdatePosition(Vector3 worldPoint)
    {
        targetPoint = screenCamera.WorldToScreenPoint(worldPoint);
    }

    private void Update()
    {
        if (reticle.enabled)
        {
            crossHairRectTransform.position = Vector3.SmoothDamp(crossHairRectTransform.position, targetPoint, ref m_CurrentVelocity, smoothTime* Time.deltaTime);
        }
    }
}
