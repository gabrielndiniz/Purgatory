using UnityEngine;
using System.Collections;
using Survivor.Trigger;

namespace Survivor.Trigger
{
    public class InitialTrigger : BaseTrigger
    {
        [SerializeField] private StartGameTrigger startGameTrigger;
        [SerializeField] private Fader fader;
        [SerializeField] private float timer = 1f;

        private void Start()
        {
            OnTriggered();
        }

        protected override void OnTriggered()
        {
            if (startGameTrigger == null)
            {
                Debug.LogError("InitialTrigger: StartGameTrigger not assigned.");
                return;
            }

            fader.FadeOut();

            StartCoroutine(DelayedEnable());
        }

        private IEnumerator DelayedEnable()
        {
            Debug.Log($"[InitialTrigger] Waiting {timer}s before enabling StartGameTrigger");

            yield return new WaitForSeconds(timer);

            startGameTrigger.enabled = true;

            Debug.Log("[InitialTrigger] StartGameTrigger enabled");
        }
    }
}
