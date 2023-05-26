using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCreater : MonoBehaviour
{
    public Transform[] points;
    public GameObject ballPrefab;

    public bool isFinish = false;

    void Start()
    {
        StartCoroutine(this.CreateBall());
    }

    IEnumerator CreateBall()
    {
        while (!isFinish)
        {
            yield return new WaitForSeconds(0.5f);
            int index = Random.Range(0, points.Length);
            Instantiate(ballPrefab, points[index].position, points[index].rotation, transform);
        }
    }

}
