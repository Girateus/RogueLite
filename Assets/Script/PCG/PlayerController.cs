using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    
    private Vector2 _moveInput; // Changed to Vector2 to handle more directions later
    private SpriteRenderer _sr;
    private Rigidbody2D _rb; // Added for physics-based movement

    void Start()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        
        // Safety check: ensure we have a Rigidbody2D
        if (_rb == null) Debug.LogError("Missing Rigidbody2D on Player!");
    }

    void Update()
    {
        Move();
        FlipSprite();
    }

    public void OnMoveForward(InputAction.CallbackContext ctx)
    {
        // ReadValue as Vector2 if your Input Action is set to "Value" and "Vector2"
        // Or keep float if it's just one axis. Let's assume Vector2 for a Rogue-lite.
        _moveInput = ctx.ReadValue<Vector2>();
    }

    private void Move()
    {
        // Direct velocity approach (snappy and precise for Rogue-lites)
        _rb.linearVelocity = new Vector2(_moveInput.x * _speed, _rb.linearVelocity.y);
        
        // If you want Top-Down (Y movement too), use:
        _rb.linearVelocity = _moveInput * _speed;
    }

    private void FlipSprite()
    {
        // Check movement on X axis to flip
        if (_moveInput.x > 0.1f)
        {
            _sr.flipX = false;
        }
        else if (_moveInput.x < -0.1f)
        {
            _sr.flipX = true;
        }
    }
}