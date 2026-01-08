using UnityEngine;

public class SeekBehavior : AgentBehavior
{
    [Header("Debug Settings")]
    public bool showDebugLines = true;
    public Color lineColor = Color.green;
    public float lineLength = 2f;
    [Range(3, 32f)] public int segments = 16;

    public override Vector3 CalculateForce(AgentBrain agent)
    {
        var settings = manager.seekSettings;

        Collider[] hits = Physics.OverlapSphere(transform.position, settings.seekRadius, settings.seekLayer);
        if (hits.Length == 0) return Vector3.zero;

        Transform closestTarget = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            float dist = (hit.transform.position - transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closestTarget = hit.transform;
            }
        }

        if (closestTarget == null) return Vector3.zero;

        Vector3 desired = (closestTarget.position - transform.position).normalized;
        Vector3 force = desired * settings.weight;

        if (showDebugLines)
        {
            DebugDrawUtility.DrawLabeledLine(transform.position, transform.position + desired * lineLength, lineColor);
            DebugDrawUtility.DrawCircle(transform.position, manager.seekSettings.seekRadius, lineColor, segments);
        }

        return force;
    }
}