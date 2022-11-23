using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    AudioSource audioSource;

    void Awake()
    {
        SingletonMe();

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip audioClip, float volume = 1)
    {
        if (audioSource.enabled == false) { return; }

        StopAudio();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void StopAudio()
    {
        if (audioSource.enabled == false) { return; }

        audioSource.Stop();
    }

    void SingletonMe()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
