using UnityEngine;
using System;

public class StereoController : MonoBehaviour
{
    public Transform leftEye;
    public Transform rightEye;
    public Transform convergencePoint;
    public float ipd = 0.064f;
    public bool useToeIn = true;

    public ProjectionPlane projectionPlane;
    public bool offAxisProjection = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(leftEye != null)
        {
            leftEye.localPosition = new Vector3(-ipd / 2, 0, 0);
            leftEye.localRotation = Quaternion.identity;
            ApplyOffAxisProjection(leftEye.GetComponent<Camera>());

        }
        if(rightEye != null)
        {
            rightEye.localPosition = new Vector3(ipd / 2, 0, 0);
            rightEye.localRotation = Quaternion.identity;
            ApplyOffAxisProjection(rightEye.GetComponent<Camera>());
        }

        if(useToeIn && convergencePoint != null)
        {
            leftEye.LookAt(convergencePoint);
            rightEye.LookAt(convergencePoint);
        }
        else
        {
            if(leftEye != null) 
            {
            leftEye.localRotation = Quaternion.identity;
            }
            if(rightEye != null) rightEye.localRotation = Quaternion.identity;
        }

    }

    void ApplyOffAxisProjection(Camera camera)
    {
        if(!offAxisProjection)
        {
            camera.ResetProjectionMatrix();
            camera.ResetWorldToCameraMatrix();
            return;
        }

        if(!projectionPlane)
            return;

        camera.projectionMatrix = GetProjectionMatrix(projectionPlane, camera);

        var planeWorldMatrix = projectionPlane.M;
        
        var relativePlaneRotationMatrix = Matrix4x4.Rotate(
           Quaternion.Inverse(transform.rotation) * projectionPlane.transform.rotation);
           
        var cameraTranslationMatrix = Matrix4x4.Translate(-camera.transform.position);
        
        camera.worldToCameraMatrix = planeWorldMatrix * relativePlaneRotationMatrix * cameraTranslationMatrix;

    }

    private static Matrix4x4 GetProjectionMatrix(ProjectionPlane projectionPlane, Camera camera)
    {
        Vector3 pa = projectionPlane.BottomLeft;
        Vector3 pb = projectionPlane.BottomRight;
        Vector3 pc = projectionPlane.TopLeft;
        
        // Eye position
        Vector3 pe = camera.transform.position;

        // Basis vectors of the screen
        Vector3 vr = projectionPlane.DirRight;
        Vector3 vu = projectionPlane.DirUp;
        Vector3 vn = projectionPlane.DirNormal;


        // 1. Calculate vectors from Eye (pe) to Screen Corners (pa, pb, pc)
        Vector3 va = pa - pe;
        Vector3 vb = pb - pe;
        Vector3 vc = pc - pe;

        // 2. Get distance from eye to screen plane (d)
        // d = - (va . vn)
        float d = -Vector3.Dot(va, vn);

        // Safety check: Avoid dividing by zero if eye is exactly on the plane
        if (Mathf.Abs(d) < 0.001f) d = 0.001f;

        // 3. Get Near and Far clipping planes from the camera settings
        float n = camera.nearClipPlane;
        float f = camera.farClipPlane;

        // 4. Calculate Frustum Bounds (left, right, bottom, top)
        float l = Vector3.Dot(vr, va) * n / d;
        float r = Vector3.Dot(vr, vb) * n / d;
        float b = Vector3.Dot(vu, va) * n / d;
        float t = Vector3.Dot(vu, vc) * n / d;

        // 5. Return the skewed matrix
        return Matrix4x4.Frustum(l, r, b, t, n, f);
    }
}
