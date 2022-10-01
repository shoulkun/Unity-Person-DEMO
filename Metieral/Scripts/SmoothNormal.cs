using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SmoothNormal : MonoBehaviour
{
    void Awake()
    {
        MeshFilter[] chilesMeshFilter = gameObject.GetComponentsInChildren<MeshFilter>();
        SkinnedMeshRenderer[] chilesSkinnedMeshRenderer = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (var chile in chilesMeshFilter)
        {
            changeNormal<MeshFilter>(chile);
        }
        foreach (var chile in chilesSkinnedMeshRenderer)
        {
            changeNormal<SkinnedMeshRenderer>(chile);
        }
    }

    void changeNormal<T>(T chile) where T:Component
    {
        if (chile.GetComponent<MeshFilter>())
        {
            Mesh tempMesh = (Mesh)Instantiate(chile.GetComponent<MeshFilter>().sharedMesh);
            tempMesh=MeshNormalAverage(tempMesh);
            chile.GetComponent<MeshFilter>().sharedMesh = tempMesh;
        }
        if (chile.GetComponent<SkinnedMeshRenderer>())
        {
            Mesh tempMesh = (Mesh)Instantiate(chile.GetComponent<SkinnedMeshRenderer>().sharedMesh);
            tempMesh = MeshNormalAverage(tempMesh);
            chile.GetComponent<SkinnedMeshRenderer>().sharedMesh = tempMesh;
        }
    }

    Mesh MeshNormalAverage(Mesh mesh)
    {
        Dictionary<Vector3, List<int>> map = new Dictionary<Vector3, List<int>>();
        for (int v = 0; v < mesh.vertexCount; ++v)
        {
            if (!map.ContainsKey(mesh.vertices[v]))
            {
                map.Add(mesh.vertices[v], new List<int>());
            }
            map[mesh.vertices[v]].Add(v);
        }
        Vector3[] normals = mesh.normals;
        Vector3 normal;
        foreach(var p in map)
        {
            normal = Vector3.zero;
            foreach (var n in p.Value)
            {
                normal += mesh.normals[n];
            }
            //normal /= p.Value.Count;
            normal = normal.normalized;
            foreach (var n in p.Value)
            {
                normals[n] = normal;
            }
        }
        var tangents = new Vector4[mesh.vertexCount];
        for (var j = 0; j < mesh.vertexCount; j++)
        {
            tangents[j] = new Vector4(normals[j].x, normals[j].y, normals[j].z, 0);
        }
        mesh.tangents= tangents;
        return mesh;
    }
}