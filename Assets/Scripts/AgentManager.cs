using UnityEngine;

[System.Serializable]
public class MovementSettings
{
    [Range(0.1f, 50f)] public float maxSpeed = 10f;
    [Range(0.1f, 2f)] public float maxForce = 0.1f;
    [Range(0f, 1f)] public float minSpeedFactor = 0.5f;
}

[System.Serializable]
public class StretchVisualSettings
{
    [Range(1f, 5f)] public float stretchMultiplier = 1.5f;
    [Range(0f, 100f)] public float stretchSpeed = 50f;
    [Range(0.1f, 10f)]public float stretchSensitivity = 1f;
}

[System.Serializable]
public class ContainmentSettings
{
    [Range(0f, 10f)] public float weight = 3f;
    [Range(0f, 0.2f)] public float margin = 0.1f;
}

[System.Serializable]
public class ObstacleAvoidanceSettings
{
    [Range(0f, 10f)] public float weight = 1f;
    [Range(0f, 10f)] public float feelerLength = 2f;
    public LayerMask obstacleMask;
}

[System.Serializable]
public class WanderSettings
{
    [Range(0f, 10f)] public float weight = 1f;
    [Range(0f, 50f)] public float circleDistance = 10f;
    [Range(0f, 50f)] public float circleRadius = 10f;
    [Range(0f, 1f)] public float jitter = 0.2f;
}

[System.Serializable]
public class AlignSettings
{
    [Range(0f, 10f)] public float weight = 1f;
    [Range(0f, 50f)] public float neighborRadius = 10f;
}

[System.Serializable]
public class CohesionSettings
{
    [Range(0f, 10f)] public float weight = 1f;
    [Range(0f, 50f)] public float neighborRadius = 10f;
}

[System.Serializable]
public class SeparationSettings
{
    [Range(0f, 10f)] public float weight = 1f;
    [Range(0f, 50f)] public float radius = 10f;
}

[System.Serializable]
public class AvoidSettings
{
    [Range(0f, 10f)] public float weight = 1f;
    [Range(0f, 50f)] public float radius = 10f;
}

[System.Serializable]
public class AttractSettings
{
    [Range(0f, 10f)] public float weight = 1f;
    [Range(0f, 50f)] public float radius = 2f;
}

[System.Serializable]
public class FleeSettings
{
    [Range(0f, 10f)] public float weight = 1f;
    [Range(0f, 100f)] public float fleeRadius = 10f;
    public LayerMask fleeLayer;
}

[System.Serializable]
public class SeekSettings
{
    [Range(0f, 10f)] public float weight = 1f;
    [Range(0f, 100f)] public float seekRadius = 10f;
    public LayerMask seekLayer;
}

[System.Serializable]
public class IdleSettings
{
    [Range(0f, 10f)] public float weight = 1f;
    [Range(0f, 0.1f)] public float restChance = 0.005f; // Chance per frame
    [Range(0.1f, 5f)] public float restDuration = 2f; // Seconds
}


[CreateAssetMenu(menuName = "Agent/Agent Manager")]
public class AgentManager : ScriptableObject
{
    [Header("General Movement")]
    public MovementSettings movementSettings;

    [Space(10)]
    [Header("Stretch Visuals")]
    public StretchVisualSettings stretchVisualSettings;

    [Space(10)]
    [Header("Camera Containment")]
    public ContainmentSettings containmentSettings;

    [Space(10)]
    [Header("Obstacle Avoidance")]
    public ObstacleAvoidanceSettings obstacleAvoidanceSettings;

    [Space(10)]
    [Header("Wander")]
    public WanderSettings wanderSettings;

    [Space(10)]
    [Header("Align")]
    public AlignSettings alignSettings;

    [Space(10)]
    [Header("Cohesion")]
    public CohesionSettings cohesionSettings;

    [Space(10)]
    [Header("Separation")]
    public SeparationSettings separationSettings;

    [Space(10)]
    [Header("Avoid")]
    public AvoidSettings avoidSettings;

    [Space(10)]
    [Header("Attract")]
    public AttractSettings attractSettings = new AttractSettings();

    [Space(10)]
    [Header("Flee")]
    public FleeSettings fleeSettings;

    [Space(10)]
    [Header("Seek")]
    public SeekSettings seekSettings;

    [Space(10)]
    [Header("Idle")]
    public IdleSettings idleSettings = new IdleSettings();
}