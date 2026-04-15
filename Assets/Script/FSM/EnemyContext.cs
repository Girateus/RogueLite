using UnityEngine;

namespace FSM
{
    public class EnemyContext : MonoBehaviour
    {
        public Transform Transform;
        public Transform PlayerTransform;
        public float DetectionRange;
        public float AttackRange;
        public float MoveSpeed;

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