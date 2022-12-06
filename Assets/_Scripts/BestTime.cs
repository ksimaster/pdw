using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BestTime : MonoBehaviour
{
    public TextMeshProUGUI bestTime;
    void Start()
    {
        SetBestTime();
    }

    public void SetBestTime()
    {
        bestTime.text = PlayerPrefs.GetFloat("bestTime").ToString();
    }

}
