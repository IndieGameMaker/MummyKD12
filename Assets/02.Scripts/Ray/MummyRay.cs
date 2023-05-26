using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MummyRay : Agent
{
    private Transform tr;
    private Rigidbody rb;

    private StageManager stageManager;

    public override void Initialize()
    {
        MaxStep = 100;

        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        stageManager = tr.root.GetComponent<StageManager>();
    }

    public override void OnEpisodeBegin()
    {
        stageManager.InitStage();
        // 물리엔진 초기화
        rb.velocity = rb.angularVelocity = Vector3.zero;

        // Agents 위치 초기화
        Vector3 pos = new Vector3(Random.Range(-20.0f, 20.0f)
                                , 0.0f
                                , Random.Range(-20.0f, 20.0f));
        tr.localPosition = pos;
        tr.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
    }
}
