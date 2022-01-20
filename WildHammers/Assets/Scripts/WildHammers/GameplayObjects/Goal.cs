
using UnityEngine;
using WildHammers.Round;

namespace WildHammers
{
    namespace GameplayObjects
    {
        public class Goal : MonoBehaviour
        {
            public GoalType goalType;
            private void OnTriggerEnter2D(Collider2D other)
            {
                if (other.gameObject.CompareTag("GameBall"))
                {
                    Destroy(other.gameObject);
                    ScoreController.instance.IncrementScore(1,goalType);
                }
            }
        }
        
    }
}

