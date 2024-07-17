using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ShapeMorphTest : MonoBehaviour
{
    [SerializeField]
    private Material material;

    private Vector3[] octahedronVertices = {
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, -1, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1)
    };

    private int[] octahedronTriangles = {
        0, 2, 4,
        0, 4, 3,
        0, 3, 5,
        0, 5, 2,
        1, 4, 2,
        1, 3, 4,
        1, 5, 3,
        1, 2, 5
    };

    private Vector3[] hexadecagonVertices = {
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, -1, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1),
        new Vector3(0.5f, 0.5f, 0.5f),
        new Vector3(-0.5f, 0.5f, 0.5f),
        new Vector3(-0.5f, -0.5f, 0.5f),
        new Vector3(0.5f, -0.5f, 0.5f),
        new Vector3(0.5f, 0.5f, -0.5f),
        new Vector3(-0.5f, 0.5f, -0.5f),
        new Vector3(-0.5f, -0.5f, -0.5f),
        new Vector3(0.5f, -0.5f, -0.5f)
    };

    private int[] hexadecagonTriangles = {
        0, 6, 10, 0, 10, 2, 0, 2, 6, 0, 2, 4,
        1, 7, 11, 1, 11, 2, 1, 2, 7, 1, 2, 4,
        3, 8, 12, 3, 12, 4, 3, 4, 8, 3, 4, 0,
        5, 9, 13, 5, 13, 4, 5, 4, 9, 5, 4, 3,
        6, 10, 12, 6, 12, 8, 6, 8, 12, 6, 12, 10,
        7, 11, 13, 7, 13, 9, 7, 9, 13, 7, 13, 11
    };

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = material;

        StartCoroutine(MorphShape(meshFilter));
    }

    IEnumerator MorphShape(MeshFilter meshFilter)
    {
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        float duration = 2.0f;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            Vector3[] currentVertices = new Vector3[octahedronVertices.Length];
            for (int i = 0; i < octahedronVertices.Length; i++)
            {
                currentVertices[i] = Vector3.Lerp(octahedronVertices[i], hexadecagonVertices[i], t);
            }

            mesh.Clear();
            mesh.vertices = currentVertices;
            mesh.triangles = octahedronTriangles;
            mesh.RecalculateNormals();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final shape is the hexadecagon
        mesh.Clear();
        mesh.vertices = hexadecagonVertices;
        mesh.triangles = hexadecagonTriangles;
        mesh.RecalculateNormals();
    }
}
