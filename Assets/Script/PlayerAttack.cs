using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform _pivot;      // le pivot sous Sword
    [SerializeField] private float _attackRadius = 0.5f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private LayerMask _enemyLayer;


    public void CheckHit()
    {
        Debug.Log("[PlayerAttack] CheckHit appelé !");
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            _pivot.position,
            _attackRadius,
            _enemyLayer 
        );
        
        Debug.Log($"[PlayerAttack] {hits.Length} colliders détectés");

        foreach (Collider2D hit in hits)
        {
            Debug.Log($"[PlayerAttack] touche : {hit.name} | tag : {hit.tag}");

            if (!hit.CompareTag("Enemy")) continue;

            Vector2 dir = (hit.transform.position - _pivot.position).normalized;
            hit.GetComponent<DamageTaker>()?.TakeDamage(_damage, dir);
        }
    }

    private void OnDrawGizmos()
    {
        if (_pivot == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_pivot.position, _attackRadius);
    }
}
