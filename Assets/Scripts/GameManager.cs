using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Indicates the maximum amount of fish on screen"
    public int maxFishOnScreen = 10;

    public List<GameObject> fishPrefabs = new List<GameObject>();
    public List<WaypointPath> availableRoutes = new List<WaypointPath>();

    public TMPro.TMP_Text scoreText;
    public TMPro.TMP_Text timeText;

    public float timeLeft = 20f;
    private int score = 0;

    // Start is called before the first frame update
    private void Start()
    {
        Time.timeScale = 1;

        for (int i = 0; i < maxFishOnScreen; i++)
            CreateFish();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 currentMouse = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(currentMouse);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        Debug.DrawLine(ray.origin, hit.point);

        // Handles the countdown.
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Time.timeScale = 0;
            Debug.Log("END GAME");
            // TODO: end game..
        }

        timeText.SetText(timeLeft.ToString("0"));
        timeText.color = timeLeft > 10 ? Color.white : Color.red;
    }

    protected void CreateFish()
    {
        // Picks a random fish to create a new instance and a random route for it to follow..
        var fishPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Count)];
        var fishRoute = availableRoutes[Random.Range(0, availableRoutes.Count)];

        // Creates the new fish object.
        var newFish = Instantiate(fishPrefab, fishRoute.StartWaypoint.position, Quaternion.identity);

        // Sets the fish waypoint route with 50% chance to reverse path..
        newFish.GetComponent<FishController>().SetWaypoint(fishRoute, Random.value >= .5f);
    }

    public void OnFishDestroyed(float bonusTime)
    {
        // Adds the bonus time to current countdown.
        timeLeft += bonusTime;

        // Spawns a new fish..
        CreateFish();
    }

    public void OnFishClicked(int scorePoints)
    {
        score += scorePoints;
        scoreText.SetText($"{score}");
    }

    protected void UpdateTimerGUI()
    {
        // TODO: update GUI
    }
}