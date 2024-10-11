using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float force = 10.0f;
    public float forceTurn = 10.0f;

    public float jumpForce = 5.0f;

    private bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Reads the [WSAD] input to control the player movement.
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * force);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * force);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * force);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(transform.right * -force);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded){
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded=true;
        }
    }
}

