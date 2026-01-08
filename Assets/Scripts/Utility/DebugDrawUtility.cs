using UnityEngine;

public static class DebugDrawUtility
{
    public static void DrawCircle(Vector3 center, float radius, Color color, int segments = 32)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep);
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Debug.DrawLine(prevPoint, nextPoint, color);
            prevPoint = nextPoint;
        }
    }

    public static void DrawLabeledLine(Vector3 start, Vector3 end, Color color, string label = "")
    {
        Debug.DrawLine(start, end, color);
    }

    public static void DrawLabeledPoint(Vector3 position, Color color, string label = "")
    {
        Debug.DrawRay(position + Vector3.up * 0.1f, Vector3.down * 0.2f, color);
    }
}