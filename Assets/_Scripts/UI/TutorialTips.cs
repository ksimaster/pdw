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
        "Используйте клавиши WASD или клавиши со стрелками для перемещения камеры. Вы можете увеличивать или уменьшать масштаб с помощью прокрутки мыши или кнопки + / -",
        "Нажмите пробел, чтобы замедлить время и открыть меню строительства. Изучите характеристики зданий, чтобы оптимизировать центры обороны.",
        "Готовитесь к обороне, когда услышите сигнал тревоги.",
        "Используйте энергию для строительства зданий и защиты планеты. Планета будет периодически производить небольшое количество энергии.",
        "В каждом центре обороны есть 5 доступных строительных слотов. Вы можете отменить или снести строительство в любое время.",
        "Круглая полоска в центре показывает состояние планеты. Во внутреннем круге вы можете видеть, сколько энергии производится и накапливается.",
        "Нажмите ESC, чтобы открыть игровое меню. Вы можете включить / отключить эти подсказки, изменить громкость или перезапустить игру."
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
