using System;
using Unity.Mathematics;
using UnityEngine;

namespace Script.BasicMesh
{
    public class Cube : MonoBehaviour
    {
        [SerializeField] private Material material;
        private Mesh _mesh;
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;

        private float _testSpace = 0;

        void Start()
        {
            _mesh = new Mesh();
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _meshRenderer.material = material;
            _meshFilter.mesh = _mesh;

            // Set initial mesh data
            SetVertices();
            SetTriangles();
            SetUVs();
            _mesh.RecalculateNormals();
        }

        private void Update()
        {
            _testSpace = math.sin(Time.time) * 0.5f;
            SetVertices();
            _mesh.RecalculateNormals();
        }

        private void SetVertices()
        {
            var vertices = new Vector3[]
            {
                // Front face
                new(-1 + _testSpace, -1 + _testSpace, 1 + _testSpace), // 0
                new(1, -1, 1), // 1
                new(1, 1, 1), // 2
                new(-1, 1, 1), // 3

                // Back face
                new(-1, -1, -1), // 4
                new(1, -1, -1), // 5
                new(1, 1, -1), // 6
                new(-1, 1, -1), // 7

                // Top face
                new(-1, 1, 1), // 8
                new(1, 1, 1), // 9
                new(1, 1, -1), // 10
                new(-1, 1, -1), // 11

                // Bottom face
                new(-1 + _testSpace, -1 + _testSpace, 1 + _testSpace), // 12
                new(1, -1, 1), // 13
                new(1, -1, -1), // 14
                new(-1, -1, -1), // 15

                // Left face
                new(-1 + _testSpace, -1 + _testSpace, 1 + _testSpace), // 16
                new(-1, 1, 1), // 17
                new(-1, 1, -1), // 18
                new(-1, -1, -1), // 19

                // Right face
                new(1, -1, 1), // 20
                new(1, 1, 1), // 21
                new(1, 1, -1), // 22
                new(1, -1, -1) // 23
            };
            _mesh.vertices = vertices;
        }

        private void SetTriangles()
        {
            var triangles = new[]
            {
                // Front face
                0, 2, 1,
                0, 3, 2,
                // Back face
                4, 5, 6,
                4, 6, 7,
                // Top face
                8, 10, 9,
                8, 11, 10,
                // Bottom face
                12, 13, 14,
                12, 14, 15,
                // Left face
                16, 18, 17,
                16, 19, 18,
                // Right face
                20, 21, 22,
                20, 22, 23
            };
            _mesh.triangles = triangles;
        }

        private void SetUVs()
        {
            var uvs = new Vector2[]
            {
                new (0, 0),
                new (1, 0),
                new (1, 1),
                new (0, 1),
                
                new (0, 0),
                new (1, 0),
                new (1, 1),
                new (0, 1),
                
                new (0, 0),
                new (1, 0),
                new (1, 1),
                new (0, 1),
                
                new (0, 0),
                new (1, 0),
                new (1, 1),
                new (0, 1),
                
                new (0, 0),
                new (1, 0),
                new (1, 1),
                new (0, 1),
                
                new (0, 0),
                new (1, 0),
                new (1, 1),
                new (0, 1)
            };
            _mesh.uv = uvs;
        }
    }
}
