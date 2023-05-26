using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.fixedDeltaTime * 2.5f);

        if (transform.position.y <= -2.0f)
        {
            transform.root.Find("Agent").GetComponent<AgentPong>().SetReward(+1.0f);
            Destroy(this.gameObject);
        }
    }
}
