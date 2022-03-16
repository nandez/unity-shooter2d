using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private void Start()
    {
        MusicController.Instance.PlayMusic();
    }

    public void OnStartGame()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}