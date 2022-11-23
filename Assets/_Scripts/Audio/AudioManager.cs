using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource sfxManager;
    [SerializeField] AudioSource machineGunManager;
    [SerializeField] AudioSource musicManager;

    [SerializeField] Slider sfxMenuSlider;
    [SerializeField] TextMeshProUGUI sfxTextValue;
    [SerializeField] Slider musicMenuSlider;
    [SerializeField] TextMeshProUGUI musicTextValue;

    [SerializeField] AudioClip buttonMenuInGame_SFX;
    [SerializeField] AudioClip introTheme;
    [SerializeField] AudioClip gameTheme;

    SFXManager sm;
    MusicManager mm;

    float initSFXVolume = 0.4f;
    float initMusicVolume = 0.75f;

    void Awake()
    {
        SingletonMe();

        sm = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SFXManager>();
        mm = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
    }

    void Start()
    {
        sfxMenuSlider.value = initSFXVolume;
        musicMenuSlider.value = initMusicVolume;
        UpdateSFXAndMusicManagerVolumes();

        mm.PlaySound(introTheme);
    }

    void Update()
    {
        SetSFXVolume();
        SetMusicVolume();
        UpdateSFXAndMusicManagerVolumes();
    }

    void SetSFXVolume()
    {
        sfxTextValue.text = (sfxMenuSlider.value * 100).ToString("0");
    }

    void SetMusicVolume()
    {
        musicTextValue.text = (musicMenuSlider.value * 100).ToString("0");
    }

    public void PlayButtonInGameSFX()
    {
        sm.PlayOneShot(buttonMenuInGame_SFX);
    }

    public void PlayIntroTheme()
    {
        mm.PlaySound(introTheme);
    }

    public void PlayGameTheme()
    {
        mm.PlaySound(gameTheme);
    }

    void UpdateSFXAndMusicManagerVolumes()
    {
        sfxManager.volume = sfxMenuSlider.value;
        machineGunManager.volume = sfxMenuSlider.value;
        musicManager.volume = musicMenuSlider.value;
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
