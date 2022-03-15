using UnityEngine;

public class FishController : MonoBehaviour
{
    public float movementSpeed = 2f;
    public float idleTime = 3f;

    public int scorePoints;
    public float bonusTime;

    private WaypointPath path;
    private bool reversePath = false;
    private bool isIdle = false;
    private float iddleCounter = 0f;
    private bool isRunning = false;

    private SpriteRenderer spriteRdr;
    private GameManager gameManager;

    private void Start()
    {
        spriteRdr = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();

        spriteRdr.flipX = reversePath;
    }

    private void Update()
    {
        if (isIdle)
        {
            // As we're in idle mode, we update the counter to wait prior moving.
            iddleCounter += Time.deltaTime;
            if (iddleCounter < idleTime)
                return;

            isIdle = false;
        }

        var waypoint = reversePath ? path.StartWaypoint : path.EndWaypoint;
        Vector3 newPosition;

        if (Vector3.Distance(transform.position, waypoint.position) < 0.01f)
        {
            // Here we've reached the waypoint
            if (isRunning)
            {
                // Here we've been hit, so we need to call the GM
                // so score can be updated, time bonus can be added
                // and then we need to destroy the current object.
                gameObject.SetActive(false);
                gameManager.OnFishDestroyed(bonusTime);
                Destroy(gameObject, 1f);
            }
            else
            {
                // Here we went across the whole path without being clicked,
                // so we set the new waypoint to the other side, we set the idle
                // mode so we can wait an amount of time prior moving.
                newPosition = waypoint.position;
                iddleCounter = 0f;
                isIdle = true;

                // Here we also handle the reverse path switch.
                reversePath = !reversePath;
                spriteRdr.flipX = reversePath;

                // Moves the fish to new position..
                transform.position = newPosition;
            }
        }
        else
        {
            // In this case, we move towards the waypoint.
            var speed = isRunning ? movementSpeed * 2 : movementSpeed;
            newPosition = Vector3.MoveTowards(transform.position, waypoint.position, speed * Time.deltaTime);

            // Moves the fish to new position..
            transform.position = newPosition;
        }
    }

    public void SetWaypoint(WaypointPath wpPath, bool reversed)
    {
        path = wpPath;
        reversePath = reversed;
    }

    public void OnMouseDown()
    {
        if (isRunning)
            return;

        isRunning = true;
        reversePath = !reversePath;
        spriteRdr.flipX = reversePath;

        gameManager.OnFishClicked(scorePoints);
    }
}