using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject goodItem;
    [SerializeField] private GameObject badItem;

    [Range(10, 50)]
    public int goodItemCount = 30;
    [Range(10, 50)]
    public int badItemCount = 20;

    public List<GameObject> goodItemList = new List<GameObject>();
    public List<GameObject> badItemList = new List<GameObject>();

    public void InitStage()
    {
        // GoodItem 생성
        for (int i = 0; i < goodItemCount; i++)
        {
            // 불규칙한 위치(좌표) 생성
            Vector3 pos = new Vector3(Random.Range(-40.0f, 40.0f)
                                    , 0.0f
                                    , Random.Range(-40.0f, 40.0f));

            // 불규칙한 회전 생성

        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
