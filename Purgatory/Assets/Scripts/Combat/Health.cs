using UnityEngine;
using UnityEngine.Events;
using Survivor.Core;

namespace Survivor.Combat
{
    public class Health : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float maxHealthPoints = 100f;

        [Header("Events")]
        [SerializeField] private UnityEvent<float> onTakeDamage;
        [SerializeField] private UnityEvent onDie;
        [SerializeField] private UnityEvent<float, float> onHealthChanged;

        private float currentHealthPoints;
        private bool isDead;

        private Animator animator;

        private static readonly int IsDeadHash = Animator.StringToHash("isDead");


        private void Awake()
        {
            currentHealthPoints = maxHealthPoints;
            animator = GetComponentInChildren<Animator>();

            // Inicializa UI / listeners
            onHealthChanged?.Invoke(currentHealthPoints, maxHealthPoints);
        }

        public bool IsDead => isDead;
        public float CurrentHealth => currentHealthPoints;
        public float MaxHealth => maxHealthPoints;


        public UnityEvent<float> OnTakeDamage => onTakeDamage;
        public UnityEvent OnDie => onDie;
        public UnityEvent<float, float> OnHealthChanged => onHealthChanged;


        public void TakeDamage(float damage)
        {
            if (isDead || damage <= 0f)
                return;

            currentHealthPoints = Mathf.Max(currentHealthPoints - damage, 0f);
            onHealthChanged?.Invoke(currentHealthPoints, maxHealthPoints);

            if (currentHealthPoints <= 0f)
            {
                Die();
            }
            else
            {
                onTakeDamage?.Invoke(damage);
            }
        }

        public void Heal(float amount)
        {
            if (isDead || amount <= 0f)
                return;

            currentHealthPoints = Mathf.Min(currentHealthPoints + amount, maxHealthPoints);
            onHealthChanged?.Invoke(currentHealthPoints, maxHealthPoints);
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;

            if (animator != null)
            {
                animator.SetBool(IsDeadHash, true);
            }

            onDie?.Invoke();

            if (CompareTag("Player"))
            {
                GameManager.Instance.SetState(GameState.Defeat);
            }
        }
    }
}
