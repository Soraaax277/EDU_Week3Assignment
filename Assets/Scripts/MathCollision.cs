using UnityEngine;

public static class MathCollision
{
    public static bool CheckCircleCollision(Vector3 pos1, float r1, Vector3 pos2, float r2)
    {
        float distanceSq = (pos1 - pos2).sqrMagnitude;
        float radiusSum = r1 + r2;
        return distanceSq <= (radiusSum * radiusSum);
    }

    public static bool IsPointInCone(Vector3 point, Vector3 coneOrigin, Vector3 coneDirection, float maxDistance, float coneAngle)
    {
        Vector3 directionToPoint = point - coneOrigin;
        float distance = directionToPoint.magnitude;

        if (distance > maxDistance) return false;

        directionToPoint.Normalize();
        float dot = Vector3.Dot(coneDirection.normalized, directionToPoint);
        
        float angleThreshold = Mathf.Cos(coneAngle * 0.5f * Mathf.Deg2Rad);
        
        return dot >= angleThreshold;
    }

    public static bool IsPointNearLine(Vector3 point, Vector3 lineStart, Vector3 lineDirection, float maxDistance, float thickness)
    {
        Vector3 p1 = new Vector3(lineStart.x, 0, lineStart.z);
        Vector3 p2 = new Vector3(point.x, 0, point.z);
        Vector3 dir = new Vector3(lineDirection.x, 0, lineDirection.z).normalized;

        Vector3 lineToPoint = p2 - p1;
        float projection = Vector3.Dot(lineToPoint, dir);

        if (projection < 0 || projection > maxDistance) return false;

        Vector3 closestPoint = p1 + dir * projection;
        return (p2 - closestPoint).sqrMagnitude <= (thickness * thickness);
    }
}
