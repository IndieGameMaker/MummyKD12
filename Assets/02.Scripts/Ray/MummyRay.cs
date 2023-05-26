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
        MaxStep = 5000;

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
        /* 
            이산 수치 Discreate Type
            1. 정지/전진/후진  (Null, W , S) (0:정지, 1:전진, 2:후진)
            2. 무회전/왼쪽회전/오른쪽회전 (Null, A, D) (0, 1, 2)
        
            Branch 갯수 2
            Branch[0], size 3
            Branch[1], size 3
        */

        var actions = actionsOut.DiscreteActions;
        actions.Clear();

        // 정지 / 전진 / 후진 : Branch 0 (0, 1, 2)
        if (Input.GetKey(KeyCode.W))
        {
            actions[0] = 1; // 전진
        }

        if (Input.GetKey(KeyCode.S))
        {
            actions[0] = 2; // 후진
        }

        // 좌/우 회전 : Branch[1] (0, 1, 2)
        if (Input.GetKey(KeyCode.A))
        {
            actions[1] = 1; // 왼쪽 회전
        }

        if (Input.GetKey(KeyCode.D))
        {
            actions[1] = 2; // 오른쪽 회전
        }
    }
}
