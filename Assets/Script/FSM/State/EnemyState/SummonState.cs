using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class SummonState : IState
    {
        private EnemyContext _ctx;
        private float _timer;
        private List<GameObject> _minions = new List<GameObject>();

        public SummonState(EnemyContext ctx) { _ctx = ctx; }

        public void Enter()
        {
            _timer = _ctx.SummonCooldown;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                TrySummon();
                _timer = _ctx.SummonCooldown;
            }
        }

        public void Exit() { }

        private void TrySummon()
        {
            _minions.RemoveAll(m => m == null);

            if (_minions.Count >= _ctx.MaxMinions) return;

            Vector2 spawnOffset = Random.insideUnitCircle * 1.5f;
            Vector3 spawnPos = _ctx.Transform.position + (Vector3)spawnOffset;
            GameObject minion = Object.Instantiate(_ctx.MinionPrefab, spawnPos, Quaternion.identity);
            _minions.Add(minion);
            Debug.Log("[SummonState] Lesser Ghost invoqué !");
        }
    }
}