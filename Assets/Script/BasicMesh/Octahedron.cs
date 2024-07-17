using UnityEngine;

namespace Script.BasicMesh
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Octahedron : MonoBehaviour
    {
        [SerializeField]
        private Material material;

        void Start()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            Mesh mesh = new Mesh();
            meshFilter.mesh = mesh;

            // Define vertices for an octahedron with edge length 1
            Vector3[] vertices = {
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, -1, 0),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, -1)
            };

            // Define triangles (each face is made up of two triangles)
            int[] triangles = {
                0, 2, 4, // Front top right
                0, 4, 3, // Front bottom right
                0, 3, 5, // Back bottom right
                0, 5, 2, // Back top right
                1, 4, 2, // Front top left
                1, 3, 4, // Front bottom left
                1, 5, 3, // Back bottom left
                1, 2, 5  // Back top left
            };

            // Define UVs
            Vector2[] uvs = {
                new Vector2(0.5f, 0.0f),
                new Vector2(0.5f, 1.0f),
                new Vector2(0.0f, 0.5f),
                new Vector2(1.0f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f)
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();

            // Assign material
            if (material != null)
            {
                meshRenderer.material = material;
            }
            else
            {
                Debug.LogWarning("No material assigned to the octahedron.");
            }
        }
    }
}