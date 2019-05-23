using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float moveSpeed = 1.0f;

    Vector3 startPoint = Vector3.zero;
    Vector3 endPoint = Vector3.zero;

    private void FixedUpdate()
    {
        Vector3 end = transform.parent.position + endPoint;
        Vector3 start = transform.parent.position - startPoint;

        Vector3 dir = (start - end).normalized;
        transform.position -= dir * moveSpeed * Time.fixedDeltaTime;

        float startDist = Vector3.Distance(start, end);
        float thisDist = Vector3.Distance(transform.position, start);

        if (startDist < thisDist)
            Destroy(this.gameObject);
    }

    public void Spawn(Vector3 position, Vector3 destination)
    {
        startPoint = transform.position - position;
        transform.position = position;
        endPoint = destination;
    }

}
