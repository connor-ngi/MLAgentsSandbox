using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class BirdAgent : Agent
{
    private float vspeed = 0;

    [SerializeField] private Wall wallPrefab;
    [SerializeField] private Transform wallParent;
    private List<Wall> walls = new List<Wall>();

    [SerializeField] private TextMeshProUGUI text;
    private int score;

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);

        sensor.AddObservation(vspeed);
        sensor.AddObservation(transform.localPosition.y);        
        sensor.AddObservation(walls[0].transform.localPosition.x);
        sensor.AddObservation(walls[0].transform.localPosition.y);
        sensor.AddObservation(walls[1].transform.localPosition.y);
        sensor.AddObservation(walls[2].transform.localPosition.y);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);

        Debug.Log(actions.DiscreteActions[0]);

        bool shouldJump = actions.DiscreteActions[0] == 4;

        if (shouldJump)
            vspeed = 4;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.Space))
            discreteActions[0] = 1;
        else
            discreteActions[0] = 0;
    }

    private void Update()
    {
        transform.position += vspeed * Time.deltaTime * Vector3.up;

        vspeed -= Time.deltaTime * 10;

        List<Wall> flaggedForRemoval = new List<Wall>();
        foreach(Wall wall in walls)
        {
            if (wall.transform.localPosition.x < transform.localPosition.x - 1)
            {
                flaggedForRemoval.Add(wall);
                AddReward(10);
                score++;
            }
        }
        foreach(Wall wall in flaggedForRemoval)
        {
            walls.Remove(wall);
            Destroy(wall.gameObject);
            walls.Add(CreateWall(18));
        }

        text.text = $"{score}";

        AddReward(Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        EndEpisode();
    }

    public override void OnEpisodeBegin()
    {
        foreach (Wall wall in walls)
            Destroy(wall.gameObject);
        walls.Clear();

        for(int i = 1; i <= 3; i++)
        {
            walls.Add(CreateWall(6 * i));
        }

        vspeed = 0;
        score = 0;
        transform.localPosition = Vector3.zero;
    }

    private Wall CreateWall(float distanceAway)
    {
        Wall wall = Instantiate(wallPrefab,  wallParent);
        wall.transform.localPosition = new Vector3(distanceAway, -4f + Random.Range(0f, 8f), 0);
        return wall;
    }
}
