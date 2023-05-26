using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentPong : Agent
{
    private Transform tr;
    private Rigidbody rb;

    public override void Initialize()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        MaxStep = 1000;
    }

    public override void OnEpisodeBegin()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("BALL");
        foreach (var ball in balls)
        {
            if (ball.transform.root == transform.root)
                Destroy(ball);
        }
        rb.velocity = rb.angularVelocity = Vector3.zero;

        tr.localPosition = new Vector3(Random.Range(-3.0f, 3.0f), 0.0f, -4.0f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var _actions = actions.ContinuousActions;
        //Debug.Log($"actions[0] = {_actions[0]}");

        Vector3 dir = Vector3.right * _actions[0];
        rb.AddForce(dir.normalized * 20.0f);

        SetReward(-1 / (float)MaxStep);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.ContinuousActions;
        actions.Clear();

        actions[0] = Input.GetAxis("Horizontal");
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BALL"))
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

}
