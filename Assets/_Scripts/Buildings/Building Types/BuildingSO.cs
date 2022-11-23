using UnityEngine;

public enum BuildingType { powerGenerator, machineGunTurret, blasterTurret, shieldDome, missileTurret, laserTurret, scrapTurret }

[CreateAssetMenu(fileName = "New building", menuName = "Building")]
public class BuildingSO : ScriptableObject
{
    public BuildingType type;

    public new string name;
    public string description;
    public int cost;

    public Sprite icon;

    public int vsShield;
    public int vsHull;
    public string range;
    public string precision;
}
