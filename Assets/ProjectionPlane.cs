using UnityEngine;

public class ProjectionPlane : MonoBehaviour
{
    public Vector3 TopLeft { get; private set; }
    public Vector3 TopRight { get; private set; }
    public Vector3 BottomLeft { get; private set; }
    public Vector3 BottomRight { get; private set; }
    
    public Vector3 DirRight { get; private set; }
    public Vector3 DirUp { get; private set; }
    public Vector3 DirNormal { get; private set; }
    public Matrix4x4 M { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tl = new Vector3(-0.5f, 0.5f, 0);
        Vector3 tr = new Vector3(0.5f, 0.5f, 0);
        Vector3 bl = new Vector3(-0.5f, -0.5f, 0);
        Vector3 br = new Vector3(0.5f, -0.5f, 0);

        TopLeft = transform.TransformPoint(tl);
        TopRight = transform.TransformPoint(tr);
        BottomLeft = transform.TransformPoint(bl);
        BottomRight = transform.TransformPoint(br);

        DirRight = (BottomRight - BottomLeft).normalized;
        DirUp = (TopLeft - BottomLeft).normalized;
        DirNormal = -transform.forward;

        M = transform.localToWorldMatrix;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(TopLeft, TopRight);
        Gizmos.DrawLine(TopRight, BottomRight);
        Gizmos.DrawLine(BottomRight, BottomLeft);
        Gizmos.DrawLine(BottomLeft, TopLeft);
    }
}
