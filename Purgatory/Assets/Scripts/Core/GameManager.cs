using Survivor.Combat;
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

        [SerializeField] private UnityEvent onPlayerWin;
        [SerializeField] private UnityEvent onPlayerLose;

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
        private void Start()
        {
            if (EnemyManager.Instance != null)
            {
                EnemyManager.Instance.OnAllEnemiesDefeated.AddListener(OnVictory);
            }
        }

        public void OnVictory()
        {
            if (CurrentState != GameState.Playing)
                return;

            Debug.Log("VICTORY!");

            SetState(GameState.Victory);
            onPlayerWin?.Invoke();
        }

        public void OnDefeat()
        {
            if (CurrentState != GameState.Playing)
                return;

            Debug.Log("DEFEAT!");

            SetState(GameState.Defeat);
            onPlayerLose?.Invoke();
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
