using UnityEngine;
using UnityEngine.UI;
using Survivor.Combat;

namespace Survivor.UI.Combat
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private Survivor.Combat.Health health;
        [SerializeField] private RectTransform healthBar;

        private void OnEnable()
        {
            if (health != null)
            {
                health.OnHealthChanged.AddListener(UpdateHealthBar);
                UpdateHealthBar(health.CurrentHealth, health.MaxHealth);
            }
        }

        private void Start()
        {
            UpdateHealthBar(health.CurrentHealth, health.MaxHealth);
        }

        private void OnDisable()
        {
            if (health != null)
            {
                health.OnHealthChanged.RemoveListener(UpdateHealthBar);
            }
        }

        private void UpdateHealthBar(float current, float max)
        {
            if (healthBar == null || max <= 0f)
                return;
            float percentage = current / max;
            percentage = percentage * 0.98f + 0.005f;
            healthBar.anchorMax = new Vector2(percentage, 0.985f);
        }
    }
}
