using UnityEngine;

public static class RuntimeDebugUtility
{
    public static void DrawLabeledLine(Vector3 start, Vector3 end, Color color, float width = 0.05f, float duration = 1f)
    {
        RuntimeDebugDraw.Instance.DrawLine(start + Vector3.up * 0.01f, end + Vector3.up * 0.01f, color, width, duration);
    }

    public static void DrawCircle(Vector3 center, float radius, Color color, int segments = 32, float width = 0.05f, float duration = 1f)
    {
        RuntimeDebugDraw.Instance.DrawCircle(center + Vector3.up * 0.01f, radius, color, segments, width, duration);
    }

    public static void DrawLabeledPoint(Vector3 point, Color color, string label = "", float width = 0.1f, float duration = 1f)
    {
        RuntimeDebugDraw.Instance.DrawCircle(point + Vector3.up * 0.01f, width, color, 8, width, duration);
    }
}