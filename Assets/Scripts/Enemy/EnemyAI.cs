using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Apocalypse {
    public class EnemyAI : MonoBehaviour {
        private const float DirectionChangeT = 2f;
        private const float CharVelocity = 1.5f;
        private const float Speed = 40f;
        private bool _following;
        private GameObject _player;
        private float _lastDirection;
        private bool _isStatic;
        private Vector2 _moveDirection;
        private Vector2 _movPerSecond;
        private Vector2 _moveVec;
        public Enemy Enemy { get; set; }

        private void Awake() {
            _player = GameObject.FindWithTag("Player");
            _lastDirection = 0f;
            _following = false;
            _isStatic = false;
        }
        
        private void Update() {
            if (_following) {
                FollowPlayer();
                return;
            }

            if (Time.time - _lastDirection > DirectionChangeT){
                _lastDirection = Time.time;
                calcuateNewMovementVector();
            }

            _moveVec = transform.position;
     
            transform.FlipToDirection2D(_moveVec);
            _moveVec.Set(
                _moveVec.x + (_movPerSecond.x * Time.deltaTime), 
                _moveVec.y + (_movPerSecond.y * Time.deltaTime)
            );
            
            transform.position = _moveVec;
        }
        
        private void calcuateNewMovementVector() {
            _moveDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            _movPerSecond = _moveDirection * CharVelocity;
        }
        
        private void FollowPlayer() {
            Vector3 displacement = _player.transform.position - transform.position;
            displacement = displacement.normalized;

            if (Vector2.Distance(_player.transform.position, transform.position) > 1.1f) {
                transform.FlipToDirection2D(displacement);
                transform.position += (displacement * Speed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player"))
                _following = true;
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.gameObject.CompareTag("Player"))
                return;

            _following = false;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            calcuateNewMovementVector();
        }
        
        private IEnumerator Recharge() {
            Enemy.InfectionCharge = false;
            yield return new WaitForSeconds(Random.Range(3f, 6f));
            Enemy.InfectionCharge = true;
        }

        public void RechargeInfection() {
            StartCoroutine(nameof(Recharge));
        }
    }
}
