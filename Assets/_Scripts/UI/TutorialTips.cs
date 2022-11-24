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
        "����������� ������� WASD ��� ������� �� ��������� ��� ����������� ������. �� ������ ����������� ��� ��������� ������� � ������� ��������� ���� ��� ������ + / -",
        "������� ������, ����� ��������� ����� � ������� ���� �������������. ������� �������������� ������, ����� �������������� ������ �������.",
        "���������� � �������, ����� �������� ������ �������.",
        "����������� ������� ��� ������������� ������ � ������ �������. ������� ����� ������������ ����������� ��������� ���������� �������.",
        "� ������ ������ ������� ���� 5 ��������� ������������ ������. �� ������ �������� ��� ������ ������������� � ����� �����.",
        "������� ������� � ������ ���������� ��������� �������. �� ���������� ����� �� ������ ������, ������� ������� ������������ � �������������.",
        "������� ESC, ����� ������� ������� ����. �� ������ �������� / ��������� ��� ���������, �������� ��������� ��� ������������� ����."
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
