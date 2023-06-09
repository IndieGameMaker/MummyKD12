using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

[RequireComponent(typeof(DecisionRequester))]
public class MummyRay : Agent
{
    private Transform tr;
    private Rigidbody rb;

    private StageManager stageManager;
    private int sumGoodItem;

    public override void Initialize()
    {
        MaxStep = 5000;

        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        stageManager = tr.root.GetComponent<StageManager>();
    }

    public override void OnEpisodeBegin()
    {
        sumGoodItem = 0;

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
        var action = actions.DiscreteActions;

        // Debug.Log($"[0]={action[0]}, [1]={action[1]}");

        // 이동방향 벡터
        Vector3 dir = Vector3.zero;
        // 회전방향 벡터
        Vector3 rot = Vector3.zero;

        // Branch[0]
        switch (action[0])
        {
            case 1: dir = tr.forward; break;
            case 2: dir = -tr.forward; break;
        }

        // Brach[1]
        switch (action[1])
        {
            case 1: rot = -tr.up; break;
            case 2: rot = tr.up; break;
        }


        tr.Rotate(rot, Time.fixedDeltaTime * 200.0f);
        rb.AddForce(dir * 1.5f, ForceMode.VelocityChange);

        AddReward(-1 / (float)MaxStep); // -1/5000 = 0.005f
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

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("GOOD_ITEM"))
        {
            AddReward(+1.0f);
            rb.velocity = rb.angularVelocity = Vector3.zero;
            Destroy(coll.gameObject);
            if (++sumGoodItem >= stageManager.goodItemCount) EndEpisode();
        }
        if (coll.collider.CompareTag("BAD_ITEM"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }
        if (coll.collider.CompareTag("WALL"))
        {
            AddReward(-0.01f);
        }
    }
}
