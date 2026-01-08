using UnityEngine;

public class AlignBehavior : AgentBehavior
{
    [Header("Debug Settings")]
    public bool showDebugLines = true;
    public Color lineColor = Color.green;
    public float lineLength = 2f;
    [Range(3, 32f)] public int segments = 16;

    public override Vector3 CalculateForce(AgentBrain agent)
    {
        Vector3 averageDirection = Vector3.zero;
        int count = 0;

        float neighborRadius = manager.alignSettings.neighborRadius;

        Collider[] neighbors = Physics.OverlapSphere(transform.position, neighborRadius);
        foreach (var neighbor in neighbors)
        {
            if (neighbor.gameObject == gameObject) continue;

            AgentBrain otherAgent = neighbor.GetComponent<AgentBrain>();
            if (otherAgent != null)
            {
                averageDirection += otherAgent.GetVelocity().normalized;
                count++;
            }
        }

        if (count == 0) return Vector3.zero;

        averageDirection /= count;
        Vector3 force = averageDirection.normalized * manager.alignSettings.weight;

        if (showDebugLines)
        {
            DebugDrawUtility.DrawCircle(transform.position, neighborRadius, lineColor, segments);
            DebugDrawUtility.DrawLabeledLine(transform.position, transform.position + force.normalized * lineLength, lineColor);
        }

        return force;
    }
}