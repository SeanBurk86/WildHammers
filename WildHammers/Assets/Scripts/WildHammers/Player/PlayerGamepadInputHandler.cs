
using UnityEngine;
using UnityEngine.InputSystem;

namespace WildHammers
{
    namespace Player
    {
        public class PlayerGamepadInputHandler : MonoBehaviour
        {
            public HammerController playerHammerController { get; private set; }
            public Vector2 RawMovementInput { get; private set; }
            public float InputXNormalized { get; private set; }
            public float InputYNormalized { get; private set; }
            
            public bool clockwiseInput { get; private set; }

            public bool counterClockwiseInput { get; private set; }
            
            public bool freezeInput { get; private set; }

            public bool massIncreaseInput { get; private set; }
            
            private void Start()
            {
                counterClockwiseInput = false;
                clockwiseInput = false;
                freezeInput = false;
                massIncreaseInput = false;
            }

            private void Update()
            {
            }

            public void OnClockwiseRotate(InputAction.CallbackContext context)
            {
                if (context.started)
                {
                    clockwiseInput = true;
                }
                
                if (context.canceled)
                {
                    clockwiseInput = false;
                }
            }

            public void OnCounterClockwiseRotate(InputAction.CallbackContext context)
            {
                if (context.started)
                {
                    counterClockwiseInput = true;
                }
                
                if (context.canceled)
                {
                    counterClockwiseInput = false;
                }
            }

            public void OnFreezePosition(InputAction.CallbackContext context)
            {
                if (context.started)
                {
                    freezeInput = true;
                }
                
                if (context.canceled)
                {
                    freezeInput = false;
                }
            }
            
            public void OnMoveInput(InputAction.CallbackContext context)
            {
                RawMovementInput = context.ReadValue<Vector2>();
                if (Mathf.Abs(RawMovementInput.x) > 0.5f)
                {
                    InputXNormalized = (RawMovementInput * Vector2.right).x;
                }
                else
                {
                    InputXNormalized = 0;
                }


                if (Mathf.Abs(RawMovementInput.y) > 0.5f)
                {
                    InputYNormalized = (RawMovementInput * Vector2.up).y;
                }
                else
                {
                    InputYNormalized = 0;
                }

            }
            
            public void OnIncreaseMass(InputAction.CallbackContext context)
            {
                if (context.started)
                {
                    massIncreaseInput = true;
                }
            
                if (context.canceled)
                {
                    massIncreaseInput = false;
                }
            }
        }
        
    }
}

