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

        private HashSet<Enemy> aliveEnemies = new();

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
            aliveEnemies.Add(enemy);
            Debug.Log($"[EnemyManager] Enemy registered. Total: {aliveEnemies.Count}");
        }

        public void NotifyEnemyKilled(Enemy enemy)
        {
            if (!aliveEnemies.Contains(enemy))
                return;

            aliveEnemies.Remove(enemy);
            Debug.Log($"[EnemyManager] Enemy killed. Remaining: {aliveEnemies.Count}");

            /*if (aliveEnemies.Count == 0)
            {
                Debug.Log("[EnemyManager] All enemies defeated!"); //It should work, but it is not working.
                AllEnemiesDefeated()
            }*/
        }

        public void AllEnemiesDefeated()
        {
            OnAllEnemiesDefeated?.Invoke();
        }

        
    }
}
