using UnityEngine;

public class FleeBehavior : AgentBehavior
{
    [Header("Debug Settings")]
    public bool showDebugLines = true;
    public Color lineColor = Color.red;
    public float lineLength = 2f;
    [Range(3, 32f)] public int segments = 16;

    public override Vector3 CalculateForce(AgentBrain agent)
    {
        var settings = manager.fleeSettings;

        Collider[] hits = Physics.OverlapSphere(transform.position, settings.fleeRadius, settings.fleeLayer);
        if (hits.Length == 0) return Vector3.zero;

        Transform closestThreat = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            float dist = (hit.transform.position - transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closestThreat = hit.transform;
            }
        }

        if (closestThreat == null) return Vector3.zero;

        Vector3 desired = (transform.position - closestThreat.position).normalized;
        Vector3 force = desired * settings.weight;

        if (showDebugLines)
        {
            DebugDrawUtility.DrawCircle(transform.position, settings.fleeRadius, lineColor, segments);
            DebugDrawUtility.DrawLabeledLine(transform.position, transform.position + desired * lineLength, lineColor);
        }

        return force;
    }
}