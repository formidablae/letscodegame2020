using System.Collections;
using UnityEngine;

namespace Apocalypse {
    public class Player : MonoBehaviour { //todo tune costanti
        private const float Speed = 6f;
        private const float RespawnTPause = 3f;
        private const float DamageTimer = 7f;
        private const float HealingTimer = 4f;
        private const float Damage = .5f;
        private const float HealthAdd = .5f;
        private bool _respawing;
        private bool _outside;
        private float _health;
        private Vector2 _moveVec;
        private Camera _cam;

        private void Awake() {
            _cam = Camera.main;
            _moveVec = new Vector2(0f, 0f);
            _respawing = false;
            _outside = true;

            transform.position = _moveVec;
        }

        private void Update() {
            if (_respawing)
                return;
            
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                _moveVec.x = Input.GetAxisRaw("Horizontal") * Speed;
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                _moveVec.y = Input.GetAxisRaw("Vertical") * Speed;
        }

        private void FixedUpdate() {
            Move();
        }

        // Debug print
        public override string ToString() {
            return "Player " + _health + ", outside: " + _outside + ", position: " + _moveVec;
        }

        private void Move() { //todo clamp, req mappa
            if (_respawing)
                return;

            _moveVec.x *= Time.fixedDeltaTime;
            _moveVec.y *= Time.fixedDeltaTime;

            transform.FlipToDirection2D(_moveVec);
            transform.Translate(_moveVec, Space.World);
            
            if (_outside)
                _cam.transform.Translate(_moveVec, Space.World);
        }

        private void Respawn() {//todo reset scene?
            _moveVec.Set(0f, 0f);
            
            _health = 100f;
            transform.position = _moveVec;
            StartCoroutine(nameof(RespawnPause));
        }

        public void TakeDamage(float dmg) {
            _health -= dmg;

            if (_health <= 0) //todo game over?
                Respawn();
            else
                StartCoroutine(nameof(Infection));
        }

        public void EnterMarket() {
            _outside = false;
        }

        private IEnumerable Healing() {
            float timer = 0f;
            
            while (timer < HealingTimer) {
                _health += HealthAdd;
                timer += Time.deltaTime;
 
                yield return null;
            }
        }

        private IEnumerable Infection() {
            float timer = 0f;
            
            while (timer < DamageTimer) {
                _health -= Damage;
                timer += Time.deltaTime;
 
                yield return null;
            }

            StartCoroutine(nameof(Healing));
        }
        
        private IEnumerable RespawnPause() {
            _respawing = true;
            yield return new WaitForSeconds(RespawnTPause);
            _respawing = false;
        }
    }
}

