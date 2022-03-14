using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Indicates the maximum amount of fish on screen"
    public int maxFishOnScreen = 10;

    // Indicates the amount of time in seconds to wait until next respawn
    public float fishSpwanTime = 3f;

    public List<GameObject> fishPrefabs = new List<GameObject>();
    public List<WaypointPath> availableRoutes = new List<WaypointPath>();

    public float timeLeft = 20f;
    private int score = 0;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < maxFishOnScreen; i++)
            CreateFish();
    }

    // Update is called once per frame
    private void Update()
    {
        // COUNTDOWN
        /*timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            
            Debug.Log("END GAME");
            // TODO: end game..
        }*/
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

    public void OnFishDestroyed(int scorePoints, float bonusTime)
    {
        score += scorePoints;
        
        // Spawns a new fish..
        CreateFish();
    }


    protected void UpdateScoreGUI()
    {
        // TODO: update GUI
    }

    protected void UpdateTimerGUI()
    {
        // TODO: update GUI
    }
}