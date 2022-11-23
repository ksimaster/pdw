using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpin : MonoBehaviour
{
    void Update()
    {
        SpinPlanet();
    }

    void SpinPlanet()
    {
        transform.Rotate(0f, 0f, 1f * Time.deltaTime);
    }
}
