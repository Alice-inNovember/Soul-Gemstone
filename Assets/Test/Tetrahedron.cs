using UnityEngine;

namespace Test
{
	public class Tetrahedron : MonoBehaviour
	{
		[SerializeField] private Material material;
		void Start()
		{
			// 정점 정의
			Vector3[] vertices = new Vector3[]
			{
				new Vector3(1, 1, 1),  // Vertex 0
				new Vector3(-1, -1, 1), // Vertex 1
				new Vector3(-1, 1, -1), // Vertex 2
				new Vector3(1, -1, -1)  // Vertex 3
			};

			// 삼각형 정의 (정점의 인덱스)
			int[] triangles = new int[]
			{
				0, 2, 1, // Face 1
				0, 3, 2, // Face 2
				0, 1, 3, // Face 3
				1, 2, 3  // Face 4
			};

			// UV 좌표 정의
			Vector2[] uvs = new Vector2[]
			{
				new Vector2(0.5f, 1.0f), // Vertex 0
				new Vector2(0.0f, 0.0f), // Vertex 1
				new Vector2(1.0f, 0.0f), // Vertex 2
				new Vector2(0.5f, 0.5f)  // Vertex 3
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
			meshRenderer.material = material;
		}
	}
}