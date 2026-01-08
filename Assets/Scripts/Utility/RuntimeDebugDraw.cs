using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeDebugDraw : MonoBehaviour
{
    private static RuntimeDebugDraw instance;
    public static RuntimeDebugDraw Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("RuntimeDebugDraw");
                instance = go.AddComponent<RuntimeDebugDraw>();
            }
            return instance;
        }
    }

    [SerializeField] private Material lineMaterial;

    private Queue<LineRenderer> linePool = new Queue<LineRenderer>();

    private void Awake()
    {
        if (lineMaterial == null)
        {
            Shader shader = Shader.Find("Sprites/Default");
            lineMaterial = new Material(shader) { color = Color.white };
        }
    }

    // === LINE ===
    public void DrawLine(Vector3 start, Vector3 end, Color color, float width = 0.05f, float duration = 0.1f)
    {
        LineRenderer lr = GetLineRenderer();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startColor = lr.endColor = color;
        lr.startWidth = lr.endWidth = width;

        StartCoroutine(ReleaseAfter(lr, duration));
    }

    // === CIRCLE ===
    public void DrawCircle(Vector3 center, float radius, Color color, int segments = 32, float width = 0.05f, float duration = 0.1f)
    {
        LineRenderer lr = GetLineRenderer();
        lr.positionCount = segments + 1;
        lr.loop = true;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius + center;
            lr.SetPosition(i, pos);
        }

        lr.startColor = lr.endColor = color;
        lr.startWidth = lr.endWidth = width;

        StartCoroutine(ReleaseAfter(lr, duration));
    }

    // === POOLING ===
    private LineRenderer GetLineRenderer()
    {
        LineRenderer lr;

        if (linePool.Count > 0)
        {
            lr = linePool.Dequeue();
            lr.gameObject.SetActive(true);
        }
        else
        {
            GameObject go = new GameObject("DebugLine");
            lr = go.AddComponent<LineRenderer>();
            lr.material = new Material(lineMaterial);
            lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lr.receiveShadows = false;
            lr.useWorldSpace = true;
        }

        return lr;
    }

    private IEnumerator ReleaseAfter(LineRenderer lr, float delay)
    {
        yield return new WaitForSeconds(delay);
        lr.gameObject.SetActive(false);
        linePool.Enqueue(lr);
    }
}