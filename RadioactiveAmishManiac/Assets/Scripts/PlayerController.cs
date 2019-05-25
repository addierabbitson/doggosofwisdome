using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PlayerDirection
{
    FORWARD,
    BACKWARD,
    LEFT,
    RIGHT,

    COUNT
}
public enum DraggedDirection
{
    Up,
    Down,
    Right,
    Left
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
    public GameManager gameManager;
    public int health = 1;
    public bool isPlayerDead = false;
    public int Score;
    public int maxDragDistance;
    public Transform squishObject;

    public GameObject splashParticles;
    public GameObject deathParticles;

    bool isOnPlatform = false;
    float originalY = 0.0f;
    Transform originalParent = null;

    public Vector2 fd;
    public Vector2 fu;

    [SerializeField]
    private bool detectSwipeOnlyAfterRelease = false;

    [SerializeField]
    private float minDistanceForSwipe = 20f;

    [Header("Squish")]
    public float squishStrength = 1.0f;
    public float squishDrag = 1.0f;
    public float squishAmount = 1.0f;
    float squishHeight = 1.0f;
    float squishVel = 0.0f;

    [Header("Boing")]
    public AudioSource boingSource;

    private void Start()
    {
        if (gabesfuckingugly.Instance)
            gabesfuckingugly.Instance.SetScore(Score);

        direction = new Dictionary<PlayerDirection, Vector3>();
        direction.Add(PlayerDirection.FORWARD, new Vector3(0, 0, 1));
        direction.Add(PlayerDirection.BACKWARD, new Vector3(0, 0, -1));
        direction.Add(PlayerDirection.LEFT, new Vector3(-1, 0, 0));
        direction.Add(PlayerDirection.RIGHT, new Vector3(1, 0, 0));

        originalParent = transform.parent;
        originalY = transform.position.y;
    }

    private void Update()
    {
        if (isPlayerDead)
            return;

        if (Input.GetKeyDown(KeyCode.P))
            health = 0;

        if (health == 0 && isPlayerDead == false)
        {
            //Time.timeScale = 0.0f;
            gameManager = GameManager.Instance;
            gameManager.gameUI.SetActive(false);
            gameManager.gameEnd.SetActive(true);
            isPlayerDead = true;
            gameManager.currentPopup = Popup.GAMEEND;
            OnDeath();
        }

        int nTouches = Input.touchCount;

        if (nTouches > 0)
        {
            print(nTouches + "touch detected");

            for (int i = 0; i < nTouches; i++)
            {
                Touch touch = Input.GetTouch(i);

                TouchPhase phase = touch.phase;

                if (touch.phase == TouchPhase.Began)
                {
                    fd = touch.position;
                    fu = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    fu = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    fu = touch.position;

                    Vector2 v2Difference = new Vector2(Mathf.Abs(fu.x - fd.x), Mathf.Abs(fu.y - fd.y));

                    if (v2Difference.x > maxDragDistance || v2Difference.y > maxDragDistance)
                    {
                        if (v2Difference.x > v2Difference.y)
                        {
                            if (fu.x < fd.x)
                            {
                                MoveInDirection(PlayerDirection.LEFT);
                                Debug.Log("Left");
                            }
                            else
                            {
                                MoveInDirection(PlayerDirection.RIGHT);
                                Debug.Log("Right");
                            }
                        }
                        else
                        {
                            if (fu.y > fd.y)
                            {
                                //Score++;
                                //if (gabesfuckingugly.Instance)
                                //    gabesfuckingugly.Instance.SetScore(Score);
                                //MoveInDirection(PlayerDirection.FORWARD);
                                //Debug.Log("Forward");
                            }
                            else
                            {
                                Score--;
                                if (gabesfuckingugly.Instance)
                                    gabesfuckingugly.Instance.SetScore(Score);
                                MoveInDirection(PlayerDirection.BACKWARD);
                                Debug.Log("Backward");
                            }
                        }
                    }
                    else
                    {
                        Score++;
                        if (gabesfuckingugly.Instance)
                            gabesfuckingugly.Instance.SetScore(Score);
                        MoveInDirection(PlayerDirection.FORWARD);
                    }
                }
            }
        }

        RaycastHit hit;
        if (Physics.Raycast(new Ray(this.transform.position, -transform.up), out hit, 1))
        {
            if (hit.transform.CompareTag("Water"))
            {
                health = 0;
                Instantiate(splashParticles, transform.position, splashParticles.transform.rotation);
            }
        }

        if (Input.GetKeyDown(KeyCode.W) && !isMoving)
        {
            Score++;
            if (gabesfuckingugly.Instance)
                gabesfuckingugly.Instance.SetScore(Score);
            MoveInDirection(PlayerDirection.FORWARD);
        }
        if (Input.GetKeyDown(KeyCode.S) && !isMoving)
        {
            Score--;
            if (gabesfuckingugly.Instance)
                gabesfuckingugly.Instance.SetScore(Score);
            MoveInDirection(PlayerDirection.BACKWARD);
        }
        if (Input.GetKeyDown(KeyCode.A) && !isMoving && !isOnPlatform)
        {
            MoveInDirection(PlayerDirection.LEFT);
        }
        if (Input.GetKeyDown(KeyCode.D) && !isMoving && !isOnPlatform)
        {
            MoveInDirection(PlayerDirection.RIGHT);
        }
    }

    private void FixedUpdate()
    {
        squishVel -= (squishHeight - 1.0f) * squishStrength * Time.fixedDeltaTime;
        squishVel -= (squishVel * squishDrag) * Time.fixedDeltaTime;

        squishHeight += squishVel * Time.fixedDeltaTime;

        Vector3 s = Vector3.one;
        s.y = squishHeight;
        s.x = 1.0f + (1.0f - s.y);
        s.z = s.x;
        squishObject.localScale = s;
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
        squishVel -= squishAmount*2.0f;
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

            // check for platform hits!
            if (timer / movingSpeed > 0.8f)
            {
                RaycastHit[] hits = Physics.RaycastAll(this.transform.position, Vector3.down, 2.0f);
                foreach (var hit in hits)
                {
                    if (hit.transform.CompareTag("Platform"))
                    {
                        transform.parent = hit.transform;
                        isOnPlatform = true;
                        break;
                    }
                }
            }

            yield return new WaitForFixedUpdate();
        }
        newPos.y = y;
        transform.localPosition = isOnPlatform ? Vector3.zero : newPos;

        temp = transform.position;
        temp.y = originalY;
        transform.position = temp;
        isMoving = false;
        squishVel += squishAmount;
    }

    IEnumerator Squish()
    {
        const float squishTime = 0.3f;

        for (float t = 0; t < squishTime; t += Time.fixedDeltaTime)
        {
            if (Time.timeScale == 0.0f)
                break;

            Vector3 s = Vector3.one;
            s.y = SquishEase(t / squishTime);
            s.x = 1.0f + (1.0f - s.y);
            s.z = s.x;
            squishObject.localScale = s;

            yield return new WaitForFixedUpdate();
        }

        squishObject.localScale = Vector3.one;
    }

    float SquishEase(float t)
    {
        // y = 1.2 - 6.438095*x + 15.28571*x^2 - 9.047619*x^3
        return 1.2f - (6.438095f * t) + (15.28571f * (t * t)) - (9.047619f * (t * t * t));
    }

    void OnDeath()
    {
        int high = PlayerPrefs.GetInt("highscore", 0);
        high = Mathf.Max(high, Score);
        PlayerPrefs.SetInt("highscore", high);
        if (gabesfuckingugly.Instance)
            gabesfuckingugly.Instance.SetScore(Score);

        Instantiate(deathParticles, transform.position, deathParticles.transform.rotation);
        GameController.instance.isPlaying = false;

        StopAllCoroutines();
        this.gameObject.SetActive(false);

        CameraScript.instance.ShakeCamera(0.3f, 4.0f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward * 5.0f);
    }

    void MoveInDirection(PlayerDirection dir)
    {
        if (isMoving)
            return;
        RaycastHit hit;
        if (Physics.Raycast(new Ray(this.transform.position, direction[dir]), out hit, 1))
        {
            if (hit.transform.CompareTag("Obstacle"))
            {
                squishVel += squishAmount / 2.0f;
                return;
            }
        }

        if (isOnPlatform)
        {
            transform.SetParent(originalParent, true);

            Vector3 tempPos = transform.position;
            tempPos.x = (int)tempPos.x;
            transform.position = tempPos;

            isOnPlatform = false;
        }

        UpdateDirection(dir);
        currentPos = transform.localPosition;
        targetPos = currentPos + direction[dir];

        StartCoroutine("Hop");
        PlayBoing();
    }

    void PlayBoing()
    {
        boingSource.pitch = Random.Range(0.6f, 0.8f);
        boingSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            this.health = 0;
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //        health = 0;
    //}
    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
        Debug.Log("norm + " + dragVectorDirection);
        GetDragDirection(dragVectorDirection);

    }

    private DraggedDirection GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        Debug.Log(draggedDir);
        return draggedDir;
    }
}
