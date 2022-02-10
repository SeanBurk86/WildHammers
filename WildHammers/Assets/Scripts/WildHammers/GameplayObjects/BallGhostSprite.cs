
using UnityEngine;

namespace WildHammers
{
    namespace GameplayObjects
    {
        public class BallGhostSprite : MonoBehaviour
        {
            [SerializeField] private float m_ActiveTime = 1.0f;
            private float m_TimeActivated;
            private float m_Alpha, m_Red, m_Green, m_Blue;
            [SerializeField] private float m_AlphaSet = 1.0f, m_RedSet = 1.0f, m_GreenSet = 0.0f, m_BlueSet = 0.0f;
            [SerializeField] private float m_AlphaDecay = 0.85f, m_RedDecay = 0.0f, m_GreenDecay = -1.0f, m_BlueDecay = 0.0f;
            private SpriteRenderer m_SR;
            private Color m_Color;

            private void OnEnable()
            {
                m_SR = GetComponent<SpriteRenderer>();

                m_Alpha = m_AlphaSet;
                m_Red = m_RedSet;
                m_Green = m_GreenSet;
                m_Blue = m_BlueSet;
                m_TimeActivated = Time.time;
            }

            private void Update()
            {
                m_Red -= m_RedDecay * Time.deltaTime;
                m_Green -= m_GreenDecay * Time.deltaTime;
                m_Blue -= m_BlueDecay * Time.deltaTime;
                m_Alpha -= m_AlphaDecay * Time.deltaTime;
                m_Color = new Color(m_Red, m_Green, m_Blue, m_Alpha);
                m_SR.color = m_Color;
        
                if(Time.time >= (m_TimeActivated + m_ActiveTime))
                {
                    BallGhostSpritePool.instance.Release(this);
                }
            }
        }
        
    }
}
