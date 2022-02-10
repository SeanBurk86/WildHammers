
using UnityCore.Audio;
using UnityEngine;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace GameplayObjects
    {
        public class HammerHead : MonoBehaviour
        {
            public float force = 7f;
            [SerializeField] private Rigidbody2D pommelRB;
            [SerializeField] private float angularVelocityThreshold = 1800f;
            [SerializeField] private ParticleSystem m_BallSparks;
            [SerializeField] private ParticleSystem m_HammerHeadSparks;
            void OnCollisionEnter2D(Collision2D _other)
            {
                if (Mathf.Abs(pommelRB.angularVelocity) > angularVelocityThreshold)
                {
                    if (_other.transform.gameObject.CompareTag("GameBall"))
                    {
                        Vector2 dir = _other.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
                        float finalForce = Mathf.Abs(force * (pommelRB.angularVelocity));
                        dir = -dir.normalized;
                        BallSparksSpawner.instance.Spawn(_other.contacts[0].point);
                        Rigidbody2D otherBody = _other.gameObject.GetComponent<Rigidbody2D>();
                        otherBody.AddForce(dir*finalForce, ForceMode2D.Impulse);
                        AudioController.instance.PlayAudio(AudioType.SFX_02);
                    }
                    else if (_other.transform.gameObject.CompareTag("HammerHead"))
                    {
                        Vector2 dir = _other.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
                        float finalForce = Mathf.Abs(force * (pommelRB.angularVelocity)) * 2f;
                        dir = dir.normalized;
                        Rigidbody2D otherBody = _other.gameObject.GetComponent<Rigidbody2D>();
                        otherBody.AddForce(dir*finalForce, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }
}
