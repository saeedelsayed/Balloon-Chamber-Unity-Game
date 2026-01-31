using UnityEngine;

public class spike : MonoBehaviour
{
    private float moveSpeed = 25.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        transform.position += movement * moveSpeed * Time.deltaTime;

        float x = Mathf.Clamp(transform.position.x, -21.86f, 21.95f);
        float z = Mathf.Clamp(transform.position.z, -21.48f, 21.48f);
        float y = transform.position.y;
        transform.position = new Vector3(x, y, z);
        
        
    }
}
