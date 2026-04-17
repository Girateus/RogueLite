// BossContext.cs
using UnityEngine;

namespace FSM
{
    public class BossContext
    {
        public Transform Transform;
        public Transform PlayerTransform;
        public Rigidbody2D Rb;
        public BoundsInt Room;
        public bool PlayerInRoom;

        // Lancelot
        public float DashSpeed;
        public float DashDuration;
        public float DashCooldown;
        public Animator Animator;

        // Baldufl
        public float AttackCooldown;
        public float AttackRange;

        // Morgane
        public GameObject BubblePrefab;
        public GameObject LightningPrefab;
        public float SpellCooldown;

        public float DistanceToPlayer =>
            Vector2.Distance(Transform.position, PlayerTransform.position);

        public BossContext(Transform transform, Transform player, Rigidbody2D rb)
        {
            Transform = transform;
            PlayerTransform = player;
            Rb = rb;
        }
    }
}