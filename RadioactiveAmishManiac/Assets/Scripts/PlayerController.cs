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
    public PlayerDirection directionNotToMove;
    public Vector3 currentPos;
    public Vector3 targetPos;
    public bool isMoving;
    public float movingSpeed;
    public float hopHeight = 2.0f;
    public GameManager gameManager;
    public int health = 1;
    public bool isPlayerDead = false;
    public int Score;

    private void Start()
    {
        direction = new Dictionary<PlayerDirection, Vector3>();
        direction.Add(PlayerDirection.FORWARD, new Vector3(0, 0, 1));
        direction.Add(PlayerDirection.BACKWARD, new Vector3(0, 0, -1));
        direction.Add(PlayerDirection.LEFT, new Vector3(-1, 0, 0));
        direction.Add(PlayerDirection.RIGHT, new Vector3(1, 0, 0));
        directionNotToMove = PlayerDirection.COUNT;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            health = 0;

        if (health == 0 && isPlayerDead == false)
        {
            Time.timeScale = 0.0f;
            gameManager = GameManager.Instance;
            gameManager.gameUI.SetActive(false);
            gameManager.gameEnd.SetActive(true);
            isPlayerDead = true;
        }
        RaycastHit hit;
        if (Physics.Raycast(new Ray(this.transform.position, -transform.up), out hit, 1))
        {
            if (hit.transform.CompareTag("Water"))
            {
                health = 0;
            }
        }
        else
        {
            directionNotToMove = PlayerDirection.COUNT;
        }

        if (Input.GetKeyDown(KeyCode.W) && !isMoving && directionNotToMove != PlayerDirection.FORWARD)
        {
            Score++;
            MoveInDirection(PlayerDirection.FORWARD);
        }
        if (Input.GetKeyDown(KeyCode.S) && !isMoving && directionNotToMove != PlayerDirection.BACKWARD)
        {
            MoveInDirection(PlayerDirection.BACKWARD);
        }
        if (Input.GetKeyDown(KeyCode.A) && !isMoving && directionNotToMove != PlayerDirection.LEFT)
        {
            MoveInDirection(PlayerDirection.LEFT);
        }
        if (Input.GetKeyDown(KeyCode.D) && !isMoving && directionNotToMove != PlayerDirection.RIGHT)
        {
            MoveInDirection(PlayerDirection.RIGHT);
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
        Vector3 temp = transform.localPosition;
        float y = temp.y;
        Vector3 newPos = Vector3.zero;
        while (transform.position != targetPos)
        {
            newPos = Vector3.Lerp(currentPos, targetPos, timer / movingSpeed);
            if (newPos == targetPos)
                break;
            newPos.y = y + hopHeight;
            transform.localPosition = newPos;
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        newPos.y = y;
        transform.localPosition = newPos;
        isMoving = false;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward * 5.0f);
    }

    void MoveInDirection(PlayerDirection dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(this.transform.position, direction[dir]), out hit, 1))
        {
            if (hit.transform.CompareTag("Obstacle"))
                return;
        }

        UpdateDirection(dir);
        currentPos = transform.localPosition;
        targetPos = currentPos + direction[dir];

        StartCoroutine("Hop");
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        this.health = 0;
    //    }
    //}
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //        health = 0;
    //}
}
