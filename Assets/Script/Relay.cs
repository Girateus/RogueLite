using UnityEngine;

public class Relay : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerAttack _playerAttack;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _playerAttack     = GetComponentInParent<PlayerAttack>();

        Debug.Log($"[Relay] PlayerController : {_playerController != null}");
        Debug.Log($"[Relay] PlayerAttack : {_playerAttack != null}");
    }

    public void StopAttack() => _playerController?.StopAttack();
    public void CheckHit()   => _playerAttack?.CheckHit();
}
