using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;

public class AiAgent : Agent
{
    [SerializeField] private Transform goal;
    private AgentMover agentMover;
    private Rigidbody rb;

    private Vector3 spawnPoint;

    private void Start()
    {
        agentMover = GetComponent<AgentMover>();
        rb = GetComponent<Rigidbody>();

        spawnPoint = transform.position;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);

        sensor.AddObservation(transform.position);
        sensor.AddObservation(goal.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);

        Debug.Log(actions.DiscreteActions[0] + 1);

        agentMover.Move(new Vector3(actions.DiscreteActions[0] - 1, 0, actions.DiscreteActions[1] - 1));

        float f = -Vector3.Distance(transform.position, goal.position);
        //SetReward(f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = (int)Input.GetAxisRaw("Horizontal") + 2;
    }

    private void Update()
    {
        

        if (Vector3.Distance(transform.position, goal.position) < 0.5f)
        {
            SetReward(1000);
            EndEpisode();
        }
            
        if (transform.position.y < -10)
        {
            SetReward(-1000);
            EndEpisode();
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.position = spawnPoint;
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
