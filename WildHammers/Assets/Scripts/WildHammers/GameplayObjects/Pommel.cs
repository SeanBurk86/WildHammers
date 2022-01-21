
using UnityEngine;
using WildHammers.Player;

namespace WildHammers
{
    namespace GameplayObjects
    {
        public class Pommel : MonoBehaviour
        {
            public bool debug;
            [SerializeField] private float m_KOForce = 10f;
            private void OnCollisionEnter2D(Collision2D _other)
            {
                Log("hit with force vector of "+_other.rigidbody.velocity);
                // if rb is attached to hammerhead
                if (_other.gameObject.CompareTag("HammerHead"))
                {
                    // and hammerhead has sufficient velocity
                    if (Mathf.Abs(_other.rigidbody.velocity.x) >= m_KOForce || Mathf.Abs(_other.rigidbody.velocity.y) >= m_KOForce)
                    {
                        // invoke a stun state on the hammer
                        Log("Invoking KO on hammer");
                        transform.GetComponentInParent<HammerController>().KOHammer();

                    }
                    
                }
            }

            private void Log(string _msg)
            {
                Debug.Log("[Pommel]: "+_msg);
            }
            
            private void LogWarning(string _msg)
            {
                Debug.LogWarning("[Pommel]: "+_msg);
            }
        }
        
    }
}
