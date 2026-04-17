using FSM;
using UnityEngine;

public class MorganeSpellState : IState
{
    private BossContext _ctx;
    private float _timer;
    public bool SpellDone { get; private set; }

    public MorganeSpellState(BossContext ctx) { _ctx = ctx; }

    public void Enter()
    {
        SpellDone = false;
        _timer = _ctx.SpellCooldown;
        CastRandomSpell();
    }

    public void Tick()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0) SpellDone = true;
    }

    public void Exit() { }

    private void CastRandomSpell()
    {
        if (Random.value > 0.5f)
            CastBubble();
        else
            CastLightning();
    }

    private void CastBubble()
    {
        if (_ctx.BubblePrefab == null) return;
        Vector2 dir = (_ctx.PlayerTransform.position - _ctx.Transform.position).normalized;
        GameObject bubble = Object.Instantiate(
            _ctx.BubblePrefab,
            _ctx.Transform.position,
            Quaternion.identity
        );
        bubble.GetComponent<Rigidbody2D>()?.AddForce(dir * 5f, ForceMode2D.Impulse);
        Debug.Log("[Morgane] Bulle !");
    }

    private void CastLightning()
    {
        if (_ctx.LightningPrefab == null) return;
        Object.Instantiate(
            _ctx.LightningPrefab,
            _ctx.PlayerTransform.position, Quaternion.identity
        );
        Debug.Log("[Morgane] Éclair !");
    }
}