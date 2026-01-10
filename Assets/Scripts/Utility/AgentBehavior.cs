using UnityEngine;

public abstract class AgentBehavior : MonoBehaviour
{
    protected AgentManager manager;

    public void SetManager(AgentManager manager)
    {
        this.manager = manager;
    }

    [Header("Material Override")]
    public Material behaviorMaterial;

    public abstract Vector3 CalculateForce(AgentBrain agent);
}