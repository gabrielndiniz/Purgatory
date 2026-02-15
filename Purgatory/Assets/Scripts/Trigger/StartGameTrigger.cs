using UnityEngine;
using System.Collections;
using Survivor.Core;
using Cinemachine;

namespace Survivor.Trigger
{
    public class StartGameTrigger : BaseTrigger
    {
        [SerializeField] GameObject healthBar;
        [SerializeField] GameObject initialMessage;
        [SerializeField] float timer = 0.5f;
        [SerializeField] CinemachineFreeLook actionCamera;


        public void TriggerFromAnimation()
        {
            Trigger();
        }

        protected override void OnTriggered()
        {
            Debug.Log("StartGameTrigger on. Game active.");
            GameManager.Instance.SetState(GameState.Playing);
            initialMessage.SetActive(false);

            StartCoroutine(DelayedEnable());
        }

        private IEnumerator DelayedEnable()
        {
            Debug.Log("Starting delay");
            yield return new WaitForSeconds(timer);
            healthBar.SetActive(true);
            actionCamera.Priority = 2;
        }
    }
}
