using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 velocity;
    public float radius = 0.2f;
    public bool isActive = true;

    public void UpdateProjectile(float deltaTime)
    {
        if (!isActive) return;
        transform.position += velocity * deltaTime;
    }
}
