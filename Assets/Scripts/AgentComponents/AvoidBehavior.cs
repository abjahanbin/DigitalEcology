using UnityEngine;

public class AvoidBehavior : AgentBehavior
{
    [Header("Debug Settings")]
    public bool showDebugLines = true;
    public Color lineColor = Color.blue;
    public float lineLength = 2f;
    [Range(3, 32f)] public int segments = 16;

    public override Vector3 CalculateForce(AgentBrain agent)
    {
        Vector3 avoidForce = Vector3.zero;
        float radius = manager.avoidSettings.radius;

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (var hit in hits)
        {
            if (hit.transform == transform) continue;
            if (!hit.CompareTag(AgentTags.Agent)) continue;

            Vector3 away = transform.position - hit.transform.position;
            avoidForce += away.normalized / Mathf.Max(away.magnitude, 0.1f);
        }

        Vector3 force = avoidForce.normalized * manager.avoidSettings.weight;

        if (showDebugLines)
        {
            DebugDrawUtility.DrawCircle(transform.position, radius, lineColor, segments);
            DebugDrawUtility.DrawLabeledLine(transform.position, transform.position + force.normalized * lineLength, lineColor);
        }

        return force;
    }
}