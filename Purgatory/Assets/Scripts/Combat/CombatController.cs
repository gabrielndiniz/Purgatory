using System.Collections;
using UnityEngine;

namespace Survivor.Combat
{
    public class CombatController : MonoBehaviour
    {
        [Header("Basic Attack")]
        [SerializeField] private float attackRate = 0.6f;
        [SerializeField] private float attackRange = 10f;
        [SerializeField] private float pivotDistance = 1.5f;
        [SerializeField] private GameObject firePoint;
        [SerializeField] private ProjectilePool projectilePool;

        [Header("Special Attack")]
        [SerializeField] private float specialCooldown = 5f;
        [SerializeField] private float specialRadius = 3f;
        [SerializeField] private int specialDamage = 20;
        [SerializeField] private GameObject specialVfxObject;
        [SerializeField] private float specialVfxDuration = 2f;

        [Header("Targeting")]
        [SerializeField] private string targetTag = "Enemy";

        private float attackTimer;
        private float specialTimer;
        public bool canAttack { get; set; } = false;

        private void Update()
        {
            attackTimer += Time.deltaTime;
            specialTimer += Time.deltaTime;

            if (canAttack)
            {
                HandleAutoAttack();
            }
        }

        private Transform GetClosestTarget()
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
            Transform closest = null;
            float minDist = Mathf.Infinity;

            foreach (var obj in targets)
            {
                float dist = Vector3.Distance(transform.position, obj.transform.position);
                if (dist <= attackRange && dist < minDist)
                {
                    minDist = dist;
                    closest = obj.transform;
                }
            }

            return closest;
        }


        private void HandleAutoAttack()
        {
            if (attackTimer < attackRate)
                return;

            Transform target = GetClosestTarget();
            if (target == null)
                return;

            attackTimer = 0f;
            Shoot(target.position);
        }


        private Vector3 SetShootingPivotLocation(Vector3 targetPosition)
        {
            Vector3 dir = (targetPosition - transform.position).normalized;
            Vector3 position = transform.position + dir * pivotDistance;

            firePoint.transform.position = position;
            return position;
        }

        private void Shoot(Vector3 targetPosition)
        {
            if (projectilePool == null)
            {
                Debug.LogError("CombatController: ProjectilePool is null");
                return;
            }

            Projectile proj = projectilePool.Get();
            if (proj == null)
            {
                Debug.LogError("CombatController: Projectile is null");
                return;
            }

            Vector3 spawnPos = SetShootingPivotLocation(targetPosition);

            Vector3 dir = (targetPosition - spawnPos).normalized;

            proj.transform.position = spawnPos;
            proj.transform.rotation = Quaternion.LookRotation(dir);

            proj.Fire(dir, gameObject);
        }

        


        public bool CanUseSpecial()
        {
            return specialTimer >= specialCooldown;
        }

        public void UseSpecial()
        {
            if (!CanUseSpecial())
                return;

            specialTimer = 0f;
            DoAreaDamage();
            SpawnSpecialVFX();
        }

        private void DoAreaDamage()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, specialRadius);

            foreach (var hit in hits)
            {
                if (!hit.CompareTag(targetTag))
                    continue;

                Health health = hit.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(specialDamage);
                }
            }

            Debug.Log("Special attack triggered");
        }
        private void SpawnSpecialVFX()
        {
            if (specialVfxObject == null)
                return;

            StopCoroutine(nameof(SpecialVfxRoutine));
            StartCoroutine(SpecialVfxRoutine());
        }

        private IEnumerator SpecialVfxRoutine()
        {
            specialVfxObject.SetActive(true);

            yield return new WaitForSeconds(specialVfxDuration);

            specialVfxObject.SetActive(false);
        }

    }
}
