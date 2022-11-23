using UnityEngine;

public enum EnemyType { Fighter, Corvette, Cruiser, Destroyer, Harpoon, NukeBomber }

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class WaveSO : ScriptableObject
{
    public EnemyType enemyType;
    public int enemyAmount;
}
