using System.Collections.Generic;
using UnityEngine;

namespace Survivor.Combat
{
    public class ProjectilePool : MonoBehaviour
    {
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private int poolSize = 20;

        private Queue<Projectile> pool = new Queue<Projectile>();

        private void Awake()
        {
            for (int i = 0; i < poolSize; i++)
            {
                Projectile proj = Instantiate(projectilePrefab, transform);
                proj.gameObject.SetActive(false);
                proj.Init(this);
                pool.Enqueue(proj);
            }
        }

        public Projectile Get()
        {
            if (pool.Count == 0)
                return null;

            Projectile proj = pool.Dequeue();

            proj.transform.SetParent(null); 
            proj.gameObject.SetActive(true);

            return proj;
        }

        public void Release(Projectile proj)
        {
            proj.transform.SetParent(transform);
            proj.gameObject.SetActive(false);
            pool.Enqueue(proj);
        }
    }
}
