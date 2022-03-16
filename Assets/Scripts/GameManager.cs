using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Indicates the maximum amount of fish on screen"
    public int maxFishOnScreen = 10;

    public float timeLeft = 20f;

    public List<GameObject> fishPrefabs = new List<GameObject>();
    public List<WaypointPath> availableRoutes = new List<WaypointPath>();

    // Refrences to main GUI elements..
    public TMPro.TMP_Text scoreText;

    public TMPro.TMP_Text timeText;

    // References for endgame frame panel.
    public GameObject endGamePanel;

    public TMPro.TMP_Text endGameScoreText;
    public TMPro.TMP_Text endGameTimeText;

    public AudioClip endGameAudioClip;

    private int score = 0;

    private void Start()
    {
        Time.timeScale = 1;

        for (int i = 0; i < maxFishOnScreen; i++)
            CreateFish();
    }

    private void FixedUpdate()
    {
        // Handles the countdown.
        timeLeft -= Time.fixedDeltaTime;
        if (timeLeft < 0)
        {
            timeLeft = 0;
            Time.timeScale = 0;

            // Updates the endgame frame GUI
            endGameScoreText.SetText($"Points: {score}");
            endGameTimeText.SetText($"Time: {Time.timeSinceLevelLoad:0}s");
            endGamePanel.SetActive(true);

            // Plays the endgame clip
            MusicController.Instance.Stop();
            MusicController.Instance.PlayOneShot(endGameAudioClip);
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

    public void OnFishDestroyed()
    {
        // Spawns a new fish..
        CreateFish();
    }

    public void OnFishClicked(int scorePoints, float bonusTime)
    {
        // Updates the score
        score += scorePoints;
        scoreText.SetText($"{score}");

        // Adds the bonus time to current countdown.
        timeLeft += bonusTime;
    }

    public void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene("MenuScene");
    }
}