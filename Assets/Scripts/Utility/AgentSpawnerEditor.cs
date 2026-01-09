using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AgentSpawner))]
public class AgentSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AgentSpawner spawner = (AgentSpawner)target;
        if (GUILayout.Button("Respawn Agents"))
        {
            spawner.RespawnAgents();
        }
    }
}