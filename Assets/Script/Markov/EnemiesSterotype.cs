using UnityEngine;

public enum EnemyType
{
    Mele,
    Range,
    Healer,
    Summoner
    
}

[CreateAssetMenu(fileName = "EnemiesSterotype", menuName = "Scriptable Objects/EnemiesSterotype")]
public class EnemiesSterotype : ScriptableObject
{
    public float HP;
    public float Damage;
    public EnemyType type;

}
