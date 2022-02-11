
using System;
using UnityEngine;

namespace WildHammers
{
    namespace GameplayObjects
    {
        public class WhooshPoint : MonoBehaviour
        {
            private void OnTriggerEnter2D(Collider2D _other)
            {
                if (_other.transform.gameObject.CompareTag("HammerHead"))
                {
                    _other.transform.gameObject.GetComponent<HammerHead>().HandleWhooshPointCollision();
                }
            }
        }
    }
}

