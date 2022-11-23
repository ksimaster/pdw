using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    AudioSource audioSource;

    void Awake()
    {
        SingletonMe();

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip audioClip)
    {
        if (audioSource.enabled == false) { return; }

        StopAudio();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void StopAudio()
    {
        if (audioSource.enabled == false) { return; }

        audioSource.Stop();
    }

    public void PlayOneShot(AudioClip audioClip)
    {
        if (audioSource.enabled == false) { return; }

        audioSource.PlayOneShot(audioClip);
        
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
