using DG.Tweening;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public float movementSpeed = 2f;
    public float idleTime = 3f;

    public int scorePoints;
    public float bonusTime;

    public GameObject bonusTimePrefab;
    public Color runningColor;

    public AudioClip bonusTimeSound;
    public AudioClip penaltyTimeSound;

    private WaypointPath path;
    private bool reversePath = false;
    private bool isIdle = false;
    private float iddleCounter = 0f;
    private bool isRunning = false;
    private Sequence runningSequence;

    private SpriteRenderer spriteRdr;
    private GameManager gameMgr;
    private AudioSource audioSrc;
    

    private void Start()
    {
        spriteRdr = GetComponent<SpriteRenderer>();
        gameMgr = FindObjectOfType<GameManager>();
        audioSrc = GetComponent<AudioSource>();

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
                gameMgr.OnFishDestroyed();
                runningSequence.Complete(false);
                runningSequence.Kill();
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

        // Sets the running mode to avoid normal behaviour..
        isRunning = true;

        // Updates the reverse path related logic
        reversePath = !reversePath;
        spriteRdr.flipX = reversePath;

        // Updates the sprite tint to let the user know fish is running
        // and sets up a tween to fade in fade out
        spriteRdr.color = new Color(runningColor.r, runningColor.g, runningColor.b, 1);

        runningSequence = DOTween.Sequence()
            .Append(spriteRdr.DOFade(0, 0.15f))
            .Append(spriteRdr.DOFade(1, 0.15f))
            .SetLoops(-1, LoopType.Restart)
            .Play();

        // Notifies the game manager to add current points to score.
        gameMgr.OnFishClicked(scorePoints, bonusTime);

        // Creates a floating text to display the time added as bonus.
        var bonusTimeObject = Instantiate(bonusTimePrefab, transform.position, Quaternion.identity);
        var bonusTimeText = bonusTimeObject.GetComponent<TMPro.TMP_Text>();
        var isBonus = bonusTime >= 0;
        bonusTimeText.SetText($"{(isBonus ? '+' : string.Empty)}{bonusTime}s");
        bonusTimeText.color = isBonus ? Color.white : Color.red;
        bonusTimeText.DOFade(0f, 0.7f);
        bonusTimeObject.transform.DOMove(bonusTimeObject.transform.position + Vector3.up, 0.75f).OnComplete(() => Destroy(bonusTimeObject));

        // Plays a sound based on time power-up sign..
        audioSrc.PlayOneShot(isBonus ? bonusTimeSound : penaltyTimeSound);
    }
}