
using TMPro;
using UnityEngine;

namespace WildHammers
{
    namespace Player
    {
        public class HammerController : MonoBehaviour
        {
            public float amount = 20000000f;
            public float hammerThrust = 300f;
            public TMP_Text headTMPText;

            [SerializeField] private PlayerGamepadInputHandler InputHandler;
            [SerializeField] private Rigidbody2D RBBottom, HammerRB;
            [SerializeField] private GameObject m_HammerHead;
            [SerializeField] private Vector3 m_HammerHeadOffset = new Vector3(0,7,0);
            [SerializeField] private GameObject m_Pommel;
            [SerializeField] private Vector3 m_PommelOffset = new Vector3(0, 0, 0);
            [SerializeField] private GameObject m_FullHammer;
            [SerializeField] private Vector3 m_FullHammerOffset = new Vector3(0, 0, 0);
            [SerializeField] private Vector3 m_InitialRotation = new Vector3(0, 0, 0);

            private Vector2 CurrentVelocity, workspace;
            
            #region Unity Functions

                private void Start()
                {
                    if (InputHandler == null)
                    {
                        InputHandler = gameObject.GetComponentInParent<PlayerGamepadInputHandler>();
                    }

                    ResetChildrenPositionAndRotation();
                }

                private void Update()
                {
                    float clockwiseAmount = amount * Time.deltaTime;
                    if (InputHandler.freezeInput)
                    {
                        RBBottom.velocity = Vector2.zero;
                    }
                    if (InputHandler.counterClockwiseInput && !InputHandler.clockwiseInput)
                    {
                        RBBottom.AddTorque(clockwiseAmount);
                    }
                    else if (InputHandler.clockwiseInput && !InputHandler.counterClockwiseInput)
                    {
                        RBBottom.AddTorque(-1 * clockwiseAmount);
                    }
                    else if (!InputHandler.clockwiseInput && !InputHandler.counterClockwiseInput)
                    {
                        RBBottom.angularVelocity = 0f;
                    }
                    if ((InputHandler.InputXNormalized != 0 || InputHandler.InputYNormalized != 0) 
                        && (InputHandler.clockwiseInput || InputHandler.counterClockwiseInput))
                    {
                        float hammerMovementSpeed = Mathf.Abs(hammerThrust * (RBBottom.angularVelocity/720));
                        RBBottom.AddForce(new Vector2(InputHandler.InputXNormalized, InputHandler.InputYNormalized)*hammerMovementSpeed, ForceMode2D.Impulse);
                    } else if ((InputHandler.InputXNormalized != 0 || InputHandler.InputYNormalized != 0)
                               && (InputHandler.clockwiseInput && InputHandler.counterClockwiseInput))
                    {
                        HammerRB.AddForce(new Vector2(InputHandler.InputXNormalized, InputHandler.InputYNormalized)*hammerThrust, ForceMode2D.Impulse);
                    }

                    if (InputHandler.massIncreaseInput)
                    {
                        HammerRB.mass += 50;
                    }
                    else
                    {
                        HammerRB.mass = 50;
                    }
                }
            

            #endregion

            #region Public Functions

                public void ResetChildrenPositionAndRotation()
                {
                    m_HammerHead.transform.localPosition = m_HammerHeadOffset;
                    m_HammerHead.transform.localRotation = Quaternion.Euler(m_InitialRotation);
                    m_Pommel.transform.localPosition = m_PommelOffset;
                    m_Pommel.transform.localRotation = Quaternion.Euler(m_InitialRotation);
                    m_FullHammer.transform.localPosition = m_FullHammerOffset;
                    m_FullHammer.transform.localRotation = Quaternion.Euler(m_InitialRotation);
                }
                
                public void SetVelocity(float velocity, Vector2 direction)
                {
                    workspace = direction * velocity;
                    HammerRB.velocity = workspace;
                    CurrentVelocity = workspace;
                }
            

            #endregion
            
        }
        
    }
}
