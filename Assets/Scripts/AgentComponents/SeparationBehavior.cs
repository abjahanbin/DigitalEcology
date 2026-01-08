using UnityEngine;

public class SeparationBehavior : AgentBehavior
{
    [Header("Debug Settings")]
    public bool showDebugLines = true;
    public Color lineColor = Color.white;
    public float lineLength = 2f;
    [Range(3, 32f)] public int segments = 16;

    public override Vector3 CalculateForce(AgentBrain agent)
    {
        Vector3 separationForce = Vector3.zero;
        int count = 0;

        float radius = manager.separationSettings.radius;

        Collider[] neighbors = Physics.OverlapSphere(transform.position, radius);
        foreach (var neighbor in neighbors)
        {
            if (neighbor.gameObject == gameObject) continue;

            Vector3 away = transform.position - neighbor.transform.position;
            separationForce += away.normalized / Mathf.Max(away.magnitude, 0.1f);
            count++;
        }

        if (count == 0) return Vector3.zero;

        Vector3 force = separationForce.normalized * manager.separationSettings.weight;

        if (showDebugLines)
        {
            DebugDrawUtility.DrawCircle(transform.position, radius, lineColor, segments);
            DebugDrawUtility.DrawLabeledLine(transform.position, transform.position + force.normalized * lineLength, lineColor);
        }

        return force;
    }
}