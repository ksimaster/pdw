using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    private int time = 0;
    public TextMeshProUGUI timeText;
    public GameObject bodyPlanet;

    public void StartTimer()
    {
        StartCoroutine("TimerBody");
    }

    IEnumerator TimerBody()
    {
        while(time > -1)
        {
            if (bodyPlanet.activeSelf)
            {
                yield return new WaitForSeconds(1f);
                time += 1;
                timeText.text = time.ToString();
                PlayerPrefs.SetInt("time", time);
            }
            else
            {
                break;
            }

            if(PlayerPrefs.HasKey("bestTime"))
            {
                if (PlayerPrefs.GetInt("time") > PlayerPrefs.GetInt("bestTime")) 
                {
                    PlayerPrefs.SetInt("bestTime", time);
                    int best = PlayerPrefs.GetInt("bestTime");
#if UNITY_WEBGL && !UNITY_EDITOR
    	            WebGLPluginJS.SetLeder(best);
#endif
                }
            }
            else
            {
                PlayerPrefs.SetInt("bestTime", time);
                int best = PlayerPrefs.GetInt("bestTime");
#if UNITY_WEBGL && !UNITY_EDITOR
    	            WebGLPluginJS.SetLeder(best);
#endif
            }
        }
        
    }


}
