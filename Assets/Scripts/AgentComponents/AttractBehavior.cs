using UnityEngine;

public class AttractBehavior : AgentBehavior
{
    [Header("Debug Settings")]
    public bool showDebugLines = true;
    public Color lineColor = Color.magenta;
    public float lineLength = 2f;
    [Range(3, 32f)] public int segments = 16;

    public override Vector3 CalculateForce(AgentBrain agent)
    {
        Vector3 attractForce = Vector3.zero;
        float radius = manager.attractSettings.radius;

        Collider[] neighbors = Physics.OverlapSphere(transform.position, radius);

        foreach (var neighbor in neighbors)
        {
            if (neighbor.transform == transform) continue;
            if (!neighbor.CompareTag(AgentTags.Agent)) continue;

            Vector3 toward = neighbor.transform.position - transform.position;
            attractForce += toward.normalized / Mathf.Max(toward.magnitude, 0.1f);
        }

        Vector3 force = attractForce.normalized * manager.attractSettings.weight;

        if (showDebugLines)
        {
            DebugDrawUtility.DrawCircle(transform.position, radius, lineColor, segments);
            DebugDrawUtility.DrawLabeledLine(transform.position, transform.position + force.normalized * lineLength, lineColor);
        }

        return force;
    }
}