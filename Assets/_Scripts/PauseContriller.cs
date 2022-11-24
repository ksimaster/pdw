using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseContriller : MonoBehaviour
{
    public GameObject panelStart;
    private void Start()
    {
        PauseON();
    }
    public void PauseON()
    {
        Time.timeScale = 0;
    }

    public void PauseOFF()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (panelStart.activeSelf) PauseON();
    }
}
