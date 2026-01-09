using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    [Header("Agent Spawning")]
    public GameObject agentPrefab;
    public int agentCount = 10;
    public float spawnRadius = 10f;

    [Header("Randomization")]
    public bool randomizeRotation = true;

    private bool parentToSpawner = true;
    public bool randomizeScale = true;
    
    [Tooltip("Max multiplier applied to original scale. Final scale will be between 1x and this value.")]
    [Range(1f, 5f)]
    public float maxScaleMultiplier = 1.5f;

    [Header("Spawn Control")]
    [Tooltip("If false, agents spawn one-by-one over time.")]
    public bool spawnOnStart = true;

    [Tooltip("Delay between each agent spawn when spawnOnStart is false.")]
    [Range(0.01f, 5f)]
    public float spawnInterval = 0.2f;

    void Start()
    {
        if (spawnOnStart)
        {
            SpawnAgents();
        }
        else
        {
            StartCoroutine(SpawnAgentsOverTime());
        }
    }

    public void SpawnAgents()
    {
        for (int i = 0; i < agentCount; i++)
        {
            SpawnSingleAgent();
        }
    }

    private void SpawnSingleAgent()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomCircle.x, transform.position.y, randomCircle.y);

        Quaternion rotation = randomizeRotation
            ? Quaternion.Euler(0, Random.Range(0f, 360f), 0)
            : Quaternion.identity;

        GameObject agent = Instantiate(agentPrefab, spawnPosition, rotation);

        if (randomizeScale)
        {
            float scaleFactor = Random.Range(1f, maxScaleMultiplier);
            agent.transform.localScale *= scaleFactor;
        }

        if (parentToSpawner)
        {
            agent.transform.parent = this.transform;
        }
    }

    private System.Collections.IEnumerator SpawnAgentsOverTime()
    {
        for (int i = 0; i < agentCount; i++)
        {
            SpawnSingleAgent();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void RespawnAgents()
    {
        // Delete old agents if they were parented
        if (parentToSpawner)
        {
            var children = new List<GameObject>();
            foreach (Transform child in transform)
            {
                children.Add(child.gameObject);
            }

            foreach (GameObject go in children)
            {
                DestroyImmediate(go);
            }
        }

        // Spawn based on current setting
        if (spawnOnStart)
        {
            for (int i = 0; i < agentCount; i++)
            {
                SpawnSingleAgent();
            }
        }
        else
        {
            StartCoroutine(SpawnAgentsOverTime());
        }
    }
}