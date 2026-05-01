using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Identity")]
    public string WeaponName;
    public Sprite Icon;
    
    [Header("Stats")]
    public int Damage;
}
