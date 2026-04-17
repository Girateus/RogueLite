using FSM;
using UnityEngine;

public class BatShootState : IState
{
    private EnemyContext _ctx;
    private float _shootCooldown;
    private float _timer;
    public bool ShouldStopShooting { get; private set; }

    public BatShootState(EnemyContext ctx, float shootCooldown)
    {
        _ctx = ctx;
        _shootCooldown = shootCooldown;
    }

    public void Enter()
    {
        ShouldStopShooting = false;
        _timer = 0f;
    }

    public void Tick()
    {
        float dist = _ctx.DistanceToPlayer;
        float diff = dist - _ctx.PreferredDistance;
        Vector2 dir = (_ctx.Transform.position - _ctx.PlayerTransform.position).normalized;

        if (Mathf.Abs(diff) > 0.1f)
        {
            float sign = diff < 0 ? -1f : 1f;
            _ctx.Rb.linearVelocity = dir * sign * _ctx.MoveSpeed;
        }
        else
        {
            _ctx.Rb.linearVelocity = Vector2.zero;
        }
        
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            Shoot();
            _timer = _shootCooldown;
        }
        
        if (_ctx.DistanceToPlayer > _ctx.DetectionRange)
            ShouldStopShooting = true;
    }

    public void Exit()
    {
        _ctx.Rb.linearVelocity = Vector2.zero;
    }

    private void Shoot()
    {
        if (_ctx.ProjectilePrefab == null) return;

        Vector2 dir = (_ctx.PlayerTransform.position - _ctx.Transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        GameObject projectile = Object.Instantiate(
            _ctx.ProjectilePrefab,
            _ctx.Transform.position,
            Quaternion.Euler(0, 0, angle) 
        );

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb?.AddForce(dir * _ctx.ProjectileSpeed, ForceMode2D.Impulse);

        Debug.Log("[Bat] Tir !");
    }
}