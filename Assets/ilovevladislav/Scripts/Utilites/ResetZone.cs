using UnityEngine;
using UnityEngine.UI;

namespace PlatformerController2D.Runtime.Scripts.Utilites
{
    public class ResetZone : MonoBehaviour
    {
        public Transform playerStartPosition;
        public string playerTag = "Player";
        public Transform playerTarget;
        public Slider oxygenSlider;
        public OxygenSystem oxygen;

        private bool hasReset = false;

        private void Update()
        {
            if (oxygenSlider == null)
                return;

            if (oxygenSlider.value <= 0f && !hasReset)
            {
                ResetPlayer(playerTarget != null
                    ? playerTarget.gameObject
                    : null);

                hasReset = true;
            }

            if (oxygenSlider.value > 0f)
            {
                hasReset = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(playerTag))
            {
                ResetPlayer(other.gameObject);
            }
        }
    
        private void ResetPlayer(GameObject player)
        {
        
            if (playerStartPosition != null)
            {
                playerTarget.position = playerStartPosition.position;
            }
            else
            {
                player.transform.position = Vector3.zero;
            }
        
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
                playerRb.angularVelocity = 0f;
            }

            if (oxygenSlider != null)
            {
                oxygen.currentOxygen = 100f;
            }

            EnemyHealth[] enemies = FindObjectsByType<EnemyHealth>(FindObjectsInactive.Include);
            foreach (EnemyHealth enemy in enemies)
            {
                enemy.ResetEnemy();
            }
        }
    }
}