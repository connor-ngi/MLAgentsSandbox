using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CursorAgent : Agent
{
    [SerializeField] private Cubes observingCubes;
    [SerializeField] private TextMeshProUGUI text;
    private int currentChoice;
    private float reward;
    private float timeElapsed;

    private void Start()
    {
        StartCoroutine(EpisodeLoop());
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);

        sensor.AddObservation(observingCubes.currentRed);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);

        Debug.Log(actions.DiscreteActions[0]);

        currentChoice = actions.DiscreteActions[0];

        transform.localPosition = new Vector3(-4 + currentChoice, 1, 0);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);

        int h = 0;
        if (Input.GetKey(KeyCode.Alpha2)) h = 1;
        if (Input.GetKey(KeyCode.Alpha3)) h = 2;

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = h;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if(currentChoice == observingCubes.currentRed)
        {
            AddReward(Time.deltaTime);
            reward += Time.deltaTime;
        }

        text.text = $"Reward: {reward:0.00}/{timeElapsed:0.00}";
    }

    private IEnumerator EpisodeLoop()
    {
        while(true)
        {
            yield return new WaitForSeconds(100);
            EndEpisode();
        }
    }

    public override void OnEpisodeBegin()
    {
        reward = 0;
        timeElapsed = 0;
    }
}
