using UnityEngine;

public class PowerGenerator : MonoBehaviour
{
    int productionIncrease = 20;

    Building buildingScript;
    BuildManager buildManagerScript;

    public int ProductionIncrease { get => productionIncrease; }

    bool bonusGiven = false;
    public bool BonusGiven { get => bonusGiven; }

    void Awake()
    {
        buildingScript = GetComponent<Building>();
        buildManagerScript = GameObject.FindGameObjectWithTag("BuildManager").GetComponent<BuildManager>();
    }

    void Update()
    {
        if (buildingScript.IsBuildingActive && !bonusGiven)
        {
            AddProduction();
            bonusGiven = true;
        }
    }

    void AddProduction()
    {
        buildManagerScript.IncreaseEnergyProduction(productionIncrease);
    }
}
