using UnityEngine;

namespace Apocalypse {
    public class InfectionCollider : MonoBehaviour {
        private EnemyAI _parent;
        private Enemy _parEnemy;
        private Player _player;

        private void Awake() {
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
            _parent = transform.parent.gameObject.GetComponent<EnemyAI>();
            _parEnemy = _parent.Enemy;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player") && _parEnemy.InfectionCharge) {
                _player.Infect(_parEnemy.Damage);
                _parent.RechargeInfection();
            }
        }
    }
}
