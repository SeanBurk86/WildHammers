
using UnityEngine;
using UnityEngine.InputSystem;

namespace WildHammers
{
    namespace Player
    {
        public class PlayerGamepadInputHandler : MonoBehaviour
        {
            public HammerController playerHammerController { get; private set; }
            public Vector2 rawMovementInput { get; private set; }
            public float inputXNormalized { get; private set; }
            public float inputYNormalized { get; private set; }
            
            public bool clockwiseInput { get; private set; }

            public bool counterClockwiseInput { get; private set; }
            
            public bool freezeInput { get; private set; }

            public bool massIncreaseInput { get; private set; }
            
            public bool pauseInput { get; private set;  }
            
            private void Start()
            {
                counterClockwiseInput = false;
                clockwiseInput = false;
                freezeInput = false;
                massIncreaseInput = false;
                pauseInput = false;
            }

            public void OnClockwiseRotate(InputAction.CallbackContext _context)
            {
                if (_context.started)
                {
                    clockwiseInput = true;
                }
                
                if (_context.canceled)
                {
                    clockwiseInput = false;
                }
            }

            public void OnCounterClockwiseRotate(InputAction.CallbackContext _context)
            {
                if (_context.started)
                {
                    counterClockwiseInput = true;
                }
                
                if (_context.canceled)
                {
                    counterClockwiseInput = false;
                }
            }

            public void OnFreezePosition(InputAction.CallbackContext _context)
            {
                if (_context.started)
                {
                    freezeInput = true;
                }
                
                if (_context.canceled)
                {
                    freezeInput = false;
                }
            }
            
            public void OnMoveInput(InputAction.CallbackContext _context)
            {
                rawMovementInput = _context.ReadValue<Vector2>();
                if (Mathf.Abs(rawMovementInput.x) > 0.5f)
                {
                    inputXNormalized = (rawMovementInput * Vector2.right).x;
                }
                else
                {
                    inputXNormalized = 0;
                }


                if (Mathf.Abs(rawMovementInput.y) > 0.5f)
                {
                    inputYNormalized = (rawMovementInput * Vector2.up).y;
                }
                else
                {
                    inputYNormalized = 0;
                }

            }
            
            public void OnIncreaseMass(InputAction.CallbackContext _context)
            {
                if (_context.started)
                {
                    massIncreaseInput = true;
                }
            
                if (_context.canceled)
                {
                    massIncreaseInput = false;
                }
            }

            public void OnPauseInput(InputAction.CallbackContext _context)
            {
                if (_context.started) pauseInput = true;
            }

            public void UsePauseInput()
            {
                pauseInput = false;
            }
        }
        
    }
}

