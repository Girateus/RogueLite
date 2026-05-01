using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    
    private Vector2 _moveInput; 
    private SpriteRenderer _sr;
    private Rigidbody2D _rb;
    private Animator _animator;
    private DamageTaker _damageTaker;
    
    [SerializeField] private float _hitForce;
    
    public bool _isHit = false;
    public bool _isDead = false;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _damageTaker = GetComponent<DamageTaker>();
        
        if (_rb == null) Debug.LogError("Missing Rigidbody2D on Player!");
    }

    void Update()
    {
        Move();
        FlipSprite();
    }

    public void OnMoveForward(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        _animator.SetBool("SwordAtk", true);
    }
    
    public void StopAttack()
    {
        _animator.SetBool("SwordAtk", false);
    }

    private void Move()
    {
        _rb.linearVelocity = new Vector2(_moveInput.x * _speed, _rb.linearVelocity.y);
        
        _rb.linearVelocity = _moveInput * _speed;
    }

    private void FlipSprite()
    {
        if (_moveInput.x > 0.1f)
        { 
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_moveInput.x < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile")|| other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hazard Touché !");
            
            _isHit = true;
            _rb.linearVelocity = Vector2.zero;
            
            Vector2 knockbackDir = (transform.position - other.transform.position).normalized;
            _rb.AddForce(knockbackDir * _hitForce, ForceMode2D.Impulse);
            if (_damageTaker != null)
            {
                _damageTaker.TakeDamage(1f);
                _animator.SetBool("TakeDamage", true);
                
            }
            

            StopCoroutine("ResetHit_co");
            StartCoroutine("ResetHit_co");
        }  Debug.Log("Hit what ?" + other.gameObject.name);
       
    }
    
    IEnumerator ResetHit_co()
    {
        yield return new WaitForSeconds(0.5f);
        _isHit = false;
        _animator.SetBool("TakeDamage", false);
    }
}