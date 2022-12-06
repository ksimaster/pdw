using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    private float time = 0;
    public TextMeshProUGUI timeText;

    public void StartTimer()
    {
        StartCoroutine("TimerBody");
    }

    IEnumerator TimerBody()
    {
        while(time > -1)
        {
            yield return new WaitForSeconds(1f);
            time += 1;
            timeText.text = time.ToString();
            PlayerPrefs.SetFloat("time", time);
            if(PlayerPrefs.HasKey("bestTime"))
            {
                if (PlayerPrefs.GetFloat("time") > PlayerPrefs.GetFloat("bestTime")) PlayerPrefs.SetFloat("bestTime", time);
            }
            else
            {
                PlayerPrefs.SetFloat("bestTime", time);
            }
        }
        
    }


}