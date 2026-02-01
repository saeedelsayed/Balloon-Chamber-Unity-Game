using UnityEngine;

public class StereoController : MonoBehaviour
{
    public Transform leftEye;
    public Transform rightEye;
    public Transform convergencePoint;
    public float ipd = 0.064f;
    public bool useToeIn = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(leftEye != null)
        leftEye.localPosition = new Vector3(-ipd / 2, 0, 0);

        if(rightEye != null)
        rightEye.localPosition = new Vector3(ipd / 2, 0, 0);

        if(useToeIn && convergencePoint != null)
        {
            leftEye.LookAt(convergencePoint);
            rightEye.LookAt(convergencePoint);
        }
        else
        {
            if(leftEye != null) leftEye.localRotation = Quaternion.identity;
            if(rightEye != null) rightEye.localRotation = Quaternion.identity;
        }

    }
}
