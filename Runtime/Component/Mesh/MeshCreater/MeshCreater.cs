using System;
using UnityEngine;

namespace MeshUtils
{
    public class MeshCreater : IMeshCreater
    {
        const string DEFAULT_GAMEOBJECT_NAME = "meshCreadted";

        Vector3[] vertices;
        int[] triangles;
        Vector2[] uv;
        Material material;

        public MeshCreater(Vector3[] vertices, int[] triangles, Vector2[] uv, Material material)
        {
            this.vertices = vertices;
            this.triangles = triangles;
            this.uv = uv;
            this.material = material;
        }

        GameObject IMeshCreater.CreatMeshGameObject()
        {
            var go = new GameObject(DEFAULT_GAMEOBJECT_NAME);
            var meshFilter = go.AddComponent<MeshFilter>();
            var meshRenderer = go.AddComponent<MeshRenderer>();
            var mesh = meshFilter.mesh;


            // ��������Ķ����������
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            // ���������UV
            mesh.uv = uv;

            // �����������Ⱦ������
            meshRenderer.material = material;

            return go;
        }

        void IDisposable.Dispose()
        {
            vertices = null;
            triangles = null; 
            uv = null; 
            material = null;
        }
    }

}