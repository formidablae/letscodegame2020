using System.Collections;
using UnityEngine;

namespace Apocalypse {
    public class Player : MonoBehaviour {
        [SerializeField] private GameObject RespawnP; 
        private const float Speed = 2.89f;
        private const float RespawnTPause = 3f;
        private const float DamageTimer = .43f;
        private const float HealingTimer = .25f;
        private float _infectionDamage;
        private float _healthRestore;
        private bool _respawing;
        private bool _infected;
        private float _health;
        private Vector2 _moveVec;

        private void Awake() {
            RespawnP = GameObject.FindWithTag("Respawn");
            _moveVec = new Vector2(0f, 0f);
            _respawing = false;
            _infected = false;
            _health = 100f;

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

        private void Move() {
            if (_respawing)
                return;

            _moveVec.x *= Time.fixedDeltaTime;
            _moveVec.y *= Time.fixedDeltaTime;

            transform.FlipToDirection2D(_moveVec);
            transform.Translate(_moveVec, Space.World);
        }

        private void Respawn() {//todo reset scene?
            StopAllCoroutines();
            _moveVec = RespawnP.transform.position;
            
            _health = 100f;
            _infected = false;
            transform.position = _moveVec;
            StartCoroutine(nameof(RespawnPause));
        }

        public void Infect(float dmg) {
            if (_infected)
                return;
            
            _health -= dmg;
            _infected = true;
            _healthRestore = dmg / 10;
            _infectionDamage = dmg / (float)7.2;

            if (_health <= 0) //todo game over?
                Respawn();
            else
                StartCoroutine(nameof(Infection));
        }

        private IEnumerator Healing() {
            float timer = 0f;
            
            while (timer < HealingTimer) {
                if (_infected)
                    yield break;
                
                _health += _healthRestore;
                timer += Time.deltaTime;
 Debug.Log("heal "+_healthRestore+" "+_health);
                yield return new WaitForSeconds(1f);
            }
        }

        private IEnumerator Infection() {
            float timer = 0f;
            
            while (timer < DamageTimer) {
                _health -= _infectionDamage;
                timer += Time.deltaTime;
Debug.Log("health "+_health+"  dmg "+_infectionDamage+" timer "+timer);
                if (_health <= 0) // todo game over?
                    Respawn();
 
                yield return new WaitForSeconds(1f);
            }
            
            _infected = false;
            
            StartCoroutine(nameof(Healing));
        }
        
        private IEnumerator RespawnPause() {
            _respawing = true;
            yield return new WaitForSeconds(RespawnTPause);
            _respawing = false;
        }
    }
}

