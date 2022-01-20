using System.Collections;
using UnityEngine;

namespace WildHammers
{
    namespace GameplayObject
    {
        public class BallSpawner : MonoBehaviour
        {
            public static BallSpawner instance;

            [SerializeField] private float spawnDelay = 3f;
            [SerializeField] private GameObject ballPrefab;
            [SerializeField] private Transform spawnPoint;

            private void Awake()
            {
                if (instance == null)
                {
                    instance = this;
                } 
                else if (instance != null)
                {
                    Destroy(gameObject);
                }
            }

            public void SpawnBall()
            {
                StartCoroutine(SpawnBallDelay());
            }

            IEnumerator SpawnBallDelay()
            {
                yield return new WaitForSeconds(spawnDelay);
                var tempBall = Instantiate(ballPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
                tempBall.transform.parent = transform;
                Rigidbody2D tempBallRB = tempBall.gameObject.GetComponent<Rigidbody2D>();
                tempBallRB.AddForce(Vector2.up*200,ForceMode2D.Impulse);
            }
            
        }
        
    }
}
