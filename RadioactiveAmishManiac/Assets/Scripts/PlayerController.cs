using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerDirection
{
    FORWARD,
    BACKWARD,
    LEFT,
    RIGHT,

    COUNT
}

public class PlayerController : MonoBehaviour
{
    public Dictionary<PlayerDirection, Vector3> direction;
    public PlayerDirection currentDirection;
    public Vector3 currentPos;
    public Vector3 targetPos;
    public bool isMoving;
    public float movingSpeed;
    public float hopHeight = 2.0f;
    
    private void Start()
    {
        direction = new Dictionary<PlayerDirection, Vector3>();
        direction.Add(PlayerDirection.FORWARD, new Vector3(0, 0, 1));
        direction.Add(PlayerDirection.BACKWARD, new Vector3(0, 0, -1));
        direction.Add(PlayerDirection.LEFT, new Vector3(-1, 0, 0));
        direction.Add(PlayerDirection.RIGHT, new Vector3(1, 0, 0));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) && !isMoving)
        {
            UpdateDirection(PlayerDirection.FORWARD);
            currentPos = transform.position;
            targetPos = currentPos + direction[PlayerDirection.FORWARD];

            StartCoroutine("Hop");
        }
        if(Input.GetKeyDown(KeyCode.S) && !isMoving)
        {
            UpdateDirection(PlayerDirection.BACKWARD);
            currentPos = transform.position;
            targetPos = currentPos + direction[PlayerDirection.BACKWARD];

            StartCoroutine("Hop");
        }
        if (Input.GetKeyDown(KeyCode.A) && !isMoving)
        {
            UpdateDirection(PlayerDirection.LEFT);
            currentPos = transform.position;
            targetPos = currentPos + direction[PlayerDirection.LEFT];

            StartCoroutine("Hop");
        }
        if (Input.GetKeyDown(KeyCode.D) && !isMoving)
        {
            UpdateDirection(PlayerDirection.RIGHT);
            currentPos = transform.position;
            targetPos = currentPos + direction[PlayerDirection.RIGHT];
            
            StartCoroutine("Hop");
        }
    }

    public void UpdateDirection(PlayerDirection _dir)
    {
        Vector3 target = direction[_dir];
        target = (target * 5.0f) + transform.position;
        transform.LookAt(target);
        currentDirection = _dir;
    }

    IEnumerator Hop()
    {
        isMoving = true;
        float timer = 0.0f;
        Vector3 temp = transform.position;
        float y = transform.position.y;
        Vector3 newPos = Vector3.zero;
        while (transform.position != targetPos)
        {
            newPos = Vector3.Lerp(currentPos, targetPos, timer / movingSpeed);
            if (newPos == targetPos)
                break;
            newPos.y = y + hopHeight;
            transform.position = newPos;
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        newPos.y = y;
        transform.position = newPos;
        isMoving = false;
    }
    
    void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward * 5.0f);
    }
}
