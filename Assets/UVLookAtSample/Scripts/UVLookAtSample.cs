using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mesh))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class UVLookAtSample : MonoBehaviour {

    private const float _quadMeshSizeW = 2;
    private const float _quadMeshSizeH = 2;

    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    void Awake() {
        _mesh = new Mesh();
        _meshFilter = this.GetComponent<MeshFilter>();
        _meshRenderer = this.GetComponent<MeshRenderer>();
    }

    void Start() {
        BuildMesh();
    }

    private void BuildMesh() {
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var uv0s = new List<Vector2>();

        for (var w = 0; w < 2; w++) {
            for (var h = 0; h < 2; h++) {
                vertices.Add(this.transform.TransformPoint(new Vector3((w * 2 - 1) * _quadMeshSizeW / 2f, (h * 2 - 1) * _quadMeshSizeH / 2f, 0f)));
                uv0s.Add(new Vector2(w, h));
            }
        }
        triangles.AddRange(new int[] {0, 1, 3, 2, 0, 3});
        var normal = Vector3.Cross(vertices[0] - vertices[2], vertices[1] - vertices[0]).normalized;

        _mesh.SetVertices(vertices);
        _mesh.SetIndices(triangles.ToArray(), MeshTopology.Triangles, 0);
        _mesh.SetUVs(0, uv0s);
        _mesh.SetNormals(new List<Vector3>() {
            normal, normal, normal, normal
        });
        _meshFilter.sharedMesh = _mesh;

        _meshRenderer.material.SetVector("_UpDirectionOnTex", Vector3.up);
        _meshRenderer.material.SetVector("_UVOffsetScale", Vector3.one * 0.15f);
        _meshRenderer.material.GetTexture("_MainTex").wrapMode = TextureWrapMode.Clamp;
    }

    void Update() {
        _meshRenderer.material.SetVector("_UserPosition", Camera.main.transform.position);
    }
}
