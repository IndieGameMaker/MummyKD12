using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.fixedDeltaTime * 1.5f);

        if (transform.position.y <= -3.0f) Destroy(this.gameObject);
    }
}
