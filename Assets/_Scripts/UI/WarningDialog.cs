using System.Collections;
using TMPro;
using UnityEngine;

public class WarningDialog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI threatLevelText;
    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] AudioClip warning_SFX;

    SFXManager sm;

    void Awake()
    {
        sm = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SFXManager>();
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ActivateWarningDialog(string levelText, string waveNum)
    {
        gameObject.SetActive(true);
        sm.PlayOneShot(warning_SFX);

        SetThreatLevelText(levelText);
        SetWaveNumber(waveNum);

        StartCoroutine(DeactiveDialog());
    }

    void SetThreatLevelText(string levelText)
    {
        threatLevelText.text = levelText;
    }

    void SetWaveNumber(string waveNum)
    {
        waveText.text = waveNum;
    }

    public void DisableWarningAndSFX()
    {
        gameObject.SetActive(false);
        sm.StopAudio();
    }

    IEnumerator DeactiveDialog()
    {
        yield return new WaitForSeconds(6.75f);
        gameObject.SetActive(false);
    }
}
