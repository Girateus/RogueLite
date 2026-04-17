using UnityEngine;

public class ProjectileLifetime : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float _lifetime = 3f;

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }
}
