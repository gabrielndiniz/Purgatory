using Survivor.Controller;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Survivor.Combat
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance { get; private set; }

        public UnityEvent OnAllEnemiesDefeated;

        private readonly List<Enemy> enemies = new();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Register(Enemy enemy)
        {
            if (!enemies.Contains(enemy))
            {
                enemies.Add(enemy);
                Debug.Log($"Enemy registered. Total: {enemies.Count}");
            }
        }

        public void Unregister(Enemy enemy)
        {
            if (enemies.Remove(enemy))
            {
                Debug.Log($"Enemy removed. Remaining: {enemies.Count}");

                if (enemies.Count == 0)
                {
                    Debug.Log("All enemies defeated!");
                    OnAllEnemiesDefeated?.Invoke();
                }
            }
        }

        public Transform GetClosestEnemy(Vector3 position, float range)
        {
            Transform closest = null;
            float minDist = range * range;

            foreach (var e in enemies)
            {
                if (e == null) continue;

                float dist = (e.transform.position - position).sqrMagnitude;
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = e.transform;
                }
            }

            return closest;
        }
    }
}
