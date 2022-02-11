
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
            [SerializeField] private float angularVelocityThreshold = 800f;
            void OnCollisionEnter2D(Collision2D _other)
            {
                if (Mathf.Abs(pommelRB.angularVelocity) > angularVelocityThreshold)
                {
                    if (_other.transform.gameObject.CompareTag("GameBall"))
                    {
                        Vector2 _dir = _other.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
                        float finalForce = Mathf.Abs(force * (pommelRB.angularVelocity));
                        _dir = -_dir.normalized;
                        BallSparksSpawner.instance.Spawn(_other.contacts[0].point);
                        Rigidbody2D _otherBody = _other.gameObject.GetComponent<Rigidbody2D>();
                        GameBall _otherGameBall = _other.gameObject.GetComponent<GameBall>();
                        _otherBody.AddForce(_dir*finalForce, ForceMode2D.Impulse);
                        AudioController.instance.PlayAudio(AudioType.SFX_02);
                        _otherGameBall.ActivateTrail();
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

            public void HandleWhooshPointCollision()
            {
                if (Mathf.Abs(pommelRB.angularVelocity) > angularVelocityThreshold)
                {
                    AudioController.instance.PlayAudio(AudioType.SFX_10);
                }
            }
            
        }
    }
}
