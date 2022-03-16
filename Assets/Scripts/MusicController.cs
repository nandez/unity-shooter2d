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

        PlayMusic();
    }

    public void PauseMusic()
    {
        audioSrc?.Pause();
    }

    public void PlayMusic()
    {
        if(audioSrc?.isPlaying == false)
            audioSrc.Play();
    }

    public void Stop()
    {
        audioSrc?.Stop();
    }

    public void PlayOneShot(AudioClip clip)
    {
        audioSrc?.PlayOneShot(clip);
    }
}