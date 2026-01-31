using UnityEngine;
using Tutorial_5;

public class Ballon : MonoBehaviour
{
    public Gradient ColorGradient;
    public GameObject body;

    public float speed = 2.0f;

    private MeshRenderer bodyMeshRenderer;
    private Rigidbody rb;

    private AudioController audioController;
    private bool popped = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Find the AudioController in the scene once
        audioController = FindFirstObjectByType<AudioController>(); // Unity 6+
        // If this line errors, use: audioController = FindObjectOfType<AudioController>();

        if (body != null)
        {
            bodyMeshRenderer = body.GetComponent<MeshRenderer>();
            if (bodyMeshRenderer != null) setRandomColor();
        }
    }

    void Update()
    {
        if (rb != null)
            rb.AddForce(Vector3.up * speed, ForceMode.Force);
    }

    void setRandomColor()
    {
        float randomT = Random.Range(0f, 1f);
        Color randomColor = ColorGradient.Evaluate(randomT);
        bodyMeshRenderer.material.color = randomColor;
    }

    public void pop()
    {
        if (popped) return;
        popped = true;

        // Play sound using clip already set in AudioController
        if (audioController != null)
            audioController.PlayOneShotAt(transform.position);

        if (body != null) body.SetActive(false);

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = true;
        }

        Destroy(gameObject, 3f);
    }
}
