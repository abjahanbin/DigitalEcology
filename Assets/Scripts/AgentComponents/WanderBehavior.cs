using UnityEngine;

public class WanderBehavior : AgentBehavior
{
    private Vector3 wanderTarget;

    [Header("Debug Settings")]
    public bool showDebugLines = true;
    public Color lineColor = Color.magenta;
    public float lineLength = 2f;
    [Range(3, 32f)] public int segments = 16;


    void Start()
    {
        wanderTarget = Random.insideUnitSphere;
        wanderTarget.y = 0;
        wanderTarget = wanderTarget.normalized * manager.wanderSettings.circleRadius;
    }

    public override Vector3 CalculateForce(AgentBrain agent)
    {
        Vector3 jitter = new Vector3(
            Random.Range(-1f, 1f),
            0,
            Random.Range(-1f, 1f)
        ) * manager.wanderSettings.jitter;

        wanderTarget += jitter;
        wanderTarget = wanderTarget.normalized * manager.wanderSettings.circleRadius;

        Vector3 circleCenter = transform.position + transform.forward * manager.wanderSettings.circleDistance;
        Vector3 targetWorld = circleCenter + wanderTarget;
        Vector3 force = (targetWorld - transform.position).normalized * manager.wanderSettings.weight;

        if (showDebugLines)
        {
            DebugDrawUtility.DrawCircle(circleCenter, manager.wanderSettings.circleRadius, lineColor, segments);
            DebugDrawUtility.DrawLabeledLine(transform.position, transform.position + force.normalized * lineLength, lineColor);
        }

        return force;
    }

}