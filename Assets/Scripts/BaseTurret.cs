using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public abstract class BaseTurret : MonoBehaviour
{
    [Header("Base Settings")]
    public float range = 10f;
    public float fireRate = 1f;
    public float rotationSpeed = 5f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    protected float fireTimer;
    protected LineRenderer rangeRenderer;
    protected bool canFire = true;

    protected void TrackPlayer(Vector3 playerPos, float deltaTime)
    {
        Vector3 direction = (playerPos - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * deltaTime);
        }
    }

    protected virtual void Awake()
    {
        rangeRenderer = GetComponent<LineRenderer>();
        if (firePoint == null)
        {
            Debug.LogWarning($"FirePoint not assigned on {gameObject.name}. Defaulting to transform.");
            firePoint = transform;
        }
        SetupLineRenderer();
    }

    protected abstract void SetupLineRenderer();
    public abstract void UpdateTurret(Vector3 playerPos, float deltaTime);
    protected abstract void Fire();

    public void SetTurretActive(bool active)
    {
        canFire = active;
    }

    protected void DrawCone(Vector3 origin, Vector3 direction, float range, float angle, int segments = 20)
    {
        rangeRenderer.positionCount = segments + 2;
        rangeRenderer.SetPosition(0, origin);

        float halfAngle = angle * 0.5f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfAngle, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * direction;

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = (angle / segments) * i;
            Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
            Vector3 point = origin + (rotation * leftRayDirection).normalized * range;
            rangeRenderer.SetPosition(i + 1, point);
        }
    }

    protected void DrawLine(Vector3 origin, Vector3 direction, float range)
    {
        rangeRenderer.positionCount = 2;
        rangeRenderer.SetPosition(0, origin);
        rangeRenderer.SetPosition(1, origin + direction.normalized * range);
    }
}
