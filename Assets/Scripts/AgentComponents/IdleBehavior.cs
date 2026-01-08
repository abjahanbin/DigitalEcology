using UnityEngine;

public class IdleBehavior : AgentBehavior
{
    private float restTimer = 0f;
    private bool isResting = false;

    [Header("Debug Settings")]
    public bool showDebugLines = true;
    public Color lineColor = Color.gray;
    public float lineLength = 1f;

    public override Vector3 CalculateForce(AgentBrain agent)
    {
        float chance = manager.idleSettings.restChance;
        float duration = manager.idleSettings.restDuration;

        if (!isResting && Random.value < chance)
        {
            isResting = true;
            restTimer = duration;
        }

        if (isResting)
        {
            restTimer -= Time.deltaTime;
            if (restTimer <= 0f)
                isResting = false;

            if (showDebugLines)
            {
                DebugDrawUtility.DrawLabeledLine(transform.position, transform.position + Vector3.up * lineLength, lineColor, "Resting");
            }

            return -agent.GetVelocity().normalized * manager.idleSettings.weight;
        }

        return Vector3.zero;
    }
}