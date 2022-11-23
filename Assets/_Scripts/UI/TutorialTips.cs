using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTips : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tipText;
    [SerializeField] TextMeshProUGUI tipCounterText;
    [SerializeField] Button arrowBack;
    [SerializeField] Button arrowNext;

    string[] tips =
    {
        "Use the WASD keys or arrow keys to move the camera. You can zoom in or out with the mouse scroll or + / - button.",
        "Press the space bar to slow down time and open the construction menu. Study the characteristics of the buildings to optimize the defense centers.",
        "Scan your surroundings when you hear the danger alarm to anticipate your defenses against threats.",
        "Use energy to construct buildings and defend the planet. The planet will produce a small amount of energy periodically.",
        "In each defense center there are 5 available construction slots. You can cancel or demolish the construction at any time.",
        "The circular bar in the center shows the planet´s health. In the inner circle you can see how much energy is being produced and stored.",
        "Press ESC to open the game menu. You can enable/disable these tips, level the sound, or restart the game."
    };

    int tutorialIndex;

    void Start()
    {
        tutorialIndex = 0;
        SetTutorialText();
    }

    void Update()
    {
        ChangeArrowStates();
    }

    void SetTutorialText()
    {
        tipText.text = tips[tutorialIndex];
        tipCounterText.text = $"{tutorialIndex + 1}/{tips.Length}";
    }

    public void NextTip()
    {
        tutorialIndex++;
        SetTutorialText();
    }

    public void PreviousTip()
    {
        tutorialIndex--;
        SetTutorialText();
    }

    void ChangeArrowStates()
    {
        if (tutorialIndex == 0)
        {
            arrowBack.interactable = false;
        }
        else
        {
            arrowBack.interactable = true;
        }

        if (tutorialIndex == tips.Length - 1)
        {
            arrowNext.interactable = false;
        }
        else
        {
            arrowNext.interactable = true;
        }
    }
}
