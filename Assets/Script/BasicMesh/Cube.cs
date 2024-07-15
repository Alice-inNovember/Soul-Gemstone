using UnityEngine;

namespace Script.BasicMesh
{
    public class Cube : MonoBehaviour
    {
        [SerializeField] private Material material;

        void Start()
        {
            // 정점 정의
            Vector3[] vertices = new Vector3[]
        {
            // Front face
            new Vector3(-1, -1, 1), // 0
            new Vector3(1, -1, 1),  // 1
            new Vector3(1, 1, 1),   // 2
            new Vector3(-1, 1, 1),  // 3

            // Back face
            new Vector3(-1, -1, -1), // 4
            new Vector3(1, -1, -1),  // 5
            new Vector3(1, 1, -1),   // 6
            new Vector3(-1, 1, -1),  // 7

            // Top face
            new Vector3(-1, 1, 1),   // 8
            new Vector3(1, 1, 1),    // 9
            new Vector3(1, 1, -1),   // 10
            new Vector3(-1, 1, -1),  // 11

            // Bottom face
            new Vector3(-1, -1, 1),  // 12
            new Vector3(1, -1, 1),   // 13
            new Vector3(1, -1, -1),  // 14
            new Vector3(-1, -1, -1), // 15

            // Left face
            new Vector3(-1, -1, 1),  // 16
            new Vector3(-1, 1, 1),   // 17
            new Vector3(-1, 1, -1),  // 18
            new Vector3(-1, -1, -1), // 19

            // Right face
            new Vector3(1, -1, 1),   // 20
            new Vector3(1, 1, 1),    // 21
            new Vector3(1, 1, -1),   // 22
            new Vector3(1, -1, -1)   // 23
        };

        // 삼각형 정의 (정점의 인덱스, 반시계 방향)
        int[] triangles = new int[]
        {
            // Front face
            0, 1, 2,
            0, 2, 3,

            // Back face
            4, 6, 5,
            4, 7, 6,

            // Top face
            8, 9, 10,
            8, 10, 11,

            // Bottom face
            12, 14, 13,
            12, 15, 14,

            // Left face
            16, 17, 18,
            16, 18, 19,

            // Right face
            20, 22, 21,
            20, 23, 22
        };

        // UV 좌표 정의
        Vector2[] uvs = new Vector2[]
        {
            // Front face
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),

            // Back face
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),

            // Top face
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),

            // Bottom face
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),

            // Left face
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),

            // Right face
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };

        // Mesh 생성 및 할당
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        // MeshFilter 및 MeshRenderer 추가 및 설정
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshFilter.mesh = mesh;

            // 기본 Material 할당 (필요시 Material을 변경할 수 있습니다)
            meshRenderer.material = material;
        }
    }
}
