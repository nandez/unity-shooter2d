using UnityEngine;

public class MusicController : MonoBehaviour
{
    private static MusicController _instance;

    public static MusicController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MusicController>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private AudioSource audioSrc;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
                Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        if (audioSrc.isPlaying)
            return;

        audioSrc.Play();
    }

    public void PauseMusic()
    {
        if (audioSrc.isPlaying)
            audioSrc.Pause();
    }

    public void ResumeMusic()
    {
        if (!audioSrc.isPlaying)
            audioSrc.Play();
    }
}