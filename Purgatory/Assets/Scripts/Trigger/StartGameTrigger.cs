using Cinemachine;
using Survivor.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Survivor.Trigger
{
    public class StartGameTrigger : BaseTrigger
    {
        [SerializeField] GameObject ActionUI;
        [SerializeField] GameObject initialMessage;
        [SerializeField] float firstTimer = 0.5f;
        [SerializeField] float secondTimer = 1f;
        [SerializeField] CinemachineFreeLook actionCamera;

        [Header("Events")]
        [SerializeField] private UnityEvent onStarting;


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
            yield return new WaitForSeconds(firstTimer);
            ActionUI.SetActive(true);
            actionCamera.Priority = 2;
            yield return new WaitForSeconds(secondTimer);
            onStarting?.Invoke();  
        }
    }
}
