using UnityEngine;

public class ShotgunTurret : BaseTurret
{
    [Header("Shotgun Settings")]
    public int pelletCount = 5;
    public float spreadAngle = 30f;
    public float pelletSpeed = 8f;
    public float detectionRadius = 8f;

    protected override void SetupLineRenderer()
    {
        rangeRenderer.startWidth = 0.1f;
        rangeRenderer.endWidth = 0.1f;

        DrawCone(transform.position, transform.forward, detectionRadius, spreadAngle);
    }

    public override void UpdateTurret(Vector3 playerPos, float deltaTime)
    {
        if (!canFire) return;

        DrawCone(transform.position, transform.forward, detectionRadius, spreadAngle);

        TrackPlayer(playerPos, deltaTime);

        bool playerInRange = MathCollision.IsPointInCone(playerPos, transform.position, transform.forward, detectionRadius, spreadAngle);

        if (playerInRange)
        {
            fireTimer -= deltaTime;
            if (fireTimer <= 0)
            {
                Fire();
                fireTimer = fireRate;
            }
        }
    }

    protected override void Fire()
    {
        if (projectilePrefab == null) return;

        for (int i = 0; i < pelletCount; i++)
        {
            float angle = Random.Range(-spreadAngle * 0.5f, spreadAngle * 0.5f);
            Vector3 fireDir = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;

            GameObject projObj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(fireDir));
            projObj.SetActive(true);
            Projectile proj = projObj.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.velocity = fireDir * pelletSpeed;
                GameManager.Instance.RegisterProjectile(proj);
            }
        }
        Debug.Log($"{gameObject.name} BLASTED!");
    }
}
