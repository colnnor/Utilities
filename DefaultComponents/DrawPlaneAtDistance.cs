using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteAlways]
public class DrawPlaneAtDistance : MonoBehaviour
{
    [SerializeField] private DrawPlaneMode drawMode = DrawPlaneMode.Gizmos;
    [SerializeField] private float distance = 5f;
    [SerializeField] private Camera cam;
    [SerializeField] private Material planeMaterial;
    [SerializeField] private MeshRenderer planeRenderer;

    private Camera Cam => cam ?? GetCam();
    private MeshFilter meshFilter;
    private List<Vector3> vertices = new List<Vector3>(4);

    private void Reset()
    {
        GetCam();
    }

    private Camera GetCam()
    {
        cam = GetComponent<Camera>();
        return cam;
    }

    private void Update()
    {
        switch (drawMode)
        {
            case DrawPlaneMode.MeshRenderer:
                DrawPlane();
                break;
            case DrawPlaneMode.Gizmos:
                break;
            case DrawPlaneMode.None:
                if (planeRenderer)
                    planeRenderer.gameObject.SetActive(false);
                break;
            
        }
    }

    [Button]
    void DrawPlane()
    {
        if (!planeRenderer)
        {
            GetPlaneRenderer();
        }
        planeRenderer.gameObject.SetActive(true);
        var localCorners = GetLocalCorners();

        Mesh planeMesh = new Mesh
        {
            vertices = new Vector3[]
            {
                localCorners[0],
                localCorners[1],
                localCorners[2],
                localCorners[3]
            },
            triangles = new int[]
            {
                0, 1, 2,
                0, 2, 3
            }
        };

        planeMesh.RecalculateNormals();
        planeMesh.RecalculateBounds();
        planeMesh.RecalculateTangents();

        if (!meshFilter) meshFilter = planeRenderer.gameObject.GetOrAddComponent<MeshFilter>();
        if (!planeMaterial)
        {
            planeMaterial = Materials.GetURPLit();
        }

        planeRenderer.material = planeMaterial;
        meshFilter.mesh = planeMesh;

        if (planeRenderer.TryGetComponent(out MeshCollider meshCollider))
        {
            meshCollider.sharedMesh = planeMesh;
        }
        else
        {
            meshCollider = planeRenderer.gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = planeMesh;
        }
    }

    private List<Vector3> GetLocalCorners()
    {
        Vector3[] corners = new Vector3[4];
        Vector3[] localCorners = new Vector3[4];
        Cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), distance, Camera.MonoOrStereoscopicEye.Mono, corners);

        for (int i = 0; i < 4; i++)
        {
            localCorners[i] = transform.InverseTransformPoint(Cam.transform.TransformPoint(corners[i]));
        }

        return new List<Vector3>(localCorners);
    }
    
    private void GetPlaneRenderer()
    {
        planeRenderer = GetComponentInChildren<MeshRenderer>();
        if (!planeRenderer)
        {
            GameObject planeObj = new GameObject("Plane");
            planeObj.transform.SetParent(transform);
            planeObj.transform.localPosition = Vector3.zero;
            planeObj.transform.localRotation = Quaternion.identity;
            planeRenderer = planeObj.AddComponent<MeshRenderer>();
            planeObj.AddComponent<MeshFilter>(); // Ensure MeshFilter is added
        }
    }
    
    private void OnDrawGizmos()
    {
        if(drawMode != DrawPlaneMode.Gizmos) return;
        
        var corners = GetLocalCorners();
        Gizmos.color = Color.green;
        for(int i = 0; i < 4; i++)
        {
            Vector3 from = transform.TransformPoint(corners[i]);
            Vector3 to = transform.TransformPoint(corners[(i + 1) % 4]);
            Gizmos.DrawLine(from, to);
        }
    }

    enum DrawPlaneMode
    {
        None,
        Gizmos,
        MeshRenderer
    }
}