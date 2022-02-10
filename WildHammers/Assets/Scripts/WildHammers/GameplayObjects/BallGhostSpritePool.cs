
using UnityEngine;
using UnityEngine.Pool;

namespace WildHammers
{
    namespace GameplayObjects
    {
        public class BallGhostSpritePool : MonoBehaviour
        {
            public static BallGhostSpritePool instance = null;

            [SerializeField] private BallGhostSprite m_BallGhostSprite;
            private ObjectPool<BallGhostSprite> m_GhostSpritePool;

            #region Unity Functions

            private void Awake()
            {
                if (!instance)
                {
                    instance = this;
                    ConfigurePool();
                }
                else
                {
                    Destroy(this);
                }
            }

            #endregion

            #region Public Functions

            public void Spawn(GameBall _gameBall)
            {
                BallGhostSprite _ghostSprite =  m_GhostSpritePool.Get();
                _ghostSprite.transform.position = _gameBall.transform.position;
            }

            public void Release(BallGhostSprite _ballGhostSprite)
            {
                m_GhostSpritePool.Release(_ballGhostSprite);
            }

            #endregion

            #region Private Functions

            private void ConfigurePool()
            {
                m_GhostSpritePool = new ObjectPool<BallGhostSprite>(() => { return Instantiate(m_BallGhostSprite); },
                    _ghostSprite => { _ghostSprite.gameObject.SetActive(true); }, _ghostSprite => { _ghostSprite.gameObject.SetActive(false); },
                    _ghostSprite => { Destroy(_ghostSprite.gameObject); }, false, 10, 20);
            }

            #endregion
        }
    }
}

