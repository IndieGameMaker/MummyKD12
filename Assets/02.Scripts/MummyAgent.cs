using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

using Random = UnityEngine.Random;

public class MummyAgent : Agent
{
    /*
        1. 주변 환경을 관측(Observation)
        2. 행동(Action)    
        3. 보상(Reward)
    */

    [SerializeField] private Transform tr;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform targetTr;

    // 초기화 작업을 위한 한번만 호출되는 메소드
    public override void Initialize()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        targetTr = transform.root.Find("Target").GetComponent<Transform>();
    }

    // 학습(Episode)이 시작될때 마다 호출되는 메소드
    public override void OnEpisodeBegin()
    {
        // 물리력 초기화
        rb.velocity = rb.angularVelocity = Vector3.zero;
        // 에이전트의 위치를 지정(불규칙하게)
        Vector3 pos = new Vector3(Random.Range(-4.0f, 4.0f)
                                , 0.0f
                                , Random.Range(-4.0f, 4.0f));
        tr.localPosition = pos;

        Vector3 targetPos = new Vector3(Random.Range(-4.0f, 4.0f)
                                , 0.0f
                                , Random.Range(-4.0f, 4.0f));
        targetTr.localPosition = targetPos;


    }

    // 주변환경을 관측 & 브레인에 전달하는 메소드
    public override void CollectObservations(VectorSensor sensor)
    {
    }

    // 브레인(정책:Policy)에게서 전달받은 명령을 실행하는 메소드
    public override void OnActionReceived(ActionBuffers actions)
    {
    }

    // 개발자가 직접 명령을 내릴때 사용하는 메소드
    public override void Heuristic(in ActionBuffers actionsOut)
    {
    }

}
