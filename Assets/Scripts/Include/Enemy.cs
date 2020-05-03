using UnityEngine;

namespace Apocalypse {
    public class Enemy {
        public EnemyType Type { get; private set; }
        public bool InfectionCharge { get; set; }
        public readonly float Damage;

        public Enemy(EnemyType t) {
            Damage = Random.Range(2f, 8f);
            Type = t;
            InfectionCharge = true;
        }
    }
}
