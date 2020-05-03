using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Apocalypse {
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour {
        [HideInInspector] public Vector3 playerPositionBeforeInternalShop;
        [SerializeField] private float radiusAction = 4.5f;
        [SerializeField] private GameObject RespawnP;
        private const float Speed = 50f;
        private const float RespawnTPause = 3f;
        private const float DamageTimer = .43f;
        private const float HealingTimer = .25f;
        private SpriteRenderer _spriteRndr;
        private AudioSource _audioPlayer;
        private Dictionary<string, AudioClip> _sounds;
        private PlayerStats _stats;
        private Rigidbody2D _rb;
        private float _infectionDamage;
        private float _healthRestore;
        private bool _respawing;
        private bool _infected;
        private float _health;
        private Vector2 _velocity;

        private void Awake() {
            _stats = GetComponent<PlayerStats>();
            _rb = GetComponent<Rigidbody2D>();
            _spriteRndr = GetComponentInChildren<SpriteRenderer>();
            RespawnP = GameObject.FindWithTag("Respawn");
            _respawing = false;
            _infected = false;
            _health = 100f;
            _audioPlayer = new AudioSource();
            _sounds = new Dictionary<string, AudioClip>() {
                {"walk", Resources.Load("Audio/Effects/footstep") as AudioClip}
            };
        }

        private void Update() {
            if (_respawing)
                return;

            _velocity = _rb.velocity;
            _velocity.x = 0;
            _velocity.y = 0;

            if (Input.GetKey(KeyCode.W))
                _velocity.y = Speed;
            else if (Input.GetKey(KeyCode.S))
                _velocity.y = -Speed;

            if (Input.GetKey(KeyCode.A)) {
                _velocity.x = -Speed;
                _spriteRndr.flipX = false;

            } else if (Input.GetKey(KeyCode.D)) {
                _velocity.x = Speed;
                _spriteRndr.flipX = true;
            }

            _rb.velocity = _velocity;
            
            if (Input.GetKeyDown(KeyCode.E)) {
                ShopItem nearerGameItem = GetNearerItem();

                if (nearerGameItem != null)
                    nearerGameItem.OnUsed(_stats);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radiusAction);
        }

        private void Respawn() {//todo reset scene?
            StopAllCoroutines();
            
            _health = 100f;
            _infected = false;
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
        
        private ShopItem GetNearerItem()
        {
            Collider2D[] collisionWith = new Collider2D[3];
            int collisions = Physics2D.OverlapCircleNonAlloc(transform.position, radiusAction, collisionWith);

            ShopItem nearerGameItem = null;
            float distanceNearer = 0;

            for (int i = 0; i < collisions; i++)
            {
                ShopItem current = collisionWith[i].GetComponent<ShopItem>();
                if (current != null)
                {
                    if (nearerGameItem == null)
                    {
                        nearerGameItem = current;
                        distanceNearer = Vector2.Distance(transform.position, current.transform.position);
                    }
                    else
                    {
                        float currDist = Vector2.Distance(transform.position, current.transform.position);
                        if (currDist < distanceNearer)
                        {
                            nearerGameItem = current;
                            distanceNearer = currDist;
                        }
                    }
                }
            }

            return nearerGameItem;
        }

        private IEnumerator Healing() {
            float timer = 0f;
            
            while (timer < HealingTimer) {
                if (_infected)
                    yield break;
                
                _health += _healthRestore;
                timer += Time.deltaTime;
                
                yield return new WaitForSeconds(1f);
            }
        }

        private IEnumerator Infection() {
            float timer = 0f;
            
            while (timer < DamageTimer) {
                _health -= _infectionDamage;
                timer += Time.deltaTime;
                
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

