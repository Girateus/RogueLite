using System.Collections;
using UnityEngine;

public class EnemyHitReaction : MonoBehaviour
{
    [SerializeField] private float _knockbackForce = 5f;
    [SerializeField] private float _knockbackDuration = 0.2f;
    [SerializeField] private Color _hitColor = Color.red;
    [SerializeField] private float _flashDuration = 0.15f;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Color _originalColor;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        _originalColor = _sr.color;
    }

    public void ReactToHit(Vector2 hitDirection)
    {
        StopAllCoroutines();
        StartCoroutine(KnockbackRoutine(hitDirection));
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator KnockbackRoutine(Vector2 dir)
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(dir * _knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(_knockbackDuration);
        _rb.linearVelocity = Vector2.zero;
    }

    private IEnumerator FlashRoutine()
    {
        _sr.color = _hitColor;
        yield return new WaitForSeconds(_flashDuration);
        _sr.color = _originalColor;
    }
}