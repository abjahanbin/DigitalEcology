using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AgentBrain : MonoBehaviour
{
    public AgentManager manager;

    public float maxSpeed = 5f;
    public float maxForce = 10f;

    private List<AgentBehavior> behaviors;
    private Vector3 velocity;
    private float startingY;

    private Vector3 originalScale;
    private Vector3 previousVelocity;
    private Vector3 currentScale = Vector3.one;

    private Material originalMaterial;
    private Renderer rend;


    void Awake()
    {
        if (manager == null)
        {
            manager = Resources.Load<AgentManager>("GlobalAgentManager");
        }

        // Automatically assign manager to all AgentBehaviors
        foreach (var behavior in GetComponents<AgentBehavior>())
        {
            behavior.SetManager(manager);
        }
    }

    void Start()
    {
        behaviors = GetComponents<AgentBehavior>().ToList();
        
        startingY = transform.position.y;
        originalScale = transform.localScale;

        rend = GetComponentInChildren<Renderer>();
        originalMaterial = rend.material;
    }

    void Update()
    {
        Vector3 steering = Vector3.zero;
        Vector3 containment = CalculateContainmentForce();
        AgentBehavior dominantBehavior = null;
        float maxForceMagnitude = 0f;

        foreach (var behavior in behaviors)
        {
            Vector3 force = behavior.CalculateForce(this);
            steering += force;

            if (force.magnitude > maxForceMagnitude)
            {
                maxForceMagnitude = force.magnitude;
                dominantBehavior = behavior;
            }
        }

        // Add obstacle avoidance
        Vector3 avoidance = CalculateObstacleAvoidance();
        steering += avoidance * manager.obstacleAvoidanceSettings.weight;
        steering += containment * manager.containmentSettings.weight;

        steering = Vector3.ClampMagnitude(steering, manager.movementSettings.maxForce);

        // Calculate alignment between velocity and steering
        float alignment = Vector3.Dot(velocity.normalized, steering.normalized);
        alignment = (alignment + 1f) / 2f; // remap from [-1, 1] to [0, 1]

        // Use alignment to control speed
        float minSpeed = manager.movementSettings.maxSpeed * manager.movementSettings.minSpeedFactor;
        float adjustedSpeed = Mathf.Lerp(minSpeed, manager.movementSettings.maxSpeed, alignment);

        // Apply steering and update velocity
        velocity = Vector3.ClampMagnitude(velocity + steering, adjustedSpeed);

        velocity.y = 0;


        transform.position += velocity * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, startingY, transform.position.z);

        if (velocity != Vector3.zero)
        {
            Vector3 flatDirection = new Vector3(velocity.x, 0, velocity.z);
            transform.rotation = Quaternion.LookRotation(flatDirection);
        }

        // --- Stretch Based on Acceleration ---
        float speedChange = Mathf.Abs(velocity.magnitude - previousVelocity.magnitude);
        float normalizedSpeedChange = Mathf.Clamp01(speedChange * manager.stretchVisualSettings.stretchSensitivity);

        float stretchZ = Mathf.Lerp(1f, manager.stretchVisualSettings.stretchMultiplier, normalizedSpeedChange);
        Vector3 targetScale = new Vector3(
            originalScale.x,
            originalScale.y,
            originalScale.z * stretchZ
        );

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * manager.stretchVisualSettings.stretchSpeed);
        
        previousVelocity = velocity;

        if (dominantBehavior != null && dominantBehavior.behaviorMaterial != null)
        {
            ApplyMaterial(dominantBehavior.behaviorMaterial);
        }
        else
        {
            ApplyMaterial(originalMaterial); // fallback
        }

    }

    public Vector3 GetVelocity() => velocity;

    private Vector3 CalculateObstacleAvoidance()
    {
        float feelerLength = manager.obstacleAvoidanceSettings.feelerLength;
        LayerMask mask = manager.obstacleAvoidanceSettings.obstacleMask;

        Vector3[] directions =
        {
        transform.forward,
        Quaternion.Euler(0, -30, 0) * transform.forward, // left feeler
        Quaternion.Euler(0, 30, 0) * transform.forward   // right feeler
    };

        Vector3 avoidance = Vector3.zero;
        foreach (var dir in directions)
        {
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, feelerLength, mask))
            {
                Vector3 away = Vector3.Reflect(dir, hit.normal);
                avoidance += away;
            }
        }

        return avoidance.normalized;
    }

    private Vector3 CalculateContainmentForce()
    {
        if (Camera.main == null) return Vector3.zero;

        Vector3 force = Vector3.zero;
        float margin = manager.containmentSettings.margin;

        // Project agent position into viewport (0 to 1 range)
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        // Check against bounds with margin
        if (viewportPos.x < 0f + margin) force.x = 1;
        else if (viewportPos.x > 1f - margin) force.x = -1;

        if (viewportPos.y < 0f + margin) force.z = 1;
        else if (viewportPos.y > 1f - margin) force.z = -1;

        return force.normalized;
    }

    private void ApplyMaterial(Material mat)
    {
        if (rend != null && rend.sharedMaterial != mat)
        {
            rend.sharedMaterial = mat;
        }
    }

}