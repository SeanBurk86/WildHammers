
using TMPro;
using UnityCore.Audio;
using UnityCore.Game;
using UnityEngine;
using WildHammers.Round;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace Player
    {
        public class HammerController : MonoBehaviour
        {
            public float amount = 20000000f;
            public float hammerThrust = 300f;
            public TMP_Text headTMPText;

            [SerializeField] private PlayerGamepadInputHandler m_InputHandler;
            [SerializeField] private Rigidbody2D m_RBBottom, m_HammerRB;
            [SerializeField] private GameObject m_HammerHead;
            [SerializeField] private Vector3 m_HammerHeadOffset = new Vector3(0,7,0);
            [SerializeField] private GameObject m_Pommel;
            [SerializeField] private Vector3 m_PommelOffset = new Vector3(0, 0, 0);
            [SerializeField] private GameObject m_FullHammer;
            [SerializeField] private Vector3 m_FullHammerOffset = new Vector3(0, 0, 0);
            [SerializeField] private Vector3 m_InitialRotation = new Vector3(0, 0, 0);
            [SerializeField] private float m_KOTime = 1.5f;
            [SerializeField] private Animator m_HammmerAnimator;

            private Vector2 m_CurrentVelocity, m_Workspace;
            private float m_KOTimer = 0f;
            private bool m_IsKOed;

            #region Unity Functions

                private void Start()
                {
                    if (m_InputHandler == null)
                    {
                        m_InputHandler = gameObject.GetComponentInParent<PlayerGamepadInputHandler>();
                    }

                    ResetChildrenPositionAndRotation();
                }

                private void Update()
                {
                    if (m_InputHandler.pauseInput)
                    {
                        m_InputHandler.UsePauseInput();
                        GameRoundController.instance.HandlePauseInput();
                    }

                    if (!GameController.instance.isGamePaused)
                    {
                        m_KOTimer -= Time.deltaTime;
                        if (m_KOTimer <= 0f)
                        {
                            m_IsKOed = false;
                            m_HammmerAnimator.SetBool("isKOed", false);
                        }
                        
                        if (!m_IsKOed)
                        {
                            float clockwiseAmount = amount * Time.deltaTime;
                            if (m_InputHandler.freezeInput)
                            {
                                m_RBBottom.velocity = Vector2.zero;
                            }
                            if (m_InputHandler.counterClockwiseInput && !m_InputHandler.clockwiseInput)
                            {
                                m_RBBottom.AddTorque(clockwiseAmount);
                            }
                            else if (m_InputHandler.clockwiseInput && !m_InputHandler.counterClockwiseInput)
                            {
                                m_RBBottom.AddTorque(-1 * clockwiseAmount);
                            }
                            else if (!m_InputHandler.clockwiseInput && !m_InputHandler.counterClockwiseInput)
                            {
                                m_RBBottom.angularVelocity = 0f;
                            }
                            if ((m_InputHandler.inputXNormalized != 0 || m_InputHandler.inputYNormalized != 0) 
                                && (m_InputHandler.clockwiseInput || m_InputHandler.counterClockwiseInput))
                            {
                                float hammerMovementSpeed = Mathf.Abs(hammerThrust * (m_RBBottom.angularVelocity/720));
                                m_RBBottom.AddForce(new Vector2(m_InputHandler.inputXNormalized, m_InputHandler.inputYNormalized)*hammerMovementSpeed, ForceMode2D.Impulse);
                            } else if ((m_InputHandler.inputXNormalized != 0 || m_InputHandler.inputYNormalized != 0)
                                       && (m_InputHandler.clockwiseInput && m_InputHandler.counterClockwiseInput))
                            {
                                m_HammerRB.AddForce(new Vector2(m_InputHandler.inputXNormalized, m_InputHandler.inputYNormalized)*hammerThrust, ForceMode2D.Impulse);
                            }

                            if (m_InputHandler.massIncreaseInput)
                            {
                                m_HammerRB.mass += 50;
                            }
                            else
                            {
                                m_HammerRB.mass = 50;
                            }
                            
                        }
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
                    m_Workspace = direction * velocity;
                    m_HammerRB.velocity = m_Workspace;
                    m_CurrentVelocity = m_Workspace;
                }

                public void KOHammer()
                {
                    if (!m_IsKOed)
                    {
                        m_IsKOed = true;
                        m_KOTimer = m_KOTime;
                        //Set anim to isKOed to true;
                        m_HammmerAnimator.SetBool("isKOed", true);
                        AudioController.instance.PlayAudio(AudioType.SFX_09);
                    }
                }
            

            #endregion

            #region Private Functions

            

            #endregion
            
        }
        
    }
}
