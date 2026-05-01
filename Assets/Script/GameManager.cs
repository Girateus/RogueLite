using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public WeaponData SelectedWeapon { get; private set; }

    [SerializeField] private WeaponData _defaultWeapon;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SelectedWeapon = _defaultWeapon;
    }

    public void SetWeapon(WeaponData weapon)
    {
        SelectedWeapon = weapon;
        Debug.Log($"[GameManager] weapon : {weapon.WeaponName}");
    }
}