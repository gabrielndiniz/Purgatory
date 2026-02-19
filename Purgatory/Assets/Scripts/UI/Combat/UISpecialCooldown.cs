using UnityEngine;
using UnityEngine.UI;
using Survivor.Combat;

namespace Survivor.UI.Combat
{
    public class UISpecialCooldown : MonoBehaviour
    {
        [SerializeField] private CombatController combatController;
        [SerializeField] private Image cooldownImage;

        private void Awake()
        {
            if (cooldownImage != null)
                cooldownImage.type = Image.Type.Filled;
        }

        private void Update()
        {
            if (combatController == null || cooldownImage == null)
                return;

            float cooldown = combatController.GetSpecialCooldown();
            float timer = combatController.GetSpecialTimer();

            if (cooldown <= 0f)
            {
                cooldownImage.fillAmount = 0f;
                return;
            }

            float normalized = Mathf.Clamp01(timer / cooldown);

            cooldownImage.fillAmount = 1f - normalized;
        }
    }
}
