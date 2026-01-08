using UnityEngine;

public class CohesionBehavior : AgentBehavior
{
    [Header("Debug Settings")]
    public bool showDebugLines = true;
    public Color lineColor = Color.cyan;
    public float lineLength = 2f;
    [Range(3, 32f)] public int segments = 16;

    public override Vector3 CalculateForce(AgentBrain agent)
    {
        Vector3 centerOfMass = Vector3.zero;
        int count = 0;

        float neighborRadius = manager.cohesionSettings.neighborRadius;

        Collider[] neighbors = Physics.OverlapSphere(transform.position, neighborRadius);
        foreach (var neighbor in neighbors)
        {
            if (neighbor.gameObject == gameObject) continue;

            AgentBrain other = neighbor.GetComponent<AgentBrain>();
            if (other != null)
            {
                centerOfMass += neighbor.transform.position;
                count++;
            }
        }

        if (count == 0) return Vector3.zero;

        centerOfMass /= count;
        Vector3 desired = (centerOfMass - transform.position).normalized;
        Vector3 force = desired * manager.cohesionSettings.weight;

        if (showDebugLines)
        {
            DebugDrawUtility.DrawCircle(transform.position, neighborRadius, lineColor, segments);
            DebugDrawUtility.DrawLabeledLine(transform.position, transform.position + force.normalized * lineLength, lineColor);
        }

        return force;
    }
}