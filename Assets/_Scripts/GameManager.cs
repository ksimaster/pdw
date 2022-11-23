using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Texture2D cursor;
    [SerializeField] Toggle tipsToggle;
    [SerializeField] GameObject tutorialDialog;
    [SerializeField] Button restartButton;
    [SerializeField] Button exitButton;
    [SerializeField] GameObject settingsButton;
    [SerializeField] GameObject endDialog;
    [SerializeField] TextMeshProUGUI endDialogWaveCounter;

    public static GameManager instance;

    MachineGunSFX machineGunSFX;
    GameObject menu;
    GameObject helpCover;
    GameObject infoCover;
    BuildManager buildManagerScript;
    AudioManager am;
    SFXManager sm;

    bool menuOpened = false;
    public bool MenuOpened { get => menuOpened; }

    bool isTutorialTipsEnabled = false;

    Vector3 cameraCentered = new Vector3(0f, 0f, -10f);

    bool isGameStarted = false;
    public bool IsGameStarted { get => isGameStarted; set => isGameStarted = value; }

    bool isGameOver = false;
    public bool IsGameOver { get => isGameOver; set => isGameOver = value; }

    void Awake()
    {
        SingletonMe();

        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);

        machineGunSFX = GameObject.FindGameObjectWithTag("MachineGunSFX").GetComponent<MachineGunSFX>();
        menu = GameObject.FindGameObjectWithTag("MenuInGame");
        helpCover = menu.transform.GetChild(0).Find("Help Cover").gameObject;
        infoCover = menu.transform.GetChild(0).Find("Info Cover").gameObject;
        buildManagerScript = GameObject.FindGameObjectWithTag("BuildManager").GetComponent<BuildManager>();
        am = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        sm = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SFXManager>();
    }

    void Start()
    {
        isGameStarted = true;
        isGameOver = false;

        EndDialogState(false);

        menu.transform.GetChild(0).gameObject.SetActive(menuOpened);
        tutorialDialog.SetActive(isTutorialTipsEnabled);
        tipsToggle.SetIsOnWithoutNotify(isTutorialTipsEnabled);
    }

    void Update()
    {
        MenuState();
        TutorialDialogState();
        CenterCamera();
        
        if (restartButton != null)
        {
            DisableButtonsInMainMenu();
        }
    }

    void MenuState()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            ChangeEscapeMenuState();
            PauseSFXs();
        }
    }

    public void ResumeGame()
    {
        PauseSFXs();
        ChangeEscapeMenuState();
    }

    public void ExitMenu()
    {
        ChangeEscapeMenuState();
        am.PlayIntroTheme();

        SceneManager.LoadScene(0);
    }

    public void ExitMenuFromEndDialog()
    {
        EndDialogState(false);
        SettingsButtonState(true);

        isGameStarted = true;
        isGameOver = false;

        am.PlayIntroTheme();

        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        if (!isGameOver)
        {
            ResumeGame();
        }

        isGameStarted = true;
        isGameOver = false;

        EndDialogState(false);

        SettingsButtonState(true);

        DisableMachineGunSFX();

        am.PlayGameTheme();
        sm.StopAudio();

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void PauseSFXs()
    {
        if (menuOpened)
        {
            machineGunSFX.audioSource.Pause();
        }
        else
        {
            machineGunSFX.audioSource.Play();
        }
    }

    void DisableMachineGunSFX()
    {
        machineGunSFX.Turrets.Clear();

        machineGunSFX.audioSource.Stop();
    }

    public void SwitchTutorialDialog()
    {
        isTutorialTipsEnabled = !isTutorialTipsEnabled;
        tipsToggle.SetIsOnWithoutNotify(isTutorialTipsEnabled);
    }

    void TutorialDialogState()
    {
        tutorialDialog.SetActive(isTutorialTipsEnabled);
    }

    public void ChangeEscapeMenuState()
    {
        menuOpened = !menuOpened;

        Time.timeScale = menuOpened ? 0f : 1f;

        menu.transform.GetChild(0).gameObject.SetActive(menuOpened);

        if (helpCover.activeSelf || infoCover.activeSelf)
        {
            CloseHelpCover();
            CloseInfoCover();
        }
    }

    void CenterCamera()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Camera.main.transform.position = cameraCentered;
        }
    }
    
    public void OpenHelpCover()
    {
        helpCover.SetActive(true);
    }

    public void CloseHelpCover()
    {
        helpCover.SetActive(false);
    }

    public void OpenInfoCover()
    {
        infoCover.SetActive(true);
    }

    public void CloseInfoCover()
    {
        infoCover.SetActive(false);
    }

    void DisableButtonsInMainMenu()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene == 0)
        {
            restartButton.interactable = false;
            exitButton.interactable = false;
        }
        else
        {
            restartButton.interactable = true;
            exitButton.interactable = true;
        }
    }

    public void SettingsButtonState(bool value)
    {
        settingsButton.SetActive(value);
    }

    public void EndDialogState(bool value)
    {
        endDialog.SetActive(value);
    }

    public void SetEndDialogWaveNumber()
    {
        SpawnManager spawnM = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();

        int currentWave = spawnM.WaveCounter + 1;
        endDialogWaveCounter.text = $"Wave {currentWave}";
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
