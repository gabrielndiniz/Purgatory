using UnityEngine;
using UnityEngine.Events;

namespace Survivor.Core
{
    public enum GameState
    {
        NotStarted,
        Playing,
        Victory,
        Defeat
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameState CurrentState { get; private set; } = GameState.NotStarted;

        public UnityEvent<GameState> OnGameStateChanged = new UnityEvent<GameState>();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Debug.Log($"[GameManager] Initial state: {CurrentState}");

            Instance = this;

            CurrentState = GameState.NotStarted;
        }


        public void SetState(GameState newState)
        {
            if (CurrentState == newState)
                return;
            Debug.Log($"[GameManager] Changing State: {CurrentState} → {newState}");
            CurrentState = newState;
            OnGameStateChanged.Invoke(newState);
        }
    }
}
