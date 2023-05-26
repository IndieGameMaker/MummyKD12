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

    public Material goodMt, badMt;
    private Material originMt;
    private Renderer floorRd;   // 바닦의 렌더러

    // 초기화 작업을 위한 한번만 호출되는 메소드
    public override void Initialize()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        targetTr = transform.root.Find("Target").GetComponent<Transform>();
        floorRd = tr.root.Find("Plane").GetComponent<MeshRenderer>();
        originMt = floorRd.material;

        MaxStep = 1000;
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
                                , 0.5f
                                , Random.Range(-4.0f, 4.0f));
        targetTr.localPosition = targetPos;


    }

    // 주변환경을 관측 & 브레인에 전달하는 메소드
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(targetTr.localPosition); // 3 (x, y, z)
        sensor.AddObservation(tr.localPosition);       // 3 (x, y, z)
        sensor.AddObservation(rb.velocity.x);          // 1 (x)
        sensor.AddObservation(rb.velocity.z);          // 1 (z)
    }

    // 브레인(정책:Policy)에게서 전달받은 명령을 실행하는 메소드
    public override void OnActionReceived(ActionBuffers actions)
    {
        /*
            연속적인 수치 (Continuous) : -1.0f ~ 0.0f ~ +1.0f
            이산 수치 (Discrete) : -1.0f, 0.0f, +1.0f
        */
        // 연속 수치로 값을 송수신
        var _action = actions.ContinuousActions;

        // 에이전트 이동
        /*
            전진/후진 = Input.GetAxis("Vertical") => actions[0]
            왼쪽/오른쪽 = Input.GetAxis("Horizontal") => actions[1]
        */

        Vector3 dir = (Vector3.forward * _action[0]) + (Vector3.right * _action[1]);
        rb.AddForce(dir.normalized * 20.0f);

        // 지속적인 움직임을 유도하기 위해 마이너스 리워드
        SetReward(-0.001f);
    }

    // 개발자가 직접 명령을 내릴때 사용하는 메소드
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // 키보드 입력값
        var _action = actionsOut.ContinuousActions;

        // 전/후진
        _action[0] = Input.GetAxis("Vertical"); // UP/Down -1.0f ~ 1.0f
        // 좌/우
        _action[1] = Input.GetAxis("Horizontal");
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("DEAD_ZONE"))
        {
            SetReward(-1.0f);
            StartCoroutine(this.ChangeColor(badMt));
            // 학습 종료
            EndEpisode();
        }

        if (coll.collider.CompareTag("TARGET"))
        {
            SetReward(+1.0f);
            StartCoroutine(this.ChangeColor(goodMt));
            EndEpisode();
        }
    }

    IEnumerator ChangeColor(Material changeMt)
    {
        //바닦 머리리얼 변경
        floorRd.material = changeMt;

        yield return new WaitForSeconds(0.2f);
        //원래 머티리얼로 환원
        floorRd.material = originMt;
    }

}
