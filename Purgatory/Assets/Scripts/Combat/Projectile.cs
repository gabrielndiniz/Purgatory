using UnityEngine;

namespace Survivor.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private int damage = 10;
        [SerializeField] private float lifeTime = 3f;

        private float timer;
        private ProjectilePool pool;
        private GameObject owner;
        private Vector3 direction;

        public void Init(ProjectilePool ownerPool)
        {
            pool = ownerPool;
        }

        private void OnEnable()
        {
            timer = 0f;
        }

        private void Update()
        {
            transform.position += direction * speed * Time.deltaTime;

            timer += Time.deltaTime;
            if (timer >= lifeTime)
            {
                ReturnToPool();
            }
        }

        public void Fire(Vector3 shootDirection, GameObject shooter)
        {
            owner = shooter;

            shootDirection.y = 0f;
            direction = shootDirection.normalized;

            transform.forward = direction;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"[Projectile] Hit: {other.name}");

            if (other.gameObject == owner)
                return;

            Health health = other.GetComponentInParent<Health>();
            if (health == null)
                return;

            health.TakeDamage(damage);
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            owner = null;
            direction = Vector3.zero;

            if (pool != null)
                pool.Release(this);
            else
                gameObject.SetActive(false);
        }
    }
}
