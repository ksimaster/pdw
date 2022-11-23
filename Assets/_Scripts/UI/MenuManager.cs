using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    GameManager gm;
    AudioManager am;

    void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        am = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);

        gm.IsGameStarted = true;
        gm.SwitchTutorialDialog();

        am.PlayGameTheme();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
