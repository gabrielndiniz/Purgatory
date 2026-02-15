using UnityEngine;
using System.Collections;

namespace Survivor.Trigger
{
    public abstract class BaseTrigger : MonoBehaviour
    {
        [Header("Trigger Settings")]
        [SerializeField] protected float delay = 0f;

        protected bool hasTriggered;

        public void Trigger()
        {
            if (hasTriggered)
                return;

            hasTriggered = true;

            if (delay > 0f)
            {
                StartCoroutine(TriggerAfterDelay());
            }
            else
            {
                OnTriggered();
            }
        }

        private IEnumerator TriggerAfterDelay()
        {
            yield return new WaitForSeconds(delay);
            OnTriggered();
        }

        protected abstract void OnTriggered();
    }
}

