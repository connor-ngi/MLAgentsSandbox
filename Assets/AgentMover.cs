using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        rb.MovePosition(transform.position + direction.normalized * 4 * Time.deltaTime);
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }
}
