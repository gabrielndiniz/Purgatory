using Survivor.Core;
using Survivor.Trigger;
using Survivor.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Survivor.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionAsset inputActions;

        [Header("Movement")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private Rigidbody rb;

        [Header("Combat")]
        [SerializeField] private CombatController combat;

        [Header("Animation")]
        [SerializeField] private Animator animator;

        [Header("Triger")]
        [SerializeField] private StartGameTrigger startGameTrigger;

        private InputAction moveAction;
        private InputAction specialAction;

        private Vector2 moveInput;

        private static readonly int StartHash = Animator.StringToHash("Start");
        private static readonly int SpeedHash = Animator.StringToHash("Speed");


        private void Awake()
        {
            moveAction = inputActions.FindAction("Move");
            specialAction = inputActions.FindAction("Special");

            if (rb != null)
                rb.freezeRotation = true;
            else
                Debug.LogError("PlayerController: Rigidbody not found.");
        }

        private void OnEnable()
        {
            moveAction?.Enable();
            specialAction?.Enable();
        }

        private void OnDisable()
        {
            Debug.Log("PlayerController: disabled.");
            moveAction?.Disable();
            specialAction?.Disable();
        }

        private void Update()
        {
            if (specialAction != null && specialAction.WasPressedThisFrame())
            {
                OnSpecial();
            }

            UpdateMovementAnimation();


            if (GameManager.Instance.CurrentState != GameState.Playing)
                return;

            ReadMovementInput();


        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.CurrentState != GameState.Playing)
                return;

            MoveCharacter();
        }


        private void OnGameStateChanged(GameState state)
        {
            Debug.Log($"[PlayerController] GameState = {state}");

            if (state != GameState.Playing && animator != null)
            {
                animator.SetFloat(SpeedHash, 0f);
            }
        }


        private void ReadMovementInput()
        {
            if (moveAction == null)
                return;

            moveInput = moveAction.ReadValue<Vector2>();
        }


        private void MoveCharacter()
        {
            if (rb == null)
                return;

            Vector3 move = new Vector3(-moveInput.y, 0f, moveInput.x);
            if (move.sqrMagnitude > 0.0001f)
            {
                rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);

                Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
                rb.MoveRotation(targetRotation);
            }
        }


        private void UpdateMovementAnimation()
        {
            if (animator == null)
                return;

            float speedValue = moveInput.sqrMagnitude;
            animator.SetFloat(SpeedHash, speedValue);
        }


        private void OnSpecial()
        {
            var gm = GameManager.Instance;

            if (gm.CurrentState == GameState.NotStarted)
            {
                animator?.SetBool(StartHash, true);

                if (startGameTrigger != null)
                {
                    startGameTrigger.Trigger();
                }
                else
                {
                    Debug.LogError("PlayerController: StartGameTrigger not found.");
                }

                combat.canAttack = true;

                return;
            }

            // TODO:
            // - cooldown
            // - estado de special
            // - skill execution

            if (combat != null && combat.CanUseSpecial())
            {
                combat.UseSpecial();
            }

            Debug.Log("Special performed.");
        }

    }
}
