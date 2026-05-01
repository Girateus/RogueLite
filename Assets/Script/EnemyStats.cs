using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private string _enemyName;
    [SerializeField] private int _attack;

    public string EnemyName => _enemyName;
    public int Attack => _attack;
}