using UnityEngine;

public class SniperTurret : BaseTurret
{
    [Header("Sniper Settings")]
    public float thickness = 0.5f; 
    public float projectileSpeed = 40f; 
    public float sniperFireRate = 0.5f; 
    public float sniperRotationSpeed = 10f; 

    protected override void Awake()
    {
        base.Awake();
        fireRate = sniperFireRate;
        rotationSpeed = sniperRotationSpeed;
    }

    protected override void SetupLineRenderer()
    {
        rangeRenderer.startWidth = 0.05f;
        rangeRenderer.endWidth = 0.05f;
        DrawLine(transform.position, transform.forward, range);
    }

    public override void UpdateTurret(Vector3 playerPos, float deltaTime)
    {
        if (!canFire) return;

        DrawLine(transform.position, transform.forward, range);
        
        TrackPlayer(playerPos, deltaTime);

        float totalThickness = thickness + 0.5f; 
        bool playerInSights = MathCollision.IsPointNearLine(playerPos, transform.position, transform.forward, range, totalThickness);

        if (playerInSights)
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

        GameObject projObj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projObj.SetActive(true);
        Projectile proj = projObj.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.velocity = transform.forward * projectileSpeed;
            GameManager.Instance.RegisterProjectile(proj);
            Debug.Log($"{gameObject.name} SNIPED!");
        }
    }
}
