using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    private AgentMover agentMover;

    private Vector3 inputDirection;
    private bool jump;

    private void Start()
    {
        agentMover = GetComponent<AgentMover>();
    }

    private void Update()
    {
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        
        if (Input.GetKeyDown(KeyCode.Space))
            jump = true;
    }

    private void FixedUpdate()
    {
        if (inputDirection != Vector3.zero)
            agentMover.Move(inputDirection);

        if(jump)
        {
            jump = false;
            agentMover.Jump();
        }
    }
}
