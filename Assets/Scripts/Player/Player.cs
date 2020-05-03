using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Apocalypse {
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour {
        [HideInInspector] public bool justSpawned = false;
        [HideInInspector] public Vector3 playerPositionBeforeInternalShop;
        [SerializeField] private float radiusAction = 4.5f;
        private const float Speed = 50f;
        private const float RespawnTPause = 3f;
        private const float DamageTimer = .43f;
        private const float HealingTimer = .25f;
        private SpriteRenderer _spriteRndr;
        private Dictionary<string, AudioSource> _audioPlayers;
        private Dictionary<string, AudioClip> _sounds;
        private PlayerStats _stats;
        private Rigidbody2D _rb;
        private float _infectionDamage;
        private float _healthRestore;
        private bool _infected;
        private float _health;
        private Vector2 _velocity;

        private void Awake() {
            _stats = GetComponent<PlayerStats>();
            _rb = GetComponent<Rigidbody2D>();
            _spriteRndr = GetComponentInChildren<SpriteRenderer>();
            _infected = false;
            _health = 100f;
            _audioPlayers = new Dictionary<string, AudioSource>() {
                {"walkp", gameObject.AddComponent<AudioSource>()},
                {"pickp", gameObject.AddComponent<AudioSource>()},
                {"infectedp", gameObject.AddComponent<AudioSource>()},
                {"dmgp", gameObject.AddComponent<AudioSource>()},
                {"hitsmthgp", gameObject.AddComponent<AudioSource>()}
            };
            _sounds = new Dictionary<string, AudioClip>() {
                {"walk", Resources.Load<AudioClip>("Audio/Effects/footstep")},
                {"infected", Resources.Load<AudioClip>("Audio/Effects/infected")},
                {"pickitm", Resources.Load<AudioClip>("Audio/Effects/pick-item")},
                {"lifetick", Resources.Load<AudioClip>("Audio/Effects/loose-life-tick")},
                {"hitsmthg", Resources.Load<AudioClip>("Audio/Effects/closedDoor")}
            };
        }

        private void Update() {
            if (UIManager.Instance.Finished)
                return;

            _velocity = _rb.velocity;
            _velocity.x = 0;
            _velocity.y = 0;

            if (Input.GetKey(KeyCode.W)) {
                _velocity.y = Speed;
                
                PlaySound("walkp", "walk");
                
            } else if (Input.GetKey(KeyCode.S)) {
                _velocity.y = -Speed;
                
                PlaySound("walkp", "walk");
            }

            if (Input.GetKey(KeyCode.A)) {
                _velocity.x = -Speed;
                _spriteRndr.flipX = false;
                
                PlaySound("walkp", "walk");

            } else if (Input.GetKey(KeyCode.D)) {
                _velocity.x = Speed;
                _spriteRndr.flipX = true;
                
                PlaySound("walkp", "walk");
            }

            _rb.velocity = _velocity;
            
            if (_velocity == Vector2.zero)
                PlaySound("walkp", "stop");

            if (Input.GetKeyDown(KeyCode.E)) {
                ShopItem nearerGameItem = GetNearerItem();

                if (nearerGameItem != null) {
                    nearerGameItem.OnUsed(_stats);
                    PlaySound("pickp", "pick");
                }
            }
        }

        private void PlaySound(string player, string clip) {
            if (clip == "stop") // ma si, anche lo stop sarà un audio :P
                _audioPlayers[player].Stop();
            else if (_audioPlayers[player].isPlaying)
                return;
            
            switch (clip) {
                case "walk":
                    _audioPlayers[player].PlayOneShot(_sounds["walk"]);
                    break;
                case "pick":
                    _audioPlayers[player].PlayOneShot(_sounds["pickitm"]);
                    break;
                case "infected":
                    _audioPlayers[player].PlayOneShot(_sounds["infected"]);
                    break;
                case "dmg-tick":
                    _audioPlayers[player].PlayOneShot(_sounds["lifetick"]);
                    break;
                case "hit-smthg":
                    _audioPlayers[player].PlayOneShot(_sounds["hitsmthg"]);
                    break;
                default:
                    break;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radiusAction);
        }

        public void Infect(float dmg) {
            if (_infected)
                return;
            
            _health -= dmg;
            _infected = true;
            _healthRestore = dmg / 10;
            _infectionDamage = dmg / (float)7.2;
            
            PlaySound("infectedp", "infection");

            if (_health <= 0)
                UIManager.Instance.OnGameFinished(false);
            else
                StartCoroutine(nameof(Infection));
        }

        private void OnCollisionEnter2D(Collision2D other) {
            PlaySound("hitsmthgp", "hit-smthg");
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
                PlaySound("dmgp", "dmg-tick");

                if (_health <= 0)
                    UIManager.Instance.OnGameFinished(false);
 
                yield return new WaitForSeconds(1f);
            }
            
            _infected = false;
            
            StartCoroutine(nameof(Healing));
        }
    }
}

