using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Image aimPointReticle;
    public Image hitPointReticle;

    public float smoothTime = 10f;
    
    private Camera screenCamera;
    private RectTransform crossHairRectTransform;

    private Vector3 currentHitPointVelocity;
    private Vector2 targetPoint;

    private void Awake()
    {

    }

    public void SetActiveCrosshair(bool active)
    {

    }

    public void UpdatePosition(Vector3 worldPoint)
    {
    }

    private void Update()
    {

    }
}