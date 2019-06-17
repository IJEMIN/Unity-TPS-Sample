using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private RectTransform crossHairRectTransform;
    public Image lookPoint;
    private Vector3 m_CurrentVelocity;
    public Image reticle;

    private Camera screenCamera;

    public float smoothTime = 10f;

    private Vector2 targetPoint;

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
        if (!reticle.enabled) return;

        crossHairRectTransform.position = Vector3.SmoothDamp(crossHairRectTransform.position, targetPoint,
            ref m_CurrentVelocity, smoothTime * Time.deltaTime);
    }
}