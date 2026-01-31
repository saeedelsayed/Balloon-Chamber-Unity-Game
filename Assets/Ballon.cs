using UnityEngine;

public class Ballon : MonoBehaviour
{
    public Gradient ColorGradient;
    public GameObject body;

    public float speed = 2.0f;
    private MeshRenderer bodyMeshRenderer;

    private GameObject ballonString;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballonString = transform.Find("String")?.gameObject;

        rb = GetComponent<Rigidbody>();

        if (body != null)
        {
            bodyMeshRenderer = body.GetComponent<MeshRenderer>();
            if (bodyMeshRenderer != null)
            {
                setRandomColor();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        if(rb != null)
        {
            rb.AddForce(Vector3.up * speed, ForceMode.Force);
        }
    }
    void setRandomColor()
    {
    float randomT = Random.Range(0f, 1f);
    Color randomColor = ColorGradient.Evaluate(randomT);
    bodyMeshRenderer.material.color = randomColor;

    }

    public void pop()
    {
        if(body != null)
        {
            body.SetActive(false);
        }
        if(rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = true;
        }
        Destroy(gameObject, 3f);
    }
}
