using FSM;
using UnityEngine;

public abstract class BossController : MonoBehaviour
{
    protected Machine _machine;
    protected BossContext _ctx;
    protected BossIdleState _idleState;
    private bool _activated = false;

    protected virtual void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Transform player = GameObject.FindWithTag("Player")?.transform;

        _ctx = new BossContext(transform, player, rb);
        _idleState = new BossIdleState(_ctx);
    }

    protected virtual void Start()
    {
        _machine = new Machine();
        _machine.SetState(_idleState);
        SetupStates();
    }

    protected virtual void Update()
    {
        if (_activated)
            _machine?.Tick();
    }
    
    public void OnPlayerEnterRoom()
    {
        if (_activated) return;
        _activated = true;
        OnActivated();
    }
    
    protected abstract void SetupStates();

   
    protected abstract void OnActivated();

    public void SetRoom(BoundsInt room)
    {
        _ctx.Room = room;
        
        GameObject triggerGo = new GameObject("BossRoomTrigger");
        BossRoomTrigger trigger = triggerGo.AddComponent<BossRoomTrigger>();
        trigger.Init(this, room);
    }

    protected virtual void OnDrawGizmos()
    {
        if (_ctx == null) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _ctx.AttackRange);
    }
}