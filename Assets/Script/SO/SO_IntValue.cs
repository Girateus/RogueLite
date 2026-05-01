using UnityEngine;

[CreateAssetMenu(fileName = "IntValue", menuName = "Avalon Datas/Values/Int Value")]
public class SO_IntValue : ScriptableObject
{
    [SerializeField] int _value;
    public int Value
    {
        get => _value;
        set => _value = value;
    }
}
