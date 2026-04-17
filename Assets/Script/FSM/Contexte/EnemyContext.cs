using UnityEngine;

namespace FSM
{
    public class EnemyContext
    {
        
        public Rigidbody2D Rb;
        public Transform Transform;
        public Transform PlayerTransform;
        public GameObject ProjectilePrefab;
        public float DetectionRange;
        public float AttackRange;
        public float MoveSpeed;
        public float ProjectileSpeed;
        
        [Header("Greater Ghost")]
        public GameObject MinionPrefab;
        public float SummonCooldown;
        public int MaxMinions;
        
        [Header("Bat")]
        public float PreferredDistance;

        public EnemyContext(Transform transform, Transform playerTransform)
        {
            Transform = transform;
            PlayerTransform = playerTransform;
        }

        public float DistanceToPlayer => Vector2.Distance(Transform.position, PlayerTransform.position);
    }
}