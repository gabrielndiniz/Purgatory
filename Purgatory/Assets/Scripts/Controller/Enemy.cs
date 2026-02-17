using UnityEngine;
using Survivor.Combat;

namespace Survivor.Controller
{
    public class Enemy : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float stopDistance = 4f;

        private Transform player;
        private CombatController combat;


        private void OnEnable()
        {
            if (EnemyManager.Instance != null)
                EnemyManager.Instance.Register(this);
        }
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            combat = GetComponent<CombatController>();

            if (combat != null)
            {
                combat.canAttack = true;
            }
        }

        private void Update()
        {
            if (player == null)
                return;

            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > stopDistance)
            {
                MoveTowardsPlayer();
                if (combat != null) combat.canAttack = false;
            }
            else
            {
                if (combat != null) combat.canAttack = true;
            }
        }

        private void MoveTowardsPlayer()
        {
            Vector3 dir = (player.position - transform.position).normalized;
            Vector3 move = dir * moveSpeed * Time.deltaTime;

            transform.position += move;

            dir.y = 0f;
            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(dir);
            }
        }

        private void OnDisable()
        {
            if (EnemyManager.Instance != null)
                EnemyManager.Instance.Unregister(this);
        }


    }
}
