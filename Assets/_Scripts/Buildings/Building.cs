using UnityEngine;

public class Building : MonoBehaviour
{
    bool isBuildingActive = false;
    public bool IsBuildingActive { get => isBuildingActive; set => isBuildingActive = value; }

    void Start()
    {
        isBuildingActive = false;
    }
}
